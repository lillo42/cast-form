using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Structs.Guid
{
    public class GuidMapperDifferentType : MapperDifferentType<System.Guid, string>
    {
        protected override void AreEqual(System.Guid source, string destiny)
        {
            System.Guid.Parse(destiny).Should().Be(source);
        }
    }
}
