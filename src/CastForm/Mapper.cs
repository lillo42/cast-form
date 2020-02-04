using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace CastForm
{
    public class Mapper : IMapper
    {
        private readonly IServiceProvider _provider;

        public Mapper(IServiceProvider mappers)
        {
            _provider = mappers ?? throw new ArgumentNullException(nameof(mappers));
        }

        public TDestiny Map<TSource, TDestiny>(TSource source)
        {
            return _provider.GetRequiredService<IMap<TSource, TDestiny>>()
                .Map(source);
        }

        public TDestiny Map<TDestiny>(object source)
        {
            var sourceType = source.GetType();
            if (source is IEnumerable)
            {
                sourceType = typeof(IEnumerable<>).MakeGenericType(sourceType.GetGenericArguments()[0]);
            }

            var mapperType = typeof(IMap<,>).MakeGenericType(sourceType, typeof(TDestiny));
            var mapper = (IMap)_provider.GetRequiredService(mapperType);
            return (TDestiny)mapper.Map(source);
        }
    }
}
