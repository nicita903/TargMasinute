using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibrarieModele
{
    public class Persoana : INotifyPropertyChanged
    {
        private const char SEPARATOR_PRINCIPAL_FISIER = '|';
        private string nume = string.Empty;
        private string telefon = string.Empty;
        private string rol = "Client";

        public int Id { get; set; }

        public string Nume
        {
            get => nume;
            set
            {
                nume = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public string Telefon
        {
            get => telefon;
            set
            {
                telefon = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        public string Rol
        {
            get => rol;
            set
            {
                rol = value ?? "Client";
                OnPropertyChanged();
            }
        }

        public Persoana()
        {
        }

        public Persoana(string nume)
        {
            Nume = nume;
        }

        public Persoana(string linieFisier, bool dinFisier)
        {
            string[] dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            if (dateFisier.Length >= 4)
            {
                Id = Convert.ToInt32(dateFisier[0]);
                Nume = dateFisier[1];
                Telefon = dateFisier[2];
                Rol = dateFisier[3];
            }
        }

        public string ConversieLaSirPentruFisier()
        {
            return string.Format("{1}{0}{2}{0}{3}{0}{4}",
                SEPARATOR_PRINCIPAL_FISIER,
                Id,
                Nume,
                Telefon,
                Rol);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
