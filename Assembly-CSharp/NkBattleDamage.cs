using System;
using UnityEngine;

public class NkBattleDamage
{
	public enum eDAMAGEMODE
	{
		eDAMAGEMODE_CRITICAL,
		eDAMAGEMODE_NORMAL,
		eDAMAGEMODE_HEAL,
		eDAMAGEMODE_ANGERLYPOINT,
		eDAMAGEMODE_SLEEP,
		eDAMAGEMODE_STUN,
		eDAMAGEMODE_MAX
	}

	public enum eINFOMODE
	{
		eINFOMODE_EMPTY,
		eINFOMODE_SLEEP,
		eINFOMODE_SILENCE,
		eINFOMODE_STUN,
		eINFOMODE_BLIND,
		eDAMAGEMODE_MAX
	}

	private static Rect[] m_rtDamage;

	private static string[] m_strStartAniSequence;

	private static float m_fNumberSizeHeight = 64f;

	private static float m_fNumberSizeWidth = 80f;

	private static int nMaxCount = 10;

	public static GameObject goDamageParent;

	private int m_nBattleCharID = -1;

	private GameObject m_goDamageRoot;

	private GameObject m_goFxFont;

	private GameObject m_goCritical;

	private GameObject m_goDamageNumber;

	private float fStartTime;

	private float fAnimationTime = 2f;

	private bool m_bDodge;

	private TsWeakReference<NkBattleChar> m_TargetChar;

	private NkBattleDamage.eDAMAGEMODE m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_NORMAL;

	private NkBattleDamage.eINFOMODE m_eInfoMode;

	private bool m_bSetData;

	public bool SetData
	{
		get
		{
			return this.m_bSetData;
		}
		set
		{
			this.m_bSetData = value;
		}
	}

	public static void SetStaticData()
	{
		if (NkBattleDamage.m_rtDamage == null)
		{
			Texture texture = null;
			GameObject gameObject = NkUtil.GetChild(Battle.BATTLE.DamageEffect.transform, "number1").gameObject;
			if (gameObject != null)
			{
				MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
				if (component != null)
				{
					texture = component.material.mainTexture;
				}
			}
			float num = (float)texture.width;
			float num2 = (float)texture.height;
			NkBattleDamage.m_rtDamage = new Rect[6];
			NkBattleDamage.m_fNumberSizeWidth *= num / 1024f;
			NkBattleDamage.m_fNumberSizeHeight *= num2 / 256f;
			for (int i = 0; i < 6; i++)
			{
				float top = 1f - (float)i * NkBattleDamage.m_fNumberSizeHeight / num2;
				NkBattleDamage.m_rtDamage[i] = new Rect(0f, top, NkBattleDamage.m_fNumberSizeWidth / num, NkBattleDamage.m_fNumberSizeHeight / num2);
			}
		}
		if (NkBattleDamage.m_strStartAniSequence == null)
		{
			NkBattleDamage.m_strStartAniSequence = new string[6];
			NkBattleDamage.m_strStartAniSequence[0] = "fx_criscaleup";
			NkBattleDamage.m_strStartAniSequence[1] = "fx_normalscaleup";
			NkBattleDamage.m_strStartAniSequence[2] = "fx_healscaleup";
			NkBattleDamage.m_strStartAniSequence[3] = "fx_angryscaleup";
			NkBattleDamage.m_strStartAniSequence[4] = "fx_sleepscaleup";
			NkBattleDamage.m_strStartAniSequence[5] = "fx_stunscaleup";
		}
		if (NkBattleDamage.goDamageParent != null)
		{
			UnityEngine.Object.Destroy(NkBattleDamage.goDamageParent);
			NkBattleDamage.goDamageParent = null;
		}
		if (NkBattleDamage.goDamageParent == null)
		{
			NkBattleDamage.goDamageParent = new GameObject("DamageRoot");
			GameObject gameObject2 = TsSceneSwitcher.Instance._GetSwitchData_RootSceneGO(TsSceneSwitcher.ESceneType.BattleScene);
			if (gameObject2 == null)
			{
				return;
			}
			NkBattleDamage.goDamageParent.transform.parent = gameObject2.transform;
		}
	}

	public void CreateDamageEffectFromReservationArray(int nIndex)
	{
		if (this.m_goDamageRoot == null)
		{
			this.m_goDamageRoot = (UnityEngine.Object.Instantiate(Battle.BATTLE.DamageEffect) as GameObject);
			this.m_goDamageRoot.SetActive(false);
			this.m_goDamageRoot.name = "BattleDamage(" + nIndex.ToString() + ")";
			this.m_goDamageRoot.transform.parent = NkBattleDamage.goDamageParent.transform;
		}
	}

