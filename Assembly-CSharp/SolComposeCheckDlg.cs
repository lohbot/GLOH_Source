using PROTOCOL;
using PROTOCOL.GAME;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class SolComposeCheckDlg : Form
{
	private DrawTexture dtSoldier;

	private Label lbName;

	private Label lbLevel;

	private Label lbSubNum;

	private Label lbGold;

	private Button btnOk;

	private Label lbTitle;

	private Label lbExplain;

	private Label lbMoneyName;

	private NkSoldierInfo mBaseSolInfo;

	private List<long> mSubSolList;

	private SOLCOMPOSE_TYPE m_SolType;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/dlg_solcomposecheck", G_ID.SOLCOMPOSE_CHECK_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.dtSoldier = (base.GetControl("DT_SolImg") as DrawTexture);
		this.lbName = (base.GetControl("Label_SolName") as Label);
		this.lbLevel = (base.GetControl("Label_SolLevel") as Label);
		this.lbSubNum = (base.GetControl("Label_SolNum") as Label);
		this.lbGold = (base.GetControl("Label_Gold") as Label);
		this.btnOk = (base.GetControl("BT_Compose") as Button);
		Button expr_8A = this.btnOk;
		expr_8A.Click = (EZValueChangedDelegate)Delegate.Combine(expr_8A.Click, new EZValueChangedDelegate(this.BtnClickOk));
		this.lbTitle = (base.GetControl("Label_Title") as Label);
		this.lbExplain = (base.GetControl("Label_Explain") as Label);
		this.lbMoneyName = (base.GetControl("Label_ComposeText") as Label);
		base.SetScreenCenter();
	}

	public void SetData(NkSoldierInfo kBase, List<long> kSubList, SOLCOMPOSE_TYPE _Type = SOLCOMPOSE_TYPE.COMPOSE)
	{
		if (SolComposeMainDlg.Instance == null)
		{
			base.CloseNow();
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		int kind = 0;
		this.mBaseSolInfo = kBase;
		this.mSubSolList = kSubList;
		this.m_SolType = _Type;
		NkSoldierInfo nkSoldierInfo = null;
		if (this.mSubSolList != null || this.mSubSolList.Count != 0)
		{
			foreach (long current in this.mSubSolList)
			{
				NkSoldierInfo soldierInfo = SolComposeMainDlg.GetSoldierInfo(current);
				if (nkSoldierInfo == null)
				{
					nkSoldierInfo = soldierInfo;
				}
				else if (nkSoldierInfo.GetCombatPower() < soldierInfo.GetCombatPower())
				{
					nkSoldierInfo = soldierInfo;
				}
			}
			if (nkSoldierInfo != null)
			{
				kind = nkSoldierInfo.GetCharKind();
				text = nkSoldierInfo.GetName();
				text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1031");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text2,
					"count",
					nkSoldierInfo.GetLevel()
				});
			}
			int num = this.mSubSolList.Count - 1;
			string text3 = string.Empty;
			text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2034");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text3, new object[]
			{
				text3,
				"count",
				num
			});
			this.lbName.SetText(text);
			this.lbLevel.SetText(text2);
			this.dtSoldier.SetTexture(eCharImageType.SMALL, kind, (int)nkSoldierInfo.GetGrade());
			this.lbSubNum.SetText(text3);
			this.lbSubNum.Visible = (0 < num);
			this.lbGold.SetText(string.Format("{0:###,###,###,##0}", SolComposeMainDlg.Instance.COST));
			SOLCOMPOSE_TYPE solType = this.m_SolType;
			if (solType != SOLCOMPOSE_TYPE.COMPOSE)
			{
				if (solType == SOLCOMPOSE_TYPE.SELL)
				{
					this.lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("57"));
					this.lbExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("58"));
					this.lbMoneyName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("34"));
					this.btnOk.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("35"));
				}
			}
			else
			{
				this.lbTitle.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2033"));
				this.lbExplain.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1717"));
				this.lbMoneyName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1728"));
				this.btnOk.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1729"));
			}
			return;
		}
		this.Close();
	}

	private void BtnClickOk(IUIObject obj)
	{
		if (this.m_SolType == SOLCOMPOSE_TYPE.COMPOSE)
		{
			if (this.mBaseSolInfo != null && this.mSubSolList != null && 0 < this.mSubSolList.Count)
			{
				byte b = (byte)Mathf.Min(50, this.mSubSolList.Count);
				GS_SOLDIERS_UPGRADE_REQ gS_SOLDIERS_UPGRADE_REQ = new GS_SOLDIERS_UPGRADE_REQ();
				gS_SOLDIERS_UPGRADE_REQ.i64BaseSolID = this.mBaseSolInfo.GetSolID();
				gS_SOLDIERS_UPGRADE_REQ.i8Cnt = b;
				for (int i = 0; i < (int)b; i++)
				{
					gS_SOLDIERS_UPGRADE_REQ.i64SubSolID[i] = this.mSubSolList[i];
				}
				SendPacket.GetInstance().SendObject(23, gS_SOLDIERS_UPGRADE_REQ);
				if (SolComposeMainDlg.Instance != null)
				{
					SolComposeMainDlg.Instance.ClearList();
				}
			}
		}
		else if (this.m_SolType == SOLCOMPOSE_TYPE.SELL && this.mSubSolList != null && 0 < this.mSubSolList.Count)
		{
			byte b2 = (byte)Mathf.Min(50, this.mSubSolList.Count);
			GS_SOLDIERS_SELL_REQ gS_SOLDIERS_SELL_REQ = new GS_SOLDIERS_SELL_REQ();
			gS_SOLDIERS_SELL_REQ.i8Cnt = (byte)this.mSubSolList.Count;
			gS_SOLDIERS_SELL_REQ.i8Cnt = b2;
			for (int j = 0; j < (int)b2; j++)
			{
				gS_SOLDIERS_SELL_REQ.i64SolID[j] = this.mSubSolList[j];
			}
			SendPacket.GetInstance().SendObject(128, gS_SOLDIERS_SELL_REQ);
			if (SolComposeMainDlg.Instance != null)
			{
				SolComposeMainDlg.Instance.ClearList();
			}
		}
		this.Close();
	}

	private void BtnClickCancel(IUIObject obj)
	{
		this.Close();
	}
}
