using System;

public class ChatMsg
{
	public string Name = string.Empty;

	public string Msg = string.Empty;

	public int Color;

	public ChatMsg(string name, string msg, int color)
	{
		this.Name = name;
		this.Msg = msg;
		this.Color = color;
	}
}
