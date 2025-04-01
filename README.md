# SolarPower

This is a .NET Core tidbit solution for a Web API that simulates solar power plants.  
&copy; 2025 Daniel Žagar

## Personal note

The compilation of material serves two purposes:
* To solve the tasks at hand, as given in the home assignment. 
* To illustrate my programming style, and how I usually prefer to arrange Visual Studio projects


## Getting started

After cloning the material to a local disk, the following steps should be carried out:  
1. Use a client connection to an MS SQL database (I used MS SQL Server Management Studio), and run the scripts in the folder:  

`$/SQL/DDL`  

Some paths in the local file system might have to be modified first (good to know for people still using floppy disk drives).  

2. Open Visual Studio, and run the NuGet package manager console. Set the default project to `SolPwr.DomainModel.Orm`.  
Run:  
```PowerShell
Update-Database -context UtilitiesContext 
```  

3. Still in the NuGet package manager console, switch the default project to `SolPwr.AuthModel`.  
Run:  
```PowerShell
Update-Database -context AuthIdentityContext
```  

4. Now, the application can be fired up. 

## The lazy way

If you don't want to run everything all the way, a testing approach can be used to illustrate the main principles and concepts.

In the test project `SolPwr.BusinessLogic.Test` there are two unit tests:  

```C#
CrudTests.PrePopulated_SelectAll_Success()

CrudTests.Empty_CreateOne_Success()
```  

Execute, or debug at your own discretion.


## Using the API

### Authorization
Create at least one user account with the **POST** call:

`http://localhost:5132/auth/register`  

Fill in the body according to this example:  
```
{
    "Email":"a.b@com",
    "Password":"X.x1",
    "ConfirmPassword":"X.x1"
}
```

To retrieve a JWT token, user can now authenticate with **POST**:

`http://localhost:5132/auth/signon`  

Fill in the body:  
```
{
    "Email":"a.b@com",
    "Password":"X.x1",
}
```

### CRUD Operations
> [!NOTE]
> All CRUD operations must be authorized with a JWT bearer token in the header.

To select all power plants, call this **GET**  

`http://localhost:5132/api/GetAllPlants`  

To create a new plant, use this **POST**  

`http://localhost:5132/api/CreatePlant`  

Fill in the body according to this example:  
```
{
    "id": "*",
    "plantName": "Plant API",
    "utcInstallDate": "2025-02-09T09:40:00",
    "location": {
        "latitude": 10,
        "longitude": 20
    },
    "powerCapacity": 400
}
```

To delete an existing plant, and all its related power history, use the following **PUT**:  

`http://localhost:5132/api/DeletePlant?id={Plant ID}`  

where `Plant ID` is the GUID value of the plant in question. Example: 

`http://localhost:5132/api/DeletePlant?id=9C8D7E47-5F83-4BE8-B323-D9D2F1439FD0`  

