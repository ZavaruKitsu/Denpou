using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using TelegramBotBase.Args;
using TelegramBotBase.Attributes;
using TelegramBotBase.Base;
using TelegramBotBase.Enums;

namespace TelegramBotBase.Form
{
    /// <summary>
    ///     A form which cleans up old messages sent within
    /// </summary>
    public class AutoCleanForm : FormBase
    {
        public AutoCleanForm()
        {
            OldMessages = new List<int>();
            DeleteMode = eDeleteMode.OnEveryCall;
            DeleteSide = eDeleteSide.BotOnly;

            Init += AutoCleanForm_Init;

            Closed += AutoCleanForm_Closed;
        }

        [SaveState] public List<int> OldMessages { get; set; }

        [SaveState] public eDeleteMode DeleteMode { get; set; }

        [SaveState] public eDeleteSide DeleteSide { get; set; }

        private async Task AutoCleanForm_Init(object sender, InitEventArgs e)
        {
            if (Device == null)
                return;

            Device.MessageSent += Device_MessageSent;

            Device.MessageReceived += Device_MessageReceived;

            Device.MessageDeleted += Device_MessageDeleted;
        }

        private void Device_MessageDeleted(object sender, MessageDeletedEventArgs e)
        {
            if (OldMessages.Contains(e.MessageId))
                OldMessages.Remove(e.MessageId);
        }

        private void Device_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (DeleteSide == eDeleteSide.BotOnly)
                return;

            OldMessages.Add(e.Message.MessageId);
        }

        private void Device_MessageSent(object sender, MessageSentEventArgs e)
        {
            if (DeleteSide == eDeleteSide.UserOnly)
                return;

            OldMessages.Add(e.Message.MessageId);
        }

        public override async Task PreLoad(MessageResult message)
        {
            if (DeleteMode != eDeleteMode.OnEveryCall)
                return;

            await MessageCleanup();
        }

        /// <summary>
        ///     Adds a message to this of removable ones
        /// </summary>
        /// <param name="Id"></param>
        public void AddMessage(Message m)
        {
            OldMessages.Add(m.MessageId);
        }


        /// <summary>
        ///     Adds a message to this of removable ones
        /// </summary>
        /// <param name="Id"></param>
        public void AddMessage(int messageId)
        {
            OldMessages.Add(messageId);
        }

        /// <summary>
        ///     Keeps the message by removing it from the list
        /// </summary>
        /// <param name="Id"></param>
        public void LeaveMessage(int Id)
        {
            OldMessages.Remove(Id);
        }

        /// <summary>
        ///     Keeps the last sent message
        /// </summary>
        public void LeaveLastMessage()
        {
            if (OldMessages.Count == 0)
                return;

            OldMessages.RemoveAt(OldMessages.Count - 1);
        }

        private async Task AutoCleanForm_Closed(object sender, EventArgs e)
        {
            if (DeleteMode != eDeleteMode.OnLeavingForm)
                return;

            MessageCleanup().Wait();
        }

        /// <summary>
        ///     Cleans up all remembered messages.
        /// </summary>
        /// <returns></returns>
        public async Task MessageCleanup()
        {
            var oldMessages = OldMessages.AsEnumerable();

#if !NETSTANDARD2_0
            while (oldMessages.Any())
            {
                using var cts = new CancellationTokenSource();
                var deletedMessages = new ConcurrentBag<int>();
                var parallelQuery = OldMessages.AsParallel()
                                                .WithCancellation(cts.Token);
                Task retryAfterTask = null;
                try
                {
                    parallelQuery.ForAll(i =>
                    {
                        try
                        {
                            Device.DeleteMessage(i).GetAwaiter().GetResult();
                            deletedMessages.Add(i);
                        }
                        catch (ApiRequestException req) when (req.ErrorCode == 400)
                        {
                            deletedMessages.Add(i);
                        }
                    });
                }
                catch (AggregateException ex)
                {
                    cts.Cancel();

                    var retryAfterSeconds = ex.InnerExceptions
                        .Where(e => e is ApiRequestException apiEx && apiEx.ErrorCode == 429)
                        .Max(e => (int?)((ApiRequestException)e).Parameters.RetryAfter) ?? 0;
                    retryAfterTask = Task.Delay(retryAfterSeconds * 1000);
                }

                //deletedMessages.AsParallel().ForAll(i => Device.OnMessageDeleted(new MessageDeletedEventArgs(i)));

                oldMessages = oldMessages.Where(x => !deletedMessages.Contains(x));
                if (retryAfterTask != null)
                    await retryAfterTask;
            }
#else
            while (oldMessages.Any())
                using (var cts = new CancellationTokenSource())
                {
                    var deletedMessages = new ConcurrentBag<int>();
                    var parallelQuery = OldMessages.AsParallel()
                        .WithCancellation(cts.Token);
                    Task retryAfterTask = null;
                    try
                    {
                        parallelQuery.ForAll(i =>
                        {
                            try
                            {
                                Device.DeleteMessage(i).GetAwaiter().GetResult();
                                deletedMessages.Add(i);
                            }
                            catch (ApiRequestException req) when (req.ErrorCode == 400)
                            {
                                deletedMessages.Add(i);
                            }
                        });
                    }
                    catch (AggregateException ex)
                    {
                        cts.Cancel();

                        var retryAfterSeconds = ex.InnerExceptions
                            .Where(e => e is ApiRequestException apiEx && apiEx.ErrorCode == 429)
                            .Max(e => ((ApiRequestException)e).Parameters.RetryAfter) ?? 0;
                        retryAfterTask = Task.Delay(retryAfterSeconds * 1000);
                    }

                    //deletedMessages.AsParallel().ForAll(i => Device.OnMessageDeleted(new MessageDeletedEventArgs(i)));
                    oldMessages = oldMessages.Where(x => !deletedMessages.Contains(x));
                    if (retryAfterTask != null)
                        await retryAfterTask;
                }


#endif

            OldMessages.Clear();
        }
    }
}