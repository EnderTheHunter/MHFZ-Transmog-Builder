#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public static class BindPoseFixer
{
	[MenuItem("Tools/Fix Bindposes On Selected Model")]
	public static void FixSelected()
	{
		GameObject selected = Selection.activeGameObject;
		if (selected == null)
		{
			Debug.LogError("Sélectionne un GameObject (instance du prefab) dans la scène ou le prefab en mode édition.");
			return;
		}

		SkinnedMeshRenderer[] renderers = selected.GetComponentsInChildren<SkinnedMeshRenderer>();

		foreach (SkinnedMeshRenderer smr in renderers)
		{
			FixBindposes(smr);
		}

		Debug.Log($"{renderers.Length} SkinnedMeshRenderer(s) corrigé(s) sur {selected.name}");
	}

	private static string SanitizeFileName(string name)
	{
		// Remplace tout caractère invalide dans un nom de fichier par "_"
		foreach (char invalidChar in Path.GetInvalidFileNameChars())
		{
			name = name.Replace(invalidChar, '_');
		}
		return name;
	}

	private static void FixBindposes(SkinnedMeshRenderer smr)
	{
		Mesh originalMesh = smr.sharedMesh;
		Transform[] bones = smr.bones;

		Matrix4x4[] newBindPoses = new Matrix4x4[bones.Length];
		for (int i = 0; i < bones.Length; i++)
		{
			newBindPoses[i] = bones[i].worldToLocalMatrix * smr.transform.localToWorldMatrix;
		}

		Mesh newMesh = Object.Instantiate(originalMesh);
		string safeName = SanitizeFileName(originalMesh.name) + "_FixedBindpose";
		newMesh.name = safeName;
		newMesh.bindposes = newBindPoses;

		string folder = "Assets/FixedMeshes";
		if (!Directory.Exists(folder))
			AssetDatabase.CreateFolder("Assets", "FixedMeshes");

		string path = $"{folder}/{safeName}_{smr.GetInstanceID()}.asset";
		AssetDatabase.CreateAsset(newMesh, path);
		AssetDatabase.SaveAssets();

		smr.sharedMesh = newMesh;
		EditorUtility.SetDirty(smr);

		Debug.Log($"Bindpose recalculé et sauvegardé : {path}");
	}

	[MenuItem("Tools/Force Reimport All Models")]
	public static void ReimportAllModels()
	{
		AssetDatabase.ImportAsset("Assets/Resources/Armors/m", ImportAssetOptions.ImportRecursive | ImportAssetOptions.ForceUpdate);
	}
}
#endif