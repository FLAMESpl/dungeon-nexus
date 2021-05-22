@pushd src\DungeonNexus.Model
@dotnet ef migrations remove --startup-project ..\DungeonNexus\DungeonNexus.csproj --no-build
@popd