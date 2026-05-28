using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EntrePaginas.DataAccess.Context;

public class EntrePaginasDbContextFactory : IDesignTimeDbContextFactory<EntrePaginasDbContext>
{
    public EntrePaginasDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EntrePaginasDbContext>();
        optionsBuilder.UseSqlServer(
            @"Server=localhost\SQLEXPRESS;Database=EntrePaginasDb;Trusted_Connection=true;TrustServerCertificate=true");
        return new EntrePaginasDbContext(optionsBuilder.Options);
    }
}
