﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CastForm
{
    /// <summary>
    /// 
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

        /// <summary>
        /// Register Create Type in <see cref="IServiceCollection"/>
        /// </summary>
        void Register(IEnumerable<MapperProperty> mapperProperties);
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
        /// <param name="destiny">property to be ignore</param>
        /// <returns></returns>
        IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TDestiny, TMember>> destiny);


        /// <summary>
        /// Reverse map
        /// </summary>
        /// <returns></returns>
        IMapperBuilder<TDestiny, TSource> Reverse();
    }
}
