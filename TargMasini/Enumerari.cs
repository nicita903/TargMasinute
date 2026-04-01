namespace NivelEntitati
{
    // Enum simplu pentru culoare
    public enum CuloareMasina
    {
        Rosu,
        Alb,
        Negru,
        Albastru,
        Gri,
        Verde
    }

    // Enum cu atribut [Flags] pentru optiuni (permite combinatii)
    [Flags]
    public enum OptiuniMasina
    {
        Niciuna = 0,
        AerConditionat = 1,
        Navigatie = 2,
        CutieAutomata = 4,
        ScauneIncalzite = 8,
        SenzoriParcare = 16,
        CameraMarsarier = 32
    }
}