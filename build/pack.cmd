SET msbuild="C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

%msbuild% ..\src\ChannelAdam.Azure.DocumentDB\ChannelAdam.Azure.DocumentDB.csproj /t:Rebuild /p:Configuration=Release;OutDir=bin\Release

..\tools\nuget\nuget.exe pack ..\src\ChannelAdam.Azure.DocumentDB\ChannelAdam.Azure.DocumentDB.nuspec -Symbols

pause
