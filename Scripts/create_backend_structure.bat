@echo off
REM Batch script to create a .NET project structure for BioMicroscopeAdmin

set "SOLUTION_NAME=UCR.EB.BioMicroscopeAdmin"

echo Creating .gitignore...
dotnet new gitignore

echo Creating Web API project and solution...
dotnet new webapi -o Backend.Api -n %SOLUTION_NAME%.Backend.Api -f net9.0
dotnet new sln -n %SOLUTION_NAME%
dotnet sln add .\Backend.Api\ --solution-folder Backend

echo Creating Application layer...
dotnet new classlib -o Backend.Application -n %SOLUTION_NAME%.Backend.Application -f net9.0
dotnet sln add .\Backend.Application\ --solution-folder Backend

echo Creating Application unit tests...
dotnet new xunit -o Backend.Application.Tests.Unit -n %SOLUTION_NAME%.Backend.Application.Tests.Unit -f net9.0
dotnet sln add .\Backend.Application.Tests.Unit\ --solution-folder Backend

echo Creating Dependency Injection layer...
dotnet new classlib -o Backend.DependencyInjection -n %SOLUTION_NAME%.Backend.DependencyInjection -f net9.0
dotnet sln add .\Backend.DependencyInjection\ --solution-folder Backend

echo Creating Infrastructure layer...
dotnet new classlib -o Backend.Infrastructure -n %SOLUTION_NAME%.Backend.Infrastructure -f net9.0
dotnet sln add .\Backend.Infrastructure\ --solution-folder Backend

echo Creating Infrastructure unit tests...
dotnet new xunit -o Backend.Infrastructure.Tests.Unit -n %SOLUTION_NAME%.Backend.Infrastructure.Tests.Unit -f net9.0
dotnet sln add .\Backend.Infrastructure.Tests.Unit\ --solution-folder Backend

echo Creating Domain layer...
dotnet new classlib -o Backend.Domain -n %SOLUTION_NAME%.Backend.Domain -f net9.0
dotnet sln add .\Backend.Domain\ --solution-folder Backend

echo Creating Domain unit tests...
dotnet new xunit -o Backend.Domain.Tests.Unit -n %SOLUTION_NAME%.Backend.Domain.Tests.Unit -f net9.0
dotnet sln add .\Backend.Domain.Tests.Unit\ --solution-folder Backend

echo Creating Presentation layer...
dotnet new classlib -o Backend.Presentation -n %SOLUTION_NAME%.Backend.Presentation -f net9.0
dotnet sln add .\Backend.Presentation\ --solution-folder Backend

echo Creating Presentation unit tests...
dotnet new xunit -o Backend.Presentation.Tests.Unit -n %SOLUTION_NAME%.Backend.Presentation.Tests.Unit -f net9.0
dotnet sln add .\Backend.Presentation.Tests.Unit\ --solution-folder Backend

echo Project structure created successfully!
pause