using System;
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
    public class MapperBuilder<TSource, TDestiny> : IMapperBuilder<TSource, TDestiny>
    {
        private readonly ICollection<IRuleMapper> _rules = new List<IRuleMapper>();
        private readonly IMapperBuilder _parent;
        private readonly IServiceCollection _service;
        private readonly MapperGenerator _generator;
        private readonly HashCodeFactoryGenerator _hashCodeFactoryGenerator;

        public MapperBuilder(IMapperBuilder parent, IServiceCollection service, HashCodeFactoryGenerator hashCodeFactoryGenerator)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _hashCodeFactoryGenerator = hashCodeFactoryGenerator;
            _generator = new MapperGenerator(typeof(TSource), typeof(TDestiny), _rules, hashCodeFactoryGenerator);
        }

        public virtual IMapperBuilder<TDestiny, TSource> Reverse()
            => AddMapper<TDestiny, TSource>();

        public virtual Type Source => typeof(TSource);
        public virtual Type Destiny => typeof(TDestiny);

        public IEnumerable<IRuleMapper> Rules 
            => _generator.CreateRules();

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

        public virtual void Register(IEnumerable<MapperProperty> mapperProperties)
        {
            var mapper = _generator.Generate(mapperProperties);

            _service.TryAddSingleton(typeof(IMap<TSource, TDestiny>), mapper);

            var enumerable = typeof(LazyEnumerableMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
            _service.TryAddSingleton(typeof(IMap<IEnumerable<TSource>, IEnumerable<TDestiny>>), enumerable);

            var linkedList = typeof(LinkedListCollectionMapping<,>).MakeGenericType(typeof(TSource), typeof(TDestiny));
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
