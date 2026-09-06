# ⚡ Daily Task & Habit Tracker — Full-Stack Portfolio Application

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120?logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16.0-4169E1?logo=postgresql)](https://www.postgresql.org/)
[![EF Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4)](https://docs.microsoft.com/en-us/ef/core/)
[![React](https://img.shields.io/badge/React-18.0-61DAFB?logo=react)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.8-3178C6?logo=typescript)](https://www.typescriptlang.org/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker)](https://www.docker.com/)
[![xUnit](https://img.shields.io/badge/Tests-100%25%20Passing-brightgreen)](https://xunit.net/)

A production-grade, full-stack **Daily Task & Habit Tracker** web application engineered using **ASP.NET Core Web API**, **Clean Architecture**, **Entity Framework Core**, **PostgreSQL**, and a modern **React + TypeScript** glassmorphic frontend client.

---

## 🏛️ Application Architecture

The backend follows strict **Clean Architecture (4-Layer Pattern)**, decoupling business domain rules from framework and persistence infrastructure.

```text
DailyTaskTracker/
├── src/
│   ├── DailyTaskTracker.Domain/          # Pure C# Domain Entities, Enums & Rules (Zero Dependencies)
│   ├── DailyTaskTracker.Application/     # Interfaces, DTOs, Use Cases & Services
│   ├── DailyTaskTracker.Infrastructure/  # EF Core DbContext, PostgreSQL Fluent API Configurations
│   ├── DailyTaskTracker.API/             # Controllers, Global Exception Middleware, Swagger UI
│   └── DailyTaskTracker.ConsoleApp/     # Phase 1 Terminal Engine Demonstrator
│
├── tests/
│   ├── DailyTaskTracker.UnitTests/       # xUnit Domain & Service Logic Tests
│   └── DailyTaskTracker.IntegrationTests/# ASP.NET Core API Integration Tests
│
├── frontend/
│   └── daily-task-tracker/               # React + TypeScript Vite Web Client
│
└── docker-compose.yml                    # Multi-container orchestration (PostgreSQL + API + Web)
```

---

## 🗄️ Database ERD & Entity Relationships

```text
+-------------------+       1:N      +-------------------+
|       User        |--------------->|     Category      |
+-------------------+                +-------------------+
  |               |
  | 1:N           | 1:N
  v               v
+-------------------+       1:N      +-------------------+
|     TaskItem      |--------------->|  TaskCompletion   |
+-------------------+                +-------------------+
  |
  | 1:N
  v
+-------------------+       1:N      +-------------------+
|       Habit       |--------------->|  HabitCompletion  |
+-------------------+                +-------------------+
```

---

## ✨ Key Features

### 📋 1. Advanced Task Management Engine
- **Full Task CRUD**: Create, read, update, delete, complete, and uncomplete tasks.
- **Priority Levels**: `Low`, `Medium`, `High`, `Urgent`.
- **Query Engine**: Server-side filtering by priority, category, completion status, keyword search, and pagination.
- **Recurrence Engine**: Auto-spawns next recurring task instances (`Daily`, `Weekly`, `Monthly`) upon completion.

### 🔥 2. Habit Tracker & Streak Calculator
- **Streak Algorithms**: Calculates continuous `Current Streak` and all-time `Longest Streak` in daily/weekly habits.
- **Completion Logging**: Uses `DateOnly` completion tracking for historical accuracy.

### 📅 3. Interactive Monthly Calendar
- Monthly grid rendering task workloads, completion counts, and historical daily percentages.

### 📊 4. Productivity Analytics & Statistics
- Real-time completion rates, active habit streaks, best productivity days, and weekly breakdown charts.

### 🛡️ 5. Enterprise Error Handling
- Centralized `GlobalExceptionMiddleware` returning RFC 7807 problem details JSON responses.

---

## 📡 REST API Documentation

| HTTP Method | Endpoint Path | Description | Response Codes |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/tasks` | Get paginated tasks (filters: `search`, `priority`, `isCompleted`) | `200 OK` |
| `GET` | `/api/tasks/{id}` | Get task by unique ID | `200 OK`, `404 Not Found` |
| `POST` | `/api/tasks` | Create new task item | `201 Created`, `400 Bad Request` |
| `PUT` | `/api/tasks/{id}` | Update existing task details | `200 OK`, `404 Not Found` |
| `DELETE` | `/api/tasks/{id}` | Delete task by ID | `204 No Content`, `404 Not Found` |
| `PATCH` | `/api/tasks/{id}/complete` | Mark task completed (spawns recurring next) | `200 OK`, `404 Not Found` |
| `PATCH` | `/api/tasks/{id}/uncomplete` | Mark task incomplete | `200 OK`, `404 Not Found` |
| `GET` | `/api/habits` | Get all user habits & streak metrics | `200 OK` |
| `POST` | `/api/habits` | Create new habit tracker | `201 Created`, `400 Bad Request` |
| `POST` | `/api/habits/{id}/complete` | Record daily habit check-in | `200 OK`, `404 Not Found` |
| `GET` | `/api/calendar/{year}/{month}` | Get monthly calendar grid data | `200 OK`, `400 Bad Request` |
| `GET` | `/api/statistics/overview` | Get analytics overview | `200 OK` |

---

## 🚀 How to Run Locally

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Node.js 20+](https://nodejs.org/)

### 1. Start the ASP.NET Core API
```bash
dotnet run --project src/DailyTaskTracker.API/DailyTaskTracker.API.csproj --urls "http://localhost:5000"
```
Swagger UI will be served automatically at `http://localhost:5000`.

### 2. Start the React TypeScript Frontend
```bash
cd frontend/daily-task-tracker
npm install
npm run dev
```
Navigate to `http://localhost:5173`.

---

## 🐳 Docker Deployment

Run the entire full-stack application (PostgreSQL + API + Web Client) using single Docker Compose command:

```bash
docker-compose up -d --build
```

- **React Frontend**: `http://localhost:3000`
- **ASP.NET Core API & Swagger**: `http://localhost:5000`
- **PostgreSQL Database**: `localhost:5432`

---

## 🧪 Automated Testing

Execute the xUnit test suite across the solution:

```bash
dotnet test
```

```text
Passed! - Failed: 0, Passed: 8, Skipped: 0, Total: 8 - DailyTaskTracker.UnitTests.dll
Passed! - Failed: 0, Passed: 2, Skipped: 0, Total: 2 - DailyTaskTracker.IntegrationTests.dll
```