	public void Update()
	{
		this.UpdatePosition();
	}

	public void Close()
	{
		if (this.m_goDamageRoot != null)
		{
			UnityEngine.Object.Destroy(this.m_goDamageRoot);
			this.m_goDamageRoot = null;
		}
	}

	public void Set(NkBattleChar pkTarget, int nDamage, bool bCritical, int nAngerlyPoint, int nInfoNum)
	{
		this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_NORMAL;
		this.m_TargetChar = pkTarget;
		this.m_nBattleCharID = pkTarget.GetID();
		this.m_bDodge = false;
		this.m_bSetData = true;
		int num = 0;
		if (this.m_goDamageRoot == null)
		{
			this.m_goDamageRoot = (UnityEngine.Object.Instantiate(Battle.BATTLE.DamageEffect) as GameObject);
			this.m_goDamageRoot.SetActive(true);
			this.m_goDamageRoot.transform.parent = NkBattleDamage.goDamageParent.transform;
		}
		else
		{
			this.m_goDamageRoot.SetActive(true);
		}
		if (this.m_goFxFont == null)
		{
			this.m_goFxFont = NkUtil.GetChild(this.m_goDamageRoot.transform, "fx_font").gameObject;
		}
		if (this.m_goCritical == null)
		{
			this.m_goCritical = NkUtil.GetChild(this.m_goDamageRoot.transform, "fx_critical").gameObject;
		}
		if (this.m_goCritical != null)
		{
			this.m_goCritical.SetActive(false);
		}
		if (this.m_goDamageNumber == null)
		{
			this.m_goDamageNumber = NkUtil.GetChild(this.m_goDamageRoot.transform, "damagenumber").gameObject;
		}
		if (nDamage == 0 && nAngerlyPoint == 0)
		{
			this.m_bDodge = true;
		}
		else
		{
			if (nDamage < 0)
			{
				nDamage *= -1;
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_NORMAL;
			}
			else if (nAngerlyPoint > 0)
			{
				nDamage = nAngerlyPoint;
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_ANGERLYPOINT;
			}
			else
			{
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_HEAL;
			}
			char[] array = nDamage.ToString().ToCharArray();
			num = array.Length;
			if (num > NkBattleDamage.nMaxCount)
			{
				num = NkBattleDamage.nMaxCount;
			}
			switch (nInfoNum)
			{
			case 1:
				this.m_eInfoMode = NkBattleDamage.eINFOMODE.eINFOMODE_SLEEP;
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_STUN;
				break;
			case 2:
				this.m_eInfoMode = NkBattleDamage.eINFOMODE.eINFOMODE_SILENCE;
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_STUN;
				break;
			case 3:
				this.m_eInfoMode = NkBattleDamage.eINFOMODE.eINFOMODE_STUN;
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_STUN;
				break;
			case 4:
				this.m_eInfoMode = NkBattleDamage.eINFOMODE.eINFOMODE_BLIND;
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_STUN;
				break;
			default:
				this.m_eInfoMode = NkBattleDamage.eINFOMODE.eINFOMODE_EMPTY;
				break;
			}
			if (bCritical)
			{
				this.m_eDamageMode = NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_CRITICAL;
				Battle.BATTLE.BattleCamera.CameraAnimationPlay("vibration01");
			}
			this.SetUVPosition(num, nDamage, this.m_bDodge, this.m_eDamageMode);
		}
		this.fStartTime = Time.time;
		this.UpdatePosition();
		this.PlayAnimation(num, this.m_bDodge);
	}

	public void UpdatePosition()
	{
		if (this.m_goDamageRoot == null)
		{
			return;
		}
		if (!this.m_bSetData)
		{
			return;
		}
		if (NrTSingleton<NkBattleCharManager>.Instance.GetChar(this.m_nBattleCharID) == null)
		{
			this.m_bSetData = false;
			this.m_goDamageRoot.SetActive(false);
			return;
		}
		if (this.m_TargetChar == null)
		{
			this.m_bSetData = false;
			this.m_goDamageRoot.SetActive(false);
			return;
		}
		if (this.m_TargetChar.CastedTarget == null)
		{
			this.m_bSetData = false;
			this.m_goDamageRoot.SetActive(false);
			return;
		}
		if (this.m_TargetChar.CastedTarget.GetNameDummy() == null)
		{
			this.m_bSetData = false;
			this.m_goDamageRoot.SetActive(false);
			return;
		}
		if (this.fStartTime == 0f)
		{
			this.m_bSetData = false;
			this.m_goDamageRoot.SetActive(false);
			return;
		}
		if (Time.time - this.fStartTime < this.fAnimationTime)
		{
			this.m_goDamageRoot.transform.position = this.m_TargetChar.GetCenterPosition();
		}
		else
		{
			this.m_bSetData = false;
			this.m_goDamageRoot.SetActive(false);
		}
	}

