using System;
using UnityEngine;
using UnityEngine.UI;

public class ParametersManager : MonoBehaviour
{
	private static ParametersManager instance;
	public static ParametersManager Instance { get { return instance; } }

	public ParametersList parameters;

	[SerializeField]
	private SoundMixerManager soundMixerManager;

	[SerializeField]
	private Slider mainVolumeSlider;
	[SerializeField]
	private Slider sfxVolumeSlider;
	[SerializeField]
	private Slider bgmVolumeSlider;

	public enum ParameterName
	{
		MainVolume,
		SFXVolume,
		BGMVolume
	}

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

		parameters = SavingSystem.Load<ParametersList>("/options.param");
	}

	private void Start()
	{
		if (parameters == null)
		{
			parameters = new ParametersList();
		}
		else
		{
			ApplyParameters();
		}
	}

	private void ApplyParameters()
	{
		soundMixerManager.SetMasterVolume(parameters.mainVolume);
		mainVolumeSlider.value = parameters.mainVolume;
		soundMixerManager.SetSFXVolume(parameters.SFXVolume);
		sfxVolumeSlider.value = parameters.SFXVolume;
		soundMixerManager.SetBGMVolume(parameters.BGMVolume);
		bgmVolumeSlider.value = parameters.BGMVolume;
	}

	public void UpdateParameters<T>(ParameterName paramName, T newValue)
	{
		switch (paramName)
		{
			case ParameterName.MainVolume:
				parameters.mainVolume = (float)Convert.ChangeType(newValue, typeof(float));
				break;
			case ParameterName.SFXVolume:
				parameters.SFXVolume = (float)Convert.ChangeType(newValue, typeof(float));
				break;
			case ParameterName.BGMVolume:
				parameters.BGMVolume = (float)Convert.ChangeType(newValue, typeof(float));
				break;
			default:
				break;
		}
		SavingSystem.Save(parameters, "/options.param");
	}
}
