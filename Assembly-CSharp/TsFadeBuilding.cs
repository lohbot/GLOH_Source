using System;
using UnityEngine;

[AddComponentMenu("TsFadeBuilding/TsFadeBuilding")]
public class TsFadeBuilding : MonoBehaviour
{
	[SerializeField]
	public float IncreaseAlphaSpeed = 1f;

	private float _LimitAlpha = 0.4f;

	private bool _fadeOut;

	private bool _fadeIn;

	private Material _originMaterial;

	private Material _alphaMaterial;

	public bool FadeOut
	{
		get
		{
			return this._fadeOut;
		}
		set
		{
			this._fadeOut = value;
		}
	}

	public bool FadeIn
	{
		get
		{
			return this._fadeIn;
		}
		set
		{
			this._fadeIn = value;
		}
	}

	private void Start()
	{
		if (base.renderer != null)
		{
			this._originMaterial = base.renderer.material;
			this._alphaMaterial = new Material(this._originMaterial);
			if (TsPlatform.IsAndroid)
			{
				this._alphaMaterial.shader = Shader.Find("Hidden/AT2_Fade_Object_mobile");
			}
			else
			{
				this._alphaMaterial.shader = Shader.Find("Hidden/Diffuse_FadeOut");
			}
		}
		else
		{
			Debug.Log(" Not Found Renderer! Can't Progress Building Fade out");
			base.enabled = false;
		}
	}

	private void OnWillRenderObject()
	{
		if (this._fadeOut)
		{
			base.renderer.material = this._alphaMaterial;
			if (base.renderer.material.HasProperty("_Alpha"))
			{
				float num = base.renderer.material.GetFloat("_Alpha");
				float num2 = this.IncreaseAlphaSpeed * Time.deltaTime;
				num -= num2;
				if (num > this._LimitAlpha)
				{
					base.renderer.material.SetFloat("_Alpha", num);
				}
			}
		}
		if (this._fadeIn && base.renderer.material.HasProperty("_Alpha"))
		{
			float num3 = base.renderer.material.GetFloat("_Alpha");
			float num4 = this.IncreaseAlphaSpeed * Time.deltaTime;
			num3 += num4;
			if (num3 < 1f)
			{
				base.renderer.material.SetFloat("_Alpha", num3);
			}
			else
			{
				num3 = 1f;
				base.renderer.material.SetFloat("_Alpha", num3);
				base.renderer.material = this._originMaterial;
			}
		}
	}
}
