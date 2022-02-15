namespace Model.Dtos;

public class CommentDto
{
    public string CommenterUsername { get; set; }

    public string Content { get; set; }

    public DateTime CommentedOn { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);

    public double? Rating { get; set; } = null;
}