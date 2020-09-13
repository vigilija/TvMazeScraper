using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TvMazeScraper.Data;
using TvMazeScraper.Entities;
using TvMazeScraper.Repository;

namespace TvMazeScraper.UseCase
{
    public class Scraper
    {
        private ITvMazeRepository rep;
        private TvMazeScraperContext context;
        private IServiceScope scope;

        public Scraper(ITvMazeRepository _scraper, IServiceProvider serviceProvider)
        {
            rep = _scraper;
            scope = serviceProvider.CreateScope();
            context = scope.ServiceProvider.GetRequiredService<TvMazeScraperContext>();
        }

        public void DownloadShows()
        {
            var waitTime = 10000;
            while (true)
            {
                try
                {
                    var lastShowId = context.Show.Max(s => (int?)s.Id) ?? 0;
                    var response = rep.GetShows(lastShowId);
                    if (response.Count == 0)
                    {
                        System.Threading.Thread.Sleep(waitTime);
                        waitTime *= 2;
                    }
                    else
                    {
                        waitTime = 10000;
                    }

                    foreach (var show in response)
                    {
                        if (context.Show.Any(s => s.Id == show.Id))
                        {
                            continue;
                        }

                        // Distinct actors
                        var castList = rep.GetCast(show.Id).GroupBy(a => a.Id).Select(g => g.First());

                        context.Show.Add(new Show { Id = show.Id, Name = show.Name });
                        foreach (var actor in castList)
                        {
                            if (!context.Actor.Any(a => a.Id == actor.Id))
                            {
                                context.Actor.Add(actor);
                            }
                        }

                        var showActors = castList.Select(s => new ShowActors { ShowId = show.Id, ActorId = s.Id });
                        context.ShowActors.AddRange(showActors);
                        context.SaveChanges();
                    }
                } 
                catch (Exception e)
                {
                    Console.Out.WriteLine($"There was a problem retrieveing data from tv maze. Sleeping for {waitTime}. Reason {e.StackTrace}");
                    System.Threading.Thread.Sleep(waitTime);
                    waitTime *= 2;
                }
            }
        }
    }
}
