using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.Float
{
    public class FloatStringMapperDifferentType : MapperDifferentType<float, string>
    {
        protected override void AreEqual(float source, string destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }

    public class FloatByteMapperDifferentType : MapperDifferentType<float, byte>
    {
        protected override void AreEqual(float source, byte destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatSByteMapperDifferentType : MapperDifferentType<float, sbyte>
    {
        protected override float UpdateValue(float source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToSingle(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(float source, sbyte destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatShortMapperDifferentType : MapperDifferentType<float, short>
    {
        protected override void AreEqual(float source, short destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatUShortMapperDifferentType : MapperDifferentType<float, ushort>
    {
        protected override void AreEqual(float source, ushort destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatIntMapperDifferentType : MapperDifferentType<float, int>
    {
        protected override void AreEqual(float source, int destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatUIntMapperDifferentType : MapperDifferentType<float, uint>
    {
        protected override void AreEqual(float source, uint destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatLongMapperDifferentType : MapperDifferentType<float, long>
    {
        protected override void AreEqual(float source, long destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }

    public class FloatULongMapperDifferentType : MapperDifferentType<float, ulong>
    {
        protected override void AreEqual(float source, ulong destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    public class FloatFloatMapperDifferentType : MapperDifferentType<float, float>
    {
        protected override void AreEqual(float source, float destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatDoubletMapperDifferentType : MapperDifferentType<float, double>
    {
        protected override void AreEqual(float source, double destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
    
    public class FloatDecimalMapperDifferentType : MapperDifferentType<float, decimal>
    {
        protected override void AreEqual(float source, decimal destiny)
        {
            Convert.ToSingle(destiny).Should().Be(source);
        }
    }
}
