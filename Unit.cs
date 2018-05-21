using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	[SerializeField] GameObject textObj;
	[SerializeField] GameObject glowObj;
	[SerializeField] GameObject spriteObj;

	string unitID;

	public string UnitID {
		get {
			return unitID;
		}
		set {
			unitID = value;
		}
	}

	public UnitType CurrentUnitType { get; set; }

	public int CurrentRow { get; set; }

	public int CurrentColumn { get; set; }

	//	public int TotalConnectedUnits { get; set; }

	public List<Unit> BelongingGroup { get; set; }

	public Vector2Int fallFrom { get; set; }

	public Vector2Int fallTo { get; set; }

	public List<List<Unit>> match4Groups { get; set; }

	public List<Unit> blockGroup { get; set; }

	//	public delegate void MyUnitEvent0 ();
	//	public event MyUnitEvent0 onFallDone;

	public event System.Action<Unit> onFallDone;
	public event System.Action onMergeDone;
	public event System.Action onJumpDone;

	[HideInInspector] public int BelongingBlocks;
	//	int levelOfBigONE;

	AllUnitTypes allTypes;


	Coroutine moveCoroutine;
	Coroutine popCoroutine;
	float fallSpeed = 0.1f;


	// Use this for initialization
	void Start ()
	{
		
	}

	public void initUnit (int column, int row, AllUnitTypes uTypes)
	{
//		TotalConnectedUnits = 1;
		blockGroup = new List<Unit> ();
		BelongingBlocks = 0;
		setUnitCoord (column, row);

		allTypes = uTypes;

		randomId ();
	}

	public void setUnitCoord (int column, int row)
	{
		CurrentColumn = column;
		CurrentRow = row;
	}

	public void randomId ()
	{
		CurrentUnitType = allTypes.getRandomType ();
		updateUnitInfo ();
	}


	public void updateCountText ()
	{
//		textObj.GetComponent<TextMesh> ().text = TotalConnectedUnits.ToString ();
	}

	public void debugText (string t)
	{
		textObj.SetActive (true);
		textObj.GetComponent<TextMesh> ().text = t;
	}

	public void startDrag ()
	{
		testMark (true);
		transform.localPosition += new Vector3 (0, 0, -1);
	}

	public void stopDrag ()
	{
		testMark (false);
//		transform.localPosition = new Vector3 (CurrentColumn, CurrentRow, 0);
		resetLocalPos ();
	}

	public void moveTo (Vector2Int v2)
	{
		resetLocalPos ();
//		transform.localPosition = new Vector3 (transform.localPosition.x + v2.x, transform.localPosition.y + v2.y, transform.localPosition.z);
		StartCoroutine (moveBounceTo (v2));
//		TotalConnectedUnits = 1;
//		updateCountText ();//--------------------------
	}

	void resetLocalPos ()
	{
		StopAllCoroutines ();
		transform.localPosition = new Vector3 (CurrentColumn, CurrentRow, 0);
	}

	IEnumerator moveBounceTo (Vector2Int v2)
	{
		float elapsedTime = 0;
		float toDuration = 0.1f;
		float backDuration = 0.5f;
		Vector3 startingPos = transform.localPosition;
		Vector3 targetPos = new Vector3 (startingPos.x + v2.x, startingPos.y + v2.y, startingPos.z);
		float overOffset = 0.1f;
		float overX;
		float overY;
		if (v2.x == 0f) {
			overX = 0f;
		} else {
			overX = v2.x > 0f ? overOffset : -overOffset;
		}
		if (v2.y == 0f) {
			overY = 0f;
		} else {
			overY = v2.y > 0f ? overOffset : -overOffset;
		}


//		float overX = Mathf.Ceil (v2.x / 1000f) * overOffset;
//		float overY = Mathf.Ceil (v2.y / 1000f) * overOffset;
		Vector3 overTargetPos = new Vector3 (targetPos.x + overX, targetPos.y + overY, targetPos.z);
		while (elapsedTime < toDuration) {
			transform.localPosition = Vector3.Lerp (startingPos, overTargetPos, (elapsedTime / toDuration));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localPosition = overTargetPos;
		elapsedTime = 0;
		while (elapsedTime < toDuration) {
			transform.localPosition = Vector3.Lerp (overTargetPos, targetPos, (elapsedTime / backDuration));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localPosition = targetPos;
	}

	public void testMark (bool on)
	{
		glowObj.SetActive (on);
	}



	public void upgrade (int levels)
	{
		if (CurrentUnitType.isUpgradable ()) {
			CurrentUnitType.upgradeToNextTier ();
			updateUnitInfo ();
		} else {
			//TODO: what to do when reach max tier
		}

	}

	public void jump_overTime_thenGone (float jumpTime)
	{
		StartCoroutine (jump_overTime_thenInactive (jumpTime));
	}

	IEnumerator jump_overTime_thenInactive (float duration)
	{
		float elapsedTime = 0;
//		Vector3 startingPos = transform.localPosition;
//		Vector3 target = new Vector3 (startingPos.x, startingPos.y + 1, startingPos.z);
		Vector3 startingScale = transform.localScale;
		Vector3 target = new Vector3 (0.1f, 1f, 1f);//~~~~~~~~~~~~~~~~~~~~~~~~~~~
		while (elapsedTime < duration) {
			transform.localScale = Vector3.Lerp (startingScale, target, (elapsedTime / duration));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localScale = startingScale;
		reset ();
		gameObject.SetActive (false);
		if (onJumpDone != null) {
			onJumpDone ();
			onJumpDone = null;
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

	public void popSprite_overTime (float popTime)
	{
		popCoroutine = StartCoroutine (popSpriteTo_overTime (popTime));
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
		transform.localPosition = startingPos;
		reset ();
		gameObject.SetActive (false);
		if (onMergeDone != null) {
			onMergeDone ();
			onMergeDone = null;
		}
	}

	IEnumerator popSpriteTo_overTime (float duration)
	{
		Vector3 target = new Vector3 (1.3f, 1.3f, 1f);//~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
		clearConnections ();
		if (onMergeDone != null) {
			onMergeDone ();
			onMergeDone = null;
		}
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
		StartCoroutine (fallTo_overTime (targetPos, fallSpeed));
	}

	IEnumerator fallTo_overTime (Vector3 target, float duration)
	{
		float elapsedTime = 0;
		Vector3 startingPos = transform.localPosition;
		while (elapsedTime < duration) {
			transform.localPosition = Vector3.Lerp (startingPos, target, (elapsedTime / duration));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.localPosition = target;
		setUnitCoord (fallTo.x, fallTo.y);

		if (onFallDone != null) {
			onFallDone (this);
			onFallDone = null;
		}
	}

	public void reset ()
	{
		clearConnections ();
		randomId ();
	}

	void clearConnections ()
	{
//		TotalConnectedUnits = 1;
		blockGroup.Clear ();
		BelongingBlocks = 0;
	}

	public void showDebugCoord ()
	{
		debugText (CurrentColumn.ToString () + ":" + CurrentRow.ToString ());
	}


	void updateUnitInfo ()
	{
		unitID = CurrentUnitType.getCurrentTypeID ();
		spriteObj.GetComponent<SpriteRenderer> ().sprite = CurrentUnitType.getCurrentSprite ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
