using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] GameObject textObj;
	[SerializeField] GameObject glowObj;
	[SerializeField] GameObject spriteObj;
	[SerializeField] Sprite[] sprites;

	string unitID;

	public string UnitID {
		get {
			return unitID;
		}
		set {
			unitID = value;
		}
	}

	public int CurrentRow { get; set; }

	public int CurrentColumn { get; set; }

	public int TotalConnectedUnits { get; set; }

	public List<Unit> BelongingGroup { get; set; }


	public List<List<Unit>> match4Groups { get; set; }

	public List<Unit> blockGroup { get; set; }

	//	bool isPartOfBigONE;
	//	int levelOfBigONE;

	UnitType unitTypeCtr;


	// Use this for initialization
	void Start ()
	{
		
	}

	public void initUnit (int column, int row)
	{
		TotalConnectedUnits = 1;
		blockGroup = new List<Unit> ();
		setUnitCoord (column, row);

		unitTypeCtr = GetComponent<UnitType> ();
		unitTypeCtr.init ();

		randomId ();
	}

	public void setUnitCoord (int column, int row)
	{
		CurrentColumn = column;
		CurrentRow = row;
	}

	public void randomId ()
	{
//		var rdmN = Random.Range (0, 3);
//		unitID = rdmN.ToString ();
//		spriteObj.GetComponent<SpriteRenderer> ().sprite = sprites [rdmN];


		unitTypeCtr.randomType ();
		unitID = unitTypeCtr.getCurrentType ();
		spriteObj.GetComponent<SpriteRenderer> ().sprite = unitTypeCtr.getCurrentSprite ();
	}

	public void updateCountText ()
	{
//		textObj.GetComponent<TextMesh> ().text = TotalConnectedUnits.ToString ();
	}

	public void debugText (string t)
	{
		textObj.GetComponent<TextMesh> ().text += t;
	}

	public void startDrag ()
	{
		testMark (true);
		transform.localPosition += new Vector3 (0, 0, -1);
	}

	public void stopDrag ()
	{
		testMark (false);
		transform.localPosition = new Vector3 (CurrentColumn, CurrentRow, 0);
	}

	public void moveTo (Vector2Int v2)
	{
		transform.localPosition = new Vector3 (transform.localPosition.x + v2.x, transform.localPosition.y + v2.y, transform.localPosition.z);

//		TotalConnectedUnits = 1;
//		updateCountText ();//--------------------------
	}

	public void testMark (bool on)
	{
		glowObj.SetActive (on);
	}

	public void becomeCueUnit ()
	{
		
	}



	public void upgrade (int levels)
	{
		
	}

	public void drop (int steps)
	{
		
	}

	public void reset ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
