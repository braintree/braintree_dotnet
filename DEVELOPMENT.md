# Development

## Development environment

The Makefile and Dockerfile will build images containing mono and dotnetcore 3.1, and will drop you into a terminal where you can run tests.

```
make mono
```

```
make core3
```

## Running tests

The unit specs can be run by anyone on any system, but the integration specs are meant to be run against a local development server of our gateway code. These integration specs are not meant for public consumption and will likely fail if run on your system. To run unit tests use rake (`rake mono:test:unit` or `rake core3:test:unit`) or run the unit tests manually. Here is an example of how to run unit tests using the dotnet CLI tool from within /test/Braintree.Tests:

#### On Mac OS X or Unix-like environment

For .NET Core 3.1:
```
dotnet restore
dotnet test . -f netcoreapp3.1
```

#### On a Windows environment

```
dotnet restore
dotnet build
dotnet test .
```

## Adding new libraries and dependencies
.NET Framework dependencies need to be added to the .csproj in the `'$(TargetFramework)' == 'net452'` ItemGroup. .NET Standard 2.0 dependencies need to be added to the .csproj in the `'$(TargetFramework)' == 'netstandard2.0'` ItemGroup.

## Cross-compatibility
We support .NET Framework 4.5.2+ and .NET Standard 2.0 (This is so we can continue to support NET Core 2.1. For more information, see Microsoft's docs on [NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)). All new code and tests need to compile against both. If the standard library methods being used are not shared, use preprocessor directives:

```csharp
#if netcore
NetCore.Method();
#else
NetFramework.Method();
#end
```

You will need to use the dotnet tool to check that it compiles against .NET Standard 2.0.
