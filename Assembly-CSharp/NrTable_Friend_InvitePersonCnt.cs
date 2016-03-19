using System;
using TsLibs;

public class NrTable_Friend_InvitePersonCnt : NrTableBase
{
	public NrTable_Friend_InvitePersonCnt(string a_strFilePath) : base(a_strFilePath)
	{
	}

	public override bool ParseDataFromNDT(TsDataReader dr)
	{
		foreach (TsDataReader.Row data in dr)
		{
			Friend_InvitePersonCnt friend_InvitePersonCnt = new Friend_InvitePersonCnt();
			friend_InvitePersonCnt.SetData(data);
			Friend_InvitePersonManager.Get_Instance().Add(friend_InvitePersonCnt);
		}
		return true;
	}
}
