using System;
using UnityEngine;

internal class DefaultCameraController : MonoBehaviour
{
	private void Start()
	{
		Camera.main.cullingMask &= ~(1 << GUICamera.UILayer);
	}
}
