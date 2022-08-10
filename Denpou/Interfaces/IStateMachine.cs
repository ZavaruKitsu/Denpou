using System;
using Denpou.Args;
using Denpou.Base;

namespace Denpou.Interfaces;

public interface IStateMachine
{
    Type FallbackStateForm { get; }

    void SaveFormStates(SaveStatesEventArgs e);

    StateContainer LoadFormStates();
}