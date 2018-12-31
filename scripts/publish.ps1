dotnet publish -c Release /p:AssemblyVersion="$env:APPVEYOR_BUILD_VERSION"
#if ($env:APPVEYOR_REPO_TAG.Contains("true"))
#{
#	$version = $env:APPVEYOR_REPO_TAG_NAME.Substring(1)
#	$TAG = $env:APPVEYOR_REPO_TAG_NAME
#	$temp = "./publish/MyMessenger_Client_$TAG" + "_linux-x64"
#	dotnet publish -c Release -r linux-x64 -p:AssemblyVersion="$env:APPVEYOR_BUILD_VERSION" -o "$temp" -p:Version="$version" MyMessenger.Client.Console/MyMessenger.Client.Console.csproj
#	$temp = "./publish/MyMessenger_Client_$TAG" + "_win-x64"
3	dotnet publish -c Release -r win-x64 -p:AssemblyVersion="$env:APPVEYOR_BUILD_VERSION" -o "$temp" -p:Version="$version" MyMessenger.Client.Console/MyMessenger.Client.Console.csproj
#	$temp = "./publish/MyMessenger_Client_$TAG" + "_win-x86"
#	dotnet publish -c Release -r win-x86 -p:AssemblyVersion="$env:APPVEYOR_BUILD_VERSION" -o "$temp" -p:Version="$version" MyMessenger.Client.Console/MyMessenger.Client.Console.csproj
#	$temp = "./publish/MyMessenger_Server_$TAG" + "_linux-x64"
#	dotnet publish -c Release -r linux-x64 -p:AssemblyVersion="$env:APPVEYOR_BUILD_VERSION" -o "$temp" -p:Version="$version" MyMessenger.Server.Console/MyMessenger.Server.Console.csproj
#	$temp = "./publish/MyMessenger_Server_$TAG" + "_win-x64"
#	dotnet publish -c Release -r win-x64 -p:AssemblyVersion="$env:APPVEYOR_BUILD_VERSION" -o "$temp" -p:Version="$version" MyMessenger.Server.Console/MyMessenger.Server.Console.csproj
#	$temp = "./publish/MyMessenger_Server_$TAG" + "_win-x86"
#	dotnet publish -c Release -r win-x86 -p:AssemblyVersion="$env:APPVEYOR_BUILD_VERSION" -o "$temp" -p:Version="$version" MyMessenger.Server.Console/MyMessenger.Server.Console.csproj
#}