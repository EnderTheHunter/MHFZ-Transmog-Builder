using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;
public class BaseArmorMaker : EditorWindow
{
	public string folderPath;
	public string csvNameFilePath;
	public GameObject armorList;
	public ArmorItem.ArmorType armorType;
	public ArmorItem.Gender gender;

	[MenuItem("Window/BaseArmorMaker")]
	public static void ShowWindow()
	{
		GetWindow(typeof(BaseArmorMaker));
	}

	private void OnGUI()
	{
		GUILayout.Label("Base Armor Maker Script", EditorStyles.boldLabel);

		folderPath = EditorGUILayout.TextField("Your path", folderPath);
		csvNameFilePath = EditorGUILayout.TextField("CSV containing armor names", csvNameFilePath);
		armorList = (GameObject)EditorGUILayout.ObjectField(armorList, typeof(GameObject), true);
		armorType = (ArmorItem.ArmorType)EditorGUILayout.EnumPopup(armorType);
		gender = (ArmorItem.Gender)EditorGUILayout.EnumPopup(gender);

		if (GUILayout.Button("Launch"))
		{
			CreateAllFiles();
		}
	}

	private void CreateAllFiles()
	{
		string fullFolderPath = Application.dataPath + "/" + folderPath;
		ArmorTypeList list = armorList.GetComponent<ArmorTypeList>();
		if (Directory.Exists(fullFolderPath) == true)
		{
			Debug.Log("Test");
			var fileData = File.ReadAllText(csvNameFilePath);
			var lines = fileData.Split(new char[] {'\n'});
			var csvContent = CreateDictFromCSV(lines);
			DirectoryInfo dir = new DirectoryInfo(fullFolderPath);
			var subDirs = dir.GetDirectories();
			foreach (var subDir in subDirs)
			{
				string filename = subDir.Name + ".asset";
				string filePath = subDir.FullName + "\\" + filename;
				string armorId = subDir.Name.Replace("m_hair", "");
				if (armorId.StartsWith("0"))
				{
					armorId = armorId[1..];
					if (armorId.StartsWith("0"))
					{
						armorId = armorId[1..];
					}
				}
				if (File.Exists(filePath) == false)
				{
					ScriptableObject item = CreateInstance(typeof(ArmorItem));
					((ArmorItem)item).name = subDir.Name;
					if (!csvContent.ContainsKey(armorId))
					{
						foreach (FileInfo file in subDir.GetFiles())
						{
							file.Delete();
						}
						var path = "Assets/" + folderPath + "/" + subDir.Name;
						AssetDatabase.DeleteAsset(path);
						continue;
					}
					((ArmorItem)item).armorName = csvContent[armorId];
					((ArmorItem)item).type = armorType;
					((ArmorItem)item).gender = gender;
					((ArmorItem)item).availableTransmog = ArmorItem.AvailableTransmog.Transmog;
					string fbxPath = folderPath + "/" + subDir.Name + "/" + "0001_0000001C";
					fbxPath = fbxPath.Replace("Resources/", "");
					//Debug.Log(fbxPath);
					GameObject fbx = Resources.Load<GameObject>(fbxPath);
					if (fbx == null)
					{
						fbxPath = fbxPath.Replace("0001_0000001C", "0001_00000024");
						fbx = Resources.Load<GameObject>(fbxPath);
					}
					((ArmorItem)item).modelPrefab = fbx;
					AssetDatabase.CreateAsset(item, "Assets/" + folderPath + "/" + subDir.Name + "/" + subDir.Name + ".asset");
					list.armorPieces.Add((ArmorItem)item);
				}
			}
		}
	}

	private Dictionary<string, string> CreateDictFromCSV(string[] lines)
	{
		Dictionary<string, string> result = new Dictionary<string, string>();

		foreach (string line in lines)
		{
			string[] parts = line.Split(new char[]{ ',' } );
			if (parts.Length == 1)
			{
				break;
			}
			//parts[3] = parts[3].Remove(parts[3].Length - 1);
			if (result.ContainsKey(parts[3]))
			{
				Debug.Log(parts[3]);
			} else
			{
				result.Add(parts[3], parts[1]);
			}
		}
		return result;
	}
}
