namespace Model.Helpers;

public class BookParams : PaginationParams
{
    public int AuthorId { get; set; }
    public int MinPages { get; set; }
    public int MaxPages { get; set; }
    public DateTime DateFromPub { get; set; } = DateTime.MinValue;
    public DateTime DateToPub { get; set; } = DateTime.UtcNow;

    public string OrderBy { get; set; } = "title";
    public bool Ascending { get; set; }
}