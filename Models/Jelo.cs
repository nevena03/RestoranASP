namespace RestoranASP.Models
{
    public class Jelo
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;

        public decimal? Cena {  get; set; }

        public string Slika { get; set; } = string.Empty;


        public int? KategorijaId { get; set; }

        public Kategorija? Kategorija { get; set; }

        public List<StavkaNarudzbine> Narudzbine { get; set; } = new();

    }
}
