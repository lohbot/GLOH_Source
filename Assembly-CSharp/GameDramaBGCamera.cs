using System;
using UnityEngine;

public class GameDramaBGCamera : MonoBehaviour
{
	private void Awake()
	{
		base.gameObject.transform.localPosition = new Vector3(0f, 0f, -10.5f);
		Camera camera = base.gameObject.AddComponent<Camera>();
		if (camera != null)
		{
			camera.clearFlags = CameraClearFlags.Nothing;
			camera.orthographicSize = 0.1f;
			camera.nearClipPlane = 0.1f;
			camera.farClipPlane = 0.2f;
			camera.orthographic = true;
			Camera camera2 = UnityEngine.Object.FindObjectOfType(typeof(GUICamera)) as Camera;
			if (camera2 != null)
			{
				camera.cullingMask = camera2.cullingMask;
			}
			camera.depth = -1f;
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			gameObject.transform.parent = base.gameObject.transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0.15f);
			gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
			gameObject.layer = LayerMask.NameToLayer("GUI");
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
			MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
			if (component2 != null)
			{
				component2.castShadows = false;
				component2.receiveShadows = false;
				Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				texture2D.filterMode = FilterMode.Bilinear;
				texture2D.wrapMode = TextureWrapMode.Repeat;
				Color[] pixels = texture2D.GetPixels();
				for (int i = 0; i < pixels.Length; i++)
				{
					pixels[i].a = 1f;
					pixels[i].b = (pixels[i].g = (pixels[i].r = 0f));
				}
				texture2D.SetPixels(pixels);
				texture2D.Apply();
				component2.material.mainTexture = texture2D;
			}
		}
	}
}
