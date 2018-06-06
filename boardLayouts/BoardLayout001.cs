public class BoardLayout001 : BoardLayout
{
    string S1 = SpecialType.Unmovable.ToString();

    protected override void InitBoardLayout()
    {
        layout = new string[,] {
            { __, XX, __, __, __, XX,__ },
            { XX, XX, __, __, __, XX,XX },
            { XX, XX, __, __, __, XX,XX },
            { S1, __, __, __, __, __,S1 },
            { __, __, __, __, __, __,__ },
            { __, __, __, __, __, __,__ },
            { __, __, __, __, __, __,__ },
            { XX, __, __, __, __, __,XX }
        };
    }
}
