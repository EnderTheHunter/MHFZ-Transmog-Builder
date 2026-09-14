using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmorMeshManager : MonoBehaviour
{
	public Transform parentOverride;
	public GameObject[] currentModel = new GameObject[6];
	public Transform rootBone;
	private PlayerArmorInventory playerArmorInventory;
	public Texture maleSkin;
	public Texture femaleSkin;
	public Material transparentMat;

	private void Start()
	{
		playerArmorInventory = GetComponent<PlayerArmorInventory>();
	}

	/// <summary>
	/// Convert an armorPiece type into a int value.
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public int ChooseArmorPiece(ArmorItem.ArmorType type)
	{
		switch (type)
		{
			case ArmorItem.ArmorType.Helmet:
				return 0;
			case ArmorItem.ArmorType.Torso:
				return 1;
			case ArmorItem.ArmorType.Arms:
				return 2;
			case ArmorItem.ArmorType.Belt:
				return 3;
			case ArmorItem.ArmorType.Legs:
				return 4;
			case ArmorItem.ArmorType.Face:
				return 5;
			default:
				return -1;
		}
	}

	/// <summary>
	/// Hide the selected armor piece.
	/// </summary>
	/// <param name="type"></param>
	public void UnloadArmorPiece(ArmorItem.ArmorType type)
	{
		int piece = ChooseArmorPiece(type);
		if (currentModel[piece] != null)
		{
			currentModel[piece].SetActive(false);
		}
	}

	/// <summary>
	/// Destroy the selected armor piece.
	/// </summary>
	/// <param name="type"></param>
	public void UnloadArmorPieceAndDestroy(ArmorItem.ArmorType type)
	{
		int piece = ChooseArmorPiece(type);
		if (currentModel[piece] != null)
		{
			Destroy(currentModel[piece]);
			if (piece == 0 && currentModel[5] != null)
			{
				Destroy(currentModel[5]);
			}
		}
	}

	/// <summary>
	/// Load an armor piece and initialize its materials properly.
	/// </summary>
	/// <param name="armorItem"></param>
	public async void LoadArmorPieceModel(ArmorItem armorItem)
	{
		UnloadArmorPieceAndDestroy(armorItem.type);
		int piece = ChooseArmorPiece(armorItem.type);

		if (armorItem == null)
		{
			UnloadArmorPiece(armorItem.type);
			return;
		}

		Debug.Log(armorItem.id);
		GameObject model = await ModelManager.Instance.LoadModelById(armorItem.id, transform);

		if (model != null)
		{
			if (parentOverride != null)
			{
				model.transform.parent = parentOverride;
			}
			else
			{
				model.transform.parent = transform;
			}
			model.transform.localPosition = Vector3.zero;
			model.transform.localRotation = Quaternion.identity;
			model.transform.localScale = Vector3.one;

			SkinnedMeshRenderer[] meshes = model.GetComponentsInChildren<SkinnedMeshRenderer>();
			Transform[] newBones = new Transform[meshes[0].bones.Length];
			newBones = HardcodeBaseBonesForArmorType(newBones, armorItem.type);
			/*if (armorItem.type == ArmorItem.ArmorType.Belt)
			{
				newBones[7] = meshes[0].bones[7];
				newBones[8] = meshes[0].bones[8];
				newBones[9] = meshes[0].bones[9];
				newBones[10] = meshes[0].bones[10];
				newBones[11] = meshes[0].bones[11];
				newBones[12] = meshes[0].bones[12];
				newBones[13] = meshes[0].bones[13];
				newBones[14] = meshes[0].bones[14];
				newBones[15] = meshes[0].bones[15];
				newBones[7].SetParent(rootBone.GetComponentsInChildren<Transform>()[2]);
				newBones[10].SetParent(rootBone.GetComponentsInChildren<Transform>()[2]);
				newBones[13].SetParent(rootBone.GetComponentsInChildren<Transform>()[2]);
				newBones[8].SetParent(newBones[7]);
				newBones[11].SetParent(newBones[10]);
				newBones[14].SetParent(newBones[13]);
				newBones[9].SetParent(newBones[8]);
				newBones[12].SetParent(newBones[11]);
				newBones[15].SetParent(newBones[14]);
			}*/
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].rootBone = rootBone;
				meshes[i].bones = newBones;
				foreach (Material mat in meshes[i].materials)
				{
					mat.EnableKeyword("_ALPHATEST_ON");
					mat.SetFloat("_AlphaClip", 1);
					mat.SetFloat("_Cull", 0);
					mat.SetFloat("_Smoothness", 0);
					if (mat.GetTexture("_BaseMap") == null)
					{
						if (playerArmorInventory.GetGender() == ArmorItem.Gender.Male)
						{
							mat.SetTexture("_BaseMap", maleSkin);
						} else
						{
							mat.SetTexture("_BaseMap", femaleSkin);
						}
					}
					if (i > 0)
					{
						Material newMat = new Material(transparentMat);
						newMat.CopyMatchingPropertiesFromMaterial(transparentMat);
						newMat.SetTexture("_BaseMap", mat.GetTexture("_BaseMap"));
						meshes[i].sharedMaterial = newMat;
					}
				}
			}
		}
		currentModel[piece] = model;
	}

	/// <summary>
	/// Retarget all the bones from an armor piece to the playerbase skeleton
	/// </summary>
	/// <param name="newBones"></param>
	/// <param name="type"></param>
	/// <returns></returns>
	public Transform[] HardcodeBaseBonesForArmorType(Transform[] newBones, ArmorItem.ArmorType type)
	{
		Transform[] mainSkel = rootBone.GetComponentsInChildren<Transform>();
		switch (type)
		{
			case ArmorItem.ArmorType.Face:
				newBones[0] = mainSkel[0];
				newBones[1] = mainSkel[24];
				break;
			case ArmorItem.ArmorType.Helmet:
				newBones[0] = mainSkel[11];
				newBones[1] = mainSkel[12];
				newBones[2] = mainSkel[23];
				newBones[3] = mainSkel[24];
				break;
			case ArmorItem.ArmorType.Torso:
				newBones[0] = mainSkel[0];
				newBones[1] = mainSkel[2];
				newBones[2] = mainSkel[2];
				newBones[3] = mainSkel[12];
				newBones[4] = mainSkel[13];
				newBones[5] = mainSkel[14];
				newBones[6] = mainSkel[18];
				newBones[7] = mainSkel[19];
				newBones[8] = mainSkel[23];
				newBones[9] = mainSkel[24];
				break;
			case ArmorItem.ArmorType.Arms:
				newBones[0] = mainSkel[0];
				newBones[1] = mainSkel[12];
				newBones[2] = mainSkel[13];
				newBones[3] = mainSkel[14];
				newBones[4] = mainSkel[15];
				newBones[5] = mainSkel[16];
				newBones[6] = mainSkel[18];
				newBones[7] = mainSkel[19];
				newBones[8] = mainSkel[20];
				newBones[9] = mainSkel[21];
				break;
			case ArmorItem.ArmorType.Belt:
				newBones[0] = mainSkel[0];
				newBones[1] = mainSkel[2];
				newBones[2] = mainSkel[11];
				newBones[3] = mainSkel[3];
				newBones[4] = mainSkel[4];
				newBones[5] = mainSkel[7];
				newBones[6] = mainSkel[8];
				break;
			case ArmorItem.ArmorType.Legs:
				newBones[0] = mainSkel[0];
				newBones[1] = mainSkel[1];
				newBones[2] = mainSkel[2];
				newBones[3] = mainSkel[3];
				newBones[4] = mainSkel[4];
				newBones[5] = mainSkel[5];
				newBones[6] = mainSkel[7];
				newBones[7] = mainSkel[8];
				newBones[8] = mainSkel[9];
				newBones[9] = mainSkel[11];
				newBones[10] = mainSkel[12];
				newBones[11] = mainSkel[13];
				newBones[12] = mainSkel[14];
				newBones[13] = mainSkel[15];
				newBones[14] = mainSkel[16];
				newBones[15] = mainSkel[18];
				newBones[16] = mainSkel[19];
				newBones[17] = mainSkel[20];
				newBones[18] = mainSkel[21];
				newBones[19] = mainSkel[23];
				newBones[19] = mainSkel[24];
				break;
			default:
				break;
			}
		return newBones;
	}

	public void RemoveExtraBonesFromMainSkel(ArmorItem.ArmorType type)
	{
		Transform[] mainSkel = rootBone.GetComponentsInChildren<Transform>();
		List<string> armorTypes = new List<string>() { "Head", "Body", "Arms", "Waist", "Leg" };
		foreach (Transform bone in mainSkel)
		{
			if (bone.name.Contains("Extra") && bone.name.Contains(armorTypes[(int)type]))
			{
				Destroy(bone.gameObject);
			}
		}
	}

	public Transform[] RetargetBonesByName(Transform[] newBones, Transform[] modelBones)
	{
		Transform[] mainSkel = rootBone.GetComponentsInChildren<Transform>();
		for(int i = 0; i < newBones.Length; i++)
		{
			if (modelBones[i].name.Contains("Extra"))
			{
				newBones[i] = modelBones[i];
				newBones[i].SetParent(newBones[0].Find(modelBones[i].parent.name));
			} else
			{
				newBones[i] = FindChildWithNameRecursive(mainSkel[0], modelBones[i].name);
			}
		}
		return newBones;
	}

	public Transform FindChildWithNameRecursive(Transform parent, string name)
	{
		Transform correctChild = null;
		foreach(Transform child in parent)
		{
			if(child.name == name)
			{
				correctChild = child;
				break;
			}
			else
			{
				correctChild = FindChildWithNameRecursive(child, name);
				if (correctChild != null)
					break;
			}
		}
		return correctChild;
	}

	private Material SetModeTransparent(Material originalMat)
	{
		Material materialTrans = new Material(transparentMat);
		materialTrans.CopyMatchingPropertiesFromMaterial(originalMat);
		return materialTrans;
	}
}
