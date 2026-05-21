using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareTranzactiiFisierText : IStocareData
    {
        private const int ID_PRIMUL_TRANZACTIE = 1;
        private const int ID_PRIMUL_PERSOANA = 1;
        private const int INCREMENT = 1;
        private readonly string numeFisier;
        private readonly string numeFisierPersoane;

        public AdministrareTranzactiiFisierText(string numeFisier)
        {
            if (string.IsNullOrWhiteSpace(numeFisier))
            {
                numeFisier = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Tranzactii.txt");
            }

            this.numeFisier = numeFisier;
            string director = Path.GetDirectoryName(this.numeFisier)
                ?? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            numeFisierPersoane = Path.Combine(director, "Persoane.txt");

            CreeazaFisierDacaNuExista(this.numeFisier);
            CreeazaFisierDacaNuExista(numeFisierPersoane);
        }

        private static void CreeazaFisierDacaNuExista(string caleFisier)
        {
            string? director = Path.GetDirectoryName(caleFisier);

            if (!string.IsNullOrWhiteSpace(director) && !Directory.Exists(director))
            {
                Directory.CreateDirectory(director);
            }

            if (!File.Exists(caleFisier))
            {
                File.Create(caleFisier).Close();
            }
        }

        public void AddTranzactie(Tranzactie tranzactie)
        {
            tranzactie.Id = GetNextIdTranzactie();

            using StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, true);
            streamWriterFisierText.WriteLine(tranzactie.ConversieLaSirPentruFisier());
        }

        public List<Tranzactie> GetTranzactii()
        {
            List<Tranzactie> tranzactii = new List<Tranzactie>();

            using StreamReader streamReader = new StreamReader(numeFisier);
            string? linieFisier;
            while ((linieFisier = streamReader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(linieFisier))
                {
                    tranzactii.Add(new Tranzactie(linieFisier));
                }
            }

            return tranzactii;
        }

        public Tranzactie? GetTranzactie(int id)
        {
            return GetTranzactii().FirstOrDefault(t => t.Id == id);
        }

        public bool UpdateTranzactie(Tranzactie tranzactieActualizata)
        {
            List<Tranzactie> tranzactii = GetTranzactii();
            bool actualizareCuSucces = false;

            for (int i = 0; i < tranzactii.Count; i++)
            {
                if (tranzactii[i].Id == tranzactieActualizata.Id)
                {
                    tranzactii[i] = tranzactieActualizata;
                    actualizareCuSucces = true;
                    break;
                }
            }

            if (actualizareCuSucces)
            {
                ScrieTranzactii(tranzactii);
            }

            return actualizareCuSucces;
        }

        public bool DeleteTranzactie(int id)
        {
            List<Tranzactie> tranzactii = GetTranzactii();
            bool sters = tranzactii.RemoveAll(t => t.Id == id) > 0;

            if (sters)
            {
                ScrieTranzactii(tranzactii);
            }

            return sters;
        }

        public void ModificaTranzactie(Tranzactie tranzactieNoua)
        {
            UpdateTranzactie(tranzactieNoua);
        }

        private int GetNextIdTranzactie()
        {
            List<Tranzactie> tranzactii = GetTranzactii();
            return tranzactii.Count == 0 ? ID_PRIMUL_TRANZACTIE : tranzactii.Max(t => t.Id) + INCREMENT;
        }

        private void ScrieTranzactii(List<Tranzactie> tranzactii)
        {
            using StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, false);
            foreach (Tranzactie t in tranzactii)
            {
                streamWriterFisierText.WriteLine(t.ConversieLaSirPentruFisier());
            }
        }

        public void AddPersoana(Persoana persoana)
        {
            persoana.Id = GetNextIdPersoana();

            using StreamWriter streamWriterFisierText = new StreamWriter(numeFisierPersoane, true);
            streamWriterFisierText.WriteLine(persoana.ConversieLaSirPentruFisier());
        }

        public List<Persoana> GetPersoane()
        {
            List<Persoana> persoane = new List<Persoana>();

            using StreamReader streamReader = new StreamReader(numeFisierPersoane);
            string? linieFisier;
            while ((linieFisier = streamReader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(linieFisier))
                {
                    persoane.Add(new Persoana(linieFisier, true));
                }
            }

            return persoane;
        }

        public Persoana? GetPersoana(int id)
        {
            return GetPersoane().FirstOrDefault(p => p.Id == id);
        }

        public bool UpdatePersoana(Persoana persoanaActualizata)
        {
            List<Persoana> persoane = GetPersoane();
            bool actualizareCuSucces = false;

            for (int i = 0; i < persoane.Count; i++)
            {
                if (persoane[i].Id == persoanaActualizata.Id)
                {
                    persoane[i] = persoanaActualizata;
                    actualizareCuSucces = true;
                    break;
                }
            }

            if (actualizareCuSucces)
            {
                ScriePersoane(persoane);
            }

            return actualizareCuSucces;
        }

        public bool DeletePersoana(int id)
        {
            List<Persoana> persoane = GetPersoane();
            bool sters = persoane.RemoveAll(p => p.Id == id) > 0;

            if (sters)
            {
                ScriePersoane(persoane);
            }

            return sters;
        }

        private int GetNextIdPersoana()
        {
            List<Persoana> persoane = GetPersoane();
            return persoane.Count == 0 ? ID_PRIMUL_PERSOANA : persoane.Max(p => p.Id) + INCREMENT;
        }

        private void ScriePersoane(List<Persoana> persoane)
        {
            using StreamWriter streamWriterFisierText = new StreamWriter(numeFisierPersoane, false);
            foreach (Persoana p in persoane)
            {
                streamWriterFisierText.WriteLine(p.ConversieLaSirPentruFisier());
            }
        }
    }
}
