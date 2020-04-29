using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.UShort
{
    public class UShortStringMapperDifferentType : MapperDifferentType<ushort, string>
    {
        protected override void AreEqual(ushort source, string destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortCharMapperDifferentType : MapperDifferentType<ushort, char>
    {
        protected override void AreEqual(ushort source, char destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortByteMapperDifferentType : MapperDifferentType<ushort, byte>
    {
        protected override ushort UpdateValue(ushort source)
        {
            if (source > byte.MaxValue)
            {
                return Convert.ToUInt16(byte.MaxValue);
            }

            return base.UpdateValue(source);
        }

        protected override void AreEqual(ushort source, byte destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortShortMapperDifferentType : MapperDifferentType<ushort, short>
    {
        protected override void AreEqual(ushort source, short destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortSByteMapperDifferentType : MapperDifferentType<ushort, sbyte>
    {
        protected override ushort UpdateValue(ushort source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToUInt16(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(ushort source, sbyte destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortIntMapperDifferentType : MapperDifferentType<ushort, int>
    {
        protected override void AreEqual(ushort source, int destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortUIntMapperDifferentType : MapperDifferentType<ushort, uint>
    {
        protected override void AreEqual(ushort source, uint destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortLongMapperDifferentType : MapperDifferentType<ushort, long>
    {
        protected override void AreEqual(ushort source, long destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortULongMapperDifferentType : MapperDifferentType<ushort, ulong>
    {
        protected override void AreEqual(ushort source, ulong destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortFloatMapperDifferentType : MapperDifferentType<ushort, float>
    {
        protected override void AreEqual(ushort source, float destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortDoubletMapperDifferentType : MapperDifferentType<ushort, double>
    {
        protected override void AreEqual(ushort source, double destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
    
    public class UShortDecimalMapperDifferentType : MapperDifferentType<ushort, decimal>
    {
        protected override void AreEqual(ushort source, decimal destiny)
        {
            Convert.ToUInt16(destiny).Should().Be(source);
        }
    }
}
