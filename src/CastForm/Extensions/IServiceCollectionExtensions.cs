using System;
using CastForm;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Add CastForm mapper
        /// </summary>
        /// <param name="service"></param>
        /// <param name="mapperBuilder">Build all maps that should be used</param>
        public static void AddCastFrom(this IServiceCollection service, Action<IMapperBuilder> mapperBuilder)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (mapperBuilder == null)
            {
                throw new ArgumentNullException(nameof(mapperBuilder));
            }

            service.TryAddSingleton<IMapper, Mapper>();
            service.TryAddSingleton<Counter>();

            var builder = new MapperBuilder(service);
            mapperBuilder(builder);
            builder.Build();
        }
    }
}
