using System;
using FluentAssertions;

namespace CastForm.Integration.DifferentType.NonNullable.Structs.DateTime
{
    public class DateTimeMapperDifferentType : MapperDifferentType<System.DateTime, string>
    {
        protected override void AreEqual(System.DateTime source, string destiny)
        {
            Convert.ToDateTime(destiny).Should().Be(source);
        }
    }
}
