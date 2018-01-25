task :default => "test"

task :clean do
  sh "rm -f src/Braintree/project.lock.json"
  sh "rm -f test/Braintree.Tests/project.lock.json"
  sh "rm -f test/Braintree.TestUtil/project.lock.json"
  sh "rm -f test/Braintree.Tests.Integration/project.lock.json"
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
  task :compile => :clean do
    sh "dotnet restore src/Braintree && dotnet build -f netstandard1.3 src/Braintree"

    sh "dotnet restore test/Braintree.TestUtil && dotnet build -f netcoreapp1.0 test/Braintree.TestUtil"

    sh "dotnet restore test/Braintree.Tests && dotnet build -f netcoreapp1.0 test/Braintree.Tests"

    sh "dotnet restore test/Braintree.Tests.Integration && dotnet build -f netcoreapp1.0 test/Braintree.Tests.Integration"
  end

  desc "run tests"
  namespace :test do
    task :unit => [:compile] do
      sh "dotnet test test/Braintree.Tests -f netcoreapp1.0"
    end

    task :integration => [:compile] do
      sh "dotnet test test/Braintree.Tests.Integration -f netcoreapp1.0"
    end

    task :all => [:compile] do
      sh "dotnet test test/Braintree.Tests -f netcoreapp1.0 && dotnet test test/Braintree.Tests.Integration -f netcoreapp1.0"
    end
  end
end

namespace :mono do
  task :compile => :clean do
    sh "xbuild Braintree.sln"
  end

  desc "run tests"
  namespace :test do
    task :unit => [:compile] do
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests/bin/Debug/Braintree.Tests.dll"
    end

    task :integration => [:compile] do
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests.Integration/bin/Debug/Braintree.Tests.Integration.dll"
    end

    task :all => [:compile] do
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests/bin/Debug/Braintree.Tests.dll"
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests.Integration/bin/Debug/Braintree.Tests.Integration.dll"
    end
  end

  namespace :test_focus do
    desc "e.g. rake mono:test_focus:unit[Braintree.Tests.PaymentMethodTest.ToXml_IncludesDeviceData]"
    task :unit, [:test_name] do |t, args|
      sh "xbuild Braintree.sln"
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests/bin/Debug/Braintree.Tests.dll --test=#{args[:test_name]}"
    end

    desc "e.g. rake mono:test_focus:integration[Braintree.Tests.Integration.PaymentMethodIntegrationTest.Delete_DeletesPayPalAccount]"
    task :integration, [:test_name] do |t, args|
      sh "xbuild Braintree.sln"
      sh "mono test/lib/NUnit-3.4.1/bin/Release/nunit3-console.exe test/Braintree.Tests.Integration/bin/Debug/Braintree.Tests.Integration.dll --test=#{args[:test_name]}"
    end
  end
end

task :test => "mono:test:all"

