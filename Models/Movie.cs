using System.ComponentModel.DataAnnotations;

namespace CRApiSolution.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string OriginalTitle { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(255)]
        public string OriginalLanguage { get; set; }

        [Required]
        public bool Adult { get; set; }
    }
}
