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
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:

                    foreach (var a in Actions) a();

                    return false;

                default:
                    return false;
            }
        }

        private delegate bool EventHandler(CtrlType sig);

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
    }
}