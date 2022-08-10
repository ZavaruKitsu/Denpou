using System;

namespace TelegramBotBase.Args
{
    public class RenderViewEventArgs : EventArgs
    {
        public RenderViewEventArgs(int ViewIndex)
        {
            CurrentView = ViewIndex;
        }

        public int CurrentView { get; set; }
    }
}