using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
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
		if (PlayerArmorInventory.Instance.GetGender() == ArmorItem.Gender.Female)
		{
			newActiveList += 5;
		}
		currentActiveList = armorList[newActiveList];
		currentActiveList.gameObject.SetActive(true);
		StartCoroutineApplySearch();
	}

	public void StartCoroutineApplySearch()
	{
		StopAllCoroutines();
		StartCoroutine(ApplySearch());
	}

	private IEnumerator ApplySearch()
	{
		string searchBarInput = "";

		if (searchBar != null)
		{
			searchBarInput = searchBar.GetComponent<TMP_InputField>().text;
		}

		DestroyAll();
		foreach (ArmorItem armorItem in currentActiveList.armorPieces)
		{
			List<Enum> activeFilters = FindFirstObjectByType<FilterManager>(FindObjectsInactive.Include).listFilters;
			List<Enum> listFilters = new List<Enum>(){armorItem.blademasterOrGunner, armorItem.mainColor, armorItem.secondaryColor, armorItem.baseType, armorItem.style};
			List<Enum> listDefaultValues = new List<Enum>() {ArmorItem.BlademasterOrGunner.None, ArmorItem.ArmorColor.None, ArmorItem.ArmorColor.None, ArmorItem.BaseType.None, ArmorItem.ArmorStyle.None};
			if (armorItem.armorName.Length >= searchBarInput.Length)
			{
				if (searchBarInput.Length == 0 || armorItem.armorName.ToLower().Contains(searchBarInput.ToLower()))
				{
					int validFilters = 0;
					int i = 0;
					foreach (Enum item in listFilters)
					{
						if (SortArmorElement(item, activeFilters[i], listDefaultValues[i]) == true)
						{
							validFilters++;
						}
						i++;
					}
					if (validFilters == listFilters.Count)
					{
						GameObject armor = Instantiate(resultPrefab, resultContent.transform);
						armor.GetComponentInChildren<TextMeshProUGUI>().text = armorItem.armorName;
						armor.GetComponent<ArmorInfoInPrefab>().m_Item = armorItem;
						yield return null;
					}
				}
			}
		}
		yield return null;
	}

	private void DestroyAll()
	{
		foreach(Transform child in resultContent.transform)
		{
			Destroy(child.gameObject);
		}
	}

	public List<Enum> FetchAllFilters()
	{
		List<Enum> filters = new List<Enum>();
		return filters;
	}

	public static bool SortArmorElement<T>(T armorItem, T filterValue, T defaultValue) where T : Enum
	{
		if (armorItem != null && filterValue != null)
		{
			if (EqualityComparer<T>.Default.Equals(filterValue, defaultValue) == true || EqualityComparer<T>.Default.Equals(armorItem, filterValue) == true)
			{
				return true;
			}
		}
		return false;
	}
}
