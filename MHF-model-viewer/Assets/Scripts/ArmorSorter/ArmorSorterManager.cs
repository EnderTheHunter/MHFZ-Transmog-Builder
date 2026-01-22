using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArmorSorterManager : MonoBehaviour
{
	public enum ArmorTypeListName
	{
		Helmet_M,
		Torso_M,
		Arms_M,
		Waist_M,
		Legs_M,
		Helmet_F,
		Torso_F,
		Arms_F,
		Waist_F,
		Legs_F
	}

	[SerializeField]
	List<ArmorTypeList> armorList;

	ArmorTypeList currentActiveList;

	[SerializeField]
	private GameObject resultPrefab;

	[SerializeField]
	private GameObject resultContent;

	[SerializeField]
	private GameObject searchBar;

	public void UpdateActiveList(int newActiveList)
	{
		if (currentActiveList != null)
		{
			currentActiveList.gameObject.SetActive(false);
		}
		currentActiveList = armorList[newActiveList];
		currentActiveList.gameObject.SetActive(true);
	}

	public void ApplySearch()
	{
		string searchBarInput = "";

		if (searchBar != null)
		{
			searchBarInput = searchBar.GetComponent<TMP_InputField>().text;
		}

		DestroyAll();
		foreach (ArmorItem armorItem in currentActiveList.armorPieces)
		{
			Debug.Log("Ici");
			if (armorItem.armorName.Length >= searchBarInput.Length)
			{
				Debug.Log("Là");
				if (searchBarInput.Length == 0 || armorItem.armorName.ToLower().Contains(searchBarInput.ToLower()))
				{
					Debug.Log("Win");
					GameObject armor = Instantiate(resultPrefab, resultContent.transform);
					armor.GetComponentInChildren<TextMeshProUGUI>().text = armorItem.armorName;
					armor.GetComponent<ArmorInfoInPrefab>().m_Item = armorItem;
				}
			}
		}
	}

	private void DestroyAll()
	{
		foreach(Transform child in resultContent.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
