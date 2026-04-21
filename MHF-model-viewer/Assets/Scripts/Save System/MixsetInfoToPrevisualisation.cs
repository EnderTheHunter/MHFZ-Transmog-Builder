using UnityEngine;
using UnityEngine.EventSystems;

public class MixsetInfoToPrevisualisation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public int mixsetID = 0;

	public void UpdateID(int newID)
	{
		mixsetID = newID;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		MixsetManager.Instance.LoadMixsetPreview(mixsetID);
	}

	public void OnPointerExit(PointerEventData eventData)
	{

	}
}