	private void SetUVPosition(int nNumberCount, int nDamage, bool bDodge, NkBattleDamage.eDAMAGEMODE eMode)
	{
		if (bDodge)
		{
			return;
		}
		char[] array = nDamage.ToString().ToCharArray();
		for (int i = 0; i < nNumberCount; i++)
		{
			int num = int.Parse(array[i].ToString());
			string strName = string.Format("number{0}", (i + 1).ToString());
			Transform child = NkUtil.GetChild(this.m_goFxFont.transform, strName);
			if (child != null)
			{
				GameObject gameObject = child.gameObject;
				if (gameObject)
				{
					Rect rect = new Rect(NkBattleDamage.m_rtDamage[(int)eMode]);
					rect.x += rect.width * (float)num;
					Vector2[] uv = new Vector2[]
					{
						new Vector2(rect.x, rect.y),
						new Vector2(rect.x + rect.width, rect.y - rect.height),
						new Vector2(rect.x, rect.y - rect.height),
						new Vector2(rect.x + rect.width, rect.y)
					};
					MeshFilter component = gameObject.GetComponent<MeshFilter>();
					if (component != null)
					{
						component.mesh.uv = uv;
					}
				}
			}
		}
	}

	private void PlayAnimation(int nNumberCount, bool bDodge)
	{
		if (this.m_goDamageRoot == null)
		{
			return;
		}
		if (this.m_goDamageNumber == null)
		{
			return;
		}
		if (this.m_goFxFont == null)
		{
			return;
		}
		Animation component = this.m_goDamageNumber.GetComponent<Animation>();
		if (component != null)
		{
			if (component.isPlaying)
			{
				component.Stop();
			}
			component.Play(NkBattleDamage.m_strStartAniSequence[(int)this.m_eDamageMode]);
			AnimationClip clip = component.GetClip(NkBattleDamage.m_strStartAniSequence[(int)this.m_eDamageMode]);
			if (clip != null)
			{
				this.fAnimationTime = clip.length + 0.1f;
			}
		}
		component = this.m_goFxFont.GetComponent<Animation>();
		string animation = string.Format("number_{0}", nNumberCount.ToString());
		if (component != null)
		{
			if (component.isPlaying)
			{
				component.Stop();
			}
			if (!bDodge)
			{
				component.Play(animation);
			}
			else
			{
				component.Play("miss");
			}
		}
		if (this.m_eDamageMode == NkBattleDamage.eDAMAGEMODE.eDAMAGEMODE_CRITICAL)
		{
			this.m_goCritical.SetActive(true);
			component = this.m_goCritical.GetComponent<Animation>();
			if (component != null)
			{
				if (component.isPlaying)
				{
					component.Stop();
				}
				component.Play("critical");
			}
		}
		if (this.m_eInfoMode == NkBattleDamage.eINFOMODE.eINFOMODE_SLEEP)
		{
			this.m_goCritical.SetActive(true);
			component = this.m_goCritical.GetComponent<Animation>();
			if (component != null)
			{
				if (component.isPlaying)
				{
					component.Stop();
				}
				component.Play("sleep");
			}
		}
		if (this.m_eInfoMode == NkBattleDamage.eINFOMODE.eINFOMODE_SILENCE)
		{
			this.m_goCritical.SetActive(true);
			component = this.m_goCritical.GetComponent<Animation>();
			if (component != null)
			{
				if (component.isPlaying)
				{
					component.Stop();
				}
				component.Play("silence");
			}
		}
		if (this.m_eInfoMode == NkBattleDamage.eINFOMODE.eINFOMODE_STUN)
		{
			this.m_goCritical.SetActive(true);
			component = this.m_goCritical.GetComponent<Animation>();
			if (component != null)
			{
				if (component.isPlaying)
				{
					component.Stop();
				}
				component.Play("stun");
			}
		}
		if (this.m_eInfoMode == NkBattleDamage.eINFOMODE.eINFOMODE_BLIND)
		{
			this.m_goCritical.SetActive(true);
			component = this.m_goCritical.GetComponent<Animation>();
			if (component != null)
			{
				if (component.isPlaying)
				{
					component.Stop();
				}
				component.Play("blind");
			}
		}
	}
}
