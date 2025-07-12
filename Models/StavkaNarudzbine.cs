using System.ComponentModel.DataAnnotations;

namespace RestoranASP.Models
{
    public class StavkaNarudzbine
    {
        public int? NarudzbinaId { get; set; }

        public Narudzbina? Narudzbina { get; set; }

        public int? JeloId { get; set; }

        public Jelo? Jelo { get; set; }

        [Range(0, 50, ErrorMessage = "Broj porcija ne može biti veći od 50!")]
        public int? BrojPorcija {  get; set; }



    }
}
