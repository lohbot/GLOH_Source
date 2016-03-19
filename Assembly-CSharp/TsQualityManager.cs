using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[AddComponentMenu("TsScripts/Singletons/QualitySettings")]
public class TsQualityManager : MonoBehaviour, ITsQualityManager, ITsSelfDestroyer, ITsQualityManagerQuery, ITsDebugQualityManager
{
	[Serializable]
	public class GameSettings
	{
		public TsQualityManager.Quality[] QualityTable = new TsQualityManager.Quality[5];

		private TsQualityManager.Quality m_MemoriedQuality = new TsQualityManager.Quality();

		private TsQualityManager.Level m_MemoriedLevel = TsQualityManager.Level._length;

		private TsQualityManager m_Owner;

		public GameSettings() : this(null)
		{
		}

		public GameSettings(TsQualityManager owner)
		{
			for (int i = 0; i < this.QualityTable.Length; i++)
			{
				this.QualityTable[i] = new TsQualityManager.Quality((TsQualityManager.Level)i);
			}
			this.Init(owner);
		}

		internal void Memory(TsQualityManager.Level level)
		{
			if (level == TsQualityManager.Level._length)
			{
				TsLog.LogError("[TsQualityManager] GameSettings.Recovery({0}) => failed, (memLevel={1})", new object[]
				{
					level,
					this.m_MemoriedLevel
				});
				return;
			}
			if (level != this.m_MemoriedLevel)
			{
				this.m_MemoriedQuality.CopyFrom(this.QualityTable[(int)level]);
			}
			this.m_MemoriedLevel = level;
		}

		internal void Recovery(TsQualityManager.Level level)
		{
			if (level == TsQualityManager.Level._length)
			{
				TsLog.LogError("[TsQualityManager] GameSettings.Recovery({0}) => failed, (memLevel={1})", new object[]
				{
					level,
					this.m_MemoriedLevel
				});
				return;
			}
			if (this.m_MemoriedLevel == level)
			{
				this.QualityTable[(int)level].CopyFrom(this.m_MemoriedQuality);
			}
			this.m_MemoriedLevel = TsQualityManager.Level._length;
		}

		internal void Init(TsQualityManager owner)
		{
			this.m_Owner = owner;
			this.m_MemoriedLevel = TsQualityManager.Level._length;
			TsQualityManager.Quality[] qualityTable = this.QualityTable;
			for (int i = 0; i < qualityTable.Length; i++)
			{
				TsQualityManager.Quality quality = qualityTable[i];
				quality.Init();
			}
			TsQualityManager.Quality quality2 = this.QualityTable[0];
			quality2.renderableLayers = TsLayer.EFFECT_LOW + TsLayer.PC_DECORATION;
			quality2.qualityLevel = "Fast";
			quality2.shaderMaxLOD = 200;
			quality2.shadowType = LightShadows.None;
			quality2.textureQuality = TsQualityManager.TextureQuality.QUARTER;
			quality2.ActiveShadow = false;
			if (TsPlatform.IsMobile)
			{
				quality2.renderableLayers = TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW;
				quality2.shaderMaxLOD = 400;
			}
			TsQualityManager.Quality quality3 = this.QualityTable[1];
			quality3.renderableLayers = TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW + TsLayer.PC_DECORATION;
			quality3.qualityLevel = "Simple";
			quality3.shaderMaxLOD = 200;
			quality3.shadowType = LightShadows.None;
			quality3.textureQuality = TsQualityManager.TextureQuality.HALF;
			quality3.ActiveShadow = true;
			if (TsPlatform.IsMobile)
			{
				quality3.renderableLayers = TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW;
				quality3.shaderMaxLOD = 400;
			}
			TsQualityManager.Quality quality4 = this.QualityTable[2];
			quality4.renderableLayers = TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW + TsLayer.SMALL_OBJECT + TsLayer.PC_DECORATION;
			quality4.shaderMaxLOD = 300;
			quality4.qualityLevel = "Good";
			quality4.anisotropicFiltering = AnisotropicFiltering.Enable;
			quality4.colorCorrection = true;
			quality4.sunShafts = true;
			quality4.bloomAndFlares = true;
			quality4.shadowType = LightShadows.None;
			quality4.textureQuality = TsQualityManager.TextureQuality.FULL;
			quality4.ActiveShadow = true;
			if (TsPlatform.IsMobile)
			{
				quality4.shaderMaxLOD = 400;
			}
			TsQualityManager.Quality quality5 = this.QualityTable[3];
			quality5.renderableLayers = TsLayer.EFFECT_HIGH + TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW + TsLayer.DETAIL + TsLayer.SMALL_OBJECT + TsLayer.PC_DECORATION;
			quality5.qualityLevel = "Beautiful";
			quality5.shaderMaxLOD = 1000;
			quality5.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
			quality5.ambientParticles = true;
			quality5.colorCorrection = true;
			quality5.bloomAndFlares = true;
			quality5.sunShafts = false;
			quality5.depthOfField = false;
			quality5.SSAO = false;
			quality5.shadowType = LightShadows.Hard;
			quality5.textureQuality = TsQualityManager.TextureQuality.FULL;
			quality5.ActiveShadow = true;
			TsQualityManager.Quality quality6 = this.QualityTable[4];
			quality6.renderableLayers = TsLayer.EFFECT_HIGH + TsLayer.EFFECT_MIDDLE + TsLayer.EFFECT_LOW + TsLayer.DETAIL + TsLayer.SMALL_OBJECT + TsLayer.PC_DECORATION;
			quality6.qualityLevel = "Fantastic";
			quality6.shaderMaxLOD = 1000;
			quality6.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
			quality6.ambientParticles = true;
			quality6.colorCorrection = true;
			quality6.bloomAndFlares = true;
			quality6.sunShafts = true;
			quality6.depthOfField = true;
			quality6.SSAO = true;
			quality6.shadowType = LightShadows.Soft;
			quality6.textureQuality = TsQualityManager.TextureQuality.FULL;
			quality6.ActiveShadow = true;
			this._InitTerrainSettings();
		}

