using Model.Dtos.Creation;

namespace Services.Services;

public class MessageHub : Hub
{
    private readonly IMapper _mapper;
    private readonly IHubContext<PresenceHub> _presenceHub;
    private readonly PresenceTracker _tracker;
    private readonly IRepositoryUnit _repositoryUnit;

    public MessageHub(IMapper mapper, IRepositoryUnit repositoryUnit,
        IHubContext<PresenceHub> presenceHub, PresenceTracker tracker)
    {
        _repositoryUnit = repositoryUnit;
        _tracker = tracker;
        _presenceHub = presenceHub;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request.Query["user"].ToString();
        var groupName =
            GetGroupName(Context?.User?.GetUsername() ?? throw new Exception("No user with the provided username"),
                otherUser);
        if (Context?.ConnectionId != null) await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        var group = await AddToGroup(groupName);

        await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

        var messages =
            await _repositoryUnit.MessageRepository.GetMessageThread(Context?.User?.GetUsername(), otherUser);

        if (_repositoryUnit.HasChanges())
        {
            await _repositoryUnit.SaveChangesAsync();
        }

        await Clients.Caller.SendAsync("ReceiveMessageThread", messages as object);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var group = await RemoveFromMessageGroup();
        await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessageDto createMessageDto)
    {
        var username = Context?.User?.GetUsername();
        if (username == createMessageDto.RecipientUsername.ToLower())
        {
            throw new HubException("You cannot send messages to yourself");
        }

        var sender = await _repositoryUnit.UserRepository.GetUserByUsernameAsync(username);
        var recipient = await _repositoryUnit.UserRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

        if (recipient == null) throw new HubException("User not found");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };

        var groupName = GetGroupName(sender.UserName, recipient.UserName);
        var group = await _repositoryUnit.MessageRepository.GetMessageGroup(groupName);

        if (group.Connections.Any(cn => cn.Username == recipient.UserName))
        {
            message.DateRead = DateTime.UtcNow;
        }
        else
        {
            var connections = await _tracker.GetConnectionsForUser(recipient.UserName);
            await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageNotification",
                new {username = sender.UserName});
        }

        _repositoryUnit.MessageRepository.AddMessage(message);

        if (await _repositoryUnit.SaveChangesAsync())
        {
            await Clients.Group(groupName).SendAsync("ReceiveMessage", _mapper.Map<MessageDto>(message));
        }
    }

    private string GetGroupName(string caller, string? other)
    {
        return String.CompareOrdinal(caller, other) < 0 ? $"{caller}-{other}" : $"{other}-{caller}";
    }

    private async Task<Group> AddToGroup(string groupName)
    {
        var group = await _repositoryUnit.MessageRepository.GetMessageGroup(groupName);
        var connection = new Connection(Context.ConnectionId, Context?.User?.GetUsername());

        if (group == null)
        {
            group = new Group(groupName);
            _repositoryUnit.MessageRepository.AddGroup(group);
        }

        group.Connections.Add(connection);

        if (await _repositoryUnit.SaveChangesAsync())
        {
            return group;
        }

        throw new HubException("Failed to join group");
    }

    private async Task<Group?> RemoveFromMessageGroup()
    {
        var group = await _repositoryUnit.MessageRepository.GetGroupForConnection(Context.ConnectionId);
        var connection = group.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
        _repositoryUnit.MessageRepository.RemoveConnection(connection);
        if (await _repositoryUnit.SaveChangesAsync())
        {
            return group;
        }

        throw new HubException("Failed to remove from group");
    }
}