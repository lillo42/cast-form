using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Boolean
{
    public class BooleanStringMapperDifferentType : MapperDifferentType<bool, string>
    {
        protected override void AreEqual(bool source, string destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanByteMapperDifferentType : MapperDifferentType<bool, byte>
    {
        protected override void AreEqual(bool source, byte destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanSByteMapperDifferentType : MapperDifferentType<bool, sbyte>
    {
        protected override void AreEqual(bool source, sbyte destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanShortMapperDifferentType : MapperDifferentType<bool, short>
    {
        protected override void AreEqual(bool source, short destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanUShortMapperDifferentType : MapperDifferentType<bool, ushort>
    {
        protected override void AreEqual(bool source, ushort destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanIntMapperDifferentType : MapperDifferentType<bool, int>
    {
        protected override void AreEqual(bool source, int destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanUIntMapperDifferentType : MapperDifferentType<bool, uint>
    {
        protected override void AreEqual(bool source, uint destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanLongMapperDifferentType : MapperDifferentType<bool, long>
    {
        protected override void AreEqual(bool source, long destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanULongMapperDifferentType : MapperDifferentType<bool, ulong>
    {
        protected override void AreEqual(bool source, ulong destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanFloatMapperDifferentType : MapperDifferentType<bool, float>
    {
        protected override void AreEqual(bool source, float destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanDoubleMapperDifferentType : MapperDifferentType<bool, double>
    {
        protected override void AreEqual(bool source, double destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
    
    public class BooleanDecimalMapperDifferentType : MapperDifferentType<bool, decimal>
    {
        protected override void AreEqual(bool source, decimal destiny)
        {
            Convert.ToBoolean(destiny).Should().Be(source);
        }
    }
}
