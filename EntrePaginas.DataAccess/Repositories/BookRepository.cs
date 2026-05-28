using Microsoft.EntityFrameworkCore;
using EntrePaginas.DataAccess.Context;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;

namespace EntrePaginas.DataAccess.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(EntrePaginasDbContext context) : base(context) { }

    public async Task<Book?> GetByIsbnAsync(string isbn)
        => await _dbSet.FirstOrDefaultAsync(b => b.ISBN.ToLower() == isbn.ToLower());

    public async Task<IEnumerable<Book>> GetAllWithDetailsAsync()
        => await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
            .ToListAsync();

    public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
        => await _dbSet.Where(b => b.CategoryId == categoryId).ToListAsync();

    public async Task<IEnumerable<Book>> GetByPublisherAsync(int publisherId)
        => await _dbSet.Where(b => b.PublisherId == publisherId).ToListAsync();

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        => await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .Where(b => b.AvailableCopies > 0)
            .ToListAsync();

    public async Task<Book?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
}
