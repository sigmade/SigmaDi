# SigmaDi

------
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
  "AppAssemply": {
    "IDataService": "DataService",
    "MainController": "MainController"
  },
  "DataAssemply": {
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
