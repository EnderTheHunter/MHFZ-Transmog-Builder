using TMPro;
using UnityEngine;

public class UIWindowManager : MonoBehaviour
{
	private static UIWindowManager instance;
	public static UIWindowManager Instance { get { return instance; } }

	[SerializeField]
	GameObject ArmorListUI;
	[SerializeField]
	GameObject ColorPicker;
	[SerializeField]
	GameObject MixsetUI;
	[SerializeField]
	TextMeshProUGUI genderButtonText;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}
	}

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

	public void SwitchGender()
	{
		if (PlayerArmorInventory.Instance.GetGender() == ArmorItem.Gender.Male)
		{
			PlayerArmorInventory.Instance.SetGender(false);
		} else
		{
			PlayerArmorInventory.Instance.SetGender(true);
		}
	}

	public void UpdateGenderButton()
	{
		if (PlayerArmorInventory.Instance.GetGender() == ArmorItem.Gender.Male)
		{
			genderButtonText.text = "Male";
		}
		else
		{
			genderButtonText.text = "Female";
		}
	}
}
