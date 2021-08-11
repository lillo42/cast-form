using System;

namespace CastForm.Exceptions
{
    public class DuplicatedMappingException : CastFormException
    {
        public DuplicatedMappingException(Type destiny, Type source)
            : base($"Duplicated mapping found. Destiny: {destiny.FullName} -> Source: {source.FullName}")
        {
            
        }
        
    }
}
