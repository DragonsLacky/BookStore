using Model.Dtos.Creation;
using Model.Helpers;

namespace Repository;

public class BookRepository : IBookRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public BookRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedList<BookDto>> GetBooksPagedAsync(BookParams bookParams)
    {
        var query = _context.Books
            .AsQueryable()
            .Where(book =>
                book.PublicationDate >= bookParams.DateFromPub && book.PublicationDate <= bookParams.DateToPub)
            .Where(book => book.Pages >= bookParams.MinPages && book.Pages <= bookParams.MaxPages)
            .Where(book => book.AuthorId == bookParams.AuthorId)
            .OrderByField(bookParams.OrderBy, bookParams.Ascending)
            .ProjectTo<BookDto>(_mapper.ConfigurationProvider);

        return await PagedList<BookDto>.CreateAsync(query, bookParams.PageNumber, bookParams.PageSize);
    }

    public async Task<Book> GetBookByIsbnAsync(string bookIsbn)
    {
        return await _context.Books.SingleOrDefaultAsync(book => book.ISBN == bookIsbn) ??
               throw new Exception("Could not find the book you were looking for");
    }

    public async Task<BookDto> GetBookByIsbnClientAsync(string bookIsbn)
    {
        return await _context.Books
                   .Include(book => book.Author)
                   .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                   .SingleOrDefaultAsync(book => book.ISBN == bookIsbn) ??
               throw new Exception("Book with the given isbn could not be found");
    }

    public async Task<Book> GetBookByIdAsync(int bookId)
    {
        return await _context.Books
                   .Include(book => book.Author)
                   .SingleOrDefaultAsync(book => book.Id == bookId) ??
               throw new Exception("Book with the given id could not be found");
    }

    public void AddBook(Book book)
    {
        _context.Authors.Add(book.Author);
        // _context.SaveChangesAsync();

        _context.Books.Add(book);
    }

    public void DeleteBook(Book book)
    {
        _context.Books.Remove(book);
    }

    public async Task<PagedList<CommentDto>> GetCommentsForBook(CommentParams commentParams)
    {
        var query = _context.Books.AsQueryable()
            .Where(b => b.ISBN == commentParams.Isnb)
            .Include(b => b.Comments)
            .SelectMany(b => b.Comments!)
            .OrderByField(commentParams.OrderBy, commentParams.Ascending)
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

        return await PagedList<CommentDto>.CreateAsync(query, commentParams.PageNumber, commentParams.PageSize);
    }

    public async Task<bool> AddComment(CreateCommentDto createCommentDto)
    {
        var commenter =
            await _context.Users.SingleOrDefaultAsync(user => user.UserName == createCommentDto.CommenterUsername);
        if (commenter == null) throw new Exception("Can not find a user with the specified username");

        var book = await _context.Books.SingleOrDefaultAsync(book => book.ISBN == createCommentDto.Isbn);

        if (book == null) throw new Exception("Can not find a book with the specified ISBN");


        var comment = new Comment
        {
            Content = createCommentDto.Content,
            Commenter = commenter,
            CommenterUsername = commenter.UserName,
            BookISBN = createCommentDto.Isbn,
            Book = book,
            BookId = book.Id,
            Rating = createCommentDto.Rating,
            CommenterId = commenter.Id
        };

        var result = await _context.Comments.AddAsync(comment);

        return await Task.FromResult(true);
    }
}