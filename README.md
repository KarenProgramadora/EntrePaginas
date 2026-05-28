# Entre Páginas — Sistema de Gestión de Biblioteca

Proyecto final para la materia **APIs Web**. Sistema de gestión de biblioteca con backend en .NET 8 y frontend en Angular 17+.

## Tecnologías

| Capa | Tecnología |
|---|---|
| Backend | .NET 8 Web API, Entity Framework Core 8, AutoMapper, Swagger |
| Base de datos | SQL Server Express |
| Frontend | Angular 17+, Angular Material (M3) |

## Estructura del proyecto

```
EntrePaginas/
├── EntrePaginas.Domain/        # Entidades, interfaces, servicios de dominio
├── EntrePaginas.DataAccess/    # DbContext, repositorios, migraciones, seeder
├── EntrePaginas.API/           # Controllers, DTOs, mappings, Program.cs
└── entrepaginas-frontend/      # Aplicación Angular
```

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
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

Aplicar migraciones:

```bash
cd EntrePaginas.DataAccess
dotnet ef database update --startup-project ../EntrePaginas.API
```

La base de datos se puebla automáticamente con datos de prueba al iniciar el API por primera vez (DataSeeder).

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
- **Préstamos** — Crear préstamo, registrar devolución, marcar vencidos
- **Dashboard** — Estadísticas generales, libros por categoría, más prestados
