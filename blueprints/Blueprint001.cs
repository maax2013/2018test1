public class Blueprint001 : Blueprint
{
	string S1 = GemType.Sapphire.ToString () + GemTier._Tier3.ToString();
	string s2 = GemType.Gold.ToString () + GemTier._Tier1.ToString();

	protected override void init ()
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

