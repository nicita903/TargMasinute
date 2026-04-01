using NivelEntitati;
using System.Collections.Generic;

namespace NivelStocareData
{
    public class Masina
    {
        public int Id { get; set; }
        public string NumeFirma { get; set; }
        public string Model { get; set; }
        public int AnFabricatie { get; set; }
        public CuloareMasina Culoare { get; set; }
        public OptiuniMasina Optiuni { get; set; }

        public Masina()
        {
            Optiuni = OptiuniMasina.Niciuna;
            NumeFirma = string.Empty;
            Model = string.Empty;
            Culoare = CuloareMasina.Alb;
        }

        public string TipComplet
        {
            get { return $"{NumeFirma} {Model}"; }
        }
    }
}