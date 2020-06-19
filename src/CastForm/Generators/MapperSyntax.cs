using System;
using System.Collections.Generic;
using CastForm.Generators.Rules;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CastForm.Generators
{
    internal class MapperSyntax : IEquatable<MapperSyntax>
    {
        public MapperSyntax(TypeSyntax @from, TypeSyntax to)
        {
            From = @from;
            To = to;
        }

        public TypeSyntax From { get; }
        public TypeSyntax To { get; }
        
        public List<IRule> Rules { get; } = new List<IRule>();

        public bool Equals(MapperSyntax? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((MapperSyntax) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }
    }
}
