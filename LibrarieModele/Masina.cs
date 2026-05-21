using System;

namespace LibrarieModele
{
    public class Masina
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = '|';

        public int Id { get; set; }
        public string NumeFirma { get; set; } = string.Empty;  // Adaugat = string.Empty
        public string Model { get; set; } = string.Empty;      // Adaugat = string.Empty
        public int AnFabricatie { get; set; }
        public CuloareMasina Culoare { get; set; }
        public string Combustibil { get; set; } = "Benzină"; 
        public OptiuniMasina Optiuni { get; set; }

        public Masina()
        {
            NumeFirma = string.Empty;
            Model = string.Empty;
            Culoare = CuloareMasina.Alb;
            Optiuni = OptiuniMasina.Niciuna;
        }

        public Masina(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            if (dateFisier.Length >= 6)
            {
                Id = Convert.ToInt32(dateFisier[0]);
                NumeFirma = dateFisier[1];
                Model = dateFisier[2];
                AnFabricatie = Convert.ToInt32(dateFisier[3]);
                Culoare = (CuloareMasina)Enum.Parse(typeof(CuloareMasina), dateFisier[4]);
                Optiuni = (OptiuniMasina)Enum.Parse(typeof(OptiuniMasina), dateFisier[5]);
            }
            else
            {
                NumeFirma = string.Empty;
                Model = string.Empty;
                Culoare = CuloareMasina.Alb;
                Optiuni = OptiuniMasina.Niciuna;
            }
        }

        public string ConversieLaSirPentruFisier()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}",
                SEPARATOR_PRINCIPAL_FISIER,
                Id,
                NumeFirma ?? "NECUNOSCUT",
                Model ?? "NECUNOSCUT",
                AnFabricatie,
                Culoare,
                Optiuni);
        }

        public string TipComplet
        {
            get { return $"{NumeFirma} {Model}"; }
        }
    }
}