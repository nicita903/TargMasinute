using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareTranzactiiFisierText : IStocareData
    {
        private const int ID_PRIMUL_TRANZACTIE = 1;
        private const int INCREMENT = 1;
        private string numeFisier;

        public AdministrareTranzactiiFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisierText.Close();
        }

        public void AddTranzactie(Tranzactie tranzactie)
        {
            tranzactie.Id = GetNextIdTranzactie();

            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, true))
            {
                streamWriterFisierText.WriteLine(tranzactie.ConversieLaSirPentruFisier());
            }
        }

        public List<Tranzactie> GetTranzactii()
        {
            List<Tranzactie> tranzactii = new List<Tranzactie>();

            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;
                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    tranzactii.Add(new Tranzactie(linieFisier));
                }
            }

            return tranzactii;
        }

        public Tranzactie GetTranzactie(int id)
        {
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;
                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    Tranzactie t = new Tranzactie(linieFisier);
                    if (t.Id == id)
                        return t;
                }
            }
            return null;
        }

        public bool UpdateTranzactie(Tranzactie tranzactieActualizata)
        {
            List<Tranzactie> tranzactii = GetTranzactii();
            bool actualizareCuSucces = false;

            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, false))
            {
                foreach (Tranzactie t in tranzactii)
                {
                    Tranzactie tranzactiePentruScris = t;
                    if (t.Id == tranzactieActualizata.Id)
                    {
                        tranzactiePentruScris = tranzactieActualizata;
                    }
                    streamWriterFisierText.WriteLine(tranzactiePentruScris.ConversieLaSirPentruFisier());
                }
                actualizareCuSucces = true;
            }

            return actualizareCuSucces;
        }

        public bool DeleteTranzactie(int id)
        {
            List<Tranzactie> tranzactii = GetTranzactii();
            bool stergereCuSucces = false;

            using (StreamWriter streamWriterFisierText = new StreamWriter(numeFisier, false))
            {
                foreach (Tranzactie t in tranzactii)
                {
                    if (t.Id != id)
                    {
                        streamWriterFisierText.WriteLine(t.ConversieLaSirPentruFisier());
                    }
                    else
                    {
                        stergereCuSucces = true;
                    }
                }
            }

            return stergereCuSucces;
        }

        private int GetNextIdTranzactie()
        {
            List<Tranzactie> tranzactii = GetTranzactii();

            if (tranzactii.Count == 0)
            {
                return ID_PRIMUL_TRANZACTIE;
            }

            return tranzactii.Last().Id + INCREMENT;
        }
    }
}