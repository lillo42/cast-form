using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.Bytes
{
    public class ByteStringMapperDifferentType : MapperDifferentType<byte, string>
    {
        protected override void AreEqual(byte source, string destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteCharMapperDifferentType : MapperDifferentType<byte, char>
    {
        protected override void AreEqual(byte source, char destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteSByteMapperDifferentType : MapperDifferentType<byte, sbyte>
    {
        protected override byte UpdateValue(byte source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToByte(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(byte source, sbyte destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteShortMapperDifferentType : MapperDifferentType<byte, short>
    {
        protected override void AreEqual(byte source, short destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteUShortMapperDifferentType : MapperDifferentType<byte, ushort>
    {
        protected override void AreEqual(byte source, ushort destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteIntMapperDifferentType : MapperDifferentType<byte, int>
    {
        protected override void AreEqual(byte source, int destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteUIntMapperDifferentType : MapperDifferentType<byte, uint>
    {
        protected override void AreEqual(byte source, uint destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteLongMapperDifferentType : MapperDifferentType<byte, long>
    {
        protected override void AreEqual(byte source, long destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteULongMapperDifferentType : MapperDifferentType<byte, ulong>
    {
        protected override void AreEqual(byte source, ulong destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteFloatMapperDifferentType : MapperDifferentType<byte, float>
    {
        protected override void AreEqual(byte source, float destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteDoubletMapperDifferentType : MapperDifferentType<byte, double>
    {
        protected override void AreEqual(byte source, double destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
    
    public class ByteDecimalMapperDifferentType : MapperDifferentType<byte, decimal>
    {
        protected override void AreEqual(byte source, decimal destiny)
        {
            Convert.ToByte(destiny).Should().Be(source);
        }
    }
}
