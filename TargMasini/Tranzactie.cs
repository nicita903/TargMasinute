using System;

namespace NivelStocareData
{
    public class Tranzactie
    {
        public int Id { get; set; }
        public Persoana Vanzator { get; set; }
        public Persoana Cumparator { get; set; }
        public Masina Masina { get; set; }
        public DateTime DataTranzactie { get; set; }
        public decimal Pret { get; set; }

        public Tranzactie()
        {
            Vanzator = new Persoana();
            Cumparator = new Persoana();
            Masina = new Masina();
            DataTranzactie = DateTime.Now;
        }

        public string DataTranzactieShort
        {
            get { return DataTranzactie.ToString("yyyy-MM-dd"); }
        }
    }
}