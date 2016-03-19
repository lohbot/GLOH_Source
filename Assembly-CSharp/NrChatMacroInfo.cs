using PROTOCOL;
using System;

public class NrChatMacroInfo
{
	private string[] m_htChatMacroList = new string[8];

	public NrChatMacroInfo()
	{
		for (int i = 0; i < 8; i++)
		{
			this.m_htChatMacroList[i] = string.Empty;
		}
	}

	public void Init()
	{
	}

	public void AddChatMacro(CHATMACRO kChatMacroInfo)
	{
		if (kChatMacroInfo.i32ChatMacroIndex >= 8)
		{
			return;
		}
		string text = new string(kChatMacroInfo.szChatMacro);
		this.m_htChatMacroList[kChatMacroInfo.i32ChatMacroIndex] = text;
	}

	public string GetChatMacro(int i32ChatMacroIndex)
	{
		if (i32ChatMacroIndex >= 8)
		{
			return null;
		}
		return this.m_htChatMacroList[i32ChatMacroIndex];
	}
}
