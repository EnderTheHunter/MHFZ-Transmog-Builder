using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
	[SerializeField]
	private AudioMixer audioMixer;

	public void SetMasterVolume(float volume)
	{
		audioMixer.SetFloat("Master Volume", Mathf.Log10(volume) * 20f);
	}

	public void SetSFXVolume(float volume)
	{
		audioMixer.SetFloat("SFX Volume", Mathf.Log10(volume) * 20f);
	}

	public void SetBGMVolume(float volume)
	{
		audioMixer.SetFloat("BGM Volume", Mathf.Log10(volume) * 20f);
	}
}
