using System;
using UnityEngine;
using UnityForms;

public class ChatTabDefine
{
	public string[] TabKey = new string[]
	{
		"TabKeyNormal",
		"TabKeyAlliance",
		"TabKeyParty",
		"TabKeyArea"
	};

	public int[] i32ChatKind = new int[]
	{
		1,
		2,
		4,
		8,
		16
	};

	public string[] TabName = new string[]
	{
		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("450"),
		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("17"),
		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("459"),
		NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("355")
	};

	public int i32CheckTabChat = 65535;

	public void UserLoadChatTab()
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
		if (form != null)
		{
			int selectTap = ChatManager.GetSelectTap();
			if (selectTap < 0)
			{
				return;
			}
			if (PlayerPrefs.HasKey(this.TabKey[selectTap] + "TapName"))
			{
				string @string = PlayerPrefs.GetString(this.TabKey[selectTap] + "TapName");
				if (@string != string.Empty)
				{
					this.TabName[selectTap] = @string;
				}
			}
			else
			{
				this.TabName[0] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("450");
				this.TabName[1] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("17");
				this.TabName[2] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("459");
				this.TabName[3] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("355");
			}
			if (PlayerPrefs.HasKey(this.TabKey[selectTap] + "Kind"))
			{
				this.i32CheckTabChat = PlayerPrefs.GetInt(this.TabKey[selectTap] + "Kind");
			}
			else if (selectTap != 0)
			{
				this.i32CheckTabChat = this.i32ChatKind[selectTap];
			}
			else
			{
				this.i32CheckTabChat = 65535;
			}
		}
	}

	public void UserSaveChatTab(int i32Index)
	{
		string value = this.TabName[i32Index];
		PlayerPrefs.SetString(this.TabKey[i32Index] + "TapName", value);
		PlayerPrefs.SetInt(this.TabKey[i32Index] + "Kind", this.i32CheckTabChat);
		value = PlayerPrefs.GetString(this.TabKey[i32Index] + "TapName");
	}

	public void UserReSetChatTab(int i32Index)
	{
		PlayerPrefs.DeleteKey(this.TabKey[i32Index] + "TapName");
		PlayerPrefs.DeleteKey(this.TabKey[i32Index] + "Kind");
	}
}
