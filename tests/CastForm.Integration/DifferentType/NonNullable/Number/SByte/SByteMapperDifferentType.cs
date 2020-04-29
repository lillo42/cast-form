using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.SByte
{
    public class SByteStringMapperDifferentType : MapperDifferentType<sbyte, string>
    {
        protected override void AreEqual(sbyte source, string destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteCharMapperDifferentType : MapperDifferentType<sbyte, char>
    {
        protected override sbyte UpdateValue(sbyte source) 
            => Math.Abs(source);

        protected override void AreEqual(sbyte source, char destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteByteMapperDifferentType : MapperDifferentType<sbyte, byte>
    {
        
        protected override sbyte UpdateValue(sbyte source) 
            => Math.Abs(source);
        
        protected override void AreEqual(sbyte source, byte destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteShortMapperDifferentType : MapperDifferentType<sbyte, short>
    {
        protected override void AreEqual(sbyte source, short destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteUShortMapperDifferentType : MapperDifferentType<sbyte, ushort>
    {
        protected override sbyte UpdateValue(sbyte source) 
            => Math.Abs(source);
        
        protected override void AreEqual(sbyte source, ushort destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteIntMapperDifferentType : MapperDifferentType<sbyte, int>
    {
        protected override void AreEqual(sbyte source, int destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteUIntMapperDifferentType : MapperDifferentType<sbyte, uint>
    {
        protected override sbyte UpdateValue(sbyte source) 
            => Math.Abs(source);
        
        protected override void AreEqual(sbyte source, uint destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteLongMapperDifferentType : MapperDifferentType<sbyte, long>
    {
        protected override void AreEqual(sbyte source, long destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteULongMapperDifferentType : MapperDifferentType<sbyte, ulong>
    {
        protected override sbyte UpdateValue(sbyte source) 
            => Math.Abs(source);
        
        protected override void AreEqual(sbyte source, ulong destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteFloatMapperDifferentType : MapperDifferentType<sbyte, float>
    {
        protected override void AreEqual(sbyte source, float destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteDoubletMapperDifferentType : MapperDifferentType<sbyte, double>
    {
        protected override void AreEqual(sbyte source, double destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
    
    public class SByteDecimalMapperDifferentType : MapperDifferentType<sbyte, decimal>
    {
        protected override void AreEqual(sbyte source, decimal destiny)
        {
            Convert.ToSByte(destiny).Should().Be(source);
        }
    }
}
