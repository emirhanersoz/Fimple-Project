--> STEP 1

// Install Templates to local. If you already installed before, skip this step.
// "/Users/UserName/WorkingDirectory" => This should be changed as your local code working directory.

// Note: Check if templates exist or not:
dotnet new --list

// Commands:
dotnet new --install /Users/UserName/WorkingDirectory/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api
dotnet new --install /Users/UserName/WorkingDirectory/Fi.Tools/Tools/Fi.Templates/Fi.Template.Schema
dotnet new --install /Users/UserName/WorkingDirectory/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api.IntegrationTests
// Samples:
dotnet new --install /Users/suatcilingir/Workazure1/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api
dotnet new --install /Users/suatcilingir/Workazure1/Fi.Tools/Tools/Fi.Templates/Fi.Template.Schema
dotnet new --install /Users/suatcilingir/Workazure1/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api.IntegrationTests

// If execution is succeeded, you will se below information on the command output.
Template Name                       Short Name          Language  Tags
---------------                     ---------------     --------  ----
Fi.Template.Api                     Fi.Template.Api     [C#]      Web 
Fi.Template.Schema                  Fi.Template.Schema  [C#]      Web 
Fi.Template.Api.IntegrationTests    Fi.Template.Schema  [C#]      Web 

--> STEP 2

// Run Template as a new project. Parameters:
// n: Project Name Fi.XXXX.Api
// o: Output Directory ("/Users/UserName/WorkingDirectory/RepoNameOfApiServiceProject/Src/" This is your local working directory. Project will be placed in a Api Service Project Directory.)
// ServiceUniqueName: Service Unique Name (Request this information from Fimple Team. Service definitions should be done by Fimple Team before this step).
// Database: Relational | NoSql
// StartErrorCode: Start ErrorCode (Request this information from Fimple Team. Service definitions should be done by Fimple Team before this step).
// EndErrorCode: End ErrorCode (Request this information from Fimple Team. Service definitions should be done by Fimple Team before this step).
// LocalHostPort: LocalHost Port (Request this information from Fimple Team. Service definitions should be done by Fimple Team before this step).
// TenantPersistenceStrategy: Database tenant strategy, TenantlessDatabase | DatabasePerTenant

// Commands:
dotnet new Fi.Template.Api -n Fi.XXXX.Api -o /Users/UserName/WorkingDirectory/RepoNameOfApiServiceProject/Src/Fi.XXXX.Api --ServiceUniqueName ServiceUniqueName --Database Relational --StartErrorCode 41000 --EndErrorCode 42000 --LocalHostPort 10041 --TenantPersistenceStrategy DatabasePerTenant --force
dotnet new Fi.Template.Schema -n Fi.XXXX.Schema -o /Users/UserName/WorkingDirectory/RepoNameOfApiServiceProject/Src/Fi.XXXX.Schema --force
dotnet new Fi.Template.Api.IntegrationTests -n Fi.XXXX.Api.IntegrationTests -o /Users/UserName/WorkingDirectory/RepoNameOfApiServiceProject/Test/Fi.ServiceUniqueName.Api.IntegrationTests --ServiceUniqueName ServiceUniqueName --force
// Samples:
dotnet new Fi.Template.Api -n Fi.Patika.Api -o /Users/ibrahimuvet/Desktop/Fi/Fi.Patika.Api/Src/Fi.Patika.Api --ServiceUniqueName Patika --Database Relational --StartErrorCode 41000 --EndErrorCode 42000 --LocalHostPort 10041 --TenantPersistenceStrategy DatabasePerTenant --force
dotnet new Fi.Template.Schema -n Fi.Patika.Schema -o /Users/ibrahimuvet/Desktop/Fi/Fi.Patika.Api/Src/Fi.Patika.Schema --force
dotnet new Fi.Template.Api.IntegrationTests -n Fi.Patika.Api.IntegrationTests -o /Users/ibrahimuvet/Desktop/Fi/Fi.Patika.Api/Test/Fi.Patika.Api.IntegrationTests --ServiceUniqueName Patika --force

// Note: If you do not have a sln, create a sln file under root folder.

--> STEP 3

// Add csproj to related .sln 
dotnet sln Fi.SLNNAME.Api.sln add Src/Fi.XXXX.Api/Fi.XXXX.Api.csproj;  
dotnet sln Fi.SLNNAME.Api.sln add Src/Fi.XXXX.Schema/Fi.XXXX.Schema.csproj;  
dotnet sln Fi.SLNNAME.Api.sln add Test/Fi.XXXX.Api.IntegrationTests/Fi.XXXX.Schema.csproj;  

/////////////////// Important Notes !!!!!!!! ///////////////////

// After creating API project,
-- You must change Src/Fi.XXXX.Api/Persistence/Fi(XXXX)DbContext.cs class name !!!
-- You must change Src/Fi.XXXX.Api/Migrations/Fi(XXXX)DbContextModelSnapshot class name !!!

// Check nuget versions and set properly for Fimple nuget packages. Do not prefer "*", use a static nuget version number.
// Sample:
<PackageReference Include="Fi.ApiBase" Version="1.14.*" />

// After creating IntegrationTests project,
-- You must change all class file names from Template to your ServiceUniqueName like Template... => Loan... !!!

/////////////////// Other Useful Scripts ///////////////////

// Use for global:
dotnet new Fimple --search

// List local templates, you can see Fi.Template.Api:
dotnet new --list

// Un-Install local template:
// Commands:
dotnet new --uninstall /Users/UserName/WorkingDirectory/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api
dotnet new --uninstall /Users/UserName/WorkingDirectory/Fi.Tools/Tools/Fi.Templates/Fi.Template.Schema
dotnet new --uninstall /Users/UserName/WorkingDirectory/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api.IntegrationTests
// Samples:
dotnet new --uninstall /Users/suatcilingir/Workazure1/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api
dotnet new --uninstall /Users/suatcilingir/Workazure1/Fi.Tools/Tools/Fi.Templates/Fi.Template.Schema
dotnet new --uninstall /Users/suatcilingir/Workazure1/Fi.Tools/Tools/Fi.Templates/Fi.Template.Api.IntegrationTests

// Help:
dotnet new Fi.Template.Api -h 

// Use for Nuget Package:
nuget.exe pack .\Fi.Template.Api.NetCoreTool.nuspec -OutputDirectory .\nupkg


