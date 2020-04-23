using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CastForm.Generator;
using CastForm.RegisterServiceCollection;
using CastForm.Rules;
using CastForm.Rules.Factories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace CastForm.Test
{
    public class MapperBuilderTest
    {
        private readonly IHashCodeFactoryGenerator _hashCodeFactory;
        private readonly IServiceCollection _service;
        
        private readonly MapperBuilder _mapper;


        public MapperBuilderTest()
        {
            _service = new ServiceCollection();
            _hashCodeFactory = Substitute.For<IHashCodeFactoryGenerator>();
            _mapper = new MapperBuilder(_service, _hashCodeFactory);
        }


        [Fact]
        public void AddMapperGeneric()
        {
            var result = _mapper.AddMapper<int, string>();
            result.Should().NotBeNull();
            result.Should().NotBe(_mapper);
        }
        
        
        [Fact]
        public void AddMapperIMapperBuilder()
        {
            var mapper = Substitute.For<IMapperBuilder>();
            var result = _mapper.AddMapper(mapper);
            result.Should().NotBeNull();
            result.Should().Be(_mapper);
        }
        
        
        [Fact]
        public void Build()
        {
            var mapper1 = Substitute.For<IMapperBuilder>();
            var mapper2 = Substitute.For<IMapperBuilder>();
            
            _mapper.AddMapper(mapper1);
            _mapper.AddMapper(mapper2);


            var fakeMapper = Substitute.For<IMapper>();
            _service.AddSingleton(fakeMapper);
            
            var map = _mapper.Build();
            
            map.Should().Be(fakeMapper);
        }
        
        [Fact]
        public void AddRegister()
        {
            var register = Substitute.For<IRegisterServiceCollectionType>();
            
            _mapper.AddRegisterServiceCollectionType(register)
                .Should().Be(_mapper);

            Registers.RegisterTypes.Should().Contain(register);
        }
        
        [Fact]
        public void AddRuleFactory()
        {
            var register = Substitute.For<IRuleFactory>();
            
            _mapper.AddRuleFactory(register)
                .Should().Be(_mapper);

            Registers.RuleFactories.Should().Contain(register);
        }
        
        
        [Fact]
        public void Build_With_Roles()
        {
            var mapper1 = Substitute.For<IMapperBuilder>();
            mapper1.Destiny.Returns(Substitute.For<Type>());
            mapper1.Source.Returns(Substitute.For<Type>());
            
            var rule1 = Substitute.For<IRuleMapper>();
            
            rule1.DestinyProperty.Returns(Substitute.For<PropertyInfo>());
            rule1.SourceProperty.Returns(Substitute.For<PropertyInfo>());
            
            var rule2 = Substitute.For<IRuleMapper>();
            
            rule2.DestinyProperty.Returns(Substitute.For<PropertyInfo>());
            rule2.SourceProperty.Returns(Substitute.For<PropertyInfo>());

            mapper1.Rules.Returns(new List<IRuleMapper> {rule1, rule2});
            
            _mapper.AddMapper(mapper1);


            var fakeMapper = Substitute.For<IMapper>();
            _service.AddSingleton(fakeMapper);
            
            var map = _mapper.Build();
            
            map.Should().Be(fakeMapper);
        }
        
        
        [Fact]
        public void Register()
        {
            var fakeMapper = Substitute.For<IMapper>();
            _service.AddSingleton(fakeMapper);
            
            _mapper.Register(Enumerable.Empty<MapperProperty>());
        }
    }
}
