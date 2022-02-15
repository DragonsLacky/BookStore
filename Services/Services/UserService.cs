using Model.Params;
using Repository.Helpers;
using Services.Services.Interfaces;

namespace Services.Services;

public class UserService : IUserService
{
    private readonly IRepositoryUnit _repositoryUnit;
    private readonly IMapper _mapper;

    public UserService(IRepositoryUnit repositoryUnit, IMapper mapper)
    {
        _repositoryUnit = repositoryUnit;
        _mapper = mapper;
    }

    public async Task<bool> AddBookToCart(string isbn, string username)
    {
        Book book = await _repositoryUnit.BookRepository.GetBookByIsbnAsync(isbn);
        _repositoryUnit.UserRepository.AddToCart(book, username);
        return await _repositoryUnit.SaveChangesAsync();
    }

    public async Task<bool> RemoveBookFromCart(string isbn, string username)
    {
        Book book = await _repositoryUnit.BookRepository.GetBookByIsbnAsync(isbn);
        _repositoryUnit.UserRepository.RemoveFromCart(book, username);
        return await _repositoryUnit.SaveChangesAsync();
    }

    public async Task<ICollection<BookDto>> GetUserBooks(string username)
    {
        return await _repositoryUnit.UserRepository.GetCartBooks(username);
    }

    public async Task<PagedList<CommentDto>> GetUserComments(CommentParams commentParams)
    {
        return await _repositoryUnit.UserRepository.GetCommentsForBook(commentParams);
    }

    public async Task<ICollection<BookDto>> GetOwnedBooks(int userId)
    {
        return await _repositoryUnit.UserRepository.GetOwnedBooksForUser(userId);
    }

    public async Task<bool> BuyFromCart(int userId)
    {
        var user = await _repositoryUnit.UserRepository.GetUserByIdAsync(userId);
        user.Cart.Books.ToList().ForEach(book => user.OwnedBooks.Add(book));
        user.Cart.Books.Clear();
        return await _repositoryUnit.SaveChangesAsync();
    }
}