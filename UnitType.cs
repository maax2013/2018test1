using UnityEngine;

public class UnitType
{
	GemType currentType;
	Sprite[] typeSprites;
	int currentTier;
	string currentTypeID;
	Sprite currentSprite;

	string maxTier = GemTier._MaxTier.ToString();
	string noTier = GemTier._NoTier.ToString();

	public UnitType (GemType type, Sprite[] tSprites)
	{
		currentType = type;
		typeSprites = tSprites;
		currentTier = 0;
		updateTypeInfo ();
	}

	public GemType getCurrentType ()
	{
		return currentType;
	}

	public string getCurrentTypeID ()
	{
		return currentTypeID;
	}

	public Sprite getCurrentSprite ()
	{
		return currentSprite;
	}

	public bool isUpgradable ()
	{
		/*tier starts from 0, so compare to length-1*/
		return currentTier < typeSprites.Length - 1;
	}

	public void upgradeToNextTier ()
	{
		/*tier starts from 0, so compare to length-1*/
		if (currentTier < typeSprites.Length - 1) {
			currentTier++;
			updateTypeInfo ();
		} else {
			//TODO: can't upgrade tier
			Debug.Log ("can't upgrade tier");
		}
	}

	public bool isMaxTier ()
	{
		return currentTypeID.Contains (maxTier);
	}

	public bool isNoTier ()
	{
		return currentTypeID.Contains (noTier);
	}

	string createTypeID (int current_Tier)
	{
		if (typeSprites.Length > 1) {
			/*tier starts from 0, so compare to length-1*/
			if (current_Tier < typeSprites.Length - 1) {
				//currentTypeID = currentType.ToString () + currentTier.ToString ();
                currentTypeID = currentType.ToString() + ((GemTier)current_Tier).ToString();
			} else {
				currentTypeID = currentType.ToString () + maxTier;
			}

		} else if (typeSprites.Length < 1) {
			/*no sprite images added to Editor*/
			Debug.Log ("no gem image added to Editor");
			//TODO: throw Exception;
		} else {
			/*only one sprite image, meaning no tier*/
			currentTypeID = currentType.ToString () + noTier;
		}
		return currentTypeID;
	}

	void updateTypeInfo ()
	{
		currentTypeID = createTypeID (currentTier);
		currentSprite = typeSprites [currentTier];
	}
}