		private void _InitTerrainSettings()
		{
			TsQualityManager.Quality quality = this.QualityTable[0];
			quality.terrain.pixelError = 200f;
			quality.terrain.castShadow = false;
			quality.terrain.renderLevel = TsQualityManager.TerrainRender.ONLY_TERRAIN;
			quality.terrain.pixelErrorScale = 1f;
			quality.terrain.grassDistance = 0f;
			quality.terrain.treeBillboardDistance = 30f;
			TsQualityManager.Quality quality2 = this.QualityTable[1];
			quality2.terrain.pixelError = 100f;
			quality2.terrain.castShadow = false;
			quality2.terrain.renderLevel = TsQualityManager.TerrainRender.ONLY_TERRAIN;
			quality2.terrain.pixelErrorScale = 1f;
			quality2.terrain.grassDistance = 10f;
			quality2.terrain.treeBillboardDistance = 30f;
			TsQualityManager.Quality quality3 = this.QualityTable[2];
			quality3.terrain.pixelError = 10f;
			quality3.terrain.castShadow = false;
			quality3.terrain.renderLevel = TsQualityManager.TerrainRender.TREE;
			quality3.terrain.pixelErrorScale = 1f;
			quality3.terrain.grassDistance = 30f;
			quality3.terrain.treeBillboardDistance = 40f;
			TsQualityManager.Quality quality4 = this.QualityTable[3];
			quality4.terrain.pixelError = 5f;
			quality4.terrain.castShadow = true;
			quality4.terrain.renderLevel = TsQualityManager.TerrainRender.ALL;
			quality4.terrain.pixelErrorScale = 1f;
			quality4.terrain.grassDistance = 50f;
			quality4.terrain.treeBillboardDistance = 80f;
			TsQualityManager.Quality quality5 = this.QualityTable[4];
			quality5.terrain.pixelError = 1f;
			quality5.terrain.castShadow = true;
			quality5.terrain.renderLevel = TsQualityManager.TerrainRender.ALL;
			quality5.terrain.pixelErrorScale = 1f;
			quality5.terrain.grassDistance = 50f;
			quality5.terrain.treeBillboardDistance = 80f;
		}

		public bool SaveCustomSettings()
		{
			if (!TsQualityManager._Immortality.CustomMode)
			{
				return false;
			}
			if (TsQualityManager._SupportDetailSettings())
			{
				try
				{
					this._WriteSetting();
					bool result = true;
					return result;
				}
				catch (PlayerPrefsException arg)
				{
					TsLog.LogWarning("[TsQualityManager] SaveCustomSettings() => " + arg, new object[0]);
					bool result = false;
					return result;
				}
				return true;
			}
			return true;
		}

		public bool LoadCustomSettings()
		{
			if (!TsQualityManager._Immortality.CustomMode)
			{
				return false;
			}
			if (TsQualityManager._SupportDetailSettings())
			{
				try
				{
					this._ReadSetting();
					bool result = true;
					return result;
				}
				catch (PlayerPrefsException arg)
				{
					TsLog.LogWarning("[TsQualityManager] LoadCustomSettings() => " + arg, new object[0]);
					bool result = false;
					return result;
				}
				return true;
			}
			return true;
		}

		private void _ReadSetting()
		{
			TsQualityManager.Quality quality = this.m_Owner.CurrQuality as TsQualityManager.Quality;
			string text = NrPrefsKey.QUALITY_SHADOW;
			if (PlayerPrefs.HasKey(text))
			{
				int @int = PlayerPrefs.GetInt(text, (!quality.ActiveShadow) ? 0 : 1);
				quality.ActiveShadow = (@int == 1);
			}
			else
			{
				TsLog.LogWarning("[TsQualityManager] key({0}) => not found", new object[]
				{
					text
				});
			}
			text = NrPrefsKey.QUALITY_TEXTURE;
			if (PlayerPrefs.HasKey(text))
			{
				int @int = PlayerPrefs.GetInt(text, (int)quality.textureQuality);
				quality.textureQuality = (TsQualityManager.TextureQuality)@int;
			}
			else
			{
				TsLog.LogWarning("[TsQualityManager] key({0}) => not found", new object[]
				{
					text
				});
			}
			text = NrPrefsKey.QUALITY_TERRAIN_PIXEL_ERROR_SCALE;
			if (PlayerPrefs.HasKey(text))
			{
				quality.terrain.pixelErrorScale = PlayerPrefs.GetFloat(text, quality.terrain.pixelErrorScale);
			}
			else
			{
				TsLog.LogWarning("[TsQualityManager] key({0}) => not found", new object[]
				{
					text
				});
			}
			text = NrPrefsKey.QUALITY_BLOOM;
			if (PlayerPrefs.HasKey(text))
			{
				int @int = PlayerPrefs.GetInt(text, (!quality.bloomAndFlares) ? 0 : 1);
				quality.bloomAndFlares = (@int == 1);
			}
			else
			{
				TsLog.LogWarning("[TsQualityManager] key({0}) => not found", new object[]
				{
					text
				});
			}
			text = NrPrefsKey.QUALITY_DOF;
			if (PlayerPrefs.HasKey(text))
			{
				int @int = PlayerPrefs.GetInt(text, (!quality.depthOfField) ? 0 : 1);
				quality.depthOfField = (@int == 1);
			}
			else
			{
				TsLog.LogWarning("[TsQualityManager] key({0}) => not found", new object[]
				{
					text
				});
			}
		}

		private void _WriteSetting()
		{
			TsQualityManager.Quality quality = this.m_Owner.CurrQuality as TsQualityManager.Quality;
			string key = NrPrefsKey.QUALITY_SHADOW;
			PlayerPrefs.SetInt(key, (!quality.ActiveShadow) ? 0 : 1);
			key = NrPrefsKey.QUALITY_TEXTURE;
			PlayerPrefs.SetInt(key, (int)quality.textureQuality);
			key = NrPrefsKey.QUALITY_TERRAIN_PIXEL_ERROR_SCALE;
			PlayerPrefs.SetFloat(key, quality.terrain.pixelErrorScale);
			key = NrPrefsKey.QUALITY_BLOOM;
			PlayerPrefs.SetInt(key, (!quality.bloomAndFlares) ? 0 : 1);
			key = NrPrefsKey.QUALITY_DOF;
			PlayerPrefs.SetInt(key, (!quality.depthOfField) ? 0 : 1);
		}
	}

	private static class _Immortality
	{
		public static TsLayerMask DefaultRenderLayer;

		public static TsQualityManager.Level CurrLevel;

		public static Dictionary<string, Shader> Shaders;

		public static Dictionary<MonoBehaviour, TsQualityManager.Level> ScriptCollection;

		public static bool CustomMode;

