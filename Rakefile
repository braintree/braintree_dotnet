task :default => "test:all"

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

task :compile => :clean do
  sh "dotnet restore src/Braintree && dotnet build -f netstandard1.6 src/Braintree"

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

task :test => "test:all"

# Not implemented in dotnet CLI
desc "run single test file (rake test_focus[Braintree.Tests.ClientTokenTestIT.Generate_GatewayRespectsMakeDefault], for example"
task :test_focus, [:test_name] => [:ensure_boolean_type, :compile] do |t, args|
  sh ""
end
