properties {
}

task default -depends Test

task Test -depends Compile { 
  &"C:\Program Files (x86)\NUnit 2.5.2\bin\net-2.0\nunit-console.exe" .\Braintree.Tests\bin\debug\Braintree.Tests.dll
}

task Compile { 
  C:\Windows\Microsoft.NET\Framework64\v3.5\MSBuild
}
