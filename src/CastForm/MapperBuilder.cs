using System;
using System.Collections.Generic;
using System.Linq;
using CastForm.Generator;
using CastForm.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm
{
    /// <summary>
    /// 
    /// </summary>
    public class MapperBuilder : IMapperBuilder
    {
        private readonly IServiceCollection _service;
        private readonly HashCodeFactoryGenerator _hashCodeFactoryGenerator;

        /// <summary>
        /// Collection of <see cref="IMapperBuilder"/>
        /// </summary>
        public ICollection<IMapperBuilder> Mappers { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="MapperBuilder"/>
        /// </summary>
        public MapperBuilder()
            : this(new ServiceCollection())
        {
            
        }

        /// <summary>
        /// Initialize a new instance of <see cref="MapperBuilder"/>
        /// </summary>
        /// <param name="service"></param>
        public MapperBuilder(IServiceCollection service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            Mappers = new LinkedList<IMapperBuilder>();
            _service.TryAddSingleton<IMapper, Mapper>();
            _service.TryAddSingleton<Counter>();
            _hashCodeFactoryGenerator = new HashCodeFactoryGenerator();
        }

        
        Type IMapperBuilder.Source => default!;
        

        Type IMapperBuilder.Destiny => default!;

        IEnumerable<IRuleMapper> IMapperBuilder.Rules => Enumerable.Empty<IRuleMapper>();

        /// <summary>
        /// Add <see cref="IMapperBuilder"/>
        /// </summary>
        /// <typeparam name="TSource">Source type</typeparam>
        /// <typeparam name="TDestiny">Destination type to create</typeparam>
        /// <returns></returns>
        public virtual IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>()
        {
            var mapper = new MapperBuilder<TSource, TDestiny>(this, _service, _hashCodeFactoryGenerator);
            Mappers.Add(mapper);
            return mapper;
        }

        /// <summary>
        /// Add <see cref="IMapperBuilder"/>
        /// </summary>
        /// <param name="mapperBuilder">The <see cref="IMapperBuilder"/> to add </param>
        /// <returns></returns>
        public virtual IMapperBuilder AddMapper(IMapperBuilder mapperBuilder)
        {
            Mappers.Add(mapperBuilder);
            return this;
        }

        /// <summary>
        /// Create a new instance of <see cref="IMapper"/>
        /// </summary>
        /// <returns>New instance of <see cref="IMapper"/></returns>
        public virtual IMapper Build()
        {
            var mappers = new LinkedList<MapperProperty>();

            foreach (var mapper in Mappers)
            {
                _hashCodeFactoryGenerator.Add(mapper.Destiny);
                _hashCodeFactoryGenerator.Add(mapper.Source);

                foreach (var rule in mapper.Rules)
                {
                    mappers.AddLast(new MapperProperty(mapper.Destiny, rule.DestinyProperty, mapper.Source, rule.SourceProperty));
                }
            }

            _hashCodeFactoryGenerator.Build();

            return Build(mappers);
        }

        /// <summary>
        /// Create new instance of <see cref="IMapper"/>
        /// </summary>
        /// <param name="mapperProperties">All mapper</param>
        /// <returns>New instance of <see cref="IMapper"/></returns>
        public virtual IMapper Build(IEnumerable<MapperProperty> mapperProperties)
        {
            foreach (var mapper in Mappers)
            {
                mapper.Register(mapperProperties);
            }

            return _service.BuildServiceProvider()
                .GetRequiredService<IMapper>();
        }

        /// <summary>
        /// Create <see cref="IMapper"/> and register in <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="mapperProperties"></param>
        public virtual void Register(IEnumerable<MapperProperty> mapperProperties) 
            => Build(mapperProperties);
    }
}
