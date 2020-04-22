using System;
using System.Collections.Generic;
using CastForm.Collection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm.RegisterServiceCollection
{
    internal class RegisterIAsyncEnumerableMapping : IRegisterServiceCollectionType
    {
        public void Register(Type source, Type destiny, IServiceCollection service)
        {
            var enumerable = typeof(IAsyncEnumerableMapping<,>).MakeGenericType(source, destiny);
            var mapper = typeof(IMap<,>).MakeGenericType(
                typeof(IAsyncEnumerable<>).MakeGenericType(source),
                typeof(IAsyncEnumerable<>).MakeGenericType(destiny)
            );
            service.TryAddSingleton(mapper, enumerable);
        }
    }
}
