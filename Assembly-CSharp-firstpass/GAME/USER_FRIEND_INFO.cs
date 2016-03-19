using System;

namespace GAME
{
	public class USER_FRIEND_INFO
	{
		public long nPersonID;

		public char[] szName = new char[21];

		public short i16Level;

		public byte i8Location;

		public int i32MapUnique;

		public int i32FriendWorldID;

		public int i32WorldID_Connect;

		public int i32FaceCharKind;

		public byte ui8HelpUse;

		public FRIEND_HELPSOLINFO FriendHelpSolInfo = new FRIEND_HELPSOLINFO();

		public short i16BattleMatch;

		public byte i8UserPlayState;

		public char[] szFaceBookID = new char[65];

		public char[] szPlatformName = new char[21];

		public long i64LogoutTIme;

		public char[] szGuildName = new char[11];

		public short i16ColosseumGrade;

		public byte ui8UserPortrait;

		public void Update(USER_FRIEND_INFO _user_friend_info)
		{
			TKString.CharChar(_user_friend_info.szName, ref this.szName);
			this.i32FaceCharKind = _user_friend_info.i32FaceCharKind;
			this.i32FriendWorldID = _user_friend_info.i32FriendWorldID;
			this.i32WorldID_Connect = _user_friend_info.i32WorldID_Connect;
			this.i16Level = _user_friend_info.i16Level;
			this.i32MapUnique = _user_friend_info.i32MapUnique;
			this.i8Location = _user_friend_info.i8Location;
			this.i8UserPlayState = _user_friend_info.i8UserPlayState;
			this.i16BattleMatch = _user_friend_info.i16BattleMatch;
			this.ui8HelpUse = _user_friend_info.ui8HelpUse;
			this.i64LogoutTIme = _user_friend_info.i64LogoutTIme;
			this.szGuildName = _user_friend_info.szGuildName;
			this.i16ColosseumGrade = _user_friend_info.i16ColosseumGrade;
		}
	}
}
