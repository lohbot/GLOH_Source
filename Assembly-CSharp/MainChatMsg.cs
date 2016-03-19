using GAME;
using System;

public class MainChatMsg
{
	public CHAT_TYPE type;

	public string name = string.Empty;

	public string msg = string.Empty;

	public string color = string.Empty;

	public ITEM linkItem = new ITEM();

	public MainChatMsg(CHAT_TYPE type, string name, string msg, ITEM linkItem = null, string color = "")
	{
		this.SetData(type, name, msg, linkItem, color);
	}

	private void SetData(CHAT_TYPE type, string name, string msg, ITEM linkItem, string color)
	{
		this.type = type;
		this.name = ChatManager.GetChatFrontString(name, type);
		this.msg = msg;
		this.linkItem = linkItem;
		if (string.Empty != color && string.Empty != color)
		{
			this.color = color;
		}
		else
		{
			this.color = MainChatDlg.GetChatColorKey(type);
		}
	}
}
