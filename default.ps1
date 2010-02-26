properties {
    $base_dir = Split-Path -Path $MyInvocation.MyCommand.ScriptBlock.File
    $sandcastle_base = "C:\Program Files (x86)\Sandcastle"
    $sandcastle_presentation = "$sandcastle_base\Presentation\vs2005"
    
    $nunit = "C:\Program Files (x86)\NUnit 2.5.2\bin\net-2.0\nunit-console.exe"
    $msbuild = "C:\Windows\Microsoft.NET\Framework64\v3.5\MSBuild"
    $mrefbuilder = "$sandcastle_base\ProductionTools\MrefBuilder.exe"
    $xsltransform = "$sandcastle_base\ProductionTools\XslTransform.exe"
    $buildassembler = "$sandcastle_base\ProductionTools\BuildAssembler.exe"
    
    $assembly = "$base_dir\Braintree\bin\Debug\Braintree-1.1.1.dll"
    $test_assembly = "$base_dir\Braintree.Tests\bin\Debug\Braintree.Tests.dll"
    $reflection = "$base_dir\reflection.xml"
    $manifest = "$base_dir\manifest.xml"
    $toc = "$base_dir\toc.xml"

    $production_transforms = "$sandcastle_base\ProductionTransforms"

    $sandcastle_config = "$base_dir\sandcastle.config"
}

task default -depends Test

task Clean {
    if (Test-Path "$base_dir\Output")
    {
        Remove-Item "$base_dir\Output" -Recurse
    }
}

task Test -depends Compile { 
    & $nunit .\Braintree.Tests\bin\debug\Braintree.Tests.dll
}

task Compile { 
    & $msbuild
}

task GenerateDocs -depends Compile, Clean {
    mkdir "$base_dir\Output"
    mkdir "$base_dir\Output\html"
    Copy-Item "$sandcastle_presentation\configuration\sandcastle.config" $sandcastle_config
    Copy-Item "$sandcastle_presentation\icons" "$base_dir\Output" -Recurse
    Copy-Item "$sandcastle_presentation\Scripts" "$base_dir\Output" -Recurse
    Copy-Item "$sandcastle_presentation\Styles" "$base_dir\Output" -Recurse
    
    # Generate reflection.xml
    & $mrefbuilder $assembly /out:$reflection
    $reflection_transforms = "$production_transforms\ApplyVSDocModel.xsl", "$production_transforms\AddFriendlyFilenames.xsl"
    foreach ($transform in $reflection_transforms)
    {
        Copy-Item $reflection "temp.xml"
        & $xsltransform "temp.xml" /xsl:$transform /out:$reflection
        Remove-Item "temp.xml"
    }
    
    # Generate toc.xml
    & $xsltransform $reflection /xsl:"$production_transforms\CreateVSToc.xsl" /out:$toc
    
    # Generate manifest.xml
    & $xsltransform $reflection /xsl:"$production_transforms\ReflectionToManifest.xsl" /out:$manifest
    
    # Generate HTML docs
    & $buildassembler $manifest /config:$sandcastle_config
    
    Remove-Item $reflection
    Remove-Item $manifest
    Remove-Item $sandcastle_config
}