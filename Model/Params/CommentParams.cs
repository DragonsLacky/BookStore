namespace Model.Params;

public class CommentParams: PaginationParams
{
    public string? Isnb { get; set; }
    public string? OrderBy { get; set; }
    public bool Ascending { get; set; }
    public string? Username { get; set; }
}