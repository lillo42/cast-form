using System;
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
            var mapperType = typeof(IMap<,>).MakeGenericType(source.GetType(), typeof(TDestiny));
            var mapper = (IMap)_provider.GetRequiredService(mapperType);
            return (TDestiny)mapper.Map(source);
        }
    }
}
