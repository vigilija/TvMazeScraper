using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using TvMazeScraper.Entities;

namespace TvMazeScraper.Repository
{
    public class TvMazeRepository: ITvMazeRepository
    {
        private string showPath = "http://api.tvmaze.com/shows?page={0}";
        private string castPath = "http://api.tvmaze.com/shows/{0}/cast";
        HttpClient client;

        public TvMazeRepository(HttpClient _client)
        {
            client = _client;
        }

        public IList<Show> GetShows(int fromId)
        {
            var page = (fromId + 1) / 250;
            var path = string.Format(showPath, page);
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(path).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var respString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var result = JsonSerializer.Deserialize<IList<Show>>(
                            respString,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        return result;
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Http error: " + e);
                }
                System.Threading.Thread.Sleep(2000);
            }
            throw new Exception($"Failed to reach path {path} after five tries");
            }

        public IList<Actor> GetCast(int id)
        {
            var path = string.Format(castPath, id);
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(path).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var respString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var result = JsonSerializer.Deserialize<IList<CastResponse>>(
                            respString,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        return result.Select(s => new Actor { 
                            Id = s.Person.Id, 
                            Birthday = s.Person.Birthday == null ? DateTime.MinValue : DateTime.Parse(s.Person.Birthday), Name = s.Person.Name }).ToList();
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Http error: " + e);
                }
                System.Threading.Thread.Sleep(2000);
            }
            throw new Exception($"Failed to reach path {path} after five tries");
        }
    }
}
