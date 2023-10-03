using System;
using System.Collections.Generic;

namespace CRApiSolution.DTO
{
    public class MediaRecommendationDTO
    {
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Website { get; set; }
        public List<string> Keywords { get; set; }
    }
}
