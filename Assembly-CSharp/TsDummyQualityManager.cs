using System;
using UnityEngine;

internal class TsDummyQualityManager : ITsQualityManager
{
	private class DummyGameQuality : ITsGameQuality
	{
		public bool DepthOfField
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool Bloom
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float TerrainPixelErrorScale
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public TsQualityManager.TextureQuality TextureQuality
		{
			get
			{
				return TsQualityManager.TextureQuality.FULL;
			}
			set
			{
			}
		}

		public bool EnableShadow
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public int ShaderMaxLOD
		{
			get
			{
				return -1;
			}
		}

		public bool IsSupportShadow
		{
			get
			{
				return false;
			}
		}

		public LightShadows ShadowType
		{
			get
			{
				return LightShadows.None;
			}
		}

		public float CamFar
		{
			get
			{
				return (!(Camera.main == null)) ? Camera.main.farClipPlane : 0f;
			}
		}
	}

	private static TsDummyQualityManager.DummyGameQuality ms_DummyQuality = new TsDummyQualityManager.DummyGameQuality();

	public event Action UserSetting
	{
		add
		{
		}
		remove
		{
		}
	}

	public TsQualityManager.Level CurrLevel
	{
		get
		{
			return TsQualityManager.Level.MEDIUM;
		}
		set
		{
		}
	}

	public ITsGameQuality CurrQuality
	{
		get
		{
			return TsDummyQualityManager.ms_DummyQuality;
		}
	}

	public ITsGameQuality this[int index]
	{
		get
		{
			return TsDummyQualityManager.ms_DummyQuality;
		}
	}

	public ITsGameQuality this[TsQualityManager.Level level]
	{
		get
		{
			return TsDummyQualityManager.ms_DummyQuality;
		}
	}

	public bool IsCustomMode
	{
		get
		{
			return false;
		}
	}

	public int ShaderMaxLOD
	{
		get
		{
			return -1;
		}
	}

	public void CollectShader(GameObject modelRoot)
	{
	}

	public void CollectShader(Shader shader)
	{
	}

	public void Refresh()
	{
	}

	public bool SaveCustomSettings()
	{
		return false;
	}

	public void RecoveryCustomSettings()
	{
	}

	public bool RegisterScript(MonoBehaviour mono, TsQualityManager.Level level)
	{
		return false;
	}

	public void ReleaseScript(MonoBehaviour mono)
	{
	}

	public void AutoEnableScript(MonoBehaviour mono)
	{
	}
}
