using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test.CircleReference
{
    public class Circle
    {
        private readonly Fixture _fixture;

        public Circle()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void WithOverrideHashCode()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Reverse()
                .Build();

            var a = new SimpleA
            {
                Id= _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            var b = new SimpleB
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            a.Simple = b;
            b.Simple = a;

            var newB = mapper.Map<SimpleB>(a);
            newB.Id.Should().Be(a.Id);
            newB.Text.Should().Be(a.Text);
            newB.IsEnable.Should().Be(a.IsEnable);
        }


        public class SimpleA
        {
            public SimpleB Simple { get;  set; }

            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsEnable { get; set; }

            public override int GetHashCode() 
                => HashCode.Combine(Id, Text, IsEnable);
        }

        public class SimpleB
        {
            public SimpleA Simple { get; set; }
            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsEnable { get; set; }

            public override int GetHashCode() 
                => HashCode.Combine(Id, Text, IsEnable);
        }
    }
}
