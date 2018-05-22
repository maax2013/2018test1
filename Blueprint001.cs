public class Blueprint001
{
	string __ = "";
	string S1 = GemType.Sapphire.ToString () + "2";
	string s2 = GemType.Gold.ToString () + "0";

	string[,] blueprint1;



	public string[,] getBlueprint1 ()
	{
		init ();
		return BoardUtilities.flipBlueprintTableCoord (blueprint1);
	}

	void init ()
	{
		blueprint1 = new string[,] {
			{ __, __, __, __, __, __ },
			{ __, __, S1, S1, __, __ },
			{ __, __, S1, S1, __, __ },
			{ __, s2, __, __, s2, __ },
			{ __, s2, __, __, s2, __ },
			{ __, __, s2, s2, __, __ },
			{ __, __, __, __, __, __ }
		};
	}

	//	string[,] flipBlueprintTableC__rd (string[,] originalT)
	//	{
	//		/*flip the inner and outer array, so the table has same c__rd as the gameboard*/
	//		string[,] targetT = new string[originalT.GetLength (1), originalT.GetLength (0)];
	//		int column = targetT.GetLength (0);
	//		int row = targetT.GetLength (1);
	//		for (int c = 0; c < column; c++) {
	//			for (int r = 0; r < row; r++) {
	//				targetT [c, r] = originalT [r, c];
	//			}
	//		}
	//		return targetT;
	//	}

}

