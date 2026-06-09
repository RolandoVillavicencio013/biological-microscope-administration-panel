@echo off
REM Batch script to add a Blazor frontend to existing BioMicroscopeAdmin solution

set "SOLUTION_NAME=UCR.EB.BioMicroscopeAdmin"

echo Creating Blazor project...
dotnet new blazor -o Frontend.Blazor -n %SOLUTION_NAME%.Frontend.Blazor -f net9.0 -ai -au None -int Auto

echo.
echo ===================================================================
echo IMPORTANT: Check the folder structure created by the command.
echo You may need to manually examine the generated files and adjust
echo the project names before continuing.
echo ===================================================================
echo.
pause

echo Adding Blazor projects to solution...
dotnet sln %SOLUTION_NAME%.sln add .\Frontend.Blazor --solution-folder Frontend
dotnet sln %SOLUTION_NAME%.sln add .\Frontend.Blazor.Client --solution-folder Frontend 

echo Creating Domain layer...
dotnet new classlib -o Frontend.Blazor.Domain -n %SOLUTION_NAME%.Frontend.Blazor.Domain -f net9.0

echo Creating Application layer...
dotnet new classlib -o Frontend.Blazor.Application -n %SOLUTION_NAME%.Frontend.Blazor.Application -f net9.0

echo Creating Infrastructure layer...
dotnet new classlib -o Frontend.Blazor.Infrastructure -n %SOLUTION_NAME%.Frontend.Blazor.Infrastructure -f net9.0

echo Creating Presentation (Razor Class Library) layer...
dotnet new razorclasslib -o Frontend.Blazor.Presentation -n %SOLUTION_NAME%.Frontend.Blazor.Presentation -f net9.0

echo Creating Domain unit tests...
dotnet new xunit -o Frontend.Blazor.Domain.Tests.Unit -n %SOLUTION_NAME%.Frontend.Blazor.Domain.Tests.Unit -f net9.0

echo Creating Application unit tests...
dotnet new xunit -o Frontend.Blazor.Application.Tests.Unit -n %SOLUTION_NAME%.Frontend.Blazor.Application.Tests.Unit -f net9.0

echo Creating Infrastructure unit tests...
dotnet new xunit -o Frontend.Blazor.Infrastructure.Tests.Unit -n %SOLUTION_NAME%.Frontend.Blazor.Infrastructure.Tests.Unit -f net9.0

echo Adding Domain and related projects to solution...
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.Domain --solution-folder Frontend/Domain
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.Application --solution-folder Frontend/Application
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.Infrastructure --solution-folder Frontend/Infrastructure
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.Presentation --solution-folder Frontend/Presentation

echo Adding test projects to solution...
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.Domain.Tests.Unit --solution-folder Frontend/Domain
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.Application.Tests.Unit --solution-folder Frontend/Application
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.Infrastructure.Tests.Unit --solution-folder Frontend/Infrastructure

echo Creating Dependency Injection layer...
dotnet new classlib -o Frontend.Blazor.DependencyInjection -n %SOLUTION_NAME%.Frontend.Blazor.DependencyInjection -f net9.0
dotnet sln %SOLUTION_NAME%.sln add Frontend.Blazor.DependencyInjection --solution-folder Frontend/DependencyInjection

echo Installing Kiota tool...
dotnet tool install --global Microsoft.OpenApi.Kiota

echo Frontend structure added to solution successfully!
echo Note: Use kiota-generate.ps1 to generate API client code when backend is ready.
pause