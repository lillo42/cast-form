using System;
using System.Runtime.Serialization;

namespace CastForm.Exceptions
{
    /// <summary>
    /// The base <see cref="Exception"/> class.
    /// </summary>
    public abstract class CastFormException : Exception
    {
        protected CastFormException()
        {
        }

        protected CastFormException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected CastFormException(string? message) : base(message)
        {
        }

        protected CastFormException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
