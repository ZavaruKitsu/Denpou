using System;
using System.Threading.Tasks;
using Denpou.Base;

namespace Denpou.Form;

public class ModalDialog : FormBase
{
    /// <summary>
    ///     Contains the parent from where the modal dialog has been opened.
    /// </summary>
    public FormBase ParentForm { get; set; }

    /// <summary>
    ///     This is a modal only function and does everything to close this form.
    /// </summary>
    public async Task CloseForm()
    {
        await CloseControls();

        await OnClosed(EventArgs.Empty);


        await ParentForm?.ReturnFromModal(this);
    }
}