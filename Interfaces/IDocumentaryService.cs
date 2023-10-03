using CRApiSolution.DTO;

namespace CRApiSolution.Interfaces
{
    public interface IDocumentaryService
    {
        List<DocumentaryRecommendationDTO> GetRecommendedDocumentaries(string topic);
    }
}
