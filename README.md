# ASP.NET Core 8 Web API - Clean Architecture

This project implements an **ASP.NET Core 8 Web API** following **Clean Architecture** and applying best practices such as 
**Dependency Injection, AutoMapper, FluentValidation, and Logging**.

## **Architecture Overview**

The project follows **Clean Architecture**, separating the code into different layers:

- **Domain -> Entities and business logic contracts.  
- **Application -> Use cases, validation, and model mapping.  
- **Infrastructure -> Repository implementations and third-party dependencies.  
- **WebApi -> Presentation layer and HTTP controllers.  

## **Best Practices Applied**

## **Separation of Concerns **
Each layer has a **single responsibility**:
- **Domain** → Contains only entities and contracts.  
- **Application** → Handles business logic, validation, and mapping.  
- **Infrastructure** → Implements database access and external services.  
- **WebApi** → Exposes endpoints without containing business logic.  

## **Dependency Injection**
-We use Iservices and IRepository in every layer
-We use `ApplicationDependencyInjection` and `InfrastructureDependencyInjection` to register services in `Program.cs`.
 useful to make `Program.cs` legible and short

 ## **Using AutoMapper for Model Conversion**
 -Instead of manual conversions, we use AutoMapper to map User -> UserDto.

 ## **Validation with FluentValidation**
 -Input data is validated before reaching the business logic, making the controllers clean.

 ## **Logging with ILogger**
 -Every error is logged and some log information in controllers.

## **Testable Code**
-Interfaces (IUserRepository, IUserService) are used to enable dependency injection.
-No direct dependency on SQL, allowing unit testing with Moq.

## **Global Exception Handling Middleware**
-To ensure consistent error responses, we implemented a custom exception middleware.
-This middleware intercepts all unhandled exceptions and returns a structured JSON response.

-Uses async/await for asynchronous operations, improving responsiveness.
-Avoids thread blocking when connecting to the database.
-Uses await using to properly release database connections.
-Moq allows simulating the database for unit testing without requiring a real DB.
-By not using an ORM and implementing IDbConnectionFactory, my repository layer becomes testable because 
it allows dependency injection and abstraction of the database connection. 
Instead of directly instantiating a database connection, the repository depends on IDbConnectionFactory, 
which can be easily mocked in unit tests. This enables testing the repository logic without requiring a real database, 
ensuring better isolation, faster test execution, and improved reliability

## **Bonus Integration Test And Unit Test**
-I have created an example of integration test using WebApplicationFactory and sql lite in memory.
-I have created an example of Unit test using Xunit only of the Controller part.

## **Script Users **
-you have to create a database and then execute the following script, and update the ConnectionStrings in appsettings.
-Script of table user with Email unique index.
CREATE TABLE [dbo].[Users](
[Id] [int] IDENTITY(1,1) NOT NULL,
[FirstName] [varchar](128) NULL,
[LastName] [varchar](128) NULL,
[Email] [varchar](250) NULL,
[DateOfBirth] [datetime] NULL,
[PhoneNumber] [bigint] NULL,
 CONSTRAINT [UQ_Users_Email] UNIQUE NONCLUSTERED
(
[Email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 


