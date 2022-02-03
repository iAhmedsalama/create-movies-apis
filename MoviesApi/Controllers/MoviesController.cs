using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IGenresService _genresService;
        private readonly IMapper _mapper;
        public MoviesController(IMoviesService moviesService, IGenresService genresService, IMapper mapper)
        {
            _moviesService = moviesService;
            _genresService = genresService;
            _mapper = mapper;
        }

        //Determine the images extensions will user forced to enter
        private new List<string> _allowedExtensions = new List<string>{".jpg", ".png"};

        //Determine the image size will user forced to enter
        private long _maxAllowedPosterSize = 1048576;

       

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var movies = _moviesService.GetAll();

            //map movies to DTOs
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            if (movies == null)
                return NotFound();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound();

            var dto = _mapper.Map<MovieDetailsDto>(movie);

            return Ok(dto);
        }

        [HttpGet("GetMoviesByGenreId")]
        public async Task<IActionResult> GetMoviesByGenreId(byte genreId)
        {
            var movies = await _moviesService.GetAllMovies(genreId);

            //map movies to DTOs
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            if (movies == null)
                return NotFound();

            return Ok(data);

        }


        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromForm] CreateMovieDto dto)
        {
            //if file name extension does not contains wanted extensions 
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg formats is allowed");

            //if poster max size is over than required
            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Only 1 MB poster size is allowed");

            //if movie genre is not found
            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid Genre Id");


            //using memory stream to convert poster to array of bytes
            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _moviesService.CreateMovie(movie);

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromForm] CreateMovieDto dto)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No Movie Found With this Id {id}");

            //if movie genre is not found
            var isValidGenre = await _genresService.IsValidGenre(dto.GenreId);

            if (dto.Poster != null)
            {
                //if file name extension does not contains wanted extensions 
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg formats is allowed");

                //if poster max size is over than required
                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Only 1 MB poster size is allowed");

                //using memory stream to convert poster to array of bytes
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }


            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.Stroyline = dto.Stroyline;
            movie.Rate = dto.Rate;

            _moviesService.UpdateMovie(movie);

            return Ok(movie);
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieById(int id)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
                return NotFound($"No Movie Found With this Id {id}");

            _moviesService.DeleteMovie(movie);

            return Ok(movie);
        }


    }
}
