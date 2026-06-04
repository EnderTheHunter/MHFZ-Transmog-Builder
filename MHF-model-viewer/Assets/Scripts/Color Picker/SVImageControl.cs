using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UNUSED FEATURE SINCE MHFRONTIER USES RGB TO COLOR THE HAIRS INSTEAD OF SVI
/// </summary>
public class SVImageControl : MonoBehaviour, IDragHandler, IPointerClickHandler
{
	[SerializeField]
	private Image pickerImage;

	private RawImage SVImage;

	private ColorPickerControl CC;

	private RectTransform rectTransform;

	private RectTransform pickerTransform;

	private void Awake()
	{
		SVImage = GetComponent<RawImage>();
		CC = FindFirstObjectByType<ColorPickerControl>();
		rectTransform = GetComponent<RectTransform>();

		pickerTransform = pickerImage.GetComponent<RectTransform>();
		pickerTransform.position = new Vector2(-(rectTransform.sizeDelta.x * 0.5f), -(rectTransform.sizeDelta.y * 0.5f));
	}

	private void UpdateColor(PointerEventData eventData)
	{
		Vector3 pos = rectTransform.InverseTransformPoint(eventData.position);

		float deltaX = rectTransform.sizeDelta.x * 0.5f;
		float deltaY = rectTransform.sizeDelta.y * 0.5f;

		pos.x = Mathf.Clamp(pos.x, -deltaX, deltaX);
		pos.y = Mathf.Clamp(pos.y, -deltaY, deltaY);

		float x = pos.x + deltaX;
		float y = pos.y + deltaY;

		float xNorm = x / rectTransform.sizeDelta.x;
		float yNorm = y / rectTransform.sizeDelta.y;

		pickerTransform.localPosition = pos;
		pickerImage.color = Color.HSVToRGB(0, 0, 1 - yNorm);
		//CC.SetSaturationValue(xNorm, yNorm);
	}

	public void OnDrag(PointerEventData eventData)
	{
		UpdateColor(eventData);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		UpdateColor(eventData);
	}
}
