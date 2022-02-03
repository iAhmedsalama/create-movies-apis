using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplicationDbContext _context;

        public GenresService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> CreateGenre(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        

        public async Task<IEnumerable<Genre>> GetAllGenre()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre> GetGenreById(byte id)
        {
            return await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);            
        }

        public Genre UpdateGenre(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return (genre);
        }

        public Genre DeleteGenre(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return (genre);
        }

        public async Task<bool> IsValidGenre(byte id)
        {
            return await _context.Genres.AnyAsync(g => g.Id == id);
        }
    }
}
