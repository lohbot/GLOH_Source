using PROTOCOL;
using System;
using System.Collections.Generic;

namespace GAME
{
	public class INVITE_PERSONINFO
	{
		public eFRIEND_INVITETYPE eInvte_type;

		public long Invite_PersonID;

		public int Invite_PersonLevel;

		public int Invite_PersonFaceCharKind;

		public string Invite_UserName = string.Empty;

		public string Invite_User_InfoMsg = string.Empty;

		public List<INIVITEPERSON_FRIENDINFO> list_InvitePerson_FriendList = new List<INIVITEPERSON_FRIENDINFO>();

		public void Init()
		{
			this.Invite_PersonID = 0L;
			this.Invite_PersonLevel = 0;
			this.Invite_PersonFaceCharKind = 0;
			this.Invite_UserName = string.Empty;
			this.Invite_User_InfoMsg = string.Empty;
			this.list_InvitePerson_FriendList.Clear();
		}

		public void Set(INVITE_PERSONINFO info)
		{
			this.eInvte_type = info.eInvte_type;
			this.Invite_PersonID = info.Invite_PersonID;
			this.Invite_PersonLevel = info.Invite_PersonLevel;
			this.Invite_PersonFaceCharKind = info.Invite_PersonFaceCharKind;
			this.Invite_UserName = info.Invite_UserName;
			this.Invite_User_InfoMsg = info.Invite_User_InfoMsg;
			foreach (INIVITEPERSON_FRIENDINFO current in info.list_InvitePerson_FriendList)
			{
				this.list_InvitePerson_FriendList.Add(current);
			}
		}
	}
}
