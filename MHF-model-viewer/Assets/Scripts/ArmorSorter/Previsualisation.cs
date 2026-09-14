using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.Audio.ProcessorInstance;

public class Previsualisation : MonoBehaviour
{
    private static Previsualisation instance;
    public static Previsualisation Instance => instance;

	private GameObject currentArmorDisplayed;
	private string currentArmorId;
	private CancellationTokenSource _debounceCts;
	private int _requestVersion = 0;

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

	/// <summary>
	/// Duplicate from PlayerArmorInventory, to remove eventually
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
	/// Singleton initialization
	/// </summary>
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

	/// <summary>
	/// Instantiate the selected armor piece in the previsualisation space
	/// </summary>
	/// <param name="item"></param>
	public async void UpdatePrevisualisation(ArmorItem item)
	{
		_debounceCts?.Cancel();
		_debounceCts = new CancellationTokenSource();
		var token = _debounceCts.Token;

		try
		{
			await Task.Delay(100, token); // attend que le scroll se stabilise
		}
		catch (TaskCanceledException)
		{
			return; // un nouveau scroll a annulé cette attente
		}
		previsualationObject.SetActive(true);
		await ShowModel(item.id);
		Transform currentPivot = armorPivot[ChooseArmorPiece(item.type)];
		this.transform.position = currentPivot.position;
		UpdateMaterials(currentArmorDisplayed.GetComponentsInChildren<SkinnedMeshRenderer>());
		if (item.type == ArmorItem.ArmorType.Arms)
		{
			ApplyBaseArmsPosition();
		}
	}

	private async Task ShowModel(string modelId)
	{
		int thisRequest = ++_requestVersion;

		GameObject instance = await ModelManager.Instance.LoadModelById(
			modelId, mainPivot);

		// si une requête plus récente a été lancée entre-temps, on jette ce résultat
		if (thisRequest != _requestVersion)
		{
			if (instance != null)
			{
				Destroy(instance);
				ModelManager.Instance.ReleaseModel(modelId);
			}
			return;
		}

		// requête toujours valide : on remplace proprement l'instance affichée
		if (currentArmorDisplayed != null)
		{
			Destroy(currentArmorDisplayed);
			ModelManager.Instance.ReleaseModel(currentArmorId);
		}

		currentArmorDisplayed = instance;
		currentArmorId = modelId;
	}

	private void OnDestroy()
	{
		_debounceCts?.Cancel();

		if (currentArmorDisplayed != null)
		{
			Destroy(currentArmorDisplayed);
			ModelManager.Instance.ReleaseModel(currentArmorId);
		}
	}

	private async void ShowModel(ArmorItem item)
	{
		if (currentArmorDisplayed != null)
		{
			Destroy(currentArmorDisplayed);
			ModelManager.Instance.ReleaseModel(currentArmorId);
		}
		previsualationObject.SetActive(true);
		Transform currentPivot = armorPivot[ChooseArmorPiece(item.type)]; //Choose the right pivot so the armor piece is always centered in the preview
		currentArmorDisplayed = await ModelManager.Instance.LoadModelById(item.id, mainPivot);
		currentArmorId = item.id;
		this.transform.position = currentPivot.position;
		//this.transform.rotation = currentPivot.rotation;
		UpdateMaterials(currentArmorDisplayed.GetComponentsInChildren<SkinnedMeshRenderer>());
		if (item.type == ArmorItem.ArmorType.Arms)
		{
			ApplyBaseArmsPosition();
		}
	}

	/// <summary>
	/// Change the arms position so they don't T pose and fit in the preview space
	/// </summary>
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
			ModelManager.Instance.ReleaseModel(currentArmorId);
		}
		previsualationObject.SetActive(false);
	}

	/// <summary>
	/// Update the materials to fit with the changes made in the ArmorMeshManager. Could probably be improved.
	/// </summary>
	/// <param name="meshes"></param>
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
