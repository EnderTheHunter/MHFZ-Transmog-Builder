using System;
using UnityEngine;

[Serializable]
public class MixsetStruct
{
	public int id;
	public bool isMale;
	public string mixsetName;
	public string[] armorPieces;

	public MixsetStruct(PlayerArmorInventory armorInventory, int newID, string newName)
	{
		id = newID;
		if (armorInventory.GetGender() == ArmorItem.Gender.Male)
		{
			isMale = true;
		}
		else
		{
			isMale = false;
		}

		armorPieces = new string[5];
		for(int i = 0; i < armorInventory.armor.Length; i++)
		{
			Debug.Log(armorInventory.armor[i]);
			if (!armorInventory.armor[i])
			{
				armorPieces[i] = "None";
			} else
			{
				armorPieces[i] = armorInventory.armor[i].name;
			}
		}

		mixsetName = newName;
	}

	public void ChangeMixsetName(string newMixsetName)
	{
		mixsetName = newMixsetName;
	}
}
