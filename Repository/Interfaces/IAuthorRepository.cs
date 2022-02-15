namespace Repository.Interfaces;

public interface IAuthorRepository
{
    public Task<ICollection<AuthorDto>> GetAuthorsAsync();
}