[CmdLetBinding()]
param(
    [Parameter(Mandatory=$false)][string]$NugetApiKey="",
    [Parameter(Mandatory=$false)][string]$MygetApiKey=""
)

# terminate upon any error encountered
$ErrorActionPreference="Stop"

function Say($message)
{
    Write-Host $message -foreground "Blue"
}

Say "nuget-apikey: $NugetApiKey"
Say "myget-apikey: $MygetApiKey"

# install .net core sdk
Say "dotnet-install: installing .net core 2.0 sdk"
.\build\dotnet-install.ps1 -Channel "2.0"
$DotNetVersion = dotnet --version
Say "dotnet-version: running on $DotNetVersion"

# restore and run tooling
Say "dotnet-restore: installing tools"
dotnet restore .\build\tools.csproj --packages .\build\tools --no-dependencies
if($Error.Count -ne 0 -or $LastExitCode -eq 0) {
    Say "gitversion: detecting semantic version"
    $MajorMinorPatch = .\build\tools\gitversion.commandline\3.6.5\tools\GitVersion.exe /output json /showvariable MajorMinorPatch
    $InformationalVersion = .\build\tools\gitversion.commandline\3.6.5\tools\GitVersion.exe /output json /showvariable InformationalVersion
    Say "gitversion: Major.Minor.Patch=$MajorMinorPatch"
    Say "gitversion: InformationalVersion=$InformationalVersion"

    # build, test, pack and push
    Say "dotnet-build: building solution"
    dotnet build .\src\All.sln --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
    if($Error.Count -ne 0 -or $LastExitCode -eq 0) {
        Push-Location
        Set-Location -Path .\src\Projac.Tests
        Say "dotnet-test: running tests"
        dotnet test --no-build --configuration Release
        Set-Location -Path ..\Projac.Sql.Tests
        Say "dotnet-test: running sql tests"
        dotnet test --no-build --configuration Release
        Set-Location -Path ..\Projac.SqlClient.Tests
        Say "dotnet-test: running sqlclient tests"
        dotnet test --no-build --configuration Release
        Set-Location -Path ..\Projac.SQLite.Tests
        Say "dotnet-test: running sqlite tests"
        dotnet test --no-build --configuration Release
        if($Error.Count -ne 0 -or $LastExitCode -eq 0) {
            Pop-Location
            Say "dotnet-pack: packaging"
            dotnet pack .\src\All.sln --no-build --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
            if($LastExitCode -eq 0) {
                if ($NugetApiKey -ne "") {
                    Say "dotnet-nuget-push: pushing projac package to nuget"
                    dotnet nuget push .\src\Projac\bin\Release\Projac.$MajorMinorPatch.nupkg --source https://www.nuget.org/api/v2/package --api-key $NugetApiKey
                    Say "dotnet-nuget-push: pushing projac.sql package to nuget"
                    dotnet nuget push .\src\Projac.Sql\bin\Release\Projac.Sql.$MajorMinorPatch.nupkg --source https://www.nuget.org/api/v2/package --api-key $NugetApiKey
                    Say "dotnet-nuget-push: pushing projac.sqlclient package to nuget"
                    dotnet nuget push .\src\Projac.SqlClient\bin\Release\Projac.SqlClient.$MajorMinorPatch.nupkg --source https://www.nuget.org/api/v2/package --api-key $NugetApiKey
                    Say "dotnet-nuget-push: pushing projac.sqlite package to nuget"
                    dotnet nuget push .\src\Projac.SQLite\bin\Release\Projac.SQLite.$MajorMinorPatch.nupkg --source https://www.nuget.org/api/v2/package --api-key $NugetApiKey
                }
                if ($MygetApiKey -ne "") {
                    Say "dotnet-nuget-push: pushing projac package to myget"
                    dotnet nuget push .\src\Projac\bin\Release\Projac.$MajorMinorPatch.nupkg --source https://www.myget.org/F/projac/api/v2/package --api-key $MygetApiKey
                    Say "dotnet-nuget-push: pushing projac.sql package to myget"
                    dotnet nuget push .\src\Projac.Sql\bin\Release\Projac.Sql.$MajorMinorPatch.nupkg --source https://www.myget.org/F/projac/api/v2/package --api-key $MygetApiKey
                    Say "dotnet-nuget-push: pushing projac.sqlclient package to myget"
                    dotnet nuget push .\src\Projac.SqlClient\bin\Release\Projac.SqlClient.$MajorMinorPatch.nupkg --source https://www.myget.org/F/projac/api/v2/package --api-key $MygetApiKey
                    Say "dotnet-nuget-push: pushing projac.sqlite package to myget"
                    dotnet nuget push .\src\Projac.SQLite\bin\Release\Projac.SQLite.$MajorMinorPatch.nupkg --source https://www.myget.org/F/projac/api/v2/package --api-key $MygetApiKey
                }
            }
        } else {
            Pop-Location
        }
    }
}