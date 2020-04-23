using System;
using System.Collections.Generic;
using System.Linq;
using CastForm.Generator;
using CastForm.RegisterServiceCollection;
using CastForm.Rules;
using CastForm.Rules.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm
{
    /// <summary>
    /// Define rules of mapping. Implementation of <see cref="IMapperBuilder"/>
    /// </summary>
    public class MapperBuilder : IMapperBuilder
    {
        private readonly IServiceCollection _service;
        private readonly IHashCodeFactoryGenerator _hashCodeFactoryGenerator;

        /// <summary>
        /// Collection of <see cref="IMapperBuilder"/>
        /// </summary>
        public ICollection<IMapperBuilder> Mappers { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="MapperBuilder"/>.
        /// </summary>
        public MapperBuilder()
            : this(new ServiceCollection())
        {
            
        }

        /// <summary>
        /// Initialize a new instance of <see cref="MapperBuilder"/>
        /// </summary>
        /// <param name="service">The <see cref="IServiceCollection"/>.</param>
        public MapperBuilder(IServiceCollection service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            Mappers = new LinkedList<IMapperBuilder>();
            _service.TryAddSingleton<IMapper, Mapper>();
            _service.TryAddSingleton<Counter>();
            _hashCodeFactoryGenerator = new HashCodeFactoryGenerator();
        }

        /// <summary>
        /// Initialize a new instance of <see cref="MapperBuilder"/>.
        /// </summary>
        /// <param name="service">The <see cref="IServiceCollection"/>.</param>
        /// <param name="hashCodeFactoryGenerator">The <see cref="IHashCodeFactoryGenerator"/>.</param>
        public MapperBuilder(IServiceCollection service, IHashCodeFactoryGenerator hashCodeFactoryGenerator)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            Mappers = new LinkedList<IMapperBuilder>();
            _service.TryAddSingleton<IMapper, Mapper>();
            _service.TryAddSingleton<Counter>();
            _hashCodeFactoryGenerator = hashCodeFactoryGenerator;
        }
        
        Type IMapperBuilder.Source => default!;

        Type IMapperBuilder.Destiny => default!;

        IEnumerable<IRuleMapper> IMapperBuilder.Rules => Enumerable.Empty<IRuleMapper>();

        /// <inheritdoc/>
        public virtual IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>()
        {
            var mapper = new MapperBuilder<TSource, TDestiny>(this, _service, _hashCodeFactoryGenerator);
            Mappers.Add(mapper);
            return mapper;
        }

        /// <inheritdoc/>
        public virtual IMapperBuilder AddMapper(IMapperBuilder mapperBuilder)
        {
            Mappers.Add(mapperBuilder);
            return this;
        }

        /// <inheritdoc/>
        public IMapperBuilder AddRegisterServiceCollectionType(IRegisterServiceCollectionType registerType)
        {
            Registers.Add(registerType);
            return this;
        }

        /// <inheritdoc/>
        public IMapperBuilder AddRuleFactory(IRuleFactory factory)
        {
            Registers.Add(factory);
            return this;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public virtual void Register(IEnumerable<MapperProperty> mapperProperties) 
            => Build(mapperProperties);
    }
}
