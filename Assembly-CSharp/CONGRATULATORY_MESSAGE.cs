using PROTOCOL.GAME;
using System;

public class CONGRATULATORY_MESSAGE
{
	public short m_nMsgType;

	public long m_nPersonID;

	public char[] char_name = new char[21];

	public short level;

	public int m_nItemUnique;

	public int m_nItemNum;

	public int[] i32params = new int[6];

	public long i64Param;

	public char[] szparam1 = new char[21];

	public char[] szparam2 = new char[21];

	public char[] szparam3 = new char[21];

	public void Set(short type, GS_CONGRATULATORY_MESSAGE_NFY nfy)
	{
		this.m_nMsgType = type;
		this.m_nPersonID = nfy.m_nPersonID;
		this.char_name = nfy.char_name;
		this.level = nfy.level;
		this.m_nItemUnique = nfy.m_nItemUnique;
		this.m_nItemNum = nfy.m_nItemNum;
		for (int i = 0; i < 5; i++)
		{
			this.i32params[i] = nfy.i32params[i];
		}
	}
}
