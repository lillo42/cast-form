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
    public class NullableRuleForNetTypeOneIsNullableFactoryTest
    {
        private readonly NullableRuleForNetTypeOneIsNullableFactory _factory;

        public NullableRuleForNetTypeOneIsNullableFactoryTest()
        {
            _factory = new NullableRuleForNetTypeOneIsNullableFactory();
        }
        
        [Theory]
        [ClassData(typeof(DifferentType))]
        public void CanCreateRule_Should_BeTrue_When_Have2DifferentNetType(Type source, Type destiny)
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(source);
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(destiny);

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeTrue();
        }
        
        [Theory]
        [ClassData(typeof(SameType))]
        public void CanCreateRule_Should_BeFalse_When_HaveSameNetType(Type source, Type destiny)
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(source);
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(destiny);

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeFalse();
        }
        
        [Theory]
        [InlineData(typeof(int?), typeof(Foo))]
        [InlineData(typeof(Foo), typeof(int?))]
        public void CanCreateRule_Should_BeFalse_When_HaveIsNotNetType(Type source, Type destiny)
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(source);
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(destiny);

            _factory.CanCreateRule(propertySource, propertyDestiny)
                .Should().BeFalse();
        }

        
        [Fact]
        public void CreateRule_When_IsSameType()
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(typeof(int?));
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(typeof(int));

            var rule =  _factory.CreateRule(propertySource, propertyDestiny,
                Substitute.For<IHashCodeFactoryGenerator>());

            rule.Should().NotBeNull();
            rule.Should().BeAssignableTo<NullableRuleForSameTypeWhenOneIsNullable>();
            
            rule.SourceProperty.Should().NotBeNull();
            rule.SourceProperty.Should().BeSameAs(propertySource);
            
            rule.DestinyProperty.Should().NotBeNull();
            rule.DestinyProperty.Should().BeSameAs(propertyDestiny);
        }
        
        [Fact]
        public void CreateRule_When_IsDifferntType()
        {
            var propertySource = Substitute.For<PropertyInfo>();
            propertySource.PropertyType.Returns(typeof(int?));
            
            var propertyDestiny = Substitute.For<PropertyInfo>();
            propertyDestiny.PropertyType.Returns(typeof(char));

            var rule =  _factory.CreateRule(propertySource, propertyDestiny,
                Substitute.For<IHashCodeFactoryGenerator>());

            rule.Should().NotBeNull();
            rule.Should().BeAssignableTo<NullableRuleForDifferentTypeWhenOneIsNullable>();
            
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
                typeof(DateTime),
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
                    for (var j = 0; j < s_netType.Length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        
                        yield return new object[] {typeof(Nullable<>).MakeGenericType(s_netType[i]), s_netType[j] };
                    }
                }
                
                for (var i = 0; i < s_netType.Length; i++)
                {
                    for (var j = 0; j < s_netType.Length; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        
                        yield return new object[] {s_netType[i], typeof(Nullable<>).MakeGenericType(s_netType[j])};
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        
        public class SameType : IEnumerable<object[]>
        {
            private static readonly Type[] s_netType = new[]
            {
                typeof(char?),
                typeof(DateTime?),
                typeof(bool?),
                typeof(byte?),
                typeof(sbyte?),
                typeof(short?),
                typeof(ushort?),
                typeof(int?),
                typeof(uint?),
                typeof(long?),
                typeof(ulong?),
                typeof(float?),
                typeof(double?),
                typeof(decimal?)
            };
            public IEnumerator<object[]> GetEnumerator()
            {
                foreach (var type in s_netType)
                {
                    yield return new object[] {type, type };
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class Foo
        {
            
        }
        
        #endregion
    }
}
