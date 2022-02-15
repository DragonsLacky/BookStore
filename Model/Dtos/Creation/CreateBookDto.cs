namespace Model.Dtos.Creation;

public class CreateBookDto
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public int Pages { get; set; }
    public DateTime PublicationDate { get; set; }
    public string AuthorName { get; set; }
    public string AuthorLastName { get; set; }
}