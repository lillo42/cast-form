using CastForm.Maps;
using Microsoft.CodeAnalysis;

namespace CastForm.Generators
{
    internal class MapSpecification
    {
        public MapSpecification(ITypeSymbol destiny, 
            ITypeSymbol source,
            PropertyMapCollection properties)
        {
            Destiny = destiny;
            Source = source;
            Properties = properties;
        }

        public ITypeSymbol Destiny { get; }
        public ITypeSymbol Source { get; }
        public PropertyMapCollection Properties { get; }
    }
}
