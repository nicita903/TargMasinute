using NivelEntitati;
using NivelStocareData;
using System;
using System.Collections.Generic;

namespace TargMasini
{
    class Program
    {
        private static AdministrareTranzactiiMemorie admin = new AdministrareTranzactiiMemorie();

        static void Main(string[] args)
        {
            bool ruleaza = true;
            while (ruleaza)
            {
                MeniuPrincipal();
                string optiune = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        CitireDateTastatura();      // citirea datelor de la tastatura
                        break;
                    case "2":
                        AfisareDate();               // afisarea datelor dintr-o colectie
                        break;
                    case "3":
                        CautareDupaCriterii();       // cautarea dupa anumite criterii
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
            Console.WriteLine("1. Adauga tranzactie noua (citire de la tastatura)");
            Console.WriteLine("2. Afiseaza toate tranzactiile");
            Console.WriteLine("3. Cauta tranzactii dupa criterii");
            Console.WriteLine("0. Iesire");
            Console.Write("Alegeti o optiune: ");
        }

        // CITIREA DATELOR DE LA TASTATURA
        static void CitireDateTastatura()
        {
            Console.WriteLine("\n--- Adaugare tranzactie noua ---");

            Tranzactie tranzactie = new Tranzactie();

            // Citire date vanzator
            Console.Write("Nume vanzator: ");
            tranzactie.Vanzator.Nume = Console.ReadLine();

            // Citire date cumparator
            Console.Write("Nume cumparator: ");
            tranzactie.Cumparator.Nume = Console.ReadLine();

            // Citire date masina
            Console.Write("Firma masina: ");
            tranzactie.Masina.NumeFirma = Console.ReadLine();

            Console.Write("Model masina: ");
            tranzactie.Masina.Model = Console.ReadLine();

            Console.Write("An fabricatie: ");
            tranzactie.Masina.AnFabricatie = int.Parse(Console.ReadLine());

            Console.Write("Culoare (Rosu, Alb, Negru, Albastru, Gri, Verde): ");
            string culoareInput = Console.ReadLine();
            tranzactie.Masina.Culoare = (CuloareMasina)Enum.Parse(typeof(CuloareMasina), culoareInput, true);

            Console.Write("Optiuni (AerConditionat, Navigatie, CutieAutomata, ScauneIncalzite, SenzoriParcare, CameraMarsarier - separate prin virgula): ");
            string optiuniInput = Console.ReadLine();
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
            tranzactie.DataTranzactie = DateTime.Parse(Console.ReadLine());

            Console.Write("Pret: ");
            tranzactie.Pret = decimal.Parse(Console.ReadLine());

            // SALVAREA DATELOR INTR-O COLECTIE DE OBIECTE
            admin.AdaugaTranzactie(tranzactie);
            Console.WriteLine("\nTranzactie adaugata cu succes!");
        }

        // AFISAREA DATELOR DINTR-O COLECTIE DE OBIECTE
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

                // Afisare optiuni
                Console.Write("Optiuni: ");
                if (t.Masina.Optiuni != OptiuniMasina.Niciuna)
                {
                    Console.WriteLine(t.Masina.Optiuni);
                }
                else
                {
                    Console.WriteLine("Nicio optiune");
                }

                Console.WriteLine($"Data: {t.DataTranzactieShort}");
                Console.WriteLine($"Pret: {t.Pret:C}");
                Console.WriteLine("------------------------");
            }

            Console.WriteLine($"\nTotal tranzactii: {tranzactii.Count}");
        }

        // CAUTAREA DUPA ANUMITE CRITERII
        static void CautareDupaCriterii()
        {
            Console.WriteLine("\n--- Cautare tranzactii ---");
            Console.WriteLine("1. Cauta dupa firma masina");
            Console.WriteLine("2. Cauta dupa model masina");
            Console.WriteLine("3. Cauta dupa vanzator");
            Console.WriteLine("4. Cauta dupa cumparator");
            Console.WriteLine("5. Cauta dupa interval pret");
            Console.WriteLine("6. Cauta dupa an fabricatie");
            Console.Write("Alegeti criteriul: ");

            string optiune = Console.ReadLine();
            List<Tranzactie> rezultate = new List<Tranzactie>();

            switch (optiune)
            {
                case "1": // Cautare dupa firma
                    Console.Write("Introduceti firma: ");
                    string firma = Console.ReadLine();
                    rezultate = admin.GetTranzactiiByMasina(firma, null);
                    Console.WriteLine($"\n=== Tranzactii pentru firma '{firma}' ===");
                    break;

                case "2": // Cautare dupa model
                    Console.Write("Introduceti modelul: ");
                    string model = Console.ReadLine();
                    rezultate = admin.GetTranzactiiByMasina(null, model);
                    Console.WriteLine($"\n=== Tranzactii pentru modelul '{model}' ===");
                    break;

                case "3": // Cautare dupa vanzator
                    Console.Write("Introduceti numele vanzatorului: ");
                    string vanzator = Console.ReadLine();
                    rezultate = admin.GetTranzactiiByVanzator(vanzator);
                    Console.WriteLine($"\n=== Tranzactii pentru vanzatorul '{vanzator}' ===");
                    break;

                case "4": // Cautare dupa cumparator
                    Console.Write("Introduceti numele cumparatorului: ");
                    string cumparator = Console.ReadLine();
                    rezultate = admin.GetTranzactiiByCumparator(cumparator);
                    Console.WriteLine($"\n=== Tranzactii pentru cumparatorul '{cumparator}' ===");
                    break;

                case "5": // Cautare dupa interval pret
                    Console.Write("Pret minim: ");
                    decimal pretMin = decimal.Parse(Console.ReadLine());
                    Console.Write("Pret maxim: ");
                    decimal pretMax = decimal.Parse(Console.ReadLine());
                    rezultate = admin.GetTranzactiiByPret(pretMin, pretMax);
                    Console.WriteLine($"\n=== Tranzactii cu pret intre {pretMin:C} si {pretMax:C} ===");
                    break;

                case "6": // Cautare dupa an fabricatie
                    Console.Write("Anul fabricatiei: ");
                    int an = int.Parse(Console.ReadLine());
                    rezultate = admin.GetTranzactiiByAnFabricatie(an);
                    Console.WriteLine($"\n=== Tranzactii pentru masini fabricate in anul {an} ===");
                    break;

                default:
                    Console.WriteLine("Optiune invalida!");
                    return;
            }

            // Afisare rezultate cautare
            if (rezultate.Count == 0)
            {
                Console.WriteLine("Nu s-au gasit tranzactii care sa corespunda criteriilor.");
            }
            else
            {
                foreach (Tranzactie t in rezultate)
                {
                    Console.WriteLine($"\nID: {t.Id}");
                    Console.WriteLine($"Vanzator: {t.Vanzator.Nume}");
                    Console.WriteLine($"Cumparator: {t.Cumparator.Nume}");
                    Console.WriteLine($"Masina: {t.Masina.TipComplet}");
                    Console.WriteLine($"An fabricatie: {t.Masina.AnFabricatie}");
                    Console.WriteLine($"Culoare: {t.Masina.Culoare}");
                    Console.WriteLine($"Data: {t.DataTranzactieShort}");
                    Console.WriteLine($"Pret: {t.Pret:C}");
                    Console.WriteLine("------------------------");
                }
                Console.WriteLine($"\nTotal rezultate: {rezultate.Count}");
            }
        }
    }
}