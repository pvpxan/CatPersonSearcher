# CatPersonSearcher
* Test Application using `MVVM`, `Entity Framework`, and `System.Data.SQLite`.
* Created in `Visual Studio 2017`
* Requires `.net Framework 4.6.2`
* No database initialization required!
* Repo contains a compiled version ready to go in the Compiled directory.

# Package Version:
`<package id="EntityFramework" version="6.2.0" targetFramework="net462" />`\
`<package id="System.Data.SQLite" version="1.0.111.0" targetFramework="net462" />`\
`<package id="System.Data.SQLite.Core" version="1.0.111.0" targetFramework="net462" />`\
`<package id="System.Data.SQLite.EF6" version="1.0.111.0" targetFramework="net462" />`\
`<package id="System.Data.SQLite.Linq" version="1.0.111.0" targetFramework="net462" />`

# Contains Embedded Referance to StreamlineMVVM
https://github.com/pvpxan/StreamlineMVVM

# The Data
Seeded from https://www.generatedata.com/

# Layout
* Classes:
  * `SQLConnection` Initiates a connection to the database.
  * `SQLFunctions` Class spread across multiple files using partial keywork. Does lots of work.
  * `TaskWorker` Toying with a `Task` wrapper that simulates `BackgroundWorker`.
  * `WindowFactory` Event Driven Window opening.
  * `ImageSourceReader` Used to convert internal resources or external images to a `BitmapSource`.

* `MainWindow` View can changes out different controls.
  * `Connect` connects to the database.
  * `QuickSearch` uses a 2 column paginated `ListView`.
  * `DetailedSearch` has a fully expanded `ListView`.

* There are couple controls that leverage `StreamlineMVVM` `DialogBaseWindow`.
  * `Settings`
  * `AddEdit`
  * `About` (Technically uses another feature of `StreamlineMVVM` to open.)

* `Catalog` Place to browse the embedded images.

* `MultiModel` class inherits from ORM classes and is used through the application.

* All controls and windows have a `DataContext` `ViewModel` class.

* Using some slight cleverness that I am probably too proud of, `QuickSearch` and `DetailedSearch` both use the SAME `ViewModel` Class.

# Features
* Simulated delay can be turned on and even adjusted in `Settings`. This helps test concurrency.
* Default search control can be set in `Settings`.
* The search windows can show full paginated results on blank searches.
* The seeded data can be reloaded if the first time opened version gets all jacked up.

# Required Files and DLLs
`.\x64\SQLite.Interop.dll`\
`.\x86\SQLite.Interop.dll`\
`CatPersonSearcher.exe`\
`CatPersonSearcher.exe.config`\
`CatPersonSearcher.pdb`\
`EntityFramework.dll`\
`EntityFramework.SqlServer.dll`\
`EntityFramework.SqlServer.xml`\
`EntityFramework.xml`\
`System.Data.SQLite.dll\
`System.Data.SQLite.dll.config`\
`System.Data.SQLite.EF6.dll`\
`System.Data.SQLite.Linq.dll`\
`System.Data.SQLite.xml`

* After application runs, the `.\Resources\PersonData.db` file is used.

# Unit Testing
* Most testing was done with the UnitTesting program along side this.
* There was some janky test as you go done also :/
