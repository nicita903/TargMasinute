using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareTranzactiiMemorie : IStocareData
    {
        private List<Tranzactie> tranzactii;
        private int nextId;

        public AdministrareTranzactiiMemorie()
        {
            tranzactii = new List<Tranzactie>();
            nextId = 1;
        }

        public void AddTranzactie(Tranzactie tranzactie)
        {
            tranzactie.Id = nextId++;
            tranzactii.Add(tranzactie);
        }

        public List<Tranzactie> GetTranzactii()
        {
            return tranzactii;
        }

        public Tranzactie GetTranzactie(int id)
        {
            foreach (Tranzactie t in tranzactii)
            {
                if (t.Id == id)
                    return t;
            }
            return null;
        }

        public bool UpdateTranzactie(Tranzactie tranzactieActualizata)
        {
            for (int i = 0; i < tranzactii.Count; i++)
            {
                if (tranzactii[i].Id == tranzactieActualizata.Id)
                {
                    tranzactii[i] = tranzactieActualizata;
                    return true;
                }
            }
            return false;
        }

        public bool DeleteTranzactie(int id)
        {
            Tranzactie t = GetTranzactie(id);
            if (t != null)
            {
                return tranzactii.Remove(t);
            }
            return false;
        }
    }
}