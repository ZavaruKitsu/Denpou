using System;

namespace Denpou.Attributes;

/// <summary>
///     Declares that the field or property should be saved and recovered on restart.
/// </summary>
public class SaveStateAttribute : Attribute
{
    public string Key { get; set; }
}