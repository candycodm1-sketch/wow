VEHICLE RENTAL & BOOKING SYSTEM - LOGIN SCREEN (WinForms, C#)
================================================================

REQUIREMENTS
------------
- Windows OS (WinForms only runs on Windows)
- .NET 8 SDK installed (https://dotnet.microsoft.com/download)
- VS Code with the "C# Dev Kit" extension installed

HOW TO RUN
----------
1. Extract this folder anywhere on your computer.
2. Open the folder in VS Code:
      code VehicleRentalLogin
3. Open a terminal inside VS Code (Terminal > New Terminal).
4. Run:
      dotnet run
5. A window will open showing the login screen.

ADDING THE CAR IMAGE
---------------------
Put a car illustration (PNG) inside the "images" folder and name it:
      images/car.png
The app automatically loads it if present. If no image is found,
that area will just stay blank - the app still runs fine either way.

DEFAULT TEST LOGIN
-------------------
Email:    test@test.com
Password: 1234

(This is just a placeholder check in Form1.cs - BtnLogin_Click method.
Replace it with real authentication, e.g. a database lookup, when
you're ready to connect this to a backend.)

PROJECT FILES
-------------
- Program.cs               -> App entry point
- Form1.cs                 -> The login screen UI + logic (built entirely in code)
- VehicleRentalLogin.csproj -> Project file
- images/                  -> Put car.png here (optional)
