using Model.Dtos.Creation;
using Model.Params;
using Repository.Helpers;

namespace Services.Services.Interfaces;

public interface IUserService
{
    public Task<bool> AddBookToCart(string isbn, string username);
    public Task<bool> RemoveBookFromCart(string isbn, string username);
    public Task<ICollection<BookDto>> GetUserBooks(string username);
    public Task<PagedList<CommentDto>> GetUserComments(CommentParams commentParams);

    public Task<ICollection<BookDto>> GetOwnedBooks(int userId);

    public Task<bool> BuyFromCart(int userId);
}