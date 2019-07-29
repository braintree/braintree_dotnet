task :default => "test"

def use_verbose_mode
  ENV["USE_VERBOSE_MODE"] ? " -v n" : ""
end

task :clean do
  sh "rm -rf src/Braintree/bin"
  sh "rm -rf src/Braintree/obj"
  sh "rm -rf test/Braintree.Tests/bin"
  sh "rm -rf test/Braintree.Tests/obj"
  sh "rm -rf test/Braintree.TestUtil/bin"
  sh "rm -rf test/Braintree.TestUtil/obj"
  sh "rm -rf test/Braintree.Tests.Integration/bin"
  sh "rm -rf test/Braintree.Tests.Integration/obj"
end

namespace :core do
  task :compile => [:clean, "sanity:test"] do
    sh "dotnet restore src/Braintree && dotnet build -f netstandard1.3 src/Braintree"

    sh "dotnet restore test/Braintree.TestUtil && dotnet build -f netcoreapp1.0 test/Braintree.TestUtil"

    sh "dotnet restore test/Braintree.Tests && dotnet build -f netcoreapp1.0 test/Braintree.Tests"

    sh "dotnet restore test/Braintree.Tests.Integration && dotnet build -f netcoreapp1.0 test/Braintree.Tests.Integration"
  end

  desc "run tests"
  namespace :test do
    task :unit => [:compile] do
      sh "dotnet test #{use_verbose_mode} test/Braintree.Tests/Braintree.Tests.csproj -f netcoreapp1.0"
    end

    task :integration => [:compile] do
      sh "dotnet test #{use_verbose_mode} test/Braintree.Tests.Integration/Braintree.Tests.Integration.csproj -f netcoreapp1.0"
    end

    task :all => [:compile] do
      sh "dotnet test #{use_verbose_mode} test/Braintree.Tests/Braintree.Tests.csproj -f netcoreapp1.0 && dotnet test test/Braintree.Tests.Integration/Braintree.Tests.Integration.csproj -f netcoreapp1.0"
    end
  end

  namespace :test_focus do
    desc "e.g. rake core:test_focus:unit[ToXml_IncludesDeviceData]"
    task :unit, [:test_name] => [:compile] do |t, args|
      sh "dotnet test #{use_verbose_mode} --filter Name~#{args[:test_name]} test/Braintree.Tests/Braintree.Tests.csproj -f netcoreapp1.0"
    end

    desc "e.g. rake core:test_focus:integration[Delete_DeletesPayPalAccount]"
    task :integration, [:test_name] => [:compile] do |t, args|
      sh "dotnet test #{use_verbose_mode} --filter Name~#{args[:test_name]} test/Braintree.Tests.Integration/Braintree.Tests.Integration.csproj -f netcoreapp1.0"
    end
  end
end

namespace :core2 do
  task :compile => [:clean, "sanity:test"] do
    sh "dotnet restore src/Braintree && dotnet build -f netstandard1.3 src/Braintree"

    sh "dotnet restore test/Braintree.TestUtil && dotnet build -f netcoreapp2.0 test/Braintree.TestUtil"

    sh "dotnet restore test/Braintree.Tests && dotnet build -f netcoreapp2.0 test/Braintree.Tests"

    sh "dotnet restore test/Braintree.Tests.Integration && dotnet build -f netcoreapp2.0 test/Braintree.Tests.Integration"
  end

  desc "run tests"
  namespace :test do
    task :unit => [:compile] do
      sh "dotnet test #{use_verbose_mode} test/Braintree.Tests/Braintree.Tests.csproj -f netcoreapp2.0"
    end

    task :integration => [:compile] do
      sh "dotnet test #{use_verbose_mode} test/Braintree.Tests.Integration/Braintree.Tests.Integration.csproj -f netcoreapp2.0"
    end

    task :all => [:compile] do
      sh "dotnet test #{use_verbose_mode} test/Braintree.Tests/Braintree.Tests.csproj -f netcoreapp2.0 && dotnet test test/Braintree.Tests.Integration/Braintree.Tests.Integration.csproj -f netcoreapp2.0"
    end
  end

  namespace :test_focus do
    desc "e.g. rake core2:test_focus:unit[ToXml_IncludesDeviceData]"
    task :unit, [:test_name] => [:compile] do |t, args|
      sh "dotnet test #{use_verbose_mode} --filter Name~#{args[:test_name]} test/Braintree.Tests/Braintree.Tests.csproj -f netcoreapp2.0"
    end

    desc "e.g. rake core2:test_focus:integration[Delete_DeletesPayPalAccount]"
    task :integration, [:test_name] => [:compile] do |t, args|
      sh "dotnet test #{use_verbose_mode} --filter Name~#{args[:test_name]} test/Braintree.Tests.Integration/Braintree.Tests.Integration.csproj -f netcoreapp2.0"
    end
  end
end

namespace :mono do
  task :compile => [:clean, "sanity:test"] do
    sh "MONO_BASE_PATH=/usr/lib/mono/ FrameworkPathOverride=$MONO_BASE_PATH/4.5-api/ dotnet build --framework net452"
  end

  desc "run tests"
  namespace :test do
    task :unit => [:compile] do
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests/bin/Debug/net452/Braintree.Tests.dll"
    end

    task :integration => [:compile] do
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests.Integration/bin/Debug/net452/Braintree.Tests.Integration.dll"
    end

    task :all => [:compile] do
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests/bin/Debug/net452/Braintree.Tests.dll"
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests.Integration/bin/Debug/net452/Braintree.Tests.Integration.dll"
    end
  end

  namespace :test_focus do
    desc "e.g. rake mono:test_focus:unit[Braintree.Tests.PaymentMethodTest.ToXml_IncludesDeviceData]"
    task :unit, [:test_name] => [:compile] do |t, args|
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests/bin/Debug/net452/Braintree.Tests.dll --test=#{args[:test_name]}"
    end

    desc "e.g. rake mono:test_focus:integration[Braintree.Tests.Integration.PaymentMethodIntegrationTest.Delete_DeletesPayPalAccount]"
    task :integration, [:test_name] => [:compile] do |t, args|
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests.Integration/bin/Debug/net452/Braintree.Tests.Integration.dll --test=#{args[:test_name]}"
    end
  end
end

namespace :sanity do
  desc "run sanity tests"
  task :test do
    sh "(! grep -rnw . --include=\*.cs -e double && echo 'PASS') || (echo 'FAIL: Use decimal instead of double for amounts and quantities for more reliable precision.' && exit 1)"
  end
end

task :test => "mono:test:all"

