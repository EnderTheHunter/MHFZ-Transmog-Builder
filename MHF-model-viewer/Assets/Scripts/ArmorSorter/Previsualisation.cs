using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Audio.ProcessorInstance;

public class Previsualisation : MonoBehaviour
{
    private static Previsualisation instance;
    public static Previsualisation Instance => instance;

	private GameObject currentArmorDisplayed;

	[SerializeField]
	private Transform mainPivot;
	[SerializeField]
	private Transform[] armorPivot = new Transform[5];

	[SerializeField]
	private GameObject previsualationObject;

	[SerializeField]
	private Material alphaMaterial;

	[SerializeField]
	private Material transparentMat;

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

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		} else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public void UpdatePrevisualisation(ArmorItem item)
	{
		if (currentArmorDisplayed != null)
		{
			Destroy(currentArmorDisplayed);
		}
		previsualationObject.SetActive(true);
		Transform currentPivot = armorPivot[ChooseArmorPiece(item.type)];
		currentArmorDisplayed = Instantiate(item.modelPrefab, mainPivot);
		this.transform.position = currentPivot.position;
		//this.transform.rotation = currentPivot.rotation;
		UpdateMaterials(currentArmorDisplayed.GetComponentsInChildren<SkinnedMeshRenderer>());
		if (item.type == ArmorItem.ArmorType.Arms)
		{
			ApplyBaseArmsPosition();
		}
	}

	private void ApplyBaseArmsPosition()
	{
		Transform bone3 = FindRecursiveChild(currentArmorDisplayed.transform, "Bone_3");
		Transform bone7 = FindRecursiveChild(currentArmorDisplayed.transform, "Bone_7");

		bone3.localRotation = Quaternion.Euler(0, 90, 45);
		bone7.localRotation = Quaternion.Euler(0, -90, -45);
	}

	private Transform FindRecursiveChild(Transform parent, string name)
	{
		Transform result = null;
		{
			
		}
		if (parent != null)
		{
			foreach (Transform child in parent)
			{
				if (child.name == name)
				{
					result = child;
					break;
				} else
				{
					result = FindRecursiveChild(child, name);
					if (result != null)
					{
						break;
					}
				}
			}
		}
		return result;
	}

	public void HidePrevisualisation()
	{
		if (currentArmorDisplayed != null)
		{
			Destroy(currentArmorDisplayed.gameObject);
		}
		previsualationObject.SetActive(false);
	}

	private void UpdateMaterials(SkinnedMeshRenderer[] meshes)
	{
		for (int i = 0; i < meshes.Length; i++)
		{
			foreach (Material mat in meshes[i].materials)
			{
				mat.EnableKeyword("_ALPHATEST_ON");
				mat.SetFloat("_AlphaClip", 1);
				mat.SetFloat("_Cull", 0);
				mat.SetFloat("_Smoothness", 0);
				if (mat.GetTexture("_BaseMap") == null)
				{
					mat.CopyPropertiesFromMaterial(alphaMaterial);
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
}
