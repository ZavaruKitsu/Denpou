﻿using System.Net.Http;
using Telegram.Bot;

namespace Denpou.Builder.Interfaces;

public interface INetworkingSelectionStage
{
    /// <summary>
    ///     Chooses a proxy as network configuration.
    /// </summary>
    /// <param name="proxyAddress"></param>
    /// <returns></returns>
    IBotCommandsStage WithProxy(string proxyAddress);

    /// <summary>
    ///     Do not choose a proxy as network configuration.
    /// </summary>
    /// <returns></returns>
    IBotCommandsStage NoProxy();


    /// <summary>
    ///     Chooses a custom instance of TelegramBotClient.
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    IBotCommandsStage WithBotClient(TelegramBotClient client);


    /// <summary>
    ///     Sets the custom proxy host and port.
    /// </summary>
    /// <param name="proxyHost"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    IBotCommandsStage WithHostAndPort(string proxyHost, int port);

    /// <summary>
    ///     Uses a custom http client.
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    IBotCommandsStage WithHttpClient(HttpClient client);
}