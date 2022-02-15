namespace Repository.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<AppUser> GetUserByIdAsync(int id);

    Task<AppUser> GetUserByUsernameAsync(string username);

    Task<UserDto> GetUserByUsernameClientAsync(string username);
    Task<PagedList<UserDto>> GetUsersClientAsync(UserParams userParams);

    public Task<PagedList<CommentDto>> GetCommentsForBook(CommentParams commentParams);

    public Task<ICollection<BookDto>> GetCartBooks(string username);

    public void AddToCart(Book book, string username);

    public void RemoveFromCart(Book book, string username);

    public Task<ICollection<BookDto>> GetOwnedBooksForUser(int userId);
}