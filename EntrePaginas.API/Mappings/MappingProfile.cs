using AutoMapper;
using EntrePaginas.API.DTOs.Request;
using EntrePaginas.API.DTOs.Response;
using EntrePaginas.Domain.Entities;
using EntrePaginas.Domain.Enums;
using EntrePaginas.Domain.Interfaces.Repositories;

namespace EntrePaginas.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Category
        CreateMap<CategoryRequestDTO, Category>();
        CreateMap<Category, CategoryResponseDTO>();

        // Publisher
        CreateMap<PublisherRequestDTO, Publisher>();
        CreateMap<Publisher, PublisherResponseDTO>();

        // Author
        CreateMap<AuthorRequestDTO, Author>();
        CreateMap<Author, AuthorResponseDTO>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        // Book
        CreateMap<BookRequestDTO, Book>();
        CreateMap<Book, BookResponseDTO>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForMember(dest => dest.PublisherName,
                opt => opt.MapFrom(src => src.Publisher != null ? src.Publisher.Name : string.Empty))
            .ForMember(dest => dest.ConditionName,
                opt => opt.MapFrom(src => ToConditionName(src.Condition)));

        // Member
        CreateMap<MemberRequestDTO, Member>();
        CreateMap<Member, MemberResponseDTO>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.StatusName,
                opt => opt.MapFrom(src => ToMemberStatusName(src.Status)));

        // Fine
        CreateMap<FineRequestDTO, Fine>();
        CreateMap<Fine, FineResponseDTO>();

        // Loan
        CreateMap<Loan, LoanResponseDTO>()
            .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty))
            .ForMember(dest => dest.MemberFullName,
                opt => opt.MapFrom(src => src.Member != null
                    ? $"{src.Member.FirstName} {src.Member.LastName}"
                    : string.Empty))
            .ForMember(dest => dest.StatusName,
                opt => opt.MapFrom(src => ToLoanStatusName(src.Status)))
            .ForMember(dest => dest.IsOverdue,
                opt => opt.MapFrom(src =>
                    src.Status == LoanStatus.Overdue ||
                    (src.Status == LoanStatus.Active && src.DueDate < DateTime.UtcNow)));

        // Reports — mapeo desde records crudos del repo hacia DTOs de respuesta
        CreateMap<LibraryStatsRaw, LibraryStatsDTO>();
        CreateMap<BooksByCategoryRaw, BooksByCategoryDTO>();
        CreateMap<MostLoanedBookRaw, MostLoanedBookDTO>();
        CreateMap<MemberActivityRaw, MemberActivityDTO>();
        CreateMap<LoanDueSoonRaw, LoanDueSoonDTO>();
    }

    private static string ToConditionName(BookCondition c) => c switch
    {
        BookCondition.New  => "Nuevo",
        BookCondition.Good => "Bueno",
        BookCondition.Fair => "Regular",
        BookCondition.Poor => "Malo",
        _                  => c.ToString()
    };

    private static string ToMemberStatusName(MemberStatus s) => s switch
    {
        MemberStatus.Active    => "Activo",
        MemberStatus.Suspended => "Suspendido",
        MemberStatus.Expired   => "Expirado",
        _                      => s.ToString()
    };

    private static string ToLoanStatusName(LoanStatus s) => s switch
    {
        LoanStatus.Active   => "Activo",
        LoanStatus.Returned => "Devuelto",
        LoanStatus.Overdue  => "Vencido",
        _                   => s.ToString()
    };
}
