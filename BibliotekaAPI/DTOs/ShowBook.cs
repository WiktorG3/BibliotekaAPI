using BibliotekaAPI.Models;

namespace BibliotekaAPI.DTOs
{
    public class ShowBook
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int Year { get; set; }
        public ShowAuthor Author { get; set; } = null!;

        public ShowBook(Book book)
        {
            Id = book.Id;
            Title = book.Title;
            Year = book.Year;
            Author = new ShowAuthor(book.Author!);
        }
    }
}
