using AutoFixture;

namespace CastForm.Acceptance.Test
{
    public class MapperSampleObject<T>
    {
        private readonly Fixture _fixture;

        public MapperSampleObject()
        {
            _fixture = new Fixture();
        }

        public class Source<T>
        {
            public T Value { get; set; }
        }
        
        
        public class Destiny<T>
        {
            public T Value { get; set; }
        }
    }
}
