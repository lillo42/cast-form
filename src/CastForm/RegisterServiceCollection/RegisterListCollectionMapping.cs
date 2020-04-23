using System;
using System.Collections.Generic;
using CastForm.Collection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm.RegisterServiceCollection
{
    /// <summary>
    /// Implementation of <see cref="IRegisterServiceCollectionType"/> for <see cref="ListCollectionMapping{TSource, TDestiny}"/>
    /// </summary>
    internal class RegisterListCollectionMapping : IRegisterServiceCollectionType
    {
        /// <inheritdoc />
        public void Register(Type source, Type destiny, IServiceCollection service)
        {
            var enumerable = typeof(ListCollectionMapping<,>).MakeGenericType(source, destiny);
            var mapper = typeof(IMap<,>).MakeGenericType(
                typeof(IEnumerable<>).MakeGenericType(source),
                typeof(List<>).MakeGenericType(destiny)
            );
            service.TryAddSingleton(mapper, enumerable);
        }
    }
}
