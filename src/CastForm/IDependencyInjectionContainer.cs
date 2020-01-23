using System;
namespace CastForm
{
    public interface IDependencyInjectionContainer
    {
        void RegisterType(Type type);

        IMapper Resolver();
    }
}
