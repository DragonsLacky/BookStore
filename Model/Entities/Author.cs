namespace Model.Entities;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Lastname { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}