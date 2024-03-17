<#
.SYNOPSIS
    (Re)generate benchmark migrations.

.DESCRIPTION
    Remove existing migrations for Postgres and Sqlserver, and regenerate with current model.
#>

Push-Location .
Set-Location $PSScriptRoot

$project = ".\Benchmarks.Core\Benchmarks.Core.csproj"

function CreateMigration {
    param(
        [Parameter(Mandatory=$true)]
        [string]$dbServer
    )
    $context = "$dbServer" + "DbContext"
    $path = ".\Benchmarks.Core\Database\$dbServer\Migrations"
    $name = "Current$dbServer"
    if (Test-Path $path) {
        Write-Host "Removing existing migrations for $dbServer"
        Remove-Item -Recurse -Force -Path $path
    }
    $outputDir = "Database\$dbServer\Migrations"
    Write-Host "Generating migrations for $dbServer"
    dotnet ef migrations add $name -p $project -c $context -o $outputDir
}

CreateMigration -dbServer Postgres
CreateMigration -dbServer SqlServer

Write-Host "Invoking dotnet format to fix code-generated namespace ordering"
dotnet format $project

Pop-Location
