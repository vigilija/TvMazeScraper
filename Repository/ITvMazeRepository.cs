using System.Collections.Generic;
using TvMazeScraper.Entities;

namespace TvMazeScraper.Repository
{
    public interface ITvMazeRepository
    {
        public IList<Show> GetShows(int fromId);
        public IList<Actor> GetCast(int id);
    }
}