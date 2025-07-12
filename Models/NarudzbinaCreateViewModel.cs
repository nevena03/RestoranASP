using System.ComponentModel.DataAnnotations;

namespace RestoranASP.Models
{
    public class NarudzbinaCreateViewModel
    {
        public int? NarudzbinaId { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
        public List<JeloCheckboxViewModel> Jela { get; set; }
    }

    public class JeloCheckboxViewModel
    {
        public int JeloId { get; set; }

        [Required(ErrorMessage = "Naziv jela je obavezan!")]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Range(0, 5000, ErrorMessage = "Cena jela može biti između 0 i 5000 dinara!")]
        public decimal? Cena { get; set; }
        public bool IsChecked { get; set; }

        [Range(0, 50, ErrorMessage = "Broj porcija ne može biti veći od 50!")]
        public int BrojPorcija { get; set; } = 1;  
    }
}
