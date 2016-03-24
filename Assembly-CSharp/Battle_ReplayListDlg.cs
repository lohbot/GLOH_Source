using System;
using System.IO;
using UnityForms;

public class Battle_ReplayListDlg : Form
{
	private NewListBox m_nlbFileList;

	private Button m_btReplay;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_replaylist", G_ID.BATTLE_REPLAY_LIST_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_nlbFileList = (base.GetControl("replay_filelist") as NewListBox);
		this.m_nlbFileList.Clear();
		this.m_btReplay = (base.GetControl("Button_Replay") as Button);
		Button expr_3D = this.m_btReplay;
		expr_3D.Click = (EZValueChangedDelegate)Delegate.Combine(expr_3D.Click, new EZValueChangedDelegate(this.OnClickReplay));
		this.SetFileList();
		this._SetDialogPos();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, GUICamera.height / 2f - base.GetSizeY() / 2f);
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void SetFileList()
	{
		string directory = NrTSingleton<NkBattleReplayManager>.Instance.Directory;
		string[] files = Directory.GetFiles(directory, "*.dat");
		for (int i = 0; i < files.Length; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_nlbFileList.ColumnNum, true, string.Empty);
			string text = files[i].Replace(directory, string.Empty);
			newListItem.SetListItemData(0, text, null, null, null);
			newListItem.Data = text;
			this.m_nlbFileList.Add(newListItem);
			this.m_nlbFileList.RepositionItems();
		}
	}

	public void OnClickReplay(IUIObject obj)
	{
		IUIObject selectItem = this.m_nlbFileList.GetSelectItem();
		if (selectItem != null)
		{
			string text = selectItem.Data as string;
			if (text != null)
			{
				NrTSingleton<NkBattleReplayManager>.Instance.LoadFile = text;
				NrTSingleton<NkBattleReplayManager>.Instance.ReplayStart();
			}
		}
		this.Close();
	}
}
