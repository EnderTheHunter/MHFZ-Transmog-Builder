using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SavingSystem
{
	public static void SaveMixset(List<MixsetStruct> mixsetsList)
	{
		BinaryFormatter formatter = new BinaryFormatter();

		string path = Application.persistentDataPath + "/mixsets.jsmt";
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, mixsetsList);
		stream.Close();
	}

	public static List<MixsetStruct> LoadMixset()
	{
		string path = Application.persistentDataPath + "/mixsets.jsmt";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			Debug.Log(path);
			FileStream stream = new FileStream(path, FileMode.Open);

			List<MixsetStruct> mixset = formatter.Deserialize(stream) as List<MixsetStruct>;
			stream.Close();
			return mixset;
		} else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}
}
