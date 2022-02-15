namespace Repository;

public class AuthorRepository : IAuthorRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public AuthorRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<ICollection<AuthorDto>> GetAuthorsAsync()
    {
        return await _dataContext.Authors.ProjectTo<AuthorDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
}