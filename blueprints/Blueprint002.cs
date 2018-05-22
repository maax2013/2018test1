public class Blueprint002 : Blueprint
{
    string S1 = GemType.Sapphire.ToString() + "2";
    string s2 = GemType.Gold.ToString() + "0";

    protected override void init()
    {
        blueprint = new string[,] {
          { __, __, __, __, __, __ },
          { __, __, S1, S1, __, __ },
          { __, __, S1, S1, __, __ },
          { __, s2, __, __, s2, __ },
          { __, s2, __, __, s2, __ },
          { __, __, s2, s2, __, __ },
          { __, __, __, __, __, __ }
        };
    }

}

