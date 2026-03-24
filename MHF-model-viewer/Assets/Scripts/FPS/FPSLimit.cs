using UnityEngine;

public class FPSLimit : MonoBehaviour
{
	private void Awake()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}
}
