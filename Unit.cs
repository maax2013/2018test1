using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] GameObject[] dotPrefabs;
	[SerializeField] GameObject dot1Prefab;
	[SerializeField] GameObject dot2Prefab;
	[SerializeField] GameObject dot3Prefab;

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

	public int TotalUnitsThisConnectsTo { get; set; }

	List<Unit> belongingGroup;

	bool isPartOfBigONE;
	int levelOfBigONE;





	GameObject tempUnitSprite;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void initRandomUnit (int column, int row)
	{
		var rdmN = Random.Range (0, 3);
		unitID = rdmN.ToString;
		CurrentColumn = column;
		CurrentRow = row;
		tempUnitSprite = Instantiate (dotPrefabs [rdmN], new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		tempUnitSprite.transform.parent = transform;
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
