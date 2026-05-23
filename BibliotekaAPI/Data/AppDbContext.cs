using BibliotekaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Copy> Copies { get; set; } = null!;
}
