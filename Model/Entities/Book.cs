namespace Model.Entities;

public class Book
{
    public int Id { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public int AuthorId { get; set; }
    public int Pages { get; set; }
    public DateTime PublicationDate { get; set; }
    public Author Author { get; set; }
    public ICollection<LikedBook> LikedBy { get; set; }
    public IEnumerable<Comment>? Comments { get; set; }
    public BookCoverPhoto Photo { get; set; }
}