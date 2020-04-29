using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Structs.DateTimeOffset
{
    public class DateTimeOffsetMapperDifferentType : MapperDifferentType<System.DateTimeOffset, string>
    {
        protected override void AreEqual(System.DateTimeOffset source, string destiny)
        {
            System.DateTimeOffset.Parse(destiny).Should().Be(source);
        }
    }
}
