using UnityEngine;

public class CharacterSpin : MonoBehaviour
{
	public float speed = 10f;
	private bool isRotating = false;
	private bool isHovering = false;
	private float startMousePosition;

	void OnMouseEnter()
	{
		isHovering = true;

	}

	void OnMouseExit()
	{
		isHovering = false;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) == true && isHovering == true)
		{
			isRotating = true;
			startMousePosition = Input.mousePosition.x;
		} else if (Input.GetMouseButtonUp(0) == true)
		{
			isRotating = false;
		}
		if (isRotating)
		{
			float currentMousePosition = Input.mousePosition.x;
			float mouseMovement = currentMousePosition - startMousePosition;

			transform.Rotate(Vector3.up, -mouseMovement * speed * Time.deltaTime);
			startMousePosition = currentMousePosition;
		}
	}
}
