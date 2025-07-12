using System.ComponentModel.DataAnnotations;

namespace RestoranASP.Models
{
    public class Jelo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv jela je obavezan!")]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cena jela je obavezna!")]
        [Range(0, 5000, ErrorMessage = "Cena jela može biti između 0 i 5000 dinara!")]
        public decimal? Cena {  get; set; }

        [Required(ErrorMessage = "Slika je obavezna!")]
        [Url(ErrorMessage = "Unesite ispravan URL slike!")]
        public string Slika { get; set; } = string.Empty;


        [Required(ErrorMessage = "Kategorija je obavezna!")]
        public int? KategorijaId { get; set; }

        public Kategorija? Kategorija { get; set; }

        public List<StavkaNarudzbine> Narudzbine { get; set; } = new();

    }
}
