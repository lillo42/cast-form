using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm
{
    public class MapperBuilder : IMapperBuilder, IRegisterMap
    {
        private readonly IServiceCollection _service;
        private readonly ICollection<IMapperBuilder> _mappers;
        public MapperBuilder()
            : this(new ServiceCollection())
        {
            
        }

        public MapperBuilder(IServiceCollection service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mappers = new LinkedList<IMapperBuilder>();
            _service.TryAddSingleton<IMapper, Mapper>();
        }

        public virtual IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>()
        {
            var mapper = new MapperBuilder<TSource, TDestiny>(this, _service);
            _mappers.Add(mapper);
            return mapper;
        }

        public virtual IMapperBuilder AddMapper(IMapperBuilder mapperBuilder)
        {
            _mappers.Add(mapperBuilder);
            return this;
        }

        public virtual IMapper Build() 
            => _service.BuildServiceProvider()
                .GetRequiredService<IMapper>();
        void IRegisterMap.Register()
        {
            foreach (var mapper in _mappers)
            {
                ((IRegisterMap)mapper).Register();
            }
        }
    }
}
