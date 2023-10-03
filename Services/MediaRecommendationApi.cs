using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CRApiSolution.Contexts;
using CRApiSolution.DTO;
using CRApiSolution.Exceptions;
using CRApiSolution.Interfaces;
using CRApiSolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CRApiSolution.Services
{
    public class MediaRecommendationApi : IMediaRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly MovieDbContext _dbContext;

        public MediaRecommendationApi(IConfiguration configuration, IHttpClientFactory httpClientFactory, MovieDbContext dbContext)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
            _dbContext = dbContext;
        }

        // Method not implemented
        public List<MediaRecommendationDTO> GetRecommendedMediaAsync(string[] keywords, string[] genres)
        {
            throw new NotImplementedException();
        }

        // Method not implemented
        public List<MediaRecommendationDTO> GetUpcomingMedia(string[] keywords, string[] genres, DateTime startDate)
        {
            throw new NotImplementedException();
        }

        // Method not implemented
        public List<TVShowDTO> GetRecommendedTVShows(string[] keywords, string[] genres)
        {
            throw new NotImplementedException();
        }

        // Method not implemented
        public List<DocumentaryRecommendationDTO> GetRecommendedDocumentaries(string topic)
        {
            throw new NotImplementedException();
        }

        // Method not implemented
        public List<MediaRecommendationDTO> GetUpcomingMoviesForCinema(string[] genres, int ageRating)
        {
            throw new NotImplementedException();
        }

        // Method not implemented
        public List<MovieScheduleDTO> GenerateMovieSchedule(DateTime startDate, int screenCount)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IntelligentMovieScheduleDTO>> GenerateIntelligentMovieSchedule(DateTime startTime, DateTime? endTime, int largeScreenCount, int smallScreenCount)
        {
            try
            {
                var successfulMovies = GetSuccessfulMovies(startTime, endTime, largeScreenCount, smallScreenCount);
                List<MovieScheduleDTO> popularMoviesApi = await GetPopularMoviesFromApi(startTime);

                if (popularMoviesApi.Count == 0)
                    throw new PopularMoviesException("Failed to fetch the list of popular movies from the API.");

                if (successfulMovies.Count == 0)
                    throw new SuccessfulMoviesException("Error while fetching successful movies.");

                List<IntelligentMovieScheduleDTO> intelligentScheduleList = new List<IntelligentMovieScheduleDTO>();

                // Define popularity rate 
                double popularityMeter = 8.0;

                foreach (var successfulMovie in successfulMovies)
                {
                    foreach (var popularMovie in popularMoviesApi)
                    {
                        if (string.Equals(successfulMovie.OriginalTitle, popularMovie.MovieTitle, StringComparison.OrdinalIgnoreCase))
                        {
                            // Movies match, you can add them to the intelligent schedule
                            var intelligentMovieSchedule = new IntelligentMovieScheduleDTO
                            {
                                LargeScreensCount = largeScreenCount,
                                SmallScreensCount = smallScreenCount,
                                WeekStartDate = DateTime.Today, // Set the week's start date as needed
                                LargeScreenMovies = new List<MovieScheduleDTO>(),
                                SmallScreenMovies = new List<MovieScheduleDTO>(),
                            };

                            // Add movies to the appropriate large or small screen lists
                            if (popularMovie.Popularity >= popularityMeter)
                            {
                                intelligentMovieSchedule.LargeScreenMovies.Add(popularMovie);
                            }
                            else
                            {
                                intelligentMovieSchedule.SmallScreenMovies.Add(popularMovie);
                            }

                            // Add the IntelligentMovieScheduleDTO object to the list
                            intelligentScheduleList.Add(intelligentMovieSchedule);
                        }
                    }
                }

                if (intelligentScheduleList.Count == 0)
                    throw new NoMatchException("No matches found in the data sources.");

                return intelligentScheduleList;

            }
            catch (PopularMoviesException ex)
            {
                throw;
            }
            catch (SuccessfulMoviesException ex)
            {
                throw;
            }
            catch (NoMatchException ex)
            {
                throw;
            }
            catch (InvalidQueryParametersException ex)
            {
                throw new InvalidQueryParametersException("Invalid query parameters.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string ToQueryString(object values)
        {
            var properties = from p in values.GetType().GetProperties()
                             where p.GetValue(values, null) != null
                             select p.Name + "=" + p.GetValue(values, null);

            return "?" + string.Join("&", properties.ToArray());
        }

        public List<Movie> GetSuccessfulMovies(DateTime startTime, DateTime? endTime, int largeScreenCount, int smallScreenCount)
        {
            try
            {
                var query = _dbContext.Session.AsQueryable();
                query = query.Where(session => session.StartTime >= startTime);
                if (endTime.HasValue)
                {
                    query = query.Where(session => session.StartTime <= endTime);
                }               

                // Group by movie 
                var successfulMovies = _dbContext.Session
                                        .GroupBy(s => s.MovieId)
                                        .Select(g => new
                                        {
                                            MovieId = g.Key,
                                            TotalSeatsSold = g.Sum(s => s.SeatsSold)
                                        })
                                        .OrderByDescending(m => m.TotalSeatsSold)
                                        .Take(5) // Define how many movies are going to be retrieved
                                        .ToList();

                // get selected movie details
                var successfulMoviesDetails = _dbContext.Movie
                                                .Where(m => successfulMovies.Select(sm => sm.MovieId).Contains(m.Id))
                                                .ToList();

                return successfulMoviesDetails;
            }
            catch (Exception ex)
            {
                // Maneja excepciones según tus necesidades
                return null;
            }
        }

        private async Task<List<MovieScheduleDTO>> GetPopularMoviesFromApi(DateTime startTime)
        {
            string tmdbApiKey = _configuration["API_READ_ACCESS_TOKEN"];
            string apiUrl = "https://api.themoviedb.org/3/discover/movie";

            var queryParameters = new
            {
                include_adult = false,
                include_video = false,
                language = "en-US",
                page = 4,
                sort_by = "popularity.desc",
                primary_release_date = new { lte = startTime },
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tmdbApiKey);

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl + ToQueryString(queryParameters));

            if (response.IsSuccessStatusCode)
            {
                List<MovieScheduleDTO> intelligentScheduleList = new List<MovieScheduleDTO>();
                // Read the response content as a JSON string

                string json = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(json);

                // Access the "results" field of the dynamic object
                var results = responseObject.results;
                foreach (var movie in results)
                {
                    MovieScheduleDTO result = new MovieScheduleDTO();
                    result.MovieTitle = movie.original_title;
                    //result.ReleaseDate = movie.release_date? movie.release_date: DateTime.Now;
                    result.Popularity = movie.popularity;
                    result.VoteAverage = movie.vote_average;
                    result.VoteCount = movie.vote_count;
                    intelligentScheduleList.Add(result);
                }
                return intelligentScheduleList;
            }
            else
            {
                // The request could not be made successfully
                return null;
            }
        }
    }
}