		static _Immortality()
		{
			TsQualityManager._Immortality.CurrLevel = TsQualityManager.DEFAULT_LEVEL;
			TsQualityManager._Immortality.Shaders = new Dictionary<string, Shader>();
			TsQualityManager._Immortality.ScriptCollection = new Dictionary<MonoBehaviour, TsQualityManager.Level>();
			TsQualityManager._Immortality.CustomMode = false;
			TsQualityManager._Immortality.DefaultRenderLayer = TsLayer.DEFAULT + TsLayer.IGNORE_RAYCAST + TsLayer.WATER + TsLayer.WATER_COLLISION + TsLayer.TERRAIN + TsLayer.PC + TsLayer.NPC + TsLayer.PC_OTHER + TsLayer.BULLET + TsLayer.MEDIUM_OBJECT + TsLayer.LANDSCAPE + TsLayer.FADE_OBJECT + TsLayer.IGNORE_PICK + TsLayer.IGNORE_LIGHT;
			TsQualityManager._Immortality.LoadCustomMode();
			TsQualityManager._Immortality.ShaderDatabase();
		}

		private static void ShaderDatabase()
		{
			string[] array = new string[]
			{
				"TK/TKBG_Ice",
				"TKEffect/TKEffect_TransperedObject",
				"TKEffect/TKEffect_StandardRim",
				"TKEffect/TKEffect_FlowStandard",
				"TKEffect/TKEffect_FlowMulti",
				"TKEffect/TKEffect_FlowAdd",
				"TKEffect/DistortionTexture_Multi",
				"TKEffect/DistortionTexture_Add",
				"TKEffect/Distortion",
				"TK/TK_Char_Diffuse_rim_dye"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				TsQualityManager._Immortality.Shaders.Add(text, Shader.Find(text));
			}
		}

		internal static void LoadCurrLevel()
		{
			string qUALITY_LEVEL = NrPrefsKey.QUALITY_LEVEL;
			if (PlayerPrefs.HasKey(qUALITY_LEVEL))
			{
				int @int = PlayerPrefs.GetInt(qUALITY_LEVEL, (int)TsQualityManager._Immortality.CurrLevel);
				TsQualityManager._Immortality.CurrLevel = (TsQualityManager.Level)@int;
			}
			else if (TsQualityManager._SupportLevelAutoDetection())
			{
				TsQualityManager._Immortality.CurrLevel = NrHardwareIndex.GetOptimizedQualityLevel();
			}
		}

		internal static void SaveCurrLevel()
		{
			if (TsQualityManager._Immortality.CurrLevel != TsQualityManager.Level._length)
			{
				string qUALITY_LEVEL = NrPrefsKey.QUALITY_LEVEL;
				PlayerPrefs.SetInt(qUALITY_LEVEL, (int)TsQualityManager._Immortality.CurrLevel);
			}
			else
			{
				TsLog.LogWarning("TsQualityManager> save => failed : invalid quality level", new object[0]);
			}
		}

		internal static void LoadCustomMode()
		{
			TsQualityManager._Immortality.CustomMode = (PlayerPrefs.GetInt(NrPrefsKey.QUALITY_MODE, (!TsQualityManager._Immortality.CustomMode) ? 0 : 1) == 1);
		}

		internal static void ActiveCustomMode()
		{
			if (!TsQualityManager._Immortality.CustomMode)
			{
				PlayerPrefs.SetInt(NrPrefsKey.QUALITY_MODE, 1);
				TsQualityManager._MemoryOriginalSetting();
			}
			TsQualityManager._Immortality.CustomMode = true;
		}

		internal static void InactiveCustomMode()
		{
			if (TsQualityManager._Immortality.CustomMode)
			{
				PlayerPrefs.SetInt(NrPrefsKey.QUALITY_MODE, 0);
				TsQualityManager._RecoveryOriginalSetting();
			}
			TsQualityManager._Immortality.CustomMode = false;
		}
	}

	[Serializable]
	public class Quality : TsQualityManager.IEditorQuery, ITsGameQuality
	{
		[XmlAttribute("Level")]
		public TsQualityManager.Level m_Level = TsQualityManager.Level._length;

		public string qualityLevel;

		public int shaderMaxLOD;

		public AnisotropicFiltering anisotropicFiltering;

		public TsQualityManager.TextureQuality textureQuality;

		public bool ambientParticles;

		public bool colorCorrection;

		public bool bloomAndFlares;

		public bool sunShafts;

		public bool depthOfField;

		public bool SSAO;

		public LightShadows shadowType;

		public bool useSkybox;

		[NonSerialized]
		public TsLayerMask renderableLayers;

		private bool m_activeShadow = true;

		public TsQualityManager.TerrainSetting terrain = new TsQualityManager.TerrainSetting();

		bool TsQualityManager.IEditorQuery.IsActiveShadow
		{
			get
			{
				return TsQualityManager.IsSupportShadow(this.m_Level);
			}
		}

		int ITsGameQuality.ShaderMaxLOD
		{
			get
			{
				return this.shaderMaxLOD;
			}
		}

		LightShadows ITsGameQuality.ShadowType
		{
			get
			{
				return this.shadowType;
			}
		}

		bool ITsGameQuality.DepthOfField
		{
			get
			{
				return this.depthOfField;
			}
			set
			{
				if (this.depthOfField != value)
				{
					TsQualityManager._Immortality.ActiveCustomMode();
					this.depthOfField = value;
				}
			}
		}

		bool ITsGameQuality.Bloom
		{
			get
			{
				return this.bloomAndFlares;
			}
			set
			{
				if (this.bloomAndFlares != value)
				{
					TsQualityManager._Immortality.ActiveCustomMode();
					this.bloomAndFlares = value;
				}
			}
		}

		float ITsGameQuality.TerrainPixelErrorScale
		{
			get
			{
				return this.terrain.pixelErrorScale;
			}
			set
			{
				if (!Mathf.Approximately(this.terrain.pixelErrorScale, value))
				{
					TsQualityManager._Immortality.ActiveCustomMode();
					this.terrain.pixelErrorScale = value;
				}
			}
		}

		TsQualityManager.TextureQuality ITsGameQuality.TextureQuality
		{
			get
			{
				return this.textureQuality;
			}
			set
			{
				if (this.textureQuality != value)
				{
					TsQualityManager._Immortality.ActiveCustomMode();
					this.textureQuality = value;
				}
			}
		}

