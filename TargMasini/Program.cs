using System;
using System.Collections.Generic;

public class Masina
{
    public int Id { get; set; }
    public string NumeFirma { get; set; }
    public string Model { get; set; }
    public int AnFabricatie { get; set; }
    public string Culoare { get; set; }
    public List<string> Optiuni { get; set; }

    public string TipComplet => $"{NumeFirma} {Model}";

    public Masina()
    {
        Optiuni = new List<string>();
    }
}

public class Persoana
{
    public int Id { get; set; }
    public string Nume { get; set; }

    public Persoana(string nume)
    {
        Nume = nume;
    }
}

public class Tranzactie
{
    public int Id { get; set; }
    public Persoana Vanzator { get; set; }
    public Persoana Cumparator { get; set; }
    public Masina Masina { get; set; }
    public DateTime DataTranzactie { get; set; }
    public decimal Pret { get; set; }

    public Tranzactie()
    {
        DataTranzactie = DateTime.Now;
    }

    public string DataTranzactieShort => DataTranzactie.ToString("yyyy-MM-dd");
}

public class RaportCautareMasina
{
    public string Criteriu { get; set; } // "firma" sau "model"
    public string Valoare { get; set; }
    public int NumarVanzari { get; set; }
    public DateTime DataInceput { get; set; }
    public DateTime DataSfarsit { get; set; }
}

public class GraficPret
{
    public string Model { get; set; }
    public DateTime DataInceput { get; set; }
    public DateTime DataSfarsit { get; set; }
    public List<DataPret> PuncteGrafic { get; set; }

    public GraficPret()
    {
        PuncteGrafic = new List<DataPret>();
    }
}

public class DataPret
{
    public DateTime Data { get; set; }
    public decimal Pret { get; set; }
}