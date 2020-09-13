using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TvMazeScraper.Entities
{
    public class ShowActors
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int ShowId { get; set; }
        public Show Show { get; set; }
    }
}
