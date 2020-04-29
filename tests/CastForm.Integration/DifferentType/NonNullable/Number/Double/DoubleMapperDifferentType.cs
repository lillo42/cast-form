using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.Double
{
    public class DoubleStringMapperDifferentType : MapperDifferentType<double, string>
    {
        protected override void AreEqual(double source, string destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }

    public class DoubleByteMapperDifferentType : MapperDifferentType<double, byte>
    {
        protected override void AreEqual(double source, byte destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    
    public class DoubleSByteMapperDifferentType : MapperDifferentType<double, sbyte>
    {
        protected override double UpdateValue(double source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToDouble(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(double source, sbyte destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    
    public class DoubleShortMapperDifferentType : MapperDifferentType<double, short>
    {
        protected override void AreEqual(double source, short destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    
    public class DoubleUShortMapperDifferentType : MapperDifferentType<double, ushort>
    {
        protected override void AreEqual(double source, ushort destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    
    public class DoubleIntMapperDifferentType : MapperDifferentType<double, int>
    {
        protected override void AreEqual(double source, int destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    
    public class DoubleUIntMapperDifferentType : MapperDifferentType<double, uint>
    {
        protected override void AreEqual(double source, uint destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    
    public class DoubleLongMapperDifferentType : MapperDifferentType<double, long>
    {
        protected override void AreEqual(double source, long destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }

    public class DoubleULongMapperDifferentType : MapperDifferentType<double, ulong>
    {
        protected override void AreEqual(double source, ulong destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    public class DoubleFloatMapperDifferentType : MapperDifferentType<double, float>
    {
        protected override void AreEqual(double source, float destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
    
    public class DoubleDecimalMapperDifferentType : MapperDifferentType<double, decimal>
    {
        protected override void AreEqual(double source, decimal destiny)
        {
            Convert.ToDouble(destiny).Should().Be(source);
        }
    }
}
