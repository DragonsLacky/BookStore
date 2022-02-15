namespace Model.Entities;

public class LikedBook
{
    public int BookId { get; set; }
    public Book Book { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; }
}