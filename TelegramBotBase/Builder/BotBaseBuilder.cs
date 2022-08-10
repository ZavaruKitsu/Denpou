using System;
using System.Collections.Generic;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotBase.Base;
using TelegramBotBase.Builder.Interfaces;
using TelegramBotBase.Commands;
using TelegramBotBase.Factories;
using TelegramBotBase.Factories.MessageLoops;
using TelegramBotBase.Form;
using TelegramBotBase.Interfaces;
using TelegramBotBase.Localizations;
using TelegramBotBase.States;

namespace TelegramBotBase.Builder
{
    public class BotBaseBuilder : IApiKeySelectionStage, IMessageLoopSelectionStage, IStartFormSelectionStage,
        IBuildingStage, INetworkingSelectionStage, IBotCommandsStage, ISessionSerializationStage,
        ILanguageSelectionStage
    {
        private readonly List<BotCommand> _botcommands = new List<BotCommand>();
        private string _apiKey;

        private MessageClient _client;

        private IStartFormFactory _factory;

        private IMessageLoopFactory _messageloopfactory;

        private IStateMachine _statemachine;

        private BotBaseBuilder()
        {
        }


        public BotBase Build()
        {
            var bb = new BotBase();

            bb.ApiKey = _apiKey;
            bb.StartFormFactory = _factory;

            bb.Client = _client;

            bb.Sessions.Client = bb.Client;

            bb.BotCommands = _botcommands;

            bb.StateMachine = _statemachine;

            bb.MessageLoopFactory = _messageloopfactory;

            bb.MessageLoopFactory.UnhandledCall += bb.MessageLoopFactory_UnhandledCall;

            return bb;
        }

        public static IApiKeySelectionStage Create()
        {
            return new BotBaseBuilder();
        }

        #region "Step 1 (Basic Stuff)"

        public IMessageLoopSelectionStage WithApiKey(string apiKey)
        {
            _apiKey = apiKey;
            return this;
        }


        public IBuildingStage QuickStart(string apiKey, Type startForm)
        {
            _apiKey = apiKey;
            _factory = new DefaultStartFormFactory(startForm);

            DefaultMessageLoop();

            NoProxy();

            OnlyStart();

            NoSerialization();

            DefaultLanguage();

            return this;
        }


        public IBuildingStage QuickStart<T>(string apiKey)
            where T : FormBase
        {
            _apiKey = apiKey;
            _factory = new DefaultStartFormFactory(typeof(T));

            DefaultMessageLoop();

            NoProxy();

            OnlyStart();

            NoSerialization();

            DefaultLanguage();

            return this;
        }

        public IBuildingStage QuickStart(string apiKey, IStartFormFactory startFormFactory)
        {
            _apiKey = apiKey;
            _factory = startFormFactory;

            DefaultMessageLoop();

            NoProxy();

            OnlyStart();

            NoSerialization();

            DefaultLanguage();

            return this;
        }

        #endregion


        #region "Step 2 (Message Loop)"

        public IStartFormSelectionStage DefaultMessageLoop()
        {
            _messageloopfactory = new FormBaseMessageLoop();

            return this;
        }

        public IStartFormSelectionStage CustomMessageLoop(IMessageLoopFactory messageLoopClass)
        {
            _messageloopfactory = messageLoopClass;

            return this;
        }

        public IStartFormSelectionStage CustomMessageLoop<T>()
            where T : class, new()
        {
            _messageloopfactory =
                typeof(T).GetConstructor(new Type[] { })?.Invoke(new object[] { }) as IMessageLoopFactory;

            return this;
        }

        #endregion


        #region "Step 3 (Start Form/Factory)"

        public INetworkingSelectionStage WithStartForm(Type startFormClass)
        {
            _factory = new DefaultStartFormFactory(startFormClass);
            return this;
        }

        public INetworkingSelectionStage WithStartForm<T>()
            where T : FormBase, new()
        {
            _factory = new DefaultStartFormFactory(typeof(T));
            return this;
        }

        public INetworkingSelectionStage WithStartFormFactory(IStartFormFactory factory)
        {
            _factory = factory;
            return this;
        }

        #endregion


        #region "Step 4 (Network Settings)"

        public IBotCommandsStage WithProxy(string proxyAddress)
        {
            var url = new Uri(proxyAddress);
            _client = new MessageClient(_apiKey, url);
            _client.TelegramClient.Timeout = new TimeSpan(0, 1, 0);
            return this;
        }


        public IBotCommandsStage NoProxy()
        {
            _client = new MessageClient(_apiKey);
            _client.TelegramClient.Timeout = new TimeSpan(0, 1, 0);
            return this;
        }


        public IBotCommandsStage WithBotClient(TelegramBotClient tgclient)
        {
            _client = new MessageClient(_apiKey, tgclient);
            _client.TelegramClient.Timeout = new TimeSpan(0, 1, 0);
            return this;
        }


        public IBotCommandsStage WithHostAndPort(string proxyHost, int proxyPort)
        {
            _client = new MessageClient(_apiKey, proxyHost, proxyPort);
            _client.TelegramClient.Timeout = new TimeSpan(0, 1, 0);
            return this;
        }

        public IBotCommandsStage WithHttpClient(HttpClient tgclient)
        {
            _client = new MessageClient(_apiKey, tgclient);
            _client.TelegramClient.Timeout = new TimeSpan(0, 1, 0);
            return this;
        }

        #endregion


        #region "Step 5 (Bot Commands)"

        public ISessionSerializationStage NoCommands()
        {
            return this;
        }

        public ISessionSerializationStage OnlyStart()
        {
            _botcommands.Start("Starts the bot");

            return this;
        }

        public ISessionSerializationStage DefaultCommands()
        {
            _botcommands.Start("Starts the bot");
            _botcommands.Help("Should show you some help");
            _botcommands.Settings("Should show you some settings");
            return this;
        }

        public ISessionSerializationStage CustomCommands(Action<List<BotCommand>> action)
        {
            action?.Invoke(_botcommands);
            return this;
        }

        #endregion


        #region "Step 6 (Serialization)"

        public ILanguageSelectionStage NoSerialization()
        {
            return this;
        }

        public ILanguageSelectionStage UseSerialization(IStateMachine machine)
        {
            _statemachine = machine;
            return this;
        }


        public ILanguageSelectionStage UseJson(string path)
        {
            _statemachine = new JsonStateMachine(path);
            return this;
        }

        public ILanguageSelectionStage UseSimpleJson(string path)
        {
            _statemachine = new SimpleJsonStateMachine(path);
            return this;
        }

        public ILanguageSelectionStage UseXml(string path)
        {
            _statemachine = new XmlStateMachine(path);
            return this;
        }

        #endregion


        #region "Step 7 (Language)"

        public IBuildingStage DefaultLanguage()
        {
            return this;
        }

        public IBuildingStage UseEnglish()
        {
            Default.Language = new English();
            return this;
        }

        public IBuildingStage UseGerman()
        {
            Default.Language = new German();
            return this;
        }

        public IBuildingStage Custom(Localization language)
        {
            Default.Language = language;
            return this;
        }

        #endregion
    }
}