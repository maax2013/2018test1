using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitType : MonoBehaviour
{
	
	[SerializeField] Sprite[] type1_sprites;
	[SerializeField] string type1 = "type1";

	[SerializeField] Sprite[] type2_sprites;
	[SerializeField] string type2 = "type2";

	[SerializeField] Sprite[] type3_sprites;
	[SerializeField] string type3 = "type3";

	List<Sprite[]> typeSpritesList;
	List<string> typeNames;

	class TieredType
	{
		//		string subType;
		//		Sprite typeSprite;
		public string SubType { get; set; }

		public Sprite TypeSprite { get; set; }

		public TieredType (string subT, Sprite tSprite)
		{
			SubType = subT;
			TypeSprite = tSprite;
		}
	}

	List<TieredType[]> allTypes;

	TieredType[] currentTypeGroup;
	int currentTier;
	string currentType;
	Sprite currentSprite;
	bool isUpgradable;

	public void init ()
	{
		typeSpritesList = new List<Sprite[]>{ type1_sprites, type2_sprites, type3_sprites };
		typeNames = new List<string>{ type1, type2, type3 };

		allTypes = new List<TieredType[]> ();
		TieredType[] subTypes;

		for (int n = 0; n < typeNames.Count; n++) {
			if (typeSpritesList [n].Length > 0) {
				subTypes = new TieredType[typeSpritesList [n].Length];
				for (int i = 0; i < typeSpritesList [n].Length; i++) {
					subTypes [i] = new TieredType (typeNames [n] + i.ToString (), typeSpritesList [n] [i]);
				}
				allTypes.Add (subTypes);
			}
		}

	}

	public void randomType ()
	{
//		print (allTypes);
		var rdmN = Random.Range (0, allTypes.Count);
		currentTypeGroup = allTypes [rdmN];
		currentTier = 0;
		updateTypeInfo ();
	}

	public void upgradeToNextTier ()
	{
		if (isUpgradable) {
			currentTier++;
			updateTypeInfo ();
		}
	}

	public string getCurrentType ()
	{
		return currentType;
	}

	public Sprite getCurrentSprite ()
	{
		return currentSprite;
	}

	public bool IsUpgradable ()
	{
		return isUpgradable;
	}

	void updateTypeInfo ()
	{
		currentType = currentTypeGroup [currentTier].SubType;
		currentSprite = currentTypeGroup [currentTier].TypeSprite;
		if (currentTypeGroup.Length > currentTier + 1) {
			isUpgradable = true;
		} else {
			isUpgradable = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
