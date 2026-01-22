using UnityEngine;
using UnityEngine.EventSystems;

public class ArmorInfoInPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public ArmorItem m_Item;

	public void SendArmorPieceToInventory()
	{
		if (m_Item != null)
		{
			FindFirstObjectByType<PlayerArmorInventory>().ChangeArmor(m_Item);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Previsualisation.Instance.UpdatePrevisualisation(m_Item);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Previsualisation.Instance.HidePrevisualisation();
	}
}
