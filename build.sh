#!/usr/bin/env bash

# Parameters
# $1 = NugetApiKey
NugetApiKey=$1
# $2 = MygetApiKey
MygetApiKey=$2

# standard output may be used as a return value in the functions
# we need a way to write text on the screen in the functions so that
# it won't interfere with the return value.
# Exposing stream 3 as a pipe to standard output of the script itself
exec 3>&1

# Setup some colors to use. These need to work in fairly limited shells, like the Ubuntu Docker container where there are only 8 colors.
# See if stdout is a terminal
if [ -t 1 ]; then
    # see if it supports colors
    ncolors=$(tput colors)
    if [ -n "$ncolors" ] && [ $ncolors -ge 8 ]; then
        bold="$(tput bold       || echo)"
        normal="$(tput sgr0     || echo)"
        black="$(tput setaf 0   || echo)"
        red="$(tput setaf 1     || echo)"
        green="$(tput setaf 2   || echo)"
        yellow="$(tput setaf 3  || echo)"
        blue="$(tput setaf 4    || echo)"
        magenta="$(tput setaf 5 || echo)"
        cyan="$(tput setaf 6    || echo)"
        white="$(tput setaf 7   || echo)"
    fi
fi

say_err() {
    printf "%b/n" "${red:-}dotnet_install: Error: $1${normal:-}" >&2
}

say() {
    # using stream 3 (defined in the beginning) to not interfere with stdout of functions
    # which may be used as return value
    printf "%b\n" "${cyan:-}$1${normal:-}" >&3
}

say_verbose() {
    if [ "$verbose" = true ]; then
        say "$1"
    fi
}

# install .net core sdk
say "dotnet-install: installing .net core 2.0 sdk"
./build/dotnet-install.sh -channel "2.0"
DotNetVersion=$(dotnet --version)
say "dotnet-version: running on $DotNetVersion"

# restore and run tooling
say "dotnet-restore: installing tools"
dotnet restore ./build/tools.csproj --packages ./build/tools --no-dependencies

say "gitversion: detecting semantic version"
MajorMinorPatch=$(./build/tools/gitversion.commandline/3.6.5/tools/GitVersion.exe /output json /showvariable MajorMinorPatch)
InformationalVersion=$(./build/tools/gitversion.commandline/3.6.5/tools/GitVersion.exe /output json /showvariable InformationalVersion)
say "gitversion: Major.Minor.Patch=$MajorMinorPatch"
say "gitversion: InformationalVersion=$InformationalVersion"

# build
say "dotnet-build: building Projac"
dotnet build ./src/Projac/Projac.csproj --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
say "dotnet-build: building Projac.Sql"
dotnet build ./src/Projac.Sql/Projac.Sql.csproj --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
say "dotnet-build: building Projac.SqlClient"
dotnet build ./src/Projac.SqlClient/Projac.SqlClient.csproj --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch

say "dotnet-build: building Projac.Tests"
dotnet build ./src/Projac.Tests/Projac.Tests.csproj --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
say "dotnet-build: building Projac.Sql.Tests"
dotnet build ./src/Projac.Sql.Tests/Projac.Sql.Tests.csproj --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
say "dotnet-build: building Projac.SqlClient.Tests"
dotnet build ./src/Projac.SqlClient.Tests/Projac.SqlClient.Tests.csproj --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch

# test
say "dotnet-xunit: running tests"
cd ./src/Projac.Tests
dotnet test --no-build --configuration Release
cd ../..
cd ./src/Projac.Sql.Tests
dotnet test --no-build --configuration Release
cd ../..
cd ./src/Projac.SqlClient.Tests
dotnet test --no-build --configuration Release
cd ../..

# package
say "dotnet-pack: packaging"
dotnet pack ./src/Projac/Projac.csproj --no-build --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
dotnet pack ./src/Projac.Sql/Projac.Sql.csproj --no-build --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch
dotnet pack ./src/Projac.SqlClient/Projac.SqlClient.csproj --no-build --configuration Release /p:AssemblyVersion=$MajorMinorPatch /p:FileVersion=$MajorMinorPatch /p:InformationalVersion=$InformationalVersion /p:PackageVersion=$MajorMinorPatch

# push
if [ "$NugetApiKey" != "" ]; then
    say "dotnet-nuget-push: pushing Projac package to nuget"
    dotnet nuget push ./src/Projac/bin/Release/Projac.$MajorMinorPatch.nupkg --source https://www.nuget.org/api/v2/package --api-key $NugetApiKey
    say "dotnet-nuget-push: pushing Projac.Sql package to nuget"
    dotnet nuget push ./src/Projac.Sql/bin/Release/Projac.Sql.$MajorMinorPatch.nupkg --source https://www.nuget.org/api/v2/package --api-key $NugetApiKey
    say "dotnet-nuget-push: pushing Projac.SqlClient package to nuget"
    dotnet nuget push ./src/Projac.SqlClient/bin/Release/Projac.SqlClient.$MajorMinorPatch.nupkg --source https://www.nuget.org/api/v2/package --api-key $NugetApiKey
fi

if [ "$MygetApiKey" != "" ]; then
    say "dotnet-nuget-push: pushing Projac package to myget"
    dotnet nuget push ./src/Projac/bin/Release/Projac.$MajorMinorPatch.nupkg --source https://www.myget.org/F/projac/api/v2/package --api-key $MygetApiKey
    say "dotnet-nuget-push: pushing Projac.Sql package to nuget"
    dotnet nuget push ./src/Projac.Sql/bin/Release/Projac.Sql.$MajorMinorPatch.nupkg --source https://www.myget.org/F/projac/api/v2/package --api-key $MygetApiKey
    say "dotnet-nuget-push: pushing Projac.SqlClient package to nuget"
    dotnet nuget push ./src/Projac.SqlClient/bin/Release/Projac.SqlClient.$MajorMinorPatch.nupkg --source https://www.myget.org/F/projac/api/v2/package --api-key $MygetApiKey
fi