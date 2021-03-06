namespace Repository;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public void AddGroup(Group group)
    {
        _context.Groups.Add(group);
    }

    public void AddMessage(Message message)
    {
        _context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        _context.Messages.Remove(message);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await _context.Connections.FindAsync(connectionId);
    }

    public async Task<Group> GetGroupForConnection(string connectionId)
    {
        return await _context.Groups
            .Include(g => g.Connections)
            .Where(g => g.Connections.Any(x => x.ConnectionId == connectionId))
            .FirstOrDefaultAsync() ?? throw new Exception("Something when wrong with the group query");
    }

    public async Task<Message> GetMessage(int id)
    {
        return await _context.Messages.FindAsync(id) ?? throw new Exception("There was a problem getting message");
    }

    public async Task<Group> GetMessageGroup(string groupName)
    {
        return await _context.Groups.Include(g => g.Connections).FirstOrDefaultAsync(g => g.Name == groupName) ??
               throw new Exception("Something went wrong when getting the message group");
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = _context.Messages
            .OrderByDescending(msg => msg.MessageSent)
            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
            .AsQueryable()
            .FilterByContainer(messageParams);

        return await PagedList<MessageDto>.CreateAsync(query, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await _context.Messages
            .Where
            (
                m =>
                    (m.RecipientUsername == currentUsername
                     && m.SenderUsername == recipientUsername
                     && !m.RecipientDeleted)
                    || (m.RecipientUsername == recipientUsername
                        && m.SenderUsername == currentUsername
                        && !m.SenderDeleted)
            )
            .OrderByDescending(m => m.MessageSent)
            .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var unreadMessages = messages.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername).ToList();

        if (unreadMessages.Any())
        {
            unreadMessages.ForEach(msg => msg.DateRead = DateTime.UtcNow);
        }

        return messages;
    }

    public void RemoveConnection(Connection connection)
    {
        _context.Connections.Remove(connection);
    }
}