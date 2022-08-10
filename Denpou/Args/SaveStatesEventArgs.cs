using Denpou.Base;

namespace Denpou.Args;

public class SaveStatesEventArgs
{
    public SaveStatesEventArgs(StateContainer states)
    {
        States = states;
    }

    public StateContainer States { get; set; }
}