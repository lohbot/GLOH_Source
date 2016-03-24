using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class Battle_ColosseumEmergency_SelectDlg : Form
{
	public enum SOLCOUNT
	{
		SOLCOUNT_1,
		SOLCOUNT_2,
		SOLCOUNT_3,
		SOLCOUNT_4,
		SOLCOUNT_MAX
	}

	private COLOSSEUM_EMERGENCY_SOLDIER_COLTROLLER[] m_SoldierContoller;

	private List<NkSoldierInfo> m_kSolList = new List<NkSoldierInfo>();

	private Button m_btCancle;

	private Label m_lbRemainCount;

	private GameObject m_goCardSummon;

	private float m_fEndTime;

	private bool m_bFirstEffectSet;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.TopMost = true;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_CardSummon", G_ID.BATTLE_COLOSSEUMEMERGENCY_SELECT_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_SoldierContoller = new COLOSSEUM_EMERGENCY_SOLDIER_COLTROLLER[4];
		for (int i = 0; i < 4; i++)
		{
			this.m_SoldierContoller[i] = new COLOSSEUM_EMERGENCY_SOLDIER_COLTROLLER();
			this.m_SoldierContoller[i].m_dtRelationFace = new DrawTexture[6];
			string name = string.Empty;
			name = string.Format("Label_Name{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_lbName = (base.GetControl(name) as Label);
			name = string.Format("Label_SkillText{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_lbSkillDesc = (base.GetControl(name) as Label);
			name = string.Format("DrawTexture_CardBG{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_dwCardBG = (base.GetControl(name) as DrawTexture);
			this.m_SoldierContoller[i].m_dwCardBG.SetTextureFromBundle("UI/pvp/cardback02");
			this.m_SoldierContoller[i].m_dwCardBG.AddGameObjectDelegate(new EZGameObjectDelegate(this.DrawTextureAddEffectDelegate));
			NrTSingleton<FormsManager>.Instance.RequestAttachUIEffect("Effect/Instant/fx_card_line_mobile", this.m_SoldierContoller[i].m_dwCardBG, this.m_SoldierContoller[i].m_dwCardBG.GetSize());
			name = string.Format("DrawTexture_SolFace{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_dwSolFace = (base.GetControl(name) as DrawTexture);
			name = string.Format("DrawTexture_WeaponIcon{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_dwWeaphonIcon = (base.GetControl(name) as DrawTexture);
			name = string.Format("Button_CardButton{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_btSelect = (base.GetControl(name) as Button);
			Button expr_1E7 = this.m_SoldierContoller[i].m_btSelect;
			expr_1E7.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1E7.Click, new EZValueChangedDelegate(this.OnClickRequestEmergency));
			name = string.Format("Label_NoRelationText{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_lbNoRelation = (base.GetControl(name) as Label);
			name = string.Format("DrawTexture_NoRelationBG{0}", (i + 1).ToString("00"));
			this.m_SoldierContoller[i].m_dtNoRelationBG = (base.GetControl(name) as DrawTexture);
			for (int j = 0; j < 6; j++)
			{
				name = string.Format("DrawTexture_RelationFace{0}_{1}", (i + 1).ToString("00"), (j + 1).ToString("00"));
				this.m_SoldierContoller[i].m_dtRelationFace[j] = (base.GetControl(name) as DrawTexture);
			}
		}
		this.m_btCancle = (base.GetControl("Button_Cancel") as Button);
		Button expr_2F3 = this.m_btCancle;
		expr_2F3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2F3.Click, new EZValueChangedDelegate(this.OnClickCancle));
		this.m_lbRemainCount = (base.GetControl("Label_Text") as Label);
		string empty = string.Empty;
		int num = Battle.BATTLE.ChangeSolCount + 1;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2587"),
			"count",
			num
		});
		this.m_lbRemainCount.SetText(empty);
		this.SetSolList();
		base.ShowBlackBG(0.8f);
		this.m_bFirstEffectSet = false;
	}

	public override void Show()
	{
		if (!Battle.BATTLE.ShowColosseumSummonEffect)
		{
			base.SetShowLayer(1, false);
			this.SetFirstEffect();
		}
		else
		{
			base.SetShowLayer(1, true);
			this.ShowLabel();
			this.m_bFirstEffectSet = false;
		}
		base.Show();
	}

	public override void OnClose()
	{
		if (this.m_goCardSummon != null)
		{
			UnityEngine.Object.Destroy(this.m_goCardSummon);
			this.m_goCardSummon = null;
		}
		base.OnClose();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetScreenCenter();
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void OnClickCancle(IUIObject obj)
	{
		this.Close();
	}

	public void SetFirstEffect()
	{
		if (Battle.BATTLE.ShowColosseumSummonEffect)
		{
			return;
		}
		if (this.m_goCardSummon != null)
		{
			UnityEngine.Object.Destroy(this.m_goCardSummon);
			this.m_goCardSummon = null;
		}
		if (Battle.BATTLE.ColosseumCardSummons == null)
		{
			this.m_bFirstEffectSet = true;
			Battle.BATTLE.ShowColosseumSummonEffect = true;
			this.m_fEndTime = Time.realtimeSinceStartup + 0.2f;
		}
		this.m_goCardSummon = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.ColosseumCardSummons, Vector3.zero, Quaternion.identity);
		NkUtil.SetAllChildLayer(this.m_goCardSummon, GUICamera.UILayer);
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		this.m_goCardSummon.transform.position = effectUIPos;
		this.m_goCardSummon.SetActive(true);
		Animation componentInChildren = this.m_goCardSummon.GetComponentInChildren<Animation>();
		if (componentInChildren != null)
		{
			this.m_fEndTime = Time.realtimeSinceStartup + componentInChildren.clip.length - 0.2f;
		}
		else
		{
			this.m_fEndTime = Time.realtimeSinceStartup + 0.6f;
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_goCardSummon);
		}
		this.m_bFirstEffectSet = true;
		Battle.BATTLE.ShowColosseumSummonEffect = true;
	}

	public void OnClickRequestEmergency(IUIObject obj)
	{
		Button button = obj as Button;
		if (button == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = button.data as NkSoldierInfo;
		if (nkSoldierInfo == null)
		{
			return;
		}
		int charKind = nkSoldierInfo.GetCharKind();
		GS_BATTLE_CHANGE_SOLDIER_REQ gS_BATTLE_CHANGE_SOLDIER_REQ = new GS_BATTLE_CHANGE_SOLDIER_REQ();
		gS_BATTLE_CHANGE_SOLDIER_REQ.i32CharKind = charKind;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_CHANGE_SOLDIER_REQ, gS_BATTLE_CHANGE_SOLDIER_REQ);
		this.Close();
	}

	public void SetSolList()
	{
		this.m_kSolList.Clear();
		for (int i = 0; i < 4; i++)
		{
			COLOSSEUM_SUPPORTSOLDIER colosseumSupportSoldierdata = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetColosseumSupportSoldierdata(i);
			if (colosseumSupportSoldierdata != null)
			{
				int i32CharKind = colosseumSupportSoldierdata.i32CharKind;
				NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
				if (charKindInfo != null)
				{
					NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
					nkSoldierInfo.SetCharKind(i32CharKind);
					nkSoldierInfo.SetLevel(1);
					this.m_kSolList.Add(nkSoldierInfo);
					this.m_SoldierContoller[i].m_btSelect.data = nkSoldierInfo;
					this.m_SoldierContoller[i].m_lbName.SetText(nkSoldierInfo.GetName());
					string textureFromBundle = string.Empty;
					if (UIDataManager.IsUse256Texture())
					{
						textureFromBundle = "UI/Soldier/256/" + charKindInfo.GetPortraitFile1((int)nkSoldierInfo.GetGrade(), string.Empty) + "_256";
					}
					else
					{
						textureFromBundle = "UI/Soldier/512/" + charKindInfo.GetPortraitFile1((int)nkSoldierInfo.GetGrade(), string.Empty) + "_512";
					}
					this.m_SoldierContoller[i].m_dwSolFace.SetTextureFromBundle(textureFromBundle);
					if (charKindInfo.GetWeaponType() > 0)
					{
						this.m_SoldierContoller[i].m_dwWeaphonIcon.SetTexture(string.Format("Win_I_Weapon{0}", charKindInfo.GetWeaponType().ToString()));
					}
					int battleSkillUnique = charKindInfo.GetBattleSkillUnique(0);
					BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(battleSkillUnique);
					BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillUnique, 1);
					if (battleSkillBase == null || battleSkillDetail == null)
					{
						return;
					}
					if (battleSkillBase.m_nColosseumSkillDesc > 0)
					{
						string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_nColosseumSkillDesc.ToString());
						if (textFromInterface != string.Empty)
						{
							string empty = string.Empty;
							NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
							{
								textFromInterface,
								"skillname",
								NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
								"count",
								battleSkillDetail.m_nSkillNeedAngerlyPoint
							});
							this.m_SoldierContoller[i].m_lbSkillDesc.SetText(empty);
						}
					}
					bool bShowLabel = true;
					int num = 0;
					for (int j = 0; j < 6; j++)
					{
						if (colosseumSupportSoldierdata.i32RivalCharKind[j] > 0)
						{
							NrCharKindInfo charKindInfo2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(colosseumSupportSoldierdata.i32RivalCharKind[j]);
							if (charKindInfo2 != null)
							{
								string textureFromBundle2 = "UI/Soldier/64/" + charKindInfo2.GetPortraitFile1((int)nkSoldierInfo.GetGrade(), string.Empty) + "_64";
								this.m_SoldierContoller[i].m_dtRelationFace[num].SetTextureFromBundle(textureFromBundle2);
								num++;
								bShowLabel = false;
							}
						}
					}
					this.m_SoldierContoller[i].m_bShowLabel = bShowLabel;
				}
			}
		}
	}

	public void ShowLabel()
	{
		for (int i = 0; i < 4; i++)
		{
			this.m_SoldierContoller[i].m_lbNoRelation.Visible = this.m_SoldierContoller[i].m_bShowLabel;
			this.m_SoldierContoller[i].m_dtNoRelationBG.Visible = this.m_SoldierContoller[i].m_bShowLabel;
		}
	}

	public override void Update()
	{
		if (!this.m_bFirstEffectSet)
		{
			return;
		}
		if (Time.realtimeSinceStartup > this.m_fEndTime)
		{
			this.m_bFirstEffectSet = false;
			base.SetShowLayer(1, true);
			this.ShowLabel();
		}
	}

	public void DrawTextureAddEffectDelegate(IUIObject control, GameObject obj)
	{
		if (obj == null)
		{
			return;
		}
		obj.transform.localScale = new Vector3(1.05f, 1.1f, 1f);
	}
}
