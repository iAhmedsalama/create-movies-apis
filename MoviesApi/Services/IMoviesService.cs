using Microsoft.AspNetCore.Mvc;

namespace MoviesApi
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAllMovies(byte genreId = 0);
        Task<Movie> GetById(int id);
        Task<Movie> CreateMovie(Movie movie);
        Movie DeleteMovie(Movie movie);
        Movie UpdateMovie(Movie movie);
    }
}