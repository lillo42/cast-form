using System;
using CastForm;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddCastFrom(this IServiceCollection service, Action<IMapperBuilder> mapper)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            service.TryAddSingleton<IMapper, Mapper>();
            
            var builder = new MapperBuilder(service);
            mapper(builder);
            ((IRegisterMap)builder).Register();
        }
    }
}
