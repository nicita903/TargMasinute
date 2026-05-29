using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LibrarieModele;
using TargMasiniWPF;

namespace TargMasini.ViewModels
{
    public class MasiniViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Masina> Masini { get; set; } = new ObservableCollection<Masina>();

        private Masina masinaCurenta;
        public Masina MasinaCurenta
        {
            get => masinaCurenta;
            set { masinaCurenta = value; OnPropertyChanged(); }
        }

        public ICommand AdaugaCommand { get; }
        public ICommand StergeCommand { get; }

        public MasiniViewModel()
        {
            AdaugaCommand = new RelayCommand(AdaugaMasina);
            StergeCommand = new RelayCommand(StergeMasina, CanStergeMasina);
        }

        private void AdaugaMasina(object parameter)
        {
            Masini.Add(new Masina());
        }

        private void StergeMasina(object parameter)
        {
            if (MasinaCurenta != null)
                Masini.Remove(MasinaCurenta);
        }

        private bool CanStergeMasina(object parameter) => MasinaCurenta != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
