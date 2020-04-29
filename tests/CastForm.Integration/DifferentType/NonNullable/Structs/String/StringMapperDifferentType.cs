using System;
using System.Globalization;
using AutoFixture;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Structs.String
{
    public class StringCharMapperDifferentType : MapperDifferentType<string, char>
    {
        protected override string UpdateValue(string source)
        {
            return source[0].ToString();
        }

        protected override void AreEqual(string source, char destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringDateTimeMapperDifferentType : MapperDifferentType<string, System.DateTime>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<System.DateTime>().ToString(CultureInfo.InvariantCulture);
        }

        protected override void AreEqual(string source, System.DateTime destiny)
        {
            Convert.ToString(destiny, CultureInfo.InvariantCulture).Should().Be(source);
        }
    }
    
    public class StringByteMapperDifferentType : MapperDifferentType<string, byte>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<byte>().ToString();
        }
        
        protected override void AreEqual(string source, byte destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringSByteMapperDifferentType : MapperDifferentType<string, sbyte>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<sbyte>().ToString();
        }
        
        protected override void AreEqual(string source, sbyte destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringShortMapperDifferentType : MapperDifferentType<string, short>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<short>().ToString();
        }
        
        protected override void AreEqual(string source, short destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringUShortMapperDifferentType : MapperDifferentType<string, ushort>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<ushort>().ToString();
        }
        
        protected override void AreEqual(string source, ushort destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringIntMapperDifferentType : MapperDifferentType<string, int>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<int>().ToString();
        }
        
        protected override void AreEqual(string source, int destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringUIntMapperDifferentType : MapperDifferentType<string, uint>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<uint>().ToString();
        }
        
        protected override void AreEqual(string source, uint destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringLongMapperDifferentType : MapperDifferentType<string, long>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<long>().ToString();
        }
    
        protected override void AreEqual(string source, long destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringULongMapperDifferentType : MapperDifferentType<string, ulong>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<ulong>().ToString();
        }
        
        protected override void AreEqual(string source, ulong destiny)
        {
            Convert.ToString(destiny).Should().Be(source);
        }
    }
    
    public class StringFloatMapperDifferentType : MapperDifferentType<string, float>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<float>().ToString(CultureInfo.InvariantCulture);
        }
        
        protected override void AreEqual(string source, float destiny)
        {
            Convert.ToString(destiny, CultureInfo.InvariantCulture).Should().Be(source);
        }
    }
    
    public class StringDoubleMapperDifferentType : MapperDifferentType<string, double>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<double>().ToString(CultureInfo.InvariantCulture);
        }
        
        protected override void AreEqual(string source, double destiny)
        {
            Convert.ToString(destiny, CultureInfo.InvariantCulture).Should().Be(source);
        }
    }
    
    public class StringDecimalMapperDifferentType : MapperDifferentType<string, decimal>
    {
        protected override string UpdateValue(string source)
        {
            return Fixture.Create<decimal>().ToString(CultureInfo.InvariantCulture);
        }
        
        protected override void AreEqual(string source, decimal destiny)
        {
            Convert.ToString(destiny, CultureInfo.InvariantCulture).Should().Be(source);
        }
    }
}
