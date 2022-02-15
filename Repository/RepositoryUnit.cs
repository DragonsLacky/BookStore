namespace Repository;

public class RepositoryUnit : IRepositoryUnit
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public RepositoryUnit(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public IUserRepository UserRepository => new UserRepository(_context, _mapper);

    public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

    public IBookRepository BookRepository => new BookRepository(_context, _mapper);

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}