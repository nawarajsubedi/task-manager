# Task Manager API

A REST API to manage todo tasks, built with ASP.NET Web API 2 on .NET Framework 4.5.2.

- Users register and log in with a bearer token (OWIN OAuth + ASP.NET Identity)
- Tasks can be created, listed, updated, marked as completed and deleted
- Two roles: `admin` sees and manages all tasks, `user` only their own
- Data is stored in SQL Server with Entity Framework 6 (code first, automatic migrations)
- Unity is used for dependency injection, with a repository and unit of work for data access
