using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace CastForm.Test.CircleReference
{
    public class Circle
    {
        private readonly Fixture _fixture;

        public Circle()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void WithOverrideHashCode()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Reverse()
                .Build();

            var a = new SimpleA
            {
                Id= _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            var b = new SimpleB
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            a.Simple = b;
            b.Simple = a;

            var newB = mapper.Map<SimpleB>(a);
            newB.Id.Should().Be(a.Id);
            newB.Text.Should().Be(a.Text);
            newB.IsEnable.Should().Be(a.IsEnable);

            newB.Simple.Id.Should().Be(b.Id);
            newB.Simple.Text.Should().Be(b.Text);
            newB.Simple.IsEnable.Should().Be(b.IsEnable);
        }

        [Fact]
        public void WithOutOverrideHashCode()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleC, SimpleD>()
                .Reverse()
                .Build();

            var a = new SimpleC
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            var b = new SimpleD
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            a.Simple = b;
            b.Simple = a;

            var newB = mapper.Map<SimpleD>(a);
            newB.Id.Should().Be(a.Id);
            newB.Text.Should().Be(a.Text);
            newB.IsEnable.Should().Be(a.IsEnable);

            newB.Simple.Id.Should().Be(b.Id);
            newB.Simple.Text.Should().Be(b.Text);
            newB.Simple.IsEnable.Should().Be(b.IsEnable);
        }

        [Fact]
        public void WithOutOverrideHashCodeWithMoreThan8Parameter()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleF, SimpleE>()
                .Reverse()
                .Build();

            var a = new SimpleE
            {
                Short = _fixture.Create<short>(),
                UShort = _fixture.Create<ushort>(),
                Int = _fixture.Create<int>(),
                UInt = _fixture.Create<uint>(),
                Long = _fixture.Create<long>(),
                ULong = _fixture.Create<ulong>(),
                Float = _fixture.Create<float>(),
                Double= _fixture.Create<double>(),
                Decimal = _fixture.Create<decimal>(),
            };

            var b = new SimpleF
            {
                Short = _fixture.Create<short>(),
                UShort = _fixture.Create<ushort>(),
                Int = _fixture.Create<int>(),
                UInt = _fixture.Create<uint>(),
                Long = _fixture.Create<long>(),
                ULong = _fixture.Create<ulong>(),
                Float = _fixture.Create<float>(),
                Double = _fixture.Create<double>(),
                Decimal = _fixture.Create<decimal>(),
            };

            a.Simple = b;
            b.Simple = a;

            var newB = mapper.Map<SimpleF>(a);
            newB.Short.Should().Be(a.Short);
            newB.UShort.Should().Be(a.UShort); 
            newB.Int.Should().Be(a.Int);
            newB.UInt.Should().Be(a.UInt);
            newB.Long.Should().Be(a.Long);
            newB.ULong.Should().Be(a.ULong);
            newB.Float.Should().Be(a.Float);
            newB.Double.Should().Be(a.Double);
            newB.Decimal.Should().Be(a.Decimal);
            

            newB.Simple.Short.Should().Be(b.Short);
            newB.Simple.UShort.Should().Be(b.UShort);
            newB.Simple.Int.Should().Be(b.Int);
            newB.Simple.UInt.Should().Be(b.UInt);
            newB.Simple.Long.Should().Be(b.Long);
            newB.Simple.ULong.Should().Be(b.ULong);
            newB.Simple.Float.Should().Be(b.Float);
            newB.Simple.Double.Should().Be(b.Double);
            newB.Simple.Decimal.Should().Be(b.Decimal);
        }

        [Fact]
        public void MapTwice()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Reverse()
                .Build();

            var a = new SimpleA
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            var b = new SimpleB
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            a.Simple = b;
            b.Simple = a;

            var newB = mapper.Map<SimpleB>(a);
            newB.Id.Should().Be(a.Id);
            newB.Text.Should().Be(a.Text);
            newB.IsEnable.Should().Be(a.IsEnable);

            newB.Simple.Id.Should().Be(b.Id);
            newB.Simple.Text.Should().Be(b.Text);
            newB.Simple.IsEnable.Should().Be(b.IsEnable);
           
            newB = mapper.Map<SimpleB>(a);

            newB.Id.Should().Be(a.Id);
            newB.Text.Should().Be(a.Text);
            newB.IsEnable.Should().Be(a.IsEnable);

            newB.Simple.Id.Should().Be(b.Id);
            newB.Simple.Text.Should().Be(b.Text);
            newB.Simple.IsEnable.Should().Be(b.IsEnable);
        }


        [Fact]
        public async Task MapTwiceInDifferentTask()
        {
            var mapper = new MapperBuilder()
                .AddMapper<SimpleA, SimpleB>()
                .Reverse()
                .Build();

            var a = new SimpleA
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            var b = new SimpleB
            {
                Id = _fixture.Create<int>(),
                Text = _fixture.Create<string>(),
                IsEnable = _fixture.Create<bool>()
            };

            a.Simple = b;
            b.Simple = a;

            var task1 = Task.Factory.StartNew(() =>
            {
                var newB = mapper.Map<SimpleB>(a);
                newB.Id.Should().Be(a.Id);
                newB.Text.Should().Be(a.Text);
                newB.IsEnable.Should().Be(a.IsEnable);

                newB.Simple.Id.Should().Be(b.Id);
                newB.Simple.Text.Should().Be(b.Text);
                newB.Simple.IsEnable.Should().Be(b.IsEnable);
            });


            var task2 = Task.Factory.StartNew(() =>
            {

                var newB = mapper.Map<SimpleB>(a);
                newB.Id.Should().Be(a.Id);
                newB.Text.Should().Be(a.Text);
                newB.IsEnable.Should().Be(a.IsEnable);

                newB.Simple.Id.Should().Be(b.Id);
                newB.Simple.Text.Should().Be(b.Text);
                newB.Simple.IsEnable.Should().Be(b.IsEnable);
            });

            await Task.WhenAll(task1, task2);
        }

        public class SimpleA
        {
            public SimpleB Simple { get;  set; }

            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsEnable { get; set; }

            public override int GetHashCode() 
                => HashCode.Combine(Id, Text, IsEnable);
        }

        public class SimpleB
        {
            public SimpleA Simple { get; set; }
            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsEnable { get; set; }

            public override int GetHashCode() 
                => HashCode.Combine(Id, Text, IsEnable);
        }

        public class SimpleC
        {
            public SimpleD Simple { get; set; }
            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsEnable { get; set; }
        }

        public class SimpleD
        {
            public SimpleC Simple { get; set; }
            public int Id { get; set; }
            public string Text { get; set; }
            public bool IsEnable { get; set; }
        }

        public class SimpleE
        {
            public SimpleF Simple { get; set; }
            public short Short { get; set; }
            public ushort UShort { get; set; }
            public int Int { get; set; }
            public uint UInt { get; set; }
            public long Long { get; set; }
            public ulong ULong { get; set; }
            public float Float { get; set; }
            public double Double { get; set; }
            public decimal Decimal{ get; set; } 
        }

        public class SimpleF
        {
            public SimpleE Simple { get; set; }
            public short Short { get; set; }
            public ushort UShort { get; set; }
            public int Int { get; set; }
            public uint UInt { get; set; }
            public long Long { get; set; }
            public ulong ULong { get; set; }
            public float Float { get; set; }
            public double Double { get; set; }
            public decimal Decimal { get; set; }
        }
    }
}
