<#
.SYNOPSIS
    Build and run benchmarks app.

.DESCRIPTION
    Build application in release mode and run benchmarks.
#>

Push-Location .
Set-Location $PSScriptRoot

dotnet build --configuration Release --no-incremental
dotnet run --configuration Release --project Benchmarks.App

Pop-Location
