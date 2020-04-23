using System;
using CastForm.Generator;
using CastForm.RegisterServiceCollection;
using CastForm.Rules.Factories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CastForm.Test
{
    public class MapperBuilderGenericTest
    {
        private readonly IServiceCollection _service;
        private readonly IHashCodeFactoryGenerator _hashCodeFactory;
        private readonly IMapperBuilder _parent;
        private readonly IMapperGenerator _generator;
        private readonly MapperBuilder<Foo, Bar> _builder;
        public MapperBuilderGenericTest()
        {
            _service = Substitute.For<IServiceCollection>();
            _hashCodeFactory = Substitute.For<IHashCodeFactoryGenerator>();
            _parent = Substitute.For<IMapperBuilder>();
            _generator = Substitute.For<IMapperGenerator>();
            
            _builder = new MapperBuilder<Foo, Bar>(_parent, _service, _hashCodeFactory, _generator);
        }

        [Fact]
        public void SourceAndDestiny()
        {
            _builder.Destiny.Should().Be(typeof(Bar));
            _builder.Source.Should().Be(typeof(Foo));
        }

        [Fact]
        public void Reverse()
        {
            var mapper = _builder.Reverse();

            mapper.Destiny.Should().Be(typeof(Foo));
            mapper.Source.Should().Be(typeof(Bar));
        }
        
        [Fact]
        public void Build()
        {
            var mapper = Substitute.For<IMapper>();

            _parent.Build().Returns(mapper);


            (_builder as IMapperBuilder).Build().Should().Be(mapper);

            _parent
                .Received(1)
                .Build();
        }

        [Fact]
        public void AddMapper()
        {
            var mapper = Substitute.For<IMapperBuilder>();

            ((IMapperBuilder)_builder).AddMapper(mapper)
                .Should().Be(_builder);
        }
        
        [Fact]
        public void AddRegister()
        {
            var register = Substitute.For<IRegisterServiceCollectionType>();
            
            _builder.AddRegisterServiceCollectionType(register)
                .Should().Be(_builder);
            
            Registers.RegisterTypes.Should().Contain(register);
        }

        [Fact]
        public void Ignore()
        {
            _builder.Ignore(x => x.Value);
        }
        
        [Fact]
        public void Ignore_Should_Throw()
        {
            Assert.Throws<NotSupportedException>(() => _builder.Ignore(x => x.GetId()));
        }

        
        [Fact]
        public void For()
        {
            _builder.For(x => x.Number, x => x.Id);
        }
        
        [Fact]
        public void For_Should_Throw()
        {
            Assert.Throws<NotSupportedException>(() => _builder.For(x => x.GetId(), x => x.Id));
            Assert.Throws<NotSupportedException>(() => _builder.For(x => x.Number, x => x.GetId()));
        }
        
        [Fact]
        public void AddRuleFactory()
        {
            var register = Substitute.For<IRuleFactory>();
            
            _builder.AddRuleFactory(register)
                .Should().Be(_builder);

            Registers.RuleFactories.Should().Contain(register);
        }
        
        public class Foo
        {
            public int Id { get; set; }
            public string Text { get; set; }
         
            
            public int GetId() => Id;
        }

        public class Bar
        {
            public int Number { get; set; }
            public string Value { get; set; }

            public int GetId() => Number;
        }
    }
}
