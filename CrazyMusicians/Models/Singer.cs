using System.ComponentModel.DataAnnotations;

namespace CrazyMusicians.Models
{
    public class Musician
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Profession must be between 2 and 30 characters.")]
        public string Profession { get; set; } = "";

        [StringLength(100, MinimumLength = 3, ErrorMessage = "FunFeature must be between 3 and 100 characters.")]
        public string FunFeature { get; set; } = "";
    }
}
