using EntrePaginas.Domain.Entities;

namespace EntrePaginas.Domain.Interfaces.Repositories;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByIsbnAsync(string isbn);
    Task<IEnumerable<Book>> GetAllWithDetailsAsync();
    Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Book>> GetByPublisherAsync(int publisherId);
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
    Task<Book?> GetByIdWithDetailsAsync(int id);
}
