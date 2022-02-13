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

    public async Task<MemberDto> GetMemberByUsername(string username)
    {
        return await _context.Users
            .Where(user => user.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
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

    public async Task<string> GetUserGender(string username)
    {
        return await _context.Users
            .Where(u => u.UserName == username)
            .Select(u => u.Gender)
            .FirstOrDefaultAsync() ?? throw new Exception("Something went wrong when querying database for users  ");
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
            .Include(user => user.Photos)
            .ToListAsync();
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}