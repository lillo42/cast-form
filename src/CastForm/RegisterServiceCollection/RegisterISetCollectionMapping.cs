using System;
using System.Collections.Generic;
using CastForm.Collection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm.RegisterServiceCollection
{
    /// <summary>
    /// Implementation of <see cref="IRegisterServiceCollectionType"/> for <see cref="ISetCollectionMapping{TSource, TDestiny}"/>
    /// </summary>
    public class RegisterISetCollectionMapping : IRegisterServiceCollectionType
    {
        /// <inheritdoc />
        public void Register(Type source, Type destiny, IServiceCollection service)
        {
            var enumerable = typeof(ISetCollectionMapping<,>).MakeGenericType(source, destiny);
            var mapper = typeof(IMap<,>).MakeGenericType(
                typeof(IEnumerable<>).MakeGenericType(source),
                typeof(ISet<>).MakeGenericType(destiny)
            );
            service.TryAddSingleton(mapper, enumerable);
        }
    }
}
