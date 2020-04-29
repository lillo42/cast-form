using System.Collections.Generic;
using CastForm.Collection;
using CastForm.RegisterServiceCollection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CastForm.Test.RegisterServiceCollection
{
    public class RegisterLazyEnumerableMappingTest
    {
        private readonly RegisterLazyEnumerableMapping _register;
        
        public RegisterLazyEnumerableMappingTest()
        {
            _register = new RegisterLazyEnumerableMapping();
        }

        [Fact]
        public void Register()
        {
            var collection  = new ServiceCollection();
            
            _register.Register(typeof(int), typeof(string), collection);

            collection.Should().Contain(
                x => x.ServiceType == typeof(IMap<IEnumerable<int>, IEnumerable<string>>) 
                     && x.ImplementationType == typeof(LazyEnumerableMapping<int,string>));
        }
    }
}
