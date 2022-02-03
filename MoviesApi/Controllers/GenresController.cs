using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController( IGenresService genresService)
        {
            _genresService = genresService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllGenre()
        {
            var genres = await _genresService.GetAllGenre();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(GenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _genresService.CreateGenre(genre);

            return Ok(genre);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _genresService.GetGenreById(id);
            if (genre == null)
                return NotFound($"No Genre was found with Id : {id}");

            genre.Name = dto.Name;

            _genresService.UpdateGenre(genre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(byte id)
        {
            var genre = await _genresService.GetGenreById(id);
            if (genre == null)
                return NotFound($"No Genre was found with Id : {id}");

            _genresService.DeleteGenre(genre);

            return Ok(genre);
        }
    }
}
