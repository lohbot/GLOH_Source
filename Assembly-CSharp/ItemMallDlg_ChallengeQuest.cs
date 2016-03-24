using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityForms;

public class ItemMallDlg_ChallengeQuest : ItemMallDlg
{
	private int _challengeQuestUnique = -1;

	public int _ChallengeQuestUnique
	{
		get
		{
			return this._challengeQuestUnique;
		}
		set
		{
			this._challengeQuestUnique = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/dlg_itemmall", G_ID.ITEMMALL_CHALLENGEQUEST_DLG, true);
		base.ShowBlackBG(1f);
		base.SetScreenCenter();
	}

	public override void OnClose()
	{
		this._challengeQuestUnique = -1;
		base.OnClose();
	}

	public override void OnClickBtHeartsState(IUIObject obj)
	{
	}

	public override void OnClickBtGoldState(IUIObject obj)
	{
	}

	public override void OnClickMode(IUIObject obj)
	{
	}

	public override void SetShowMode(ItemMallDlg.eMODE tabMode)
	{
		base.SetShowMode(tabMode);
		this.DisableUnableTab();
	}

	public void InitDummyUI()
	{
		List<UIListItemContainer> list = new List<UIListItemContainer>();
		for (int i = 0; i < this.m_nlbVoucherPremium.Count; i++)
		{
			UIListItemContainer item = this.m_nlbVoucherPremium.GetItem(i);
			if (!(item == null))
			{
				ITEM_MALL_ITEM iTEM_MALL_ITEM = (ITEM_MALL_ITEM)item.Data;
				if (iTEM_MALL_ITEM != null)
				{
					if (iTEM_MALL_ITEM.m_Idx != 9010L)
					{
						list.Add(item);
					}
				}
			}
		}
		foreach (UIListItemContainer current in list)
		{
			this.m_nlbVoucherPremium.RemoveItem(current, true);
		}
	}

	public void SuccessDirectionEnd()
	{
		this.ShowGameGuideDlg();
		GS_RECOMMEND_CHALLENGE_CLEAR_REQ gS_RECOMMEND_CHALLENGE_CLEAR_REQ = new GS_RECOMMEND_CHALLENGE_CLEAR_REQ();
		gS_RECOMMEND_CHALLENGE_CLEAR_REQ.i32RecommendChallengeUnique = 1500;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_RECOMMEND_CHALLENGE_CLEAR_REQ, gS_RECOMMEND_CHALLENGE_CLEAR_REQ);
	}

	public void MsgBoxOKEvent()
	{
		SOLDIER_INFO sOLDIER_INFO = new SOLDIER_INFO();
		sOLDIER_INFO.CharKind = 1004;
		sOLDIER_INFO.Grade = 5;
		sOLDIER_INFO.SolID = 1234567L;
		sOLDIER_INFO.HP = 10000;
		NkSoldierInfo nkSoldierInfo = new NkSoldierInfo();
		nkSoldierInfo.SetCharKind(1004);
		NrReceiveGame.SolRecruitAfter(sOLDIER_INFO, null, 1, 21, null, false, nkSoldierInfo);
	}

	public static void OnShowHelpDlg_Game()
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_New.ToString());
		}
	}

	public override void OnClickTooltip(IUIObject obj)
	{
	}

	private void DisableUnableTab()
	{
		for (int i = 0; i <= 2; i++)
		{
			if (i != (int)this.m_eMode)
			{
				if (this.m_tbTab.Control_Tab.Length <= i)
				{
					break;
				}
				this.m_tbTab.Control_Tab[i].controlIsEnabled = false;
			}
		}
	}

	private void ShowGameGuideDlg()
	{
		NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.NONE, GameGuideType.CHALLENGE_PREMIUM_ONE);
		NrTSingleton<GameGuideManager>.Instance.Update();
		OnCloseCallback callback = null;
		if (this._challengeQuestUnique == 1500)
		{
			callback = new OnCloseCallback(ItemMallDlg_ChallengeQuest.OnShowHelpDlg_Game);
		}
		GameGuideDlg gameGuideDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAMEGUIDE_DLG) as GameGuideDlg;
		gameGuideDlg.RegistCloseCallback(callback);
	}
}
