
# 📝 DEV4Side - BackEnd ToDoTasks

> Mini-app stile **Trello / ToDo list** sviluppata in .NET per la gestione personale dei task divisi in categorie (liste).

Questo progetto è stato realizzato come **esercizio di valutazione tecnica**, con l'obiettivo di dimostrare competenze nella progettazione e nello sviluppo di un backend completo: dalla creazione di API REST, alla gestione del database, fino alla strutturazione pulita del codice.

---

## 🚀 Funzionalità principali

API RESTful che permette la gestione di **liste** e **task** personali, con operazioni CRUD complete e autenticazione base tramite JWT.

Ogni **task** include:
- Titolo
- Descrizione
- Data di scadenza
- Stato: `Da fare`, `In corso`, `Completato`
- Collegamento a una **lista**

---

## 📚 Stack Tecnologico

- **ASP.NET Core Web API (.NET 8)**
- **C#**
- **Entity Framework Core**
- **SQL Server (hostato in Azure)**
- **AutoMapper**
- **JWT Authentication**
- Architettura: `Controller → Service → Repository`

---

## 📂 Architettura del progetto

```
├── Controllers
│   ├── TasksController.cs
│   └── ListsController.cs
│
├── DTOs
│   ├── ToDoTaskDto.cs
│   └── ToDoTaskRequest.cs
│
├── Models
│   ├── ToDoTask.cs
│   └── TaskList.cs
│
├── Repositories
│   ├── ITaskRepository.cs
│   └── ITaskListRepository.cs
│
├── Services
│   ├── TaskService.cs
│   └── TaskListService.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Mappings
│   └── AutoMapperProfile.cs
```

---

## 🔐 Autenticazione, Autorizzazione e Sicurezza

- Autenticazione tramite **JWT**
- Gestione dei **ruoli** (es. Admin, User)
- CORS configurato per dominio in Azure
- Controllo degli accessi a livello di endpoint

---

## 📌 API Endpoint

### ✅ User
- `POST /auth/register` → Registra nuovo utente
- `POST /auth/login` → Login utente
- `GET /users` → Elenco utenti
- `POST /users` → Crea utente
- `PUT /users/{id}` → Modifica utente
- `DELETE /users/{id}` → Elimina utente

### 📋 Liste
- `GET /lists` → Ritorna tutte le liste dell’utente
- `POST /lists` → Crea una nuova lista
- `DELETE /lists/{id}` → Elimina una lista

### ✅ Task
- `GET /tasks?listId={}` → Ritorna tutti i task associati a una lista
- `POST /tasks` → Crea un nuovo task
- `PUT /tasks/{id}` → Modifica task (stato, descrizione, data)
- `DELETE /tasks/{id}` → Elimina un task

---

## ⚙️ Setup del progetto

### 🧩 Prerequisiti

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB o Cloud - Azure SQL)
- Visual Studio / VS Code

---

### 🛠️ Struttura delle Tabelle (Estratto)

| Tabella       | Colonne Principali                             |
|---------------|--------------------------------------------------|
| `Users`       | Id, FirstName, LastName, Email, PasswordHash, Salt |
| `Roles`       | Id, RoleName, Description                       |
| `UserRoles`   | Id, UserId, RoleId, CreatedById, CreateRoleDate |
| `ToDoLists`   | Id, Name, UserId                                |
| `ToDoTasks`   | Id, Title, Description, DueDate, Status, ListId |

![Database schema](https://github.com/user-attachments/assets/0162a133-5a2b-4155-aa0f-6d2ae543846b)

---

## 📦 Stored Procedure

- `sp_login.sql` → Login utente con verifica hash e salt
- `sp_register.sql` → Registrazione utente con hash e salt della password

---

### 🛠️ Configurazione del database

In `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=ToDoTasksDb;Trusted_Connection=True;"
}
```

---

### 🧱 Scaffolding EF Core

```bash
scaffold-dbcontext "server=<YOUR_SERVER>;database=ToDoTasksDb;user id=...;password=..." \
Microsoft.EntityFrameworkCore.SqlServer \
-o Models -ContextDir Data -Context MyToDoDbContext -Force
```

---

## 🧰 Pacchetti NuGet

- `AutoMapper`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.Data.SqlClient`
- `System.IdentityModel.Tokens.Jwt`

---

## ▶️ Avviare il progetto

```bash
dotnet run
```

---

## 🚧 Possibili miglioramenti futuri

- Logging avanzato (Serilog)
- Validazione con FluentValidation
- Interfaccia frontend (es. Blazor, Angular o React)
- Dashboard amministrativa per gestione utenti e ruoli
- Deployment automatico (CI/CD) su Azure

---

## 📄 Licenza

Distribuito a scopo didattico e valutativo. Nessuna licenza commerciale attiva.
