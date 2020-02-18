using System;
using System.Reflection;

namespace CastForm
{
    public readonly struct MapperProperty : IEquatable<MapperProperty>
    {
        public Type Destiny { get; }
        public PropertyInfo DestinyProperty { get; }

        public Type Source { get; }
        public PropertyInfo? SourceProperty { get; }

        public MapperProperty(Type destiny, PropertyInfo destinyType, Type source, PropertyInfo? sourceProperty)
        {
            Destiny = destiny ?? throw new ArgumentNullException(nameof(destiny));
            DestinyProperty = destinyType ?? throw new ArgumentNullException(nameof(destinyType));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            SourceProperty = sourceProperty;
        }


        public bool Equals(MapperProperty other)
        {
            return Destiny.Equals(other.Destiny) 
                   && DestinyProperty.Equals(other.DestinyProperty) 
                   && Source.Equals(other.Source) 
                   && Equals(SourceProperty, other.SourceProperty);
        }

        public override bool Equals(object? obj) 
            => obj is MapperProperty other && Equals(other);

        public override int GetHashCode() 
            => Destiny.GetHashCode() ^ DestinyProperty.GetHashCode() ^ Source.GetHashCode() ^ (SourceProperty?.GetHashCode() ?? 0);
    }
}
