using Denpou.Interfaces;

namespace Denpou.Builder.Interfaces;

public interface IMessageLoopSelectionStage
{
    /// <summary>
    ///     Chooses a default message loop.
    /// </summary>
    /// <returns></returns>
    IStartFormSelectionStage DefaultMessageLoop();

    /// <summary>
    ///     Chooses a custom message loop.
    /// </summary>
    /// <param name="startFormClass"></param>
    /// <returns></returns>
    IStartFormSelectionStage CustomMessageLoop(IMessageLoopFactory startFormClass);


    /// <summary>
    ///     Chooses a custom message loop.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IStartFormSelectionStage CustomMessageLoop<T>() where T : class, new();
}