using CRApiSolution.DTO;
using CRApiSolution.Exceptions;
using CRApiSolution.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRApiSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IMediaRecommendationService _mediaService;

        public RecommendationController(IMediaRecommendationService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpGet("recommended")]
        public IActionResult GetRecommendedMedia([FromQuery] string[] keywords, [FromQuery] string[] genres)
        {
            try
            {
                // Get recommended media based on keywords and genres
                List<MediaRecommendationDTO> recommendations = _mediaService.GetRecommendedMediaAsync(keywords, genres);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                // Handle internal server error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("upcoming")]
        public IActionResult GetUpcomingMedia([FromQuery] string[] keywords, [FromQuery] string[] genres, [FromQuery] DateTime startDate)
        {
            try
            {
                // Get upcoming media based on keywords, genres, and start date
                List<MediaRecommendationDTO> upcomingMedia = _mediaService.GetUpcomingMedia(keywords, genres, startDate);
                return Ok(upcomingMedia);
            }
            catch (Exception ex)
            {
                // Handle internal server error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("recommended-tv")]
        public IActionResult GetRecommendedTVShows([FromQuery] string[] keywords, [FromQuery] string[] genres)
        {
            try
            {
                // Get recommended TV shows based on keywords and genres
                List<TVShowDTO> upcomingMedia = _mediaService.GetRecommendedTVShows(keywords, genres);
                return Ok(upcomingMedia);
            }
            catch (Exception ex)
            {
                // Handle internal server error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("recommended-documentaries")]
        public IActionResult GetRecommendedDocumentaries([FromQuery] string topic)
        {
            try
            {
                // Get recommended documentaries based on the topic
                List<DocumentaryRecommendationDTO> upcomingMedia = _mediaService.GetRecommendedDocumentaries(topic);
                return Ok(upcomingMedia);
            }
            catch (Exception ex)
            {
                // Handle internal server error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("upcoming-cinema")]
        public IActionResult GetUpcomingMoviesForCinema([FromQuery] string[] genres, [FromQuery] int ageRating)
        {
            try
            {
                // Get upcoming movies for cinema based on genres and age rating
                List<MediaRecommendationDTO> upcomingMedia = _mediaService.GetUpcomingMoviesForCinema(genres, ageRating);
                return Ok(upcomingMedia);
            }
            catch (Exception ex)
            {
                // Handle internal server error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("movie-schedule")]
        public IActionResult GenerateMovieSchedule([FromQuery] int screenCount, [FromQuery] DateTime startDate)
        {
            try
            {
                // Generate movie schedule based on screen count and start date
                List<MovieScheduleDTO> upcomingMedia = _mediaService.GenerateMovieSchedule(startDate, screenCount);
                return Ok(upcomingMedia);
            }
            catch (Exception ex)
            {
                // Handle internal server error
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a list of recommended media.
        /// </summary>
        /// <param name="startTime">A date of format yyyy/mm/dd.</param>
        /// <param name="endTime">A date of format yyyy/mm/dd.</param>
        /// <param name="largeScreenCount">An integer meaning the number of large screens .</param>
        /// <param name="smallScreenCount">An integer meaning the number of small screens .</param>
        /// <returns>A list of recommended media.</returns>
        [HttpGet("intelligent-movie-schedule")]
        public async Task<IActionResult> GenerateIntelligentMovieSchedule([FromQuery] DateTime startTime, [FromQuery] DateTime? endTime, [FromQuery] int largeScreenCount, [FromQuery] int smallScreenCount)
        {
            try
            {
                // If endTime is not specified, the current day's date will be taken as the end of the time period
                if (!endTime.HasValue)
                {
                    endTime = DateTime.Now;
                }

                var intelligentMovieSchedule = await _mediaService.GenerateIntelligentMovieSchedule(startTime, endTime, largeScreenCount, smallScreenCount);

                if (intelligentMovieSchedule != null)
                {
                    // If the service returns valid data, respond with a 200 OK status and the actual data
                    return Ok(intelligentMovieSchedule);
                }
                else
                {
                    throw new IntelligentScheduleGenerationException("No matches found in the data sources.");
                    
                }
            }

            catch (IntelligentScheduleGenerationException ex)
            {
                return StatusCode(400, $"No data found: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle internal server error
                return StatusCode(400, $"No Movie data found: {ex.Message}");
            }
        }
    }
}
