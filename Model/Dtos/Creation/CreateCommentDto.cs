namespace Model.Dtos.Creation;

public class CreateCommentDto
{
    public string CommenterUsername { get; set; }

    public string Content { get; set; }

    [Range(1, 5, ErrorMessage = "Rating needs to be between 1 and 5")]
    public double Rating { get; set; }

    public string Isbn { get; set; }
}