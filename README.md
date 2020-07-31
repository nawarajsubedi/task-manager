# Task Manager API

A REST API to manage todo tasks, built with ASP.NET Web API 2 on .NET Framework 4.5.2.

- Users register and log in with a bearer token (OWIN OAuth + ASP.NET Identity)
- Tasks can be created, listed, updated, marked as completed and deleted
- Two roles: `admin` sees and manages all tasks, `user` only their own
- Data is stored in SQL Server with Entity Framework 6 (code first, automatic migrations)
- Unity is used for dependency injection, with a repository and unit of work for data access

## How to run

### 1. Requirements

- Windows
- Visual Studio 2019 with the **ASP.NET and web development** workload. This also installs IIS Express and SQL Server LocalDB.
- .NET Framework 4.5.2 Developer Pack (targeting pack). Visual Studio 2022 does not include it, so either install it or change the target framework to 4.8 in project properties.

### 2. Clone the repository

```bash
git clone https://github.com/nawarajsubedi/task-manager.git
cd task-manager
```

### 3. Open the solution

Open `DotNetAssignment.sln` in Visual Studio.

### 4. Restore NuGet packages

The `packages` folder is not in git. Visual Studio restores it on the first build. To do it by hand, right click the solution in Solution Explorer and click **Restore NuGet Packages**, or run:

```bash
nuget restore DotNetAssignment.sln
```

### 5. Check the connection string

`DotNetAssignment/Web.config` uses SQL Server LocalDB by default:

```xml
<add name="DatabaseConnectionString" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DotNetAssignmentDb;Integrated Security=True" providerName="System.Data.SqlClient" />
```

To use a full SQL Server instead, change `Data Source=(LocalDb)\MSSQLLocalDB` to your server, for example `Data Source=.` for the default local instance.

You do not need to create the database. It is created on the first request, and the roles `super_admin`, `admin` and `user` are added automatically.

### 6. Build and run

1. Press `Ctrl+Shift+B` to build.
2. Press `F5` (or `Ctrl+F5` without debugging).
3. The API starts on IIS Express at **http://localhost:44306**.

Bearer tokens are allowed over plain http only in the Debug build. In a Release build `/Token` requires https.
