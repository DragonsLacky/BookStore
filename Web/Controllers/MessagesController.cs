using Model.Dtos.Creation;
using Model.Entities;
using Model.Params;

namespace Web.Controllers;

public class MessagesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IRepositoryUnit _repositoryUnit;

    public MessagesController(IRepositoryUnit repositoryUnit, IMapper mapper)
    {
        _repositoryUnit = repositoryUnit;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUsername();
        if (username == createMessageDto.RecipientUsername.ToLower())
        {
            return BadRequest("You cannot send messages to yourself");
        }

        var sender = await _repositoryUnit.UserRepository.GetUserByUsernameAsync(username);
        var recipient = await _repositoryUnit.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };

        _repositoryUnit.MessageRepository.AddMessage(message);

        if (await _repositoryUnit.SaveChangesAsync())
        {
            return Ok(_mapper.Map<MessageDto>(message));
        }

        return BadRequest("Failed to send message");
    }

    [HttpGet]
    public async Task<IEnumerable<MessageDto>> GetMessagesForUser([FromQuery] MessageParams messageParams)
    {
        messageParams.Username = User.GetUsername();

        var messages = await _repositoryUnit.MessageRepository.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

        return messages;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        var username = User.GetUsername();

        var message = await _repositoryUnit.MessageRepository.GetMessage(id);

        if (message.SenderUsername != username && message.RecipientUsername != username) return Unauthorized();

        if (message.SenderUsername == username) message.SenderDeleted = true;
        if (message.RecipientUsername == username) message.RecipientDeleted = true;

        if (message.SenderDeleted && message.RecipientDeleted) _repositoryUnit.MessageRepository.DeleteMessage(message);

        if (await _repositoryUnit.SaveChangesAsync()) return Ok();

        return BadRequest("Problem Deleting message");
    }
}