using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField] GameObject blockPrefab;
	[SerializeField] Transform blocksHolder;

	List<GameObject> blocks = new List<GameObject> ();

	GameObject tempBlock;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void repositionBlocksHolder (float x, float y, float z)
	{
		blocksHolder.position = new Vector3 (x, y, z);
	}

	public void showBlockAt_overTime (Vector3 targetP, float duration)
	{
		/*get a avialable block, set it active, reposition, show animation, set it back inactive when animation is done*/
		tempBlock = getAFreeBlock ();
		tempBlock.SetActive (true);
		tempBlock.transform.localPosition = targetP;
		StartCoroutine (FadeOut (tempBlock, duration));
	}

	IEnumerator FadeOut (GameObject block, float duration)
	{
		float elapsedTime = 0.0f;
		Color c = block.GetComponent<SpriteRenderer> ().color;

		while (elapsedTime < duration) {
			elapsedTime += Time.deltaTime;
			c.a = 1.0f - Mathf.Clamp01 (elapsedTime / duration);
			block.GetComponent<SpriteRenderer> ().color = c;
			yield return new WaitForEndOfFrame ();
		}

		/*reset block for next use*/
		block.SetActive (false);
		c.a = 1f;
		block.GetComponent<SpriteRenderer> ().color = c;
	}

	GameObject getAFreeBlock ()
	{
		if (blocks.Count > 0) {
			for (int i = 0; i < blocks.Count; i++) {
				/*return the first inactive (available) Block*/
				if (!blocks [i].activeSelf) {
					return blocks [i];
				}
			}
		}
		/*if no blocks available, create a new one, then return it*/
		return InstantiateBlock ();
	}

	GameObject InstantiateBlock ()
	{
		/*create a new block from the prebab, then add it to parent, add it to the list, set it inactive, then return it*/
		tempBlock = Instantiate (blockPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		tempBlock.transform.SetParent (blocksHolder, false);

		blocks.Add (tempBlock);
		tempBlock.SetActive (false);

		return tempBlock;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
