using System;
using CastForm.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CastForm.RegisterServiceCollection
{ 
    /// <summary>
    /// Register default mappers
    /// </summary>
    public class RegisterMapper : IRegisterServiceCollectionType
    {
        /// <inheritdoc />
        public void Register(Type source, Type destiny, IServiceCollection service)
        {
            service.TryAddSingleton<IMap<string, TimeSpan>>(new StringToTimeSpanMapper());
            service.TryAddSingleton<IMap<TimeSpan, string>>(new TimeSpanToStringMapper());
        }
    }
}
