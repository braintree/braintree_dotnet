# Development

## Development environment

The Makefile and Dockerfile will build images containing mono and dotnetcore 1.x and 2.x and will drop you to a terminal where you can run tests.

```
make mono
```

```
make core
```

```
make core2
```

## Running tests

The unit specs can be run by anyone on any system, but the integration specs are meant to be run against a local development server of our gateway code. These integration specs are not meant for public consumption and will likely fail if run on your system. To run unit tests use rake (`rake mono:test:unit` or `rake dotnet:test:unit`) or run the unit tests manually. Here is an example of how to run unit tests using the dotnet CLI tool from within /test/Braintree.Tests:

#### On Mac OS X or Unix-like environment
```
dotnet restore
dotnet build -f netcoreapp1.0
dotnet test . -f netcoreapp1.0
```

Or for .NET Core 2.x:
```
dotnet restore
dotnet build -f netcoreapp2.0
dotnet test . -f netcoreapp2.0
```

#### On a Windows environment
```
dotnet restore
dotnet build
dotnet test .
```

## Adding new libraries and dependencies
.NET Framework dependencies need to be added to the .csproj in the `'$(TargetFramework)' == 'net452'` ItemGroup. .NET Standard 1.3 dependencies need to be added to the .csproj in the `'$(TargetFramework)' == 'netstandard1.3'` ItemGroup.

## Cross-compatibility
We support .NET Framework 4.5.2+ and .NET Standard 1.3 (aka .NET Core 1.0). All new code and tests need to compile against both. If the standard library methods being used are not shared, use preprocessor directives:

```csharp
#if netcore
NetCore.Method();
#else
NetFramework.Method();
#end
```

You will need to use the dotnet tool to check that it compiles against .NET Standard 1.3.
