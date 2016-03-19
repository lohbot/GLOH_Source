using System;
using System.Collections.Generic;
using UnityEngine;

public class TsMobileQualityManager : ITsLightCollector, ITsQualityManager
{
	private enum PropertyName
	{
		Bloom,
		TextureQuality
	}

	private static class SavedProperty
	{
		private static bool mUserModified;

		private static bool mBloom;

		private static int mTexQuality;

		private static string KEY_USER_MODIFIED;

		private static string KEY_BLOOM;

		private static string KEY_TEXQUALITY;

		static SavedProperty()
		{
			TsMobileQualityManager.SavedProperty.KEY_USER_MODIFIED = "QUALITY_SET_USER_MODIFY";
			TsMobileQualityManager.SavedProperty.KEY_BLOOM = "QUALITY_SET_BLOOM";
			TsMobileQualityManager.SavedProperty.KEY_TEXQUALITY = "QUALITY_SET_TEXQUALITY";
			TsMobileQualityManager.SavedProperty.mUserModified = (PlayerPrefs.GetInt(TsMobileQualityManager.SavedProperty.KEY_USER_MODIFIED, (!TsMobileQualityManager.SavedProperty.mUserModified) ? 0 : 1) == 1);
			ITsGameQuality tsGameQuality = TsQualityManager.Instance[TsQualityManager.Instance.CurrLevel];
			TsMobileQualityManager.SavedProperty.mBloom = (PlayerPrefs.GetInt(TsMobileQualityManager.SavedProperty.KEY_BLOOM, (!tsGameQuality.Bloom) ? 0 : 1) == 1);
			TsMobileQualityManager.SavedProperty.mTexQuality = PlayerPrefs.GetInt(TsMobileQualityManager.SavedProperty.KEY_TEXQUALITY, (int)tsGameQuality.TextureQuality);
		}

		public static void Reset()
		{
			TsMobileQualityManager.SavedProperty.mUserModified = false;
			PlayerPrefs.SetInt(TsMobileQualityManager.SavedProperty.KEY_USER_MODIFIED, (!TsMobileQualityManager.SavedProperty.mUserModified) ? 0 : 1);
			ITsGameQuality tsGameQuality = TsQualityManager.Instance[TsQualityManager.Instance.CurrLevel];
			TsMobileQualityManager.SavedProperty.mBloom = tsGameQuality.Bloom;
			TsMobileQualityManager.SavedProperty.mTexQuality = (int)tsGameQuality.TextureQuality;
		}

		public static void Save(TsMobileQualityManager.PropertyName propertyName, bool value)
		{
			if (propertyName == TsMobileQualityManager.PropertyName.Bloom)
			{
				TsMobileQualityManager.SavedProperty.mBloom = value;
			}
			TsMobileQualityManager.SavedProperty.SaveProperties_();
		}

		public static void Save(TsMobileQualityManager.PropertyName propertyName, int value)
		{
			if (propertyName == TsMobileQualityManager.PropertyName.TextureQuality)
			{
				TsMobileQualityManager.SavedProperty.mTexQuality = value;
			}
			TsMobileQualityManager.SavedProperty.SaveProperties_();
		}

		private static void SaveProperties_()
		{
			TsMobileQualityManager.SavedProperty.mUserModified = true;
			PlayerPrefs.SetInt(TsMobileQualityManager.SavedProperty.KEY_USER_MODIFIED, 1);
			PlayerPrefs.SetInt(TsMobileQualityManager.SavedProperty.KEY_BLOOM, (!TsMobileQualityManager.SavedProperty.mBloom) ? 0 : 1);
			PlayerPrefs.SetInt(TsMobileQualityManager.SavedProperty.KEY_TEXQUALITY, TsMobileQualityManager.SavedProperty.mTexQuality);
		}

