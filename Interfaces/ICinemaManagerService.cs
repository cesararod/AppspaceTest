using CRApiSolution.DTO;

namespace CRApiSolution.Interfaces
{
    public interface ICinemaManagerService
    {
        List<MediaRecommendationDTO> GetUpcomingMoviesForCinema(string[] genres, int ageRating);
        List<MovieScheduleDTO> GenerateMovieSchedule(DateTime startDate, int screenCount);
        List<IntelligentMovieScheduleDTO> GenerateIntelligentMovieSchedule(DateTime startDate, int largeScreensCount, int smallScreensCount);
    }
}
