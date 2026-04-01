namespace NivelStocareData
{
    public class Persoana
    {
        public int Id { get; set; }
        public string Nume { get; set; }

        public Persoana()
        {
            Nume = string.Empty;
        }

        public Persoana(string nume)
        {
            Nume = nume;
        }
    }
}