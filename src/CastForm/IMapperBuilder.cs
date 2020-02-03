using System;
using System.Linq.Expressions;

namespace CastForm
{
    public interface IMapperBuilder
    {
        IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>();
        IMapperBuilder AddMapper(IMapperBuilder mapperBuilder);

        IMapper Build();
    }

    public interface IMapperBuilder<TSource, TDestiny> : IMapperBuilder
    {
        IMapperBuilder<TSource, TDestiny> For<TDestinyMember, TSourceMember>(Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source);

        IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TSource, TMember>> source);
        IMapperBuilder<TDestiny, TSource> Reverse();
    }

    internal interface IRegisterMap
    {
        void Register();
    }
}
