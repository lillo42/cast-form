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
    public class MapperBuilder<TSource, TDestiny> : IMapperBuilder<TSource, TDestiny>, IRegisterMap
    {
        private readonly ICollection<IRuleMapper> _rules = new List<IRuleMapper>();
        private readonly IMapperBuilder _parent;
        private readonly IServiceCollection _service;

        public MapperBuilder(IMapperBuilder parent, IServiceCollection service)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public IMapperBuilder<TDestiny, TSource> Reverse()
            => AddMapper<TDestiny, TSource>();

        public IMapperBuilder<TSource1, TDestiny1> AddMapper<TSource1, TDestiny1>()
        {
            var mapper = new MapperBuilder<TSource1, TDestiny1>(this, _service);
            _parent.AddMapper(mapper);
            return mapper;
        } 

        IMapperBuilder IMapperBuilder.AddMapper(IMapperBuilder mapperBuilder)
        {
            _parent.AddMapper(mapperBuilder);
            return this;
        }

        void IRegisterMap.Register()
            => Register();

        private void Register()
        {
            var mapper = new MapperGenerator(typeof(TSource), typeof(TDestiny), _rules)
                .Generate();

            _service.TryAddSingleton(typeof(IMap<TSource, TDestiny>), mapper);
            
            var mappingEnumerable = typeof(LazyEnumerableMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, IEnumerable<TDestiny>>), mappingEnumerable);
            
        }

        IMapper IMapperBuilder.Build()
        {
            Register();
            return _parent.Build();
        }

        public IMapperBuilder<TSource, TDestiny> For<TDestinyMember, TSourceMember>(Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source)
        {
            if(source.Body.NodeType != ExpressionType.MemberAccess && destiny.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new NotSupportedException();
            }

            var sourceProperty = ((source.Body as MemberExpression)!.Member as PropertyInfo)!;
            var destinyProperty = ((destiny.Body as MemberExpression)!.Member as PropertyInfo)!;
            _rules.Add(ForRuleFactory.CreateRule(sourceProperty, destinyProperty));
            return this;
        }

        public IMapperBuilder<TSource, TDestiny> Ignore<TMember>(Expression<Func<TSource, TMember>> source)
        {
            if (source.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new NotSupportedException();
            }
            var member = (MemberExpression)source.Body;
            _rules.Add(new IgnoreRule(member.Member));

            return this;
        }

        
    }
}
