﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using CastForm.Collection;
using CastForm.Generator;
using CastForm.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm
{
    /// <summary>
    /// Define rules of mapping
    /// </summary>
    /// <typeparam name="TSource">Source type</typeparam>
    /// <typeparam name="TDestiny">Destiny type to be created</typeparam>
    public class MapperBuilder<TSource, TDestiny> : IMapperBuilder<TSource, TDestiny>
    {
        private readonly ICollection<IRuleMapper> _rules = new List<IRuleMapper>();
        private readonly IMapperBuilder _parent;
        private readonly IServiceCollection _service;
        private readonly MapperGenerator _generator;
        private readonly HashCodeFactoryGenerator _hashCodeFactoryGenerator;

        /// <summary>
        /// Initialize a new instance of <see cref="MapperBuilder{TSource, TDestiny}"/>
        /// </summary>
        /// <param name="parent">The parent of this mapper</param>
        /// <param name="service">The <see cref="IServiceCollection"/> to register type. </param>
        /// <param name="hashCodeFactoryGenerator">The <see cref="HashCodeFactoryGenerator"/> is used to create GenHashCode.</param>
        public MapperBuilder(IMapperBuilder parent, IServiceCollection service, HashCodeFactoryGenerator hashCodeFactoryGenerator)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _hashCodeFactoryGenerator = hashCodeFactoryGenerator;
            _generator = new MapperGenerator(typeof(TSource), typeof(TDestiny), _rules, hashCodeFactoryGenerator);
        }

        /// <summary>
        /// Reverse map, but not reverse rules
        /// </summary>
        /// <returns></returns>
        public virtual IMapperBuilder<TDestiny, TSource> Reverse()
            => AddMapper<TDestiny, TSource>();

        /// <summary>
        /// Source type
        /// </summary>
        public virtual Type Source => typeof(TSource);

        /// <summary>
        /// Destination type
        /// </summary>
        public virtual Type Destiny => typeof(TDestiny);

        /// <summary>
        /// Rules mapper
        /// </summary>
        public IEnumerable<IRuleMapper> Rules 
            => _generator.CreateRules();

        /// <summary>
        /// Add Mapper
        /// </summary>
        /// <typeparam name="TSource1">Source type</typeparam>
        /// <typeparam name="TDestiny1">Destiny type to be created</typeparam>
        /// <returns></returns>
        public virtual IMapperBuilder<TSource1, TDestiny1> AddMapper<TSource1, TDestiny1>()
        {
            var mapper = new MapperBuilder<TSource1, TDestiny1>(this, _service, _hashCodeFactoryGenerator);
            _parent.AddMapper(mapper);
            return mapper;
        } 

        IMapperBuilder IMapperBuilder.AddMapper(IMapperBuilder mapperBuilder)
        {
            _parent.AddMapper(mapperBuilder);
            return this;
        }

        IMapper IMapperBuilder.Build() 
            => _parent.Build();
        
        /// <summary>
        /// Register Type in <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="mapperProperties"></param>
        public virtual void Register(IEnumerable<MapperProperty> mapperProperties)
        {
            var mapper = _generator.Generate(mapperProperties);

            _service.TryAddSingleton(typeof(IMap<TSource, TDestiny>), mapper);

            var enumerable = typeof(LazyEnumerableMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, IEnumerable<TDestiny>>), enumerable);
            
            var asyncEnumerable = typeof(IAsyncEnumerableMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IAsyncEnumerable<TSource>, IAsyncEnumerable<TDestiny>>), asyncEnumerable);
            
            var linkedList = typeof(ICollectionMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, ICollection<TDestiny>>), linkedList);

            var list = typeof(ListCollectionMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, List<TDestiny>>), list);

            var iList = typeof(IListCollectionMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, IList<TDestiny>>), iList);

            var hashSet = typeof(HashSetCollectionMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, HashSet<TDestiny>>), hashSet);

            var iSet = typeof(ISetCollectionMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, ISet<TDestiny>>), iSet);
        }

        /// <summary>
        /// Specific what property to property
        /// </summary>
        /// <typeparam name="TDestinyMember"></typeparam>
        /// <typeparam name="TSourceMember"></typeparam>
        /// <param name="destiny"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual IMapperBuilder<TSource, TDestiny> For<TDestinyMember, TSourceMember>(Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source)
        {

            if(destiny.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new NotSupportedException();
            }


            if(source.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new NotImplementedException();
            }

            var sourceProperty = ((source.Body as MemberExpression)!.Member as PropertyInfo)!;
            var destinyProperty = ((destiny.Body as MemberExpression)!.Member as PropertyInfo)!;
            _rules.Add(ForRuleFactory.CreateRule(destinyProperty, sourceProperty, _hashCodeFactoryGenerator));
            return this;
        }

        /// <summary>
        /// Property to be ignore
        /// </summary>
        /// <typeparam name="TMember"></typeparam>
        /// <param name="destiny"></param>
        /// <returns></returns>
        public virtual IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TDestiny, TMember>> destiny)
        {
            if (destiny.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new NotSupportedException();
            }

            var member = (MemberExpression)destiny.Body;
            _rules.Add(new IgnoreRule(member.Member));

            return this;
        }
    }
}
