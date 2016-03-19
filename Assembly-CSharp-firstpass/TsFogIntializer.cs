using System;
using UnityEngine;

public class TsFogIntializer : MonoBehaviour
{
	public Color fogColor = Color.gray;

	public float fogStart = 50f;

	public float fogEnd = 1000f;

	private void OnEnable()
	{
		RenderSettings.fogColor = this.fogColor;
		RenderSettings.fogStartDistance = this.fogStart;
		RenderSettings.fogEndDistance = this.fogEnd;
	}
}
