using UnityEngine;
using UnityEngine.Rendering;

public class SoundFXManager : MonoBehaviour
{
	private static SoundFXManager instance;
	public static SoundFXManager Instance { get { return instance; } }

	[SerializeField]
	private AudioSource soundFXObject;

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

	public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float time=0.0f)
	{
		AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

		audioSource.clip = audioClip;

		audioSource.volume = volume;

		audioSource.Play();

		audioSource.time = time;

		float clipLength = audioSource.clip.length;

		Destroy(audioSource.gameObject, clipLength);
	}
}
