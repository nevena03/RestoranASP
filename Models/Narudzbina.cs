namespace RestoranASP.Models
{
    public class Narudzbina
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime Datum { get; set; } = DateTime.UtcNow;

        public List<StavkaNarudzbine> Jela { get; set; } = new();

    }
}
