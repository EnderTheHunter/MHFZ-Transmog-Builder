using UnityEditor.PackageManager.UI;
using UnityEngine;

public class UIWindowManager : MonoBehaviour
{
	[SerializeField]
	GameObject ArmorListUI;
	[SerializeField]
	GameObject ColorPicker;
	[SerializeField]
	GameObject MixsetUI;

	public void OpenWindow(int window) //0 = Armor List, 1 = Color Picker, 2 = Mixset
	{
		ArmorListUI.SetActive(false);
		ColorPicker.SetActive(false);
		MixsetUI.SetActive(false);
		switch (window)
		{
			case 0:
				ArmorListUI.SetActive(true);
				break;
			case 1:
				ColorPicker.SetActive(true);
				break;
			case 2:
				MixsetUI.SetActive(true);
				break;
			default:
				break;
		}
	}

	public void CloseWindow(int window) //0 = Armor List, 1 = Color Picker, 2 = Mixset
	{
		switch (window)
		{
			case 0:
				ArmorListUI.SetActive(false);
				break;
			case 1:
				ColorPicker.SetActive(false);
				break;
			case 2:
				MixsetUI.SetActive(false);
				break;
			default:
				break;
		}
	}
}
