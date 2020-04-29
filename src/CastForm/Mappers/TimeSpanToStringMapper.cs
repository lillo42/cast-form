using System;

namespace CastForm.Mappers
{
    /// <summary>
    /// Map <see cref="string"/> to <see cref="TimeSpan"/>.
    /// </summary>
    public class TimeSpanToStringMapper : IMap<TimeSpan, string>
    {
        /// <inheritdoc/>
        public string Map(TimeSpan source)
        {
            return source.ToString();
        }
    }
}
