namespace CRApiSolution.Models
{
    public class SuccessfulMovie
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public int SeatsSold { get; set; }
        public DateTime ScreeningDate { get; set; }
        public int LargeScreensCount { get; set; }
        public int SmallScreensCount { get; set; }
    }
}
