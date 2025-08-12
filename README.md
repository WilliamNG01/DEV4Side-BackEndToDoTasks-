
# 📝 DEV4Side - BackEnd ToDoTasks

> Mini-app stile **Trello / ToDo list** sviluppata in .NET per la gestione personale dei task divisi in categorie (liste).

Questo progetto è stato realizzato come **esercizio di valutazione tecnica**, con l'obiettivo di dimostrare competenze nella progettazione e nello sviluppo di un backend completo: dalla creazione di API REST, alla gestione del database, fino alla strutturazione pulita del codice.  
> URL per testare: [Swagger API](https://webapitodolist20250805111326-fxdcgvctgqctb6cg.canadacentral-01.azurewebsites.net/swagger/v1/swagger.json)

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
- **xUnit** per test automatici
- **Middleware personalizzati**:
  - `JwtMiddleware` → gestione e validazione dei token JWT
  - `RateLimitingMiddleware` → protezione anti-DDoS e limitazione delle richieste per evitare sovraccarico della base dati Azure
- Architettura: `Controller → Service → Repository`

---

## 📂 Architettura del progetto

```
├───Tests
│   └───TestToDoList
│       │   AuthTest.cs
│       │   RateLimitingMiddlewareTests.cs
│       │   TestToDoList.csproj
│
└───WebAPITodoList
    │   appsettings.json
    │   program.cs
    │   (scriptDatabase.sql)
    ├───Controllers
    │       AuthController.cs
    │       TasksController.cs
    │       ToDoListController.cs
    │       UsersController.cs
    │
    ├───Data
    │       MyToDoDbContext.cs
    │
    ├───DTOs
    │       LoginRequest.cs
    │       RegisterUserDto.cs
    │       TaskStatus.cs
    │       ToDoListRequest.cs
    │       ToDoTaskDto.cs
    │       UserDto.cs
    │
    ├───Mappings
    │       MappingProfile.cs
    │
    ├───Middlewares
    │       ErrorHandlingMiddleware.cs
    │       JwtMiddleware.cs
    │       RateLimitingMiddleware.cs
    │
    ├───Migrations
    │       MyToDoDbContextModelSnapshot.cs
    │
    ├───Models
    │       Role.cs
    │       ToDoList.cs
    │       ToDoTask.cs
    │       User.cs
    │       UserRole.cs
    │
    ├───Repositories
    │   │   ToDoListRepository.cs
    │   │   ToDoTaskRepository.cs
    │   │   UserRepository.cs
    │   │
    │   └───Interfaces
    │           IToDoListRepository.cs
    │           IToDoTaskRepository.cs
    │           IUserRepository.cs
    │
    ├───Services
    │       TokenService.cs
    │
    └───Settings
            JwtSettings.cs
```
---

## 🔐 Autenticazione, Autorizzazione e Sicurezza

- Autenticazione tramite **JWT**
- Gestione dei **ruoli** (es. Admin, User)
- CORS configurato per dominio in Azure
- Controllo degli accessi a livello di endpoint

## 🔐 Sicurezza applicativa

### JwtMiddleware
Middleware personalizzato per:
- Intercettare tutte le richieste entranti
- Estrarre il token JWT dall’header `Authorization`
- Validare firma, scadenza e integrità
- Impostare l’utente autenticato nel `HttpContext`

Questo garantisce che **solo gli utenti autenticati** possano accedere agli endpoint protetti.

---

### RateLimitingMiddleware (Protezione DDoS)
Middleware per:
- Limitare il numero di richieste per IP in una finestra temporale
- Prevenire abusi e flood di richieste
- Evitare saturazione della mia **base dati in Azure** e consumo eccessivo delle risorse

Esempio di configurazione:
```json
"RateLimitSettings": {
  "Limit": 5,
  "Period": 10
}
```
---

### 📌 API Endpoint

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
- `scriptDatabase.sql` → Script per creare il data base con le store procedure assoiciati

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
## 🧪 Test automatici (xUnit)
I test sono stati implementati per garantire il corretto funzionamento delle funzionalità principali e la sicurezza del sistema.

  ## Tipologie di test inclusi:
    - AuthTest → verifica la registrazione e login utente
    
    - TokenServiceTests → verifica:
    
        - Generazione corretta di token JWT
        
        - Validazione di token validi e rifiuto di token invalidi
    
    - JwtMiddlewareTests → verifica:
    
        - Richieste con token valido → HttpContext.User popolato
        
        - Richieste con token invalido → accesso negato
    
    - RateLimitingMiddlewareTests → verifica:
    
        - Richieste entro il limite → 200 OK
        
        - Richieste oltre il limite → 429 Too Many Requests
---
## ▶️ Avviare del test
```bash
dotnet test
```
---
## 🚧 Possibili miglioramenti futuri

- Logging avanzato (Serilog)
- Interfaccia frontend (es. Blazor, Angular o React)
- Dashboard amministrativa per gestione utenti e ruoli
- Deployment automatico (CI/CD) su Azure
- Rate limiting dinamico basato su ruoli
---

## 📄 Licenza

Distribuito a scopo didattico e valutativo. Nessuna licenza commerciale attiva.
