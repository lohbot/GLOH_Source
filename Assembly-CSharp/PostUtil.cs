using GAME;
using System;

public class PostUtil : NrTSingleton<PostUtil>
{
	private PostUtil()
	{
	}

	~PostUtil()
	{
	}

	public string GetSendObjectName(eMAIL_TYPE mailType, string szCharName, bool bHistory, bool bDidISend)
	{
		bool flag = false;
		bool flag2 = mailType > eMAIL_TYPE.MAIL_TYPE_REPORT_BEGIN && mailType < eMAIL_TYPE.MAIL_TYPE_REPORT_END;
		string text = "N/A";
		switch (mailType)
		{
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_GAMEMASTER:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_GAMEMASTER_MESSAGE:
			goto IL_11F;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_MARKET_ITEM_SOLD:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("13");
			goto IL_1C2;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_MARKET_RETURN:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("13");
			flag = true;
			goto IL_1C2;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_QUEST:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(szCharName);
			goto IL_1C2;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_BUY_ITEM:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_QUEST_ITEM:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_BATTLE:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_DAILYEVENT:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_ADD_FRIEND_FACEBOOK:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_PLUNDERAGREE:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_SEND:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_RECV:
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_REWARD:
		case eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT:
			IL_94:
			switch (mailType)
			{
			case eMAIL_TYPE.MAIL_TYPE_PROMOTION_EVENT:
			case eMAIL_TYPE.MAIL_TYPE_CHALLENGE_REWARD:
			case eMAIL_TYPE.MAIL_TYPE_KAKAOFRIEND_EVENT_REWARD:
			case eMAIL_TYPE.MAIL_TYPE_INVITE_EVENT_REWARD:
				goto IL_11F;
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD:
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_REGISTER:
			case eMAIL_TYPE.MAIL_TYPE_AUCTION_HOLD_CANCEL_TENDER:
				goto IL_160;
			case eMAIL_TYPE.MAIL_TYPE_SOLDIERGROUP_RECRUIT:
			case eMAIL_TYPE.MAIL_TYPE_GM_CREATESOL:
			case eMAIL_TYPE.MAIL_TYPE_SOLDIERGROUP_CASH_RECRUIT:
			case eMAIL_TYPE.MAIL_TYPE_INVENTORY_FULL:
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2311");
				goto IL_1C2;
			case eMAIL_TYPE.MAIL_TYPE_INFIBATTLE_REWARD:
			case eMAIL_TYPE.MAIL_TYPE_INFIBATTLE_WEEK_REWARD:
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574");
				goto IL_1C2;
			case eMAIL_TYPE.MAIL_TYPE_BOUNTYHUNT_REWARD:
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2344");
				goto IL_1C2;
			}
			text = szCharName;
			goto IL_1C2;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_EXTRACTED_ITEM:
			goto IL_1C2;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_RETURN:
			flag = true;
			goto IL_1C2;
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_BEFORETENDER:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_TENDER:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_CANCEL:
		case eMAIL_TYPE.MAIL_TYPE_AUCTION_REGISTER_FAIL:
			goto IL_160;
		case eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_GUILD:
			text = szCharName;
			goto IL_1C2;
		}
		goto IL_94;
		IL_11F:
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574");
		goto IL_1C2;
		IL_160:
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1144");
		IL_1C2:
		if (!flag2 && bHistory)
		{
			if (flag)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1539");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					textFromInterface,
					"targetname",
					text
				});
			}
			else if (bDidISend)
			{
				string textFromInterface2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1538");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					textFromInterface2,
					"targetname",
					text
				});
			}
			else
			{
				string text2 = string.Empty;
				if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MARKET_ITEM_SOLD)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1524");
				}
				else
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1537");
				}
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
				{
					text2,
					"targetname",
					text
				});
			}
		}
		else if (flag)
		{
			string textFromInterface3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1539");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromInterface3,
				"targetname",
				text
			});
		}
		else if (bDidISend)
		{
			string textFromInterface4 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1538");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromInterface4,
				"targetname",
				text
			});
		}
		else
		{
			string text3 = string.Empty;
			if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MARKET_ITEM_SOLD)
			{
				text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1524");
			}
			else
			{
				text3 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1537");
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				text3,
				"targetname",
				text
			});
		}
		if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_INVITE_FRIEND)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2084");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_ADD_FRIEND_FACEBOOK)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("362");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_SEND || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_RECV || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_RECOMMEND_REWARD)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1188");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_REPORT_MINE_RESULT || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_GIVEITEM || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_BACKMOVE_GETITEM || mailType == eMAIL_TYPE.MAILTYPE_SYSTEM_MINE_DELMINE_BACK || mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_MINE_FAIL_DEFENGUILD)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1361");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_NEWGUILD_CHANGEMASTER)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1339");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_SUPPORTER)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1918");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_SUPPORTER_REWARD)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1918");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_BABEL_HIDDEN_TREASURE)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1936");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_BABEL_FIRSTREWARD)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1936");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_USER_TO_GUILD)
		{
			text = szCharName;
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_GMSETITEM)
		{
			string textFromInterface5 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1537");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromInterface5,
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574")
			});
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_SYSTEM_SOLDELITEM)
		{
			string textFromInterface6 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1537");
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				textFromInterface6,
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1574")
			});
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_NEWGUILDBOSS_REWARD)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1901");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_EXPEDITION_RESULT_REPORT || mailType == eMAIL_TYPE.MAIL_TYPE_EXPEDITION_GIVE_ITEM)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2428");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_MYTHRAID_ITEM)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3295");
		}
		else if (mailType == eMAIL_TYPE.MAIL_TYPE_INVENTORY_FULL)
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3500");
		}
		return text;
	}

	public short GetAttachIconType(long i64Money, long i64ItemUnique)
	{
		if (i64Money != 0L && i64ItemUnique != 0L)
		{
			return 4;
		}
		if (i64Money != 0L)
		{
			return 1;
		}
		if (i64ItemUnique != 0L)
		{
			return 2;
		}
		return 0;
	}

	public string GetAttachIconTextureName(eMAILBOX_ICONTYPE iconType)
	{
		string result = string.Empty;
		switch (iconType)
		{
		case eMAILBOX_ICONTYPE.ICONTYPE_MONEY:
			result = "Win_I_LetterC05";
			break;
		case eMAILBOX_ICONTYPE.ICONTYPE_ITEM:
			result = "Win_I_LetterC05";
			break;
		case eMAILBOX_ICONTYPE.ICONTYPE_SOL:
			result = "Win_I_LetterC05";
			break;
		case eMAILBOX_ICONTYPE.ICONTYPE_MONEY_ITEM:
			result = "Win_I_LetterC05";
			break;
		default:
			result = "Win_I_LetterC01";
			break;
		}
		return result;
	}

	public string GetAttachIconTextureName(long i64Money, long i64ItemUnique)
	{
		return this.GetAttachIconTextureName((eMAILBOX_ICONTYPE)this.GetAttachIconType(i64Money, i64ItemUnique));
	}
}
