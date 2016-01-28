#!/bin/sh

SCRIPTPATH="$( cd "$(dirname "$0")" ; pwd -P )"
cd $SCRIPTPATH

nuget restore -PackagesDirectory packages
nuget install NUnit.Console -ExcludeVersion -OutputDirectory packages

xbuild /p:Configuration=Release nugetory.sln

mono packages/NUnit.Console/tools/nunit3-console.exe \
	nugetory.tests/bin/Release/nugetory.tests.dll --labels=On --nocolor \
	--verbose --workers=1 --full --result:"../nunit-result.xml;format=nunit2"

pushd nugetory
nuget pack nugetory.nuspec
mv *.nupkg ../../
popd