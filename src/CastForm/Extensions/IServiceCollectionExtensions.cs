using System;
using CastForm;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    public static class IServiceCollectionExtensions
    {
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
