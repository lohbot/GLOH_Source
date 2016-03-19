using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class QuestList_ChapterInfo_GameDramaView_DLG : Form
{
	private Label m_Quest_Info_Name;

	private Toolbar m_Quest_Info_ToolBar;

	private DrawTexture m_QuestChapter_Pic;

	private Label m_QuestChapter_Story;

	private QUESTLIST_TAB_MODE2 m_eMode;

	private CQuestGroup m_CurQuestGroup;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/DLG_QuestList_ChapterInfo_GameDramaView", G_ID.QUESTLIST_CHAPTERINFO_GAMEDRAMAVIEW_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_Quest_Info_Name = (base.GetControl("Quest_Info_Name") as Label);
		this.m_Quest_Info_ToolBar = (base.GetControl("Quest_Info_ToolBar") as Toolbar);
		this.m_QuestChapter_Story = (base.GetControl("QuestChapter_Story") as Label);
		this.m_QuestChapter_Pic = (base.GetControl("QuestChapter_Pic") as DrawTexture);
		this.m_Quest_Info_ToolBar.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1196");
		this.m_Quest_Info_ToolBar.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1197");
		this.m_Quest_Info_ToolBar.FirstSetting();
		UIPanelTab expr_B2 = this.m_Quest_Info_ToolBar.Control_Tab[0];
		expr_B2.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_B2.ButtonClick, new EZValueChangedDelegate(this.OnTabClick));
		UIPanelTab expr_E0 = this.m_Quest_Info_ToolBar.Control_Tab[1];
		expr_E0.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_E0.ButtonClick, new EZValueChangedDelegate(this.OnTabClick));
		base.ShowLayer(1);
	}

	private void SetSummary()
	{
		base.ShowLayer(1);
		if (this.m_CurQuestGroup == null)
		{
			return;
		}
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("667");
		string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("666");
		this.m_Quest_Info_Name.Text = string.Concat(new string[]
		{
			this.m_CurQuestGroup.GetChapterUnique().ToString(),
			textFromInterface,
			" ",
			this.m_CurQuestGroup.GetPage(),
			textFromInterface2,
			" ",
			this.m_CurQuestGroup.GetGroupTitle()
		});
		this.m_QuestChapter_Story.Text = string.Empty;
		this.RequestDownload();
	}

	private void SetDrama()
	{
		base.ShowLayer(2);
		this.m_QuestChapter_Pic.Visible = false;
	}

	public void SetQuest(CQuestGroup kQuestGroup, QUESTLIST_TAB_MODE2 eMode)
	{
		this.m_CurQuestGroup = kQuestGroup;
		this.m_Quest_Info_ToolBar.Control_Tab[(int)eMode].Value = true;
	}

	private void OnTabClick(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		if (uIPanelTab.panel.index == (int)this.m_eMode)
		{
			return;
		}
		this.m_eMode = (QUESTLIST_TAB_MODE2)uIPanelTab.panel.index;
		QUESTLIST_TAB_MODE2 eMode = this.m_eMode;
		if (eMode != QUESTLIST_TAB_MODE2.QuestSummary)
		{
			if (eMode == QUESTLIST_TAB_MODE2.Drama)
			{
				this.SetDrama();
			}
		}
		else
		{
			this.SetSummary();
		}
	}

	public bool RequestDownload()
	{
		WWWItem wWWItem = Holder.TryGetOrCreateBundle("BUNDLE/Map/BF_Map2DImage" + Option.extAsset, null);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this._OnCompleteDownload), null);
		if (wWWItem != null)
		{
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, false);
			return true;
		}
		return false;
	}

	private void _OnCompleteDownload(IDownloadedItem wItem, object obj)
	{
		if (wItem != null && wItem.canAccessAssetBundle)
		{
			Texture2D texture = wItem.GetSafeBundle().Load("BF_fieldwar_001", typeof(Texture2D)) as Texture2D;
			this.m_QuestChapter_Pic.SetTexture(texture);
		}
	}
}
