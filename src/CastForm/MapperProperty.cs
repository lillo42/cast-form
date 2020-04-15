using System;
using System.Reflection;

namespace CastForm
{
    /// <summary>
    /// Keep information about Map. How to how, source/source property to destiny/destiny property
    /// </summary>
    public readonly struct MapperProperty : IEquatable<MapperProperty>
    {
        /// <summary>
        /// Destiny type
        /// </summary>
        public Type Destiny { get; }

        /// <summary>
        /// Destiny property
        /// </summary>
        public PropertyInfo DestinyProperty { get; }

        /// <summary>
        /// Source type
        /// </summary>
        public Type Source { get; }

        /// <summary>
        /// Source property
        /// </summary>
        public PropertyInfo? SourceProperty { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destiny"></param>
        /// <param name="destinyType"></param>
        /// <param name="source"></param>
        /// <param name="sourceProperty"></param>
        public MapperProperty(Type destiny, PropertyInfo destinyType, Type source, PropertyInfo? sourceProperty)
        {
            Destiny = destiny ?? throw new ArgumentNullException(nameof(destiny));
            DestinyProperty = destinyType ?? throw new ArgumentNullException(nameof(destinyType));
            Source = source ?? throw new ArgumentNullException(nameof(source));
            SourceProperty = sourceProperty;
        }


        /// <summary>
        /// Check if it is equal to other <see cref="MapperProperty"/>
        /// </summary>
        /// <param name="other">The <see cref="MapperProperty"/> to comparer</param>
        /// <returns></returns>
        public bool Equals(MapperProperty other)
        {
            return DestinyProperty.Equals(other.DestinyProperty)
                   && Equals(SourceProperty, other.SourceProperty);
        }

        /// <summary>
        /// Check if it is equal to other <see cref="MapperProperty"/>
        /// </summary>
        /// <param name="obj">The <see cref="MapperProperty"/> to comparer</param>
        /// <returns></returns>
        public override bool Equals(object? obj) 
            => obj is MapperProperty other && Equals(other);

        /// <summary>
        /// Get object hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() 
            => Destiny.GetHashCode() ^ DestinyProperty.GetHashCode() ^ Source.GetHashCode() ^ (SourceProperty?.GetHashCode() ?? 0);
    }
}
