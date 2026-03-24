using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class FilterButton : MonoBehaviour
{
	private Button mButton;

	private void Start()
	{
		mButton = GetComponent<Button>();
		mButton.onClick.AddListener(UpdateFilter);
	}

	private void UpdateFilter()
	{
		string text = GetComponentInChildren<TextMeshProUGUI>().text;
		Object.FindFirstObjectByType<FilterManager>().UpdateCurrentFilter(text);
		GameObject.FindWithTag("FilterValues").gameObject.SetActive(false);
	}
}
