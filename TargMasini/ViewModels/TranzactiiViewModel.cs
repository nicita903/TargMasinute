using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LibrarieModele;

namespace TargMasini.ViewModels
{
    public class TranzactiiViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Tranzactie> Tranzactii { get; set; } = new ObservableCollection<Tranzactie>();

        private Tranzactie tranzactieCurenta;
        public Tranzactie TranzactieCurenta
        {
            get => tranzactieCurenta;
            set { tranzactieCurenta = value; OnPropertyChanged(); }
        }

        public ICommand AdaugaCommand { get; }
        public ICommand StergeCommand { get; }

        public TranzactiiViewModel()
        {
            AdaugaCommand = new RelayCommand(AdaugaTranzactie);
            StergeCommand = new RelayCommand(StergeTranzactie, CanStergeTranzactie);
        }

        private void AdaugaTranzactie(object parameter)
        {
            Tranzactii.Add(new Tranzactie());
        }

        private void StergeTranzactie(object parameter)
        {
            if (TranzactieCurenta != null)
                Tranzactii.Remove(TranzactieCurenta);
        }

        private bool CanStergeTranzactie(object parameter) => TranzactieCurenta != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
