# Cast Form

![build](https://github.com/lillo42/cast-form/workflows/Build%20Master/badge.svg)
[![NuGet](http://img.shields.io/nuget/v/CastForm.svg)](https://www.nuget.org/packages/CastForm/)


### What is Cast Form?

Cast Form is a simple library built to solve problem to map an object to another.


### How do I get started?

First, configure Cast Form to know the types you want to map, in the startup of your application:

```csharp
var mapper = new MapperBuilder()
      .AddMapper<Foo, FooDto>()
      .AddMapper<Bar, BarDto>()
      .Build();
```
Then in your application code, execute the mappings:

```csharp
var fooDto = mapper.Map<FooDto>(foo);
var barDto = mapper.Map<BarDto>(bar);
```

### Where can I get it?

Via terminal:

```bash
dotnet add package CastForm
```

Via nuget, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Cast Form](https://www.nuget.org/packages/CastForm/) from the package manager console:

```
PM> Install-Package CastForm
```

### Current limitation

- Not support expression

### Do you have an issue?

You might want to know exactly what [your mapping does](https://github.com/lillo42/cast-form/wiki/How-it-works%3F) at runtime.

If you're still running into problems, file an issue above.

### License, etc.

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct) and contributors under the [MIT license](/LICENSE).

