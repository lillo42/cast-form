using System;
using System.Collections.Generic;
using CastForm.Collection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm.RegisterServiceCollection
{
    /// <summary>
    /// Implementation of <see cref="IRegisterServiceCollectionType"/> for <see cref="IListCollectionMapping{TSource, TDestiny}"/>
    /// </summary>
    public class RegisterIListCollectionMapping : IRegisterServiceCollectionType
    {
        /// <inheritdoc />
        public void Register(Type source, Type destiny, IServiceCollection service)
        {
            var enumerable = typeof(IListCollectionMapping<,>).MakeGenericType(source, destiny);
            var mapper = typeof(IMap<,>).MakeGenericType(
                typeof(IEnumerable<>).MakeGenericType(source),
                typeof(IList<>).MakeGenericType(destiny)
            );
            service.TryAddSingleton(mapper, enumerable);
        }
    }
}
