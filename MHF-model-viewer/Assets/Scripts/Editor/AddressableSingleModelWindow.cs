#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System;
using System.IO;
using UnityEngine;

public class AddressableSingleModelWindow : EditorWindow
{
	private const string GroupName = "Modelsv2";

	private GameObject model;

	// ---------- Fenêtre ----------

	[MenuItem("Tools/Add Single Addressable Model")]
	public static void Open()
	{
		GetWindow<AddressableSingleModelWindow>("Add Armor Piece");
	}

	private void OnGUI()
	{
		EditorGUILayout.HelpBox(
			"Glisse un fichier .fbx ici. L'adresse sera le nom de son dossier parent.",
			MessageType.Info);

		model = (GameObject)EditorGUILayout.ObjectField(
			"Pièce d'armure (.fbx)", model, typeof(GameObject), false);

		using (new EditorGUI.DisabledScope(model == null))
		{
			if (GUILayout.Button("Ajouter aux Addressables"))
			{
				if (TryAdd(model, out string message))
					Debug.Log(message);
				else
					EditorUtility.DisplayDialog("Ajout impossible", message, "OK");
			}
		}
	}

	// ---------- Menu contextuel (clic droit dans le Project) ----------

	[MenuItem("Assets/Add to Addressable Models", true)]
	private static bool ValidateAddSelected()
	{
		var go = Selection.activeObject as GameObject;
		if (go == null) return false;
		return AssetDatabase.GetAssetPath(go).EndsWith(".fbx", StringComparison.OrdinalIgnoreCase);
	}

	[MenuItem("Assets/Add to Addressable Models")]
	private static void AddSelected()
	{
		var go = Selection.activeObject as GameObject;
		if (TryAdd(go, out string message))
			Debug.Log(message);
		else
			EditorUtility.DisplayDialog("Ajout impossible", message, "OK");
	}

	// ---------- Logique commune ----------

	public static bool TryAdd(GameObject asset, out string message)
	{
		if (asset == null)
		{
			message = "Aucun asset sélectionné.";
			return false;
		}

		string path = AssetDatabase.GetAssetPath(asset);

		if (string.IsNullOrEmpty(path))
		{
			message = "Cet objet n'est pas un asset du projet (objet de scène ?).";
			return false;
		}

		if (!path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
		{
			message = $"'{path}' n'est pas un fichier .fbx.";
			return false;
		}

		var settings = AddressableAssetSettingsDefaultObject.Settings;
		if (settings == null)
		{
			message = "Les paramètres Addressables sont introuvables (Window > Asset Management > Addressables > Groups).";
			return false;
		}

		string guid = AssetDatabase.AssetPathToGUID(path);

		// Déjà Addressable (dans n'importe quel groupe)
		var existing = settings.FindAssetEntry(guid);
		if (existing != null)
		{
			message = $"'{path}' est déjà Addressable (groupe '{existing.parentGroup.Name}', adresse '{existing.address}').";
			return false;
		}

		// L'id correspond au nom du dossier parent
		string parentFolder = Path.GetFileName(Path.GetDirectoryName(path));
		if (string.IsNullOrEmpty(parentFolder))
		{
			message = $"Impossible de déterminer le dossier parent de '{path}'.";
			return false;
		}

		// Adresse déjà utilisée par un autre Addressable
		foreach (var g in settings.groups)
		{
			if (g == null) continue;
			foreach (var e in g.entries)
			{
				if (e.address == parentFolder)
				{
					message = $"L'adresse '{parentFolder}' est déjà utilisée (groupe '{g.Name}').";
					return false;
				}
			}
		}

		AddressableAssetGroup group = settings.FindGroup(GroupName);
		if (group == null)
		{
			group = settings.CreateGroup(GroupName, false, false, false, null,
				typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema));
		}

		var entry = settings.CreateOrMoveEntry(guid, group);
		entry.address = parentFolder;

		AssetDatabase.SaveAssets();

		message = $"'{path}' ajouté au groupe '{GroupName}' avec l'adresse '{parentFolder}'.";
		return true;
	}
}
#endif