namespace Model.Entities;

public class Cart
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public AppUser Owner { get; set; }
    public string Username { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}