using GAME;
using System;
using UnityForms;

public class PlunderTargetInfoDlg : Form
{
	private Label m_lCharName;

	private Label m_lCharLevel;

	private Label m_lGold;

	private Label m_lbRank;

	private Label m_lbStraightWin;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/dlg_pvp_opponentinfo", G_ID.PLUNDERTARGETINFO_DLG, false);
		base.ChangeSceneDestory = false;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lCharName = (base.GetControl("Label_ChaName") as Label);
		this.m_lCharLevel = (base.GetControl("Label_ChaLv") as Label);
		this.m_lGold = (base.GetControl("Label_GoldLabel") as Label);
		this.m_lbRank = (base.GetControl("Label_rank2") as Label);
		this.m_lbRank.SetText(string.Empty);
		this.m_lbStraightWin = (base.GetControl("Label_win2") as Label);
		this.m_lbStraightWin.SetText(string.Empty);
		float y = 55f;
		float x = GUICamera.width - base.GetSize().x;
		base.SetLocation(x, y, base.GetLocation().z);
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PLUNDER)
		{
			base.ShowLayer(1);
		}
		else if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_INFIBATTLE || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PRACTICE_INFIBATTLE)
		{
			base.ShowLayer(2);
		}
	}

	public override void InitData()
	{
	}

	public void SetTargetInfo(string strCharName, int nLevel, long nGold)
	{
		this.m_lCharName.Text = strCharName;
		this.m_lCharLevel.Text = "Lv. " + nLevel.ToString();
		this.m_lGold.SetText(ANNUALIZED.Convert(nGold));
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null)
		{
			kMyCharInfo.PlunderMoney = nGold;
			kMyCharInfo.PlunderCharName = strCharName;
			kMyCharInfo.PlunderCharLevel = nLevel;
		}
	}

	public void SetTargetInfoInfiBattle(bool bTargetShow, string strEnemyName, short iLevel, int iRank, int iStraightWin)
	{
		if (bTargetShow)
		{
			this.m_lCharName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3176");
			this.m_lCharLevel.SetText(string.Empty);
			this.m_lbRank.SetText(string.Empty);
			this.m_lbStraightWin.SetText(string.Empty);
			COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
			string empty = string.Empty;
			if (instance != null)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("850"),
					"time",
					instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_INFIBATTLE_BLIND_MATCH_TIME)
				});
				Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
		else
		{
			this.m_lCharName.Text = strEnemyName;
			this.m_lCharLevel.Text = "Lv. " + iLevel.ToString();
			string text = string.Empty;
			if (0 < iRank)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1413"),
					"rank",
					iRank
				});
			}
			else
			{
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
			}
			this.m_lbRank.SetText(text);
			if (0 <= iStraightWin)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2221"),
					"count",
					iStraightWin
				});
				this.m_lbStraightWin.SetText(text);
			}
			if (iLevel == 0)
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCharLevel = 1;
			}
			else
			{
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.InfiBattleCharLevel = (int)iLevel;
			}
		}
		SoldierBatch.SOLDIERBATCH.SetEnemyUserName(this.m_lCharName.Text);
	}
}
