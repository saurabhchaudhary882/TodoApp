# TodoApp

## Overview
TodoApp is a RESTful API built with ASP.NET Core that allows users to manage a To-Do list. The project is a combination of Clean Architecture, CQRS, Repository Pattern and utilizes MySQL as the database.

## Features
- Create, read, update, and delete (CRUD) todo items.
- Mark todo items as completed.
- Retrieve completed and incomplete todo items.
- Uses MediatR for command handling.
- Implements Repository Pattern with Dapper for database interactions.
- Includes Postman tests for API validation.

## Technologies Used
- **ASP.NET Core 8**
- **C#**
- **MediatR**
- **Dapper**
- **MySQL**
- **Docker**
- **Postman**

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Postman](https://www.postman.com/)

## Setting Up the Project
### 1. Run MySQL with Docker
```sh
docker run --name todo-mysql -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=TodoDb -p 3306:3306 -d mysql:latest
```
### 2. Setup Database
Run the SQL script to create necessary tables:
```sh
mysql -u root -p TodoDb < TodoApp.Infrastructure/Data/Scripts/InitDatabase.sql
```

### 3. Configure Environment Variables
Update `appsettings.json` with your MySQL connection details.

### 4. Run the API
```sh
dotnet run --project TodoApp.API
```

## API Endpoints
### Todos

**GET**  `/api/Todos`  
_Get all todo items_

**POST**  `/api/Todos`  
_Create a new todo item_

**GET**  `/api/Todos/{id}`  
_Get a specific todo item by ID_

**PUT**  `/api/Todos/{id}`  
_Update an existing todo item_

**DELETE**  `/api/Todos/{id}`  
_Delete a todo item_

**PATCH**  `/api/Todos/{id}/complete`  
_Mark a todo item as completed_

**GET**  `/api/Todos/completed`  
_Get all completed todo items_

**GET**  `/api/Todos/incomplete`  
_Get all incomplete todo items_

## Running Tests
Use Postman to import the test collection and execute requests.

## Contributors

- **saurabhchaudhary882@gmail.com**

