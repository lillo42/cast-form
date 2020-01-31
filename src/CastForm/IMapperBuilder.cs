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
        IMapperBuilder<TSource, TDestiny> For<TSourceMember, TDestinyMember>(Expression<Func<TSource, TSourceMember>> source, Expression<Func<TDestiny, TDestinyMember>> destiny);

        IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TSource, TMember>> source);
        IMapperBuilder<TDestiny, TSource> Reverse();
    }
}
