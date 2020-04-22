using System;
using Microsoft.Extensions.DependencyInjection;

namespace CastForm.RegisterServiceCollection
{
    /// <summary>
    /// Register a custom type mapper
    /// </summary>
    public interface IRegisterServiceCollectionType
    {
        /// <summary>
        /// Register custom mapper
        /// </summary>
        /// <param name="source">The source <see cref="Type"/>.</param>
        /// <param name="destiny">The destiny <see cref="Type"/>.</param>
        /// <param name="service">The <see cref="IServiceCollection"/>.</param>
        void Register(Type source, Type destiny, IServiceCollection service);
    }
}
