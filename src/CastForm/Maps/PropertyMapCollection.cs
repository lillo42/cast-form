using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CastForm.Maps
{
    internal class PropertyMapCollection : ICollection<PropertyMap>
    {
        private readonly Dictionary<IPropertySymbol, PropertyMap> _propertiesMaps = new Dictionary<IPropertySymbol, PropertyMap>(SymbolEqualityComparer.Default);
        public IEnumerator<PropertyMap> GetEnumerator() => _propertiesMaps.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(PropertyMap item) => _propertiesMaps.Add(item.Destiny, item);

        public void Clear() => _propertiesMaps.Clear();

        public bool Contains(PropertyMap item) => _propertiesMaps.ContainsKey(item.Destiny);

        public void CopyTo(PropertyMap[] array, int arrayIndex) => _propertiesMaps.Values.CopyTo(array, arrayIndex);

        public bool Remove(PropertyMap item) => _propertiesMaps.Remove(item.Destiny);

        public int Count => _propertiesMaps.Count;

        public bool IsReadOnly => false;
    }
}
