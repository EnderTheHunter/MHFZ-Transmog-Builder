using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using System.Reflection;

public class FilterManager : MonoBehaviour
{
	public List<Enum> listFilters;

	[SerializeField]
	private Transform filterBox;

	[SerializeField]
	private GameObject filterPrefab;

	private int currentFilter;

	// List to update whenever you add a new filter to the tool
	public FilterManager()
	{
		listFilters = new List<Enum>() {ArmorItem.BlademasterOrGunner.None, 
										ArmorItem.ArmorColor.None, 
										ArmorItem.ArmorColor.None, 
										ArmorItem.BaseType.None, 
										ArmorItem.ArmorStyle.None };
	}

	/// <summary>
	/// Create a button dynamically for every option in an enum type in the filter list.
	/// </summary>
	/// <param name="filter"></param>
	public void InstantiateFilters(int filter)
	{
		var enumFilter = listFilters[filter];

		currentFilter = filter;
		if (enumFilter != null)
		{
			DestroyAll();
			int i = 0;
			foreach (var value in Enum.GetValues(enumFilter.GetType()))
			{
				GameObject newFilter = Instantiate(filterPrefab, filterBox);
				newFilter.GetComponentInChildren<TextMeshProUGUI>().text = Enum.GetName(value.GetType(), i);
				i++;
			}
		}
	}

	public void UpdateCurrentFilter(string filterName)
	{
		var allEnumValues = Enum.GetValues(listFilters[currentFilter].GetType());
		var enumType = listFilters[currentFilter].GetType();
		var result = (int)Enum.Parse(listFilters[currentFilter].GetType(), filterName);
		listFilters[currentFilter] = FromInt(enumType, result);
	}

	private void DestroyAll()
	{
		foreach (Transform child in filterBox.transform)
		{
			Destroy(child.gameObject);
		}
	}

	public static Enum FromInt(Type enumType, int value)
	{
		return (Enum)Enum.ToObject(enumType, value);
	}

	public T UpdateFilter<T>(object value) where T : Enum
	{
		T enumUpdate = (T)Enum.Parse(typeof(T), value.ToString());
		return enumUpdate;
	}

}
