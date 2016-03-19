using System;
using UnityEngine;
using UnityForms;

public class GameGuideMinePlunder : GameGuideInfo
{
	private int _itemunique;

	public override void Init()
	{
		base.Init();
	}

	public override void InitData()
	{
		this.m_nCheckTime = Time.realtimeSinceStartup;
	}

	public void SetInfo(int itemunique)
	{
		this._itemunique = itemunique;
	}

	public override void ExcuteGameGuide()
	{
		NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.POST_DLG);
		PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
		if (postDlg != null)
		{
			postDlg.ChangeTab_RecvList();
		}
		this.InitData();
	}

	public void OpenUI()
	{
	}

	public override bool CheckGameGuideOnce()
	{
		return true;
	}

	public override bool CheckGameGuide()
	{
		return this.CheckGameGuideOnce();
	}

	public override string GetGameGuideText()
	{
		string textFromToolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip(this.m_strTalkKey);
		string empty = string.Empty;
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this._itemunique);
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromToolTip,
			"targetname",
			itemNameByItemUnique
		});
		return empty;
	}
}
