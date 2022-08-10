using Denpou.Interfaces;

namespace Denpou.Builder.Interfaces;

public interface ISessionSerializationStage
{
    /// <summary>
    ///     Do not uses serialization.
    /// </summary>
    /// <returns></returns>
    ILanguageSelectionStage NoSerialization();


    /// <summary>
    ///     Sets the state machine for serialization.
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    ILanguageSelectionStage UseSerialization(IStateMachine machine);


    /// <summary>
    ///     Using the complex version of .Net JSON, which can serialize all objects.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    ILanguageSelectionStage UseJson(string path);


    /// <summary>
    ///     Use the easy version of .Net JSON, which can serialize basic types, but not generics and others.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    ILanguageSelectionStage UseSimpleJson(string path);


    /// <summary>
    ///     Uses the XML serializer for session serialization.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    ILanguageSelectionStage UseXml(string path);
}