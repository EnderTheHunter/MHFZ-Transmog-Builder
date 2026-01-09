using UnityEngine;

[CreateAssetMenu]
public class ArmorItem : ScriptableObject
{
	public enum ArmorType
	{
		Helmet,
		Torso,
		Arms,
		Belt,
		Legs
	}

	public string armorName;
	public ArmorType type;
	public GameObject modelPrefab;
}
