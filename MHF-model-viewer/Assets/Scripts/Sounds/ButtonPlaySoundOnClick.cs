using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class ButtonPlaySoundOnClick : MonoBehaviour
{
	[SerializeField]
	private AudioClip soundEffect;

	[SerializeField, Range(0.0f, 1.0f)]
	private float volume = 1.0f;

	[SerializeField]
	private float trimIntroTime = 0.0f;

	private void Start()
	{
		Button btn= GetComponent<Button>();
		btn.onClick.AddListener(PlaySound);
	}

	private void PlaySound()
	{
		SoundFXManager.Instance.PlaySoundFXClip(soundEffect, this.transform, volume, trimIntroTime);
	}
}
