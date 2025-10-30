# HRPlatform

**HRPlatform** is a .NET Web API project for managing job candidates and their related skills. It allows adding, searching, and managing candidates and their skills.  

---

## Technologies Used

- ASP.NET Core 8.0 Web API  
- Entity Framework Core  
- SQL Server  
- Swagger UI  
- Visual Studio 2022  
- C#

---

## Features

- Add job candidate, add skills, update job candidate with skill, remove skill from job 
candidate, remove candidate, search candidate by name and/or given skill(s)
- Get all candidates, update candidate(used for test)

---

## How to Run

1. Open the project in **Visual Studio 2022** (first, git clone)
2. Ensure SQL Server is running (e.g., `(localdb)\MSSQLLocalDB`)  
3. Run migrations:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
4. Start the application (Ctrl + F5)
5. Open Swagger UI at: https://localhost:xxxx/swagger

---

## Author

Sara Djordjev,
Final project — 2025
