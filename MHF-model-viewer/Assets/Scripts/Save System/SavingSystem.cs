using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SavingSystem
{

	public static void Save<T>(T obj, string filename)
	{
		BinaryFormatter formatter = new BinaryFormatter();

		string path = Application.persistentDataPath + filename;
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, obj);
		stream.Close();
	}

	public static T Load<T>(string filename)
	{
		string path = Application.persistentDataPath + filename;
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			Debug.Log(path);
			FileStream stream = new FileStream(path, FileMode.Open);

			T obj = (T)formatter.Deserialize(stream);
			stream.Close();
			return obj;
		}
		else
		{
			Debug.LogError("Save file not found in " + path);
			return default;
		}
	}
}
