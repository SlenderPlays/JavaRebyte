param (
    [Parameter(HelpMessage="The command you wish to execute. Must be: help, compile, jar or build. Use '-Command help' for details.")]
    [ValidateSet("help", "compile", "jar", "build", "deploy")]
    [string] $command = "help",

    [Parameter(HelpMessage="Path to the JDK bin directory. By default uses the 'JAVA_REBYTE_JDK' environment variable.")]
    [ValidateNotNullOrEmpty()]
    [string] $JDK = ($env:JAVA_REBYTE_JDK + "\"),

    [Parameter(HelpMessage="If present, any clean command will not ask you to confirm deletion. Does not have any effect if the clean paramater is not present.")]
    [switch] $force,

    [Parameter(HelpMessage="If present, cleans the output directory before running any compile command.")]
    [switch] $clean
)

$scriptDir      = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcDir         = Join-Path -Path $scriptDir -ChildPath .\src
$compiledDir    = Join-Path -Path $scriptDir -ChildPath .\out\compiled
$jarDir         = Join-Path -Path $scriptDir -ChildPath .\out\jar

$checkmark = [System.Char]::ConvertFromUtf32([System.Convert]::toInt32("2705",16))

if( !$JDK ) {
    Write-Output "Please set the JDK path to be used. Refer to the README.md for more information."
    exit
}
function Executa-Compile {
    if($clean.IsPresent) {
        if($force.IsPresent) {
            Remove-Item -Recurse ($compiledDir + "\*")
        }
        else {
            Remove-Item -Recurse -Confirm ($compiledDir + "\*")
        }
    }

    & ($JDK + "javac.exe") -classpath $srcDir -d $compiledDir ($srcDir + "\rebyte\helloworld\App.java") # 2>&1 | Out-String | Write-Output
    if (!(Test-Path -Path ($compiledDir + "\META-INF"))) {
        New-Item -ItemType Directory -Path ($compiledDir + "\META-INF")
    }
    Copy-Item -Path ($srcDir + "\META-INF\*") -Destination ($compiledDir + "\META-INF") -Recurse -Force
    Write-Output ($checkmark + " Compiled!")
}
function Executa-Jar {
    Push-Location -Path $compiledDir

    & ($JDK + "jar.exe") -cvfm  ($jarDir + "\RebyteHello.jar") ".\META-INF\MANIFEST.MF" .
    
    Write-Output ($checkmark + " Jar created!")
    Pop-Location
}

function Copy-Result {
    if($force.IsPresent) {
        Copy-Item -Path ($jarDir + "\RebyteHello.jar") -Destination ($scriptDir + "\..\JavaRebyte.Tests\RebyteHello.jar") -Force
    }
    else {
        Copy-Item -Path ($jarDir + "\RebyteHello.jar") -Destination ($scriptDir + "\..\JavaRebyte.Tests\RebyteHello.jar")
    }
    Write-Output ($checkmark + " Copied jar to JavaRebyte.Tests!")
}

Write-Output ("Using JDK Path: " + $JDK)
if($command -eq "help") {
    Write-Output "rebyte.ps1 Help Page:"
    Write-Output ""
    Write-Output "Paramater '-Command'"
    Write-Output "--------------------"
    Write-Output "help    -> Shows you this page."
    Write-Output "compile -> Compiles the files inside the 'src' directory, and outputs the class files to 'out/compiled'."
    Write-Output "jar     -> Builds the compiled files from 'out/compiled' and outputs a jar into 'out/jar'"
    Write-Output "build   -> Runs 'compile', then 'jar'."
    Write-Output "deploy  -> Builds the jar, then copies the output to 'JavaRebyte.Tests' so that it can be used in tests."
    Write-Output ""
    Write-Output "Paramater '-Clean'"
    Write-Output "--------------------"
    Write-Output "If present, cleans the output directory before running any compile command."
    Write-Output ""
    Write-Output "Paramater '-Force'"
    Write-Output "--------------------"
    Write-Output "If present, any clean command will not ask you to confirm deletion. Does not have any effect if the clean paramater is not present."
    Write-Output ""
    Write-Output "Paramater '-JDK'"
    Write-Output "--------------------"
    Write-Output "The path for the java development kit's bin directory. Default value is the environment variable named 'JAVA_REBYTE_JDK'."
    Write-Output ""
    Write-Output "For more information, refer to README.md"
    exit
}
if($command -eq "compile")  {
    Executa-Compile
    exit
}
if($command -eq "jar")  {
    Executa-Jar
    exit
}

if($command -eq "build") {
    Executa-Compile
    Executa-Jar
    exit
}

if($command -eq "deploy") {
    Executa-Compile
    Executa-Jar
    Copy-Result
    exit
}

<#
.SYNOPSIS
    Runs and compies the HelloJavaRebyte project.
.DESCRIPTION
    Runs and compies the HelloJavaRebyte project.
.EXAMPLE
    Test-MyTestFunction -Verbose
    Explanation of the function or its result. You can include multiple examples with additional .EXAMPLE lines
#>