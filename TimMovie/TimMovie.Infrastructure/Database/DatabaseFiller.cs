using System.Text.Json;
using TimMovie.Core.Entities;
using TimMovie.Infrastructure.Database.Repositories;

namespace TimMovie.Infrastructure.Database;

public class DatabaseFiller
{
    private const string Path = @"C:\Users\danil\Desktop\result.json";
    private readonly GenreRepository _genreRepository;
    private readonly ActorRepository _actorRepository;
    private readonly ProducerRepository _producerRepository;
    private readonly CountryRepository _countryRepository;
    private readonly FilmRepository _filmRepository;

    public DatabaseFiller(ApplicationContext context)
    {
        _genreRepository = new GenreRepository(context);
        _actorRepository = new ActorRepository(context);
        _producerRepository = new ProducerRepository(context);
        _countryRepository = new CountryRepository(context);
        _filmRepository = new FilmRepository(context);
    }

    public async Task StartFilling()
    {
        var films = GetFilmsFromJson();
        // await AddAllGenres(films);
        // await AddActors(films);
        // await AddProducers(films);
        // await AddCountries(films);
        await AddFilms(films);
    }

    private async Task AddFilms(List<Film> films)
    {
        foreach (var film in films)
        {
            if (await _filmRepository.FindByTitleAsync(film.Title) is not null) continue;
            var producers = new List<Producer>();
            foreach (var producer in film.Producers)
                producers.Add(await _producerRepository.FindByNameAndSurnameAsync(producer.Name, producer.Surname));

            var actors = new List<Actor>();
            foreach (var actor in film.Actors)
                actors.Add(await _actorRepository.FindByNameAndSurnameAsync(actor.Name, actor.Surname));

            var genres = new List<Genre>();
            foreach (var genre in film.Genres)
                genres.Add(await _genreRepository.FindByNameAsync(genre.Name));

            var newFilm = new Film
            {
                Title = film.Title,
                Year = film.Year,
                Description = film.Description,
                Country = await _countryRepository.FindByNameAsync(film.Country.Name),
                Image = film.Image,
                FilmLink = film.FilmLink,
                Producers = producers,
                Actors = actors,
                Genres = genres
            };
            await _filmRepository.AddAsync(newFilm);
        }
    }

    private async Task AddCountries(List<Film> films)
    {
        foreach (var film in films)
        {
            if (await _countryRepository.FindByNameAsync(film.Country.Name) is null)
                await _countryRepository.AddAsync(film.Country);
        }
    }

    private async Task AddProducers(List<Film> films)
    {
        foreach (var film in films)
        {
            foreach (var producer in film.Producers)
            {
                if (await _producerRepository.FindByNameAndSurnameAsync(producer.Name, producer.Surname) is null)
                    await _producerRepository.AddAsync(producer);
            }
        }
    }

    private async Task AddActors(List<Film> films)
    {
        foreach (var film in films)
        {
            foreach (var actor in film.Actors)
            {
                if (await _actorRepository.FindByNameAndSurnameAsync(actor.Name, actor.Surname) is null)
                    await _actorRepository.AddAsync(actor);
            }
        }
    }

    private List<Film> GetFilmsFromJson()
    {
        using var streamReader = new StreamReader(Path);
        var json = streamReader.ReadToEnd();
        var films = JsonSerializer.Deserialize<ICollection<Film>>(json)!.ToList();
        return films;
    }

    private async Task AddAllGenres(List<Film> films)
    {
        var uniqueGenres = GetUniqueGenres(films);
        foreach (var genre in uniqueGenres)
        {
            if (await _genreRepository.FindByNameAsync(genre.Name) is null)
                await _genreRepository.AddAsync(genre);
        }
    }

    private List<Genre> GetUniqueGenres(List<Film> films)
    {
        var list = new List<Genre>();
        foreach (var film in films)
        {
            foreach (var genre in film.Genres)
            {
                if (list.All(e => !e.Name.Equals(genre.Name)))
                    list.Add(genre);
            }
        }

        return list.ToList();
    }
}