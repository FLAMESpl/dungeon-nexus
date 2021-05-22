@pushd src\DungeonNexus.Model
@dotnet ef database drop --startup-project ..\DungeonNexus\DungeonNexus.csproj --no-build --force
@popd