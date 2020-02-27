using System;
using System.Collections.Generic;
using System.Linq;
using CastForm.Generator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm
{
    public class MapperBuilder : IMapperBuilder
    {
        private readonly IServiceCollection _service;
        public ICollection<IMapperBuilder> Mappers { get; }

        public MapperBuilder()
            : this(new ServiceCollection())
        {
            
        }

        public MapperBuilder(IServiceCollection service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            Mappers = new LinkedList<IMapperBuilder>();
            _service.TryAddSingleton<IMapper, Mapper>();
            _service.TryAddSingleton<Counter>();
        }

        public Type Source => default!;
        public Type Destiny => default!;

        public IEnumerable<IRuleMapper> Rules => Enumerable.Empty<IRuleMapper>();

        public virtual IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>()
        {
            var mapper = new MapperBuilder<TSource, TDestiny>(this, _service);
            Mappers.Add(mapper);
            return mapper;
        }

        public virtual IMapperBuilder AddMapper(IMapperBuilder mapperBuilder)
        {
            Mappers.Add(mapperBuilder);
            return this;
        }

        public virtual IMapper Build()
        {
            var mappers = new LinkedList<MapperProperty>();

            HashCodeFactoryGenerator.Instance = new HashCodeFactoryGenerator();
            foreach (var mapper in Mappers)
            {
                HashCodeFactoryGenerator.Instance.Add(mapper.Destiny);
                HashCodeFactoryGenerator.Instance.Add(mapper.Source);

                foreach (var rule in mapper.Rules)
                {
                    mappers.AddLast(new MapperProperty(mapper.Destiny, rule.DestinyProperty, mapper.Source, rule.SourceProperty));
                }
            }

            HashCodeFactoryGenerator.Instance.Build();
            return Build(mappers);
        }

        public virtual IMapper Build(IEnumerable<MapperProperty> mapperProperties)
        {
            foreach (var mapper in Mappers)
            {
                mapper.Register(mapperProperties);
            }

            return _service.BuildServiceProvider()
                .GetRequiredService<IMapper>();
        }

        public virtual void Register(IEnumerable<MapperProperty> mapperProperties) 
            => Build(mapperProperties);
    }
}
