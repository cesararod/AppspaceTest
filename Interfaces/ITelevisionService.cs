using CRApiSolution.DTO;

namespace CRApiSolution.Interfaces
{
    public interface ITelevisionService
    {
        List<TVShowDTO> GetRecommendedTVShows(string[] keywords, string[] genres);
    }
}
