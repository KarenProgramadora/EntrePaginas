using EntrePaginas.DataAccess.Context;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EntrePaginas.DataAccess.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(EntrePaginasDbContext context)
    {
        // Si ya hay categorías asumimos que la BD ya está poblada
        if (await context.Categories.AnyAsync()) return;

        await SeedCategoriesAsync(context);
        await SeedPublishersAsync(context);
        await SeedAuthorsAsync(context);
        await SeedBooksAsync(context);
        await SeedMembersAsync(context);
    }

    // ------------------------------------------------------------------ //

    private static async Task SeedCategoriesAsync(EntrePaginasDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new() { Name = "Novela",              Description = "Obras de ficción narrativa de largo aliento" },
            new() { Name = "Cuento",              Description = "Relatos breves de ficción" },
            new() { Name = "Poesía",              Description = "Composiciones en verso" },
            new() { Name = "Historia",            Description = "Obras sobre eventos y procesos históricos" },
            new() { Name = "Ciencia Ficción",     Description = "Ficción basada en avances científicos y tecnológicos" },
            new() { Name = "Ensayo",              Description = "Textos de reflexión y análisis sobre diversos temas" },
            new() { Name = "Biografía",           Description = "Narración de la vida de personas reales" },
            new() { Name = "Infantil y Juvenil",  Description = "Literatura dirigida a niños y jóvenes" },
        };

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPublishersAsync(EntrePaginasDbContext context)
    {
        if (await context.Publishers.AnyAsync()) return;

        var publishers = new List<Publisher>
        {
            new() { Name = "Alfaguara",       Country = "España",    Email = "info@alfaguara.com",     Website = "https://www.alfaguara.com" },
            new() { Name = "Seix Barral",     Country = "España",    Email = "info@seixbarral.com",    Website = "https://www.seixbarral.com" },
            new() { Name = "Editorial Planeta", Country = "España",  Email = "info@planeta.es",        Website = "https://www.planeta.es" },
            new() { Name = "Fondo de Cultura Económica", Country = "México", Email = "info@fce.com.mx", Website = "https://www.fondodeculturaeconomica.com" },
            new() { Name = "Norma",           Country = "Colombia",  Email = "info@norma.com",         Website = "https://www.norma.com" },
            new() { Name = "Anagrama",        Country = "España",    Email = "info@anagrama-ed.es",    Website = "https://www.anagrama-ed.es" },
        };

        context.Publishers.AddRange(publishers);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAuthorsAsync(EntrePaginasDbContext context)
    {
        if (await context.Authors.AnyAsync()) return;

        var authors = new List<Author>
        {
            new()
            {
                FirstName = "Gabriel",
                LastName = "García Márquez",
                Nationality = "Colombiana",
                BirthDate = new DateTime(1927, 3, 6),
                Biography = "Novelista, cuentista y periodista colombiano, ganador del Premio Nobel de Literatura en 1982. Máximo exponente del realismo mágico."
            },
            new()
            {
                FirstName = "Jorge Luis",
                LastName = "Borges",
                Nationality = "Argentina",
                BirthDate = new DateTime(1899, 8, 24),
                Biography = "Escritor argentino considerado uno de los autores más destacados de la literatura hispanoamericana y universal del siglo XX."
            },
            new()
            {
                FirstName = "Isabel",
                LastName = "Allende",
                Nationality = "Chilena",
                BirthDate = new DateTime(1942, 8, 2),
                Biography = "Escritora chilena, autora de la novela más leída en lengua española, La casa de los espíritus."
            },
            new()
            {
                FirstName = "Mario",
                LastName = "Vargas Llosa",
                Nationality = "Peruana",
                BirthDate = new DateTime(1936, 3, 28),
                Biography = "Escritor peruano ganador del Premio Nobel de Literatura en 2010. Figura central del Boom latinoamericano."
            },
            new()
            {
                FirstName = "Julio",
                LastName = "Cortázar",
                Nationality = "Argentina",
                BirthDate = new DateTime(1914, 8, 26),
                Biography = "Escritor y traductor argentino, renovador del cuento latinoamericano del siglo XX."
            },
            new()
            {
                FirstName = "Pablo",
                LastName = "Neruda",
                Nationality = "Chilena",
                BirthDate = new DateTime(1904, 7, 12),
                Biography = "Poeta chileno ganador del Premio Nobel de Literatura en 1971. Uno de los más influyentes poetas del siglo XX."
            },
            new()
            {
                FirstName = "Octavio",
                LastName = "Paz",
                Nationality = "Mexicana",
                BirthDate = new DateTime(1914, 3, 31),
                Biography = "Poeta y diplomático mexicano, ganador del Premio Nobel de Literatura en 1990."
            },
            new()
            {
                FirstName = "Laura",
                LastName = "Esquivel",
                Nationality = "Mexicana",
                BirthDate = new DateTime(1950, 9, 30),
                Biography = "Escritora mexicana, autora de la exitosa novela Como agua para chocolate."
            },
        };

        context.Authors.AddRange(authors);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBooksAsync(EntrePaginasDbContext context)
    {
        if (await context.Books.AnyAsync()) return;

        var categories  = await context.Categories.ToListAsync();
        var publishers  = await context.Publishers.ToListAsync();
        var authors     = await context.Authors.ToListAsync();

        int catNovela   = categories.First(c => c.Name == "Novela").Id;
        int catCuento   = categories.First(c => c.Name == "Cuento").Id;
        int catPoesia   = categories.First(c => c.Name == "Poesía").Id;
        int catHistoria = categories.First(c => c.Name == "Historia").Id;
        int catEnsayo   = categories.First(c => c.Name == "Ensayo").Id;

        int pubAlfaguara = publishers.First(p => p.Name == "Alfaguara").Id;
        int pubSeix      = publishers.First(p => p.Name == "Seix Barral").Id;
        int pubAnagrama  = publishers.First(p => p.Name == "Anagrama").Id;
        int pubFCE       = publishers.First(p => p.Name == "Fondo de Cultura Económica").Id;
        int pubNorma     = publishers.First(p => p.Name == "Norma").Id;

        int aGGM     = authors.First(a => a.LastName == "García Márquez").Id;
        int aBorges  = authors.First(a => a.LastName == "Borges").Id;
        int aAllende = authors.First(a => a.LastName == "Allende").Id;
        int aVargas  = authors.First(a => a.LastName == "Vargas Llosa").Id;
        int aCortazar = authors.First(a => a.LastName == "Cortázar").Id;
        int aNeruda  = authors.First(a => a.LastName == "Neruda").Id;
        int aPaz     = authors.First(a => a.LastName == "Paz").Id;
        int aEsquivel = authors.First(a => a.LastName == "Esquivel").Id;

        var books = new List<(Book book, int authorId)>
        {
            (new Book
            {
                Title = "Cien años de soledad",
                ISBN = "978-0-06-088328-7",
                PublicationYear = 1967,
                TotalCopies = 5,
                AvailableCopies = 5,
                Condition = BookCondition.Good,
                CategoryId = catNovela,
                PublisherId = pubAlfaguara
            }, aGGM),
            (new Book
            {
                Title = "El amor en los tiempos del cólera",
                ISBN = "978-0-14-028622-5",
                PublicationYear = 1985,
                TotalCopies = 4,
                AvailableCopies = 4,
                Condition = BookCondition.Good,
                CategoryId = catNovela,
                PublisherId = pubAlfaguara
            }, aGGM),
            (new Book
            {
                Title = "Ficciones",
                ISBN = "978-84-663-0407-4",
                PublicationYear = 1944,
                TotalCopies = 3,
                AvailableCopies = 3,
                Condition = BookCondition.Fair,
                CategoryId = catCuento,
                PublisherId = pubSeix
            }, aBorges),
            (new Book
            {
                Title = "El Aleph",
                ISBN = "978-84-206-8491-8",
                PublicationYear = 1949,
                TotalCopies = 3,
                AvailableCopies = 3,
                Condition = BookCondition.Good,
                CategoryId = catCuento,
                PublisherId = pubAlfaguara
            }, aBorges),
            (new Book
            {
                Title = "La casa de los espíritus",
                ISBN = "978-84-01-38103-5",
                PublicationYear = 1982,
                TotalCopies = 4,
                AvailableCopies = 4,
                Condition = BookCondition.New,
                CategoryId = catNovela,
                PublisherId = pubSeix
            }, aAllende),
            (new Book
            {
                Title = "La ciudad y los perros",
                ISBN = "978-84-322-0408-1",
                PublicationYear = 1963,
                TotalCopies = 3,
                AvailableCopies = 3,
                Condition = BookCondition.Good,
                CategoryId = catNovela,
                PublisherId = pubSeix
            }, aVargas),
            (new Book
            {
                Title = "Conversación en La Catedral",
                ISBN = "978-84-322-0714-3",
                PublicationYear = 1969,
                TotalCopies = 2,
                AvailableCopies = 2,
                Condition = BookCondition.Fair,
                CategoryId = catNovela,
                PublisherId = pubSeix
            }, aVargas),
            (new Book
            {
                Title = "Rayuela",
                ISBN = "978-84-204-8655-8",
                PublicationYear = 1963,
                TotalCopies = 4,
                AvailableCopies = 4,
                Condition = BookCondition.Good,
                CategoryId = catNovela,
                PublisherId = pubAlfaguara
            }, aCortazar),
            (new Book
            {
                Title = "Veinte poemas de amor y una canción desesperada",
                ISBN = "978-84-376-0283-7",
                PublicationYear = 1924,
                TotalCopies = 5,
                AvailableCopies = 5,
                Condition = BookCondition.New,
                CategoryId = catPoesia,
                PublisherId = pubFCE
            }, aNeruda),
            (new Book
            {
                Title = "El laberinto de la soledad",
                ISBN = "978-968-16-0250-3",
                PublicationYear = 1950,
                TotalCopies = 3,
                AvailableCopies = 3,
                Condition = BookCondition.Good,
                CategoryId = catEnsayo,
                PublisherId = pubFCE
            }, aPaz),
            (new Book
            {
                Title = "Como agua para chocolate",
                ISBN = "978-968-19-0419-9",
                PublicationYear = 1989,
                TotalCopies = 4,
                AvailableCopies = 4,
                Condition = BookCondition.New,
                CategoryId = catNovela,
                PublisherId = pubNorma
            }, aEsquivel),
            (new Book
            {
                Title = "El coronel no tiene quien le escriba",
                ISBN = "978-84-397-0524-4",
                PublicationYear = 1961,
                TotalCopies = 3,
                AvailableCopies = 3,
                Condition = BookCondition.Good,
                CategoryId = catNovela,
                PublisherId = pubAnagrama
            }, aGGM),
        };

        foreach (var (book, authorId) in books)
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();

            context.BookAuthors.Add(new BookAuthor
            {
                BookId = book.Id,
                AuthorId = authorId
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedMembersAsync(EntrePaginasDbContext context)
    {
        if (await context.Members.AnyAsync()) return;

        var members = new List<Member>
        {
            new()
            {
                FirstName = "Valentina",
                LastName = "Rodríguez",
                Email = "valentina.rodriguez@email.com",
                Phone = "3001234567",
                MembershipDate = DateTime.UtcNow.AddMonths(-12),
                Status = MemberStatus.Active
            },
            new()
            {
                FirstName = "Sebastián",
                LastName = "Martínez",
                Email = "sebastian.martinez@email.com",
                Phone = "3109876543",
                MembershipDate = DateTime.UtcNow.AddMonths(-8),
                Status = MemberStatus.Active
            },
            new()
            {
                FirstName = "Camila",
                LastName = "López",
                Email = "camila.lopez@email.com",
                Phone = "3205551234",
                MembershipDate = DateTime.UtcNow.AddMonths(-6),
                Status = MemberStatus.Active
            },
            new()
            {
                FirstName = "Andrés",
                LastName = "García",
                Email = "andres.garcia@email.com",
                Phone = "3154449876",
                MembershipDate = DateTime.UtcNow.AddMonths(-3),
                Status = MemberStatus.Active
            },
            new()
            {
                FirstName = "Lucía",
                LastName = "Hernández",
                Email = "lucia.hernandez@email.com",
                Phone = "3007778899",
                MembershipDate = DateTime.UtcNow.AddMonths(-18),
                Status = MemberStatus.Suspended
            },
            new()
            {
                FirstName = "Felipe",
                LastName = "Torres",
                Email = "felipe.torres@email.com",
                Phone = "3112223344",
                MembershipDate = DateTime.UtcNow.AddMonths(-2),
                Status = MemberStatus.Active
            },
            new()
            {
                FirstName = "Daniela",
                LastName = "Ramírez",
                Email = "daniela.ramirez@email.com",
                Phone = "3016665577",
                MembershipDate = DateTime.UtcNow.AddMonths(-9),
                Status = MemberStatus.Active
            },
            new()
            {
                FirstName = "Santiago",
                LastName = "Vargas",
                Email = "santiago.vargas@email.com",
                Phone = "3208883322",
                MembershipDate = DateTime.UtcNow.AddMonths(-1),
                Status = MemberStatus.Active
            },
        };

        context.Members.AddRange(members);
        await context.SaveChangesAsync();
    }
}
