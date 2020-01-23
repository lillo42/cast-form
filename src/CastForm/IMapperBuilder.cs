using System;
using System.Linq.Expressions;

namespace CastForm
{
    public interface IMapperBuilder
    {
        IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>();

        IMapper Build();
    }

    public interface IMapperBuilder<TSource, TDestiny> : IMapperBuilder
    {
        IMapperBuilder<TSource, TDestiny> For(Expression<TSource> source, Expression<TDestiny> destiny);

        IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TSource, TMember>> source);

        IMapperBuilder<TDestiny, TSource> Reverse();
    }
}
