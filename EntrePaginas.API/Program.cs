using EntrePaginas.DataAccess.Context;
using EntrePaginas.DataAccess.Repositories;
using EntrePaginas.DataAccess.Seeders;
using EntrePaginas.Domain.Helper;
using EntrePaginas.Domain.Interfaces.Repositories;
using EntrePaginas.Domain.Interfaces.Services;
using EntrePaginas.Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<EntrePaginasDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IFineRepository, FineRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IReportsRepository, ReportsRepository>();

// ── Services ──
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IFineService, FineService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<LoanValidationHelper>(); //Helper de validaciones reutilizables

// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ── Controllers ──
builder.Services.AddControllers();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── CORS (frontend Angular en localhost:4200) ──
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// ── Data Seeder ──
using (var scope = app.Services.CreateScope()) //Scoped, Singleton y Transient
{
    var context = scope.ServiceProvider.GetRequiredService<EntrePaginasDbContext>();

    await context.Database.MigrateAsync(); // Crea la BD + aplica migraciones
    await DataSeeder.SeedAsync(context);
}

// ── Middleware Pipeline ──
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
