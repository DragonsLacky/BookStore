using Model.Dtos.Creation;

namespace Services.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // CreateMap<AppUser, MemberDto>()
        //     .ForMember(
        //         dest => dest.PhotoUrl,
        //         opt =>
        //             opt.MapFrom(
        //                 src =>
        //                     src.Photos
        //                         .FirstOrDefault(photo => photo.IsMain)
        //                         .Url
        //             )
        //     )
        //     .ForMember(
        //         dest => dest.Age,
        //         opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge())
        //     );
        //
        // CreateMap<MemberUpdateDto, AppUser>();
        //
        // CreateMap<Photo, PhotoDto>();
        //
        CreateMap<RegisterDto, AppUser>();
        //
        // CreateMap<Message, MessageDto>()
        //     .ForMember(dest => dest.SenderPhotoUrl,
        //         opt => opt.MapFrom(src =>
        //             src.Sender.Photos.FirstOrDefault(ph => ph.IsMain).Url))
        //     .ForMember(dest => dest.RecipientPhotoUrl,
        //         opt => opt.MapFrom(src =>
        //             src.Recipient.Photos.FirstOrDefault(ph => ph.IsMain).Url));
        //
        CreateMap<DateTime, DateTime>().ConvertUsing(dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
        CreateMap<Author, AuthorDto>();
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author.Lastname));
        CreateMap<CreateBookDto, Book>()
            .ForMember(book => book.Author,
                opt => opt.MapFrom(src => new Author {Name = src.AuthorName, Lastname = src.AuthorLastName}));
        CreateMap<Comment, CommentDto>();
    }
}