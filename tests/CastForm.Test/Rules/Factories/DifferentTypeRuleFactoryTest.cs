using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CastForm.Generator;
using CastForm.Rules;
using CastForm.Rules.Factories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CastForm.Test.Rules.Factories
{
    public class DifferentTypeRuleFactoryTest
    {
        private readonly DifferentTypeRuleFactory _factory;

        public DifferentTypeRuleFactoryTest()
        {
            _factory = new DifferentTypeRuleFactory();
        }
        
        [Theory]
        [ClassData(typeof(DifferentType))]
        public void CanCreateRule_Should_BeFalse_When_Have2DifferentNetType(Type source, Type destiny)
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(source);
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(destiny);

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeFalse();
        }
        
        [Fact]
        public void CanCreateRule_Should_BeFalse_When_HaveSameNetType()
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(typeof(Foo));
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(typeof(Foo));

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeFalse();
        }
        
        [Theory]
        [InlineData(typeof(Bar), typeof(Foo))]
        [InlineData(typeof(Foo), typeof(Bar))]
        public void CanCreateRule_Should_BeTrue_When_HaveIsNotNetType(Type source, Type destiny)
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(source);
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(destiny);

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeTrue();
        }

        [Fact]
        public void CreateRule()
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(typeof(Foo));
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(typeof(Bar));

            var rule =  _factory.CreateRule(propertySource, propertyDestiny,
                Substitute.For<IHashCodeFactoryGenerator>());

            rule.Should().NotBeNull();
            rule.Should().BeAssignableTo<DifferentTypeRule>();
            
            rule.SourceProperty.Should().NotBeNull();
            rule.SourceProperty.Should().BeSameAs(propertySource);
            
            rule.DestinyProperty.Should().NotBeNull();
            rule.DestinyProperty.Should().BeSameAs(propertyDestiny);
        }

        #region Collection 

        public class DifferentType : IEnumerable<object[]>
        {
            private static readonly Type[] s_netType = new[]
            {
                typeof(char),
                typeof(string),
                typeof(bool),
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal)
            };
            public IEnumerator<object[]> GetEnumerator()
            {
                for (var i = 0; i < s_netType.Length; i++)
                {
                    yield return new object[] {s_netType[i], typeof(Foo) };
                }
                
                for (var j = 0; j < s_netType.Length; j++)
                {
                    yield return new object[] {typeof(Foo), s_netType[j] };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        

        public class Foo
        {
            
        }
        
        public class Bar
        {
            
        }
        
        #endregion
    }
}
