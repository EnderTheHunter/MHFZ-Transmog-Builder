#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class AddressableModelSetup
{
	private const string GroupName = "Equipment_Models";

	[MenuItem("Tools/Setup Addressable Models")]
	public static void SetupAll()
	{
		var settings = AddressableAssetSettingsDefaultObject.Settings;

		AddressableAssetGroup group = settings.FindGroup(GroupName);
		if (group == null)
		{
			group = settings.CreateGroup(GroupName, false, false, false, null,
				typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema));
		}

		// On cherche large (tous les GameObject), puis on filtre nous-mêmes sur l'extension .fbx
		string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/Resources/Prefabs/Base model/f/" });

		int count = 0;
		var seenIds = new HashSet<string>();

		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);

			// Ne garder que les fichiers .fbx (on ignore les prefabs standards, etc.)
			if (!path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
				continue;

			// L'id correspond au nom du dossier parent, pas au nom du fichier
			string parentFolder = Path.GetFileName(Path.GetDirectoryName(path));

			if (string.IsNullOrEmpty(parentFolder))
			{
				Debug.LogWarning($"Impossible de déterminer le dossier parent pour {path}, asset ignoré.");
				continue;
			}

			// Sécurité : détecter les doublons (deux .fbx dans des dossiers de même nom, ou un dossier avec plusieurs .fbx)
			if (!seenIds.Add(parentFolder))
			{
				Debug.LogWarning($"ID en doublon détecté : '{parentFolder}' (fichier {path}). " +
								  $"Vérifie qu'il n'y a qu'un seul .fbx par dossier et que les noms de dossiers sont uniques.");
				continue;
			}

			var entry = settings.CreateOrMoveEntry(guid, group);
			entry.address = parentFolder;

			count++;
		}

		Debug.Log($"{count} modèles .fbx configurés en Addressable dans le groupe {GroupName}");
		AssetDatabase.SaveAssets();
	}
}
#endif