using System.Reflection;
using CastForm.Generator;
using CastForm.Rules;
using CastForm.Rules.Factories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CastForm.Test.Rules.Factories
{
    public class SameTypeRuleFactoryTest
    {
        private readonly SameTypeRuleFactory _factory;

        public SameTypeRuleFactoryTest()
        {
            _factory = new SameTypeRuleFactory();
        }
        
        [Fact]
        public void CanCreateRule_Should_BeTrue_When_SameType()
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(typeof(Foo));
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(typeof(Foo));

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeTrue();
        }

        [Fact]
        public void CanCreateRule_Should_BeFalse_When_IsDifferentType()
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(typeof(Foo));
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(typeof(Bar));

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeFalse();
        }
        
        
        [Fact]
        public void CreateRule()
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(typeof(Foo));
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(typeof(Foo));

            var rule =  _factory.CreateRule(propertySource, propertyDestiny,
                Substitute.For<IHashCodeFactoryGenerator>());

            rule.Should().NotBeNull();
            rule.Should().BeAssignableTo<SameTypeRule>();
            
            rule.SourceProperty.Should().NotBeNull();
            rule.SourceProperty.Should().BeSameAs(propertySource);
            
            rule.DestinyProperty.Should().NotBeNull();
            rule.DestinyProperty.Should().BeSameAs(propertyDestiny);
        }
        
        public class Foo
        {
            
        }
        
        public class Bar
        {
            
        }
    }
}
