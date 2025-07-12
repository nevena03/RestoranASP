using System.ComponentModel.DataAnnotations;

namespace RestoranASP.Models
{
    public class Kategorija
    {
        public int Id {  get; set; }

        [Required(ErrorMessage = "Naziv kategorije je obavezan!")]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        public List<Jelo> Jela { get; set; } = new();
    }
}
