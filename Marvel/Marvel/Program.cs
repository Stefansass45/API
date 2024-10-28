using System;

namespace Marvel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var characters = new List<Character> {
                new Character("Maya Lopez", "Echo", "Protagonist"),
                new Character("Agatha Harkness", "Agatha: Darkhold Diaries", "Antogonist"),
                new Character("Peter Parker", "Your Friendly Neigborhood Spider-Man", "Protagonist")
                };
            var episodes = new List<Episode> {
                new Episode("Echo", 1, "Origin Story"),
                new Episode("Agatha: Darkhold Diaries", 1, "The Witch Returns"),
                new Episode("Your Friendly Neigborhood Spider-Man", 1, "Freshman year")
                };
            var locations = new List<Location> {
                new Location("New York", "Echo"),
                new Location("Wetview", "Agatha: Darkhold Diaries"),
                new Location("Queens", "Your Friendly Neigborhood Spider-Man")
                };
            //GetEndpoints
            app.MapGet("/characters", () => characters).WithName("GetCharacters").WithOpenApi();
            app.MapGet("/episodes", () => episodes).WithName("GetEpisodes").WithOpenApi();
            app.MapGet("/locations", () => locations).WithName("GetLocations").WithOpenApi();
            // post endpoints
            app.MapPost("/characters", (Character character) =>
            {
                characters.Add(character);
                return Results.Created($"/characters/{character.Name}", character);
            }).WithName("CreateCharacter").WithOpenApi();

            app.MapPost("/Episodes", (Episode episode) =>
            {
                episodes.Add(episode);
                return Results.Created($"/episodes/{episode.seriesName}/{episode.eipsodeNumber}", episode);
            }).WithName("CreateEpisode").WithOpenApi();

            app.MapPost("/location", (Location location) =>
            {
                locations.Add(location);
                return Results.Created($"/locations/{location.Name}/{location.seriesName}", location);
            }).WithName("CreateLocation").WithOpenApi();
            // put
            app.MapPut("/characters/{name}", (string name, Character updatedCharacter) =>
            {
                var character = characters.FirstOrDefault(c => c.Name == name);
                if (character != null)
                {
                    characters.Remove(character);
                    characters.Add(character);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("UpdateCharacter").WithOpenApi();

            app.MapPut("/Episodes/{seriesName}/{episodeNumer}", (string seriesName, int episodeNumer, Episode updatedEpisode) =>
            {
                var episode = episodes.FirstOrDefault(e => e.seriesName == seriesName && e.eipsodeNumber == episodeNumer);
                if (episode != null)
                {
                    episodes.Remove(episode);
                    episodes.Add(updatedEpisode);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("UpdateEpisode").WithOpenApi();

            app.MapPut("/location/{name}", (string name, Location updateLocation) =>
            {
                var location = locations.FirstOrDefault(l => l.Name == name);
                if (location != null)
                {
                    locations.Remove(location);
                    locations.Add(updateLocation);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("UpdateLocation").WithOpenApi();



            //Delete endpoints
            app.MapDelete("/characters/{name}", (string name) =>
            {
                var character = characters.FirstOrDefault(c => c.Name == name);
                if (character != null)
                {
                    characters.Remove(character);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("DeleteCharacter").WithOpenApi();

            app.MapDelete("/Episodes/{seriesName}/{episodeNumer}", (string seriesName, int episodeNumer) =>
            {
                var episode = episodes.FirstOrDefault(e => e.seriesName == seriesName && e.eipsodeNumber == episodeNumer);
                if (episode != null)
                {
                    episodes.Remove(episode);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("DeleteEpisode").WithOpenApi();

            app.MapDelete("/location/{name}", (string name) =>
            {
                var location = locations.FirstOrDefault(l => l.Name == name);
                if (location != null)
                {
                    locations.Remove(location);
                    return Results.NoContent();
                }
                return Results.NotFound();
            }).WithName("DeleteLocation").WithOpenApi();

            //New series filter endpoint
            app.MapGet("/characters/series/{seriesName}", (string seriesName) =>
            {
                var result = characters.Where(c => c.seriesName.Equals(seriesName, StringComparison.OrdinalIgnoreCase)).ToList();
                return result;
            }).WithName("GetCharactersBySeries").WithOpenApi();

            app.Run();
        }
        public record Character(string Name, string seriesName, string Role);
        public record Episode(string seriesName, int eipsodeNumber, string Title);
        public record Location(string Name, string seriesName);
    }
}