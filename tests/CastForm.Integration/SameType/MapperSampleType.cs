using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Integration.SameType
{
    public abstract class MapperSampleType<T>
    {
        private readonly Fixture _fixture;

        protected MapperSampleType()
        {
            _fixture = new Fixture();
        }

        public class Source
        {
            public T Value { get; set; }
        }
        
        public class Destiny
        {
            public T Value { get; set; }
        }
        
        public class DifferentSource
        {
            public T Id { get; set; }
        }

        public class DifferentDestiny
        {
            public T Value { get; set; }
        }

        [Fact]
        public void SamePropertyNameWithNoSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<Source, Destiny>()
                .Build();

            var source = _fixture.Create<Source>();

            var destiny = mapper.Map<Destiny>(source);

            destiny.Should().NotBeNull();
            destiny.Value.Should().Be(source.Value);
        }
        
        [Fact]
        public void SamePropertyNameWithSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<Source, Destiny>()
                .Build();

            var source = _fixture.Create<Source>();

            var destiny = mapper.Map<Source, Destiny>(source);

            destiny.Should().NotBeNull();
            destiny.Value.Should().Be(source.Value);
        }
        
        [Fact]
        public void DifferentPropertyNameWithNoSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<DifferentSource, DifferentDestiny>()
                    .For(x => x.Value, x => x.Id)
                .Build();

            var source = _fixture.Create<DifferentSource>();
            
            var destiny = mapper.Map<DifferentDestiny>(source);

            destiny.Should().NotBeNull();
            destiny.Value.Should().Be(source.Id);
        }
        
        [Fact]
        public void DifferentPropertyNameWithSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<DifferentSource, DifferentDestiny>()
                    .For(x => x.Value, x => x.Id)
                .Build();

            var source = _fixture.Create<DifferentSource>();
            
            var destiny = mapper.Map<DifferentSource, DifferentDestiny>(source);

            destiny.Should().NotBeNull();
            destiny.Value.Should().Be(source.Id);
        }
    }
}
