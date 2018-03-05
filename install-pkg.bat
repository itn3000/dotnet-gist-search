if "%DOTNET_EXE%" == "" set DOTNET_EXE="dotnet"

set ROOTDIR=%~dp0
pushd "%ROOTDIR%/src/gist-search"
"%DOTNET_EXE%" pack -c Release
popd

"%DOTNET_EXE%" install tool -g --source "%ROOTDIR%/src/gist-search/bin/Release" --version 1.0.2 gist-search