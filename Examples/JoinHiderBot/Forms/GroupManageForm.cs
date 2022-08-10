using System.Threading.Tasks;
using Denpou.Args;
using Denpou.Form;
using Telegram.Bot.Types.Enums;

namespace JoinHiderBot.Forms
{
    public class GroupManageForm : GroupForm
    {
        public override async Task OnMemberChanges(MemberChangeEventArgs e)
        {
            if (e.Type != MessageType.ChatMembersAdded && e.Type != MessageType.ChatMemberLeft)
                return;

            var m = e.Result.Message;
            await Device.DeleteMessage(m);
        }
    }
}