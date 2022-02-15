namespace Repository.Interfaces;

public interface IRepositoryUnit
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
    IBookRepository BookRepository { get; }
    
    Task<bool> SaveChangesAsync();

    bool HasChanges();
}