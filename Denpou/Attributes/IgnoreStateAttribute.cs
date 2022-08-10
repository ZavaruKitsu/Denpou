using System;

namespace Denpou.Attributes;

/// <summary>
///     Declares that this class should not be getting serialized
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreStateAttribute : Attribute
{
}