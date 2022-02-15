using Model.Dtos.Creation;
using Model.Entities;
using Model.Helpers;
using Model.Params;
using Repository;

namespace Web.Controllers;

public class BookController : BaseApiController
{
    private readonly IRepositoryUnit _repositoryUnit;
    private readonly IMapper _mapper;

    public BookController(IRepositoryUnit repositoryUnit, IMapper mapper)
    {
        _repositoryUnit = repositoryUnit;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<BookDto>>> GetBooks([FromQuery] BookParams bookParams)
    {
        var books = await _repositoryUnit.BookRepository.GetBooksPagedAsync(bookParams);

        Response.AddPaginationHeader(books.CurrentPage, books.PageSize, books.TotalCount, books.TotalPages);

        return Ok(books);
    }

    [HttpGet("{isbn}")]
    public async Task<ActionResult<BookDto>> GetSingleBook(string isbn)
    {
        return Ok(await _repositoryUnit.BookRepository.GetBookByIsbnAsync(isbn));
    }

    [HttpPost("/Comment")]
    public async Task<ActionResult<bool>> AddComment(CreateCommentDto createCommentDto)
    {
        createCommentDto.CommenterUsername = User.GetUsername();
        await _repositoryUnit.BookRepository.AddComment(createCommentDto);
        return await _repositoryUnit.SaveChangesAsync();
    }

    [HttpGet("/comment")]
    public async Task<ActionResult<ICollection<CommentDto>>> GetCommentsForBook([FromQuery] CommentParams commentParams)
    {
        commentParams.Username = User.GetUsername();
        var comments = await _repositoryUnit.BookRepository.GetCommentsForBook(commentParams);

        Response.AddPaginationHeader(comments.CurrentPage, comments.PageSize, comments.TotalCount, comments.TotalPages);

        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult> AddBook(CreateBookDto createBookDto)
    {
        var book = _mapper.Map<Book>(createBookDto);
        _repositoryUnit.BookRepository.AddBook(book);
        if (await _repositoryUnit.SaveChangesAsync())
        {
            return Ok();
        }

        return BadRequest("Something went wrong");
    }
}