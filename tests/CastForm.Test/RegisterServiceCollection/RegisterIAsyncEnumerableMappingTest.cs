using System.Collections.Generic;
using CastForm.Collection;
using CastForm.RegisterServiceCollection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CastForm.Test.RegisterServiceCollection
{
    public class RegisterIAsyncEnumerableMappingTest
    {
        private readonly RegisterIAsyncEnumerableMapping _register;

        public RegisterIAsyncEnumerableMappingTest()
        {
            _register = new RegisterIAsyncEnumerableMapping();
        }

        [Fact]
        public void Register()
        {
            var collection  = new ServiceCollection();
            
            _register.Register(typeof(int), typeof(string), collection);

            collection.Should().Contain(
                x => x.ServiceType == typeof(IMap<IAsyncEnumerable<int>, IAsyncEnumerable<string>>) 
                     && x.ImplementationType == typeof(IAsyncEnumerableMapping<int,string>));
        }
    }
}
