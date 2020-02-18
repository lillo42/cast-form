using System;
using System.Collections.Generic;
using System.Linq;
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
            return _service.BuildServiceProvider()
                .GetRequiredService<IMapper>();
        }
    }
}
