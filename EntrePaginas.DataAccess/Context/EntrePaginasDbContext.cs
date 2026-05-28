using Microsoft.EntityFrameworkCore;
using EntrePaginas.Domain.Entities;

namespace EntrePaginas.DataAccess.Context;

public class EntrePaginasDbContext : DbContext
{
    public EntrePaginasDbContext(DbContextOptions<EntrePaginasDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Fine> Fines => Set<Fine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // -- Category --
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).HasMaxLength(300);
            entity.HasIndex(c => c.Name).IsUnique();
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.UpdatedAt).IsRequired(false);
        });

        // -- Publisher --
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Country).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Email).HasMaxLength(150);
            entity.Property(p => p.Website).HasMaxLength(250);
            entity.HasIndex(p => p.Name).IsUnique();
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.Property(p => p.UpdatedAt).IsRequired(false);
        });

        // -- Author --
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Nationality).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Biography).HasMaxLength(1000);
            entity.Property(a => a.BirthDate).IsRequired(false);
            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.UpdatedAt).IsRequired(false);
        });

        // -- Book --
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(250);
            entity.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
            entity.Property(b => b.PublicationYear).IsRequired();
            entity.Property(b => b.TotalCopies).IsRequired();
            entity.Property(b => b.AvailableCopies).IsRequired();
            entity.Property(b => b.Condition).IsRequired().HasConversion<int>();
            entity.Property(b => b.CoverUrl).HasMaxLength(500);
            entity.HasIndex(b => b.ISBN).IsUnique();
            entity.Property(b => b.CreatedAt).IsRequired();
            entity.Property(b => b.UpdatedAt).IsRequired(false);

            entity.HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(b => b.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // -- BookAuthor (N:M) --
        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasKey(ba => ba.Id);
            entity.Property(ba => ba.CreatedAt).IsRequired();
            entity.Property(ba => ba.UpdatedAt).IsRequired(false);
            entity.HasIndex(ba => new { ba.BookId, ba.AuthorId }).IsUnique();

            entity.HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // -- Member --
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(m => m.LastName).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(150);
            entity.Property(m => m.Phone).IsRequired().HasMaxLength(20);
            entity.Property(m => m.MembershipDate).IsRequired();
            entity.Property(m => m.Status).IsRequired().HasConversion<int>();
            entity.HasIndex(m => m.Email).IsUnique();
            entity.Property(m => m.CreatedAt).IsRequired();
            entity.Property(m => m.UpdatedAt).IsRequired(false);
        });

        // -- Loan --
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.LoanDate).IsRequired();
            entity.Property(l => l.DueDate).IsRequired();
            entity.Property(l => l.ReturnDate).IsRequired(false);
            entity.Property(l => l.Status).IsRequired().HasConversion<int>();
            entity.Property(l => l.CreatedAt).IsRequired();
            entity.Property(l => l.UpdatedAt).IsRequired(false);

            entity.HasOne(l => l.Book)
                .WithMany(b => b.Loans)
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.Member)
                .WithMany(m => m.Loans)
                .HasForeignKey(l => l.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // -- Fine (1:1 con Loan) --
        modelBuilder.Entity<Fine>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Amount).IsRequired().HasColumnType("decimal(10,2)");
            entity.Property(f => f.IssuedDate).IsRequired();
            entity.Property(f => f.PaidDate).IsRequired(false);
            entity.Property(f => f.IsPaid).IsRequired();
            entity.Property(f => f.Notes).HasMaxLength(500);
            entity.Property(f => f.CreatedAt).IsRequired();
            entity.Property(f => f.UpdatedAt).IsRequired(false);

            entity.HasOne(f => f.Loan)
                .WithOne(l => l.Fine)
                .HasForeignKey<Fine>(f => f.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
