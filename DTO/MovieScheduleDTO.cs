namespace CRApiSolution.DTO
{
    public class MovieScheduleDTO
    {
        public int ScreenNumber { get; set; }
        public string MovieTitle { get; set; }
        public double Popularity { get; set; }
        public string VoteAverage { get; set; }
        public string VoteCount { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
