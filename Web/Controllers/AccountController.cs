using Microsoft.AspNetCore.Authorization;
using Model.Dtos.Creation;
using Services.Services.Interfaces;

namespace Web.Controllers;

public class AccountController : BaseApiController
{
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        ITokenService tokenService, IMapper mapper, IUserService userService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserAuthDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName)) return BadRequest("Username is already taken");

        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.UserName.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var roleResult = await _userManager.AddToRoleAsync(user, Enum.GetName(AppRoleEnum.User));

        if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

        return new UserAuthDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserAuthDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .Include(user => user.Photos)
            .SingleOrDefaultAsync(user => user.UserName == loginDto.userName.ToLower());

        if (user == null) return Unauthorized("Invalid username");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Invalid Password");

        return new UserAuthDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(photo => photo.IsMain)?.Url,
        };
    }

    [HttpGet]
    public async Task<List<AppUser>> GetUsers()
    {
        return await _userManager.Users.ToListAsync();
    }

    [Authorize]
    [HttpPost("cart/add/{isbn}")]
    public async Task<ActionResult<bool>> AddToCart(string isbn)
    {
        var username = User.GetUsername();
        return await _userService.AddBookToCart(isbn, username);
    }

    [Authorize]
    [HttpPost("cart/remove/{isbn}")]
    public async Task<ActionResult<bool>> RemoveFromCart(string isbn)
    {
        var username = User.GetUsername();
        return await _userService.RemoveBookFromCart(isbn, username);
    }

    [Authorize]
    [HttpGet("owned")]
    public async Task<ActionResult<ICollection<BookDto>>> GetOwnedBooks()
    {
        return Ok(await _userService.GetOwnedBooks(User.GetUserId()));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> BuyBooksInCart()
    {
        if (await _userService.BuyFromCart(User.GetUserId()))
        {
            return Ok();
        }

        return BadRequest("Could not buy the books");
    }

    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());
    }
}