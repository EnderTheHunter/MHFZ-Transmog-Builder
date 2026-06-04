using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

	[SerializeField]
	GameObject mixsetPreview;
	[SerializeField]
	TextMeshProUGUI previewMixsetNameText;
	[SerializeField]
	TextMeshProUGUI previewGenderText;
	[SerializeField]
	TextMeshProUGUI[] previewArmorNamesTextList;

	/// <summary>
	/// Singleton initialization + Loading all saved mixsets
	/// </summary>
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

		mixsetList = SavingSystem.Load<List<MixsetStruct>>("/mixsets.jsmt");
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

	/// <summary>
	/// Save a nex mixset
	/// </summary>
	/// <param name="id"></param>
	/// <param name="name"></param>
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
		SavingSystem.Save(mixsetList, "/mixsets.jsmt");
		LoadMixsetPreview(id);
	}

	/// <summary>
	/// Load a pre existing mixset
	/// </summary>
	/// <param name="value">The mixset ID</param>
	public void LoadMixset(int value)
	{
		if (value >= mixsetList.Count)
			return;
		MixsetStruct mixsetToApply = mixsetList[value];
		ArmorItem.Gender gender;

		if (mixsetToApply.isMale == true)
		{
			gender = ArmorItem.Gender.Male;
			PlayerArmorInventory.Instance.SetGender(true, false);
		} else
		{
			gender = ArmorItem.Gender.Female;
			PlayerArmorInventory.Instance.SetGender(false, false);
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
		mixsetPreview.SetActive(false);
	}

	/// <summary>
	/// Modify an existing mixset
	/// </summary>
	/// <param name="newPrefab"></param>
	/// <param name="newMixset"></param>
	/// <param name="i"></param>
	public void UpdateMixset(GameObject newPrefab, MixsetStruct newMixset, int i = 1)
	{
		if (newMixset == null)
		{
			newPrefab.GetComponentInChildren<TMP_InputField>().text = "Mixset " + i.ToString();
		} else
		{
			newPrefab.GetComponentInChildren<TMP_InputField>().enabled = true;
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
		newPrefab.GetComponent<MixsetInfoToPrevisualisation>().UpdateID(i - 1);
	}

	public void RenameMixset(string newName, int mixsetID)
	{
		mixsetList[mixsetID].ChangeMixsetName(newName);
		SavingSystem.Save(mixsetList, "/mixsets.jsmt");
		LoadMixsetPreview(mixsetID);
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

	public void OpenClose()
	{
		if (isActiveAndEnabled == true)
		{
			gameObject.SetActive(false);
		} else
		{
			gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// Load every information about a mixset in the preview page
	/// </summary>
	/// <param name="id"></param>
	public void LoadMixsetPreview(int id)
	{
		if (id >= mixsetList.Count)
		{
			mixsetPreview.SetActive(false);
			return;
		}
		mixsetPreview.SetActive(true);
		previewMixsetNameText.text = mixsetList[id].mixsetName;

		ArmorItem.Gender gender;
		if (mixsetList[id].isMale == true)
		{
			previewGenderText.text = "Gender : Male";
			gender = ArmorItem.Gender.Male;
		} else
		{
			previewGenderText.text = "Gender : Female";
			gender = ArmorItem.Gender.Female;
		}
		for (int i = 0; i < mixsetList[id].armorPieces.Length; i++)
		{
			if (mixsetList[id].armorPieces[i] == "None" || mixsetList[id].armorPieces[i].Contains("000"))
			{
				previewArmorNamesTextList[i].text = "None";
			} else
			{
				List<ArmorItem> armorList = armorSorterManager.GetArmorList(i, gender).armorPieces;
				foreach (ArmorItem armorItem in armorList)
				{
					if (armorItem.name == mixsetList[id].armorPieces[i])
					{
						previewArmorNamesTextList[i].text = armorItem.armorName;
					}
				}
			}
		}
	}
}
