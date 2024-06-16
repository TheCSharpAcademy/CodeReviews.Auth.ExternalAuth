# Quiz Game

Application where user can create his own Quizzes play them and track scores in leaderboard.</p>

## 📸 Screenshots

<div>
<h3>Desktop</h3>
<img src="./screenshots/main-menu-desktop.png" width="500" />
<img src="./screenshots/create-quiz-desktop.png" width="500" />
<img src="./screenshots/leaderboard-desktop.png" width="500" />
<img src="./screenshots/register-desktop.png" width="500"/>
</div>
<div>
<h3>Mobile</h3>
<img src="./screenshots/main-menu-mobile.png" width="200" />
<img src="./screenshots/create-quiz-mobile.png" width="200" />
<img src="./screenshots/leaderboard-mobile.png" width="200" />
<img src="./screenshots/register-mobile.png" width="200" />
</div>

## 🧐 Features

Here're some of the project's best features:

- Creating Quizzes (choosing name selecting number of questions)
- Playing Quizzes (with username input quiz and difficulty level selection)
- Animated timer based on selected difficulty
- Saving records in leaderboards
- Ability to delete Quiz or Game(s)
- User-Friendly error and loading handling
- Authentication and Authorization
- Authenticated users can delete and create Quizzes/Games
- External Providers Auth (Facebook, Google, Github, Microsoft, Twitter)

## 🛠️ Installation Steps:

1. Go to **appsettings.json** and replace **ConnectionString:Default** with your connection string.

```appsettings.json
"ConnectionStrings": {
"Default": "your_connection_string"
}
```

2. In **Program.cs** change DbContext options to your Database Provider

```Program.cs
builder.Services.AddDbContext(options =>
{
options.YOUR_DATABASE(builder.Configuration.GetConnectionString("Default"));
});
```

3. Open **NuGet Package Manager Console** and run:

```Package-Manager Console
Add-Migration Initial
Update-Database
```

4. In UI folder go to **config** folder and change url:

```config.ts
export const url = 'https://localhost:{your_port}/api/';
```

5. In UI folder run:

```powershell
npm install
```

6. For external authentication you must configure applications in developer's portal and configure secrets below (or contact me if you want ready to go secrets)

```appsettings.json
"Authentication": {
    "Google": {
      "ClientId": "secret",
      "ClientSecret": "secret"
    },
    "Facebook": {
      "AppId": "secret",
      "AppSecret": "secret"
    },
    "Microsoft": {
      "ClientId": "secret",
      "ClientSecret": "secret"
    },
    "Github": {
      "ClientId": "secret",
      "ClientSecret": "secret"
    },
    "Twitter": {
      "ClientId": "secret",
      "ClientSecret": "secret"
    }
  },
```

## 💻 Built with

- Angular
- TypeScript
- ASP.NET Core
- Entity Framwork Core
- PostgreSQL
- Angular Material
- TailwindCSS
- ASP.NET Identity
- Logger
- OAuth 2.0

## 🧠 What I've learned

- Children Routing to organize layout for component groups
- Solid grasp on RxJS Observable Pattern concept
- Keeping possible losable progress using localStorage (Quiz Creator Stepper, Quiz Session Creator)
- Mapping through related tables using Entity Framework
- Server-Side Pagination
- Handling loading state and potential errors
- Handling Authentication and Authorization using ASP.NET Core Identity
- Logging errors in Database using ILogger
- Storing token in cookies on Authentication
- Configuring OAuth 2.0 Auth-Flow for External providers
- Using PKCE for authorization
- Configuring Apps in dedicated developer portals (Azure, Meta, Google Identity, etc.)
