using GAME;
using System;
using System.Collections.Generic;
using UnityForms;

public class DailyDungeon_Difficulty_Dlg : Form
{
	private NewListBox m_nlDifficulty;

	private Button m_btChangeDifficulty;

	private Button m_btClose;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "DailyDungeon/dlg_difficulty_set", G_ID.DAILYDUNGEON_DIFFICULTY, true);
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.draggable = false;
		}
	}

	public override void SetComponent()
	{
		this.m_nlDifficulty = (base.GetControl("NewListBox_difficultylist") as NewListBox);
		this.m_btChangeDifficulty = (base.GetControl("Button_OK") as Button);
		Button expr_32 = this.m_btChangeDifficulty;
		expr_32.Click = (EZValueChangedDelegate)Delegate.Combine(expr_32.Click, new EZValueChangedDelegate(this.OnClickChangeDifficult));
		this.m_btClose = (base.GetControl("Button_Exit") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this._SetDialogPos();
		this.SetDifficultData();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		float width = GUICamera.width;
		float height = GUICamera.height;
		base.SetLocation((width - base.GetSizeX()) / 2f, (height - base.GetSizeY()) / 2f);
	}

	private void SetDifficultData()
	{
		sbyte dayOfWeek = NrTSingleton<DailyDungeonManager>.Instance.GetDayOfWeek();
		Dictionary<sbyte, EVENT_DAILY_DUNGEON_INFO> dailyDungeonInfo = EVENT_DAILY_DUNGEON_DATA.GetInstance().GetDailyDungeonInfo(dayOfWeek);
		if (dailyDungeonInfo == null)
		{
			return;
		}
		this.m_nlDifficulty.Clear();
		foreach (EVENT_DAILY_DUNGEON_INFO current in dailyDungeonInfo.Values)
		{
			if (current != null)
			{
				NewListItem newListItem = new NewListItem(this.m_nlDifficulty.ColumnNum, true, string.Empty);
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1697"),
					"itemname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(current.i32RewardItemUnique),
					"count",
					current.i32RewardItemNum.ToString()
				});
				newListItem.SetListItemData(0, empty, null, null, null);
				UIBaseInfoLoader loader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_I_WorrGradeS" + current.i8Difficulty.ToString());
				newListItem.SetListItemData(1, loader, null, null, null);
				newListItem.SetListItemData(3, new ITEM
				{
					m_nItemUnique = current.i32RewardItemUnique,
					m_nItemNum = current.i32RewardItemNum
				}, null, null, null);
				newListItem.Data = current;
				this.m_nlDifficulty.Add(newListItem);
				this.m_nlDifficulty.RepositionItems();
			}
		}
	}

	public void OnClickChangeDifficult(IUIObject obj)
	{
		if (this.m_nlDifficulty.GetSelectItem() == null)
		{
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnChangeDifficult), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("179"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("180"), eMsgType.MB_OK_CANCEL, 2);
	}

	public void OnChangeDifficult(object a_oObject)
	{
		DailyDungeon_Main_Dlg dailyDungeon_Main_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DAILYDUNGEON_MAIN) as DailyDungeon_Main_Dlg;
		if (dailyDungeon_Main_Dlg == null)
		{
			this.Close();
			return;
		}
		IUIListObject selectItem = this.m_nlDifficulty.GetSelectItem();
		EVENT_DAILY_DUNGEON_INFO eVENT_DAILY_DUNGEON_INFO = selectItem.Data as EVENT_DAILY_DUNGEON_INFO;
		if ((int)dailyDungeon_Main_Dlg.Difficult == (int)eVENT_DAILY_DUNGEON_INFO.i8Difficulty)
		{
			this.Close();
			return;
		}
		dailyDungeon_Main_Dlg.SetDifficuly(eVENT_DAILY_DUNGEON_INFO.i8Difficulty);
		this.Close();
	}
}
