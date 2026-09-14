using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class ArmorSorterManager : MonoBehaviour
{
	//All armor pieces lists
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

	List<string> armorList = new List<string>() { "m_hair", "m_body", "m_arm", "m_wst", "m_leg", "f_hair", "f_body", "f_arm", "f_wst", "f_leg"};
	string currentActiveList;
	private int currentListValue = -1;

	[SerializeField]
	private GameObject resultPrefab;

	[SerializeField]
	private GameObject resultContent;

	[SerializeField]
	private ScrollRect resultScrollRect;

	[SerializeField]
	private GameObject searchBar;

	[SerializeField]
	private List<Sprite> armorIconSprites;

	[SerializeField]
	private ArmorMeshManager armorMeshManager;

	[HideInInspector]
	public string dbName;

	private void Start()
	{
		dbName = Application.streamingAssetsPath + "/DB/ArmorDB.db";
		dbName.Replace(Directory.GetCurrentDirectory().Replace("\\", "/") + "/", "");
		dbName = "URI=file:" + dbName;
	}

	/// <summary>
	/// Change the current armor pieces list to display
	/// </summary>
	/// <param name="newActiveList"></param>
	public void UpdateActiveList(int newActiveList)
	{
		/*if (currentActiveList != null)
		{
			currentActiveList.gameObject.SetActive(false);
		}*/
		if (PlayerArmorInventory.Instance.GetGender() == ArmorItem.Gender.Female)
		{
			newActiveList += 5;
		}
		/*if (newActiveList != currentListValue)
		{
			currentListValue = newActiveList;
			currentActiveList = armorList[newActiveList];
			currentActiveList.gameObject.SetActive(true);
			//ApplySearch();
			SearchArmorInDatabase();
		}*/
		Debug.Log(armorList[newActiveList]);
		currentActiveList = armorList[newActiveList];
		SearchArmorInDatabase();
	}

	/// <summary>
	/// Iterate through the current armorList, remove the elements not corresponding to all filters selected and instantiate a gameobject for every result found
	/// </summary>
	/*public void ApplySearch()
	{
		string searchBarInput = "";

		if (searchBar != null)
		{
			searchBarInput = searchBar.GetComponent<TMP_InputField>().text;
		}

		DestroyAll();
		int iterations = 0;
		List<Enum> activeFilters = FindFirstObjectByType<FilterManager>(FindObjectsInactive.Include).listFilters;
		List<Enum> listDefaultValues = new List<Enum>() {ArmorItem.BlademasterOrGunner.None, ArmorItem.ArmorColor.None, ArmorItem.ArmorColor.None, ArmorItem.BaseType.None, ArmorItem.ArmorStyle.None};
		foreach (ArmorItem armorItem in currentActiveList.armorPieces)
		{
			List<Enum> listFilters = new List<Enum>(){armorItem.blademasterOrGunner, armorItem.mainColor, armorItem.secondaryColor, armorItem.baseType, armorItem.style};

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
						armor.GetComponent<ArmorInfoInPrefab>().armorIcon.sprite = armorIconSprites[armorMeshManager.ChooseArmorPiece(armorItem.type)];
						iterations++;
					}
					if (iterations == 20)
					{
						iterations = 0;
					}
				}
			}
		}
		resultScrollRect.verticalNormalizedPosition = 1;
	}*/

	public void SearchArmorInDatabase()
	{
		DestroyAll();
		using (SqliteConnection connection = new SqliteConnection(dbName))
		{
			if (File.Exists(connection.ConnectionString))
				Debug.Log("Oui + " + dbName);
			else
				Debug.Log("Non + " + connection.ConnectionString);
			connection.Open();

			using (var command = connection.CreateCommand())
			{
				command.CommandText = CreateDatabaseSearchCommand(currentActiveList);

				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						ArmorItem armorPiece = ScriptableObject.CreateInstance<ArmorItem>();
						armorPiece.armorName = (string)reader["armorName"];
						armorPiece.gender = ConvertObjToEnum<ArmorItem.Gender>(reader["gender"]);
						armorPiece.type = ConvertObjToEnum<ArmorItem.ArmorType>(reader["type"]);
						armorPiece.isHeadVisible = Convert.ToBoolean(reader["isHeadVisible"]);
						armorPiece.isHairDyable = Convert.ToBoolean(reader["isHairDyable"]);
						armorPiece.blademasterOrGunner = ConvertObjToEnum<ArmorItem.BlademasterOrGunner>(reader["blademasterOrGunner"]);
						armorPiece.mainColor = ConvertObjToEnum<ArmorItem.ArmorColor>(reader["mainColor"]);
						armorPiece.secondaryColor = ConvertObjToEnum<ArmorItem.ArmorColor>(reader["secondaryColor"]);
						armorPiece.baseType = ConvertObjToEnum<ArmorItem.BaseType>(reader["baseType"]);
						armorPiece.style = ConvertObjToEnum<ArmorItem.ArmorStyle>(reader["style"]);
						armorPiece.id = CreateArmorId(armorPiece, (string)reader["id"]);

						GameObject armor = Instantiate(resultPrefab, resultContent.transform);
						armor.GetComponentInChildren<TextMeshProUGUI>().text = armorPiece.armorName;
						armor.GetComponent<ArmorInfoInPrefab>().m_Item = armorPiece;
						armor.GetComponent<ArmorInfoInPrefab>().armorIcon.sprite = armorIconSprites[armorMeshManager.ChooseArmorPiece(armorPiece.type)];

						//Debug.Log(armorPiece.armorName + " color is " + armorPiece.mainColor);
					}
					reader.Close();
				}
			}

			connection.Close();
		}
	}

	public string CreateDatabaseSearchCommand(string tableName)
	{
		string stringVal = "SELECT * FROM " + currentActiveList;

		if (searchBar != null)
		{
			stringVal += " WHERE armorName LIKE \'%" + searchBar.GetComponent<TMP_InputField>().text + "%\'";
		}

		List<string> filtersDbName = new List<string>() { "blademasterOrGunner", "mainColor", "secondaryColor", "baseType", "style"};
		List<Enum> listDefaultValues = new List<Enum>() { ArmorItem.BlademasterOrGunner.None, ArmorItem.ArmorColor.None, ArmorItem.ArmorColor.None, ArmorItem.BaseType.None, ArmorItem.ArmorStyle.None };
		List<Enum> activeFilters = FindFirstObjectByType<FilterManager>(FindObjectsInactive.Include).listFilters;
		for (int i = 0; i < filtersDbName.Count; i++)
		{
			if (activeFilters[i] != null && NewSortArmorElement(activeFilters[i], listDefaultValues[i]) == false)
			{
				stringVal += " AND " + filtersDbName[i] + " = " + activeFilters[i].ToString("d");
			}
		}
		Debug.Log(stringVal);
		return stringVal;
	}

	public T ConvertObjToEnum<T>(object obj)
	{
		if (obj != null && obj != System.DBNull.Value)
		{
			T enumVal= (T)Enum.Parse(typeof(T), obj.ToString());
			return enumVal;
		} else
		{
			return ((T[])Enum.GetValues(typeof(T)))[0];
		}
	}

	public static bool NewSortArmorElement<T>(T filterValue, T defaultValue) where T : Enum
	{
		if (filterValue != null)
		{
			if (EqualityComparer<T>.Default.Equals(filterValue, defaultValue) == true)
			{
				return true;
			}
		}
		return false;
	}

	public string CreateArmorId(ArmorItem item, string modelId)
	{
		string id;

		if (item.gender == ArmorItem.Gender.Male)
		{
			id = "m_";
		} else
		{
			id = "f_";
		}
		switch (item.type)
		{
			case ArmorItem.ArmorType.Helmet :
				id += "hair";
				break;
			case ArmorItem.ArmorType.Torso :
				id += "body";
				break;
			case ArmorItem.ArmorType.Arms :
				id += "arm";
				break;
			case ArmorItem.ArmorType.Belt :
				id += "wst";
				break;
			case ArmorItem.ArmorType.Legs :
				id += "leg";
				break;
			default:
				break;
		}
		while (modelId.Length < 3)
		{
			modelId = "0" + modelId;
		}
		id += modelId;
		Debug.Log(id);
		return id;
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

	/// <summary>
	/// Check if the current armorItem corresponds to the filterValue or defaultValue (if no filter has been selected)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="armorItem"></param>
	/// <param name="filterValue"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
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

	public string GetArmorList(int value, ArmorItem.Gender gender)
	{
		if (gender == ArmorItem.Gender.Male)
		{
			return armorList[value];
		} else
		{
			return armorList[value + 5];
		}
	}
}