		bool ITsGameQuality.EnableShadow
		{
			get
			{
				return this.ActiveShadow && this.shadowType != LightShadows.None;
			}
			set
			{
				if ((this.ActiveShadow && this.shadowType != LightShadows.None) != value)
				{
					TsQualityManager._Immortality.ActiveCustomMode();
					this.ActiveShadow = value;
				}
			}
		}

		float ITsGameQuality.CamFar
		{
			get
			{
				return TsQualityManager.CAMERA_FAR[(int)this.m_Level];
			}
		}

		internal bool ActiveShadow
		{
			get
			{
				return this.m_activeShadow;
			}
			set
			{
				this.m_activeShadow = value;
			}
		}

		public bool IsSupportShadow
		{
			get
			{
				return this.shadowType != LightShadows.None;
			}
		}

		public Quality()
		{
		}

		public Quality(TsQualityManager.Level level)
		{
			this.m_Level = level;
			this.Init();
		}

		public void CopyFrom(TsQualityManager.Quality src)
		{
			this.qualityLevel = src.qualityLevel;
			this.shaderMaxLOD = src.shaderMaxLOD;
			this.anisotropicFiltering = src.anisotropicFiltering;
			this.ambientParticles = src.ambientParticles;
			this.colorCorrection = src.colorCorrection;
			this.bloomAndFlares = src.bloomAndFlares;
			this.sunShafts = src.sunShafts;
			this.depthOfField = src.depthOfField;
			this.SSAO = src.SSAO;
			this.textureQuality = src.textureQuality;
			this.useSkybox = src.useSkybox;
			this.shadowType = src.shadowType;
			this.m_activeShadow = src.m_activeShadow;
			this.terrain.CopyFrom(src.terrain);
		}

		public void Init()
		{
			this.qualityLevel = "Good";
			this.shaderMaxLOD = 2147483647;
			this.anisotropicFiltering = AnisotropicFiltering.Disable;
			this.ambientParticles = false;
			this.colorCorrection = false;
			this.bloomAndFlares = false;
			this.sunShafts = false;
			this.depthOfField = false;
			this.SSAO = false;
			this.textureQuality = TsQualityManager.TextureQuality.FULL;
			this.useSkybox = true;
			this.shadowType = LightShadows.None;
			this.ActiveShadow = true;
			this.terrain.Init();
		}
	}

	[Serializable]
	public class TerrainSetting
	{
		private const float MIN_PIXEL_ERR_SCALE = 0.1f;

		private const float MAX_PIXEL_ERR_SCALE = 1.5f;

		public TsQualityManager.TerrainRender renderLevel;

		public bool castShadow;

		public float pixelError;

		public float baseMapDistance;

		public float grassDistance;

		public float grassDsensity;

		public float treeDisntance;

		public float treeBillboardDistance;

		public float treeFadeLength;

		public int treeMaxLODCount;

		private float m_pixelErrorScale = 1f;

		public float pixelErrorScale
		{
			get
			{
				return this.m_pixelErrorScale;
			}
			set
			{
				this.m_pixelErrorScale = value;
			}
		}

		public TerrainSetting()
		{
			this.Init();
		}

		public void Init()
		{
			this.renderLevel = TsQualityManager.TerrainRender.ALL;
			this.castShadow = false;
			this.pixelError = 5f;
			this.baseMapDistance = 1000f;
			this.grassDistance = 80f;
			this.grassDsensity = 1f;
			this.treeDisntance = 2000f;
			this.treeBillboardDistance = 50f;
			this.treeFadeLength = 5f;
			this.treeMaxLODCount = 50;
			this.pixelErrorScale = 1f;
		}

		public void CopyFrom(TsQualityManager.TerrainSetting src)
		{
			this.renderLevel = src.renderLevel;
			this.castShadow = src.castShadow;
			this.pixelError = src.pixelError;
			this.baseMapDistance = src.baseMapDistance;
			this.grassDistance = src.grassDistance;
			this.grassDsensity = src.grassDsensity;
			this.treeDisntance = src.treeDisntance;
			this.treeBillboardDistance = src.treeBillboardDistance;
			this.treeFadeLength = src.treeFadeLength;
			this.treeMaxLODCount = src.treeMaxLODCount;
			this.pixelErrorScale = src.pixelErrorScale;
		}

		public void CopyFrom(Terrain terrain)
		{
			if (terrain == null)
			{
				return;
			}
			this.renderLevel = (TsQualityManager.TerrainRender)terrain.editorRenderFlags;
			this.castShadow = terrain.castShadows;
			this.pixelError = terrain.heightmapPixelError;
			this.baseMapDistance = terrain.basemapDistance;
			this.grassDistance = terrain.detailObjectDistance;
			this.grassDsensity = terrain.detailObjectDensity;
			this.treeDisntance = terrain.treeDistance;
			this.treeBillboardDistance = terrain.treeBillboardDistance;
			this.treeFadeLength = terrain.treeCrossFadeLength;
			this.treeMaxLODCount = terrain.treeMaximumFullLODCount;
		}

		public void ApplyTo(Terrain terrain)
		{
			if (terrain == null)
			{
				return;
			}
			terrain.editorRenderFlags = (TerrainRenderFlags)this.renderLevel;
			terrain.castShadows = false;
			terrain.heightmapPixelError = this.pixelError * this.m_pixelErrorScale;
			if (!TsQualityManager._IgnoreTerrainBasemap())
			{
				terrain.basemapDistance = this.baseMapDistance;
			}
			if (TsQualityManager._IgnoreTreeBilboard())
			{
				terrain.treeCrossFadeLength = 200f;
				terrain.treeMaximumFullLODCount = 500;
				terrain.treeBillboardDistance = 2000f;
			}
			else
			{
				terrain.detailObjectDistance = this.grassDistance;
				terrain.detailObjectDensity = this.grassDsensity;
				terrain.treeDistance = this.treeDisntance;
				terrain.treeBillboardDistance = this.treeBillboardDistance;
				terrain.treeCrossFadeLength = this.treeFadeLength;
				terrain.treeMaximumFullLODCount = this.treeMaxLODCount;
			}
			terrain.Flush();
		}
	}

	[Serializable]
	public enum Level
	{
		LOWEST,
		LOW,
		MEDIUM,
		HIGH,
		HIGHEST,
		_length
	}

	[Serializable]
	public enum GameVersion
	{
		ORIGINAL_VER,
		LITE_VER,
		_length
	}

	[Serializable]
	public enum TerrainRender
	{
		ONLY_TERRAIN = 1,
		TREE = 3,
		GRASS = 5,
		ALL = 7
	}

