using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class SolSkillUpdateDlg : Form
{
	private const int MAX_NOMALSOL_BATTLESKILL_LEVEL = 50;

	private const int MAX_USERSOL_BATTLESKILL_LEVEL = 70;

	private const int MAX_LEGENDSOL_BATTLESKILL_LEVEL = 100;

	private DrawTexture SkillTargetIcon;

	private Label SkillTargetName;

	private Label SkillTargetLevel;

	private Label SkillCurrentLevel;

	private ScrollLabel SkillCurrentExplain;

	private Label SkillNextLevel;

	private ScrollLabel SkillNextExplain;

	private Label SkillAngerlypoint;

	private Label MyMoney;

	private Label SkillNeedMoney;

	private Button SkillUpdateButton;

	private float fClickTime;

	private bool m_UpdateCheck;

	private NkSoldierInfo pkSolinfo;

	private BATTLESKILL_TRAINING pkSkillinfo;

	private long nHaveMoney;

	private long nNeedMoney;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/DLG_SolSkillUpdate", G_ID.SOLSKILLUPDATE_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.SkillTargetIcon = (base.GetControl("DrawTexture_skillicon") as DrawTexture);
		this.SkillTargetName = (base.GetControl("Label_skillname") as Label);
		this.SkillTargetLevel = (base.GetControl("Label_skill_level") as Label);
		this.SkillCurrentLevel = (base.GetControl("Label_t_level01") as Label);
		this.SkillCurrentExplain = (base.GetControl("ScrollLabel_Explanation01") as ScrollLabel);
		this.SkillNextLevel = (base.GetControl("Label_t_level02") as Label);
		this.SkillNextExplain = (base.GetControl("ScrollLabel_Explanation02") as ScrollLabel);
		this.SkillAngerlypoint = (base.GetControl("Label_skill_angerlypoint") as Label);
		this.MyMoney = (base.GetControl("Label_gold_num") as Label);
		this.SkillNeedMoney = (base.GetControl("Label_Cost_num") as Label);
		this.SkillUpdateButton = (base.GetControl("Button_skillup") as Button);
		this.SkillUpdateButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickSkillUpdate));
		if (TsPlatform.IsMobile)
		{
			base.SetScreenCenter();
		}
	}

	public override void InitData()
	{
	}

	public void SetLocationByForm(Form pkTargetDlg)
	{
		float x = 100f;
		float y = 100f;
		if (pkTargetDlg != null)
		{
			x = pkTargetDlg.GetLocationX() + (pkTargetDlg.GetSizeX() - base.GetSizeX()) / 2f - 15f;
			y = pkTargetDlg.GetLocationY();
		}
		base.SetLocation(x, y);
	}

	public override void Update()
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if (this.nHaveMoney != kMyCharInfo.m_Money)
		{
			this.nHaveMoney = kMyCharInfo.m_Money;
			this.MyMoney.Text = ANNUALIZED.Convert(kMyCharInfo.m_Money);
		}
		if (this.fClickTime > 0f && Time.time - this.fClickTime >= 1f && this.m_UpdateCheck)
		{
			this.SkillUpdateButton.SetEnabled(true);
		}
	}

	public void Refresh()
	{
		int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.pkSkillinfo.m_nSkillUnique);
		this.pkSkillinfo = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(this.pkSkillinfo.m_nSkillUnique, battleSkillLevel);
		this.SetSkillAndMoneyInfo();
	}

	public void SetData(ref NkSoldierInfo solinfo, int skillunique)
	{
		this.SkillUpdateButton.SetEnabled(true);
		this.pkSolinfo = solinfo;
		int num = this.pkSolinfo.GetBattleSkillLevel(skillunique);
		if (num == 0)
		{
			num = 1;
		}
		this.pkSkillinfo = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(skillunique, num);
		this.SetSkillAndMoneyInfo();
	}

	private void SetSkillAndMoneyInfo()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.pkSkillinfo == null)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.pkSkillinfo.m_nSkillUnique);
		if (battleSkillBase == null)
		{
			return;
		}
		int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.pkSkillinfo.m_nSkillUnique);
		BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(this.pkSkillinfo.m_nSkillUnique, battleSkillLevel + 1);
		if (battleSkillTraining != null)
		{
		}
		bool flag = battleSkillLevel < (int)this.pkSolinfo.GetLevel() && battleSkillLevel < battleSkillBase.m_nSkillMaxLevel;
		bool flag2 = false;
		int num = battleSkillLevel + 1;
		int solMaxLevel = (int)this.pkSolinfo.GetSolMaxLevel();
		int skillLevel = Math.Min(num, solMaxLevel);
		if (num > solMaxLevel)
		{
			flag2 = true;
		}
		string text = string.Empty;
		int num2 = battleSkillLevel;
		UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique);
		this.SkillTargetIcon.SetTexture(battleSkillIconTexture);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1293"),
			"skillname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey)
		});
		this.SkillTargetName.Text = text;
		if (num2 == 0)
		{
			num2 = 1;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1885"),
			"skilllevel",
			num2.ToString()
		});
		this.SkillTargetLevel.Text = text;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1868"),
			"skilllevel",
			battleSkillLevel.ToString()
		});
		this.SkillCurrentLevel.Text = text;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1869"),
			"nextskilllevel",
			skillLevel.ToString()
		});
		this.SkillNextLevel.Text = text;
		if (battleSkillLevel == 0)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1893");
		}
		else
		{
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillBase.m_nSkillUnique, battleSkillLevel);
			if (battleSkillDetail != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref text, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, this.pkSolinfo);
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2153"),
					"count",
					battleSkillDetail.m_nSkillNeedAngerlyPoint.ToString()
				});
				this.SkillAngerlypoint.Text = empty;
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1868"),
					"skilllevel",
					battleSkillLevel.ToString()
				});
			}
		}
		this.SkillCurrentExplain.SetScrollLabel(text);
		if (flag2)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1299");
		}
		else
		{
			BATTLESKILL_DETAIL battleSkillDetail2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillBase.m_nSkillUnique, skillLevel);
			if (battleSkillDetail2 != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref text, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail2.m_nSkillTooltip), battleSkillDetail2, this.pkSolinfo);
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1869"),
					"nextskilllevel",
					skillLevel.ToString()
				});
			}
		}
		this.SkillNextExplain.SetScrollLabel(text);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			text = ANNUALIZED.Convert(kMyCharInfo.m_Money);
		}
		else
		{
			text = "0";
		}
		this.MyMoney.Text = text;
		if (!flag)
		{
			text = ANNUALIZED.Convert(this.pkSkillinfo.m_nSkillNeedGold);
			this.SkillNeedMoney.Text = text;
			this.nNeedMoney = (long)this.pkSkillinfo.m_nSkillNeedGold;
			this.SkillUpdateButton.SetEnabled(false);
			this.m_UpdateCheck = false;
		}
		else
		{
			text = ANNUALIZED.Convert(battleSkillTraining.m_nSkillNeedGold);
			this.SkillNeedMoney.Text = text;
			this.nNeedMoney = (long)battleSkillTraining.m_nSkillNeedGold;
			this.m_UpdateCheck = true;
		}
		if (this.pkSolinfo != null && this.pkSolinfo.IsSolWarehouse())
		{
			this.SkillUpdateButton.SetEnabled(false);
			this.m_UpdateCheck = false;
		}
	}

	private void OnClickSkillUpdate(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("547"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this.pkSkillinfo == null)
		{
			return;
		}
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.nNeedMoney > this.nHaveMoney)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
			if (lackGold_dlg != null)
			{
				lackGold_dlg.SetData(this.nNeedMoney - this.nHaveMoney);
			}
			return;
		}
		if (this.pkSolinfo.GetSolPosType() == 2 || this.pkSolinfo.GetSolPosType() == 6)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("357"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_SET_SOLDIER_BATTLESKILL_REQ gS_SET_SOLDIER_BATTLESKILL_REQ = new GS_SET_SOLDIER_BATTLESKILL_REQ();
		gS_SET_SOLDIER_BATTLESKILL_REQ.nSolID = this.pkSolinfo.GetSolID();
		gS_SET_SOLDIER_BATTLESKILL_REQ.nBattleSkillUnique = this.pkSkillinfo.m_nSkillUnique;
		gS_SET_SOLDIER_BATTLESKILL_REQ.nBattleSkillLevel = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_BATTLESKILL_REQ, gS_SET_SOLDIER_BATTLESKILL_REQ);
		this.SkillUpdateButton.SetEnabled(false);
		this.fClickTime = Time.time;
	}

	public void OnSendOKSetBattleSkill(object obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("547"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		GS_SET_SOLDIER_BATTLESKILL_REQ obj2 = obj as GS_SET_SOLDIER_BATTLESKILL_REQ;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_BATTLESKILL_REQ, obj2);
		this.SkillUpdateButton.SetEnabled(false);
	}
}
