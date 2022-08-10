using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TelegramBotBase.Tools
{
    public static class Console
    {
        private static EventHandler _handler;

        private static readonly List<Action> Actions = new List<Action>();

        static Console()
        {
        }

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        public static void SetHandler(Action action)
        {
            Actions.Add(action);

            if (_handler != null)
                return;

            _handler += Handler;
            SetConsoleCtrlHandler(_handler, true);
        }

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CtrlCEvent:
                case CtrlType.CtrlLogoffEvent:
                case CtrlType.CtrlShutdownEvent:
                case CtrlType.CtrlCloseEvent:

                    foreach (var a in Actions) a();

                    return false;

                default:
                    return false;
            }
        }

        private delegate bool EventHandler(CtrlType sig);

        private enum CtrlType
        {
            CtrlCEvent = 0,
            CtrlBreakEvent = 1,
            CtrlCloseEvent = 2,
            CtrlLogoffEvent = 5,
            CtrlShutdownEvent = 6
        }
    }
}