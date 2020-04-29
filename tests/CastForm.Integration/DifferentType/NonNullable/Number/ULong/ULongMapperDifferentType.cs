using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Number.ULong
{
    public class ULongStringMapperDifferentType : MapperDifferentType<ulong, string>
    {
        protected override void AreEqual(ulong source, string destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongCharMapperDifferentType : MapperDifferentType<ulong, char>
    {
        protected override void AreEqual(ulong source, char destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }

    public class ULongByteMapperDifferentType : MapperDifferentType<ulong, byte>
    {
        protected override void AreEqual(ulong source, byte destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongSByteMapperDifferentType : MapperDifferentType<ulong, sbyte>
    {
        protected override ulong UpdateValue(ulong source)
        {
            if (source > Convert.ToUInt64(sbyte.MaxValue))
            {
                return Convert.ToUInt64(sbyte.MaxValue);
            }
            
            return base.UpdateValue(source);
        }

        protected override void AreEqual(ulong source, sbyte destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongShortMapperDifferentType : MapperDifferentType<ulong, short>
    {
        protected override void AreEqual(ulong source, short destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongUShortMapperDifferentType : MapperDifferentType<ulong, ushort>
    {
        protected override void AreEqual(ulong source, ushort destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongIntMapperDifferentType : MapperDifferentType<ulong, int>
    {
        protected override void AreEqual(ulong source, int destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongUIntMapperDifferentType : MapperDifferentType<ulong, uint>
    {
        protected override void AreEqual(ulong source, uint destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongLongMapperDifferentType : MapperDifferentType<ulong, long>
    {
        protected override void AreEqual(ulong source, long destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongFloatMapperDifferentType : MapperDifferentType<ulong, float>
    {
        protected override void AreEqual(ulong source, float destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongDoubletMapperDifferentType : MapperDifferentType<ulong, double>
    {
        protected override void AreEqual(ulong source, double destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
    
    public class ULongDecimalMapperDifferentType : MapperDifferentType<ulong, decimal>
    {
        protected override void AreEqual(ulong source, decimal destiny)
        {
            Convert.ToUInt64(destiny).Should().Be(source);
        }
    }
}
