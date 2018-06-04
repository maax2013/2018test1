using UnityEngine;

public class AllUnitTypes : MonoBehaviour
{

	[System.Serializable] class SpriteArray
	{
        public GemType gemType;
		public Sprite[] typeSpriteArray;
	}

	[SerializeField] SpriteArray[] normalUnitTypes;


	public UnitType getRandomType ()
	{
		int rdmN = Random.Range (0, normalUnitTypes.Length);
		UnitType ut = getTypeOf ((int)normalUnitTypes [rdmN].gemType);
		return ut;
	}

	public UnitType getTypeOf (int i)
	{
		foreach (var t in normalUnitTypes) {
			if ((int)t.gemType == i) {
				UnitType ut = new UnitType (t.gemType, t.typeSpriteArray);
				return ut;
			}
		}
        throw new System.Exception("can't find the asked gem type");
		//return null;
	}
}