		public static bool Pass(TsMobileQualityManager.PropertyName propertyName, bool originalValue)
		{
			bool flag = originalValue;
			if (propertyName == TsMobileQualityManager.PropertyName.Bloom)
			{
				flag = TsMobileQualityManager.SavedProperty.mBloom;
			}
			return (!TsMobileQualityManager.SavedProperty.mUserModified) ? originalValue : flag;
		}

		public static int Pass(TsMobileQualityManager.PropertyName propertyName, int originalValue)
		{
			int num = originalValue;
			if (propertyName == TsMobileQualityManager.PropertyName.TextureQuality)
			{
				num = TsMobileQualityManager.SavedProperty.mTexQuality;
			}
			return (!TsMobileQualityManager.SavedProperty.mUserModified) ? originalValue : num;
		}
	}

	private class QualityInitializer : IDisposable
	{
		public static bool IsActive
		{
			get;
			set;
		}

		public QualityInitializer()
		{
			TsMobileQualityManager.QualityInitializer.IsActive = true;
		}

		public void Dispose()
		{
			TsMobileQualityManager.QualityInitializer.IsActive = false;
		}
	}

	private class Quality : ITsGameQuality
	{
		public TsQualityManager.GameVersion Version;

		public TsQualityManager.Level Level;

		public string Name;

		internal LightShadows m_ShadowType;

		internal int m_ShaderMaxLOD;

		internal bool m_EnableShadow;

		private bool m_Bloom;

		private TsQualityManager.TextureQuality m_TextureQuality;

		public bool DepthOfField
		{
			get;
			set;
		}

		public float TerrainPixelErrorScale
		{
			get;
			set;
		}

		public bool EnableShadow
		{
			get
			{
				return this.m_EnableShadow && this.m_ShadowType != LightShadows.None;
			}
			set
			{
				this.m_EnableShadow = value;
			}
		}

		public LightShadows ShadowType
		{
			get
			{
				return this.m_ShadowType;
			}
			set
			{
				this.m_ShadowType = value;
			}
		}

		public bool Bloom
		{
			get
			{
				return this.m_Bloom;
			}
			set
			{
				this.m_Bloom = value;
				if (!TsMobileQualityManager.QualityInitializer.IsActive)
				{
					Camera main = Camera.main;
					MonoBehaviour monoBehaviour = main.GetComponent("MobileBloom") as MonoBehaviour;
					if (monoBehaviour != null)
					{
						monoBehaviour.enabled = value;
					}
					TsMobileQualityManager.SavedProperty.Save(TsMobileQualityManager.PropertyName.Bloom, value);
				}
			}
		}

		public TsQualityManager.TextureQuality TextureQuality
		{
			get
			{
				return this.m_TextureQuality;
			}
			set
			{
				this.m_TextureQuality = value;
				if (!TsMobileQualityManager.QualityInitializer.IsActive)
				{
					QualitySettings.masterTextureLimit = (int)value;
					TsMobileQualityManager.SavedProperty.Save(TsMobileQualityManager.PropertyName.TextureQuality, (int)value);
				}
			}
		}

		public int ShaderMaxLOD
		{
			get
			{
				return this.m_ShaderMaxLOD;
			}
			set
			{
				this.m_ShaderMaxLOD = value;
			}
		}

		public bool IsSupportShadow
		{
			get
			{
				return this.ShadowType != LightShadows.None;
			}
		}

		public float CamFar
		{
			get
			{
				return TsMobileQualityManager.mCameraFar[(int)this.Level];
			}
		}

		public void CopyFrom(TsMobileQualityManager.Quality src)
		{
			bool isActive = TsMobileQualityManager.QualityInitializer.IsActive;
			TsMobileQualityManager.QualityInitializer.IsActive = true;
			this.Version = src.Version;
			this.Level = src.Level;
			this.Name = src.Name;
			this.m_ShadowType = src.m_ShadowType;
			this.m_ShaderMaxLOD = src.m_ShaderMaxLOD;
			this.m_EnableShadow = src.m_EnableShadow;
			this.DepthOfField = src.DepthOfField;
			this.TerrainPixelErrorScale = src.TerrainPixelErrorScale;
			this.m_Bloom = TsMobileQualityManager.SavedProperty.Pass(TsMobileQualityManager.PropertyName.Bloom, src.m_Bloom);
			this.m_TextureQuality = TsQualityManager.TextureQuality.FULL;
			TsMobileQualityManager.QualityInitializer.IsActive = isActive;
		}
	}

