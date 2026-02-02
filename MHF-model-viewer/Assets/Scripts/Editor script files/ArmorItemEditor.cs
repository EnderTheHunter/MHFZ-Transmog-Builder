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

	SerializedProperty blademasterOrGunner;
	SerializedProperty mainColor;
	SerializedProperty secondaryColor;
	SerializedProperty baseType;
	SerializedProperty style;


	void OnEnable()
	{
		armorItem = serializedObject.FindProperty("armorItem");
		armorType = serializedObject.FindProperty(nameof(ArmorItem.type));
		armorVisibleHead = serializedObject.FindProperty(nameof(ArmorItem.isHeadVisible));
		armorDyableHair = serializedObject.FindProperty(nameof(ArmorItem.isHairDyable));

		blademasterOrGunner = serializedObject.FindProperty("blademasterOrGunner");
		mainColor = serializedObject.FindProperty("mainColor");
		secondaryColor = serializedObject.FindProperty("secondaryColor");
		baseType = serializedObject.FindProperty("baseType");
		style = serializedObject.FindProperty("style");
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
		EditorGUILayout.PropertyField(blademasterOrGunner);
		EditorGUILayout.PropertyField(mainColor);
		EditorGUILayout.PropertyField(secondaryColor);
		EditorGUILayout.PropertyField(baseType);
		EditorGUILayout.PropertyField(style);
		serializedObject.ApplyModifiedProperties();
	}
}
#endif
