using System;
using System.Collections;
using TsBundle;
using UnityEngine;
using UnityForms;

public class MsgBoxAutoSellUI : Form
{
	public enum eMODE
	{
		BABEL_TOWER,
		NEWEXPLORATION
	}

	private MsgBoxAutoSellUI.eMODE m_eMode;

	private Button m_btnSave;

	private DropDownList m_DropListEquipment;

	private DropDownList m_DropListItmeRank;

	private bool m_bCheckItemUser;

	private bool m_bCheckFastBattle;

	private int m_iAutoSell_Rank;

	private int m_iAutoSell_Grade;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Message/DLG_AutoSell", G_ID.MSGBOX_AUTOSELL_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_DropListEquipment = (base.GetControl("DropDownList_Equipment") as DropDownList);
		this.m_DropListItmeRank = (base.GetControl("DropDownList_ItemRank") as DropDownList);
		this.m_btnSave = (base.GetControl("Button_Save") as Button);
		Button expr_48 = this.m_btnSave;
		expr_48.Click = (EZValueChangedDelegate)Delegate.Combine(expr_48.Click, new EZValueChangedDelegate(this.OnClickSave));
		this.Initialize();
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
		base.DonotDepthChange(90f);
		this.m_DropListEquipment.SetParentList(G_ID.MSGBOX_AUTOSELL_DLG, true);
		this.m_DropListItmeRank.SetParentList(G_ID.MSGBOX_AUTOSELL_DLG, true);
		base.SetScreenCenter();
		this.Hide();
	}

	public void Initialize()
	{
		this.m_DropListEquipment.AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_Equipment));
		this.m_DropListItmeRank.AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_ItmeRank));
		this.m_iAutoSell_Rank = PlayerPrefs.GetInt(NrPrefsKey.AUTOSELLRANK, 0);
		this.m_iAutoSell_Grade = PlayerPrefs.GetInt(NrPrefsKey.AUTOSELLGRADE, 0);
		this.SetEquipment();
		this.SetItemRank();
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_COMMON", "WINDOW", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		this.m_bCheckItemUser = false;
		this.m_bCheckFastBattle = false;
	}

	private void SetEquipment()
	{
		this.m_DropListEquipment.Clear();
		string str = string.Empty;
		ICollection autoSell = NrTSingleton<NrBaseTableManager>.Instance.GetAutoSell();
		if (autoSell != null)
		{
			foreach (AutoSell_info autoSell_info in autoSell)
			{
				ListItem listItem = new ListItem();
				listItem.Key = autoSell_info.i32SellNumber;
				str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(autoSell_info.i32ItemTextKey.ToString());
				listItem.SetColumnStr(0, str);
				this.m_DropListEquipment.Add(listItem);
			}
		}
		this.m_DropListEquipment.RepositionItems();
		if (this.m_iAutoSell_Grade < 1)
		{
			this.m_iAutoSell_Grade = 1;
		}
		this.m_DropListEquipment.SetIndex(this.m_iAutoSell_Grade - 1);
	}

	private void GetRankTextKey(string strTextKey, byte hKey)
	{
		ListItem listItem = new ListItem();
		listItem.Key = hKey;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(strTextKey.ToString())
		});
		listItem.SetColumnStr(0, empty);
		this.m_DropListItmeRank.Add(listItem);
	}

	private void SetItemRank()
	{
		this.m_DropListItmeRank.Clear();
		for (int i = 0; i < 6; i++)
		{
			this.GetRankTextKey((3056 + i).ToString(), (byte)(i + 1));
		}
		this.m_DropListItmeRank.RepositionItems();
		if (this.m_iAutoSell_Rank < 1)
		{
			this.m_iAutoSell_Rank = 1;
		}
		this.m_DropListItmeRank.SetIndex(this.m_iAutoSell_Rank - 1);
	}

	private void Change_Equipment(IUIObject obj)
	{
		ListItem listItem = this.m_DropListEquipment.SelectedItem.Data as ListItem;
		if (listItem != null)
		{
			this.m_iAutoSell_Grade = (int)listItem.Key;
		}
	}

	private void Change_ItmeRank(IUIObject obj)
	{
		ListItem listItem = this.m_DropListItmeRank.SelectedItem.Data as ListItem;
		if (listItem != null)
		{
			this.m_iAutoSell_Rank = (int)((byte)listItem.Key);
		}
	}

	public void OnClickSave(IUIObject obj)
	{
		PlayerPrefs.SetInt(NrPrefsKey.AUTOSELLGRADE, this.m_iAutoSell_Grade);
		PlayerPrefs.SetInt(NrPrefsKey.AUTOSELLRANK, this.m_iAutoSell_Rank);
		MsgBoxAutoSellUI.eMODE eMode = this.m_eMode;
		if (eMode != MsgBoxAutoSellUI.eMODE.BABEL_TOWER)
		{
			if (eMode == MsgBoxAutoSellUI.eMODE.NEWEXPLORATION)
			{
				NrTSingleton<NewExplorationManager>.Instance.SetAutoBattle(true, this.m_bCheckItemUser, this.m_bCheckFastBattle);
				NrTSingleton<NewExplorationManager>.Instance.SetAutoBatch();
				if (!NrTSingleton<NewExplorationManager>.Instance.Send_GS_NEWEXPLORATION_START_REQ())
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("889"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
				}
			}
		}
		else
		{
			NrTSingleton<NkBabelMacroManager>.Instance.Start(this.m_bCheckItemUser, this.m_bCheckFastBattle);
		}
		this.Close();
	}

	public void SetLoadData(bool bCheckItemUse, bool bCheckFastBattle, MsgBoxAutoSellUI.eMODE eLoadedMode)
	{
		this.m_bCheckItemUser = bCheckItemUse;
		this.m_bCheckFastBattle = bCheckFastBattle;
		this.m_eMode = eLoadedMode;
		this.Show();
	}

	public void SetButtonSaveText(string strText)
	{
		this.m_btnSave.SetText(strText);
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_COMMON", "WINDOW", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
