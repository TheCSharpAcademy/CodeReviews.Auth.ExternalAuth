# ProductManagementSystem — Setup & Run Instructions

This README explains how to get the project running locally.

> Target framework: .NET 8  
> Recommended IDE: Visual Studio 2022 / VS Code (with C#/.NET 8 SDK)

## Prerequisites
- .NET 8 SDK installed
- SQL Server, LocalDB, or SQL Express available for the connection string.
- Visual Studio 2022 (recommended) or VS Code + C# extension.
- Accounts & credentials for the external providers you want to test:
  - Google (OAuth client)
  - Microsoft (Azure AD app or Microsoft Account)
  - GitHub (OAuth app)
  - Facebook (App)
  - Twitter (Developer app)
- Optional: SMTP credentials for sending confirmation emails (or local SMTP tool for testing).

## Repository clone
1. Clone the repository
2. Open the solution in Visual Studio 2022 or open the folder in VS Code.

## App configuration overview
The app reads configuration from:
- appsettings.json / appsettings.Development.json
- User Secrets (strongly recommended for secrets such as client secrets and DB passwords)
- Environment variables

Important configuration keys (use these exact keys when setting secrets):

- Connection string
  - ConnectionStrings:DefaultConnection
- External auth
  - Authentication:Google:ClientId
  - Authentication:Google:ClientSecret
  - Authentication:Microsoft:ClientId
  - Authentication:Microsoft:ClientSecret
  - Authentication:GitHub:ClientId
  - Authentication:GitHub:ClientSecret
  - Authentication:Facebook:ClientId
  - Authentication:Facebook:ClientSecret
  - Authentication:Twitter:ConsumerAPIKey
  - Authentication:Twitter:ConsumerSecret
- Email configuration (example group name used by Program.cs)
  - EmailConfiguration:Host
  - EmailConfiguration:Port
  - EmailConfiguration:Username
  - EmailConfiguration:Password
  - EmailConfiguration:From

## Recommended setup using User Secrets (local dev)
1. From the project folder containing the .csproj file, initialize user secrets (if not already):
   - dotnet user-secrets init
2. Add required secrets (example):
   - dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\\MSSQLLocalDB;Database=Database_Name;Trusted_Connection=True;MultipleActiveResultSets=true"
   - dotnet user-secrets set "Authentication:Google:ClientId" "<your-google-client-id>"
   - dotnet user-secrets set "Authentication:Google:ClientSecret" "<your-google-client-secret>"
   - dotnet user-secrets set "Authentication:GitHub:ClientId" "<your-github-client-id>"
   - dotnet user-secrets set "Authentication:GitHub:ClientSecret" "<your-github-client-secret>"
   - dotnet user-secrets set "Authentication:Microsoft:ClientId" "<your-ms-client-id>"
   - dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "<your-ms-client-secret>"
   - dotnet user-secrets set "Authentication:Facebook:ClientId" "<your-facebook-client-id>"
   - dotnet user-secrets set "Authentication:Facebook:ClientSecret" "<your-facebook-client-secret>"
   - dotnet user-secrets set "Authentication:Twitter:ConsumerAPIKey" "<your-twitter-key>"
   - dotnet user-secrets set "Authentication:Twitter:ConsumerSecret" "<your-twitter-secret>"
   - dotnet user-secrets set "EmailConfiguration:Host" "smtp.example.com"
   - dotnet user-secrets set "EmailConfiguration:Port" "587"
   - dotnet user-secrets set "EmailConfiguration:Username" "smtp-user"
   - dotnet user-secrets set "EmailConfiguration:Password" "smtp-password"
   - dotnet user-secrets set "EmailConfiguration:From" "noreply@example.com"
