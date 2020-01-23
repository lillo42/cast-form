using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CastForm.Generator;
using CastForm.Rules;

namespace CastForm
{
    public class MapperBuilder<TSource, TDestiny> : IMapperBuilder<TSource, TDestiny>
    {
        private readonly IMapperBuilder _parent;
        private readonly ICollection<IRuleMapper> _rules = new List<IRuleMapper>();
        private readonly IDependencyInjectionContainer _container;

        public MapperBuilder(IMapperBuilder parent, IDependencyInjectionContainer container)
        {
            _parent = parent ?? throw new ArgumentNullException(nameof(parent));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IMapperBuilder<TSource1, TDestiny1> AddMapper<TSource1, TDestiny1>()
        {
            Register();
            return new MapperBuilder<TSource1, TDestiny1>(this, _container);
        }

        private void Register()
        {
            var gen = new MapperGenerator(typeof(TSource), typeof(TDestiny), _rules);
            var mapper = gen.Generate();
            _container.RegisterType(mapper);
        }

        IMapper IMapperBuilder.Build()
        {
            Register();
            return _parent.Build();
        }

        public IMapperBuilder<TSource, TDestiny> For(Expression<TSource> source, Expression<TDestiny> destiny)
        {
            throw new NotImplementedException();
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

        public IMapperBuilder<TDestiny, TSource> Reverse() 
            => new MapperBuilder<TDestiny, TSource>(this, _container);
    }
}
