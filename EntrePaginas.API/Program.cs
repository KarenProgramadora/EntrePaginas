using EntrePaginas.API.Mappings;
using EntrePaginas.DataAccess.Context;
using EntrePaginas.DataAccess.Repositories;
using EntrePaginas.DataAccess.Seeders;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using EntrePaginas.Domain.Interfaces.Services;
using EntrePaginas.Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<EntrePaginasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
builder.Services.AddScoped<IGenericRepository<Publisher>, GenericRepository<Publisher>>();
builder.Services.AddScoped<IGenericRepository<Author>, GenericRepository<Author>>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IFineRepository, FineRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

// Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IFineService, FineService>();
builder.Services.AddScoped<ILoanService, LoanService>();

// Seeder
builder.Services.AddScoped<DataSeeder>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Entre Páginas API", Version = "v1" });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Ejecutar seeder al iniciar
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Entre Páginas API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAngular");
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
