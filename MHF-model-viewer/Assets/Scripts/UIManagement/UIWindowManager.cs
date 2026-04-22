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

	public void OpenCloseWindow(int window)
	{
		switch (window)
		{
			case 0:
				if (ArmorListUI.activeInHierarchy == false)
				{
					OpenWindow(window);
				} else
				{
					CloseWindow(window);
				}
				break;
			case 1:
				if (ColorPicker.activeInHierarchy == false)
				{
					OpenWindow(window);
				}
				else
				{
					CloseWindow(window);
				}
				break;
			case 2:
				if (MixsetUI.activeInHierarchy == false)
				{
					OpenWindow(window);
				}
				else
				{
					CloseWindow(window);
				}
				break;
			default:
				break;
		}
	}

	public void OpenWindow(int window) //0 = Armor List, 1 = Color Picker, 2 = Mixset
	{
		switch (window)
		{
			case 0:
				ArmorListUI.SetActive(true);
				ColorPicker.SetActive(false);
				MixsetUI.SetActive(false);
				break;
			case 1:
				ColorPicker.SetActive(true);
				ArmorListUI.SetActive(false);
				MixsetUI.SetActive(false);
				break;
			case 2:
				MixsetUI.SetActive(true);
				ArmorListUI.SetActive(false);
				ColorPicker.SetActive(false);
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
