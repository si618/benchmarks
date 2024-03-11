<#
.SYNOPSIS
    (Re)generate benchmark migrations.

.DESCRIPTION
    Remove existing migrations for Postgres and Sqlserver, and regenerate with current model.
#>

Push-Location .
Set-Location $PSScriptRoot

function CreateMigration {
    param(
        [Parameter(Mandatory=$true)]
        [string]$dbServer
    )
    $name = "Benchmarks.Database.$dbServer"
    $project = ".\$name\$name.csproj"
    $context = "$dbServer" + "DbContext"
    $path = ".\$name\Migrations"
    if (Test-Path $path) {
        Write-Host "Removing existing $dbServer migrations"
        Remove-Item -Recurse -Force -Path $path
    }
    Write-Host "Generating migrations for $dbServer"
    dotnet ef migrations add Current -p $project -c $context
    Write-Host "Formatting $dbServer project"
    dotnet format $project
}

CreateMigration -dbServer Postgres
CreateMigration -dbServer SqlServer

Pop-Location
