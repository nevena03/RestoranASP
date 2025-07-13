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

        public string Naziv { get; set; } = string.Empty;

        public decimal? Cena { get; set; }
        public bool IsChecked { get; set; }

        public int BrojPorcija { get; set; } = 1;  
    }
}
