public class Blueprint001 : Blueprint
{
	string S1 = GemType.Sapphire.ToString () + GemTier._Tier2.ToString();
    string s2 = GemType.Sapphire.ToString () + GemTier._Tier1.ToString();

    protected override void initBlueprint ()
	{
        blueprint = new string[,] {
            { __, __, __, __, __, __,__ },
            { __, __, __, S1, __, __,__ },
            { __, __, s2, __, s2, __,__ },
            { __, s2, __, __, __, s2,__ },
            { __, s2, __, __, __, s2,__ },
            { __, __, s2, __, s2, __,__ },
            { __, __, __, s2, __, __,__ },
            { __, __, __, __, __, __,__ }
        };
	}

}

