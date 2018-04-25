﻿using System.Collections;
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

	bool isPartOfBigONE;
	int levelOfBigONE;





	GameObject tempUnitSprite;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void initRandomUnit (int column, int row)
	{
		CurrentColumn = column;
		CurrentRow = row;
		TotalConnectedUnits = 1;

		var rdmN = Random.Range (0, 3);
		unitID = rdmN.ToString ();

		spriteObj.GetComponent<SpriteRenderer> ().sprite = sprites [rdmN];
	}

	public void updateCountText ()
	{
		textObj.GetComponent<TextMesh> ().text = TotalConnectedUnits.ToString ();
	}

	public void debugText (string t)
	{
		textObj.GetComponent<TextMesh> ().text += t;
	}

	public void startDrag ()
	{
		glowObj.SetActive (true);
		transform.localPosition += new Vector3 (0, 0, -1);
	}

	public void stopDrag ()
	{
		glowObj.SetActive (false);
		transform.localPosition += new Vector3 (0, 0, 1);
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
