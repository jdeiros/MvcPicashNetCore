# MvcPicashNetCore

Crear una aplicacion web dotnet 
-------------------------------------------------------------------------
//instalo los certificados para que no de errores el explorador al debug
dotnet dev-certs https --trust



Crear una aplicación web mvc
-------------------------------------------------------------------------
dotnet new mvc -o MvcPicashNetCore

-------------------------------------------------------------------------
crear repo github--------------------------------------------------------
nota: lo primero fue crear el repo en git desde la pagina web luego:
-------------------------------------------------------------------------
echo "# MvcPicashNetCore" >> README.md
git init
git add README.md
git commit -m "first commit"
git remote add origin https://github.com/jdeiros/MvcPicashNetCore.git
git push -u origin master
-------------------------------------------------------------------------
-------------------------------------------------------------------------

dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
-------------------------------------------------------------------------
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet restore
-------------------------------------------------------------------------
agregar vistas y controladores (scaffolding)
--------------------------------------------
dotnet aspnet-codegenerator controller `
-name DebtCollectorsController `
-m DebtCollector `
-dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f
-------------------------------------------------------------------------
 dotnet aspnet-codegenerator controller `
 -name CustomersController `
 -m Customer `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f
-------------------------------------------------------------------------
 dotnet aspnet-codegenerator controller `
 -name AddressesController `
 -m Address `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f
-------------------------------------------------------------------------
 dotnet aspnet-codegenerator controller `
 -name PaymentCommitmentsController `
 -m PaymentCommitment `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f
-------------------------------------------------------------------------
 dotnet aspnet-codegenerator controller `
 -name RoutesController `
 -m Route `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f
