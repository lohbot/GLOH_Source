using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Battle_Colossum_CharinfoDlg : Form
{
	private Label m_lbAlly0CharName;

	private Label m_lbAlly1Name;

	private DrawTexture m_dwBGTurnAlly0;

	private DrawTexture m_dwBGTurnAlly1;

	private eBATTLE_ALLY m_eCurrentTurnAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private int nDeadCount0;

	private int nDeadCount1;

	private GameObject m_goAlly0Effect;

	private GameObject m_goAlly1Effect;

	private GameObject m_goStartCount;

	private float m_fEndTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_colosseum_charinfo", G_ID.BATTLE_COLOSSEUM_CHARINFO_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lbAlly0CharName = (base.GetControl("Label_mycharname") as Label);
		this.m_lbAlly1Name = (base.GetControl("Label_ChaName") as Label);
		this.m_dwBGTurnAlly0 = (base.GetControl("DrawTexture_DrawTexture9") as DrawTexture);
		this.m_dwBGTurnAlly1 = (base.GetControl("DrawTexture_DrawTexture10") as DrawTexture);
		if (this.m_dwBGTurnAlly0 != null)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_PLAYERTURN_LEFT", this.m_dwBGTurnAlly0, this.m_dwBGTurnAlly0.GetSize());
			this.m_dwBGTurnAlly0.AddGameObjectDelegate(new EZGameObjectDelegate(this.effectAlly0));
		}
		if (this.m_dwBGTurnAlly1 != null)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_PLAYERTURN_RIGHT", this.m_dwBGTurnAlly1, this.m_dwBGTurnAlly1.GetSize());
			this.m_dwBGTurnAlly1.AddGameObjectDelegate(new EZGameObjectDelegate(this.effectAlly1));
		}
		this.nDeadCount0 = 0;
		this.nDeadCount1 = 0;
		this.Hide();
	}

	public override void InitData()
	{
		base.InitData();
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, 0f);
	}

	public override void Update()
	{
		base.Update();
		if (Battle.BATTLE == null)
		{
			return;
		}
		if (Battle.BATTLE.CurrentTurnAlly != this.m_eCurrentTurnAlly)
		{
			this.m_eCurrentTurnAlly = Battle.BATTLE.CurrentTurnAlly;
			this.SetCurrentTurnAlly(this.m_eCurrentTurnAlly);
		}
		if (this.m_fEndTime != 0f && this.m_fEndTime < Time.realtimeSinceStartup)
		{
			UnityEngine.Object.Destroy(this.m_goStartCount);
			this.m_goStartCount = null;
			this.m_fEndTime = 0f;
		}
	}

	public override void OnClose()
	{
		if (this.m_goAlly0Effect != null)
		{
			UnityEngine.Object.Destroy(this.m_goAlly0Effect);
			this.m_goAlly0Effect = null;
		}
		if (this.m_goAlly1Effect != null)
		{
			UnityEngine.Object.Destroy(this.m_goAlly1Effect);
			this.m_goAlly1Effect = null;
		}
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, 0f);
	}

	public void Set(string szAlly0Name, string szAlly1Name)
	{
		if (Battle.BATTLE == null)
		{
			this.Close();
			return;
		}
		this.m_lbAlly0CharName.SetText(szAlly0Name);
		this.m_lbAlly1Name.SetText(szAlly1Name);
		this.Show();
	}

	public void SetDeadCount(eBATTLE_ALLY eAlly)
	{
		if (eAlly == eBATTLE_ALLY.eBATTLE_ALLY_0)
		{
			this.nDeadCount1++;
		}
		else
		{
			this.nDeadCount0++;
		}
	}

	public void SetCurrentTurnAlly(eBATTLE_ALLY eAlly)
	{
		if (eAlly == eBATTLE_ALLY.eBATTLE_ALLY_0)
		{
			if (this.m_goAlly0Effect != null)
			{
				this.m_goAlly0Effect.SetActive(true);
				this.m_goAlly0Effect.transform.localPosition = Vector3.zero;
			}
			if (this.m_goAlly1Effect != null)
			{
				this.m_goAlly1Effect.SetActive(false);
				this.m_goAlly1Effect.transform.localPosition = Vector3.zero;
			}
		}
		else
		{
			if (this.m_goAlly1Effect != null)
			{
				this.m_goAlly1Effect.SetActive(true);
				this.m_goAlly1Effect.transform.localPosition = Vector3.zero;
			}
			if (this.m_goAlly0Effect != null)
			{
				this.m_goAlly0Effect.SetActive(false);
				this.m_goAlly0Effect.transform.localPosition = Vector3.zero;
			}
		}
	}

	public void effectAlly0(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_goAlly0Effect = obj;
		this.m_goAlly0Effect.SetActive(false);
		this.m_goAlly0Effect.transform.localPosition = Vector3.zero;
		this.SetCurrentTurnAlly(this.m_eCurrentTurnAlly);
	}

	public void effectAlly1(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_goAlly1Effect = obj;
		this.m_goAlly1Effect.SetActive(false);
		this.m_goAlly1Effect.transform.localPosition = Vector3.zero;
		this.SetCurrentTurnAlly(this.m_eCurrentTurnAlly);
	}

	public void SetStartCountEffect()
	{
		if (this.m_goStartCount != null)
		{
			UnityEngine.Object.Destroy(this.m_goStartCount);
			this.m_goStartCount = null;
			this.m_fEndTime = 0f;
		}
		if (Battle.BATTLE.ColosseumCount == null)
		{
			return;
		}
		this.m_goStartCount = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.ColosseumCount, Vector3.zero, Quaternion.identity);
		NkUtil.SetAllChildLayer(this.m_goStartCount, GUICamera.UILayer);
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		this.m_goStartCount.transform.position = effectUIPos;
		this.m_goStartCount.SetActive(true);
		Animation componentInChildren = this.m_goStartCount.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			this.m_fEndTime = Time.realtimeSinceStartup + componentInChildren.clip.length;
		}
		else
		{
			this.m_fEndTime = Time.realtimeSinceStartup + 3.3f;
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goStartCount);
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COLOSSEUM", "COUNTDOWN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
