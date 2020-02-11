using System;
using System.Linq.Expressions;

namespace CastForm
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMapperBuilder
    {
        /// <summary>
        /// Add Mapper
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestiny"></typeparam>
        /// <returns></returns>
        IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>();

        /// <summary>
        /// Add Mapper
        /// </summary>
        /// <param name="mapperBuilder"></param>
        /// <returns></returns>
        IMapperBuilder AddMapper(IMapperBuilder mapperBuilder);


        /// <summary>
        /// Create <see cref="IMapper"/>
        /// </summary>
        /// <returns></returns>
        IMapper Build();
    }

    public interface IMapperBuilder<TSource, TDestiny> : IMapperBuilder
    {
        /// <summary>
        /// Specific what property to property
        /// </summary>
        /// <typeparam name="TDestinyMember"></typeparam>
        /// <typeparam name="TSourceMember"></typeparam>
        /// <param name="destiny"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        IMapperBuilder<TSource, TDestiny> For<TDestinyMember, TSourceMember>(Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source);


        /// <summary>
        /// Ignore Property
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="source">property to be ignore</param>
        /// <returns></returns>
        IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TSource, TMember>> source);


        /// <summary>
        /// Reverse map
        /// </summary>
        /// <returns></returns>
        IMapperBuilder<TDestiny, TSource> Reverse();
    }

    internal interface IRegisterMap
    {
        void Register();
    }
}
