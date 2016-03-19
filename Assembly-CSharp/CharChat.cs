using System;

public class CharChat
{
	private TsWeakReference<NrCharBase> m_RefChar;

	public string msg = string.Empty;

	public CHAT_TYPE MsgType;

	public NrCharBase Character
	{
		get
		{
			return this.m_RefChar;
		}
	}

	public CharChat(NrCharBase charBase, string msg, CHAT_TYPE _ChatType)
	{
		this.m_RefChar = charBase;
		this.msg = msg;
		this.MsgType = _ChatType;
	}
}
