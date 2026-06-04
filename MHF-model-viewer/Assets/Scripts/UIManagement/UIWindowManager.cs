using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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
	GameObject MainUI;
	[SerializeField]
	GameObject ParameterUI;
	[SerializeField]
	TextMeshProUGUI genderButtonText;

	[SerializeField]
	private AudioClip audioOpen;
	[SerializeField]
	private AudioClip audioClose;
	[SerializeField, Range(0.0f, 1.0f)]
	private float volume;
	[SerializeField]
	private float timeOpen;
	[SerializeField]
	private float timeClose;

	[SerializeField]
	private InputActionReference parameterKey;

	private void OnEnable()
	{
		parameterKey.action.started += ManageParameterWindow;
	}

	private void OnDisable()
	{
		parameterKey.action.started -= ManageParameterWindow;
	}

	/// <summary>
	/// Singleton initialization
	/// </summary>
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

	/// <summary>
	/// Check if given window needs to be opened or closed.
	/// 0 = ArmorList,
	/// 1 = ColorPicker,
	/// 2 = MixsetPage,
	/// 3 = Parameters
	/// </summary>
	/// <param name="window"></param>
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
			case 3:
				if (ParameterUI.activeInHierarchy == false)
				{
					OpenWindow(window);
				} else
				{
					CloseWindow(window);
				}
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Open given window and close the others
	/// </summary>
	/// <param name="window"></param>
	public void OpenWindow(int window) //0 = Armor List, 1 = Color Picker, 2 = Mixset, 3 = Parameters
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
			case 3:
				MainUI.SetActive(false);
				ParameterUI.SetActive(true);
				break;
			default:
				break;
		}
		SoundFXManager.Instance.PlaySoundFXClip(audioOpen, this.transform, volume, timeOpen);
	}

	/// <summary>
	/// Close the given window
	/// </summary>
	/// <param name="window"></param>
	public void CloseWindow(int window) //0 = Armor List, 1 = Color Picker, 2 = Mixset, 3 = Parameters
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
			case 3:
				MainUI.SetActive(true);
				ParameterUI.SetActive(false);
				break;
			default:
				break;
		}
		SoundFXManager.Instance.PlaySoundFXClip(audioClose, this.transform, volume, timeClose);
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

	/// <summary>
	/// Change the text on the gender button to display the current gender
	/// </summary>
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

	private void ManageParameterWindow(InputAction.CallbackContext context)
	{
		OpenCloseWindow(3);
	}
}
