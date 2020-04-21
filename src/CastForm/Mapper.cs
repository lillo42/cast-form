using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace CastForm
{
    /// <summary>
    /// Default implementation of <see cref="IMapper"/>.
    /// </summary>
    public class Mapper : IMapper
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Initialize a new instance of <see cref="Mapper"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to resolve mapper.</param>
        public Mapper(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc/>
        public TDestiny Map<TSource, TDestiny>(TSource source)
        {
            try
            {
                return _provider.GetRequiredService<IMap<TSource, TDestiny>>()
                    .Map(source);
            }
            finally
            {
                _provider.GetRequiredService<Counter>().Clean();
            }
        }

        /// <inheritdoc/>
        public TDestiny Map<TDestiny>(object source)
        {
            var sourceType = source.GetType();
            if (source is IEnumerable)
            {
                sourceType = typeof(IEnumerable<>).MakeGenericType(sourceType.GetGenericArguments()[0]);
            }

            if (source is IAsyncDisposable)
            {
                var interfaces = sourceType.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (@interface.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
                    {
                        sourceType = typeof(IAsyncEnumerable<>).MakeGenericType(sourceType.GetGenericArguments()[0]);
                        break;
                    }
                }
            }

            var mapperType = typeof(IMap<,>).MakeGenericType(sourceType, typeof(TDestiny));
            var mapper = (IMap)_provider.GetRequiredService(mapperType);

            try
            {
                return (TDestiny)mapper.Map(source);
            }
            finally
            {
                _provider.GetRequiredService<Counter>().Clean();
            }
        }
    }
}
