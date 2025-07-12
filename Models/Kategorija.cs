namespace RestoranASP.Models
{
    public class Kategorija
    {
        public int Id {  get; set; }

        public string Naziv { get; set; } = string.Empty;

        public List<Jelo> Jela { get; set; } = new();
    }
}
