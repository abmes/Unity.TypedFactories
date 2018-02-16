NuGet install src\Unity.TypedFactories\packages.config -OutputDirectory packages

MsBuild .\src\Unity.TypedFactories\Unity.TypedFactories.csproj /verbosity:minimal

if (Test-Path -Path bin)
{
    Remove-Item bin -Recurse -Force
}

mkdir bin

Copy-Item .\Unity.TypedFactories.nuspec bin
mkdir bin\lib\netstandard2.0


Copy-Item .\src\Unity.TypedFactories\bin\Release\Unity.TypedFactories.dll .\bin\lib\netstandard2.0

$packageVersion = $env:PackageVersion

cd bin
NuGet pack Unity.TypedFactories.nuspec -Version $packageVersion
cd..

if (-not (Test-Path "build"))
{
    mkdir build
}

Copy-Item .\bin\*.nupkg .\build


Remove-Item bin -Recurse -Force
