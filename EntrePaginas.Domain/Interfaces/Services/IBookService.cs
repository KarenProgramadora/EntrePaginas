using EntrePaginas.Domain.Entities;

namespace EntrePaginas.Domain.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> GetAllWithDetailsAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByIdWithDetailsAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task UpdateAsync(int id, Book book);
    Task DeleteAsync(int id);
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
}
