using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR)
[CustomEditor(typeof(ArmorItem))]
[CanEditMultipleObjects]
public class ArmorItemEditor : Editor
{
	SerializedProperty armorItem;
	SerializedProperty armorType;
	SerializedProperty armorVisibleHead;
	SerializedProperty armorDyableHair;


	void OnEnable()
	{
		armorItem = serializedObject.FindProperty("armorItem");
		armorType = serializedObject.FindProperty(nameof(ArmorItem.type));
		armorVisibleHead = serializedObject.FindProperty(nameof(ArmorItem.isHeadVisible));
		armorDyableHair = serializedObject.FindProperty(nameof(ArmorItem.isHairDyable));
	}

	public override void OnInspectorGUI()
	{
		var armorItem = (ArmorItem)target;

		DrawDefaultInspector();
		serializedObject.Update();
		if (armorType.intValue == 1)
		{
			EditorGUILayout.PropertyField(armorVisibleHead);
			EditorGUILayout.PropertyField(armorDyableHair);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
#endif
