using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotekaAPI.Data;
using BibliotekaAPI.Models;
using BibliotekaAPI.DTOs;

namespace BibliotekaAPI.Controllers
{
    [Route("books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowBook>>> GetBooks([FromQuery] int? authorId)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (authorId.HasValue)
            {
                query = query.Where(b => b.AuthorId == authorId.Value);
            }

            return await query
                .Select(b => new ShowBook(b))
                .ToListAsync();
        }

        // GET: books/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShowBook>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            return new ShowBook(book);
        }

        // PUT: books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBook(int id, CreateBook input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(input.Title) || input.Year < 0)
            {
                return BadRequest();
            }

            var authorExists = await _context.Authors.AnyAsync(a => a.Id == input.AuthorId);
            if (!authorExists)
            {
                return BadRequest();
            }

            Book book = new Book
            {
                Id = id,
                Title = input.Title,
                Year = input.Year,
                AuthorId = input.AuthorId,
            };

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShowBook>> PostBook(CreateBook input)
        {
            if (string.IsNullOrEmpty(input.Title) || input.Year < 0)
            {
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(input.AuthorId);
            if (author == null)
            {
                return BadRequest();
            }

            var book = new Book
            {
                Title = input.Title,
                Year = input.Year,
                AuthorId = author.Id,
                Author = author,
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, new ShowBook(book));
        }

        // DELETE: books/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
