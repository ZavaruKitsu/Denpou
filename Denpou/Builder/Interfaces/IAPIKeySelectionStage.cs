using System;
using Denpou.Base;
using Denpou.Interfaces;

namespace Denpou.Builder.Interfaces;

public interface IApiKeySelectionStage
{
    /// <summary>
    ///     Sets the API Key which will be used by the telegram bot client.
    /// </summary>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    IMessageLoopSelectionStage WithApiKey(string apiKey);


    /// <summary>
    ///     Quick and easy way to create a BotBase instance.
    ///     Uses: DefaultMessageLoop, NoProxy, OnlyStart, NoSerialization, DefaultLanguage
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="startForm"></param>
    /// <returns></returns>
    IBuildingStage QuickStart(string apiKey, Type startForm);

    /// <summary>
    ///     Quick and easy way to create a BotBase instance.
    ///     Uses: DefaultMessageLoop, NoProxy, OnlyStart, NoSerialization, DefaultLanguage
    /// </summary>
    /// <param name="apiKey"></param>
    /// <returns></returns>
    IBuildingStage QuickStart<T>(string apiKey) where T : FormBase;

    /// <summary>
    ///     Quick and easy way to create a BotBase instance.
    ///     Uses: DefaultMessageLoop, NoProxy, OnlyStart, NoSerialization, DefaultLanguage
    /// </summary>
    /// <param name="apiKey"></param>
    /// <param name="startFormFactory"></param>
    /// <returns></returns>
    IBuildingStage QuickStart(string apiKey, IStartFormFactory startFormFactory);
}