using System.Collections.Generic;

namespace TvMazeScraper.Controllers
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ActorDto> Actors { get; set; }
    }
}
