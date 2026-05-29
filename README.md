# Entre Páginas — Sistema de Gestión de Biblioteca

Proyecto final para la materia **APIs Web**. Sistema de gestión de biblioteca con backend en .NET y frontend en Angular. Sigue los mismos patrones y arquitectura que el proyecto guía **SportsLeague** (Fases 1–6 + F1).

## Tecnologías

| Capa | Tecnología |
|---|---|
| Backend | .NET Web API, Entity Framework Core 8, AutoMapper, Swagger |
| Base de datos | SQL Server Express |
| Frontend | Angular 21+, Angular Material |

## Estructura del proyecto

```
EntrePaginas/
├── EntrePaginas.Domain/        # Entidades, enums, helpers, interfaces, servicios de dominio
│   ├── Entities/
│   ├── Enums/
│   ├── Helper/                 # Validaciones reutilizables (LoanValidationHelper)
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   └── Services/
│   └── Services/
├── EntrePaginas.DataAccess/    # DbContext, repositorios, migraciones, seeder
│   ├── Context/
│   ├── Migrations/
│   ├── Repositories/
│   └── Seeders/                # DataSeeder estático
├── EntrePaginas.API/           # Controllers, DTOs, mappings, Program.cs
│   ├── Controllers/
│   ├── DTOs/
│   │   ├── Request/
│   │   └── Response/
│   ├── Mappings/
│   ├── Middlewares/            # Placeholder para middlewares futuros
│   └── Program.cs
└── entrepaginas-frontend/      # Aplicación Angular
    └── src/app/
        ├── core/               # ApiService, NotificationService, modelos, interceptors
        │   ├── guards/
        │   ├── interceptors/
        │   ├── models/         # Un archivo por entidad: book.model.ts, etc.
        │   └── services/       # <entidad>.service.ts usando ApiService
        ├── features/           # Páginas por dominio (books, authors, members, loans, dashboard)
        ├── layout/             # navbar, sidebar, footer
        └── shared/             # Componentes y material.imports.ts
```

## Convenciones de código

- **Namespaces C#**: file-scoped namespaces (`namespace X;`) en todo el proyecto.
- **Repositorios**: el genérico se registra con el patrón open-generic (`AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))`). Los repositorios con queries específicas (Book, Member, Loan, Fine, Reports) tienen interfaz y clase concreta propias.
- **Servicios de dominio**: todos consumen `ILogger<T>` y exponen su contrato vía interfaz.
- **Validaciones**: lógicas reutilizables viven en `Domain/Helper/` (ej. `LoanValidationHelper`).
- **DTOs**: `Request/` y `Response/` planos, sin subcarpetas. Cada endpoint devuelve un DTO tipado (nada de `object` ni anónimos).
- **AutoMapper**: perfiles registrados por assembly (`AddAutoMapper(typeof(Program).Assembly)`).
- **Seeder**: clase estática invocada tras `MigrateAsync` en `Program.cs`.
- **Frontend services**: usan `inject(ApiService)`; los componentes consumen el service, no `HttpClient` directo.
- **Notificaciones**: el `errorInterceptor` muestra el error al usuario vía `NotificationService`.

## Requisitos previos

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express](https://www.microsoft.com/es-ar/sql-server/sql-server-downloads)
- [Node.js 20+](https://nodejs.org/) y Angular CLI (`npm install -g @angular/cli`)

## Configuración inicial

### 1. Base de datos

Editar la cadena de conexión en `EntrePaginas.API/appsettings.json` si la instancia de SQL Server es diferente:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=EntrePaginasDb;Trusted_Connection=true;TrustServerCertificate=true"
}
```

Aplicar migraciones (también se aplican automáticamente al iniciar la API):

```bash
cd EntrePaginas.DataAccess
dotnet ef database update --startup-project ../EntrePaginas.API
```

La base de datos se puebla con datos de prueba al iniciar el API por primera vez (`DataSeeder.SeedAsync`).

### 2. Backend

```bash
cd EntrePaginas.API
dotnet run --launch-profile http
```

La API queda disponible en `http://localhost:5143`. Swagger en `http://localhost:5143/swagger`.

### 3. Frontend

```bash
cd entrepaginas-frontend
npm install
ng serve
```

La aplicación queda disponible en `http://localhost:4200`.

## Funcionalidades

- **Libros** — CRUD completo con categoría, editorial y condición física
- **Autores** — CRUD con asociación a libros
- **Miembros** — CRUD con estados (Activo / Suspendido / Expirado)
- **Préstamos** — Crear préstamo, registrar devolución, marcar vencidos (con validaciones vía `LoanValidationHelper`)
- **Reportes** — Estadísticas generales, libros por categoría, más prestados, actividad de miembros, próximos a vencer
- **Dashboard** — Panel resumen con tarjetas de métricas y tablas
