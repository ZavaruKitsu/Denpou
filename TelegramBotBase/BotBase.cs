using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotBase.Args;
using TelegramBotBase.Base;
using TelegramBotBase.Enums;
using TelegramBotBase.Interfaces;
using TelegramBotBase.Sessions;
using Console = TelegramBotBase.Tools.Console;

namespace TelegramBotBase
{
    /// <summary>
    ///     Bot base class for full Device/Context and Messagehandling
    /// </summary>
    public class BotBase
    {
        public BotBase()
        {
            SystemSettings = new Dictionary<eSettings, uint>();

            SetSetting(eSettings.MaxNumberOfRetries, 5);
            SetSetting(eSettings.NavigationMaximum, 10);
            SetSetting(eSettings.LogAllMessages, false);
            SetSetting(eSettings.SkipAllMessages, false);
            SetSetting(eSettings.SaveSessionsOnConsoleExit, false);

            BotCommands = new List<BotCommand>();

            Sessions = new SessionBase();
            Sessions.BotBase = this;
        }

        public MessageClient Client { get; set; }

        /// <summary>
        ///     Your TelegramBot APIKey
        /// </summary>
        public string APIKey { get; set; } = "";

        /// <summary>
        ///     List of all running/active sessions
        /// </summary>
        public SessionBase Sessions { get; set; }

        /// <summary>
        ///     Contains System commands which will be available at everytime and didnt get passed to forms, i.e. /start
        /// </summary>
        public List<BotCommand> BotCommands { get; set; }


        /// <summary>
        ///     Enable the SessionState (you need to implement on call forms the IStateForm interface)
        /// </summary>
        public IStateMachine StateMachine { get; set; }

        /// <summary>
        ///     Offers functionality to manage the creation process of the start form.
        /// </summary>
        public IStartFormFactory StartFormFactory { get; set; }

        /// <summary>
        ///     Contains the message loop factory, which cares about "message-management."
        /// </summary>
        public IMessageLoopFactory MessageLoopFactory { get; set; }

        /// <summary>
        ///     All internal used settings.
        /// </summary>
        public Dictionary<eSettings, uint> SystemSettings { get; }


        /// <summary>
        ///     Start your Bot
        /// </summary>
        public void Start()
        {
            if (Client == null)
                return;

            Client.MessageLoop += Client_MessageLoop;


            if (StateMachine != null) Sessions.LoadSessionStates(StateMachine);

            //Enable auto session saving
            if (GetSetting(eSettings.SaveSessionsOnConsoleExit, false))
                Console.SetHandler(() => { Sessions.SaveSessionStates(); });

            DeviceSession.MaxNumberOfRetries = GetSetting(eSettings.MaxNumberOfRetries, 5);

            Client.StartReceiving();
        }


        private async Task Client_MessageLoop(object sender, UpdateResult e)
        {
            var ds = Sessions.GetSession(e.DeviceId);
            if (ds == null)
            {
                ds = Sessions.StartSession(e.DeviceId).GetAwaiter().GetResult();
                e.Device = ds;
                ds.LastMessage = e.RawData.Message;

                OnSessionBegins(new SessionBeginEventArgs(e.DeviceId, ds));
            }

            var mr = new MessageResult(e.RawData);

            var i = 0;

            //Should formulars get navigated (allow maximum of 10, to dont get loops)
            do
            {
                i++;

                //Reset navigation
                ds.FormSwitched = false;

                await MessageLoopFactory.MessageLoop(this, ds, e, mr);

                mr.IsFirstHandler = false;
            } while (ds.FormSwitched && i < GetSetting(eSettings.NavigationMaximum, 10));
        }


        /// <summary>
        ///     Stop your Bot
        /// </summary>
        public void Stop()
        {
            if (Client == null)
                return;

            Client.MessageLoop -= Client_MessageLoop;


            Client.StopReceiving();

            Sessions.SaveSessionStates();
        }

        /// <summary>
        ///     Send a message to all active Sessions.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SentToAll(string message)
        {
            if (Client == null)
                return;

            foreach (var s in Sessions.SessionList) await Client.TelegramClient.SendTextMessageAsync(s.Key, message);
        }


        /// <summary>
        ///     This will invoke the full message loop for the device even when no "userevent" like message or action has been
        ///     raised.
        /// </summary>
        /// <param name="DeviceId">Contains the device/chat id of the device to update.</param>
        public async Task InvokeMessageLoop(long DeviceId)
        {
            var mr = new MessageResult();

            await InvokeMessageLoop(DeviceId, mr);
        }

