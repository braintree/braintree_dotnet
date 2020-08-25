task :default => "test"

def red(mytext) ; "\e[31m#{mytext}\e[0m" ; end

def use_verbose_mode
  ENV["USE_VERBOSE_MODE"] ? " -v n" : ""
end

def dotnet_test_unit
  "dotnet test #{use_verbose_mode} test/Braintree.Tests/Braintree.Tests.csproj --logger:'console;verbosity=detailed'"
end

def dotnet_test_integration
  "dotnet test #{use_verbose_mode} test/Braintree.Tests.Integration/Braintree.Tests.Integration.csproj --logger:'console;verbosity=detailed'"
end

def mono_test_unit
  "mono test/lib/NUnit-3.12.0/bin/net45/nunitlite-runner.exe test/Braintree.Tests/bin/Debug/net452/Braintree.Tests.dll --labels=After"
end

def mono_test_integration
  "mono test/lib/NUnit-3.12.0/bin/net45/nunitlite-runner.exe test/Braintree.Tests.Integration/bin/Debug/net452/Braintree.Tests.Integration.dll --labels=After"
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

namespace :core3 do
  task :compile => [:clean, "use_decimal_type:test"] do
    sh "dotnet restore src/Braintree && dotnet build -f netstandard2.0 src/Braintree"
    sh "dotnet restore test/Braintree.TestUtil && dotnet build -f netcoreapp3.1 test/Braintree.TestUtil"
    sh "dotnet restore test/Braintree.Tests && dotnet build -f netcoreapp3.1 test/Braintree.Tests"
    sh "dotnet restore test/Braintree.Tests.Integration && dotnet build -f netcoreapp3.1 test/Braintree.Tests.Integration"
  end

  namespace :test do
    # Usage:
    #   rake core3:test:unit
    #   rake core3:test:unit[ConfigurationTest]
    #   rake core3:test:unit[ConfigurationTest,ConfigurationWithStringEnvironment_Initializes]
    desc "run core unit tests"
    task :unit, [:class_name, :test_name] => [:compile] do |task, args|
      if args.class_name.nil?
        sh "#{dotnet_test_unit} -f netcoreapp3.1"
      elsif args.test_name.nil?
        sh "#{dotnet_test_unit} -f netcoreapp3.1 --filter FullyQualifiedName~#{args.class_name}"
      else
        sh "#{dotnet_test_unit} -f netcoreapp3.1 --filter FullyQualifiedName~#{args.class_name}.#{args.test_name}"
      end
    end

    # Usage:
    #   rake core3:test:integration
    #   rake core3:test:integration[PlanIntegrationTest]
    #   rake core3:test:integration[PlanIntegrationTest,All_ReturnsAllPlans]
    desc "run core integration tests"
    task :integration, [:class_name, :test_name] => [:compile] do |task, args|
      if args.class_name.nil?
        sh "#{dotnet_test_integration} -f netcoreapp3.1"
      elsif args.test_name.nil?
        sh "#{dotnet_test_integration} -f netcoreapp3.1 --filter FullyQualifiedName~#{args.class_name}"
      else
        sh "#{dotnet_test_integration} -f netcoreapp3.1 --filter FullyQualifiedName~#{args.class_name}.#{args.test_name}"
      end
    end

    # Usage:
    #   rake core3:test:all
    task :all => [:compile] do
      sh "#{dotnet_test_unit} -f netcoreapp3.1 && #{dotnet_test_integration} -f netcoreapp3.1"
    end
  end
end

namespace :mono do
  task :compile => [:clean, "use_decimal_type:test"] do
    sh "MONO_BASE_PATH=/usr/lib/mono/ FrameworkPathOverride=$MONO_BASE_PATH/4.5-api/ dotnet build --framework net452"
  end

  desc "run tests"
  namespace :test do
    # Usage:
    #   rake mono:test:unit
    #   rake mono:test:unit[ConfigurationTest]
    #   rake mono:test:unit[ConfigurationTest,ConfigurationWithStringEnvironment_Initializes]
    desc "run mono unit tests"
    task :unit, [:class_name, :test_name] => [:compile] do |task, args|
      if args.class_name.nil?
        sh "#{mono_test_unit}"
      elsif args.test_name.nil?
        sh "#{mono_test_unit} --test=Braintree.Tests.#{args.class_name}"
      else
        sh "#{mono_test_unit} --test=Braintree.Tests.#{args.class_name}.#{args.test_name}"
      end
    end

    # Usage:
    #   rake mono:test:integration
    #   rake mono:test:integration[PlanIntegrationTest]
    #   rake mono:test:integration[PlanIntegrationTest,All_ReturnsAllPlans]
    desc "run mono integration tests"
    task :integration, [:class_name, :test_name] => [:compile] do |task, args|
      if args.class_name.nil?
        # Integration tests have a bug which causes them to hang
        # indefinitely when run at once. The select, map, and each statements
        # are a temp workaround to only run each test file on its own.
        atLeastOneTestFailed = false
        integration_tests_to_run = Dir.entries('test/Braintree.Tests.Integration').select do |filename|
          filename[/Test.cs/]
        end
        integration_tests_to_run.map { |testfile| testfile[0..testfile.size-4] }.each do |testname|
            begin
              sh "#{mono_test_integration} --test=Braintree.Tests.Integration.#{testname}"
            rescue
              atLeastOneTestFailed = true
            end
        end

        if atLeastOneTestFailed
          raise Exception.new red("Some of the integration tests failed. Scroll up for details.")
        end
      elsif args.test_name.nil?
        sh "#{mono_test_integration} --test=Braintree.Tests.Integration.#{args.class_name}"
      else
        sh "#{mono_test_integration} --test=Braintree.Tests.Integration.#{args.class_name}.#{args.test_name}"
      end
    end

    # Usage:
    #   rake mono:test:all
    task :all => [:compile, :unit, :integration] do
    end
  end
end

namespace :use_decimal_type do
  desc "run sanity tests"
  task :test do
    sh "(! grep -rnw . --include=\*.cs -e double && echo 'PASS') || (echo 'FAIL: Use decimal instead of double for amounts and quantities for more reliable precision.' && exit 1)"
  end
end

task :test => "mono:test:all"
