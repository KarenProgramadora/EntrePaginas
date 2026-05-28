using Microsoft.Extensions.Logging;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Interfaces.Repositories;
using EntrePaginas.Domain.Interfaces.Services;

namespace EntrePaginas.Domain.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<BookService> _logger;

    public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all books");
        return await _bookRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Book>> GetAllWithDetailsAsync()
    {
        _logger.LogInformation("Retrieving all books with details");
        return await _bookRepository.GetAllWithDetailsAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving book with ID: {BookId}", id);
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            _logger.LogWarning("Book with ID {BookId} not found", id);
        return book;
    }

    public async Task<Book?> GetByIdWithDetailsAsync(int id)
    {
        _logger.LogInformation("Retrieving book with details, ID: {BookId}", id);
        return await _bookRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<Book> CreateAsync(Book book)
    {
        var existing = await _bookRepository.GetByIsbnAsync(book.ISBN);
        if (existing != null)
        {
            _logger.LogWarning("Book with ISBN '{ISBN}' already exists", book.ISBN);
            throw new InvalidOperationException($"Ya existe un libro con el ISBN '{book.ISBN}'");
        }

        if (book.AvailableCopies > book.TotalCopies)
            throw new InvalidOperationException("Las copias disponibles no pueden superar el total de copias");

        _logger.LogInformation("Creating book: {Title}", book.Title);
        return await _bookRepository.CreateAsync(book);
    }

    public async Task UpdateAsync(int id, Book book)
    {
        var existing = await _bookRepository.GetByIdAsync(id);
        if (existing == null)
        {
            _logger.LogWarning("Book with ID {BookId} not found for update", id);
            throw new KeyNotFoundException($"No se encontró el libro con ID {id}");
        }

        if (existing.ISBN != book.ISBN)
        {
            var bookWithSameIsbn = await _bookRepository.GetByIsbnAsync(book.ISBN);
            if (bookWithSameIsbn != null)
                throw new InvalidOperationException($"Ya existe un libro con el ISBN '{book.ISBN}'");
        }

        if (book.AvailableCopies > book.TotalCopies)
            throw new InvalidOperationException("Las copias disponibles no pueden superar el total de copias");

        existing.Title = book.Title;
        existing.ISBN = book.ISBN;
        existing.PublicationYear = book.PublicationYear;
        existing.TotalCopies = book.TotalCopies;
        existing.AvailableCopies = book.AvailableCopies;
        existing.Condition = book.Condition;
        existing.CoverUrl = book.CoverUrl;
        existing.CategoryId = book.CategoryId;
        existing.PublisherId = book.PublisherId;

        _logger.LogInformation("Updating book with ID: {BookId}", id);
        await _bookRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _bookRepository.ExistsAsync(id);
        if (!exists)
        {
            _logger.LogWarning("Book with ID {BookId} not found for deletion", id);
            throw new KeyNotFoundException($"No se encontró el libro con ID {id}");
        }

        _logger.LogInformation("Deleting book with ID: {BookId}", id);
        await _bookRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        _logger.LogInformation("Retrieving available books");
        return await _bookRepository.GetAvailableBooksAsync();
    }
}
