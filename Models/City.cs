using System.ComponentModel.DataAnnotations;

namespace CRApiSolution.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public int Population { get; set; }
    }
}
