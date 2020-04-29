using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Structs.Char
{
    public class CharStringMapperDifferentType : MapperDifferentType<char, string>
    {
        protected override void AreEqual(char source, string destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharByteMapperDifferentType : MapperDifferentType<char, byte>
    {
        protected override void AreEqual(char source, byte destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharSByteMapperDifferentType : MapperDifferentType<char, sbyte>
    {
        protected override void AreEqual(char source, sbyte destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharShortMapperDifferentType : MapperDifferentType<char, short>
    {
        protected override void AreEqual(char source, short destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharUShortMapperDifferentType : MapperDifferentType<char, ushort>
    {
        protected override void AreEqual(char source, ushort destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharIntMapperDifferentType : MapperDifferentType<char, int>
    {
        protected override void AreEqual(char source, int destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharUIntMapperDifferentType : MapperDifferentType<char, uint>
    {
        protected override void AreEqual(char source, uint destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharLongMapperDifferentType : MapperDifferentType<char, long>
    {
        protected override void AreEqual(char source, long destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
    
    public class CharULongMapperDifferentType : MapperDifferentType<char, ulong>
    {
        protected override void AreEqual(char source, ulong destiny)
        {
            Convert.ToChar(destiny).Should().Be(source);
        }
    }
}
