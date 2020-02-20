using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using FluentAssertions;

namespace CastForm.Test.CircleReference
{
    public class Circle
    {
        private readonly Fixture _fixture;

        public Circle()
        {
            _fixture = new Fixture();
        }


        public void WithOverrideHashCode()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = _fixture.Create<SimpleB>();
            
            a.Simple = b;
            b.Simple = a;


            var newB = mapper.Map<SimpleB>(a);
            newB.Id.Should().Be(a.Id);
            newB.Text.Should().Be(a.Text);
            newB.IsEnable.Should().Be(a.IsEnable);
        }


        public class SimpleA
        {
            public SimpleB Simple { get; set; }

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
