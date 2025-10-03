# Clinic Management System

This project is a **Clinic Management System** designed to manage visits, invoices, and providers. It includes a backend API built with .NET and a frontend application built with Angular.

---

## Project Structure

```
Assignment/
├── Assignment.Backend/       # Backend API built with .NET
│   ├── PracticeApi/          # Main API project
│   └── PracticeApiTests/     # Unit tests for the API
├── Assignment.Frontend/      # Frontend application built with Angular
│   └── PracticeApp/          # Angular application
└── docker-compose.yml        # Docker Compose configuration
```

---

## Features

- **Home Page**: Displays scheduled visits for the day.
- **Invoice Management**: View and manage visit invoices.
- **Provider-Specific View**: Providers can view their scheduled visits.
- **Reusable Components**: Components like the visits table are reusable across views.

---

## Prerequisites

- **.NET SDK**: Install the .NET SDK from [Microsoft](https://dotnet.microsoft.com/).
- **Node.js**: Install Node.js from [Node.js](https://nodejs.org/).
- **Angular CLI**: Install Angular CLI globally:
  ```bash
  npm install -g @angular/cli
  ```
- **Docker**: Install Docker from [Docker](https://www.docker.com/).

---

## Running the Project

### Using Docker Compose

1. Navigate to the project root directory:
   ```bash
   cd /path/to/Assignment
   ```

2. Build and start the services:
   ```bash
   docker-compose up --build
   ```

3. Access the application:
   - Backend API: [http://localhost:5265](http://localhost:5265)
   - Frontend App: [http://localhost:4200](http://localhost:4200)

4. Stop the services:
   ```bash
   docker-compose down
   ```

### Without Docker Compose

#### Backend API

1. Navigate to the backend directory:
   ```bash
   cd Assignment.Backend/PracticeApi
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

4. Access the API at [http://localhost:5265](http://localhost:5265).

#### Frontend Application

1. Navigate to the frontend directory:
   ```bash
   cd Assignment.Frontend/PracticeApp
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Run the application:
   ```bash
   ng serve
   ```

4. Access the app at [http://localhost:4200](http://localhost:4200).

---

## Testing the API

1. Navigate to the test project directory:
   ```bash
   cd Assignment.Backend/PracticeApiTests
   ```

2. Run the tests:
   ```bash
   dotnet test
   ```

---

## License

This project is licensed under the MIT License.