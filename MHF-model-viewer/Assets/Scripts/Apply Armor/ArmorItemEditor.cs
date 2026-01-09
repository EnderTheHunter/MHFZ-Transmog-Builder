using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArmorItem))]
[CanEditMultipleObjects]
public class ArmorItemEditor : Editor
{
	SerializedProperty armorItem;
	SerializedProperty armorType;
	SerializedProperty armorVisibleHead;


	void OnEnable()
	{
		armorItem = serializedObject.FindProperty("armorItem");
		armorType = serializedObject.FindProperty(nameof(ArmorItem.type));
		armorVisibleHead = serializedObject.FindProperty(nameof(ArmorItem.isHeadVisible));
	}

	public override void OnInspectorGUI()
	{
		var armorItem = (ArmorItem)target;

		DrawDefaultInspector();
		serializedObject.Update();
		if (armorType.intValue == 1)
		{
			EditorGUILayout.PropertyField(armorVisibleHead);
		}
		serializedObject.ApplyModifiedProperties();
	}

}
