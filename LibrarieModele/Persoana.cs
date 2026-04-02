namespace LibrarieModele
{
    public class Persoana
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;  // Adaugat = string.Empty

        public Persoana()
        {
            Nume = string.Empty;
        }

        public Persoana(string nume)
        {
            Nume = nume ?? string.Empty;
        }
    }
}