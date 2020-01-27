using System;

namespace System.Reflection
{
    internal static class TypeExtensions
    {
        public static bool IsNetType(this Type type)
            => type.IsPrimitive
               || type == typeof(string)
               || type == typeof(DateTime)
               || type == typeof(decimal);
    }
}
