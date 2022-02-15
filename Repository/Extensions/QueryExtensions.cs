namespace Repository.Extensions;

public static class QueryExtensions
{
    public static IQueryable<AppUser> OrderByField(this IQueryable<AppUser> query, string field)
    {
        return field switch
        {
            "created" => query.OrderByDescending(user => user.Created),
            _ => query.OrderByDescending(user => user.LastActive)
        };
    }

    public static IQueryable<MessageDto> FilterByContainer(this IQueryable<MessageDto> query,
        MessageParams messageParams)
    {
        return messageParams.Status switch
        {
            "Inbox" => query.Where(msg => msg.RecipientUsername == messageParams.Username && !msg.RecipientDeleted),
            "Outbox" => query.Where(msg => msg.SenderUsername == messageParams.Username && !msg.SenderDeleted),
            _ => query.Where(msg =>
                msg.RecipientUsername == messageParams.Username && !msg.RecipientDeleted && msg.DateRead == null)
        };
    }

    public static IQueryable<Book> OrderByField(this IQueryable<Book> query, string field, bool asc)
    {
        return field switch
        {
            "author" => asc
                ? query.OrderBy(book => book.Author.Name)
                    .ThenBy(book => book.Author.Lastname)
                    .ThenBy(book => book.Author.Id)
                : query.OrderByDescending(book => book.AuthorId)
                    .ThenBy(book => book.Author.Lastname)
                    .ThenBy(book => book.Author.Id),
            "release" => asc
                ? query.OrderBy(book => book.PublicationDate)
                : query.OrderByDescending(book => book.PublicationDate),
            _ => asc
                ? query.OrderBy(book => book.Title.ToLower())
                : query.OrderByDescending(book => book.Title.ToLower()),
        };
    }

    public static IQueryable<Comment> OrderByField(this IQueryable<Comment> query, string? field, bool asc)
    {
        return field switch
        {
            "Stars" => asc
                ? query.OrderBy(book => book.Rating)
                : query.OrderByDescending(comment => comment.Rating),
            _ => asc
                ? query.OrderBy(comment => comment.CommentedOn)
                : query.OrderByDescending(book => book.CommentedOn),
        };
    }
}