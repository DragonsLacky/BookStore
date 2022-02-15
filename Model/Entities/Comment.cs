namespace Model.Entities;

public class Comment
{
    public int Id { get; set; }

    public int CommenterId { get; set; }

    public string CommenterUsername { get; set; }

    public AppUser? Commenter { get; set; }

    public int BookId { get; set; }

    public string BookISBN { get; set; }

    public Book Book { get; set; }

    public string Content { get; set; }

    public DateTime CommentedOn { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    
    [Range(1, 5, ErrorMessage = "Rating needs to be between 1 and 5")]
    public double? Rating { get; set; } = null;
}