using System;
using System.Runtime.Serialization;

namespace CastForm.Exceptions
{
    public class CastFormException : Exception
    {
        public CastFormException()
        {
        }

        public CastFormException(string message) 
            : base(message)
        {
        }

        public CastFormException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected CastFormException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
