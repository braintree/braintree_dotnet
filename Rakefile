require "rubygems"

task :default => :test

task :clean do
  sh "rm -rf Braintree/bin"
  sh "rm -rf Braintree/obj"
  sh "rm -rf Braintree.Tests/bin"
  sh "rm -rf Braintree.Tests/obj"
end

task :compile => :clean do
  sh "xbuild Braintree.sln"
end

desc "run test"
task :test => :compile do
  sh "mono Braintree.Tests/lib/NUnit-2.4.8-net-2.0/bin/nunit-console.exe Braintree.Tests/bin/Debug/Braintree.Tests.dll"
end
