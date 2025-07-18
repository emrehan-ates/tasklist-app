# 📝 TaskList App

A full-stack task management application where users can create task lists, add and manage tasks, and track progress with ease.

## 🚀 Features

- 🗂️ Create, update, and delete task **lists**
- ✅ Add tasks under each list
- 🔁 Update task content or mark as **done**
- 🔐 Secure authentication with **JWT**
- 📈 Logging with **NLog**
- ⚙️ Built with modern and scalable technologies

## 🧠 Tech Stack

### 🖥 Backend

- **ASP.NET Core Web API**
- **JWT Authentication** for secure access control
- **Entity Framework Core** for data persistence
- **NLog** for structured logging

### 💻 Frontend

- **React** with **TypeScript (TSX)**
- **Axios** for API communication
- **React Router** for navigation
- Clean and responsive UI

## 📦 Installation

### Backend (ASP.NET Core)

1. Navigate to the backend directory:

```bash
cd backend
```

2. Restore and run:

```bash
dotnet restore
dotnet run
```

> Ensure your database connection string and JWT secrets are configured properly in `appsettings.json`.

### Frontend (React + TSX)

1. Navigate to the frontend directory:

```bash
cd frontend
```

2. Install dependencies:

```bash
npm install
```

3. Start the development server:

```bash
npm run dev
```

> Make sure your frontend is configured to call the correct backend API endpoints.

## 🛡️ Authentication

- All sensitive routes are protected using JWT-based access control.
- Users must sign in to access and manage tasks and lists.

## 📋 Example Use Case

1. Register or log in.
2. Create a new **list** (e.g., “Work”, “Personal”).
3. Add **tasks** to a list (e.g., “Finish report”, “Buy groceries”).
4. Mark tasks as done or update details.
5. Stay productive and organized!

## 📌 Project Structure

```
/backend       → ASP.NET Core Web API
/frontend      → React + TSX client app
```

## 🛠 Logging

All backend events and errors are logged using **NLog**. Logs can be viewed in the specified file path (configured in `nlog.config`).

---

## 🤝 Contributions

Contributions and feedback are welcome! Feel free to fork the repo, open issues, or submit pull requests.

## 📄 License

MIT License – feel free to use and modify.
