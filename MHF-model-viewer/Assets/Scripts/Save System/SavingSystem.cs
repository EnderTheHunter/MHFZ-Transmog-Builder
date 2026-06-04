using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SavingSystem
{
	/// <summary>
	/// Saves an obj into a file. The file is stored in the App persistent data path
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object to save</param>
	/// <param name="filename">The name of the file. Has to be named "/[filename].[extension]"</param>
	public static void Save<T>(T obj, string filename)
	{
		BinaryFormatter formatter = new BinaryFormatter();

		string path = Application.persistentDataPath + filename;
		FileStream stream = new FileStream(path, FileMode.Create);

		formatter.Serialize(stream, obj);
		stream.Close();
	}

	/// <summary>
	/// Load a file stored in the App persistent data path and extract the struct inside it
	/// </summary>
	/// <typeparam name="T">Expected type for the structure found inside the file</typeparam>
	/// <param name="filename">The name of the file. Has to be named "/[filename].[extension]</param>
	/// <returns>Returns the loaded structure</returns>
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
