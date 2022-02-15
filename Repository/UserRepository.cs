using System.Net.Mime;

namespace Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<UserDto> GetMemberByUsername(string username)
    {
        return await _context.Users
            .Where(user => user.UserName == username)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync() ?? throw new Exception("There was a problem getting the user");
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id) ?? throw new Exception("Could not find the specified user");
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
                   .Include(user => user.Photos)
                   .SingleOrDefaultAsync(user => user.UserName == username) ??
               throw new Exception("Could not find a user with the given username");
    }

    public async Task<UserDto> GetUserByUsernameClientAsync(string username)
    {
        return await _context.Users
            .Where(user => user.UserName == username)
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync() ?? throw new Exception("Not Found");
    }

    public async Task<PagedList<UserDto>> GetUsersClientAsync(UserParams userParams)
    {
        var query = _context.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider).AsNoTracking();
        return await PagedList<UserDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
    }

    public async Task<PagedList<CommentDto>> GetCommentsForBook(CommentParams commentParams)
    {
        var query = _context.Users.AsQueryable()
            .Where(u => u.UserName == commentParams.Username)
            .Include(u => u.Comments)
            .SelectMany(u => u.Comments!)
            .OrderByField("New", true)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<CommentDto>.CreateAsync(query, commentParams.PageNumber, commentParams.PageSize);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
            .Include(user => user.Photos)
            .ToListAsync();
    }

    public async Task<ICollection<BookDto>> GetCartBooks(string username)
    {
        return await _context.Carts
            .Where(c => c.Username == username)
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async void AddToCart(Book book, string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("No user with the specified username");
        user.Cart.Books.Add(book);
    }

    public async void RemoveFromCart(Book book, string userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new Exception("No user with the specified username");
        user.Cart.Books.Remove(book);
    }

    public async Task<ICollection<BookDto>> GetOwnedBooksForUser(int userId)
    {
        return await _context.Users.AsQueryable()
            .Where(user => user.Id == userId)
            .Include(u => u.OwnedBooks)
            .SelectMany(u => u.OwnedBooks)
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}