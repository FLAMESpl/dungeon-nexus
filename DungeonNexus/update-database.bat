@pushd src\DungeonNexus.Model
@dotnet ef database update --startup-project ..\DungeonNexus\DungeonNexus.csproj --no-build
@popd