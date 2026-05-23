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
    [Route("authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowAuthor>>> GetAuthors()
        {
            return await _context.Authors
                .Select(a => new ShowAuthor(a))
                .ToListAsync();
        }

        // GET: authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowAuthor>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return new ShowAuthor(author);
        }

        // PUT: authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, CreateAuthor input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(input.FirstName) || string.IsNullOrEmpty(input.LastName))
            {
                return BadRequest();
            }

            Author author = new Author
            {
                Id = id,
                FirstName = input.FirstName,
                LastName = input.LastName,
            };

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShowAuthor>> PostAuthor(CreateAuthor input)
        {
            if (string.IsNullOrEmpty(input.FirstName) || string.IsNullOrEmpty(input.LastName))
            {
                return BadRequest();
            }

            var author = new Author
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, new ShowAuthor(author));
        }

        // DELETE: authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
