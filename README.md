# Cast Form

![build](https://github.com/lillo42/cast-form/workflows/Build%20Master/badge.svg)

### What is Cast Form?

Cast Form is a simple little library built to solve problem to map one object to another.


### How do I get started?

First, configure Cast Form to know what types you want to map, in the startup of your application:

```csharp
var ampper = new MapperBuilder()
      .AddMapper<Foo, FooDto>()
      .AddMapper<Bar, BarDto>()
      .Build();
```
Then in your application code, execute the mappings:

```csharp
var fooDto = mapper.Map<FooDto>(foo);
var barDto = mapper.Map<BarDto>(bar);
```

Check out the [getting started guide](https://automapper.readthedocs.io/en/latest/Getting-started.html). When you're done there, the [wiki](https://automapper.readthedocs.io/en/latest/) goes in to the nitty-gritty details. If you have questions, you can post them to [Stack Overflow](https://stackoverflow.com/questions/tagged/automapper) or in our [Gitter](https://gitter.im/AutoMapper/AutoMapper).

### Where can I get it?

Via terminal:

```bash
dotnet add package CastForm
```

Via nuget, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Cast Form](https://www.nuget.org/packages/CastForm/) from the package manager console:

```
PM> Install-Package AutoMapper
```


### Do you have an issue?

You might want to know exactly what [your mapping does](https://<To add>) at runtime.

If you're still running into problems, file an issue above.

### License, etc.

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct) and contributors under the [MIT license](/LICENSE).

