using System;
using System.Collections.Generic;
using System.Linq;
using NivelStocareData;

namespace NivelStocareData
{
    public class AdministrareTranzactiiMemorie
    {
        // Colectia generica List<T> pentru stocarea tranzactiilor
        private List<Tranzactie> tranzactii;
        private int nextId;

        public AdministrareTranzactiiMemorie()
        {
            tranzactii = new List<Tranzactie>();
            nextId = 1;
        }

        // Metoda pentru a returna toate tranzactiile
        public List<Tranzactie> GetTranzactii()
        {
            return tranzactii;
        }

        // Metoda pentru adaugarea unei tranzactii noi
        public void AdaugaTranzactie(Tranzactie tranzactie)
        {
            tranzactie.Id = nextId++;
            tranzactii.Add(tranzactie);
        }

        // Metoda pentru cautare dupa firma si model
        public List<Tranzactie> GetTranzactiiByMasina(string firma = null, string model = null)
        {
            var rezultat = tranzactii;

            if (!string.IsNullOrEmpty(firma))
            {
                rezultat = rezultat.Where(t => t.Masina.NumeFirma.ToLower().Contains(firma.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(model))
            {
                rezultat = rezultat.Where(t => t.Masina.Model.ToLower().Contains(model.ToLower())).ToList();
            }

            return rezultat;
        }

        // Metoda pentru cautare dupa numele vanzatorului
        public List<Tranzactie> GetTranzactiiByVanzator(string numeVanzator)
        {
            return tranzactii.Where(t => t.Vanzator.Nume.ToLower().Contains(numeVanzator.ToLower())).ToList();
        }

        // Metoda pentru cautare dupa numele cumparatorului
        public List<Tranzactie> GetTranzactiiByCumparator(string numeCumparator)
        {
            return tranzactii.Where(t => t.Cumparator.Nume.ToLower().Contains(numeCumparator.ToLower())).ToList();
        }

        // Metoda pentru cautare dupa interval de pret
        public List<Tranzactie> GetTranzactiiByPret(decimal pretMin, decimal pretMax)
        {
            var rezultat = from t in tranzactii
                           where t.Pret >= pretMin && t.Pret <= pretMax
                           select t;
            return rezultat.ToList();
        }

        // Metoda pentru cautare dupa anul fabricatiei
        public List<Tranzactie> GetTranzactiiByAnFabricatie(int an)
        {
            return tranzactii.Where(t => t.Masina.AnFabricatie == an).ToList();
        }
    }
}