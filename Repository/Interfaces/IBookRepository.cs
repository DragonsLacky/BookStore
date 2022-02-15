using Model.Dtos.Creation;
using Model.Helpers;

namespace Repository.Interfaces;

public interface IBookRepository
{
    public Task<PagedList<BookDto>> GetBooksPagedAsync(BookParams bookParams);
    public Task<BookDto> GetBookByIsbnClientAsync(string bookIsbn);
    public Task<Book> GetBookByIsbnAsync(string bookIsbn);
    public Task<Book> GetBookByIdAsync(int bookId);
    public void AddBook(Book book);
    public void DeleteBook(Book book);
    public Task<PagedList<CommentDto>> GetCommentsForBook(CommentParams commentParams);

    public Task<bool> AddComment(CreateCommentDto createCommentDto);
}