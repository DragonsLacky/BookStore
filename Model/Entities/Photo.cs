namespace Model.Entities;

[Table("UserPhotos")]
public class UserPhoto
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public bool IsMain { get; set; }

    public string PublicId { get; set; }

    public AppUser AppUser { get; set; }

    public int AppUserId { get; set; }
}

public class BookCoverPhoto
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public string PublicId { get; set; }

    public Book Book { get; set; }

    public int BookId { get; set; }
}