        /// <summary>
        ///     This will invoke the full message loop for the device even when no "userevent" like message or action has been
        ///     raised.
        /// </summary>
        /// <param name="DeviceId">Contains the device/chat id of the device to update.</param>
        /// <param name="e"></param>
        public async Task InvokeMessageLoop(long DeviceId, MessageResult e)
        {
            try
            {
                var ds = Sessions.GetSession(DeviceId);
                e.Device = ds;

                await MessageLoopFactory.MessageLoop(this, ds, new UpdateResult(e.UpdateData, ds), e);
                //await Client_Loop(this, e);
            }
            catch (Exception ex)
            {
                var ds = Sessions.GetSession(DeviceId);
                OnException(new SystemExceptionEventArgs(e.Message.Text, DeviceId, ds, ex));
            }
        }


        /// <summary>
        ///     Will get invoke on an unhandled call.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MessageLoopFactory_UnhandledCall(object sender, UnhandledCallEventArgs e)
        {
            OnUnhandledCall(e);
        }

        /// <summary>
        ///     This method will update all local created bot commands to the botfather.
        /// </summary>
        public async Task UploadBotCommands()
        {
            await Client.SetBotCommands(BotCommands);
        }

        /// <summary>
        ///     Could set a variety of settings to improve the bot handling.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="Value"></param>
        public void SetSetting(eSettings set, uint Value)
        {
            SystemSettings[set] = Value;
        }

        /// <summary>
        ///     Could set a variety of settings to improve the bot handling.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="Value"></param>
        public void SetSetting(eSettings set, bool Value)
        {
            SystemSettings[set] = Value ? 1u : 0u;
        }

        /// <summary>
        ///     Could get the current value of a setting
        /// </summary>
        /// <param name="set"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public uint GetSetting(eSettings set, uint defaultValue)
        {
            if (!SystemSettings.ContainsKey(set))
                return defaultValue;

            return SystemSettings[set];
        }

        /// <summary>
        ///     Could get the current value of a setting
        /// </summary>
        /// <param name="set"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetSetting(eSettings set, bool defaultValue)
        {
            if (!SystemSettings.ContainsKey(set))
                return defaultValue;

            return SystemSettings[set] == 0u ? false : true;
        }


        #region "Events"

        private readonly EventHandlerList __Events = new EventHandlerList();

        private static readonly object __evSessionBegins = new object();

        private static readonly object __evMessage = new object();

        private static object __evSystemCall = new object();

        public delegate Task BotCommandEventHandler(object sender, BotCommandEventArgs e);

        private static readonly object __evException = new object();

        private static readonly object __evUnhandledCall = new object();

        #endregion

        #region "Events"

        /// <summary>
        ///     Will be called if a session/context gets started
        /// </summary>
        public event EventHandler<SessionBeginEventArgs> SessionBegins
        {
            add => __Events.AddHandler(__evSessionBegins, value);
            remove => __Events.RemoveHandler(__evSessionBegins, value);
        }

        public void OnSessionBegins(SessionBeginEventArgs e)
        {
            (__Events[__evSessionBegins] as EventHandler<SessionBeginEventArgs>)?.Invoke(this, e);
        }

        /// <summary>
        ///     Will be called on incomming message
        /// </summary>
        public event EventHandler<MessageIncomeEventArgs> Message
        {
            add => __Events.AddHandler(__evMessage, value);
            remove => __Events.RemoveHandler(__evMessage, value);
        }

        public void OnMessage(MessageIncomeEventArgs e)
        {
            (__Events[__evMessage] as EventHandler<MessageIncomeEventArgs>)?.Invoke(this, e);
        }

        /// <summary>
        ///     Will be called if a bot command gets raised
        /// </summary>
        public event BotCommandEventHandler BotCommand;


        public async Task OnBotCommand(BotCommandEventArgs e)
        {
            if (BotCommand != null)
                await BotCommand(this, e);
        }

        /// <summary>
        ///     Will be called on an inner exception
        /// </summary>
        public event EventHandler<SystemExceptionEventArgs> Exception
        {
            add => __Events.AddHandler(__evException, value);
            remove => __Events.RemoveHandler(__evException, value);
        }

        public void OnException(SystemExceptionEventArgs e)
        {
            (__Events[__evException] as EventHandler<SystemExceptionEventArgs>)?.Invoke(this, e);
        }

        /// <summary>
        ///     Will be called if no form handeled this call
        /// </summary>
        public event EventHandler<UnhandledCallEventArgs> UnhandledCall
        {
            add => __Events.AddHandler(__evUnhandledCall, value);
            remove => __Events.RemoveHandler(__evUnhandledCall, value);
        }

        public void OnUnhandledCall(UnhandledCallEventArgs e)
        {
            (__Events[__evUnhandledCall] as EventHandler<UnhandledCallEventArgs>)?.Invoke(this, e);
        }

        #endregion
    }
}