	[Serializable]
	public enum TextureQuality
	{
		FULL,
		HALF,
		QUARTER,
		EIGHTH = 4
	}

	public interface IEditorQuery
	{
		bool IsActiveShadow
		{
			get;
		}
	}

	private delegate void QualityAction(TsQualityManager.Quality quality);

	private const string DEFAULT_XML = "XML/TsQualitySettings";

	private static float[] CAMERA_FAR = new float[]
	{
		150f,
		200f,
		250f,
		400f,
		600f
	};

	private static TsQualityManager.Level DEFAULT_LEVEL = TsQualityManager.Level.MEDIUM;

	private static TsQualityManager.GameVersion DEFAULT_GAMEVERSION;

	private static ITsQualityManager ms_Instance;

	public Terrain m_TargetTerrain;

	public Camera m_TargetCamera;

	[SerializeField]
	private TsQualityManager.GameSettings m_GameSettings = new TsQualityManager.GameSettings(null);

	[SerializeField]
	private bool m_AutoLoading;

	private List<Light> m_TargetLights = new List<Light>();

	private TsQualityManager.TerrainSetting m_TerrainOriginalSetting = new TsQualityManager.TerrainSetting();

	private int m_ChagingShaderMaxLOD = 2147483647;

	private bool m_FirstRefreshed;

	private static Action UserInitalizer;

	private Action _RefreshAction;

	private TsQualityManager.QualityAction _ApplyCameraAction;

	private bool m_EnableCollectShader;

	public event Action UserSetting
	{
		add
		{
			TsQualityManager.UserInitalizer = (Action)Delegate.Combine(TsQualityManager.UserInitalizer, value);
		}
		remove
		{
			TsQualityManager.UserInitalizer = (Action)Delegate.Remove(TsQualityManager.UserInitalizer, value);
		}
	}

	bool ITsQualityManagerQuery.IsFixedBasemap
	{
		get
		{
			return !TsQualityManager._IgnoreTerrainBasemap();
		}
	}

	bool ITsQualityManagerQuery.IsFixedTreeBillboard
	{
		get
		{
			return !TsQualityManager._IgnoreTreeBilboard();
		}
	}

	public static ITsQualityManager Instance
	{
		get
		{
			if (TsQualityManager.ms_Instance == null)
			{
				if (TsPlatform.IsMobile)
				{
					TsQualityManager.ms_Instance = new TsMobileQualityManager();
				}
				else
				{
					TsQualityManager[] array = UnityEngine.Object.FindObjectsOfType(typeof(TsQualityManager)) as TsQualityManager[];
					TsQualityManager[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						TsQualityManager tsQualityManager = array2[i];
						if (tsQualityManager.gameObject.activeInHierarchy)
						{
							TsQualityManager.ms_Instance = tsQualityManager;
							break;
						}
					}
					if (TsQualityManager.ms_Instance == null)
					{
						GameObject gameObject = GameObject.Find("@TsQualityManager");
						if (gameObject == null)
						{
							gameObject = new GameObject("@TsQualityManager");
						}
						TsQualityManager.ms_Instance = gameObject.AddComponent<TsQualityManager>();
						TsQualityManager tsQualityManager2 = TsQualityManager.ms_Instance as TsQualityManager;
						tsQualityManager2.AutoLoading = true;
						tsQualityManager2.IsAutoGenerated = true;
					}
				}
			}
			return TsQualityManager.ms_Instance;
		}
	}

	public bool IsAutoGenerated
	{
		get;
		private set;
	}

	public int ShaderMaxLOD
	{
		get
		{
			return this.CurrQuality.ShaderMaxLOD;
		}
		set
		{
			this.m_ChagingShaderMaxLOD = value;
		}
	}

	public TsQualityManager.Level CurrLevel
	{
		get
		{
			if (TsQualityManager._Immortality.CurrLevel >= (TsQualityManager.Level)this.Length)
			{
				this._RegnerateTable();
			}
			return (TsQualityManager._Immortality.CurrLevel >= TsQualityManager.Level._length) ? TsQualityManager.DEFAULT_LEVEL : TsQualityManager._Immortality.CurrLevel;
		}
		set
		{
			if (value != TsQualityManager.Level._length && TsQualityManager._Immortality.CurrLevel != value)
			{
				TsQualityManager._Immortality.InactiveCustomMode();
				TsQualityManager._Immortality.CurrLevel = value;
				TsQualityManager._Immortality.SaveCurrLevel();
				this._Flush();
			}
		}
	}

	public TsQualityManager.GameVersion CurrVersion
	{
		get
		{
			return TsQualityManager.DEFAULT_GAMEVERSION;
		}
	}

	public ITsGameQuality CurrQuality
	{
		get
		{
			return this[this.CurrLevel];
		}
	}

	public int Length
	{
		get
		{
			return this.m_GameSettings.QualityTable.Length;
		}
	}

	public ITsGameQuality this[int index]
	{
		get
		{
			if (index >= this.m_GameSettings.QualityTable.Length)
			{
				index = this.m_GameSettings.QualityTable.Length - 1;
			}
			else if (index < 0)
			{
				index = 0;
			}
			return this.m_GameSettings.QualityTable[index];
		}
	}

	public ITsGameQuality this[TsQualityManager.Level level]
	{
		get
		{
			if (level == TsQualityManager.Level._length)
			{
				level = TsQualityManager.Level.HIGHEST;
			}
			if (level >= (TsQualityManager.Level)this.Length)
			{
				this._RegnerateTable();
			}
			return this.m_GameSettings.QualityTable[(int)level];
		}
	}

	public bool AutoLoading
	{
		get
		{
			return this.m_AutoLoading;
		}
		set
		{
			this.m_AutoLoading = value;
		}
	}

	public bool IsCustomMode
	{
		get
		{
			return TsQualityManager._Immortality.CustomMode;
		}
	}

	public string SettingsXmlPath
	{
		get
		{
			if (TsPlatform.IsMobile)
			{
				return "Mobile/XML/TsQualitySettings.xml";
			}
			return "WebPlayer/XML/TsQualitySettings.xml";
		}
	}

	public bool EnableCollectShader
	{
		get
		{
			return this.m_EnableCollectShader;
		}
		set
		{
			this.m_EnableCollectShader = value;
		}
	}

	private TsQualityManager()
	{
		this._Init();
	}

