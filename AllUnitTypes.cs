using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUnitTypes : MonoBehaviour
{

	[System.Serializable] class SpriteArray
	{
		public GemType gemType;
		public Sprite[] typeSpriteArray;
	}

	[SerializeField] SpriteArray[] unitTypes;

	//	GemType[] enabledGemTypes;


	//	public class UnitType
	//	{
	//		GemType currentType;
	//		Sprite[] typeSprites;
	//		int currentTier;
	//		string currentTypeID;
	//		Sprite currentSprite;
	//
	//		const string maxTier = "MAX_TIER";
	//		const string noTier = "NO_TIER";
	//
	//		public UnitType (GemType type, Sprite[] tSprites)
	//		{
	//			currentType = type;
	//			typeSprites = tSprites;
	//			currentTier = 0;
	//			updateTypeInfo ();
	//		}
	//
	//		public GemType getCurrentType ()
	//		{
	//			return currentType;
	//		}
	//
	//		public string getCurrentTypeID ()
	//		{
	//			return currentTypeID;
	//		}
	//
	//		public Sprite getCurrentSprite ()
	//		{
	//			return currentSprite;
	//		}
	//
	//		public bool isUpgradable ()
	//		{
	//			/*tier starts from 0, so compare to length-1*/
	//			return currentTier < typeSprites.Length - 1;
	//		}
	//
	//		public void upgradeToNextTier ()
	//		{
	//			/*tier starts from 0, so compare to length-1*/
	//			if (currentTier < typeSprites.Length - 1) {
	//				currentTier++;
	//				updateTypeInfo ();
	//			} else {
	//				//TODO: can't upgrade tier
	//				Debug.Log ("can't upgrade tier");
	//			}
	//		}
	//
	//		public bool isMaxTier ()
	//		{
	//			return currentTypeID.Contains (maxTier);
	//		}
	//
	//		public bool isNoTier ()
	//		{
	//			return currentTypeID.Contains (noTier);
	//		}
	//
	//		string createTypeID (int currentTier)
	//		{
	//			if (typeSprites.Length > 1) {
	//				/*tier starts from 0, so compare to length-1*/
	//				if (currentTier < typeSprites.Length - 1) {
	//					currentTypeID = currentType.ToString () + currentTier.ToString ();
	//				} else {
	//					currentTypeID = currentType.ToString () + maxTier;
	//				}
	//
	//			} else if (typeSprites.Length < 1) {
	//				/*no sprite images added to Editor*/
	//				Debug.Log ("no gem image added to Editor");
	//				//TODO: throw Exception;
	//			} else {
	//				/*only one sprite image, meaning no tier*/
	//				currentTypeID = currentType.ToString () + noTier;
	//			}
	//			return currentTypeID;
	//		}
	//
	//		void updateTypeInfo ()
	//		{
	//			currentTypeID = createTypeID (currentTier);
	//			currentSprite = typeSprites [currentTier];
	//		}
	//	}
	//

	//	List<TieredType[]> allTypes;
	//
	//	TieredType[] currentTypeGroup;
	//	int currentTier;
	//	string currentType;
	//	Sprite currentSprite;
	//	bool upgradable;

	//	public void init ()
	//	{
	////		typeSpritesList = new List<Sprite[]>{ type1_sprites, type2_sprites, type3_sprites };
	////		typeNames = new List<string>{ type1, type2, type3 };
	////
	////		allTypes = new List<TieredType[]> ();
	////		TieredType[] subTypes;
	////
	////		for (int n = 0; n < typeNames.Count - 1; n++) {
	////			if (typeSpritesList [n].Length > 0) {
	////				subTypes = new TieredType[typeSpritesList [n].Length];
	////				for (int i = 0; i < typeSpritesList [n].Length; i++) {
	////					subTypes [i] = new TieredType (typeNames [n] + i.ToString (), typeSpritesList [n] [i]);
	////				}
	////				allTypes.Add (subTypes);
	////			}
	////		}
	//		var ut = getRandomType ();
	//		print (ut.getCurrentTypeID ());
	//		print (ut.isNoTier ()); 
	//	}

	public UnitType getRandomType ()
	{
		int rdmN = Random.Range (0, unitTypes.Length);
		UnitType ut = getTypeOf ((int)unitTypes [rdmN].gemType);
		return ut;
	}

	public UnitType getTypeOf (int i)
	{
		foreach (var t in unitTypes) {
			if ((int)t.gemType == i) {
				UnitType ut = new UnitType (t.gemType, t.typeSpriteArray);
				return ut;
			}
		}
		Debug.Log ("can't find the asked gem type");
		return null;
	}

	//	public void upgradeToNextTier ()
	//	{
	//		if (upgradable) {
	//			currentTier++;
	//			updateTypeInfo ();
	//		}
	//	}
	//
	//	public string getCurrentType ()
	//	{
	//		return currentType;
	//	}
	//
	//	public Sprite getCurrentSprite ()
	//	{
	//		return currentSprite;
	//	}
	//
	//	public bool isUpgradable ()
	//	{
	//		return upgradable;
	//	}
	//
	//	void updateTypeInfo ()
	//	{
	//		currentType = currentTypeGroup [currentTier].SubType;
	//		currentSprite = currentTypeGroup [currentTier].TypeSprite;
	//		if (currentTypeGroup.Length > currentTier + 1) {
	//			upgradable = true;
	//		} else {
	//			upgradable = false;
	//		}
	//	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
