using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test
{
    public class CounterTest
    {
        private readonly Fixture _fixture;
        private readonly Counter _counter;

        public CounterTest()
        {
            _fixture = new Fixture();
            _counter = new Counter();
        }

        [Fact]
        public void GetCounter_Should_Return1_When_HashCodeNotExist()
        {
            var result = _counter.GetCounter(_fixture.Create<int>());
            result.Should().Be(1);
        }
        
        [Fact]
        public void GetCounter_Should_IncrementCount_When_HashCodeExist()
        {
            var value = _fixture.Create<int>();
            _counter.GetCounter(value).Should().Be(1);
            _counter.GetCounter(value).Should().Be(2);
            _counter.GetCounter(value).Should().Be(3);
        }
        
        [Fact]
        public void Clean_Should_CleanCounter()
        {
            var value = _fixture.Create<int>();
            _counter.GetCounter(value).Should().Be(1);
            _counter.GetCounter(value).Should().Be(2);
            _counter.GetCounter(value).Should().Be(3);
            _counter.Clean();
            
            _counter.GetCounter(value).Should().Be(1);
        }
    }
}