	void ITsSelfDestroyer.DestroyImmediate()
	{
		TsQualityManager tsQualityManager = TsQualityManager.ms_Instance as TsQualityManager;
		if (tsQualityManager && tsQualityManager.IsAutoGenerated)
		{
			UnityEngine.Object.DestroyImmediate(tsQualityManager.gameObject);
			TsQualityManager.ms_Instance = null;
		}
	}

	bool ITsQualityManager.RegisterScript(MonoBehaviour script, TsQualityManager.Level startLevel)
	{
		if (!TsQualityManager._Immortality.ScriptCollection.ContainsKey(script))
		{
			this._EnableScript(this.CurrLevel, script, startLevel);
			TsQualityManager._Immortality.ScriptCollection.Add(script, startLevel);
			return true;
		}
		return false;
	}

	void ITsQualityManager.ReleaseScript(MonoBehaviour script)
	{
		TsQualityManager._Immortality.ScriptCollection.Remove(script);
	}

	void ITsQualityManager.AutoEnableScript(MonoBehaviour script)
	{
		TsQualityManager.Level startLevel;
		if (TsQualityManager._Immortality.ScriptCollection.TryGetValue(script, out startLevel))
		{
			this._EnableScript(this.CurrLevel, script, startLevel);
		}
	}

	internal static bool IsSupportShadow(TsQualityManager.Level level)
	{
		return level >= TsQualityManager.Level.MEDIUM;
	}

