using LibrarieModele;

namespace NivelStocareDate
{
    public interface IStocareData
    {
        void AddTranzactie(Tranzactie t);
        List<Tranzactie> GetTranzactii();
        Tranzactie? GetTranzactie(int id);  // Adaugat ? pentru ca poate returna null
        bool UpdateTranzactie(Tranzactie t);
        bool DeleteTranzactie(int id);
    }
}