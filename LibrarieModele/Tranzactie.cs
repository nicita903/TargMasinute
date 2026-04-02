using System;

namespace LibrarieModele
{
    public class Tranzactie
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = '|';

        public int Id { get; set; }
        public Persoana Vanzator { get; set; } = new Persoana();  // Initializat direct
        public Persoana Cumparator { get; set; } = new Persoana(); // Initializat direct
        public Masina Masina { get; set; } = new Masina();         // Initializat direct
        public DateTime DataTranzactie { get; set; }
        public decimal Pret { get; set; }

        public Tranzactie()
        {
            Vanzator = new Persoana();
            Cumparator = new Persoana();
            Masina = new Masina();
            DataTranzactie = DateTime.Now;
        }

        public Tranzactie(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            if (dateFisier.Length >= 10)
            {
                Id = Convert.ToInt32(dateFisier[0]);
                Vanzator = new Persoana(dateFisier[1]);
                Cumparator = new Persoana(dateFisier[2]);
                Masina = new Masina();
                Masina.NumeFirma = dateFisier[3];
                Masina.Model = dateFisier[4];
                Masina.AnFabricatie = Convert.ToInt32(dateFisier[5]);
                Masina.Culoare = (CuloareMasina)Enum.Parse(typeof(CuloareMasina), dateFisier[6]);
                Masina.Optiuni = (OptiuniMasina)Enum.Parse(typeof(OptiuniMasina), dateFisier[7]);
                DataTranzactie = DateTime.Parse(dateFisier[8]);
                Pret = Convert.ToDecimal(dateFisier[9]);
            }
            else
            {
                Vanzator = new Persoana();
                Cumparator = new Persoana();
                Masina = new Masina();
                DataTranzactie = DateTime.Now;
            }
        }

        public string ConversieLaSirPentruFisier()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}",
                SEPARATOR_PRINCIPAL_FISIER,
                Id,
                Vanzator?.Nume ?? "NECUNOSCUT",
                Cumparator?.Nume ?? "NECUNOSCUT",
                Masina?.NumeFirma ?? "NECUNOSCUT",
                Masina?.Model ?? "NECUNOSCUT",
                Masina?.AnFabricatie ?? 0,
                Masina?.Culoare ?? CuloareMasina.Alb,
                Masina?.Optiuni ?? OptiuniMasina.Niciuna,
                DataTranzactie.ToString("yyyy-MM-dd"),
                Pret);
        }

        public string DataTranzactieShort
        {
            get { return DataTranzactie.ToString("yyyy-MM-dd"); }
        }
    }
}