To update the properties of a plant (albeit altering the location dosen't make sense in reality), use the **PUT**:  

`http://localhost:5132/api/UpdatePlant?id={Plant ID}`  

Fill in the body according to this example:  
```
{
    "plantName": "Plant API",
    "utcInstallDate": "2025-02-09T09:40:00",
    "location": {
        "latitude": 10,
        "longitude": 20
    },
    "powerCapacity": 400
}
```

### Power output queries
> [!NOTE]
> All power queries must be authorized with a JWT bearer token in the header.

To retrieve power data, use the follwing **GET**:  

`http://localhost:5132/api/GetPowerData?id={Plant ID}&type={Type Code}&resolution={Resolution Key}&timespan={Time Span Value}&timespancode={Time Span Type}`  

The fields in the query string can have the following values:  

| Field | Values |
| ------- | ------- |
| `Plant ID` | The GUID id for the plant |
| `Type Code` | 1 = History<br/> 2 = Prognosis |
| `Resolution Key` | 15 = Every fifteen minutes<br/> 60 = Every hour |
| `Time Span Value` | The quantity of the desired time, according to its units |
| `Time Span Type` | ['d' \| 'h' \| 'm'] |

### Seeding

For test purposes, existing plants can be seeded back in time with random data, using this **POST**:  

`http://localhost:5132/tools/SeedPlants?quarters={Num fifteen minute intervals}`  


## Component structure

Separation of concerns was a driving factor for the component architecture, with the following characteristics:  
1. The dependency injection and configuration of containers, is forwarded to extension methods in corresponding sub system. This will hide details from `Program.cs` and keep it clean and tidy as well.  
2. The ORM data layer is in general separated from the Dto data layer.  
3. The authentication and authorization part is separated from the rest of the application, and also with respect to backing storage. Two different SQL schemes were used to keep track of that separation in the very database (`spu` and `spa`), instead of the commonly used `dbo`.  
4. The part that concerns a dependency to a specific RDBMS is kept in the projects with suffixes Orm. In this case, there is a dependency to MsSql Server there, but not elsewhere.  
5. The part which is handling the connectivity to a meteorological service, is implemented using a plugin architecture. It means that the application can dynamically handle various meteorological services (one at a time), if so desired. The `appsettings.json` contains a section which will point out what provider should be used at runtime. An assembly scoped attribute is used inside the plugin project to tag the DLL for plugin identification. Only one provider is implemented, though (www.open-meteo.com).  

_Please note that the_ `MeteoService` _configuration section has the value_ `open-meteo.com`. _That is the name of the plugin identifier, and has nothing to do with the URL to that very meteo service. In fact, any name could have been used there._

## VS Projects

| Project | Purpose |
| ------- | ------- |
| `SolPwr.Application.Api` | The main REST executable |
| `SolPwr.Core` | Contains base classes and various primitives |
| `SolPwr.BusinessLogic` | This is where things are stitched together |
| `SolPwr.BusinessLogic.Test` | Unit tests for the business logic |
| `SolPwr.DomainModel` | Domain classes for code first |
| `SolPwr.DomainModel.Orm` | The Entity Framework implementation |
| `SolPwr.AuthModel` | The identity implementation using JWT |
| `SolPwr.Integrations.Core` | Generic loading and management of any meteorological extension |
| `SolPwr.Integrations.Meteo` | Specific provider implementation for the selected service |
| `SolPwr.Protocols` | Service contracts and DTOs |


## Logging

Logging is not covered everywhere or on every level, but more like to illustrate some useful patterns.  

For instance, the [class](https://github.com/oniondeluxe/SolarPower/blob/master/SolPwr.Core/BusinessLogic/UnitOfWork.cs)  
```C#
UnitOfWork<T, D>
```  
has logging implemented as an aspect.  

There is also a [LinkedIn](https://www.linkedin.com/posts/daniel-z-07868a24_javascript-programming-nodejs-activity-6898956619485462528-Z7xs?utm_source=share&utm_medium=member_desktop&rcm=ACoAAAUVU9wBYfpttxgf_cVWjWKGLlje3SE6dAw) post to read, about logging in general.  


## Taxonomy

The way projects and namespaces are arranged in the solution is following a strict taxonomy

Namespaces are all originated from the templated file `Directory.Build.props`, residing in the solution directory. All new projects are set to have the default namespace set to the configuration property value `$(MasterNamespace)`. This means that all VS projects will have an identical root namespace. Sub namespaces will then be named according to the folder structure in each project.

Please note that namespaces are in general orthogonal to the names of the projects and their physical names in the file system.

An example:  

```C#
namespace OnionDlx.SolPwr.BusinessObjects
```  

is used in both the projects `SolPwr.DomainModel` and `SolPwr.DomainModel.Orm` 

The purpose of this, is to keep a separation between the logical object oriented abstractions inside the software stack, apart from the dependency and component hierarchy incarnated in the project files. These _can_ correlate incidentally, but not in general.


## Flaws, sources of improvements and other remarks

Some shortcuts have been taken to limit the scope, and to stay on focus  
1. The Tools controller and seeding API is not authorized, for testing convenience, but could very easily become so.  
2. The instantatation of the `IntegrationEndpoint` and the plugin loader with the `IBackgroundWorker`pattern, might be a bit inside-out. Could be refactored to become more straightforward. But, as this is beyond the programming task anyway, I'll leave it like that.  
3. When asking for 15 min data points, some interpolation needs to take place. This is not implemented.  
4. SQL Migrations should be generated as standalone SQL script files, so that they could be run in a more generic setup. Now, the NuGet package manager has to be used interactively. And also, a more robust DB versioning mechanism, including startup check, would be needed in a real system.  
5. And, of course the obvious - much more test coverage is needed.  
