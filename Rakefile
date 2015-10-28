task :default => "test:all"

task :clean do
  sh "rm -rf Braintree/bin"
  sh "rm -rf Braintree/obj"
  sh "rm -rf Braintree.Tests/bin"
  sh "rm -rf Braintree.Tests/obj"
end

task :compile => :clean do
  sh "xbuild Braintree.sln"
end

desc "run tests"
namespace :test do
  task :unit => [:ensure_boolean_type, :compile] do
    sh "mono Braintree.Tests/lib/NUnit-2.4.8-net-2.0/bin/nunit-console.exe -exclude=Integration Braintree.Tests/bin/Debug/Braintree.Tests.dll"
  end

  task :integration => [:ensure_boolean_type, :compile] do
    sh "mono Braintree.Tests/lib/NUnit-2.4.8-net-2.0/bin/nunit-console.exe -exclude=Unit Braintree.Tests/bin/Debug/Braintree.Tests.dll"
  end

  task :all => [:ensure_boolean_type, :compile] do
    sh "mono Braintree.Tests/lib/NUnit-2.4.8-net-2.0/bin/nunit-console.exe Braintree.Tests/bin/Debug/Braintree.Tests.dll"
  end
end

task :test => "test:all"

desc "run single test file (rake test_focus[Braintree.Tests.ClientTokenTestIT.Generate_GatewayRespectsMakeDefault], for example"
task :test_focus, [:test_name] => [:ensure_boolean_type, :compile] do |t, args|
  sh "mono Braintree.Tests/lib/NUnit-2.4.8-net-2.0/bin/nunit-console.exe -run=#{args[:test_name]} Braintree.Tests/bin/Debug/Braintree.Tests.dll"
end

desc "ensure that Request objects use Boolean? type instead of Boolean"
task :ensure_boolean_type do
  output = `find . -iname '*Request.cs' | xargs grep 'Boolean '`
  raise "\nUse Boolean? instead of Boolean in Request classes to prevent erroneous falses from being sent to the gateway:\n\n#{output}\n" unless output.empty?
end
