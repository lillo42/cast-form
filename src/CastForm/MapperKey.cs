using System;

namespace CastForm
{
    public readonly struct MapperKey : IEquatable<MapperKey>
    {
        public MapperKey(Type source, Type destiny)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Destiny = destiny ?? throw new ArgumentNullException(nameof(destiny));
        }

        public Type Source { get; }
        public Type Destiny { get; }

        public bool Equals(MapperKey other)
        {
            return Source == other.Source
                   && Destiny == other.Destiny;
        }

        public override bool Equals(object obj)
            => obj is MapperKey other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Source, Destiny);

        public static bool operator ==(MapperKey obj1, MapperKey obj2)
            => obj1.Equals(obj2);

        public static bool operator !=(MapperKey obj1, MapperKey obj2)
            => !obj1.Equals(obj2);
    }
}
