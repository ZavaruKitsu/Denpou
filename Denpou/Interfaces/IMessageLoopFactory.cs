using System;
using System.Threading.Tasks;
using Denpou.Args;
using Denpou.Base;
using Denpou.Sessions;

namespace Denpou.Interfaces;

public interface IMessageLoopFactory
{
    Task MessageLoop(BotBase bot, DeviceSession session, UpdateResult ur, MessageResult e);

    event EventHandler<UnhandledCallEventArgs> UnhandledCall;
}