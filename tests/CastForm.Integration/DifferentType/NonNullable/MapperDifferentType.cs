using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Integration.DifferentType.NonNullable
{
    public abstract class MapperDifferentType<TSource, TDestiny>
    {
        protected Fixture Fixture { get; }

        protected MapperDifferentType()
        {
            Fixture = new Fixture();
        }

        public class Source
        {
            public TSource Value { get; set; }
        }
        
        public class Destiny
        {
            public TDestiny Value { get; set; }
        }
        
        public class DifferentSource
        {
            public TSource Id { get; set; }
        }

        public class DifferentDestiny
        {
            public TDestiny Value { get; set; }
        }

        protected abstract void AreEqual(TSource source, TDestiny destiny);

        protected virtual TSource UpdateValue(TSource source) 
            => source;

        [Fact]
        public void SamePropertyNameWithNoSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<Source, Destiny>()
                .Build();

            var source = Fixture.Create<Source>();

            source.Value = UpdateValue(source.Value);

            var destiny = mapper.Map<Destiny>(source);

            destiny.Should().NotBeNull();
            
            AreEqual(source.Value, destiny.Value);
        }
        
        [Fact]
        public void SamePropertyNameWithSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<Source, Destiny>()
                .Build();

            var source = Fixture.Create<Source>();
            
            source.Value = UpdateValue(source.Value);

            var destiny = mapper.Map<Source, Destiny>(source);

            destiny.Should().NotBeNull();
            
            AreEqual(source.Value, destiny.Value);
        }
        
        [Fact]
        public void DifferentPropertyNameWithNoSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<DifferentSource, DifferentDestiny>()
                .For(x => x.Value, x => x.Id)
                .Build();

            var source = Fixture.Create<DifferentSource>();
            
            source.Id = UpdateValue(source.Id);
            
            var destiny = mapper.Map<DifferentDestiny>(source);

            destiny.Should().NotBeNull();
            AreEqual(source.Id, destiny.Value);
        }
        
        [Fact]
        public void DifferentPropertyNameWithSetSourceType()
        {
            var mapper = new MapperBuilder()
                .AddMapper<DifferentSource, DifferentDestiny>()
                .For(x => x.Value, x => x.Id)
                .Build();

            var source = Fixture.Create<DifferentSource>();
            
            source.Id = UpdateValue(source.Id);
            
            var destiny = mapper.Map<DifferentSource, DifferentDestiny>(source);

            destiny.Should().NotBeNull();
            AreEqual(source.Id, destiny.Value);
        }

    }
}
