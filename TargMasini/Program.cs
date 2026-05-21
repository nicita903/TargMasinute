using LibrarieModele;
using NivelStocareDate;

namespace TargMasini
{
    class Program
    {
        private static IStocareData? admin;

        static void Main(string[] args)
        {
            // Alege locul unde vrei sa salvezi fisierul
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string caleFisier = desktopPath + "\\Tranzactii.txt";

            admin = new AdministrareTranzactiiFisierText(caleFisier);

            Console.WriteLine($"Fisierul se salveaza la: {caleFisier}");
            Console.WriteLine();

            bool ruleaza = true;
            while (ruleaza)
            {
                MeniuPrincipal();
                string? optiune = Console.ReadLine();  // Adaugat ?

                switch (optiune)
                {
                    case "1":
                        CitireDateTastatura();
                        break;
                    case "2":
                        AfisareDate();
                        break;
                    case "3":
                        CautareDupaId();
                        break;
                    case "0":
                        ruleaza = false;
                        Console.WriteLine("La revedere!");
                        break;
                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }

                Console.WriteLine("\nApasati o tasta pentru a continua...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void MeniuPrincipal()
        {
            Console.WriteLine("=== TARG DE MASINI ===");
            Console.WriteLine("1. Adauga tranzactie noua");
            Console.WriteLine("2. Afiseaza toate tranzactiile");
            Console.WriteLine("3. Cauta tranzactie dupa ID");
            Console.WriteLine("0. Iesire");
            Console.Write("Alegeti o optiune: ");
        }

        static void CitireDateTastatura()
        {
            Console.WriteLine("\n--- Adaugare tranzactie noua ---");

            Tranzactie tranzactie = new Tranzactie();

            Console.Write("Nume vanzator: ");
            string? input = Console.ReadLine();
            tranzactie.Vanzator.Nume = input ?? string.Empty;

            Console.Write("Nume cumparator: ");
            input = Console.ReadLine();
            tranzactie.Cumparator.Nume = input ?? string.Empty;

            Console.Write("Firma masina: ");
            input = Console.ReadLine();
            tranzactie.Masina.NumeFirma = input ?? string.Empty;

            Console.Write("Model masina: ");
            input = Console.ReadLine();
            tranzactie.Masina.Model = input ?? string.Empty;

            Console.Write("An fabricatie: ");
            input = Console.ReadLine();
            tranzactie.Masina.AnFabricatie = int.Parse(input ?? "0");

            Console.Write("Culoare (Rosu, Alb, Negru, Albastru, Gri, Verde): ");
            input = Console.ReadLine();
            tranzactie.Masina.Culoare = (CuloareMasina)Enum.Parse(typeof(CuloareMasina), input ?? "Alb", true);

            Console.Write("Optiuni (AerConditionat, Navigatie, CutieAutomata, ScauneIncalzite, SenzoriParcare, CameraMarsarier - separate prin virgula): ");
            input = Console.ReadLine();
            string optiuniInput = input ?? "";
            string[] optiuniArray = optiuniInput.Split(',');
            OptiuniMasina optiuniSelectate = OptiuniMasina.Niciuna;
            foreach (string opt in optiuniArray)
            {
                string optTrim = opt.Trim();
                if (!string.IsNullOrEmpty(optTrim))
                {
                    optiuniSelectate |= (OptiuniMasina)Enum.Parse(typeof(OptiuniMasina), optTrim, true);
                }
            }
            tranzactie.Masina.Optiuni = optiuniSelectate;

            Console.Write("Data tranzactie (yyyy-mm-dd): ");
            input = Console.ReadLine();
            tranzactie.DataTranzactie = DateTime.Parse(input ?? DateTime.Now.ToString("yyyy-MM-dd"));

            Console.Write("Pret: ");
            input = Console.ReadLine();
            tranzactie.Pret = decimal.Parse(input ?? "0");

            admin.AddTranzactie(tranzactie);
            Console.WriteLine("\nTranzactie adaugata cu succes!");
        }

        static void AfisareDate()
        {
            List<Tranzactie> tranzactii = admin.GetTranzactii();

            if (tranzactii.Count == 0)
            {
                Console.WriteLine("\nNu exista tranzactii inregistrate.");
                return;
            }

            Console.WriteLine("\n=== TOATE TRANZACTIILE ===");

            foreach (Tranzactie t in tranzactii)
            {
                Console.WriteLine($"\nID: {t.Id}");
                Console.WriteLine($"Vanzator: {t.Vanzator.Nume}");
                Console.WriteLine($"Cumparator: {t.Cumparator.Nume}");
                Console.WriteLine($"Masina: {t.Masina.TipComplet}");
                Console.WriteLine($"An fabricatie: {t.Masina.AnFabricatie}");
                Console.WriteLine($"Culoare: {t.Masina.Culoare}");
                Console.WriteLine($"Optiuni: {t.Masina.Optiuni}");
                Console.WriteLine($"Data: {t.DataTranzactieShort}");
                Console.WriteLine($"Pret: {t.Pret:C}");
                Console.WriteLine("------------------------");
            }

            Console.WriteLine($"\nTotal tranzactii: {tranzactii.Count}");
        }

        static void CautareDupaId()
        {
            Console.Write("\nIntroduceti ID-ul tranzactiei: ");
            string? input = Console.ReadLine();
            int id = int.Parse(input ?? "0");

            Tranzactie? t = admin.GetTranzactie(id);

            if (t == null)
            {
                Console.WriteLine("Tranzactie negasita!");
            }
            else
            {
                Console.WriteLine($"\nID: {t.Id}");
                Console.WriteLine($"Vanzator: {t.Vanzator.Nume}");
                Console.WriteLine($"Cumparator: {t.Cumparator.Nume}");
                Console.WriteLine($"Masina: {t.Masina.TipComplet}");
                Console.WriteLine($"An fabricatie: {t.Masina.AnFabricatie}");
                Console.WriteLine($"Culoare: {t.Masina.Culoare}");
                Console.WriteLine($"Optiuni: {t.Masina.Optiuni}");
                Console.WriteLine($"Data: {t.DataTranzactieShort}");
                Console.WriteLine($"Pret: {t.Pret:C}");
            }
        }
    }
}