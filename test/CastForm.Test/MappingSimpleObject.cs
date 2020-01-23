﻿using System;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class MappingSimpleObject
    {
        private readonly Fixture _fixture;

        public MappingSimpleObject()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void SamePropertyName()
        {
            var builder = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>();

            var mapper = builder.Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();
            b.Should().BeEquivalentTo(a);
        }


        [Fact]
        public void Ignore()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Ignore(x => x.Id)
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleB>(a);
            b.Should().NotBeNull();
            b.Text.Should().Be(a.Text);
            b.Id.Should().NotBe(a.Id);
            b.Id.Should().Be(0);
        }


        [Fact]
        public void For()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleC>()
                    .For(x => x.Id, x => x.Id2)
                .Build();

            var a = _fixture.Create<SimpleA>();
            var b = mapper.Map<SimpleC>(a);
            b.Should().NotBeNull();
            b.Text.Should().Be(a.Text);
            b.Id2.Should().Be(a.Id);
        }

        public class SimpleA
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        public class SimpleB
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }


        public class SimpleC
        {
            public int Id2 { get; set; }
            public string Text { get; set; }
        }
    }
}
