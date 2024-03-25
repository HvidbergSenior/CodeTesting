@ECHO OFF

REM Untrust development certificate. Requires .NET SDK
dotnet dev-certs https --clean
