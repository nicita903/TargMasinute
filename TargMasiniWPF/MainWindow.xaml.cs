using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using LibrarieModele;
using NivelStocareDate;

namespace TargMasiniWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IStocareData _admin;
        private Tranzactie? tranzactieCurentaModificata = null;

        public ObservableCollection<Persoana> Persoane { get; } = new ObservableCollection<Persoana>();

        private Persoana persoanaCurenta = new Persoana();
        public Persoana PersoanaCurenta
        {
            get => persoanaCurenta;
            set
            {
                persoanaCurenta = value;
                OnPropertyChanged();
            }
        }

        private Persoana? persoanaSelectata;
        public Persoana? PersoanaSelectata
        {
            get => persoanaSelectata;
            set
            {
                persoanaSelectata = value;
                if (value != null)
                {
                    PersoanaCurenta = new Persoana
                    {
                        Id = value.Id,
                        Nume = value.Nume,
                        Telefon = value.Telefon,
                        Rol = value.Rol
                    };
                }
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private const int MIN_NUME = 2;
        private const int MAX_NUME = 30;
        private const int MIN_FIRMA = 2;
        private const int MAX_FIRMA = 25;
        private const int MIN_MODEL = 1;
        private const int MAX_MODEL = 25;
        private const int AN_MINIM = 1900;
        private const decimal PRET_MINIM = 1;

        private readonly Brush culoareNormala = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8"));
        private readonly Brush culoareEroare = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5370"));

        public MainWindow()
        {
            InitializeComponent();

            string caleFisier = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                "Tranzactii.txt");

            _admin = new AdministrareTranzactiiFisierText(caleFisier);
            DataContext = this;

            IncarcaCulori();
            ResetFormular();
            IncarcaTranzactii();
            PopuleazaDotariModifica(); // populăm lista de dotări o dată la pornire
        }

        private void IncarcaCulori()
        {
            cboCuloare.Items.Clear();

            string[] culori =
            {
                "Alb", "Negru", "Gri", "Argintiu", "Roșu", "Bordo",
                "Albastru", "Bleumarin", "Verde", "Verde închis",
                "Galben", "Portocaliu", "Maro", "Bej", "Auriu",
                "Mov", "Roz", "Turcoaz", "Cameleon", "Mat"
            };

            foreach (string culoare in culori)
                cboCuloare.Items.Add(culoare);

            cboCuloare.SelectedIndex = 0;
        }

        // ==================== PAGINA MODIFICĂ ====================

        private void PopuleazaDotariModifica()
        {
            // Lista de dotări disponibile pentru modificare (aceleași ca în ListBox-ul din adăugare)
            lstDotariModifica.Items.Clear();
            lstDotariModifica.Items.Add("Aer condiționat");
            lstDotariModifica.Items.Add("Navigație GPS");
            lstDotariModifica.Items.Add("Cutie automată");
            lstDotariModifica.Items.Add("Scaune încălzite");
            lstDotariModifica.Items.Add("Senzori parcare");
            lstDotariModifica.Items.Add("Cameră marșarier");
        }

        private void NavModifica_Click(object sender, RoutedEventArgs e)
        {
            ArataPagina(PaginaModifica);
            // Curăță căutarea și selecția anterioară
            txtCautaModifica.Clear();
            lstRezultateModifica.ItemsSource = null;
            tranzactieCurentaModificata = null;
            // Asigură că lista de dotări este populată
            PopuleazaDotariModifica();
        }

        private void BtnCautaTranzactiiModifica_Click(object sender, RoutedEventArgs e)
        {
            string criteriu = txtCautaModifica.Text.Trim();
            if (string.IsNullOrWhiteSpace(criteriu))
            {
                MessageBox.Show("Introduceți un criteriu de căutare (model mașină sau nume cumpărător).",
                                "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<Tranzactie> toate = _admin.GetTranzactii();
            List<Tranzactie> rezultate = toate.FindAll(t =>
                (t.Masina?.Model?.IndexOf(criteriu, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (t.Cumparator?.Nume?.IndexOf(criteriu, StringComparison.OrdinalIgnoreCase) >= 0)
            );

            if (rezultate.Count == 0)
            {
                MessageBox.Show("Nu s-a găsit nicio tranzacție pentru criteriul specificat.",
                                "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
                lstRezultateModifica.ItemsSource = null;
            }
            else
            {
                lstRezultateModifica.ItemsSource = rezultate;
            }
        }

        private void lstRezultateModifica_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRezultateModifica.SelectedItem is not Tranzactie t) return;

            tranzactieCurentaModificata = t;

            txtVanzatorModifica.Text = t.Vanzator?.Nume ?? string.Empty;
            txtCumparatorModifica.Text = t.Cumparator?.Nume ?? string.Empty;
            txtFirmaModifica.Text = t.Masina?.NumeFirma ?? string.Empty;
            txtModelModifica.Text = t.Masina?.Model ?? string.Empty;
            txtAnModifica.Text = t.Masina?.AnFabricatie.ToString() ?? string.Empty;
            txtPretModifica.Text = t.Pret.ToString();
            dpDataModifica.SelectedDate = t.DataTranzactie;
            SeteazaDotariSelectate(t.Masina?.Optiuni ?? OptiuniMasina.Niciuna);
        }

        private void BtnActualizeaza_Click(object sender, RoutedEventArgs e)
        {
            if (tranzactieCurentaModificata == null)
            {
                MessageBox.Show("Selectați o tranzacție din lista de rezultate pentru actualizare.",
                                "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtVanzatorModifica.Text) ||
                string.IsNullOrWhiteSpace(txtCumparatorModifica.Text) ||
                string.IsNullOrWhiteSpace(txtFirmaModifica.Text) ||
                string.IsNullOrWhiteSpace(txtModelModifica.Text))
            {
                MessageBox.Show("Completați vânzătorul, cumpărătorul, marca și modelul mașinii.",
                                "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtAnModifica.Text, out int an) || an < AN_MINIM || an > DateTime.Now.Year)
            {
                MessageBox.Show("Introduceți un an de fabricație valid.",
                                "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPretModifica.Text, out decimal pret) || pret < PRET_MINIM)
            {
                MessageBox.Show("Introduceți un preț valid.",
                                "Validare", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Tranzactie tranzactieActualizata = new Tranzactie
            {
                Id = tranzactieCurentaModificata.Id,
                Vanzator = new Persoana(txtVanzatorModifica.Text.Trim()),
                Cumparator = new Persoana(txtCumparatorModifica.Text.Trim()),
                Masina = new Masina
                {
                    NumeFirma = txtFirmaModifica.Text.Trim(),
                    Model = txtModelModifica.Text.Trim(),
                    AnFabricatie = an,
                    Culoare = tranzactieCurentaModificata.Masina?.Culoare ?? CuloareMasina.Alb,
                    Combustibil = tranzactieCurentaModificata.Masina?.Combustibil ?? "Benzină",
                    Optiuni = ColecteazaOptiuniModifica()
                },
                Pret = pret,
                DataTranzactie = dpDataModifica.SelectedDate ?? DateTime.Now
            };

            bool actualizat = _admin.UpdateTranzactie(tranzactieActualizata);

            if (!actualizat)
            {
                MessageBox.Show("Tranzacția nu a fost găsită în fișier și nu a putut fi actualizată.",
                                "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show(
                "Tranzacția a fost actualizată și salvată în fișier!",
                "Succes",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            IncarcaTranzactii();
            tranzactieCurentaModificata = null;
            txtCautaModifica.Clear();
            lstRezultateModifica.ItemsSource = null;
            // Resetează câmpurile de editare
            txtVanzatorModifica.Clear();
            txtCumparatorModifica.Clear();
            txtFirmaModifica.Clear();
            txtModelModifica.Clear();
            txtAnModifica.Clear();
            txtPretModifica.Clear();
            dpDataModifica.SelectedDate = null;
            SeteazaDotariSelectate(OptiuniMasina.Niciuna);
        }

        // ==================== METODE AJUTĂTOARE PENTRU DOTĂRI ====================

        private void SeteazaDotariSelectate(OptiuniMasina optiuni)
        {
            lstDotariModifica.SelectedItems.Clear();

            foreach (object item in lstDotariModifica.Items)
            {
                if (item is string dotare && optiuni.HasFlag(ConvertesteDotare(dotare)) && ConvertesteDotare(dotare) != OptiuniMasina.Niciuna)
                {
                    lstDotariModifica.SelectedItems.Add(item);
                }
            }
        }

        private OptiuniMasina ColecteazaOptiuniModifica()
        {
            OptiuniMasina optiuni = OptiuniMasina.Niciuna;

            foreach (object item in lstDotariModifica.SelectedItems)
            {
                if (item is string dotare)
                {
                    optiuni |= ConvertesteDotare(dotare);
                }
            }

            return optiuni;
        }

        private OptiuniMasina ConvertesteDotare(string dotare)
        {
            return dotare switch
            {
                "Aer condiționat" => OptiuniMasina.AerConditionat,
                "Navigație GPS" => OptiuniMasina.Navigatie,
                "Cutie automată" => OptiuniMasina.CutieAutomata,
                "Scaune încălzite" => OptiuniMasina.ScauneIncalzite,
                "Senzori parcare" => OptiuniMasina.SenzoriParcare,
                "Cameră marșarier" => OptiuniMasina.CameraMarsarier,
                _ => OptiuniMasina.Niciuna
            };
        }

        // ==================== CELELALTE PAGINI ȘI METODE ====================

        private void NavLista_Click(object sender, RoutedEventArgs e)
        {
            ArataPagina(PaginaLista);
            IncarcaTranzactii();
        }

        private void NavAdauga_Click(object sender, RoutedEventArgs e)
        {
            ArataPagina(PaginaAdauga);
            ResetFormular();
        }

        private void NavCauta_Click(object sender, RoutedEventArgs e)
        {
            ArataPagina(PaginaCauta);
            brdRezultat.Visibility = Visibility.Collapsed;
            brdNegasit.Visibility = Visibility.Collapsed;
            txtCautaClient.Clear();
            txtCautaMarca.Clear();
        }

        private void ArataPagina(Grid pagina)
        {
            PaginaLista.Visibility = Visibility.Collapsed;
            PaginaAdauga.Visibility = Visibility.Collapsed;
            PaginaCauta.Visibility = Visibility.Collapsed;
            PaginaModifica.Visibility = Visibility.Collapsed;

            pagina.Visibility = Visibility.Visible;
        }

        private void BtnReincarca_Click(object sender, RoutedEventArgs e)
        {
            IncarcaTranzactii();
        }

        private void IncarcaTranzactii()
        {
            List<Tranzactie> lista = _admin.GetTranzactii();

            lblTotalTranzactii.Content = lista.Count.ToString();
            lblSubtitluLista.Text = lista.Count == 0
                ? "Nu există tranzacții înregistrate"
                : $"{lista.Count} tranzacții înregistrate";

            StackTranzactii.Children.Clear();

            if (lista.Count == 0)
            {
                StackTranzactii.Children.Add(new TextBlock
                {
                    Text = "Nu există tranzacții salvate.",
                    Foreground = Brushes.White,
                    FontSize = 16,
                    Margin = new Thickness(0, 20, 0, 0)
                });

                return;
            }

            foreach (Tranzactie t in lista)
                StackTranzactii.Children.Add(CreeazaCardTranzactie(t));
        }

        private UIElement CreeazaCardTranzactie(Tranzactie t)
        {
            Border card = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#172033")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155")),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(18),
                Padding = new Thickness(18),
                Margin = new Thickness(0, 0, 0, 12)
            };

            StackPanel panel = new StackPanel();

            panel.Children.Add(new TextBlock
            {
                Text = $"#{t.Id:D3}  {t.Masina?.TipComplet}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#38BDF8")),
                FontSize = 18,
                FontWeight = FontWeights.Bold
            });

            panel.Children.Add(new TextBlock
            {
                Text = $"Vânzător: {t.Vanzator?.Nume} | Cumpărător: {t.Cumparator?.Nume}",
                Foreground = Brushes.White,
                FontSize = 14,
                Margin = new Thickness(0, 8, 0, 0)
            });

            panel.Children.Add(new TextBlock
            {
                Text = $"An: {t.Masina?.AnFabricatie} | Culoare: {t.Masina?.Culoare} | Combustibil: {t.Masina?.Combustibil} | Preț: {t.Pret:N0} RON | Data: {t.DataTranzactieShort}",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8")),
                FontSize = 13,
                Margin = new Thickness(0, 5, 0, 0)
            });

            Button btnSterge = new Button
            {
                Content = "Șterge",
                Width = 90,
                Height = 32,
                Margin = new Thickness(0, 12, 0, 0),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B0A19")),
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5370")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5370")),
                Tag = t.Id,
                Cursor = Cursors.Hand
            };

            btnSterge.Click += BtnSterge_Click;
            panel.Children.Add(btnSterge);

            card.Child = panel;
            return card;
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Sigur doriți să ștergeți tranzacția #{id:D3}?",
                    "Confirmare",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _admin.DeleteTranzactie(id);
                    IncarcaTranzactii();
                }
            }
        }

        private void BtnSalveaza_Click(object sender, RoutedEventArgs e)
        {
            brdMesaj.Visibility = Visibility.Collapsed;
            ResetErori();

            if (!ValideazaDate(out int an, out decimal pret, out DateTime data))
                return;

            Tranzactie tranzactie = new Tranzactie
            {
                Vanzator = new Persoana(txtVanzator.Text.Trim()),
                Cumparator = new Persoana(txtCumparator.Text.Trim()),
                Masina = new Masina
                {
                    NumeFirma = txtFirma.Text.Trim(),
                    Model = txtModel.Text.Trim(),
                    AnFabricatie = an,
                    Culoare = ConvertesteCuloare(cboCuloare.SelectedItem?.ToString()),
                    Combustibil = GetCombustibilSelectat(),
                    Optiuni = ColecteazaOptiuni()
                },
                DataTranzactie = data,
                Pret = pret
            };

            _admin.AddTranzactie(tranzactie);

            AfiseazaMesaj("✓ Tranzacția a fost salvată cu succes!", true);
            ResetFormular();
            IncarcaTranzactii();
        }

        private bool ValideazaDate(out int an, out decimal pret, out DateTime data)
        {
            bool valid = true;
            an = 0;
            pret = 0;
            data = DateTime.Now;

            if (!ValideazaText(txtVanzator.Text, MIN_NUME, MAX_NUME))
            {
                lblVanzator.Foreground = culoareEroare;
                valid = false;
            }

            if (!ValideazaText(txtCumparator.Text, MIN_NUME, MAX_NUME))
            {
                lblCumparator.Foreground = culoareEroare;
                valid = false;
            }

            if (!ValideazaText(txtFirma.Text, MIN_FIRMA, MAX_FIRMA))
            {
                lblFirma.Foreground = culoareEroare;
                valid = false;
            }

            if (!ValideazaText(txtModel.Text, MIN_MODEL, MAX_MODEL))
            {
                lblModel.Foreground = culoareEroare;
                valid = false;
            }

            if (!int.TryParse(txtAn.Text, out an) || an < AN_MINIM || an > DateTime.Now.Year)
            {
                lblAn.Foreground = culoareEroare;
                valid = false;
            }

            if (!DateTime.TryParse(txtData.Text, out data))
            {
                lblData.Foreground = culoareEroare;
                valid = false;
            }

            if (!decimal.TryParse(txtPret.Text, out pret) || pret < PRET_MINIM)
            {
                lblPret.Foreground = culoareEroare;
                valid = false;
            }

            if (!valid)
            {
                AfiseazaMesaj("⚠ Verificați câmpurile marcate cu roșu. Datele introduse sunt invalide.", false);
            }

            return valid;
        }

        private bool ValideazaText(string text, int minim, int maxim)
        {
            return !string.IsNullOrWhiteSpace(text)
                   && text.Trim().Length >= minim
                   && text.Trim().Length <= maxim;
        }

        private void ResetErori()
        {
            lblVanzator.Foreground = culoareNormala;
            lblCumparator.Foreground = culoareNormala;
            lblFirma.Foreground = culoareNormala;
            lblModel.Foreground = culoareNormala;
            lblAn.Foreground = culoareNormala;
            lblData.Foreground = culoareNormala;
            lblPret.Foreground = culoareNormala;
        }

        private string GetCombustibilSelectat()
        {
            if (rbDiesel.IsChecked == true)
                return "Diesel";

            if (rbHibrid.IsChecked == true)
                return "Hibrid";

            if (rbElectric.IsChecked == true)
                return "Electric";

            return "Benzină";
        }

        private OptiuniMasina ColecteazaOptiuni()
        {
            OptiuniMasina optiuni = OptiuniMasina.Niciuna;

            if (chkAer.IsChecked == true)
                optiuni |= OptiuniMasina.AerConditionat;

            if (chkNav.IsChecked == true)
                optiuni |= OptiuniMasina.Navigatie;

            if (chkAuto.IsChecked == true)
                optiuni |= OptiuniMasina.CutieAutomata;

            if (chkScaune.IsChecked == true)
                optiuni |= OptiuniMasina.ScauneIncalzite;

            if (chkSenzori.IsChecked == true)
                optiuni |= OptiuniMasina.SenzoriParcare;

            if (chkCamera.IsChecked == true)
                optiuni |= OptiuniMasina.CameraMarsarier;

            return optiuni;
        }

        private CuloareMasina ConvertesteCuloare(string? culoare)
        {
            return culoare switch
            {
                "Alb" => CuloareMasina.Alb,
                "Negru" => CuloareMasina.Negru,
                "Gri" => CuloareMasina.Gri,
                "Argintiu" => CuloareMasina.Argintiu,
                "Roșu" => CuloareMasina.Rosu,
                "Bordo" => CuloareMasina.Bordo,
                "Albastru" => CuloareMasina.Albastru,
                "Bleumarin" => CuloareMasina.Bleumarin,
                "Verde" => CuloareMasina.Verde,
                "Verde închis" => CuloareMasina.VerdeInchis,
                "Galben" => CuloareMasina.Galben,
                "Portocaliu" => CuloareMasina.Portocaliu,
                "Maro" => CuloareMasina.Maro,
                "Bej" => CuloareMasina.Bej,
                "Auriu" => CuloareMasina.Auriu,
                "Mov" => CuloareMasina.Mov,
                "Roz" => CuloareMasina.Roz,
                "Turcoaz" => CuloareMasina.Turcoaz,
                "Cameleon" => CuloareMasina.Cameleon,
                "Mat" => CuloareMasina.Mat,
                _ => CuloareMasina.Alb
            };
        }

        private void AfiseazaMesaj(string text, bool succes)
        {
            lblMesaj.Text = text;

            lblMesaj.Foreground = succes
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E676"))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5370"));

            brdMesaj.Visibility = Visibility.Visible;
        }

        private void ResetFormular()
        {
            txtVanzator.Clear();
            txtCumparator.Clear();
            txtFirma.Clear();
            txtModel.Clear();
            txtAn.Clear();
            txtPret.Clear();
            txtData.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (cboCuloare.Items.Count > 0)
                cboCuloare.SelectedIndex = 0;

            rbBenzina.IsChecked = true;

            chkAer.IsChecked = false;
            chkNav.IsChecked = false;
            chkAuto.IsChecked = false;
            chkScaune.IsChecked = false;
            chkSenzori.IsChecked = false;
            chkCamera.IsChecked = false;

            ResetErori();
        }

        private void BtnAnuleaza_Click(object sender, RoutedEventArgs e)
        {
            ResetFormular();
            brdMesaj.Visibility = Visibility.Collapsed;
        }

        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            brdRezultat.Visibility = Visibility.Collapsed;
            brdNegasit.Visibility = Visibility.Collapsed;
            stackRezultat.Children.Clear();

            string clientCautat = txtCautaClient.Text.Trim();
            string marcaCautata = txtCautaMarca.Text.Trim();

            if (string.IsNullOrWhiteSpace(clientCautat) && string.IsNullOrWhiteSpace(marcaCautata))
            {
                brdNegasit.Visibility = Visibility.Visible;
                return;
            }

            List<Tranzactie> lista = _admin.GetTranzactii();

            List<Tranzactie> rezultate = lista.FindAll(t =>
            {
                bool criteriuClient = string.IsNullOrWhiteSpace(clientCautat) ||
                    (t.Cumparator?.Nume?.Contains(clientCautat, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (t.Vanzator?.Nume?.Contains(clientCautat, StringComparison.OrdinalIgnoreCase) ?? false);

                bool criteriuMarca = string.IsNullOrWhiteSpace(marcaCautata) ||
                    (t.Masina?.NumeFirma?.Contains(marcaCautata, StringComparison.OrdinalIgnoreCase) ?? false);

                return criteriuClient && criteriuMarca;
            });

            if (rezultate.Count == 0)
            {
                brdNegasit.Visibility = Visibility.Visible;
                return;
            }

            foreach (Tranzactie t in rezultate)
            {
                stackRezultat.Children.Add(CreeazaCardTranzactie(t));
            }

            brdRezultat.Visibility = Visibility.Visible;
        }

        private void AfiseazaRezultatCautare(Tranzactie t)
        {
            stackRezultat.Children.Clear();

            stackRezultat.Children.Add(CreeazaRandDetaliu("ID", $"#{t.Id:D3}"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("Vânzător", t.Vanzator?.Nume ?? "-"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("Cumpărător", t.Cumparator?.Nume ?? "-"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("Mașină", t.Masina?.TipComplet ?? "-"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("An", t.Masina?.AnFabricatie.ToString() ?? "-"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("Culoare", t.Masina?.Culoare.ToString() ?? "-"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("Dotări", t.Masina?.Optiuni.ToString() ?? "-"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("Preț", $"{t.Pret:N0} RON"));
            stackRezultat.Children.Add(CreeazaRandDetaliu("Data", t.DataTranzactieShort));

            brdRezultat.Visibility = Visibility.Visible;
        }

        private UIElement CreeazaRandDetaliu(string titlu, string valoare)
        {
            TextBlock tb = new TextBlock
            {
                Text = $"{titlu}: {valoare}",
                Foreground = Brushes.White,
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 5)
            };

            return tb;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}