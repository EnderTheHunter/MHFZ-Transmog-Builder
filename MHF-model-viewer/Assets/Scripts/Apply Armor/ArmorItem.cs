using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu]
public class ArmorItem : ScriptableObject
{
	public enum ArmorType
	{
		Face,
		Helmet,
		Torso,
		Arms,
		Belt,
		Legs
	}

	public enum Gender
	{
		Male,
		Female
	}

	public enum ArmorColor
	{
		None,
		Red,
		Blue,
		Green,
		Yellow,
		Orange,
		Purple,
		Pink,
		White,
		Grey,
		Black
	}

	public enum BaseType
	{
		None,
		Classic,
		Event,
		Collab
	}

	public enum ArmorStyle
	{
		None,
		Armor,
		Clothes,
		Hair,
		Futuristic,
		Costume
	}

	public enum BlademasterOrGunner
	{
		None,
		Blademaster,
		Gunner
	}

	[Header("MAIN PARAMETERS /!\\ MANDATORY /!\\")]
	public string armorName;
	public Gender gender;
	public GameObject modelPrefab;
	public ArmorType type;
	[HideInInspector]
	public bool isHeadVisible;
	[HideInInspector]
	public bool isHairDyable;

	[Header("FILTERS")]
	[HideInInspector]
	public BlademasterOrGunner blademasterOrGunner;
	[HideInInspector]
	public ArmorColor mainColor;
	[HideInInspector]
	public ArmorColor secondaryColor;
	[HideInInspector]
	public BaseType baseType;
	[HideInInspector]
	public ArmorStyle style;
}
