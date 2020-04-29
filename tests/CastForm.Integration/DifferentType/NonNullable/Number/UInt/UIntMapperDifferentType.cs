using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.UInt
{
    public class UIntStringMapperDifferentType : MapperDifferentType<uint, string>
    {
        protected override void AreEqual(uint source, string destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntCharMapperDifferentType : MapperDifferentType<uint, char>
    {
        protected override void AreEqual(uint source, char destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }

    public class UIntByteMapperDifferentType : MapperDifferentType<uint, byte>
    {
        protected override void AreEqual(uint source, byte destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntSByteMapperDifferentType : MapperDifferentType<uint, sbyte>
    {
        protected override uint UpdateValue(uint source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToUInt32(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(uint source, sbyte destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntShortMapperDifferentType : MapperDifferentType<uint, short>
    {
        protected override void AreEqual(uint source, short destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntUShortMapperDifferentType : MapperDifferentType<uint, ushort>
    {
        protected override void AreEqual(uint source, ushort destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntIntMapperDifferentType : MapperDifferentType<uint, int>
    {
        protected override void AreEqual(uint source, int destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntLongMapperDifferentType : MapperDifferentType<uint, long>
    {
        protected override void AreEqual(uint source, long destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntULongMapperDifferentType : MapperDifferentType<uint, ulong>
    {
        protected override void AreEqual(uint source, ulong destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntFloatMapperDifferentType : MapperDifferentType<uint, float>
    {
        protected override void AreEqual(uint source, float destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntDoubletMapperDifferentType : MapperDifferentType<uint, double>
    {
        protected override void AreEqual(uint source, double destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
    
    public class UIntDecimalMapperDifferentType : MapperDifferentType<uint, decimal>
    {
        protected override void AreEqual(uint source, decimal destiny)
        {
            Convert.ToUInt32(destiny).Should().Be(source);
        }
    }
}
