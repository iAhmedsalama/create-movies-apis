using Microsoft.AspNetCore.Mvc;

namespace MoviesApi
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAllGenre();
        Task<Genre> GetGenreById(byte id);
        Task<Genre> CreateGenre(Genre genre);
        Genre UpdateGenre(Genre genre);
        Genre DeleteGenre(Genre genre);
        Task<bool> IsValidGenre(byte id);
    }
}