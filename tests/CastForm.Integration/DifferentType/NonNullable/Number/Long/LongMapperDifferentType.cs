using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.Long
{
    public class LongStringMapperDifferentType : MapperDifferentType<long, string>
    {
        protected override void AreEqual(long source, string destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongCharMapperDifferentType : MapperDifferentType<long, char>
    {
        protected override void AreEqual(long source, char destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }

    public class LongByteMapperDifferentType : MapperDifferentType<long, byte>
    {
        protected override void AreEqual(long source, byte destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongSByteMapperDifferentType : MapperDifferentType<long, sbyte>
    {
        protected override long UpdateValue(long source)
        {
            if (source > sbyte.MaxValue)
            {
                return Convert.ToInt64(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(long source, sbyte destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongShortMapperDifferentType : MapperDifferentType<long, short>
    {
        protected override void AreEqual(long source, short destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongUShortMapperDifferentType : MapperDifferentType<long, ushort>
    {
        protected override void AreEqual(long source, ushort destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongIntMapperDifferentType : MapperDifferentType<long, int>
    {
        protected override void AreEqual(long source, int destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongUIntMapperDifferentType : MapperDifferentType<long, uint>
    {
        protected override void AreEqual(long source, uint destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongULongMapperDifferentType : MapperDifferentType<long, ulong>
    {
        protected override void AreEqual(long source, ulong destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongFloatMapperDifferentType : MapperDifferentType<long, float>
    {
        protected override void AreEqual(long source, float destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongDoubletMapperDifferentType : MapperDifferentType<long, double>
    {
        protected override void AreEqual(long source, double destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
    
    public class LongDecimalMapperDifferentType : MapperDifferentType<long, decimal>
    {
        protected override void AreEqual(long source, decimal destiny)
        {
            Convert.ToInt64(destiny).Should().Be(source);
        }
    }
}
