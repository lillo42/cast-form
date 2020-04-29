using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Structs.TimeSpan
{
    public class TimeSpanMapperDifferentType : MapperDifferentType<System.TimeSpan, string>
    {
        protected override void AreEqual(System.TimeSpan source, string destiny)
        {
            System.TimeSpan.Parse(destiny).Should().Be(source);
        }
    }
}
