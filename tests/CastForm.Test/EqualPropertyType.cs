using System.Collections.Immutable;
using CastForm.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace CastForm.Test
{
    public class EqualPropertyType
    {
        [Fact]
        public void PropertyWithSameName()
        {
            var code = @"
using System;
using System.Linq.Expressions;
using Oi;
using CastForm;

namespace Ola
{
    public class Test : MapperClass
    {
        public Test()
        {
            CreateMapper<Oi.Foo, Bar>();
        }
    }
}

namespace Oi
{
    public class Foo
    {
        public int Id { get; set; }
    }

    public class Bar
    {
        public int Id { get; set; }
    }

}

namespace CastForm
{
    public abstract class MapperClass
    {
        public MapperClass<TSource, TDestiny> CreateMapper<TSource, TDestiny>()
        {
            return new InternalMapperClass<TSource, TDestiny>();
        }
    }

    public abstract class MapperClass<TSource, TDestiny>
    {
        public MapperClass<TSource, TDestiny> CreateMapper<TSource, TDestiny>()
        {
            return new InternalMapperClass<TSource, TDestiny>();
        }
        
        public MapperClass<TDestiny, TSource> Reverse()
        {
            return new InternalMapperClass<TDestiny, TSource>();
        }

        public MapperClass<TSource, TDestiny> For<TDestinyMember, TSourceMember>(
            Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source)
        {
            return this;
        }
        
        public MapperClass<TSource, TDestiny> Ignore<TDestinyMember>(Expression<Func<TDestiny, TDestinyMember>> destiny)
        {
            return this;
        }
    }

    internal class InternalMapperClass<TSource, TDestiny> : MapperClass<TSource, TDestiny>
    {
        
    }
}
";


            var node = CSharpSyntaxTree.ParseText(code);

            var mapper = new MapperGenerator();
            var compilation = CSharpCompilation.Create("Test", new[] {node}, null, null);
            
            var driver = new CSharpGeneratorDriver(
                new CSharpParseOptions(),
                ImmutableArray.Create<ISourceGenerator>(mapper),
                ImmutableArray.Create<AdditionalText>()
            );
            
            driver.RunFullGeneration(compilation, out var outputCompilation, out var diagnostics);
            Assert.Empty(diagnostics);
        }
        
         [Fact]
        public void PropertyWithDifferentName()
        {
            const string code = @"
using System;
using System.Linq.Expressions;
using Oi;
using CastForm;

namespace Ola
{
    public class Test : MapperClass
    {
        public Test()
        {
            CreateMapper<Oi.Foo, Bar>()
                .For(x => x.Value, x => x.Id);
        }
    }
}

namespace Oi
{
    public class Foo
    {
        public int Id { get; set; }
    }

    public class Bar
    {
        public int Value { get; set; }
    }
}

namespace CastForm
{
    public abstract class MapperClass
    {
        public MapperClass<TSource, TDestiny> CreateMapper<TSource, TDestiny>()
        {
            return new InternalMapperClass<TSource, TDestiny>();
        }
    }

    public abstract class MapperClass<TSource, TDestiny>
    {
        public MapperClass<TSource, TDestiny> CreateMapper<TSource, TDestiny>()
        {
            return new InternalMapperClass<TSource, TDestiny>();
        }
        
        public MapperClass<TDestiny, TSource> Reverse()
        {
            return new InternalMapperClass<TDestiny, TSource>();
        }

        public MapperClass<TSource, TDestiny> For<TDestinyMember, TSourceMember>(
            Expression<Func<TDestiny, TDestinyMember>> destiny, Expression<Func<TSource, TSourceMember>> source)
        {
            return this;
        }
        
        public MapperClass<TSource, TDestiny> Ignore<TDestinyMember>(Expression<Func<TDestiny, TDestinyMember>> destiny)
        {
            return this;
        }
    }

    internal class InternalMapperClass<TSource, TDestiny> : MapperClass<TSource, TDestiny>
    {
        
    }
}
";


            var node = CSharpSyntaxTree.ParseText(code);

            var mapper = new MapperGenerator();
            var compilation = CSharpCompilation.Create("Test", new[] {node}, null, null);
            
            var driver = new CSharpGeneratorDriver(
                new CSharpParseOptions(),
                ImmutableArray.Create<ISourceGenerator>(mapper),
                ImmutableArray.Create<AdditionalText>()
            );
            
            driver.RunFullGeneration(compilation, out var outputCompilation, out var diagnostics);
            Assert.Empty(diagnostics);
        }
    }
}
