using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.Data;
using BookStoreApp.Models.Author;
using AutoMapper;
using BookStoreApp.Static;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            logger.LogInformation($"Request to {nameof(GetAuthors)}");
            try
            {
                if (_context.Authors == null)
                {
                    return NotFound();
                }
                var authors = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(await _context.Authors.ToListAsync());
                return Ok(authors);
            }
            catch ( Exception ex )
            {
                logger.LogError(ex, Messages.Error500Message);     
                return StatusCode(500);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            try
            {
                if (_context.Authors == null)
                {
                    return NotFound();
                }
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    logger.LogWarning($"The author you are looking for with {id} isn't found");
                    return NotFound();
                }
                var authorDto = mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, Messages.Error500Message);
                return StatusCode(500);
            }
           
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            try
            {
                if (id != authorDto.Id)
                {
                    logger.LogWarning($"{nameof(PutAuthor)} The id of {id} does not match");
                    return BadRequest();
                }

                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    logger.LogWarning($"The author you are to updat with {id} isn't found");
                    return NotFound();
                }

                mapper.Map(authorDto, author);
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
            catch (Exception ex)
            {
                logger.LogError(ex, Messages.Error500Message);
                return StatusCode(500);
            }

            
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                if (_context.Authors == null)
                {
                    return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
                }

                var author = mapper.Map<Author>(authorDto);
                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, Messages.Error500Message);
                return StatusCode(500);
            }
            
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                if (_context.Authors == null)
                {
                    logger.LogWarning($"The author you are looking for is not found in: {nameof(DeleteAuthor)}");
                    return NotFound();
                }
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, Messages.Error500Message);
                return StatusCode(500);
            }
            
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault(); 
        }
    }
}
