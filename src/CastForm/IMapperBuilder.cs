using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CastForm.Rules;
using Microsoft.Extensions.DependencyInjection;

namespace CastForm
{
    /// <summary>
    /// Define rules of mapping.
    /// </summary>
    public interface IMapperBuilder
    {
        /// <summary>
        /// Source Type
        /// </summary>
        Type Source { get; }

        /// <summary>
        /// Destiny type
        /// </summary>
        Type Destiny { get; }

        /// <summary>
        /// Rules mapper
        /// </summary>
        IEnumerable<IRuleMapper> Rules { get; }

        /// <summary>
        /// Create new <see cref="IMapperBuilder{TSource, TDestiny}"/> that could be used to map rules.
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestiny">Destiny type to be created</typeparam>
        /// <returns>New instance of <see cref="IMapperBuilder{TSource, TDestiny}"/></returns>
        IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>();

        /// <summary>
        /// Add current instance of  <see cref="IMapperBuilder"/>
        /// </summary>
        /// <param name="mapperBuilder">Instance to be add. </param>
        /// <returns>Current instance of <seealso cref="IMapperBuilder"/></returns>
        IMapperBuilder AddMapper(IMapperBuilder mapperBuilder);


        /// <summary>
        /// Create <see cref="IMapper"/>
        /// </summary>
        /// <returns>New instance of <see cref="IMapper"/></returns>
        IMapper Build();

        /// <summary>
        /// Register Create Type in <see cref="IServiceCollection"/>
        /// </summary>
        void Register(IEnumerable<MapperProperty> mapperProperties);
    }

    /// <summary>
    /// Define Mapper rules.
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destination type to be created</typeparam>
    public interface IMapperBuilder<TSource, TDestiny> : IMapperBuilder
    {
        /// <summary>
        /// Specific what property to property
        /// </summary>
        /// <typeparam name="TDestinyMember">Destiny property type</typeparam>
        /// <typeparam name="TSourceMember">Source property type</typeparam>
        /// <param name="destiny">Destiny property</param>
        /// <param name="source">Source property</param>
        /// <returns></returns>
        IMapperBuilder<TSource, TDestiny> For<TDestinyMember, TSourceMember>(Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source);


        /// <summary>
        /// Property to be ignore
        /// </summary>
        /// <typeparam name="TMember">Property to be ignore </typeparam>
        /// <param name="destiny">property to be ignore</param>
        /// <returns>Current instance of <seealso cref="IMapperBuilder"/></returns>
        IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TDestiny, TMember>> destiny);


        /// <summary>
        /// Reverse map
        /// </summary>
        /// <returns>New instance of <see cref="IMapperBuilder{TDestiny, TSource}"/></returns>
        IMapperBuilder<TDestiny, TSource> Reverse();
    }
}