	private readonly TsLayerMask DEFAULT_RENDERABLE_LAYER = TsLayer.DEFAULT + TsLayer.IGNORE_RAYCAST + TsLayer.WATER + TsLayer.WATER_COLLISION + TsLayer.TERRAIN + TsLayer.PC + TsLayer.NPC + TsLayer.PC_OTHER + TsLayer.BULLET + TsLayer.LANDSCAPE + TsLayer.FADE_OBJECT + TsLayer.IGNORE_PICK + TsLayer.IGNORE_LIGHT;

	private static float[] mCameraFar = new float[]
	{
		650f,
		650f,
		650f,
		650f,
		650f
	};

	private TsQualityManager.GameVersion m_currVersion = TsMobileQualityManager.InitCurrVersion();

	private TsQualityManager.Level m_currLevel = TsMobileQualityManager.InitCurrLevel();

	private TsMobileQualityManager.Quality[] m_GameQualityTable = new TsMobileQualityManager.Quality[5];

	private TsMobileQualityManager.Quality m_CurrQuality = new TsMobileQualityManager.Quality();

	private Action m_actionUserSetting;

	private Func<IEnumerable<Light>> m_GetTargetLights;

	public event Action UserSetting
	{
		add
		{
			this.m_actionUserSetting = (Action)Delegate.Combine(this.m_actionUserSetting, value);
		}
		remove
		{
			this.m_actionUserSetting = (Action)Delegate.Remove(this.m_actionUserSetting, value);
		}
	}

	public TsQualityManager.Level CurrLevel
	{
		get
		{
			return this.m_currLevel;
		}
		set
		{
			bool flag = this.m_currLevel != value;
			this.m_currLevel = value;
			if (flag)
			{
				TsMobileQualityManager.SavedProperty.Reset();
				this.m_CurrQuality.CopyFrom(this.m_GameQualityTable[(int)this.m_currLevel]);
				PlayerPrefs.SetInt(NrPrefsKey.QUALITY_LEVEL, (int)this.m_currLevel);
				this.Refresh();
			}
		}
	}

	public TsQualityManager.GameVersion CurrVersion
	{
		get
		{
			return this.m_currVersion;
		}
		set
		{
			bool flag = this.m_currVersion != value;
			this.m_currVersion = value;
			if (flag)
			{
				TsMobileQualityManager.SavedProperty.Reset();
				this.m_CurrQuality.CopyFrom(this.m_GameQualityTable[(int)this.m_currVersion]);
				this.Refresh();
			}
		}
	}

	public ITsGameQuality CurrQuality
	{
		get
		{
			return this.m_CurrQuality;
		}
	}

	public ITsGameQuality this[int index]
	{
		get
		{
			return this.m_GameQualityTable[index];
		}
	}

	public ITsGameQuality this[TsQualityManager.Level level]
	{
		get
		{
			return this.m_GameQualityTable[(int)level];
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
			return this.CurrQuality.ShaderMaxLOD;
		}
	}

	public TsMobileQualityManager()
	{
		for (int i = 0; i < this.m_GameQualityTable.Length; i++)
		{
			this.m_GameQualityTable[i] = new TsMobileQualityManager.Quality();
		}
		this.InitQuality();
		this.m_CurrQuality.CopyFrom(this.m_GameQualityTable[(int)this.m_currLevel]);
	}

