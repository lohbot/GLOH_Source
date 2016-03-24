using GAME;
using Ndoors.Framework.Stage;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
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

	private Label SkillMaxNeedMoney;

	private Button SkillMaxUpdateButton;

	private Button m_btClose;

	private Label m_lbDragonHeart_Num;

	private Button m_btMythSkillUp;

	private Label m_lbDragonHeartCost_Num;

	private DrawTexture m_dtDragonHeartM_C;

	private NkSoldierInfo pkSolinfo;

	private int m_i32SkillUnique;

	private long m_i64NeedItem;

	private long nHaveMoney;

	private long nMaxNeedMoney;

	private long nMaxNeedMoney_real;

	private int nMaxSkillLevel_real;

	private DrawTexture dtSkillMoneyIcon;

	private DrawTexture dtMaxSkillMoneyIcon;

	private bool m_CheckCanMaxSkillUp;

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
		this.SkillMaxNeedMoney = (base.GetControl("Label_cost_maxskill") as Label);
		this.SkillMaxUpdateButton = (base.GetControl("Button_Max_skillup") as Button);
		this.SkillMaxUpdateButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMaxSkillUpdate));
		this.dtSkillMoneyIcon = (base.GetControl("DrawTexture_MoneyIconM_C") as DrawTexture);
		this.dtMaxSkillMoneyIcon = (base.GetControl("DrawTexture_max_MoneyIconM_C") as DrawTexture);
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.m_lbDragonHeart_Num = (base.GetControl("Label_DragonHeart_Num") as Label);
		this.m_btMythSkillUp = (base.GetControl("Button_MythSkillup") as Button);
		this.m_lbDragonHeartCost_Num = (base.GetControl("Label_DragonHeartCost_num") as Label);
		this.m_dtDragonHeartM_C = (base.GetControl("DrawTexture_DragonHeartM_C") as DrawTexture);
		this.m_btMythSkillUp.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickMythSkillUpdate));
		if (TsPlatform.IsMobile)
		{
			base.SetScreenCenter();
		}
	}

	public override void InitData()
	{
	}

	private void ShowBattleSkill()
	{
		base.SetShowLayer(3, true);
		base.SetShowLayer(4, false);
		this.Show();
	}

	private void ShowMythSkill()
	{
		base.SetShowLayer(3, false);
		base.SetShowLayer(4, true);
		this.Show();
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
			this.CountMaxNeedMoneyAndSkillLevel();
		}
	}

	public void SetData(ref NkSoldierInfo solinfo, int skillunique, bool bMythSkill)
	{
		this.pkSolinfo = solinfo;
		if (this.pkSolinfo.GetBattleSkillLevel(skillunique) == 0)
		{
		}
		this.m_i32SkillUnique = skillunique;
		this.SetSkillInfo();
		if (bMythSkill)
		{
			this.m_btMythSkillUp.SetEnabled(true);
			this.ShowMythSkill();
			this.SetMythSkillItemInfo();
		}
		else
		{
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			this.ShowBattleSkill();
			this.SetBattleSkillItemInfo();
		}
	}

	private void SetSkillInfo()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.m_i32SkillUnique <= 0)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_i32SkillUnique);
		if (battleSkillBase == null)
		{
			return;
		}
		int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.m_i32SkillUnique);
		bool flag = false;
		int skillLevel;
		if (battleSkillBase.m_nMythSkillType == 0)
		{
			int num = battleSkillLevel + 1;
			int solMaxLevel = (int)this.pkSolinfo.GetSolMaxLevel();
			skillLevel = Math.Min(num, solMaxLevel);
			if (num > solMaxLevel)
			{
				flag = true;
			}
		}
		else
		{
			int num2 = battleSkillLevel + 1;
			int num3 = 4;
			skillLevel = Math.Min(num2, num3);
			if (num2 > num3)
			{
				flag = true;
			}
		}
		string text = string.Empty;
		int num4 = battleSkillLevel;
		UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique);
		this.SkillTargetIcon.SetTexture(battleSkillIconTexture);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1293"),
			"skillname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey)
		});
		this.SkillTargetName.Text = text;
		if (num4 == 0)
		{
			num4 = 1;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1885"),
			"skilllevel",
			num4.ToString()
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
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref text, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, this.pkSolinfo, -1);
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
		if (flag)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1299");
		}
		else
		{
			BATTLESKILL_DETAIL battleSkillDetail2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(battleSkillBase.m_nSkillUnique, skillLevel);
			if (battleSkillDetail2 != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref text, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail2.m_nSkillTooltip), battleSkillDetail2, this.pkSolinfo, -1);
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
	}

	private void SetBattleSkillItemInfo()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.m_i32SkillUnique <= 0)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_i32SkillUnique);
		if (battleSkillBase == null)
		{
			return;
		}
		int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.m_i32SkillUnique);
		BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(this.m_i32SkillUnique, battleSkillLevel + 1);
		if (battleSkillTraining != null)
		{
			this.m_i64NeedItem = (long)battleSkillTraining.m_nSkillNeedGold;
		}
		bool flag = battleSkillLevel < (int)this.pkSolinfo.GetLevel() && battleSkillLevel < battleSkillBase.m_nSkillMaxLevel;
		bool flag2 = false;
		int num = battleSkillLevel + 1;
		int solMaxLevel = (int)this.pkSolinfo.GetSolMaxLevel();
		if (num > solMaxLevel)
		{
			flag2 = true;
		}
		string text = string.Empty;
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
			if (flag2)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("691")
				});
				this.SkillUpdateButton.SetText(text);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2895")
				});
				this.SkillMaxUpdateButton.SetText(text);
				this.SkillUpdateButton.SetEnabled(false);
				this.SkillMaxUpdateButton.SetEnabled(false);
				this.dtSkillMoneyIcon.Visible = false;
				this.dtMaxSkillMoneyIcon.Visible = false;
				this.SkillNeedMoney.Visible = false;
				this.SkillMaxNeedMoney.Visible = false;
			}
			else
			{
				text = ANNUALIZED.Convert(this.m_i64NeedItem);
				this.SkillNeedMoney.Text = text;
				text = ANNUALIZED.Convert(this.m_i64NeedItem);
				this.SkillMaxNeedMoney.SetText(text);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2886"),
					"Level",
					"1"
				});
				this.SkillMaxUpdateButton.SetText(text);
				this.SkillUpdateButton.SetEnabled(false);
				this.SkillMaxUpdateButton.SetEnabled(false);
			}
		}
		else
		{
			text = ANNUALIZED.Convert(this.m_i64NeedItem);
			this.SkillNeedMoney.Text = text;
			this.CountMaxNeedMoneyAndSkillLevel();
		}
		if (this.pkSolinfo != null && this.pkSolinfo.IsSolWarehouse())
		{
			this.SkillMaxUpdateButton.SetEnabled(false);
			this.SkillUpdateButton.SetEnabled(false);
		}
	}

	private void SetMythSkillItemInfo()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.m_i32SkillUnique <= 0)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_i32SkillUnique);
		if (battleSkillBase == null)
		{
			return;
		}
		int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.m_i32SkillUnique);
		MYTHSKILL_TRAINING mythSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetMythSkillTraining(this.m_i32SkillUnique, battleSkillLevel + 1);
		if (mythSkillTraining != null)
		{
			this.m_i64NeedItem = (long)mythSkillTraining.m_i32SkillNeedItem;
		}
		bool flag = battleSkillLevel < (int)(this.pkSolinfo.GetGrade() - 9) && battleSkillLevel < battleSkillBase.m_nSkillMaxLevel;
		bool flag2 = false;
		int num = battleSkillLevel + 1;
		int num2 = 4;
		if (num > num2)
		{
			flag2 = true;
		}
		string text = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTH_SKILL_ITEMUNIQUE);
			int itemCnt = NkUserInventory.instance.GetItemCnt(value);
			text = ANNUALIZED.Convert(itemCnt);
		}
		else
		{
			text = "0";
		}
		this.m_lbDragonHeart_Num.SetText(text);
		if (!flag)
		{
			if (flag2 || this.m_i64NeedItem == 0L)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("691")
				});
				this.m_btMythSkillUp.SetText(text);
				this.m_btMythSkillUp.SetEnabled(false);
				this.m_dtDragonHeartM_C.Visible = false;
				this.m_lbDragonHeartCost_Num.Visible = false;
			}
			else
			{
				text = ANNUALIZED.Convert(this.m_i64NeedItem);
				this.m_lbDragonHeartCost_Num.SetText(text);
				this.m_btMythSkillUp.SetEnabled(false);
			}
		}
		else
		{
			text = ANNUALIZED.Convert(this.m_i64NeedItem);
			this.m_lbDragonHeartCost_Num.SetText(text);
		}
		if (this.pkSolinfo != null && this.pkSolinfo.IsSolWarehouse())
		{
			this.m_btMythSkillUp.SetEnabled(false);
		}
	}

	private void SetSkillAndMoneyInfo()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.m_i32SkillUnique <= 0)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_i32SkillUnique);
		if (battleSkillBase == null)
		{
			return;
		}
		int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.m_i32SkillUnique);
		if (battleSkillBase.m_nMythSkillType == 0)
		{
			BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(this.m_i32SkillUnique, battleSkillLevel + 1);
			this.m_i64NeedItem = (long)battleSkillTraining.m_nSkillNeedGold;
		}
		else
		{
			MYTHSKILL_TRAINING mythSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetMythSkillTraining(this.m_i32SkillUnique, battleSkillLevel + 1);
			this.m_i64NeedItem = (long)mythSkillTraining.m_i32SkillNeedItem;
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
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref text, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail.m_nSkillTooltip), battleSkillDetail, this.pkSolinfo, -1);
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
				NrTSingleton<CTextParser>.Instance.ReplaceBattleSkillParam(ref text, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillDetail2.m_nSkillTooltip), battleSkillDetail2, this.pkSolinfo, -1);
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
			if (flag2)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("691")
				});
				this.SkillUpdateButton.SetText(text);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2895")
				});
				this.SkillMaxUpdateButton.SetText(text);
				this.SkillUpdateButton.SetEnabled(false);
				this.SkillMaxUpdateButton.SetEnabled(false);
				this.dtSkillMoneyIcon.Visible = false;
				this.dtMaxSkillMoneyIcon.Visible = false;
				this.SkillNeedMoney.Visible = false;
				this.SkillMaxNeedMoney.Visible = false;
			}
			else
			{
				text = ANNUALIZED.Convert(this.m_i64NeedItem);
				this.SkillNeedMoney.Text = text;
				text = ANNUALIZED.Convert(this.m_i64NeedItem);
				this.SkillMaxNeedMoney.SetText(text);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2886"),
					"Level",
					"1"
				});
				this.SkillMaxUpdateButton.SetText(text);
				this.SkillUpdateButton.SetEnabled(false);
				this.SkillMaxUpdateButton.SetEnabled(false);
			}
		}
		else
		{
			text = ANNUALIZED.Convert(this.m_i64NeedItem);
			this.SkillNeedMoney.Text = text;
			this.CountMaxNeedMoneyAndSkillLevel();
		}
		if (this.pkSolinfo != null && this.pkSolinfo.IsSolWarehouse())
		{
			this.SkillMaxUpdateButton.SetEnabled(false);
			this.SkillUpdateButton.SetEnabled(false);
		}
	}

	private void CountMaxNeedMoneyAndSkillLevel()
	{
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.m_i32SkillUnique <= 0)
		{
			return;
		}
		if (NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_i32SkillUnique) == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long money = kMyCharInfo.m_Money;
		int level = (int)this.pkSolinfo.GetLevel();
		int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.m_i32SkillUnique);
		if (battleSkillLevel >= (int)this.pkSolinfo.GetSolMaxLevel())
		{
			return;
		}
		int i = battleSkillLevel + 1;
		long num = 0L;
		string text = string.Empty;
		bool flag = false;
		this.nMaxNeedMoney = 0L;
		this.nMaxNeedMoney_real = 0L;
		this.nMaxSkillLevel_real = 0;
		while (i <= level)
		{
			BATTLESKILL_TRAINING battleSkillTraining = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillTraining(this.m_i32SkillUnique, i);
			if (battleSkillTraining != null)
			{
				if (!flag && num + (long)battleSkillTraining.m_nSkillNeedGold == money)
				{
					this.nMaxNeedMoney_real = num + (long)battleSkillTraining.m_nSkillNeedGold;
					this.nMaxSkillLevel_real++;
					flag = true;
				}
				else if (!flag && num + (long)battleSkillTraining.m_nSkillNeedGold > money)
				{
					this.nMaxNeedMoney_real = num;
					flag = true;
				}
				else if (!flag)
				{
					num += (long)battleSkillTraining.m_nSkillNeedGold;
					this.nMaxSkillLevel_real++;
				}
				this.nMaxNeedMoney += (long)battleSkillTraining.m_nSkillNeedGold;
			}
			i++;
		}
		if (!flag)
		{
			this.nMaxNeedMoney_real = num;
		}
		if (this.nMaxNeedMoney_real > 0L && this.nMaxNeedMoney_real == this.nMaxNeedMoney)
		{
			this.m_CheckCanMaxSkillUp = true;
		}
		else
		{
			this.m_CheckCanMaxSkillUp = false;
		}
		if (this.nMaxSkillLevel_real == 0)
		{
			this.nMaxNeedMoney = this.m_i64NeedItem;
			this.nMaxNeedMoney_real = this.m_i64NeedItem;
		}
		text = ANNUALIZED.Convert(this.nMaxNeedMoney);
		this.SkillMaxNeedMoney.SetText(text);
	}

	private void OnClickMaxSkillUpdate(IUIObject obj)
	{
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("547"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		if (this.m_i32SkillUnique <= 0)
		{
			return;
		}
		if (this.pkSolinfo == null)
		{
			return;
		}
		if (this.nMaxNeedMoney_real > this.nHaveMoney)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
			if (lackGold_dlg != null)
			{
				lackGold_dlg.SetData(this.nMaxNeedMoney_real - this.nHaveMoney);
			}
			return;
		}
		if (this.pkSolinfo.GetSolPosType() == 2 || this.pkSolinfo.GetSolPosType() == 6)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("357"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		string empty = string.Empty;
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_i32SkillUnique);
		if (battleSkillBase != null)
		{
			string text = ANNUALIZED.Convert(this.nMaxNeedMoney_real);
			int num = this.pkSolinfo.GetBattleSkillLevel(this.m_i32SkillUnique) + this.nMaxSkillLevel_real;
			if (this.m_CheckCanMaxSkillUp)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("329"),
					"skillname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"level",
					num.ToString(),
					"maxskillgold",
					text
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("330"),
					"skillname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"level",
					num.ToString(),
					"maxskillgold",
					text
				});
			}
			msgBoxUI.SetMsg(new YesDelegate(this.OnSendOKSetBattleSkill), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1223"), empty, eMsgType.MB_OK_CANCEL, 2);
		}
	}

	private void OnClickSkillUpdate(IUIObject obj)
	{
		this.SkillUpdateButton.SetEnabled(false);
		this.SkillMaxUpdateButton.SetEnabled(false);
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("547"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			return;
		}
		if (this.m_i32SkillUnique <= 0 || this.pkSolinfo == null)
		{
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			return;
		}
		if (this.m_i64NeedItem > this.nHaveMoney)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
			if (lackGold_dlg != null)
			{
				lackGold_dlg.SetData(this.m_i64NeedItem - this.nHaveMoney);
			}
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			return;
		}
		if (this.pkSolinfo.GetSolPosType() == 2 || this.pkSolinfo.GetSolPosType() == 6)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("357"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			return;
		}
		GS_SET_SOLDIER_BATTLESKILL_REQ gS_SET_SOLDIER_BATTLESKILL_REQ = new GS_SET_SOLDIER_BATTLESKILL_REQ();
		gS_SET_SOLDIER_BATTLESKILL_REQ.nSolID = this.pkSolinfo.GetSolID();
		gS_SET_SOLDIER_BATTLESKILL_REQ.nBattleSkillUnique = this.m_i32SkillUnique;
		gS_SET_SOLDIER_BATTLESKILL_REQ.nBattleSkillLevel = 0;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_BATTLESKILL_REQ, gS_SET_SOLDIER_BATTLESKILL_REQ);
	}

	private void OnClickMythSkillUpdate(IUIObject obj)
	{
		this.m_btMythSkillUp.SetEnabled(false);
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("547"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			this.m_btMythSkillUp.SetEnabled(true);
			return;
		}
		if (this.m_i32SkillUnique <= 0 || this.pkSolinfo == null)
		{
			this.m_btMythSkillUp.SetEnabled(true);
			return;
		}
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_MYTH_SKILL_ITEMUNIQUE);
		if ((long)NkUserInventory.GetInstance().Get_First_ItemCnt(value) < this.m_i64NeedItem)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("343");
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(value);
			string empty = string.Empty;
			int battleSkillLevel = this.pkSolinfo.GetBattleSkillLevel(this.m_i32SkillUnique);
			BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(this.m_i32SkillUnique);
			if (battleSkillBase == null)
			{
				return;
			}
			string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey);
			if (itemNameByItemUnique != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"itemname",
					itemNameByItemUnique,
					"skillname",
					textFromInterface,
					"skilllevel",
					battleSkillLevel
				});
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromNotify,
					"itemname",
					"None-Itemname",
					"skillname",
					textFromInterface,
					"skilllevel",
					battleSkillLevel
				});
			}
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		else
		{
			if (this.pkSolinfo.GetSolPosType() == 2 || this.pkSolinfo.GetSolPosType() == 6)
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("357"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
				this.m_btMythSkillUp.SetEnabled(true);
				return;
			}
			GS_SET_SOLDIER_MYTH_BATTLESKILL_REQ gS_SET_SOLDIER_MYTH_BATTLESKILL_REQ = new GS_SET_SOLDIER_MYTH_BATTLESKILL_REQ();
			gS_SET_SOLDIER_MYTH_BATTLESKILL_REQ.nSolID = this.pkSolinfo.GetSolID();
			gS_SET_SOLDIER_MYTH_BATTLESKILL_REQ.nBattleSkillUnique = this.m_i32SkillUnique;
			gS_SET_SOLDIER_MYTH_BATTLESKILL_REQ.nBattleSkillLevel = 0;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_MYTH_BATTLESKILL_REQ, gS_SET_SOLDIER_MYTH_BATTLESKILL_REQ);
			return;
		}
	}

	public void OnSendOKSetBattleSkill(object obj)
	{
		this.SkillUpdateButton.SetEnabled(false);
		this.SkillMaxUpdateButton.SetEnabled(false);
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("547"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			return;
		}
		if (this.m_i32SkillUnique <= 0 || this.pkSolinfo == null)
		{
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			return;
		}
		if (this.pkSolinfo.GetSolPosType() == 2 || this.pkSolinfo.GetSolPosType() == 6)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("357"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			this.SkillUpdateButton.SetEnabled(true);
			this.SkillMaxUpdateButton.SetEnabled(true);
			return;
		}
		GS_SET_SOLDIER_BATTLESKILL_REQ gS_SET_SOLDIER_BATTLESKILL_REQ = new GS_SET_SOLDIER_BATTLESKILL_REQ();
		gS_SET_SOLDIER_BATTLESKILL_REQ.nSolID = this.pkSolinfo.GetSolID();
		gS_SET_SOLDIER_BATTLESKILL_REQ.nBattleSkillUnique = this.m_i32SkillUnique;
		gS_SET_SOLDIER_BATTLESKILL_REQ.nBattleSkillLevel = this.nMaxSkillLevel_real;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SET_SOLDIER_BATTLESKILL_REQ, gS_SET_SOLDIER_BATTLESKILL_REQ);
	}
}
