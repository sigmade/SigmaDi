# ![image](https://user-images.githubusercontent.com/55326490/184547959-704433bc-03b9-4cb6-b6b4-5fdc106d6b2e.png) SigmaDi


Registration services from code 

``` C#
internal class Program
    {
        static void Main(string[] args)
        {
            var container = new Container("./Services.json");

            container.AddNew<IService, Service>();
            container.AddNew<ILogger, Logger>();
            container.AddNew<MainController, MainController>();
        }
    }
```
and file

``` JSON
{
  "AppAssembly": {
    "IDataService": "DataService",
    "MainController": "MainController"
  },
  "DataAssembly": {
    "IService": "Service",
    "ILogger": "Logger"
  }
}
```

------
**Install**

NuGet Package Manager


.NET CLI

```powershell
dotnet add package SigmaDi
```
[Go to nuget.org](https://www.nuget.org/packages/SigmaDi/)
