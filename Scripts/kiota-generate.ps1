# PowerShell script to generate Kiota API client
$namespace = "UCR.EB.BioMicroscopeAdmin.Frontend.Blazor.Infrastructure.Kiota"
$swaggerUrl = "https://localhost:7121/swagger/v1/swagger.json"
$outputPath = Join-Path (Join-Path $PSScriptRoot "..") "Frontend.Blazor.Infrastructure/Kiota"

kiota generate -l CSharp -c ApiClient -n $namespace -o $outputPath --clean-output -d $swaggerUrl