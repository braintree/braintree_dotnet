# Development

## Development environment

The Makefile and Dockerfile will build images containing mono and dotnetcore and will drop you to a terminal where you can run tests.

```
make mono
```

```
make core
```

## Running tests

The unit specs can be run by anyone on any system, but the integration specs are meant to be run against a local development server of our gateway code. These integration specs are not meant for public consumption and will likely fail if run on your system. To run unit tests use rake (`rake mono:test:unit` or `rake dotnet:test:unit`) or run the unit tests manually. Here is an example of how to run unit tests using the dotnet CLI tool from within /test/Braintree.Tests:

#### On Mac OS X or Unix-like environment
```
dotnet restore
dotnet build -f netcoreapp1.0
dotnet test . -f netcoreapp1.0
```

#### On a Windows environment
```
dotnet restore
dotnet build
dotnet test .
```

## Adding new files
New files need to be added to the CompileInclude list in the .xproj file for the project they belong to. It's not required to add files to the project.json; developers building with dotnet will have the new files compiled automatically.

## Adding new libraries and dependencies
.NET Framework dependencies need to be added to both the .xproj and the project.json under the net452 framework option. .NET Standard 1.6 dependencies only need to be added to the project.json.

## Cross-compatibility
We support .NET Framework 4.5.2+ and .NET Standard 1.6 (aka .NET Core 1.0). All new code and tests need to compile against both. If the standard library methods being used are not shared, use preprocessor directives:

```csharp
#if netcore
NetCore.Method();
#else
NetFramework.Method();
#end
```

You will need to use the dotnet tool to check that it compiles against .NET Standard 1.6.
