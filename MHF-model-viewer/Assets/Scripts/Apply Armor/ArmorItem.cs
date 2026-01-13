using UnityEngine;

[CreateAssetMenu]
public class ArmorItem : ScriptableObject
{
	public enum ArmorType
	{
		Face,
		Helmet,
		Torso,
		Arms,
		Belt,
		Legs
	}

	public enum Gender
	{
		Male,
		Female
	}

	public string armorName;
	public Gender gender;
	public GameObject modelPrefab;
	public ArmorType type;
	[HideInInspector]
	public bool isHeadVisible;
	[HideInInspector]
	public bool isHairDyable;
}
