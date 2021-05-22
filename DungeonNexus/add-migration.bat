@pushd src\DungeonNexus.Model
@dotnet ef migrations add %1 --startup-project ..\DungeonNexus\DungeonNexus.csproj --no-build
@popd