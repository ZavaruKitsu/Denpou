using System;
using Denpou.Base;
using Denpou.Interfaces;

namespace Denpou.Factories;

public class DefaultStartFormFactory : IStartFormFactory
{
    private readonly Type _startFormClass;

    public DefaultStartFormFactory(Type startFormClass)
    {
        if (!typeof(FormBase).IsAssignableFrom(startFormClass))
            throw new ArgumentException("startFormClass argument must be a FormBase type");

        _startFormClass = startFormClass;
    }


    public FormBase CreateForm()
    {
        return _startFormClass.GetConstructor(new Type[] { })?.Invoke(new object[] { }) as FormBase;
    }
}