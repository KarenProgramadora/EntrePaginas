using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Enums;
using EntrePaginas.Domain.Interfaces.Repositories;

namespace EntrePaginas.Domain.Helper
{
    public class LoanValidationHelper
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;

        public LoanValidationHelper(
            IMemberRepository memberRepository,
            IBookRepository bookRepository,
            ILoanRepository loanRepository)
        {
            _memberRepository = memberRepository;
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
        }

        public async Task<Member> ValidateActiveMemberAsync(int memberId)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);

            if (member == null)
                throw new KeyNotFoundException(
                    $"No se encontró el miembro con ID {memberId}");

            if (member.Status != MemberStatus.Active)
                throw new InvalidOperationException(
                    $"El miembro '{member.FirstName} {member.LastName}' no puede tomar préstamos (estado: {member.Status})");

            return member;
        }

        public async Task<Book> ValidateAvailableBookAsync(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);

            if (book == null)
                throw new KeyNotFoundException(
                    $"No se encontró el libro con ID {bookId}");

            if (book.AvailableCopies <= 0)
                throw new InvalidOperationException(
                    $"El libro '{book.Title}' no tiene copias disponibles");

            return book;
        }

        public async Task ValidateNoDuplicateActiveLoanAsync(int memberId, int bookId, string bookTitle)
        {
            var hasActive = await _loanRepository.HasActiveLoanForBookAsync(memberId, bookId);
            if (hasActive)
                throw new InvalidOperationException(
                    $"El miembro ya tiene un préstamo activo de '{bookTitle}'");
        }

        public static void ValidateLoanDays(int loanDays)
        {
            // Rango permitido por la política de la biblioteca
            if (loanDays < 1 || loanDays > 30)
                throw new InvalidOperationException(
                    "El plazo del préstamo debe estar entre 1 y 30 días");
        }
    }
}
