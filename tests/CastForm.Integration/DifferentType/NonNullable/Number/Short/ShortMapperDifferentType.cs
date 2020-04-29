using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.Short
{
    public class ShortStringMapperDifferentType : MapperDifferentType<short, string>
    {
        protected override void AreEqual(short source, string destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortCharMapperDifferentType : MapperDifferentType<short, char>
    {
        protected override short UpdateValue(short source) 
            => Math.Abs(source);

        protected override void AreEqual(short source, char destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortByteMapperDifferentType : MapperDifferentType<short, byte>
    {
        
        protected override short UpdateValue(short source)
        {
            if (source > byte.MaxValue)
            {
                return Convert.ToInt16(byte.MaxValue);
            }
            
            if (source < byte.MinValue)
            {
                return Convert.ToInt16(byte.MinValue);
            }
            
            return base.UpdateValue(source);
        }
        
        protected override void AreEqual(short source, byte destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortShortMapperDifferentType : MapperDifferentType<short, sbyte>
    {
        protected override short UpdateValue(short source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToInt16(sbyte.MaxValue);
            }
            
            if (source < sbyte.MinValue)
            {
                return Convert.ToInt16(sbyte.MinValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(short source, sbyte destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortUShortMapperDifferentType : MapperDifferentType<short, ushort>
    {
        protected override short UpdateValue(short source) 
            => Math.Abs(source);
        
        protected override void AreEqual(short source, ushort destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortIntMapperDifferentType : MapperDifferentType<short, int>
    {
        protected override void AreEqual(short source, int destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortUIntMapperDifferentType : MapperDifferentType<short, uint>
    {
        protected override short UpdateValue(short source) 
            => Math.Abs(source);
        
        protected override void AreEqual(short source, uint destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortLongMapperDifferentType : MapperDifferentType<short, long>
    {
        protected override void AreEqual(short source, long destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortULongMapperDifferentType : MapperDifferentType<short, ulong>
    {
        protected override short UpdateValue(short source) 
            => Math.Abs(source);
        
        protected override void AreEqual(short source, ulong destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortFloatMapperDifferentType : MapperDifferentType<short, float>
    {
        protected override void AreEqual(short source, float destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortDoubletMapperDifferentType : MapperDifferentType<short, double>
    {
        protected override void AreEqual(short source, double destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
    
    public class ShortDecimalMapperDifferentType : MapperDifferentType<short, decimal>
    {
        protected override void AreEqual(short source, decimal destiny)
        {
            Convert.ToInt16(destiny).Should().Be(source);
        }
    }
}
