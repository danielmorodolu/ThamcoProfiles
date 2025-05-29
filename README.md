README for Profile Service Container
Profile Service
Overview

The Profile Service container manages user profile data. It allows for user authentication, retrieving and updating profile details, and managing user accounts. The service can be accessed by other containers like the Product Service to fetch user-related product data and can make outbound API calls to external services for user-related information.

This service is deployed to Azure and can be accessed via the following URL:

    Profile Service URL: https://profile-thamco-dueuc8hug6b2g7cr.uksouth-01.azurewebsites.net/api/Account

Features

    User Authentication: Handles user login and logout via Auth0.
    Profile Management: Retrieve, update, and delete user profile details.
    Resilience: Implements Polly for retry and circuit breaker patterns to handle service failures.
    API Documentation: Automatically generated using Swagger (via Swashbuckle).

Technologies

    ASP.NET Core: For building the web API and managing controller actions.
    Entity Framework Core: ORM for interacting with the database and managing user profiles.
    Auth0: For handling user authentication and authorization.
    Polly: For managing transient faults with retry policies and circuit breakers.
    xUnit / Moq: For unit testing controllers and services.
    Swagger (Swashbuckle): For generating API documentation.
    Azure Web App: For cloud deployment.

Endpoints

The following API endpoints are available in the Profile Service:

    GET /Account/Login: Starts the authentication process via Auth0.
    GET /Account/Logout: Logs the user out from Auth0 and the local system.
    GET /Account/Details: Retrieves the profile details of the authenticated user.
    GET /Account/EditField: Retrieves a specific field (e.g., FirstName, Email) for editing.
    POST /Account/EditField: Updates the specified field in the user’s profile.
    GET /Account/Delete: Provides confirmation for account deletion.
    POST /Account/Delete: Deletes the user’s profile.

Data Model

The Profile Service works with the following data model:

    User Profile: Contains fields like FirstName, LastName, Email, PhoneNumber, and Auth0UserId.
    Product Data: Fetched from the Product Service based on user preferences or interactions with products.

Setup and Configuration
Prerequisites

    .NET 9.0 SDK or later.
    SQL Server or SQLite for data storage.
    Auth0 account for authentication.

Configuration

    Update Auth0 settings in the appsettings.json file for authentication.
    Configure the database connection string for both development and production environments.

Running the Service

Clone the repository: git clone https://github.com/danielmorodolu/ThamcoProfiles.git cd ThamcoProducts

Install dependencies: dotnet restore

Run application dotnet run Testing

Run unit tests using the following command: dotnet test
