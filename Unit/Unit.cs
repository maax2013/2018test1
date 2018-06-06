using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Unit_Base
{
    [SerializeField] SpriteRenderer UnitSprite;

	public UnitType CurrentUnitType { get; set; }

	//	public int TotalConnectedUnits { get; set; }

	public List<Unit> BelongingGroup { get; set; }

	public Vector2Int FallFrom { get; set; }

	public Vector2Int FallTo { get; set; }

	public List<List<Unit>> match4Groups { get; set; }

	public List<Unit> blockGroup { get; set; }

	//	public delegate void MyUnitEvent0 ();
	//	public event MyUnitEvent0 onFallDone;

	public event System.Action<Unit> onFallDone;
	//public event System.Action onMergeDone;
	public event System.Action onJumpDone;

	[HideInInspector] public int BelongingBlocks;
	//	int levelOfBigONE;

	AllUnitTypes allTypes;

	//Coroutine moveCoroutine;
	//Coroutine popCoroutine;
	float fallSpeed = 0.1f;
    //const float dragZ = -0.5f;


	public void InitNormalUnit (int column, int row, AllUnitTypes uTypes)
	{
        UMerge = GetComponent<UnitMerge>();
        UDragDrop = GetComponent<UnitDragDrop>();

//		TotalConnectedUnits = 1;
		blockGroup = new List<Unit> ();
		BelongingBlocks = 0;
		SetUnitCoord (column, row);

		allTypes = uTypes;
        randomId ();
	}
    public void InitSpecialUnit(int column, int row, SpecialType specialType){
        
    }


    public void randomId ()
	{
		CurrentUnitType = allTypes.getRandomType ();
		UpdateUnitSprite ();
	}

    public string GetUnitID(){
        return CurrentUnitType.getCurrentTypeID();
    }


//	public void updateCountText ()
//	{
////		textObj.GetComponent<TextMesh> ().text = TotalConnectedUnits.ToString ();
	//}

	

//	public void startDrag ()
//	{
//		testMark (true);
//		transform.localPosition += new Vector3 (0f, 0f, dragZ);
//	}
//    public void dragging(Vector3 pos){
//        transform.position = pos + new Vector3(0f, 0f, dragZ);
//    }

//	public void stopDrag ()
//	{
//		testMark (false);
////		transform.localPosition = new Vector3 (CurrentColumn, CurrentRow, 0);
	//	//ResetLocalPos ();
	//}

	public void moveTo (Vector2Int v2)
	{
        StopAllCoroutines();
		ResetLocalPos ();
//		transform.localPosition = new Vector3 (transform.localPosition.x + v2.x, transform.localPosition.y + v2.y, transform.localPosition.z);
		StartCoroutine (moveBounceTo (v2));
//		TotalConnectedUnits = 1;
//		updateCountText ();//--------------------------
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
		if (v2.x == 0) {
			overX = 0f;
		} else {
			overX = v2.x > 0 ? overOffset : -overOffset;
		}
		if (v2.y == 0) {
			overY = 0f;
		} else {
			overY = v2.y > 0 ? overOffset : -overOffset;
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
    void ResetLocalPos()
    {
        //StopAllCoroutines();
        transform.localPosition = new Vector3(CurrentColumn, CurrentRow, 0f);
    }

	



	public void upgrade (int levels)
	{
		if (CurrentUnitType.isUpgradable ()) {
			CurrentUnitType.upgradeToNextTier ();
			UpdateUnitSprite ();
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

//	public void mergeTo_overTime_thenGone (Vector3 target, float mergeTime)
//	{
////		if (moveCoroutine) {
////			StopCoroutine (moveCoroutine);
////		}
////		moveCoroutine = StartCoroutine (moveSpriteTo_overTime (target, mergeTime));
	//	moveCoroutine = StartCoroutine (mergeTo_overTime_thenInactive (target, mergeTime));
	//}

	//public void popSprite_overTime (float popTime)
	//{
	//	popCoroutine = StartCoroutine (popSpriteTo_overTime (popTime));
	//}

	//IEnumerator mergeTo_overTime_thenInactive (Vector3 target, float duration)
	//{
	//	float elapsedTime = 0;
	//	Vector3 startingPos = transform.localPosition;
	//	while (elapsedTime < duration) {
	//		transform.localPosition = Vector3.Lerp (startingPos, target, (elapsedTime / duration));
	//		elapsedTime += Time.deltaTime;
	//		yield return new WaitForEndOfFrame ();
	//	}
	//	transform.localPosition = startingPos;
	//	reset ();
	//	gameObject.SetActive (false);
	//	if (onMergeDone != null) {
	//		onMergeDone ();
	//		onMergeDone = null;
	//	}
	//}

	//IEnumerator popSpriteTo_overTime (float duration)
	//{
	//	Vector3 target = new Vector3 (1.3f, 1.3f, 1f);//~~~~~~~~~~~~~~~~~~~~~~~~~~~
	//	float elapsedTime = 0;

	//	float popUpTime = duration * 0.2f;
	//	float pauseTime = duration * 0.3f;
	//	float shrinkTime = duration * 0.5f;

	//	Vector3 startingScale = transform.localScale;

	//	while (elapsedTime < popUpTime) {
	//		transform.localScale = Vector3.Lerp (startingScale, target, (elapsedTime / popUpTime));
	//		elapsedTime += Time.deltaTime;
	//		yield return new WaitForEndOfFrame ();
	//	}
	//	transform.localScale = target;

	//	yield return new WaitForSeconds (pauseTime);

	//	elapsedTime = 0;
	//	while (elapsedTime < shrinkTime) {
	//		transform.localScale = Vector3.Lerp (target, startingScale, (elapsedTime / shrinkTime));
	//		elapsedTime += Time.deltaTime;
	//		yield return new WaitForEndOfFrame ();
	//	}
	//	transform.localScale = startingScale;
	//	clearConnections ();
	//	if (onMergeDone != null) {
	//		onMergeDone ();
	//		onMergeDone = null;
	//	}
	//}

	public void fall ()
	{
		Vector3 localPos = transform.localPosition;
		localPos.x = FallFrom.x;
		localPos.y = FallFrom.y;
		transform.localPosition = localPos;

		localPos.x = FallTo.x;
		localPos.y = FallTo.y;
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
		SetUnitCoord (FallTo.x, FallTo.y);

		if (onFallDone != null) {
			onFallDone (this);
			onFallDone = null;
		}
	}

    public void HandleOnMergeDone(){
        reset();
        gameObject.SetActive(false);
    }
    public void HandleOnPopDone(){
        clearConnections();
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

	


	void UpdateUnitSprite ()
	{
		//unitID = CurrentUnitType.getCurrentTypeID ();
        UnitSprite.sprite = CurrentUnitType.getCurrentSprite ();
	}
}
