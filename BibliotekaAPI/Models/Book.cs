namespace BibliotekaAPI.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int Year { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public List<Copy> Copies { get; set; } = new();
}
