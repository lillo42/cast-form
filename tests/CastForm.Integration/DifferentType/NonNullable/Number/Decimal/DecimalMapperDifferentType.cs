using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.Decimal
{
    public class DecimalStringMapperDifferentType : MapperDifferentType<decimal, string>
    {
        protected override void AreEqual(decimal source, string destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }

    public class DecimalByteMapperDifferentType : MapperDifferentType<decimal, byte>
    {
        protected override void AreEqual(decimal source, byte destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    
    public class DecimalSByteMapperDifferentType : MapperDifferentType<decimal, sbyte>
    {
        protected override decimal UpdateValue(decimal source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToDecimal(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(decimal source, sbyte destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    
    public class DecimalShortMapperDifferentType : MapperDifferentType<decimal, short>
    {
        protected override void AreEqual(decimal source, short destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    
    public class DecimalUShortMapperDifferentType : MapperDifferentType<decimal, ushort>
    {
        protected override void AreEqual(decimal source, ushort destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    
    public class DecimalIntMapperDifferentType : MapperDifferentType<decimal, int>
    {
        protected override void AreEqual(decimal source, int destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    
    public class DecimalUIntMapperDifferentType : MapperDifferentType<decimal, uint>
    {
        protected override void AreEqual(decimal source, uint destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    
    public class DecimalLongMapperDifferentType : MapperDifferentType<decimal, long>
    {
        protected override void AreEqual(decimal source, long destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }

    public class DecimalULongMapperDifferentType : MapperDifferentType<decimal, ulong>
    {
        protected override void AreEqual(decimal source, ulong destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    public class DecimalFloatMapperDifferentType : MapperDifferentType<decimal, float>
    {
        protected override void AreEqual(decimal source, float destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
    
    public class DecimalDoubletMapperDifferentType : MapperDifferentType<decimal, double>
    {
        protected override void AreEqual(decimal source, double destiny)
        {
            Convert.ToDecimal(destiny).Should().Be(source);
        }
    }
}
