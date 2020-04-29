using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.Int
{
    public class IntStringMapperDifferentType : MapperDifferentType<int, string>
    {
        protected override void AreEqual(int source, string destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntCharMapperDifferentType : MapperDifferentType<int, char>
    {
        protected override void AreEqual(int source, char destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }

    public class IntByteMapperDifferentType : MapperDifferentType<int, byte>
    {
        protected override void AreEqual(int source, byte destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntSByteMapperDifferentType : MapperDifferentType<int, sbyte>
    {
        protected override int UpdateValue(int source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToInt32(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(int source, sbyte destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntShortMapperDifferentType : MapperDifferentType<int, short>
    {
        protected override void AreEqual(int source, short destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntUShortMapperDifferentType : MapperDifferentType<int, ushort>
    {
        protected override void AreEqual(int source, ushort destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntUIntMapperDifferentType : MapperDifferentType<int, uint>
    {
        protected override void AreEqual(int source, uint destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntLongMapperDifferentType : MapperDifferentType<int, long>
    {
        protected override void AreEqual(int source, long destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntULongMapperDifferentType : MapperDifferentType<int, ulong>
    {
        protected override void AreEqual(int source, ulong destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntFloatMapperDifferentType : MapperDifferentType<int, float>
    {
        protected override void AreEqual(int source, float destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntDoubletMapperDifferentType : MapperDifferentType<int, double>
    {
        protected override void AreEqual(int source, double destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
    
    public class IntDecimalMapperDifferentType : MapperDifferentType<int, decimal>
    {
        protected override void AreEqual(int source, decimal destiny)
        {
            Convert.ToInt32(destiny).Should().Be(source);
        }
    }
}
