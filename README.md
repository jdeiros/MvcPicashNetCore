# MvcPicashNetCore

## Crear la aplicacion web dotnet
-------------------------------------------------------------------------
###### Primero instalo los certificados para que no de errores el explorador al debug

dotnet dev-certs https --trust

###### Ahora si creo una aplicación web mvc
-------------------------------------------------------------------------

dotnet new mvc -o MvcPicashNetCore

-------------------------------------------------------------------------

###### Crear repo github
-------------------------------------------------------------------------

Lo primero fue crear el repo en git desde la pagina web 
y Luego:

echo "# MvcPicashNetCore" >> README.md
git init
git add README.md
git commit -m "first commit"
git remote add origin https://github.com/jdeiros/MvcPicashNetCore.git
git push -u origin master

-------------------------------------------------------------------------

-------------------------------------------------------------------------
###### Seguimos con la instalación de las herramientas para VSCode
-------------------------------------------------------------------------
1. **Base de datos**
-------------------------------------------------------------------------
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

-------------------------------------------------------------------------
2. **Herramientas para Scaffording**
-------------------------------------------------------------------------
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet restore

-------------------------------------------------------------------------

###### agregar vistas y controladores (scaffolding)
-------------------------------------------------------------------------
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
 -name LoansController `
 -m Loan `
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

-------------------------------------------------------------------------
 dotnet aspnet-codegenerator controller `
 -name InstallmentsController `
 -m Installment `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f

-------------------------------------------------------------------------
dotnet aspnet-codegenerator controller `
 -name LoanTypesController `
 -m LoanType `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f

-------------------------------------------------------------------------
dotnet aspnet-codegenerator controller `
 -name CollectionWeeksController `
 -m CollectionWeek `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f

-------------------------------------------------------------------------
dotnet aspnet-codegenerator controller `
 -name HolydaysController `
 -m Holyday `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f
-------------------------------------------------------------------------
dotnet aspnet-codegenerator controller `
 -name ZonesController `
 -m Zone `
 -dc PicashDbContext `
--relativeFolderPath Controllers `
--useDefaultLayout `
--referenceScriptLibraries -f
-------------------------------------------------------------------------
Solucion para gitignore
git rm -r --cached obj
git commit -am "Untrack /obj"
-------------------------------------------------------------------------
Graficos:
https://www.chartjs.org/docs/latest/charts/bar.html