using System;
using System.Runtime.Serialization;

namespace CastForm.Exceptions
{
    /// <summary>
    /// Exception base for all exception in CastForm
    /// </summary>
    public class CastFormException : Exception
    {
        /// <summary>
        /// Initialize a new instance of <see cref="CastFormException"/>
        /// </summary>
        public CastFormException()
        {
            
        }

        /// <summary>
        /// Initialize a new instance of <see cref="CastFormException"/>
        /// </summary>
        /// <param name="message">The message that describe the error</param>
        public CastFormException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="CastFormException"/>
        /// </summary>
        /// <param name="message">The message that describe the error</param>
        /// <param name="innerException">The exception that is cause orf the current error, or null reference if no inner Exception is specified
        /// </param>
        public CastFormException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="CastFormException"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CastFormException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
