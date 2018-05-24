using UnityEngine;

public class AllUnitTypes : MonoBehaviour
{

	[System.Serializable] class SpriteArray
	{
        public GemType gemType;
		public Sprite[] typeSpriteArray;
	}

	[SerializeField] SpriteArray[] unitTypes;


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
}
