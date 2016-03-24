using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ColosseumObserverControlDlg : Form
{
	public class OBSERVER_SOLDIERINFO
	{
		public short m_nBUID = -1;

		public float m_faxHpSize;

		public float m_fMaxHP;

		public DrawTexture m_dtHP;

		public DrawTexture m_dtBG;

		public DrawTexture m_dtDeadMark;

		public DrawTexture m_dtQuestion;

		public ItemTexture m_itSol;

		public GameObject m_goTurnEffect;

		public GameObject m_goHitEffect;
	}

	public class OBSERVER_ANGER
	{
		public int m_nAngerPoint;

		public int m_nMaxAngerPoint;

		public float m_fMaxAngerSize;

		public float m_fBeforeAngerSize;

		public float m_fLoactionY;

		public DrawTexture m_dtAngerGage;

		public Label m_lbAngerPoint;
	}

	private DrawTexture m_dtAngerFrame1;

	private DrawTexture m_dtAngerFrame2;

	private ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO[][] m_obSoldier;

	private ColosseumObserverControlDlg.OBSERVER_ANGER[] m_obAnger;

	private Button m_btExit;

	private GameObject m_goKillEffect;

	private GameObject m_goCriticalEffect;

	private GameObject m_goSummonEffect;

	private float m_fEndTime;

	private float m_fCriticalEndTime;

	private string faceImageKey = string.Empty;

	private float m_fSummonEndTime;

	private bool m_bSetFace = true;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Observer_Control", G_ID.COLOSSEUM_OBSERVER_CONTROL_DLG, false, true);
	}

	public override void SetComponent()
	{
		this.m_obAnger = new ColosseumObserverControlDlg.OBSERVER_ANGER[2];
		this.m_dtAngerFrame1 = (base.GetControl("DT_AngerFrame_1P") as DrawTexture);
		this.m_dtAngerFrame2 = (base.GetControl("DT_AngerFrame_2P") as DrawTexture);
		Texture2D texture2D = CResources.Load(NrTSingleton<UIDataManager>.Instance.FilePath + "Texture/AngerBG") as Texture2D;
		if (texture2D != null)
		{
			if (this.m_dtAngerFrame1 != null)
			{
				this.m_dtAngerFrame1.SetTexture(texture2D);
			}
			if (this.m_dtAngerFrame2 != null)
			{
				this.m_dtAngerFrame2.SetTexture(texture2D);
			}
		}
		this.m_btExit = (base.GetControl("btn_Retreat") as Button);
		Button expr_B9 = this.m_btExit;
		expr_B9.Click = (EZValueChangedDelegate)Delegate.Combine(expr_B9.Click, new EZValueChangedDelegate(this.OnClickRetreat));
		this.m_obSoldier = new ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO[2][];
		for (int i = 0; i < 2; i++)
		{
			this.m_obAnger[i] = new ColosseumObserverControlDlg.OBSERVER_ANGER();
			this.m_obAnger[i].m_nMaxAngerPoint = 1000;
			string name = string.Empty;
			name = string.Format("DT_AngerGage_{0}P", (i + 1).ToString("0"));
			this.m_obAnger[i].m_dtAngerGage = (base.GetControl(name) as DrawTexture);
			name = string.Format("LB_AngerNum_{0}P", (i + 1).ToString("0"));
			this.m_obAnger[i].m_lbAngerPoint = (base.GetControl(name) as Label);
			this.m_obAnger[i].m_lbAngerPoint.SetText("0");
			this.m_obAnger[i].m_fMaxAngerSize = this.m_obAnger[i].m_dtAngerGage.GetSize().y;
			this.m_obAnger[i].m_fBeforeAngerSize = -1f;
			this.m_obAnger[i].m_fLoactionY = this.m_obAnger[i].m_dtAngerGage.GetLocationY();
			this.SetAngerText((eBATTLE_ALLY)i);
			this.m_obSoldier[i] = new ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO[6];
			for (int j = 0; j < 6; j++)
			{
				this.m_obSoldier[i][j] = new ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO();
				name = string.Format("ItemTexture_SolFace_{0}P_{1}", (i + 1).ToString("0"), (j + 1).ToString("00"));
				this.m_obSoldier[i][j].m_itSol = (base.GetControl(name) as ItemTexture);
				this.m_obSoldier[i][j].m_itSol.AddGameObjectDelegate(new EZGameObjectDelegate(this.TurnEffect));
				NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_SKILL_ACTIVE", this.m_obSoldier[i][j].m_itSol, this.m_obSoldier[i][j].m_itSol.GetSize());
				name = string.Format("DrawTexture_SolBG_{0}P_{1}", (i + 1).ToString("0"), (j + 1).ToString("00"));
				this.m_obSoldier[i][j].m_dtBG = (base.GetControl(name) as DrawTexture);
				this.m_obSoldier[i][j].m_dtBG.AddGameObjectDelegate(new EZGameObjectDelegate(this.HitEffect));
				NrTSingleton<FormsManager>.Instance.AttachEffectKey("BATTLE_HIT_UI", this.m_obSoldier[i][j].m_dtBG, this.m_obSoldier[i][j].m_dtBG.GetSize());
				name = string.Format("DrawTexture_HPBarPrg_{0}P_{1}", (i + 1).ToString("0"), (j + 1).ToString("00"));
				this.m_obSoldier[i][j].m_dtHP = (base.GetControl(name) as DrawTexture);
				this.m_obSoldier[i][j].m_faxHpSize = this.m_obSoldier[i][j].m_dtHP.GetSize().x;
				name = string.Format("DrawTexture_DeadMark_{0}P_{1}", (i + 1).ToString("0"), (j + 1).ToString("00"));
				this.m_obSoldier[i][j].m_dtDeadMark = (base.GetControl(name) as DrawTexture);
				this.m_obSoldier[i][j].m_dtDeadMark.Visible = false;
				name = string.Format("DrawTexture_QuestionMark_{0}P_{1}", (i + 1).ToString("0"), (j + 1).ToString("00"));
				this.m_obSoldier[i][j].m_dtQuestion = (base.GetControl(name) as DrawTexture);
				this.m_obSoldier[i][j].m_dtQuestion.Visible = true;
			}
		}
		this.MakeAllBattleCharInfo();
		this._SetDialogPos();
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		base.Update();
		if (this.m_fEndTime != 0f && Time.realtimeSinceStartup > this.m_fEndTime && this.m_goKillEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goKillEffect);
			this.m_goKillEffect = null;
		}
		if (this.m_fCriticalEndTime != 0f && Time.realtimeSinceStartup > this.m_fCriticalEndTime && this.m_goCriticalEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goCriticalEffect);
			this.m_goCriticalEffect = null;
		}
		if (!this.m_bSetFace)
		{
			Transform child = NkUtil.GetChild(this.m_goSummonEffect.transform, "fx_face_base");
			if (child != null)
			{
				GameObject gameObject = child.gameObject;
				if (null != gameObject)
				{
					Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey);
					if (null != texture)
					{
						Renderer component = gameObject.GetComponent<Renderer>();
						if (component != null)
						{
							Material material = component.material;
							if (null != material)
							{
								material.mainTexture = texture;
								this.m_bSetFace = true;
								this.m_goSummonEffect.SetActive(true);
								TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "BATTLE", "HERO_CALL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
								Animation componentInChildren = this.m_goSummonEffect.GetComponentInChildren<Animation>();
								if (componentInChildren != null)
								{
									this.m_fSummonEndTime = Time.realtimeSinceStartup + componentInChildren.clip.length;
									componentInChildren.Stop();
									componentInChildren.Play();
								}
								else
								{
									this.m_fSummonEndTime = Time.realtimeSinceStartup + 1.5f;
								}
							}
						}
					}
				}
			}
		}
		else if (this.m_fSummonEndTime != 0f && this.m_fSummonEndTime < Time.realtimeSinceStartup && this.m_goSummonEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goSummonEffect);
			this.m_goSummonEffect = null;
			this.m_fSummonEndTime = 0f;
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.m_goKillEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goKillEffect);
			this.m_goKillEffect = null;
		}
		if (this.m_goKillEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goKillEffect);
			this.m_goKillEffect = null;
		}
		if (this.m_goCriticalEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goCriticalEffect);
			this.m_goCriticalEffect = null;
		}
		if (this.m_goSummonEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goSummonEffect);
			this.m_goSummonEffect = null;
		}
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, GUICamera.height - base.GetSizeY());
	}

	public void SetAngerPoint(eBATTLE_ALLY nAlly, int nPoint)
	{
		if (nPoint >= this.m_obAnger[(int)nAlly].m_nMaxAngerPoint)
		{
			nPoint = this.m_obAnger[(int)nAlly].m_nMaxAngerPoint;
		}
		this.m_obAnger[(int)nAlly].m_nAngerPoint = nPoint;
		this.SetAngerText(nAlly);
	}

	public void AddAngerPoint(eBATTLE_ALLY nAlly, int nPoint)
	{
		this.m_obAnger[(int)nAlly].m_nAngerPoint += nPoint;
		if (this.m_obAnger[(int)nAlly].m_nAngerPoint >= this.m_obAnger[(int)nAlly].m_nMaxAngerPoint)
		{
			this.m_obAnger[(int)nAlly].m_nAngerPoint = this.m_obAnger[(int)nAlly].m_nMaxAngerPoint;
		}
		if (this.m_obAnger[(int)nAlly].m_nAngerPoint < 0)
		{
			this.m_obAnger[(int)nAlly].m_nAngerPoint = 0;
		}
		this.SetAngerText(nAlly);
	}

	public void SetAngerText(eBATTLE_ALLY nAlly)
	{
		this.m_obAnger[(int)nAlly].m_lbAngerPoint.SetText(this.m_obAnger[(int)nAlly].m_nAngerPoint.ToString());
		float num;
		if (this.m_obAnger[(int)nAlly].m_nAngerPoint == this.m_obAnger[(int)nAlly].m_nMaxAngerPoint)
		{
			num = this.m_obAnger[(int)nAlly].m_fMaxAngerSize;
		}
		else
		{
			num = this.m_obAnger[(int)nAlly].m_fMaxAngerSize * ((float)this.m_obAnger[(int)nAlly].m_nAngerPoint / (float)this.m_obAnger[(int)nAlly].m_nMaxAngerPoint);
		}
		if (this.m_obAnger[(int)nAlly].m_fBeforeAngerSize != num)
		{
			this.m_obAnger[(int)nAlly].m_dtAngerGage.SetSize(this.m_obAnger[(int)nAlly].m_dtAngerGage.GetSize().x, num);
			this.m_obAnger[(int)nAlly].m_dtAngerGage.SetLocation(this.m_obAnger[(int)nAlly].m_dtAngerGage.GetLocationX(), this.m_obAnger[(int)nAlly].m_fLoactionY + (this.m_obAnger[(int)nAlly].m_fMaxAngerSize - this.m_obAnger[(int)nAlly].m_dtAngerGage.GetSize().y));
			this.m_obAnger[(int)nAlly].m_fBeforeAngerSize = num;
		}
	}

	public void OnClickRetreat(IUIObject obj)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string message = string.Empty;
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1068");
		textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1767");
		message = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("102");
		msgBoxUI.SetMsg(new YesDelegate(this.OnRetreatkOK), null, textFromInterface, message, eMsgType.MB_OK_CANCEL, 2);
	}

	public void OnRetreatkOK(object a_oObject)
	{
		Battle.BATTLE.Send_GS_BATTLE_CLOSE_REQ();
	}

	public ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO GetObSoldierInfo(eBATTLE_ALLY eAlly, short nBuid)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_obSoldier[(int)eAlly][i].m_nBUID == nBuid)
			{
				return this.m_obSoldier[(int)eAlly][i];
			}
		}
		for (int i = 0; i < 6; i++)
		{
			if (this.m_obSoldier[(int)eAlly][i].m_nBUID == -1)
			{
				return this.m_obSoldier[(int)eAlly][i];
			}
		}
		return null;
	}

	public void TurnEffect(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				if (this.m_obSoldier[i][j].m_itSol == control)
				{
					this.m_obSoldier[i][j].m_goTurnEffect = obj;
					this.m_obSoldier[i][j].m_goTurnEffect.transform.localScale = new Vector3(0.85f, 0.85f, 1f);
					NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(this.m_obSoldier[i][j].m_nBUID);
					if (charByBUID != null)
					{
						if (charByBUID.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE)
						{
							this.m_obSoldier[i][j].m_goTurnEffect.SetActive(true);
						}
						else
						{
							this.m_obSoldier[i][j].m_goTurnEffect.SetActive(false);
						}
					}
					else
					{
						this.m_obSoldier[i][j].m_goTurnEffect.SetActive(false);
					}
				}
			}
		}
	}

	public void HitEffect(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				if (this.m_obSoldier[i][j].m_dtBG == control)
				{
					this.m_obSoldier[i][j].m_goHitEffect = obj;
					this.m_obSoldier[i][j].m_goHitEffect.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
					this.m_obSoldier[i][j].m_goHitEffect.SetActive(false);
				}
			}
		}
	}

	public void MakeAllBattleCharInfo()
	{
		NkBattleChar[] charArray = NrTSingleton<NkBattleCharManager>.Instance.GetCharArray();
		for (int i = 0; i < charArray.Length; i++)
		{
			NkBattleChar nkBattleChar = charArray[i];
			if (nkBattleChar != null)
			{
				if (nkBattleChar.GetBUID() > -1)
				{
					ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO obSoldierInfo = this.GetObSoldierInfo(nkBattleChar.Ally, nkBattleChar.GetBUID());
					if (obSoldierInfo != null)
					{
						if (obSoldierInfo.m_nBUID == -1)
						{
							NrCharKindInfo charKindInfo = nkBattleChar.GetCharKindInfo();
							if (charKindInfo != null)
							{
								string textureFromBundle = string.Empty;
								textureFromBundle = "UI/Soldier/64/" + charKindInfo.GetPortraitFile1(0, string.Empty) + "_64";
								obSoldierInfo.m_itSol.SetTextureFromBundle(textureFromBundle);
								obSoldierInfo.m_nBUID = nkBattleChar.GetBUID();
								obSoldierInfo.m_dtQuestion.Visible = false;
								obSoldierInfo.m_fMaxHP = (float)nkBattleChar.GetMaxHP(false);
								this.UpdateHP(nkBattleChar.Ally, nkBattleChar.GetBUID(), (float)nkBattleChar.GetSoldierInfo().GetHP(), 0, false);
								this.SetEnableTurn(nkBattleChar.Ally, nkBattleChar.GetBUID(), nkBattleChar.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE);
							}
						}
					}
				}
			}
		}
	}

	public void MakeBattleCharInfo(short nBUID)
	{
		NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(nBUID);
		if (charByBUID != null)
		{
			if (charByBUID.GetBUID() <= -1)
			{
				return;
			}
			ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO obSoldierInfo = this.GetObSoldierInfo(charByBUID.Ally, charByBUID.GetBUID());
			if (obSoldierInfo == null)
			{
				return;
			}
			if (obSoldierInfo.m_nBUID == -1)
			{
				NrCharKindInfo charKindInfo = charByBUID.GetCharKindInfo();
				if (charKindInfo == null)
				{
					return;
				}
				string textureFromBundle = string.Empty;
				textureFromBundle = "UI/Soldier/64/" + charKindInfo.GetPortraitFile1(0, string.Empty) + "_64";
				obSoldierInfo.m_itSol.SetTextureFromBundle(textureFromBundle);
				obSoldierInfo.m_nBUID = charByBUID.GetBUID();
				obSoldierInfo.m_dtQuestion.Visible = false;
				obSoldierInfo.m_fMaxHP = (float)charByBUID.GetMaxHP(false);
				this.UpdateHP(charByBUID.Ally, charByBUID.GetBUID(), (float)charByBUID.GetSoldierInfo().GetHP(), 0, false);
				this.SetEnableTurn(charByBUID.Ally, charByBUID.GetBUID(), charByBUID.GetTurnState() != eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_DISABLE);
			}
		}
	}

	public void UpdateHP(eBATTLE_ALLY nAlly, short nBUID, float fCurHP, int nDamage, bool bCritical)
	{
		ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO obSoldierInfo = this.GetObSoldierInfo(nAlly, nBUID);
		if (obSoldierInfo == null)
		{
			return;
		}
		if (fCurHP > obSoldierInfo.m_fMaxHP)
		{
			fCurHP = obSoldierInfo.m_fMaxHP;
		}
		float num = fCurHP / obSoldierInfo.m_fMaxHP;
		obSoldierInfo.m_dtHP.SetSize(obSoldierInfo.m_faxHpSize * num, obSoldierInfo.m_dtHP.GetSize().y);
		if (nDamage < 0)
		{
			obSoldierInfo.m_goHitEffect.SetActive(false);
			obSoldierInfo.m_goHitEffect.SetActive(true);
			if (bCritical)
			{
				this.SetCriticalEffect();
			}
		}
	}

	public void SetDeadFlag(eBATTLE_ALLY nAlly, short nBUID)
	{
		ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO obSoldierInfo = this.GetObSoldierInfo(nAlly, nBUID);
		if (obSoldierInfo == null)
		{
			return;
		}
		obSoldierInfo.m_dtDeadMark.Visible = true;
		if (obSoldierInfo.m_goTurnEffect != null)
		{
			obSoldierInfo.m_goTurnEffect.SetActive(false);
		}
		this.SetKillEffect();
	}

	public void SetEnableTurn(eBATTLE_ALLY nAlly, short nBUID, bool bEnable)
	{
		ColosseumObserverControlDlg.OBSERVER_SOLDIERINFO obSoldierInfo = this.GetObSoldierInfo(nAlly, nBUID);
		if (obSoldierInfo == null)
		{
			return;
		}
		if (obSoldierInfo.m_goTurnEffect == null)
		{
			return;
		}
		if (bEnable)
		{
			obSoldierInfo.m_goTurnEffect.SetActive(true);
		}
		else
		{
			obSoldierInfo.m_goTurnEffect.SetActive(false);
		}
	}

	public void SetKillEffect()
	{
		if (this.m_goKillEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goKillEffect);
			this.m_goKillEffect = null;
			this.m_fEndTime = 0f;
		}
		if (Battle.BATTLE.ColosseumKill == null)
		{
			return;
		}
		this.m_goKillEffect = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.ColosseumKill, Vector3.zero, Quaternion.identity);
		NkUtil.SetAllChildLayer(this.m_goKillEffect, GUICamera.UILayer);
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		this.m_goKillEffect.transform.position = effectUIPos;
		this.m_goKillEffect.SetActive(true);
		Animation componentInChildren = this.m_goKillEffect.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			this.m_fEndTime = Time.realtimeSinceStartup + componentInChildren.clip.length;
		}
		else
		{
			this.m_fEndTime = Time.realtimeSinceStartup + 0.8f;
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goKillEffect);
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COLOSSEUM", "BATTLEKILL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SetCriticalEffect()
	{
		if (this.m_goCriticalEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goCriticalEffect);
			this.m_goCriticalEffect = null;
			this.m_fCriticalEndTime = 0f;
		}
		if (Battle.BATTLE.ColosseumKill == null)
		{
			return;
		}
		this.m_goCriticalEffect = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.ColosseumCritical, Vector3.zero, Quaternion.identity);
		NkUtil.SetAllChildLayer(this.m_goCriticalEffect, GUICamera.UILayer);
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		this.m_goCriticalEffect.transform.position = effectUIPos;
		this.m_goCriticalEffect.SetActive(true);
		Animation componentInChildren = this.m_goCriticalEffect.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			this.m_fCriticalEndTime = Time.realtimeSinceStartup + componentInChildren.clip.length;
		}
		else
		{
			this.m_fCriticalEndTime = Time.realtimeSinceStartup + 0.7f;
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goCriticalEffect);
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COLOSSEUM", "BATTLEHIT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SetSummonEffect(int nKind)
	{
		if (this.m_goSummonEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goSummonEffect);
			this.m_goSummonEffect = null;
			this.m_fSummonEndTime = 0f;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nKind);
		if (charKindInfo == null)
		{
			return;
		}
		if (Battle.BATTLE.ColosseumRecall == null)
		{
			return;
		}
		this.m_goSummonEffect = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.ColosseumRecall, Vector3.zero, Quaternion.identity);
		NkUtil.SetAllChildLayer(this.m_goSummonEffect, GUICamera.UILayer);
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		this.m_goSummonEffect.transform.position = effectUIPos;
		if (UIDataManager.IsUse256Texture())
		{
			this.faceImageKey = charKindInfo.GetPortraitFile1(0, string.Empty) + "_256";
		}
		else
		{
			this.faceImageKey = charKindInfo.GetPortraitFile1(0, string.Empty) + "_512";
		}
		if (null == NrTSingleton<UIImageBundleManager>.Instance.GetTexture(this.faceImageKey))
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(this.faceImageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetBundleImage));
		}
		this.m_bSetFace = false;
		Animation componentInChildren = this.m_goSummonEffect.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			this.m_fSummonEndTime = Time.realtimeSinceStartup + componentInChildren.clip.length;
		}
		else
		{
			this.m_fSummonEndTime = Time.realtimeSinceStartup + 1.5f;
		}
		this.m_goSummonEffect.SetActive(false);
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goKillEffect);
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "COLOSSEUM", "BATTLECALL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	private void SetBundleImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null)
		{
			if (null != _item.GetSafeBundle().mainAsset)
			{
				Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
				if (null != texture2D)
				{
					string imageKey = string.Empty;
					if (_param is string)
					{
						imageKey = (string)_param;
						NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
					}
				}
			}
		}
		else if (this.m_goSummonEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goSummonEffect);
			this.m_goSummonEffect = null;
			this.m_fSummonEndTime = 0f;
		}
	}
}
