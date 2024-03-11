dotnet tool install --global dotnet-ef

Push-Location .
Set-Location $PSScriptRoot

function CreateMigration {
    param(
        [Parameter(Mandatory=$true)]
        [string]$db
    )
    $name = "Benchmarks.Database.$name"
    $context = "$name" + "DbContext"
    Remove-Item -Recurse -Force -Path .\$name\Migrations
    dotnet ef migrations add Initial -c $context -o $name\Migrations    
}

Pop-Location

CreateMigration -db Postgres
CreateMigration -db SqlServer