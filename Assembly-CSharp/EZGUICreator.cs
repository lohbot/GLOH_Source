using System;
using UnityEngine;

public class EZGUICreator
{
	public static GUICamera CreateEZGUI()
	{
		GameObject gameObject = UnityEngine.Object.FindObjectOfType(typeof(GameDramaGUICamera)) as GameObject;
		if (gameObject == null)
		{
			gameObject = new GameObject("UI Camera");
			Camera camera = (Camera)gameObject.AddComponent(typeof(Camera));
			camera.clearFlags = CameraClearFlags.Depth;
			camera.cullingMask = 1 << LayerMask.NameToLayer("GUI");
			camera.depth = 10f;
			camera.orthographic = true;
			camera.orthographicSize = 350f;
			camera.farClipPlane = 2000f;
			camera.nearClipPlane = 0.3f;
			camera.tag = "Untagged";
			gameObject.AddComponent(typeof(GameDramaGUICamera));
		}
		return gameObject.GetComponent<GameDramaGUICamera>();
	}
}
