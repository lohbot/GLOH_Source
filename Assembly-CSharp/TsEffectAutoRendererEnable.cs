using System;
using UnityEngine;

[AddComponentMenu("Effect/Ndoors/Auto RendererEnable"), RequireComponent(typeof(Renderer))]
public class TsEffectAutoRendererEnable : MonoBehaviour
{
	public enum ECheckType
	{
		COLOR_RGB,
		COLOR_RGBA,
		COLOR_R,
		COLOR_G,
		COLOR_B,
		COLOR_A
	}

	private static string s_defaultName = "_TintColor";

	[SerializeField]
	private string _targetName = TsEffectAutoRendererEnable.s_defaultName;

	[SerializeField]
	private TsEffectAutoRendererEnable.ECheckType _checkType;

	public string TargetName
	{
		get
		{
			return this._targetName;
		}
		set
		{
			this._targetName = value;
		}
	}

	public TsEffectAutoRendererEnable.ECheckType CheckType
	{
		get
		{
			return this._checkType;
		}
		set
		{
			this._checkType = value;
		}
	}

	public void Start()
	{
		if (base.renderer == null || base.renderer.material == null)
		{
			Debug.LogWarning("TsEffectAutoRenderEnable is disabled because this object has not renderer or material => " + base.gameObject.name);
			base.enabled = false;
		}
		else
		{
			base.renderer.enabled = false;
		}
	}

	public void Update()
	{
		if (base.renderer.material.HasProperty(this._targetName))
		{
			Color color = base.renderer.material.GetColor(this._targetName);
			bool enabled = false;
			switch (this._checkType)
			{
			case TsEffectAutoRendererEnable.ECheckType.COLOR_RGB:
				if (color.r + color.g + color.b > 0f)
				{
					enabled = true;
				}
				break;
			case TsEffectAutoRendererEnable.ECheckType.COLOR_RGBA:
				if (color.r + color.g + color.b + color.a > 0f)
				{
					enabled = true;
				}
				break;
			case TsEffectAutoRendererEnable.ECheckType.COLOR_R:
				if (color.r > 0f)
				{
					enabled = true;
				}
				break;
			case TsEffectAutoRendererEnable.ECheckType.COLOR_G:
				if (color.g > 0f)
				{
					enabled = true;
				}
				break;
			case TsEffectAutoRendererEnable.ECheckType.COLOR_B:
				if (color.b > 0f)
				{
					enabled = true;
				}
				break;
			case TsEffectAutoRendererEnable.ECheckType.COLOR_A:
				if (color.a > 0f)
				{
					enabled = true;
				}
				break;
			}
			base.renderer.enabled = enabled;
			return;
		}
		Debug.LogWarning(string.Concat(new string[]
		{
			"TsEffectAutoRenderEnable is required shader using _TintColor : material=\"",
			base.renderer.material.name,
			"\", shader=\"",
			base.renderer.material.shader.name,
			"\", object=\"",
			base.gameObject.name,
			"\""
		}));
	}
}
