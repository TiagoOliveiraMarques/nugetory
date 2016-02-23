PUSH PACKAGE

nuget push <package_filename> -Source http://localhost:9000/api/v2
nuget push <package_filename> -Source http://localhost:9000/api/v2 -ApiKey <api_key>

INSTALL PACKAGE

nuget install <package_name> -Source http://localhost:9000/api/v2 -OutputDirectory . -NoCache
nuget install <package_name> -Source http://localhost:9000/api/v2 -Version <version> -OutputDirectory . -NoCache

DELETE PACKAGE

nuget delete <package_name> <version> -Source http://localhost:9000/api/v2
nuget delete <package_name> <version> -Source http://localhost:9000/api/v2 - ApiKey <api_key>
