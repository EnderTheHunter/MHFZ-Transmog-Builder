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

	public void UpdateRGBColor()
	{
		Color newColor = new Color((float)(currentRed.value/255), (float)(currentGreen.value/255), (float)(currentBlue.value/255));
		changeThisColor.material.SetColor("_BaseColor", newColor);
		redValue.text = currentRed.value.ToString();
		greenValue.text = currentGreen.value.ToString();
		blueValue.text = currentBlue.value.ToString();

		if (playerArmorInventory != null && playerArmorInventory.armor != null && playerArmorInventory.armor[0] != null && playerArmorInventory.armor[0].isHairDyable == true)
		{
			armorMeshManager.currentModel[0].GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = newColor;
		}
	}
}
