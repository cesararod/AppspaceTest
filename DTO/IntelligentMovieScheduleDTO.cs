namespace CRApiSolution.DTO
{
    public class IntelligentMovieScheduleDTO
    {
        public int LargeScreensCount { get; set; }
        public int SmallScreensCount { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<MovieScheduleDTO> LargeScreenMovies { get; set; }
        public List<MovieScheduleDTO> SmallScreenMovies { get; set; }
    }
}
