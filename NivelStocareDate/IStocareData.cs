using LibrarieModele;

namespace NivelStocareDate
{
    public interface IStocareData
    {
        void AddTranzactie(Tranzactie t);
        List<Tranzactie> GetTranzactii();
        Tranzactie? GetTranzactie(int id);
        bool UpdateTranzactie(Tranzactie t);
        bool DeleteTranzactie(int id);
        void ModificaTranzactie(Tranzactie t);

        void AddPersoana(Persoana persoana);
        List<Persoana> GetPersoane();
        Persoana? GetPersoana(int id);
        bool UpdatePersoana(Persoana persoana);
        bool DeletePersoana(int id);
    }
}
