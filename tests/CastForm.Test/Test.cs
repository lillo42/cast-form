namespace CastForm.Test
{
    public class Test : MapperClass
    {
        public Test()
        {
            CreateMapper<Foo, Bar>()
                .For(x => x.Value, x => x.Id);
        }
    }


    public class Foo
    {
        public int Id { get; set; }
    }

    public class Bar
    {
        public int Value { get; set; }
    }
}
