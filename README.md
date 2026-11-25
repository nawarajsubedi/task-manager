# Task Manager API

A REST API to manage todo tasks, built with ASP.NET Web API 2 on .NET Framework 4.5.2.

- Users register and log in with a bearer token (OWIN OAuth + ASP.NET Identity)
- Tasks can be created, listed, updated, marked as completed and deleted
- Two roles: `admin` sees and manages all tasks, `user` only their own
- Data is stored in SQL Server with Entity Framework 6 (code first, automatic migrations)
- Unity is used for dependency injection, with a repository and unit of work for data access

## Which OS can I use?

| Operating system | How to run | Status |
| --- | --- | --- |
| **Windows 10 / 11 (x64)** | [Option A: Visual Studio](#option-a-windows-with-visual-studio) | Easiest if you already use Visual Studio |
| **Windows 10 / 11 (x64)** | [Option B: Command line](#option-b-windows-from-the-command-line) | Tested on Windows 11 Pro with Visual Studio Build Tools 2022 |
| **Linux / macOS** | Not supported | Use a Windows PC or a Windows virtual machine |

The project runs on **.NET Framework 4.5.2** and is hosted by **IIS Express**. Both exist only on Windows. It does not run on .NET Core / .NET 5+, so it cannot run natively on Linux or macOS.

Both options also need a **SQL Server database**. The default is SQL Server LocalDB (see [Database](#database)).

---

## Option A: Windows with Visual Studio

### 1. Install the tools (one time)

- [Git for Windows](https://git-scm.com/download/win)
- Visual Studio 2019 or 2022 with the **ASP.NET and web development** workload. This also installs IIS Express and SQL Server LocalDB.
- The **.NET Framework 4.5.2 targeting pack**. Visual Studio 2022 does not include it. If you do not have it, use [Option B](#option-b-windows-from-the-command-line), which gets the reference assemblies from NuGet instead.

### 2. Clone the repository

```bash
git clone https://github.com/nawarajsubedi/task-manager.git
cd task-manager
```

### 3. Open the solution

Open `DotNetAssignment.sln` in Visual Studio.

### 4. Restore NuGet packages

The `packages` folder is not in git. Visual Studio restores it on the first build. To do it by hand, right-click the solution in Solution Explorer and click **Restore NuGet Packages**.

### 5. Check the connection string

The default connection string uses SQL Server LocalDB, which the Visual Studio workload installs. To use another SQL Server, see [Database](#database).

### 6. Build and run

1. Press `Ctrl+Shift+B` to build.
2. Press `F5`, or `Ctrl+F5` to run without debugging.
3. The API starts on IIS Express at **http://localhost:44306**.

Continue with [Check that it works](#check-that-it-works).

---

## Try the API

The examples use `curl` (Git Bash or any terminal with curl). You can also import `TaskManager.postman_collection.json` into Postman. The **Get token** request there saves the token for the other requests.

### 1. Register the first admin

The first admin can register without logging in. After an admin exists, only a logged in admin can add another admin.

```bash
curl -X POST http://localhost:44306/Account/RegisterAdmin \
  -H "Content-Type: application/json" \
  -d '{"Email":"admin@example.com","FirstName":"Admin","LastName":"User","Password":"Admin@123","ConfirmPassword":"Admin@123"}'
```

Password needs at least 6 characters with an uppercase letter, a lowercase letter, a digit and a symbol.

### 2. Get a token

```bash
curl -X POST http://localhost:44306/Token \
  -d "grant_type=password&username=admin@example.com&password=Admin@123"
```

Copy `access_token` from the response. It is valid for 1 hour. Send it in the `Authorization` header in the next requests:

```bash
TOKEN=paste-the-access-token-here
```

### 3. Create a task

```bash
curl -X POST http://localhost:44306/Task \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"Name":"Buy milk","Description":"Buy milk from the shop"}'
```

### 4. List tasks

```bash
curl http://localhost:44306/Task -H "Authorization: Bearer $TOKEN"
curl "http://localhost:44306/Task?completed=false" -H "Authorization: Bearer $TOKEN"
```

### 5. Update, complete and delete a task

```bash
curl -X PUT http://localhost:44306/Task/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"Name":"Buy milk","Description":"Buy 2 litres of milk"}'

curl -X PATCH http://localhost:44306/Task/1 -H "Authorization: Bearer $TOKEN"

curl -X DELETE http://localhost:44306/Task/1 -H "Authorization: Bearer $TOKEN"
```

### 6. Register a normal user

```bash
curl -X POST http://localhost:44306/Account/RegisterUser \
  -H "Content-Type: application/json" \
  -d '{"Email":"user@example.com","FirstName":"Test","LastName":"User","Organization":"Test Org","Password":"User@123","ConfirmPassword":"User@123"}'
```

Then get a token for this user as in step 2.

## Endpoints

| Method | URL                       | Who                       | Description                              |
|--------|---------------------------|---------------------------|------------------------------------------|
| POST   | `/Account/RegisterAdmin`  | anyone for first admin, then admin | register an admin               |
| POST   | `/Account/RegisterUser`   | anyone                    | register a user                          |
| POST   | `/Token`                  | anyone                    | get a bearer token                       |
| GET    | `/Account/GetAll`         | admin                     | list users                               |
| GET    | `/Task`                   | admin, user               | list tasks, `?completed=true/false` filter |
| GET    | `/Task/{id}`              | admin, user               | get one task                             |
| POST   | `/Task`                   | admin, user               | create a task                            |
| PUT    | `/Task/{id}`              | admin, user               | update name and description              |
| PATCH  | `/Task/{id}`              | admin, user               | mark task as completed                   |
| DELETE | `/Task/{id}`              | admin, user               | delete a task                            |

- A user only sees and changes tasks they created. Tasks of other users return `404`.
- An admin sees and changes all tasks.
- Task `Name` must be 5 to 30 characters, `Description` 10 to 500 characters.
- Requests without a valid token return `401`.

## Project structure

```
DotNetAssignment/
  App_Start/         Web API, auth (OAuth) and identity setup
  Controllers/       AccountController, TaskController
  Domain/            DbContext, entities, repository and unit of work
  IOC/               Unity container setup
  Migrations/        EF migrations config and role seed
  Models/            request / response models
  Providers/         OAuth provider that checks username and password
  Services/          task business logic
```
