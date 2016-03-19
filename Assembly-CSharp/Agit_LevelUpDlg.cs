using GAME;
using System;
using UnityForms;

public class Agit_LevelUpDlg : Form
{
	private enum eTYPE
	{
		eTYPE_VISIT_NPC,
		eTYPE_MONSTER_NUM,
		eTYPE_MONSTER_LEVEL,
		eTYPE_MERCHANT_RATE,
		eTYPE_VISIT_MERCHANT,
		eTYPE_MAX
	}

	private const int TEXT_KEY = 2690;

	private Label m_lbNote;

	private Button[] m_btType = new Button[5];

	private Label m_lbTypeInfo;

	private Button m_btOK;

	private Button m_btCancel;

	private string m_strText = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "NewGuild/Agit/DLG_Agit_levelup", G_ID.AGIT_LEVELUP_DLG, true);
	}

	public override void SetComponent()
	{
		short agitLevel = NrTSingleton<NewGuildManager>.Instance.GetAgitLevel();
		short num = agitLevel + 1;
		this.m_lbNote = (base.GetControl("Label_Note") as Label);
		AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(agitLevel.ToString());
		if (agitData != null)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strText, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2697"),
				"gold",
				agitData.i64NeedGuildFund
			});
			this.m_lbNote.SetText(this.m_strText);
		}
		else
		{
			this.m_lbNote.SetText(string.Empty);
		}
		AgitInfoData agitData2 = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(num.ToString());
		for (int i = 0; i < 5; i++)
		{
			this.m_strText = string.Format("Button_level{0}", i + 1);
			this.m_btType[i] = (base.GetControl(this.m_strText) as Button);
			this.m_btType[i].TabIndex = i;
			this.m_btType[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickType));
		}
		if (agitData != null && agitData2 != null)
		{
			if (agitData.i8NPCNum >= agitData2.i8NPCNum)
			{
				this.m_btType[0].Visible = false;
			}
			if (agitData.i8MonsterNum >= agitData2.i8MonsterNum)
			{
				this.m_btType[1].Visible = false;
			}
			if (agitData.i16MonsterMaxLevel >= agitData2.i16MonsterMaxLevel)
			{
				this.m_btType[2].Visible = false;
			}
			if (agitData.i16MerchantRate >= agitData2.i16MerchantRate)
			{
				this.m_btType[3].Visible = false;
			}
			if (agitData.i8MerchantVisitNum >= agitData2.i8MerchantVisitNum)
			{
				this.m_btType[4].Visible = false;
			}
		}
		this.m_lbTypeInfo = (base.GetControl("Label_levelup") as Label);
		this.m_btOK = (base.GetControl("Button_ok") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_btCancel = (base.GetControl("Button_cancel") as Button);
		this.m_btCancel.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickCancel));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	public void ClickType(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Button button = obj as Button;
		if (button == null)
		{
			return;
		}
		int num = 2690 + button.TabIndex;
		this.m_lbTypeInfo.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(num.ToString()));
	}

	public void ClickOK(IUIObject obj)
	{
		AgitInfoData agitData = NrTSingleton<NrBaseTableManager>.Instance.GetAgitData(NrTSingleton<NewGuildManager>.Instance.GetAgitLevel().ToString());
		if (agitData == null)
		{
			return;
		}
		if (NrTSingleton<NewGuildManager>.Instance.GetFund() < agitData.i64NeedGuildFund)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("754"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID);
		if (memberInfoFromPersonID == null)
		{
			return;
		}
		if (memberInfoFromPersonID.GetRank() < NewGuildDefine.eNEWGUILD_MEMBER_RANK.eNEWGUILD_MEMBER_RANK_OFFICER)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("769"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		NrTSingleton<NewGuildManager>.Instance.Send_GS_NEWGUILD_AGIT_LEVEL_REQ();
		this.Close();
	}

	public void ClickCancel(IUIObject obj)
	{
		this.Close();
	}
}
