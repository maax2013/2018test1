using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] GameObject textObj;
	[SerializeField] GameObject glowObj;
	[SerializeField] GameObject spriteObj;
	//	[SerializeField] Sprite[] sprites;

	string unitID;

	public string UnitID {
		get {
			return unitID;
		}
		set {
			unitID = value;
		}
	}

	public bool IsUpgradable { get; set; }

	public int CurrentRow { get; set; }

	public int CurrentColumn { get; set; }

	public int TotalConnectedUnits { get; set; }

	public List<Unit> BelongingGroup { get; set; }

	public Vector2Int fallFrom { get; set; }

	public Vector2Int fallTo { get; set; }

	public List<List<Unit>> match4Groups { get; set; }

	public List<Unit> blockGroup { get; set; }

	[System.NonSerialized] public int BelongingBlocks;
	//	int levelOfBigONE;

	UnitType unitTypeCtr;

	Coroutine moveCoroutine;
	Coroutine popCoroutine;
	float fallSpeed = 0.2f;


	// Use this for initialization
	void Start ()
	{
		
	}

	public void initUnit (int column, int row)
	{
		TotalConnectedUnits = 1;
		blockGroup = new List<Unit> ();
		BelongingBlocks = 0;
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
		unitTypeCtr.randomType ();
		updateUnitInfo ();
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
//		print ("upgrade");
		if (IsUpgradable) {
			unitTypeCtr.upgradeToNextTier ();
			updateUnitInfo ();
		} else {
			//TODO:
			//+++++++++++++++++
		}

	}

	public void mergeTo_overTime_thenGone (Vector3 target, float mergeTime)
	{
//		if (moveCoroutine) {
//			StopCoroutine (moveCoroutine);
//		}
//		moveCoroutine = StartCoroutine (moveSpriteTo_overTime (target, mergeTime));
		moveCoroutine = StartCoroutine (mergeTo_overTime_thenInactive (target, mergeTime));
	}

	public void popSprite_overTime (Vector3 target, float popTime)
	{
		popCoroutine = StartCoroutine (popSpriteTo_overTime (target, popTime));
	}

	IEnumerator mergeTo_overTime_thenInactive (Vector3 target, float duration)
	{
		float elapsedTime = 0;
		Vector3 startingPos = transform.localPosition;
		while (elapsedTime < duration) {
			transform.localPosition = Vector3.Lerp (startingPos, target, (elapsedTime / duration));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localPosition = target;
		gameObject.SetActive (false);
	}

	IEnumerator moveSpriteTo_overTime (Vector3 target, float duration)
	{
		float elapsedTime = 0;
		Vector3 startingPos = transform.localPosition;
		while (elapsedTime < duration) {
			transform.localPosition = Vector3.Lerp (startingPos, target, (elapsedTime / duration));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localPosition = target;
	}

	IEnumerator popSpriteTo_overTime (Vector3 target, float duration)
	{
		float elapsedTime = 0;

		float popUpTime = duration * 0.2f;
		float pauseTime = duration * 0.3f;
		float shrinkTime = duration * 0.5f;

		Vector3 startingScale = transform.localScale;

		while (elapsedTime < popUpTime) {
			transform.localScale = Vector3.Lerp (startingScale, target, (elapsedTime / popUpTime));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localScale = target;

		yield return new WaitForSeconds (pauseTime);

		elapsedTime = 0;
		while (elapsedTime < shrinkTime) {
			transform.localScale = Vector3.Lerp (target, startingScale, (elapsedTime / shrinkTime));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localScale = startingScale;
	}

	public void fall ()
	{
		Vector3 localPos = transform.localPosition;
		localPos.x = fallFrom.x;
		localPos.y = fallFrom.y;
		transform.localPosition = localPos;

		localPos.x = fallTo.x;
		localPos.y = fallTo.y;
		Vector3 targetPos = localPos;

		//TODO: calculate speed based on fall distance
		StartCoroutine (moveSpriteTo_overTime (targetPos, fallSpeed));
	}

	public void reset ()
	{
		TotalConnectedUnits = 1;
		blockGroup = new List<Unit> ();
		BelongingBlocks = 0;

		randomId ();
	}

	public void ghost ()
	{
		gameObject.SetActive (false);//~~~~~~~~~~~~~~
	}

	void updateUnitInfo ()
	{
		unitID = unitTypeCtr.getCurrentType ();
		IsUpgradable = unitTypeCtr.isUpgradable ();
		spriteObj.GetComponent<SpriteRenderer> ().sprite = unitTypeCtr.getCurrentSprite ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
