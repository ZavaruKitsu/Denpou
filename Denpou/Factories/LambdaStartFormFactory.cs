using Denpou.Base;
using Denpou.Interfaces;

namespace Denpou.Factories;

public class LambdaStartFormFactory : IStartFormFactory
{
    public delegate FormBase CreateFormDelegate();

    private readonly CreateFormDelegate _lambda;

    public LambdaStartFormFactory(CreateFormDelegate lambda)
    {
        _lambda = lambda;
    }

    public FormBase CreateForm()
    {
        return _lambda();
    }
}