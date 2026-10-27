using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPickerControl : MonoBehaviour
{
	[SerializeField]
	private PlayerArmorInventory playerArmorInventory;
	[SerializeField]
	private ArmorMeshManager armorMeshManager;

	[SerializeField]
	MeshRenderer changeThisColor;

	[SerializeField]
	private Slider currentRed;
	[SerializeField]
	private Slider currentGreen;
	[SerializeField]
	private Slider currentBlue;

	[SerializeField]
	private TextMeshProUGUI redValue;
	[SerializeField]
	private TextMeshProUGUI greenValue;
	[SerializeField]
	private TextMeshProUGUI blueValue;

	private HairColor hairColor;

	private static ColorPickerControl instance;
	public static ColorPickerControl Instance => instance;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
		hairColor = SavingSystem.Load<HairColor>("/hairColor.param");
	}

	private void Start()
	{
		if (hairColor != null)
		{
			redValue.text = hairColor.redValue.ToString();
			currentRed.value = hairColor.redValue;
			greenValue.text = hairColor.greenValue.ToString();
			currentGreen.value = hairColor.greenValue;
			blueValue.text = hairColor.blueValue.ToString();
			currentBlue.value = hairColor.blueValue;
		} else
		{
			hairColor = new HairColor();
		}
	}

	/// <summary>
	/// Update the color on the right armor piece if the armor piece is dyeable
	/// </summary>
	public void UpdateRGBColor()
	{
		Color newColor = new Color((float)(currentRed.value/255), (float)(currentGreen.value/255), (float)(currentBlue.value/255));
		changeThisColor.material.SetColor("_BaseColor", newColor);
		redValue.text = currentRed.value.ToString();
		greenValue.text = currentGreen.value.ToString();
		blueValue.text = currentBlue.value.ToString();

		if (playerArmorInventory != null && playerArmorInventory.armor != null && armorMeshManager.currentModel[0] != null && (playerArmorInventory.armor[0] == null || playerArmorInventory.armor[0].isHairDyable == true))
		{
			armorMeshManager.currentModel[0].GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = newColor;
			hairColor.redValue = currentRed.value;
			hairColor.greenValue = currentGreen.value;
			hairColor.blueValue = currentBlue.value;
			SavingSystem.Save(hairColor, "/hairColor.param");
		}
	}

	public void ApplyHairColor(GameObject obj, ArmorItem item)
	{
		Color newColor = new Color((float)(currentRed.value / 255), (float)(currentGreen.value / 255), (float)(currentBlue.value / 255));

		if (playerArmorInventory != null && playerArmorInventory.armor != null && item.isHairDyable == true)
		{
			obj.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = newColor;
		}
	}

	public void ApplySample(Image image)
	{
		currentRed.value = image.color.r * 255f;
		currentGreen.value = image.color.g * 255f;
		currentBlue.value = image.color.b * 255f;
	}
}
