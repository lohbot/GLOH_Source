using GAME;
using System;
using System.Collections.Generic;
using Ts;
using TsBundle;
using UnityEngine;

public class Nr3DCharSystem : NrTSingleton<Nr3DCharSystem>
{
	private const uint mc_ui32MaxDestroyCount = 100u;

	private Dictionary<int, Nr3DCharBase> m_mapChar = new Dictionary<int, Nr3DCharBase>();

	private uint m_ui32DestroyCnt;

	private Dictionary<string, GameObject> m_mapPlayerModels = new Dictionary<string, GameObject>();

	private Dictionary<string, BlendData[]> m_mapBlendTime = new Dictionary<string, BlendData[]>();

	private GameObject m_kPlayerModelRoot;

	private Dictionary<string, string> m_kAlphaShaderMaterials;

	private Nr3DCharSystem()
	{
		this.m_kAlphaShaderMaterials = new Dictionary<string, string>();
	}

	public bool Initialize()
	{
		return true;
	}

	public T Create3DChar<T>(int id, string strName) where T : Nr3DCharBase, new()
	{
		if (!this.m_mapChar.ContainsKey(id))
		{
			T t = Activator.CreateInstance<T>();
			t.SetID(id);
			t.SetName(strName);
			this.m_mapChar.Add(id, t);
			if (id <= 300)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(id);
				if (@char != null)
				{
					t.SetParentChar(@char);
				}
			}
			else
			{
				NkBattleChar char2 = NrTSingleton<NkBattleCharManager>.Instance.GetChar(id - 300);
				if (char2 != null)
				{
					t.SetParentChar(char2);
				}
			}
			return t;
		}
		if (NrTSingleton<NrGlobalReference>.Instance.IsEnableLog)
		{
			Debug.LogError("Create3DChar failed. (strName : " + strName);
		}
		return (T)((object)null);
	}

	public Nr3DCharBase Get3DChar(int id)
	{
		if (!this.m_mapChar.ContainsKey(id))
		{
			return null;
		}
		return this.m_mapChar[id];
	}

	public void Update()
	{
		using (new ScopeProfile("Nr3DCharSystem.Update"))
		{
			foreach (Nr3DCharBase current in this.m_mapChar.Values)
			{
				current.Update();
			}
		}
	}

	public bool Destroy3DChar(int id)
	{
		if (!this.m_mapChar.ContainsKey(id))
		{
			return false;
		}
		this.m_mapChar[id].Destroy();
		this.m_mapChar.Remove(id);
		this.m_ui32DestroyCnt += 1u;
		return true;
	}

	public void ProcessGC()
	{
		this.ProcessGC(false);
	}

	public void ResetGC()
	{
		this.m_ui32DestroyCnt = 0u;
	}

	public void ProcessGC(bool bForce)
	{
		if (100u <= this.m_ui32DestroyCnt || bForce)
		{
			Resources.UnloadUnusedAssets();
			this.m_ui32DestroyCnt = 0u;
		}
	}

	[Obsolete("캐릭터 인스턴스로 직접 지울 수 있도록 만들었는데, 사용되지 않는군요.")]
	public bool Destroy3DChar(Nr3DCharBase k3DCharInst)
	{
		return this.Destroy3DChar(k3DCharInst.GetID());
	}

	public void PushAlphaShaderRecovery(string name, string shadername)
	{
		if (!this.m_kAlphaShaderMaterials.ContainsKey(name))
		{
			this.m_kAlphaShaderMaterials.Add(name, shadername);
		}
	}

	public string PopAlphaShaderRecovery(string name)
	{
		string result = "AT2/_Char/Main_SSS_Alphatest";
		if (TsPlatform.IsMobile)
		{
			result = "AT2/AT2_Char_mobile";
		}
		if (this.m_kAlphaShaderMaterials.ContainsKey(name))
		{
			this.m_kAlphaShaderMaterials.TryGetValue(name, out result);
		}
		return result;
	}

	public void OnEvent3DModelCreated(GameObject pkCharRoot)
	{
		Nr3DCharSystem.FadeInShaderSet(pkCharRoot);
	}

	public static void FadeInShaderSet(GameObject _kGameObj)
	{
		string name = string.Empty;
		if (TsPlatform.IsMobile)
		{
			name = "Hidden/AT2_Fade_Object_mobile";
		}
		else
		{
			name = "Hidden/Diffuse_FadeOut";
		}
		Renderer[] componentsInChildren = _kGameObj.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Renderer renderer = componentsInChildren[i];
			if (!(renderer.particleEmitter != null) || !(renderer.particleSystem != null))
			{
				Material[] materials = renderer.materials;
				for (int j = 0; j < materials.Length; j++)
				{
					Material material = materials[j];
					NrTSingleton<Nr3DCharSystem>.Instance.PushAlphaShaderRecovery(material.name, material.shader.name);
					material.shader = Shader.Find(name);
					material.SetFloat("_Alpha", 0f);
				}
			}
		}
	}

	public void OnEvent3DModelPartItemChanged(GameObject pkCharRoot)
	{
		Renderer[] componentsInChildren = pkCharRoot.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Renderer renderer = componentsInChildren[i];
			renderer.renderer.enabled = true;
		}
	}

	public void FinishDownloadAnimation(IDownloadedItem wItem)
	{
	}

	public void SetBlend(string charcode, string ClipName, eCharAnimationType eType)
	{
	}

	public void SetBlend(string charcode, GameObject Target)
	{
		if (!this.m_mapBlendTime.ContainsKey(charcode) && null != Target)
		{
			NmAnimationBlendingHelper component = Target.GetComponent<NmAnimationBlendingHelper>();
			if (null != component)
			{
				BlendData[] blendDataArray = component.BlendDataArray;
				this.m_mapBlendTime.Add(charcode, blendDataArray);
			}
		}
	}

	public float GetBlend(string charcode, string SourceAni, string TargetAni)
	{
		if (this.m_mapBlendTime.ContainsKey(charcode))
		{
			BlendData[] array = this.m_mapBlendTime[charcode];
			BlendData[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				BlendData blendData = array2[i];
				if (blendData._strSourceName.Equals(SourceAni) && blendData._strTagetName.Equals(TargetAni))
				{
					return blendData._fBlendingTime;
				}
			}
		}
		return 0.3f;
	}

	public void _RegisterPlayerModel(GameObject originalPlayer, string assetPath, string charcode)
	{
		if (originalPlayer == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(originalPlayer) as GameObject;
		if (gameObject == null)
		{
			return;
		}
		if (this.m_mapPlayerModels.ContainsKey(charcode))
		{
			return;
		}
		AudioSource[] components = gameObject.GetComponents<AudioSource>();
		AudioSource[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			AudioSource audioSource = array[i];
			if (audioSource.clip == null)
			{
				audioSource.clip = TsAudioManager.Instance.GetTempClip();
			}
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		charcode = charcode.ToLower();
		Animation animation = gameObject.animation;
		if (animation)
		{
			animation.playAutomatically = false;
			try
			{
				if (this.m_kPlayerModelRoot == null)
				{
					this.m_kPlayerModelRoot = GameObject.Find("@internal chars");
					if (this.m_kPlayerModelRoot == null)
					{
						this.m_kPlayerModelRoot = new GameObject("@internal chars");
						UnityEngine.Object.DontDestroyOnLoad(this.m_kPlayerModelRoot);
					}
				}
				try
				{
					gameObject.name = "@" + charcode;
					gameObject.transform.parent = this.m_kPlayerModelRoot.transform;
					gameObject.animation.enabled = false;
					NkUtil.SetAllChildActive(gameObject, false);
					this.m_mapPlayerModels.Add(charcode, gameObject);
				}
				catch (ArgumentException ex)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"[narlamy] already registered key : key=",
						charcode,
						", Path=",
						assetPath,
						" (",
						ex,
						")"
					}));
				}
			}
			catch (NullReferenceException arg)
			{
				Debug.LogError("[narlamy] register model => error (null object) " + arg);
			}
			catch (Exception message)
			{
				Debug.Log(message);
			}
		}
	}

	public GameObject GetPlayerModelCache(string charcode)
	{
		if (!this.m_mapPlayerModels.ContainsKey(charcode))
		{
			Debug.LogError("Nr3DCharSystem GetPlayerModelCache => " + charcode);
			return null;
		}
		GameObject gameObject = this.m_mapPlayerModels[charcode];
		if (gameObject == null)
		{
			return null;
		}
		return gameObject;
	}

	public GameObject GetPlayerModelClone(string charcode)
	{
		if (!this.m_mapPlayerModels.ContainsKey(charcode))
		{
			Debug.LogError("Nr3DCharSystem GetPlayerModelClone => " + charcode);
			return null;
		}
		GameObject gameObject = this.m_mapPlayerModels[charcode];
		if (gameObject == null)
		{
			return null;
		}
		return UnityEngine.Object.Instantiate(gameObject) as GameObject;
	}
}
