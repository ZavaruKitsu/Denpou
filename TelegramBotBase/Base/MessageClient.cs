using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TelegramBotBase.Base
{
    /// <summary>
    ///     Base class for message handling
    /// </summary>
    public class MessageClient
    {
        private static readonly object __evOnMessageLoop = new object();

        private static object __evOnMessage = new object();

        private static object __evOnMessageEdit = new object();

        private static object __evCallbackQuery = new object();

        private CancellationTokenSource __cancellationTokenSource;


        public MessageClient(string APIKey)
        {
            this.APIKey = APIKey;
            TelegramClient = new TelegramBotClient(APIKey);

            Prepare();
        }

        public MessageClient(string APIKey, HttpClient proxy)
        {
            this.APIKey = APIKey;
            TelegramClient = new TelegramBotClient(APIKey, proxy);


            Prepare();
        }


        public MessageClient(string APIKey, Uri proxyUrl, NetworkCredential credential = null)
        {
            this.APIKey = APIKey;

            var proxy = new WebProxy(proxyUrl)
            {
                Credentials = credential
            };

            var httpClient = new HttpClient(
                new HttpClientHandler { Proxy = proxy, UseProxy = true }
            );

            TelegramClient = new TelegramBotClient(APIKey, httpClient);

            Prepare();
        }

        /// <summary>
        ///     Initializes the client with a proxy
        /// </summary>
        /// <param name="APIKey"></param>
        /// <param name="proxyHost">i.e. 127.0.0.1</param>
        /// <param name="proxyPort">i.e. 10000</param>
        public MessageClient(string APIKey, string proxyHost, int proxyPort)
        {
            this.APIKey = APIKey;

            var proxy = new WebProxy(proxyHost, proxyPort);

            var httpClient = new HttpClient(
                new HttpClientHandler { Proxy = proxy, UseProxy = true }
            );

            TelegramClient = new TelegramBotClient(APIKey, httpClient);

            Prepare();
        }


        public MessageClient(string APIKey, TelegramBotClient Client)
        {
            this.APIKey = APIKey;
            TelegramClient = Client;

            Prepare();
        }


        public string APIKey { get; set; }

        public ITelegramBotClient TelegramClient { get; set; }

        private EventHandlerList __Events { get; } = new EventHandlerList();


        public void Prepare()
        {
            TelegramClient.Timeout = new TimeSpan(0, 0, 30);
        }


        public void StartReceiving()
        {
            __cancellationTokenSource = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions();

            TelegramClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions,
                __cancellationTokenSource.Token);
        }

        public void StopReceiving()
        {
            __cancellationTokenSource.Cancel();
        }


        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            OnMessageLoop(new UpdateResult(update, null));

            return Task.CompletedTask;
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is ApiRequestException exAPI)
                Console.WriteLine($"Telegram API Error:\n[{exAPI.ErrorCode}]\n{exAPI.Message}");
            else
                Console.WriteLine(exception.ToString());
            return Task.CompletedTask;
        }


        /// <summary>
        ///     This will return the current list of bot commands.
        /// </summary>
        /// <returns></returns>
        public async Task<BotCommand[]> GetBotCommands()
        {
            return await TelegramClient.GetMyCommandsAsync();
        }

        /// <summary>
        ///     This will set your bot commands to the given list.
        /// </summary>
        /// <param name="botcommands"></param>
        /// <returns></returns>
        public async Task SetBotCommands(List<BotCommand> botcommands)
        {
            await TelegramClient.SetMyCommandsAsync(botcommands);
        }


        #region "Events"

        public event Async.AsyncEventHandler<UpdateResult> MessageLoop
        {
            add => __Events.AddHandler(__evOnMessageLoop, value);
            remove => __Events.RemoveHandler(__evOnMessageLoop, value);
        }

        public void OnMessageLoop(UpdateResult update)
        {
            (__Events[__evOnMessageLoop] as Async.AsyncEventHandler<UpdateResult>)?.Invoke(this, update);
        }

        #endregion
    }
}