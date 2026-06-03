using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(TMP_InputField))]
public class SearchBarPlaySoundEffect : MonoBehaviour
{
	[SerializeField]
	private AudioClip soundEffect;

	[SerializeField, Range(0.0f, 1.0f)]
	private float volume = 1.0f;

	[SerializeField]
	private float trimIntroTime = 0.0f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        TMP_InputField field = GetComponent<TMP_InputField>();
        field.onEndEdit.AddListener(delegate { PlaySound(); });
    }

	private void PlaySound()
	{
		SoundFXManager.Instance.PlaySoundFXClip(soundEffect, this.transform, volume, trimIntroTime);
	}
}