	private void InitQuality()
	{
		using (new TsMobileQualityManager.QualityInitializer())
		{
			TsQualityManager.Level level = TsQualityManager.Level.LOWEST;
			TsMobileQualityManager.Quality quality = this.m_GameQualityTable[(int)level];
			quality.Level = level;
			quality.Version = this.CurrVersion;
			quality.Name = "M_Fast";
			quality.Bloom = false;
			quality.DepthOfField = false;
			quality.EnableShadow = false;
			quality.TerrainPixelErrorScale = 1f;
			quality.TextureQuality = TsQualityManager.TextureQuality.FULL;
			quality.ShadowType = LightShadows.None;
			level = TsQualityManager.Level.LOW;
			quality = this.m_GameQualityTable[(int)level];
			quality.Level = level;
			quality.Version = this.CurrVersion;
			quality.Name = "M_Simple";
			quality.Bloom = false;
			quality.DepthOfField = false;
			quality.EnableShadow = false;
			quality.TerrainPixelErrorScale = 1f;
			quality.TextureQuality = TsQualityManager.TextureQuality.FULL;
			quality.ShadowType = LightShadows.None;
			level = TsQualityManager.Level.MEDIUM;
			quality = this.m_GameQualityTable[(int)level];
			quality.Level = level;
			quality.Version = this.CurrVersion;
			quality.Name = "M_Good";
			quality.Bloom = false;
			quality.DepthOfField = false;
			quality.EnableShadow = false;
			quality.TerrainPixelErrorScale = 1f;
			quality.TextureQuality = TsQualityManager.TextureQuality.FULL;
			quality.ShadowType = LightShadows.None;
			level = TsQualityManager.Level.HIGH;
			quality = this.m_GameQualityTable[(int)level];
			quality.Level = level;
			quality.Version = this.CurrVersion;
			quality.Name = "M_Beautiful";
			quality.Bloom = true;
			quality.DepthOfField = false;
			quality.EnableShadow = false;
			quality.TerrainPixelErrorScale = 1f;
			quality.TextureQuality = TsQualityManager.TextureQuality.FULL;
			quality.ShadowType = LightShadows.Hard;
			level = TsQualityManager.Level.HIGHEST;
			quality = this.m_GameQualityTable[(int)level];
			quality.Level = level;
			quality.Version = this.CurrVersion;
			quality.Name = "M_Fantastic";
			quality.Bloom = true;
			quality.DepthOfField = false;
			quality.EnableShadow = false;
			quality.TerrainPixelErrorScale = 1f;
			quality.TextureQuality = TsQualityManager.TextureQuality.FULL;
			quality.ShadowType = LightShadows.Hard;
		}
	}

