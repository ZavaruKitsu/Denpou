﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Denpou.Base;

[DebuggerDisplay("Device: {DeviceId}, {FormUri}")]
public class StateEntry
{
    public StateEntry()
    {
        Values = new Dictionary<string, object>();
    }

    /// <summary>
    ///     Contains the DeviceId of the entry.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    ///     Contains the Username (on privat chats) or Group title on groups/channels.
    /// </summary>
    public string ChatTitle { get; set; }

    /// <summary>
    ///     Contains additional values to save.
    /// </summary>
    public Dictionary<string, object> Values { get; set; }

    /// <summary>
    ///     Contains the full qualified namespace of the form to used for reload it via reflection.
    /// </summary>
    public string FormUri { get; set; }

    /// <summary>
    ///     Contains the assembly, where to find that form.
    /// </summary>
    public string QualifiedName { get; set; }
}