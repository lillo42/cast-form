using System;
using System.Collections.Generic;

namespace CastForm
{
    public class Mapper : IMapper
    {
        private readonly Dictionary<MapperKey, IMap> _mappers;

        public Mapper(Dictionary<MapperKey, IMap> mappers)
        {
            _mappers = mappers;
        }

        public TDestiny Map<TSource, TDestiny>(TSource source)
        {
            var key = new MapperKey(typeof(TSource), typeof(TDestiny));

            if (!_mappers.TryGetValue(key, out var mapper))
            {
                throw new Exception();
            }

            return ((IMap<TSource, TDestiny>) mapper).Map(source);
        }

        public TDestiny Map<TDestiny>(object source)
        {
            var key = new MapperKey(source.GetType(), typeof(TDestiny));

            if (!_mappers.TryGetValue(key, out var mapper))
            {
                throw new Exception();
            }

            return (TDestiny)mapper.Map(source);
        }
    }
}
