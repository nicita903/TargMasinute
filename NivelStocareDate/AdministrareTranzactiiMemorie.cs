using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareTranzactiiMemorie : IStocareData
    {
        private readonly List<Tranzactie> tranzactii;
        private readonly List<Persoana> persoane;
        private int nextIdTranzactie;
        private int nextIdPersoana;

        public AdministrareTranzactiiMemorie()
        {
            tranzactii = new List<Tranzactie>();
            persoane = new List<Persoana>();
            nextIdTranzactie = 1;
            nextIdPersoana = 1;
        }

        public void AddTranzactie(Tranzactie tranzactie)
        {
            tranzactie.Id = nextIdTranzactie++;
            tranzactii.Add(tranzactie);
        }

        public List<Tranzactie> GetTranzactii() => tranzactii;

        public Tranzactie? GetTranzactie(int id) => tranzactii.FirstOrDefault(t => t.Id == id);

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
            Tranzactie? t = GetTranzactie(id);
            return t != null && tranzactii.Remove(t);
        }

        public void ModificaTranzactie(Tranzactie t)
        {
            UpdateTranzactie(t);
        }

        public void AddPersoana(Persoana persoana)
        {
            persoana.Id = nextIdPersoana++;
            persoane.Add(persoana);
        }

        public List<Persoana> GetPersoane() => persoane;

        public Persoana? GetPersoana(int id) => persoane.FirstOrDefault(p => p.Id == id);

        public bool UpdatePersoana(Persoana persoanaActualizata)
        {
            for (int i = 0; i < persoane.Count; i++)
            {
                if (persoane[i].Id == persoanaActualizata.Id)
                {
                    persoane[i] = persoanaActualizata;
                    return true;
                }
            }

            return false;
        }

        public bool DeletePersoana(int id)
        {
            Persoana? p = GetPersoana(id);
            return p != null && persoane.Remove(p);
        }
    }
}
