using GAME;
using System;
using UnityForms;

public class PostFriendDlg : Form
{
	private NewListBox m_FriendList;

	private Button m_Confirm;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Post/Dlg_NewPost_Friend", G_ID.POSTFRIEND_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_FriendList = (base.GetControl("NewListBox_friend") as NewListBox);
		this.m_FriendList.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickFriendList));
		this.m_Confirm = (base.GetControl("Button_confirm") as Button);
		this.m_Confirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickConfirm));
		this.SetFriendList();
		base.SetScreenCenter();
	}

	private void SetFriendList()
	{
		this.m_FriendList.Clear();
		foreach (USER_FRIEND_INFO uSER_FRIEND_INFO in NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kFriendInfo.GetFriendInfoValues())
		{
			if (1L > uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID || uSER_FRIEND_INFO.FriendHelpSolInfo.i64HelpSolID >= 11L)
			{
				NewListItem newListItem = new NewListItem(this.m_FriendList.ColumnNum, true);
				string text = TKString.NEWString(uSER_FRIEND_INFO.szName);
				newListItem.SetListItemData(0, text, null, null, null);
				newListItem.SetListItemData(1, uSER_FRIEND_INFO.i16Level.ToString(), null, null, null);
				if (0 < uSER_FRIEND_INFO.i8Location)
				{
					if (0 < uSER_FRIEND_INFO.i8UserPlayState)
					{
						newListItem.SetListItemData(2, "Win_I_Comm01", null, null, null);
					}
					else
					{
						newListItem.SetListItemData(2, "Win_I_Comm03", null, null, null);
					}
				}
				else
				{
					newListItem.SetListItemData(2, "Win_I_Comm02", null, null, null);
				}
				newListItem.Data = text;
				this.m_FriendList.Add(newListItem);
			}
		}
		this.m_FriendList.RepositionItems();
	}

	private void ClickFriendList(IUIObject ojb)
	{
	}

	private void ClickConfirm(IUIObject ojb)
	{
		UIListItemContainer selectedItem = this.m_FriendList.SelectedItem;
		if (null == selectedItem)
		{
			return;
		}
		string strName = (string)selectedItem.Data;
		PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
		if (postDlg != null)
		{
			postDlg.ConfirmRequestByName(strName);
		}
		FriendPush_DLG friendPush_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.FRIEND_PUSH_DLG) as FriendPush_DLG;
		if (friendPush_DLG != null)
		{
			friendPush_DLG.ConfirmRequestByName(strName);
		}
		this.Close();
	}
}
