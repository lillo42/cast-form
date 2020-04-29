using System;

namespace CastForm.Mappers
{
    /// <summary>
    /// Map <see cref="string"/> to <see cref="TimeSpan"/>.
    /// </summary>
    public class StringToTimeSpanMapper : IMap<string, TimeSpan>
    {
        /// <inheritdoc/>
        public TimeSpan Map(string source) 
            => TimeSpan.Parse(source);
    }
}
