using System;
using System.Linq.Expressions;

namespace CastForm
{
    public abstract class MapperClass
    {
        public MapperClass<TSource, TDestiny> CreateMapper<TSource, TDestiny>()
        {
            return new InternalMapperClass<TSource, TDestiny>();
        }
    }

    public abstract class MapperClass<TSource, TDestiny>
    {
        public MapperClass<TSource, TDestiny> CreateMapper<TSource, TDestiny>()
        {
            return new InternalMapperClass<TSource, TDestiny>();
        }
        
        public MapperClass<TDestiny, TSource> Reverse()
        {
            return new InternalMapperClass<TDestiny, TSource>();
        }

        public MapperClass<TSource, TDestiny> For<TDestinyMember, TSourceMember>(
            Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source)
        {
            return this;
        }
        
        public MapperClass<TSource, TDestiny> Ignore<TDestinyMember>(Expression<Func<TDestiny, TDestinyMember>> destiny)
        {
            return this;
        }
    }

    internal class InternalMapperClass<TSource, TDestiny> : MapperClass<TSource, TDestiny>
    {
        
    }
}
