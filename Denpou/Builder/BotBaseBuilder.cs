using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Denpou.Base;
using Denpou.Factories.MessageLoops;
using Denpou.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Denpou.Builder;

public sealed class BotBaseBuilder
{
    private readonly List<BotCommand> _botCommands = new();
    private string? _apiKey;
    private ITelegramBotClient? _botClient;
    private IMessageLoopFactory _messageLoopFactory;
    private IServiceProvider? _serviceProvider;
    private Type? _startForm;
    private IStateMachine? _stateMachine;

    private BotBaseBuilder()
    {
        _messageLoopFactory = new FormBaseMessageLoop();
    }

    public BotBase Build()
    {
        if (_apiKey is null && _botClient is null)
            throw new ArgumentException("Either API key or Telegram bot must be set");

        if (_apiKey is not null && _botClient is null) _botClient = new TelegramBotClient(_apiKey);

        if (_startForm is null) throw new ArgumentNullException(nameof(_startForm), "Start form must be set");

        var client = new MessageClient(_botClient!);

        var bot = new BotBase
        {
            Client = client,
            ServiceProvider = _serviceProvider ?? new ServiceContainer(),
            StartFormType = _startForm
        };

        bot.Sessions.Client = bot.Client;
        bot.BotCommands = _botCommands;
        bot.StateMachine = _stateMachine;
        bot.MessageLoopFactory = _messageLoopFactory;
        bot.MessageLoopFactory.UnhandledCall += bot.MessageLoopFactory_UnhandledCall;

        return bot;
    }

    /// <summary>
    ///     Creates <see cref="BotBaseBuilder" /> with default settings
    /// </summary>
    /// <returns></returns>
    public static BotBaseBuilder Create()
    {
        return new BotBaseBuilder();
    }

    public BotBaseBuilder SetApiKey(string apiKey)
    {
        _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        return this;
    }

    public BotBaseBuilder SetMessageLoop(IMessageLoopFactory messageLoopFactory)
    {
        _messageLoopFactory = messageLoopFactory;
        return this;
    }

    public BotBaseBuilder SetBotClient(TelegramBotClient? client)
    {
        _botClient = client;
        return this;
    }

    public BotBaseBuilder SetCommands(Action<List<BotCommand>> action)
    {
        action.Invoke(_botCommands);
        return this;
    }

    public BotBaseBuilder SetSerializer(IStateMachine? machine)
    {
        _stateMachine = machine;
        return this;
    }

    public BotBaseBuilder SetStartForm(Type type)
    {
        if (!type.IsSubclassOf(typeof(FormBase)))
            throw new ArgumentException("Start form must be a subclass of 'FormBase'", nameof(type));

        _startForm = type;
        return this;
    }

    public BotBaseBuilder SetStartForm<T>() where T : FormBase
    {
        return SetStartForm(typeof(T));
    }

    public BotBaseBuilder SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return this;
    }
}
