using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MixsetManager : MonoBehaviour
{
	private static MixsetManager instance;
	public static MixsetManager Instance { get { return instance; } }
	[SerializeField]
	List<MixsetStruct> mixsetList;
	[SerializeField]
	ArmorSorterManager armorSorterManager;
	[SerializeField]
	GameObject mixsetPrefab;
	[SerializeField]
	GameObject mixsetListPrefab;
	[SerializeField]
	Sprite maleIcon;
	[SerializeField]
	Sprite femaleIcon;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}

		mixsetList = SavingSystem.LoadMixset();
		if (mixsetList == null )
		{
			mixsetList = new List<MixsetStruct>();
		}
		int mixsetLength = mixsetList.Count;

		for (int i = 0; i < 50; i++)
		{
			GameObject newPrefab = Instantiate(mixsetPrefab, mixsetListPrefab.transform);
			if (i >= mixsetLength)
			{
				UpdateMixset(newPrefab, null, i + 1);
			} else
			{
				UpdateMixset(newPrefab, mixsetList[i], i + 1);
			}
		}
	}

	public void SaveMixset(int id, string name = "Mixset")
	{
		MixsetStruct newMixset = new MixsetStruct(PlayerArmorInventory.Instance, id, name);
		if (id >= mixsetList.Count)
		{
			mixsetList.Add(newMixset);
		} else
		{
			mixsetList[id] = newMixset;
		}
		SavingSystem.SaveMixset(mixsetList);
	}

	public void LoadMixset(int value)
	{
		if (value >= mixsetList.Count)
			return;
		MixsetStruct mixsetToApply = mixsetList[value];
		ArmorItem.Gender gender;

		if (mixsetToApply.isMale == true)
		{
			gender = ArmorItem.Gender.Male;
		} else
		{
			gender = ArmorItem.Gender.Female;
		}
		for (int i = 0; i < mixsetToApply.armorPieces.Length; i++)
		{
			if (mixsetToApply.armorPieces[i] == "None" || mixsetToApply.armorPieces[i].Contains("000"))
			{
				PlayerArmorInventory.Instance.RemoveArmorPiece(i + 1);
			} else
			{
				List<ArmorItem> armorList = armorSorterManager.GetArmorList(i, gender).armorPieces;
				foreach(ArmorItem armorItem in armorList)
				{
					if (armorItem.name == mixsetToApply.armorPieces[i])
					{
						PlayerArmorInventory.Instance.ChangeArmor(armorItem);
					}
				}
			}
		}
	}

	public void UpdateMixset(GameObject newPrefab, MixsetStruct newMixset, int i = 1)
	{
		if (newMixset == null)
		{
			newPrefab.GetComponentInChildren<TMP_InputField>().text = "Mixset " + i.ToString();
		} else
		{
			newPrefab.GetComponentInChildren<TMP_InputField>().text = newMixset.mixsetName;
			if (newMixset.isMale == true)
			{
				UpdateGenderIcon(newPrefab, ArmorItem.Gender.Male);
			}
			else
			{
				UpdateGenderIcon(newPrefab, ArmorItem.Gender.Female);
			}
		}
		newPrefab.GetComponent<MixsetElement>().UpdateID(i - 1);
	}

	public void RenameMixset(string newName, int mixsetID)
	{
		mixsetList[mixsetID].ChangeMixsetName(newName);
		SavingSystem.SaveMixset(mixsetList);
	}

	public void UpdateGenderIcon(GameObject prefab, ArmorItem.Gender gender)
	{
		prefab.transform.Find("Gender").GetComponent<Image>().enabled = true;
		if (gender == ArmorItem.Gender.Male)
		{
			prefab.transform.Find("Gender").GetComponent<Image>().sprite = maleIcon;
		}
		else
		{
			prefab.transform.Find("Gender").GetComponent<Image>().sprite = femaleIcon;
		}
	}
}
