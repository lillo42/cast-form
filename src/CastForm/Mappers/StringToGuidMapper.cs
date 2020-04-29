using System;

namespace CastForm.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public class StringToGuidMapper : IMap<string, Guid>
    {
        /// <inheritdoc/>
        public Guid Map(string source)
        {
            return Guid.Parse(source);
        }
    }
}
