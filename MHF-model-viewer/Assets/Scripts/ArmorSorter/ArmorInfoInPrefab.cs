using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmorInfoInPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public ArmorItem m_Item;

	public Image armorIcon;

	[SerializeField]
	private AudioClip audioClip;

	/// <summary>
	/// When this armor piece is selected, apply it to the character
	/// </summary>
	public void SendArmorPieceToInventory()
	{
		if (m_Item != null)
		{
			PlayerArmorInventory.Instance.ChangeArmor(m_Item);
		}
		GameObject.FindWithTag("ArmorSorter").gameObject.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Previsualisation.Instance.UpdatePrevisualisation(m_Item);
		//SoundFXManager.Instance.PlaySoundFXClip(audioClip, this.transform, 0.5f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Previsualisation.Instance.HidePrevisualisation();
	}
}