	private void ChangeExtraQuality(TsQualityManager.Level level, TsQualityManager.GameVersion version, Camera camera)
	{
		ITsGameQuality currQuality = TsQualityManager.Instance.CurrQuality;
		camera.farClipPlane = TsMobileQualityManager.mCameraFar[(int)level];
		float[] array = new float[]
		{
			20.1f,
			35f,
			50f,
			65f,
			80f
		};
		float[] array2 = new float[]
		{
			100f,
			125f,
			150f,
			200f,
			250f
		};
		float[] array3 = new float[]
		{
			150f,
			200f,
			250f,
			400f,
			400f
		};
		float num = 20f;
		float num2 = 1f - num / 100f;
		if (version == TsQualityManager.GameVersion.LITE_VER)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] *= num2;
			}
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] *= num2;
			}
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k] *= num2;
			}
		}
		float[] layerCullDistances = camera.layerCullDistances;
		layerCullDistances[TsLayer.SMALL_OBJECT] = array[(int)level];
		layerCullDistances[TsLayer.MEDIUM_OBJECT] = array2[(int)level];
		layerCullDistances[TsLayer.LARGE_OBJECT] = array3[(int)level];
		camera.layerCullDistances = layerCullDistances;
		MonoBehaviour monoBehaviour = camera.GetComponent("MobileBloom") as MonoBehaviour;
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = currQuality.Bloom;
		}
		monoBehaviour = (camera.GetComponent("TsMobileVignetting") as MonoBehaviour);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = (level >= TsQualityManager.Level.HIGHEST);
		}
		int[] array4 = new int[]
		{
			200,
			300,
			300,
			400,
			400
		};
		Shader.globalMaximumLOD = array4[(int)level];
		TsLayerMask layerMask = TsLayer.NOTHING;
		switch (level)
		{
		case TsQualityManager.Level.LOWEST:
			layerMask = TsLayer.LARGE_OBJECT + TsLayer.MEDIUM_OBJECT + TsLayer.SMALL_OBJECT + TsLayer.EFFECT_LOW;
			break;
		case TsQualityManager.Level.LOW:
			layerMask = TsLayer.LARGE_OBJECT + TsLayer.MEDIUM_OBJECT + TsLayer.SMALL_OBJECT + TsLayer.EFFECT_LOW;
			break;
		case TsQualityManager.Level.MEDIUM:
			layerMask = TsLayer.LARGE_OBJECT + TsLayer.MEDIUM_OBJECT + TsLayer.SMALL_OBJECT + TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW;
			break;
		case TsQualityManager.Level.HIGH:
			layerMask = TsLayer.LARGE_OBJECT + TsLayer.MEDIUM_OBJECT + TsLayer.SMALL_OBJECT + TsLayer.EFFECT_HIGH + TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW;
			break;
		case TsQualityManager.Level.HIGHEST:
			layerMask = TsLayer.LARGE_OBJECT + TsLayer.MEDIUM_OBJECT + TsLayer.SMALL_OBJECT + TsLayer.EFFECT_HIGH + TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW;
			break;
		}
		camera.cullingMask = layerMask + this.DEFAULT_RENDERABLE_LAYER;
	}

	private static TsQualityManager.GameVersion InitCurrVersion()
	{
		return TsQualityManager.GameVersion.ORIGINAL_VER;
	}

	private static TsQualityManager.Level InitCurrLevel()
	{
		string qUALITY_LEVEL = NrPrefsKey.QUALITY_LEVEL;
		if (PlayerPrefs.HasKey(qUALITY_LEVEL))
		{
			return (TsQualityManager.Level)PlayerPrefs.GetInt(qUALITY_LEVEL, 2);
		}
		return TsQualityManager.Level.MEDIUM;
	}

	public void CollectShader(GameObject modelRoot)
	{
	}

	public void CollectShader(Shader shader)
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

	public void ChnageCollector(Func<IEnumerable<Light>> enumerableFunc)
	{
		this.m_GetTargetLights = enumerableFunc;
	}

	public void Clear()
	{
		this.m_GetTargetLights = null;
	}

	public void Refresh()
	{
		Camera main = Camera.main;
		if (main == null)
		{
			return;
		}
		TsMobileQualityManager.Quality quality = this.CurrQuality as TsMobileQualityManager.Quality;
		if (quality == null)
		{
			Debug.LogError("[TsMobileQualityManager] Current Quality is null");
			return;
		}
		int num = 0;
		int qualityLevel = QualitySettings.GetQualityLevel();
		string[] names = QualitySettings.names;
		for (int i = 0; i < names.Length; i++)
		{
			string a = names[i];
			if (a == quality.Name)
			{
				break;
			}
			num++;
		}
		if (num != qualityLevel)
		{
			QualitySettings.SetQualityLevel(num);
		}
		int num2 = 0;
		if (QualitySettings.masterTextureLimit != num2)
		{
			QualitySettings.masterTextureLimit = num2;
		}
		if (this.m_GetTargetLights != null)
		{
			foreach (Light current in this.m_GetTargetLights())
			{
				current.shadows = ((!quality.m_EnableShadow) ? LightShadows.None : quality.ShadowType);
			}
		}
		this.ChangeExtraQuality(this.m_currLevel, this.m_currVersion, main);
		if (this.m_actionUserSetting != null)
		{
			this.m_actionUserSetting();
		}
	}
}
