using UnityEditor;

public class ModelImport : AssetPostprocessor
{
	private void OnPreprocessModel()
	{
		ModelImporter importer = assetImporter as ModelImporter;
		string name = importer.assetPath.ToLower();
		string extension = name.Substring(name.LastIndexOf('.')).ToLower();
		switch(extension)
		{
			case ".fbx":
				importer.importNormals = ModelImporterNormals.Calculate;
				importer.normalSmoothingAngle = 180f;
				break;
			default:
				break;
		}
	}
}