	private void ApplyExtraPostEffect(TsQualityManager.Level level)
	{
		MonoBehaviour monoBehaviour = this.m_TargetCamera.GetComponent("Vignetting") as MonoBehaviour;
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = (level >= TsQualityManager.Level.MEDIUM);
		}
		monoBehaviour = (this.m_TargetCamera.GetComponent("Tonemapping") as MonoBehaviour);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = (level >= TsQualityManager.Level.LOWEST);
		}
		monoBehaviour = (this.m_TargetCamera.GetComponent("AntialiasingAsPostEffect") as MonoBehaviour);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = (level >= TsQualityManager.Level.HIGHEST);
		}
		monoBehaviour = (this.m_TargetCamera.GetComponent("ContrastEnhance") as MonoBehaviour);
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = (level >= TsQualityManager.Level.LOW);
		}
	}

	private void ApplyExtraSettings(TsQualityManager.Level level)
	{
		this.m_TargetCamera.farClipPlane = TsQualityManager.CAMERA_FAR[(int)level];
	}

	private void RecoverShadowDisatnce(TsQualityManager.Level level)
	{
		float[] array = new float[]
		{
			0f,
			0f,
			20f,
			40f,
			60f
		};
		QualitySettings.shadowDistance = array[(int)level];
	}

	private void DisableShadowDistance()
	{
		QualitySettings.shadowDistance = 0f;
	}

	private void Awake()
	{
		this.m_TerrainOriginalSetting.CopyFrom(this.m_TargetTerrain);
		Shader.globalMaximumLOD = 2147483647;
		TsQualityManager._Immortality.LoadCurrLevel();
		if (this.Length != 5)
		{
			this._RegnerateTable();
		}
	}

	private void Start()
	{
		this._LoadProperties();
	}

	private void OnEnable()
	{
		TsQualityManager tsQualityManager = TsQualityManager.ms_Instance as TsQualityManager;
		if (tsQualityManager != this && tsQualityManager != null && tsQualityManager.IsAutoGenerated)
		{
			UnityEngine.Object.Destroy(tsQualityManager.gameObject);
		}
		TsQualityManager.ms_Instance = this;
		this._LoadProperties();
	}

	private void OnDestroy()
	{
		this._ClearTargetLights();
	}

	private void _LoadProperties()
	{
		if (this.m_AutoLoading)
		{
			this.Load();
		}
		if (this.IsCustomMode)
		{
			this.m_GameSettings.Memory(this.CurrLevel);
			this.m_GameSettings.LoadCustomSettings();
		}
		if (!this.m_FirstRefreshed)
		{
			this.Refresh();
		}
	}

	private void OnDisable()
	{
		if (this.IsCustomMode)
		{
			this.m_GameSettings.Recovery(this.CurrLevel);
		}
		if (TsQualityManager.ms_Instance == this)
		{
			TsQualityManager.ms_Instance = null;
		}
		this.m_FirstRefreshed = false;
	}

	private void OnRenderObject()
	{
		if (this._ApplyCameraAction != null && Camera.main != null)
		{
			this.m_TargetCamera = Camera.main;
			this._ApplyCameraAction(this.CurrQuality as TsQualityManager.Quality);
			this._ApplyCameraAction = null;
		}
	}

	private void Update()
	{
		if (this._RefreshAction != null)
		{
			this._RefreshAction();
			this._RefreshAction = null;
		}
		this._ChangeShaderLOD();
	}

	private void _ChangeShaderLOD()
	{
		if (this.m_ChagingShaderMaxLOD != 2147483647)
		{
			Shader.globalMaximumLOD = this.m_ChagingShaderMaxLOD;
			foreach (Shader current in TsQualityManager._Immortality.Shaders.Values)
			{
				if (current != null)
				{
					current.maximumLOD = this.m_ChagingShaderMaxLOD;
				}
			}
			this.m_ChagingShaderMaxLOD = 2147483647;
		}
	}

	private void _Flush()
	{
		if (TsQualityManager._Immortality.CurrLevel == TsQualityManager.Level._length)
		{
			return;
		}
		if (this.m_TargetTerrain == null)
		{
			this.m_TargetTerrain = Terrain.activeTerrain;
		}
		if (this.m_TargetCamera == null)
		{
			this.m_TargetCamera = Camera.main;
		}
		ITsGameQuality currQuality = this.CurrQuality;
		TsQualityManager.Quality quality = currQuality as TsQualityManager.Quality;
		this.ShaderMaxLOD = quality.shaderMaxLOD;
		int num = 0;
		int qualityLevel = QualitySettings.GetQualityLevel();
		string[] names = QualitySettings.names;
		for (int i = 0; i < names.Length; i++)
		{
			string a = names[i];
			if (a == quality.qualityLevel)
			{
				break;
			}
			num++;
		}
		if (num != qualityLevel)
		{
			QualitySettings.SetQualityLevel(num);
		}
		if (QualitySettings.anisotropicFiltering != quality.anisotropicFiltering)
		{
			QualitySettings.anisotropicFiltering = quality.anisotropicFiltering;
		}
		QualitySettings.masterTextureLimit = 0;
		QualitySettings.vSyncCount = 0;
		quality.terrain.ApplyTo(this.m_TargetTerrain);
		foreach (Light current in this.m_TargetLights)
		{
			if (current.gameObject.GetComponent("TsShadowLOD") == null)
			{
				current.shadows = ((!currQuality.EnableShadow) ? LightShadows.None : quality.shadowType);
			}
		}
		if (currQuality.EnableShadow)
		{
			this.RecoverShadowDisatnce(quality.m_Level);
		}
		else
		{
			this.DisableShadowDistance();
		}
		this._AutoVisibleCharShadow();
		if (this.m_TargetCamera == null)
		{
			this._ApplyCameraAction = new TsQualityManager.QualityAction(this._ApplyCameraSettings);
		}
		else
		{
			this._ApplyCameraAction = null;
			this._ApplyCameraSettings(quality);
		}
		this._EnableScripts();
		if (TsQualityManager.UserInitalizer != null)
		{
			TsQualityManager.UserInitalizer();
		}
	}

	private void _ApplyCameraSettings(TsQualityManager.Quality quality)
	{
		if (this.m_TargetCamera == null)
		{
			return;
		}
		this.m_TargetCamera.cullingMask = (quality.renderableLayers | TsQualityManager._Immortality.DefaultRenderLayer);
		if (quality.useSkybox)
		{
			this.m_TargetCamera.clearFlags = CameraClearFlags.Skybox;
		}
		else
		{
			this.m_TargetCamera.clearFlags = CameraClearFlags.Color;
		}
		if (TsPlatform.IsWeb)
		{
			this._ActivePostEffect(quality);
			this.ApplyExtraSettings(quality.m_Level);
		}
		else if (TsPlatform.IsMobile)
		{
			this.ApplyExtraSettings(quality.m_Level);
		}
	}

	private void _ActivePostEffect(TsQualityManager.Quality quality)
	{
		MonoBehaviour monoBehaviour = this.m_TargetCamera.GetComponent("BloomAndLensFlares") as MonoBehaviour;
		if (monoBehaviour != null)
		{
			monoBehaviour.enabled = quality.bloomAndFlares;
		}
		MonoBehaviour monoBehaviour2 = this.m_TargetCamera.GetComponent("SunShafts") as MonoBehaviour;
		if (monoBehaviour2 != null)
		{
			monoBehaviour2.enabled = quality.sunShafts;
		}
		MonoBehaviour monoBehaviour3 = this.m_TargetCamera.GetComponent("DepthOfFieldScatter") as MonoBehaviour;
		if (monoBehaviour3 != null)
		{
			monoBehaviour3.enabled = quality.depthOfField;
		}
		MonoBehaviour monoBehaviour4 = this.m_TargetCamera.GetComponent("SSAOEffect") as MonoBehaviour;
		if (monoBehaviour4 != null)
		{
			monoBehaviour4.enabled = quality.SSAO;
		}
		this.ApplyExtraPostEffect(quality.m_Level);
	}

	public void Refresh()
	{
		this.m_TargetCamera = Camera.main;
		this._RefreshAction = new Action(this._RefreshTargetCamera);
	}

	public void Refresh(Camera camera)
	{
		if (camera != null)
		{
			this.m_TargetCamera = camera;
		}
		this._RefreshAction = new Action(this._RefreshTargetCamera);
	}

	private void _RefreshTargetCamera()
	{
		this.m_FirstRefreshed = true;
		if (this.EnableCollectShader)
		{
			this._ClearCollectedShaders();
			this._CollectShaders();
		}
		else
		{
			this.m_ChagingShaderMaxLOD = this.ShaderMaxLOD;
		}
		this._CollectTargetLights();
		this._Flush();
	}

	private static void _RegisterShader(Shader shader)
	{
		if (shader != null)
		{
			TsQualityManager._RegisterShader(shader.name);
		}
	}

	private static void _RegisterShader(string shaderName)
	{
		if (!TsQualityManager._Immortality.Shaders.ContainsKey(shaderName))
		{
			Shader shader = Shader.Find(shaderName);
			if (shader != null)
			{
				TsQualityManager._Immortality.Shaders.Add(shaderName, shader);
			}
		}
	}

	private void _EnableScripts()
	{
		TsQualityManager.Level currLevel = this.CurrLevel;
		foreach (KeyValuePair<MonoBehaviour, TsQualityManager.Level> current in TsQualityManager._Immortality.ScriptCollection)
		{
			this._EnableScript(currLevel, current.Key, current.Value);
		}
	}

	private void _EnableScript(TsQualityManager.Level currLevel, MonoBehaviour script, TsQualityManager.Level startLevel)
	{
		if (script == null)
		{
			return;
		}
		if (currLevel >= startLevel)
		{
			if (!script.enabled)
			{
				script.enabled = true;
			}
		}
		else if (script.enabled)
		{
			script.enabled = false;
		}
	}

	public bool Save(Stream stream)
	{
		bool result = false;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TsQualityManager.GameSettings));
			xmlSerializer.Serialize(new XmlTextWriter(stream, Encoding.UTF8)
			{
				Formatting = Formatting.Indented,
				IndentChar = '\t',
				Indentation = 1
			}, this.m_GameSettings);
			result = true;
		}
		finally
		{
			if (stream != null)
			{
				stream.Close();
			}
		}
		return result;
	}

	public bool Save(string path)
	{
		bool result;
		try
		{
			result = this.Save(new FileStream(path, FileMode.Create, FileAccess.Write));
		}
		catch (Exception ex)
		{
			TsLog.LogError(string.Concat(new object[]
			{
				"[TsQualityManager] ",
				path,
				" 파일 저장에 실패했습니다. (",
				ex,
				")"
			}), new object[0]);
			result = false;
		}
		return result;
	}

	public bool Save()
	{
		Directory.CreateDirectory("Resources");
		return this.Save("Assets/Resources/XML/TsQualitySettings.xml");
	}

	public bool SaveCustomSettings()
	{
		return this.m_GameSettings.SaveCustomSettings();
	}

	internal bool Load(Stream stream)
	{
		bool result = false;
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TsQualityManager.GameSettings));
			TsQualityManager.GameSettings src = xmlSerializer.Deserialize(stream) as TsQualityManager.GameSettings;
			this._CopyFrom(src);
			result = true;
			this.m_GameSettings.Memory(this.CurrLevel);
			this.m_GameSettings.LoadCustomSettings();
			this._Flush();
		}
		finally
		{
			if (stream != null)
			{
				stream.Close();
			}
		}
		return result;
	}

	public bool Load(string path)
	{
		bool result;
		try
		{
			result = this.Load(new FileStream(path, FileMode.Open, FileAccess.Read));
		}
		catch (Exception ex)
		{
			TsLog.LogWarning("[TsQualityManager] Quality setting file loading => failed ({0})\nDon't worry. Does not error processing. This message use just referancing about every case.\n({1})", new object[]
			{
				path,
				ex
			});
			result = false;
		}
		return result;
	}

	public bool Load()
	{
		string path = string.Empty;
		if (TsPlatform.IsMobile)
		{
			path = "Mobile/XML/TsQualitySettings";
		}
		else
		{
			path = "WebPlayer/XML/TsQualitySettings";
		}
		TextAsset textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
		return textAsset != null && this.Load(new MemoryStream(textAsset.bytes));
	}

	public bool LoadCustomSettings()
	{
		if (this.m_GameSettings.LoadCustomSettings())
		{
			this._Flush();
			return true;
		}
		return false;
	}

	public void Reset()
	{
		this._Init();
		TsQualityManager._Immortality.InactiveCustomMode();
		this._Flush();
	}

	public void RecoveryCustomSettings()
	{
		TsQualityManager._Immortality.InactiveCustomMode();
		this._Flush();
	}

	private void _Init()
	{
		this.IsAutoGenerated = false;
		this.m_GameSettings.Init(this);
	}

	private void _RegnerateTable()
	{
		if (this.m_GameSettings.QualityTable.Length == 3)
		{
			this.m_GameSettings = new TsQualityManager.GameSettings(this);
			this.m_GameSettings.QualityTable[0].CopyFrom(this.m_GameSettings.QualityTable[0]);
			this.m_GameSettings.QualityTable[1].CopyFrom(this.m_GameSettings.QualityTable[0]);
			this.m_GameSettings.QualityTable[2].CopyFrom(this.m_GameSettings.QualityTable[1]);
			this.m_GameSettings.QualityTable[3].CopyFrom(this.m_GameSettings.QualityTable[1]);
			this.m_GameSettings.QualityTable[4].CopyFrom(this.m_GameSettings.QualityTable[2]);
		}
		else
		{
			this.m_GameSettings = new TsQualityManager.GameSettings(this);
			this.Load();
		}
		this.m_GameSettings.LoadCustomSettings();
	}

	private void _CopyFrom(TsQualityManager.GameSettings src)
	{
		int num = (src.QualityTable.Length <= this.m_GameSettings.QualityTable.Length) ? src.QualityTable.Length : this.m_GameSettings.QualityTable.Length;
		for (int i = 0; i < num; i++)
		{
			this.m_GameSettings.QualityTable[i].CopyFrom(src.QualityTable[i]);
		}
	}

	private static void _MemoryOriginalSetting()
	{
		TsQualityManager tsQualityManager = TsQualityManager.Instance as TsQualityManager;
		tsQualityManager.m_GameSettings.Memory(tsQualityManager.CurrLevel);
	}

	private static void _RecoveryOriginalSetting()
	{
		TsQualityManager tsQualityManager = TsQualityManager.Instance as TsQualityManager;
		tsQualityManager.m_GameSettings.Recovery(tsQualityManager.CurrLevel);
	}

	public void CollectShader(Shader shader)
	{
		TsQualityManager._RegisterShader(shader);
		this.m_ChagingShaderMaxLOD = this.ShaderMaxLOD;
	}

	public void CollectShader(GameObject modelRoot)
	{
		Renderer[] components = modelRoot.GetComponents<Renderer>();
		this._CollectShaders(components);
	}

	private int _CollectShaders()
	{
		Renderer[] rs = UnityEngine.Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];
		return this._CollectShaders(rs);
	}

	private int _CollectShaders(Renderer[] rs)
	{
		return 0;
	}

	private void _ClearCollectedShaders()
	{
		TsQualityManager._Immortality.Shaders.Clear();
	}

	public void GetShaderList(out StringBuilder text)
	{
		text = new StringBuilder(1024);
		text.AppendFormat("GameObject = \"{1}/{0}\"", base.gameObject.name, (!base.gameObject.transform.parent) ? string.Empty : base.gameObject.transform.parent.name);
		text.AppendLine();
		text.AppendLine("===============================================");
		foreach (string current in TsQualityManager._Immortality.Shaders.Keys)
		{
			text.AppendFormat("   {0}", current);
			text.AppendLine();
		}
		text.AppendLine("===============================================");
		text.AppendFormat("Total = {0}", TsQualityManager._Immortality.Shaders.Count);
		text.AppendLine();
	}

	private void _PrintShaderLog()
	{
		if (Application.isEditor)
		{
			StringBuilder stringBuilder;
			this.GetShaderList(out stringBuilder);
			TsLog.Log("[TsQualityManager] shaders in current scene = \n{0}", new object[]
			{
				stringBuilder
			});
		}
	}

	private void _RegisterLight(Light light)
	{
		TsLightMarker tsLightMarker = light.GetComponent<TsLightMarker>();
		if (tsLightMarker != null && tsLightMarker.TargetLight != light)
		{
			UnityEngine.Object.Destroy(tsLightMarker);
			tsLightMarker = null;
		}
		if (tsLightMarker == null && light.shadows != LightShadows.None)
		{
			tsLightMarker = light.gameObject.AddComponent<TsLightMarker>();
			tsLightMarker.TargetLight = light;
		}
		if (tsLightMarker != null)
		{
			this.m_TargetLights.Add(light);
		}
	}

	private void _CollectTargetLights()
	{
		this._ClearTargetLights();
		Light[] array = UnityEngine.Object.FindObjectsOfType(typeof(Light)) as Light[];
		Light[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Light light = array2[i];
			this._RegisterLight(light);
		}
	}

	private void _ClearTargetLights()
	{
		this.m_TargetLights.Clear();
	}

	private void _AutoVisibleCharShadow()
	{
	}

	private static bool _IgnoreTerrainBasemap()
	{
		return true;
	}

	private static bool _IgnoreTreeBilboard()
	{
		return false;
	}

	private static bool _SupportLevelAutoDetection()
	{
		return true;
	}

	private static bool _SupportDetailSettings()
	{
		return true;
	}
}
