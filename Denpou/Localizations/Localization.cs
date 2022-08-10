using System.Collections.Generic;

namespace Denpou.Localizations;

public abstract class Localization
{
    public Dictionary<string, string> Values = new();

    public string this[string key] => Values[key];
}