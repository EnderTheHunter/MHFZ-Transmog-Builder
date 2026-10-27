using UnityEngine;
using UnityEngine.EventSystems;

public class HidePrevisualisation : MonoBehaviour, IPointerExitHandler
{
	public void OnPointerExit(PointerEventData eventData)
	{
		Previsualisation.Instance.HidePrevisualisation();
	}
}
