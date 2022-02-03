namespace MoviesApi.Dtos
{
    public class MovieBaseDto
    {
        [MaxLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }
        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Stroyline { get; set; }

        public byte GenreId { get; set; }
    }
}
