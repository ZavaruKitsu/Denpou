using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBotBase.Args;
using TelegramBotBase.Base;

namespace TelegramBotBase.Controls.Hybrid
{
    /// <summary>
    ///     This Control is for having a basic form content switching control.
    /// </summary>
    public abstract class MultiView : ControlBase
    {
        /// <summary>
        ///     Hold if the View has been rendered already.
        /// </summary>
        private bool _Rendered;

        private int m_iSelectedViewIndex;


        public MultiView()
        {
            Messages = new List<int>();
        }

        /// <summary>
        ///     Index of the current View.
        /// </summary>
        public int SelectedViewIndex
        {
            get => m_iSelectedViewIndex;
            set
            {
                m_iSelectedViewIndex = value;

                //Already rendered? Re-Render
                if (_Rendered)
                    ForceRender().Wait();
            }
        }

        private List<int> Messages { get; }


        private void Device_MessageSent(object sender, MessageSentEventArgs e)
        {
            if (e.Origin == null || !e.Origin.IsSubclassOf(typeof(MultiView)))
                return;

            Messages.Add(e.MessageId);
        }

        public override void Init()
        {
            Device.MessageSent += Device_MessageSent;
        }

        public override async Task Load(MessageResult result)
        {
            _Rendered = false;
        }


        public override async Task Render(MessageResult result)
        {
            //When already rendered, skip rendering
            if (_Rendered)
                return;

            await CleanUpView();

            await RenderView(new RenderViewEventArgs(SelectedViewIndex));

            _Rendered = true;
        }


        /// <summary>
        ///     Will get invoked on rendering the current controls view.
        /// </summary>
        /// <param name="e"></param>
        public virtual async Task RenderView(RenderViewEventArgs e)
        {
        }

        private async Task CleanUpView()
        {
            var tasks = new List<Task>();

            foreach (var msg in Messages) tasks.Add(Device.DeleteMessage(msg));

            await Task.WhenAll(tasks);

            Messages.Clear();
        }

        /// <summary>
        ///     Forces render of control contents.
        /// </summary>
        public async Task ForceRender()
        {
            await CleanUpView();

            await RenderView(new RenderViewEventArgs(SelectedViewIndex));

            _Rendered = true;
        }

        public override async Task Cleanup()
        {
            Device.MessageSent -= Device_MessageSent;

            await CleanUpView();
        }
    }
}