public class BoardLayout002 : BoardLayout
{
    //string S1 = GemType.Sapphire.ToString() + GemTier._Tier2.ToString();
    //string s2 = GemType.Sapphire.ToString() + GemTier._Tier1.ToString();

    protected override void InitBoardLayout()
    {
        layout = new string[,] {
            { XX, __, __, __, __, XX },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { XX, __, __, __, __, XX }
        };
    }
}
