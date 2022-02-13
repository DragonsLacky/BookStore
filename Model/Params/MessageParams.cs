namespace Model.Params;

public class MessageParams : PaginationParams
{
    public string Username { get; set; }
    public string Status { get; set; } = "Unread";
}