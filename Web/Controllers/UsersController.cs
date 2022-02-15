using Model.Entities;
using Model.Params;
using Services.Services.Interfaces;

namespace Web.Controllers;

public class UsersController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly IRepositoryUnit _repositoryUnit;

    public UsersController(IRepositoryUnit repositoryUnit, IMapper mapper, IPhotoService photoService)
    {
        _repositoryUnit = repositoryUnit;
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        userParams.CurrentUsername = User.GetUsername();

        var users = await _repositoryUnit.UserRepository.GetUsersClientAsync(userParams);

        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

        return Ok(users);
    }

    [HttpGet("{username}", Name = "GetUser")]
    public async Task<ActionResult<UserDto>> GetSingleUser(string username)
    {
        return await _repositoryUnit.UserRepository.GetUserByUsernameClientAsync(username);
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> UploadPhoto(IFormFile file)
    {
        var gender = await _repositoryUnit.UserRepository.GetUserByUsernameAsync(User.GetUsername());
        var result = await _photoService.AddPhotoToCloudAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);
        var photo = new UserPhoto()
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (gender.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        gender.Photos.Add(photo);

        if (await _repositoryUnit.SaveChangesAsync())
        {
            return CreatedAtRoute("GetUser", new {username = gender.UserName}, _mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("There was a problem uploading the photo");
    }

    [HttpPatch("photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var gender = await _repositoryUnit.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = gender.Photos.FirstOrDefault(photo => photo.Id == photoId);

        if (photo != null && photo.IsMain) return BadRequest("Photo is already your main photo");

        var currentMain = gender.Photos.FirstOrDefault(userPhoto => userPhoto.IsMain);

        if (currentMain != null) currentMain.IsMain = false;

        if (photo != null) photo.IsMain = true;

        if (await _repositoryUnit.SaveChangesAsync()) return NoContent();

        return BadRequest("Something unexpected went wrong, Failed to set main photo.");
    }

    [HttpDelete("photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _repositoryUnit.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = user.Photos.FirstOrDefault(ph => ph.Id == photoId);

        if (photo == null) return NotFound("Could not find the photo you were looking for");

        if (photo.IsMain) return BadRequest("Can not delete the main photo");

        var result = await _photoService.DeletePhotoFromCloudAsync(photo.PublicId);
        if (result.Error != null) return BadRequest(result.Error.Message);

        user.Photos.Remove(photo);

        if (await _repositoryUnit.SaveChangesAsync()) return Ok();

        return BadRequest("Failed to delete photo");
    }
}