using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
	[SerializeField]
	private AudioMixer audioMixer;

	public void SetMasterVolume(float volume)
	{
		audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20f);
		ParametersManager.Instance.UpdateParameters<float>(ParametersManager.ParameterName.MainVolume, volume);
	}

	public void SetSFXVolume(float volume)
	{
		audioMixer.SetFloat("SFX Volume", Mathf.Log10(volume) * 20f);
		ParametersManager.Instance.UpdateParameters<float>(ParametersManager.ParameterName.SFXVolume, volume);
	}

	public void SetBGMVolume(float volume)
	{
		Debug.Log(volume);
		audioMixer.SetFloat("BGM Volume", Mathf.Log10(volume) * 20f);
		ParametersManager.Instance.UpdateParameters<float>(ParametersManager.ParameterName.BGMVolume, volume);
	}
}
