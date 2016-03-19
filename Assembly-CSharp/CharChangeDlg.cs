using Ndoors.Memory;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CharChangeDlg : Form
{
	private DrawTexture m_dtMainBG;

	private DrawTexture m_dtCharBG;

	private DrawTexture m_dtShadowBG;

	private Button m_btPrev;

	private Button m_btNext;

	private Label m_lbClass;

	private DrawTexture m_dtWeapon1;

	private DrawTexture m_dtWeapon2;

	private Label m_lbInfo;

	private Button m_btMovie;

	private DrawTexture m_dtSkillIcon1;

	private Label m_lbSkillName1;

	private DrawTexture m_dtSkillIcon2;

	private Label m_lbSkillName2;

	private Label m_lbMoney;

	private Button m_btBack;

	private Button m_btChange;

	private E_CHAR_TRIBE m_eCharTribe;

	private E_CHAR_TRIBE[] m_eCharTribeList;

	private int m_iSelectIndex = -1;

	private long m_lNeedMoney;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Char/dlg_charchange", G_ID.CHARCHANGE_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_eCharTribeList = new E_CHAR_TRIBE[4];
		this.m_dtMainBG = (base.GetControl("DrawTexture_DrawTexture26") as DrawTexture);
		this.m_dtCharBG = (base.GetControl("BT_DrawTexture25") as DrawTexture);
		this.m_dtShadowBG = (base.GetControl("BT_DrawTexture_CharShadow1") as DrawTexture);
		string text = string.Format("UI/charselect/ChShadow" + NrTSingleton<UIDataManager>.Instance.AddFilePath, new object[0]);
		Texture2D texture2D = ResourceCache.GetResource(text) as Texture2D;
		if (null == texture2D)
		{
			CharChangeMainDlg.RequestDownload(text, new PostProcPerItem(CharChangeMainDlg._OnImageProcess), this.m_dtShadowBG);
		}
		else
		{
			CharChangeMainDlg.SetTexture(this.m_dtShadowBG, texture2D);
		}
		this.m_btPrev = (base.GetControl("BT_Back") as Button);
		this.m_btPrev.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_btNext = (base.GetControl("BT_Next") as Button);
		this.m_btNext.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_lbClass = (base.GetControl("LB_Title") as Label);
		this.m_dtWeapon1 = (base.GetControl("DT_Weapon01") as DrawTexture);
		this.m_dtWeapon2 = (base.GetControl("DT_Weapon02") as DrawTexture);
		this.m_lbInfo = (base.GetControl("LB_Info") as Label);
		this.m_btMovie = (base.GetControl("BT_Movie") as Button);
		this.m_btMovie.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMovie));
		this.m_dtSkillIcon1 = (base.GetControl("DT_SkillIcon01") as DrawTexture);
		this.m_lbSkillName1 = (base.GetControl("LB_SkillName01") as Label);
		this.m_dtSkillIcon2 = (base.GetControl("DT_SkillIcon02") as DrawTexture);
		this.m_lbSkillName2 = (base.GetControl("LB_SkillName02") as Label);
		this.m_lbMoney = (base.GetControl("LB_Gold") as Label);
		charSpend charSpend = NrTSingleton<NrBaseTableManager>.Instance.GetCharSpend(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetLevel().ToString());
		if (charSpend != null)
		{
			this.m_lNeedMoney = charSpend.lCharChangeGold;
		}
		this.m_lbMoney.SetText(ANNUALIZED.Convert(this.m_lNeedMoney));
		this.m_btBack = (base.GetControl("Button_Button15") as Button);
		this.m_btBack.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickBack));
		this.m_btChange = (base.GetControl("BT_Change") as Button);
		this.m_btChange.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickChange));
		this.m_eCharTribeList[0] = E_CHAR_TRIBE.FURRY;
		this.m_eCharTribeList[1] = E_CHAR_TRIBE.ELF;
		this.m_eCharTribeList[2] = E_CHAR_TRIBE.HUMAN;
		this.m_eCharTribeList[3] = E_CHAR_TRIBE.HUMANF;
		base.SetScreenCenter();
	}

	public void SetSelectCharKind(E_CHAR_TRIBE eCharTribe)
	{
		this.m_eCharTribe = eCharTribe;
		this.m_iSelectIndex = this.GetSelectIndex(this.m_eCharTribe);
		this.SelectCharInfo(this.m_eCharTribe);
	}

	public void ClickPrev(IUIObject obj)
	{
		if (0 > this.m_iSelectIndex - 1)
		{
			this.m_iSelectIndex = 3;
		}
		else
		{
			this.m_iSelectIndex--;
		}
		this.m_eCharTribe = this.m_eCharTribeList[this.m_iSelectIndex];
		this.SelectCharInfo(this.m_eCharTribe);
	}

	public void ClickNext(IUIObject obj)
	{
		if (4 <= this.m_iSelectIndex + 1)
		{
			this.m_iSelectIndex = 0;
		}
		else
		{
			this.m_iSelectIndex++;
		}
		this.m_eCharTribe = this.m_eCharTribeList[this.m_iSelectIndex];
		this.SelectCharInfo(this.m_eCharTribe);
	}

	public void ClickMovie(IUIObject obj)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(CharChangeMainDlg.GetChangeCharKindFormIndex(this.m_eCharTribe));
		if (charKindInfo == null)
		{
			return;
		}
		string sOLINTRO = charKindInfo.GetCHARKIND_INFO().SOLINTRO;
		if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				string str = string.Format("{0}SOLINTRO/", Option.GetProtocolRootPath(Protocol.HTTP));
				NmMainFrameWork.PlayMovieURL(str + sOLINTRO + ".mp4", true, false);
			}
			else
			{
				NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/SOLINTRO/" + sOLINTRO + ".mp4", true, false);
			}
		}
		else
		{
			string str2 = string.Format("{0}SOLINTRO/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			NmMainFrameWork.PlayMovieURL(str2 + sOLINTRO + ".mp4", true, false);
		}
	}

	public void ClickBack(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.CHARCHANGE_DLG);
	}

	public void ClickChange(IUIObject obj)
	{
		CharChangeMainDlg charChangeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHARCHANGEMAIN_DLG) as CharChangeMainDlg;
		if (charChangeMainDlg == null || charChangeMainDlg.SetClassChange(this.m_eCharTribe))
		{
		}
	}

	public void SelectCharInfo(E_CHAR_TRIBE eCharTribe)
	{
		int changeCharKindFormIndex = CharChangeMainDlg.GetChangeCharKindFormIndex(eCharTribe);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(changeCharKindFormIndex);
		if (charKindInfo == null)
		{
			return;
		}
		this.SetMainTexture(eCharTribe);
		CharChangeMainDlg.SetCharImage(this.m_dtCharBG, changeCharKindFormIndex);
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(charKindInfo.GetCHARKIND_STATINFO().kBattleSkillData[0].BattleSkillUnique);
		if (battleSkillBase != null)
		{
			this.m_dtSkillIcon1.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique));
			this.m_lbSkillName1.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey));
		}
		BATTLESKILL_BASE battleSkillBase2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(charKindInfo.GetCHARKIND_STATINFO().kBattleSkillData[1].BattleSkillUnique);
		if (battleSkillBase2 != null)
		{
			this.m_dtSkillIcon2.SetTexture(NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase2.m_nSkillUnique));
			this.m_lbSkillName2.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase2.m_strTextKey));
		}
		switch (eCharTribe)
		{
		case E_CHAR_TRIBE.HUMAN:
			this.m_dtWeapon1.SetTextureKey("Win_I_Weapon1");
			this.m_dtWeapon2.SetTextureKey("Win_I_Weapon2");
			this.m_lbClass.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50006"));
			this.m_lbInfo.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50011"));
			break;
		case E_CHAR_TRIBE.FURRY:
			this.m_dtWeapon1.SetTextureKey("Win_I_Weapon3");
			this.m_dtWeapon2.SetTextureKey("Win_I_Weapon6");
			this.m_lbClass.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50004"));
			this.m_lbInfo.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50008"));
			break;
		case E_CHAR_TRIBE.ELF:
			this.m_dtWeapon1.SetTextureKey("Win_I_Weapon7");
			this.m_dtWeapon2.SetTextureKey("Win_I_Weapon8");
			this.m_lbClass.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50007"));
			this.m_lbInfo.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50009"));
			break;
		case E_CHAR_TRIBE.HUMANF:
			this.m_dtWeapon1.SetTextureKey("Win_I_Weapon4");
			this.m_dtWeapon2.SetTextureKey("Win_I_Weapon5");
			this.m_lbClass.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50005"));
			this.m_lbInfo.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromCharInfo("50010"));
			break;
		}
	}

	public int GetSelectIndex(E_CHAR_TRIBE eCharTribe)
	{
		for (int i = 0; i < 4; i++)
		{
			if (this.m_eCharTribeList[i] == eCharTribe)
			{
				return i;
			}
		}
		return -1;
	}

	public void SetMainTexture(E_CHAR_TRIBE eCharTribe)
	{
		string text = string.Empty;
		switch (eCharTribe)
		{
		case E_CHAR_TRIBE.HUMAN:
		case E_CHAR_TRIBE.HUMANF:
			text = string.Format("UI/charselect/HumanBG" + NrTSingleton<UIDataManager>.Instance.AddFilePath, new object[0]);
			break;
		case E_CHAR_TRIBE.FURRY:
			text = string.Format("UI/charselect/FurryBG" + NrTSingleton<UIDataManager>.Instance.AddFilePath, new object[0]);
			break;
		case E_CHAR_TRIBE.ELF:
			text = string.Format("UI/charselect/FairyBG" + NrTSingleton<UIDataManager>.Instance.AddFilePath, new object[0]);
			break;
		}
		if (string.Empty != text)
		{
			Texture2D texture2D = ResourceCache.GetResource(text) as Texture2D;
			if (null == texture2D)
			{
				CharChangeMainDlg.RequestDownload(text, new PostProcPerItem(CharChangeMainDlg._OnImageProcess), this.m_dtMainBG);
			}
			else
			{
				CharChangeMainDlg.SetTexture(this.m_dtMainBG, texture2D);
			}
		}
	}
}
