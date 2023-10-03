using CRApiSolution.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CRApiSolution.Interfaces
{
    public interface IMediaRecommendationService
    {
        List<MediaRecommendationDTO> GetRecommendedMediaAsync(string[] keywords, string[] genres);
        List<MediaRecommendationDTO> GetUpcomingMedia(string[] keywords, string[] genres, DateTime startDate);
        List<TVShowDTO> GetRecommendedTVShows(string[] keywords, string[] genres);
        List<DocumentaryRecommendationDTO> GetRecommendedDocumentaries(string topic);
        List<MediaRecommendationDTO> GetUpcomingMoviesForCinema(string[] genres, int ageRating);
        List<MovieScheduleDTO> GenerateMovieSchedule(DateTime startDate, int screenCount);
        Task<List<IntelligentMovieScheduleDTO>> GenerateIntelligentMovieSchedule([FromQuery] DateTime startTime, [FromQuery] DateTime? endTime, [FromQuery] int largeScreenCount, [FromQuery] int smallScreenCount);
    }
}
