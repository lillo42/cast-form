using System;
using CastForm.DependencyInjection;

namespace CastForm
{
    public class MapperBuilder : IMapperBuilder
    {
        private readonly IDependencyInjectionContainer _container = new DependencyInjectionContainer();

        public MapperBuilder()
        {
            
        }

        public MapperBuilder(IDependencyInjectionContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public virtual IMapperBuilder<TSource, TDestiny> AddMapper<TSource, TDestiny>() 
            => new MapperBuilder<TSource, TDestiny>(this, _container);

        public virtual IMapper Build() 
            => _container.Resolver();
    }

}
