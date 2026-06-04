using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterSpin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private InputActionReference clickAction;
	[SerializeField]
	private InputActionReference mousePosition;

	public float speed = 10f;
	private bool isRotating = false;
	private bool isHovering = false;
	private float startMousePosition;
	private bool isClicking = false;

	private void OnEnable()
	{
		clickAction.action.performed += MouseClick;
		clickAction.action.canceled += MouseRelease;
	}

	private void OnDisable()
	{
		clickAction.action.performed -= MouseClick;
		clickAction.action.canceled -= MouseRelease;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isHovering = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isHovering = false;
	}

	private void MouseClick(InputAction.CallbackContext context)
	{
		isClicking = true;
	}

	private void MouseRelease(InputAction.CallbackContext context)
	{
		isClicking = false;
	}

	/// <summary>
	/// Check if click + drag is performed on the character. If so, make it spin !
	/// </summary>
	private void Update()
	{
		if (isClicking == true && isHovering == true && isRotating == false)
		{
			isRotating = true;
			startMousePosition = mousePosition.action.ReadValue<Vector2>().x;
		}
		else if (isClicking == false)
		{
			isRotating = false;
		}
		if (isRotating)
		{
			float currentMousePosition = mousePosition.action.ReadValue<Vector2>().x;
			float mouseMovement = currentMousePosition - startMousePosition;

			transform.Rotate(Vector3.up, -mouseMovement * speed * Time.deltaTime);
			startMousePosition = currentMousePosition;
		}
	}
}
