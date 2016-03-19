using System;
using UnityEngine;

public class FX_MS_Blink : MonoBehaviour
{
	[SerializeField]
	public float BlinkSpeed = 1f;

	[SerializeField]
	public Color TargetColor;

	private Color originColor;

	private void Start()
	{
		this.originColor = base.renderer.material.GetColor("_TintColor");
	}

	private void Update()
	{
		float t = Mathf.PingPong(Time.time, this.BlinkSpeed) / this.BlinkSpeed;
		base.renderer.material.SetColor("_TintColor", Color.Lerp(this.originColor, this.TargetColor, t));
	}
}
