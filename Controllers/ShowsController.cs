using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Data;
using TvMazeScraper.Entities;

namespace TvMazeScraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly TvMazeScraperContext _context;
        readonly int pageSize = 100;

        public ShowsController(TvMazeScraperContext context)
        {
            _context = context;
        }

        // GET: api/Shows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowDto>>> GetAllShows([FromQuery] int page = 0)
        {
            var pagedData = await _context
                .Show
                .Include(s => s.Cast)
                .ThenInclude(a => a.Actor)
                .Skip(page * pageSize)
                .Take(pageSize)
                .Select(s => new ShowDto {
                    Id = s.Id, 
                    Name = s.Name,
                    Actors = s.Cast.
                        Select(c => c.Actor).
                        Select(a => new ActorDto { Id = a.Id, Name = a.Name, Birthday = a.Birthday }).
                        OrderByDescending(o => o.Birthday)
                })
                .ToListAsync();

            return pagedData;
        }

        // GET: api/Shows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Show>> GetShow(int id)
        {
            var show = await _context.Show.FindAsync(id);

            if (show == null)
            {
                return NotFound();
            }

            return show;
        }
    }
}
