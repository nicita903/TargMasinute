namespace LibrarieModele
{
    public enum CuloareMasina
    {
        Alb,
        Negru,
        Gri,
        Argintiu,
        Rosu,
        Bordo,
        Albastru,
        Bleumarin,
        Verde,
        VerdeInchis,
        Galben,
        Portocaliu,
        Maro,
        Bej,
        Auriu,
        Mov,
        Roz,
        Turcoaz,
        Cameleon,
        Mat
    }

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