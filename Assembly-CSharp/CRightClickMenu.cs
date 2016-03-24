using GAME;
using Ndoors.Framework.Stage;
using NPatch;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class CRightClickMenu : NrTSingleton<CRightClickMenu>
{
	public enum TYPE
	{
		NONE,
		SIMPLE_SECTION_1,
		SIMPLE_SECTION_2,
		SIMPLE_SECTION_3,
		NAME_SECTION_1,
		NAME_SECTION_2,
		NAME_SECTION_3,
		NAME
	}

	public enum KIND
	{
		NONE,
		USER_CLICK,
		MONSTER_CLICK,
		COMMUNITY_FRIEND_SAME_SV_CLICK,
		COMMUNITY_FRIEND_DIFF_SV_CLICK,
		COMMUNITY_FRIEND_LOGOFF,
		COMMUNITY_ALLY,
		OTHER_FRIEND_SAME_SV_CLICK,
		OTHER_FRIEND_DIFF_SV_CLICK,
		OTHER_FRIEND_LOGOFF,
		ITEM_USABLE_CLICK,
		ITEM_EQUIP_CLICK,
		ITEM_NOTEQUIP_CLICK,
		ITEM_UNUSABLE_CLICK,
		ITEM_COMPOSE_CLICK,
		WHISPERUSERLIST_CLICK,
		PARTY_OWN,
		PARTY_LEADER_OTHER,
		PARTY_MEMBER_OTHER,
		PARTYRECOMMENDLIST_CLICK,
		CHAT_USER_LINK_TEXT,
		GUILD_MASTER_CLICK,
		GUILD_MASTER_SELECT_CLICK,
		GUILD_SUBMASTER_CLICK,
		GUILD_SUBMASTER_SELECT_CLICK,
		GUILD_MEMBER_CLICK
	}

	public enum CLOSEOPTION
	{
		NONE,
		CLICK,
		MOUSE_OUT,
		COUNT
	}

	public delegate void _OnClickMenu(object data);

	public const int MAX_SECTION_COUNT = 3;

	public const int SECTION_1 = 0;

	public const int SECTION_2 = 1;

	public const int SECTION_3 = 2;

	public const float LIST_MOBILE_SIZE = 180f;

	public const float LIST_MOBILE_SIZE_ITEM = 240f;

	public const float LIST_WEB_SIZE = 90f;

	private CRightClickMenu.KIND m_CurFormKind;

	private bool[] m_bCloseOption = new bool[3];

	private long m_nPersonID;

	private short m_i16CharUnique;

	private string m_szCharName;

	private bool m_isGuildUser;

	private object m_oItem;

	private Rect m_rcWindow = new Rect(0f, 0f, 0f, 0f);

	private static float MouseOutClosestartTime = Time.realtimeSinceStartup;

	private static float MouseOutCloseTime = 0f;

	private static string BASIC_FONT_COLOR = NrTSingleton<CTextParser>.Instance.GetTextColor("1002");

	private CRightClickMenu()
	{
		this.Init();
	}

	~CRightClickMenu()
	{
	}

	private void Init()
	{
		this.m_CurFormKind = CRightClickMenu.KIND.NONE;
		this.m_nPersonID = 0L;
		this.m_i16CharUnique = 0;
		for (int i = 0; i < 3; i++)
		{
			this.m_bCloseOption[i] = false;
		}
	}

	public bool Initialize()
	{
		return true;
	}

	public void Update()
	{
		this.InputMouseEvent();
		if (this.m_bCloseOption[2])
		{
			CRightClickMenu.MouseOutCloseTime = Time.realtimeSinceStartup;
			float x = 1f / GUICamera.width * NkInputManager.mousePosition.x * GUICamera.width;
			float num = 1f / GUICamera.height * NkInputManager.mousePosition.y * GUICamera.height;
			Vector2 point = new Vector2(x, GUICamera.height - num);
			UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
			if (uI_RightClickMenu != null)
			{
				Rect rect = new Rect(uI_RightClickMenu.GetLocation().x, uI_RightClickMenu.GetLocationY(), uI_RightClickMenu.GetSizeX(), uI_RightClickMenu.GetSizeY());
				if (uI_RightClickMenu.Visible && rect.Contains(point))
				{
					CRightClickMenu.MouseOutClosestartTime = Time.realtimeSinceStartup;
				}
				if (this.m_rcWindow.Contains(point))
				{
					CRightClickMenu.MouseOutClosestartTime = Time.realtimeSinceStartup;
				}
				if (CRightClickMenu.MouseOutCloseTime - CRightClickMenu.MouseOutClosestartTime > 1f)
				{
					uI_RightClickMenu.Close();
				}
			}
		}
	}

	public void SetSelectChar()
	{
	}

	public bool CreateUI(long personid, short charunique, string charname, CRightClickMenu.KIND formKind, CRightClickMenu.TYPE formType, bool isGuildUser = false)
	{
		this.CloseUI();
		this.m_nPersonID = personid;
		this.m_i16CharUnique = charunique;
		this.m_szCharName = charname;
		this.m_isGuildUser = isGuildUser;
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return false;
		}
		uI_RightClickMenu.SetControl(formType);
		uI_RightClickMenu.SetName(charname);
		this.m_CurFormKind = formKind;
		this.MakeList();
		return true;
	}

	public bool CreateUI(ITEM itemdata, CRightClickMenu.KIND formKind, CRightClickMenu.TYPE formType)
	{
		this.CloseUI();
		this.m_oItem = itemdata;
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null)
		{
			uI_RightClickMenu.SetControl(formType);
		}
		this.m_CurFormKind = formKind;
		this.MakeList();
		if (TsPlatform.IsMobile)
		{
			uI_RightClickMenu.SetName(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemdata));
		}
		return true;
	}

	public void SetLocation(Vector2 pos)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		uI_RightClickMenu.SetLocation(pos);
	}

	public void SetTwinForm(G_ID gid)
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(gid);
		if (form != null)
		{
			form.InteractivePanel.twinFormID = G_ID.DLG_RIGHTCLICK_MENU;
		}
	}

	private void CloseUI()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_RIGHTCLICK_MENU);
		this.Init();
	}

	public bool IsOpen()
	{
		return null != NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU);
	}

	public void CloseUI(CRightClickMenu.KIND formKind)
	{
		if (this.m_CurFormKind == formKind)
		{
			this.CloseUI();
		}
	}

	public void CloseUI(CRightClickMenu.CLOSEOPTION occurEvent)
	{
		if (this.m_bCloseOption[(int)occurEvent])
		{
			this.CloseUI();
		}
	}

	public void InputMouseEvent()
	{
		bool flag = false;
		if (TsPlatform.IsWeb)
		{
			if (NkInputManager.GetMouseButtonUp(0))
			{
				flag = true;
				G_ID g_ID = (G_ID)NrTSingleton<FormsManager>.Instance.MouseOverFormID();
				if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm() && g_ID == G_ID.DLG_RIGHTCLICK_MENU)
				{
					flag = false;
				}
			}
			else if (NkInputManager.GetMouseButtonUp(1))
			{
				if (NkRaycast.Raycast(100f, TsLayer.EVERYTHING))
				{
					RaycastHit hIT = NkRaycast.HIT;
					if (hIT.collider.gameObject != null)
					{
						NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(hIT.collider.gameObject);
						if (@char != null)
						{
							flag = false;
						}
					}
				}
				if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
				{
					flag = false;
				}
			}
		}
		else if (TsPlatform.IsMobile && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		{
			flag = true;
			if (NkRaycast.Raycast(100f, TsLayer.EVERYTHING))
			{
				RaycastHit hIT2 = NkRaycast.HIT;
				if (hIT2.collider.gameObject != null)
				{
					NrCharBase char2 = NrTSingleton<NkCharManager>.Instance.GetChar(hIT2.collider.gameObject);
					if (char2 != null)
					{
						flag = false;
					}
				}
			}
			if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
			{
				flag = false;
			}
		}
		if (flag)
		{
			NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		}
	}

	public void MakeList()
	{
		switch (this.m_CurFormKind)
		{
		case CRightClickMenu.KIND.USER_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			if (Launcher.Instance.LocalPatchLevel == Launcher.Instance.PatchLevelMax)
			{
				this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("22"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickFight));
			}
			this.AddList(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickAddFriend));
			if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() > 0L && NrTSingleton<NkCharManager>.Instance.GetCharName() != this.m_szCharName && NrTSingleton<NewGuildManager>.Instance.IsInviteMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID) && !this.m_isGuildUser)
			{
				this.AddList(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("467"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickInviteGuild));
			}
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.MONSTER_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickMonDetailInfo));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.COMMUNITY_FRIEND_SAME_SV_CLICK:
		case CRightClickMenu.KIND.COMMUNITY_FRIEND_DIFF_SV_CLICK:
		case CRightClickMenu.KIND.COMMUNITY_ALLY:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() > 0L && NrTSingleton<NkCharManager>.Instance.GetCharName() != this.m_szCharName && NrTSingleton<NewGuildManager>.Instance.IsInviteMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID) && !this.m_isGuildUser)
			{
				this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("467"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickInviteGuild));
			}
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("22"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickFight));
			this.AddList(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickFriendDel));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.COMMUNITY_FRIEND_LOGOFF:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() > 0L && NrTSingleton<NkCharManager>.Instance.GetCharName() != this.m_szCharName && NrTSingleton<NewGuildManager>.Instance.IsInviteMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID) && !this.m_isGuildUser)
			{
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("467"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickInviteGuild));
			}
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickFriendDel));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.OTHER_FRIEND_SAME_SV_CLICK:
		case CRightClickMenu.KIND.OTHER_FRIEND_DIFF_SV_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickAddFriend));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.OTHER_FRIEND_LOGOFF:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.ITEM_USABLE_CLICK:
			if (TsPlatform.IsMobile)
			{
				this.SizeSet(240f);
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowItemInfo));
			}
			else
			{
				this.SizeSet(90f);
			}
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickItemUseEquip));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickCancel));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.ITEM_EQUIP_CLICK:
			if (TsPlatform.IsMobile)
			{
				this.SizeSet(240f);
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowItemInfo));
			}
			else
			{
				this.SizeSet(90f);
			}
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickItemReleaseEquip));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickCancel));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.ITEM_NOTEQUIP_CLICK:
			if (TsPlatform.IsMobile)
			{
				this.SizeSet(240f);
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowItemInfo));
			}
			else
			{
				this.SizeSet(90f);
			}
			if (!this.addReforge() && !this.addPostdlg())
			{
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("626"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickItemUseEquip));
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("261"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickItemEnhance));
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("704"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickItemSell));
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickCancel));
			}
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.ITEM_UNUSABLE_CLICK:
			if (TsPlatform.IsMobile)
			{
				this.SizeSet(180f);
				this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowItemInfo));
			}
			else
			{
				this.SizeSet(90f);
			}
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.WHISPERUSERLIST_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickAddFriend));
			this.AddList(2, "차단", NrTSingleton<CTextParser>.Instance.GetTextColor("1102"), true, new CRightClickMenu._OnClickMenu(this.ClickChatBlock));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.PARTY_OWN:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("356"), CRightClickMenu.BASIC_FONT_COLOR, false, new CRightClickMenu._OnClickMenu(this.OnTROPHY), 1);
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("319"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.OnLEAVE));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.PARTY_LEADER_OTHER:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickAddFriend));
			this.AddList(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("349"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.OnCHANGELEADER));
			this.AddList(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("350"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.OnKICKOUT));
			this.AddList(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("356"), CRightClickMenu.BASIC_FONT_COLOR, false, new CRightClickMenu._OnClickMenu(this.OnTROPHY), 2);
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.PARTY_MEMBER_OTHER:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickAddFriend));
			this.AddList(2, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("356"), CRightClickMenu.BASIC_FONT_COLOR, false, new CRightClickMenu._OnClickMenu(this.OnTROPHY), 2);
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.CHAT_USER_LINK_TEXT:
		{
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("464"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickAddFriend));
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (NrTSingleton<NewGuildManager>.Instance.GetGuildID() > 0L && NrTSingleton<NewGuildManager>.Instance.IsInviteMember(kMyCharInfo.m_PersonID))
			{
				this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("467"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickInviteGuild));
			}
			short num = (short)COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHATLIMIT_LEVEL);
			if (kMyCharInfo.GetLevel() > (int)num)
			{
				this.AddList(1, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2577"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickReportUser));
			}
			this.m_bCloseOption[1] = true;
			break;
		}
		case CRightClickMenu.KIND.GUILD_MASTER_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1833"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickNewGuildRankChange));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1308"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickNewGuildMemberKickOut));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.GUILD_MASTER_SELECT_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1833"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickNewGuildRankChange));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.GUILD_SUBMASTER_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1833"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickNewGuildRankChange));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1308"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickNewGuildMemberKickOut));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.GUILD_SUBMASTER_SELECT_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.m_bCloseOption[1] = true;
			break;
		case CRightClickMenu.KIND.GUILD_MEMBER_CLICK:
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("229"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickShowDetailInfo));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("575"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickWhisper));
			this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("354"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickPostSend));
			this.m_bCloseOption[1] = true;
			break;
		}
		this.Rearrange();
	}

	private void AddList(int section, string name, string color, bool bCloseUIAfterCallback, CRightClickMenu._OnClickMenu callbackFunc)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return;
		}
		uI_RightClickMenu.AddListMenu(section, name, color, bCloseUIAfterCallback, callbackFunc);
	}

	private void AddList(int section, string name, string color, bool bCloseUIAfterCallback, CRightClickMenu._OnClickMenu callbackFunc, object data)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return;
		}
		uI_RightClickMenu.AddListMenu(section, name, color, bCloseUIAfterCallback, callbackFunc, data);
	}

	private void Rearrange()
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return;
		}
		uI_RightClickMenu.Rearrange();
	}

	private void SizeSet(float width)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return;
		}
		uI_RightClickMenu.SizeSet(width);
	}

	private NrCharBase GetChar()
	{
		NrCharBase nrCharBase = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique(this.m_i16CharUnique);
		if (nrCharBase == null)
		{
			nrCharBase = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(this.m_nPersonID);
		}
		return nrCharBase;
	}

	public void SetWindowRect(Rect _rect)
	{
		this.m_rcWindow = _rect;
	}

	public Vector3 GetPosition()
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null)
		{
			return uI_RightClickMenu.GetLocation();
		}
		return Vector3.zero;
	}

	public void SetPosition(float x, float y)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return;
		}
		uI_RightClickMenu.SetPosition(x, y);
	}

	public float GetSizeWidth()
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return 0f;
		}
		return uI_RightClickMenu.GetSizeX();
	}

	public void ClickShowDetailInfo(object data)
	{
		GS_OTHERCHAR_INFO_PERMIT_REQ gS_OTHERCHAR_INFO_PERMIT_REQ = new GS_OTHERCHAR_INFO_PERMIT_REQ();
		gS_OTHERCHAR_INFO_PERMIT_REQ.nPersonID = this.m_nPersonID;
		TKString.StringChar(this.m_szCharName.Trim(), ref gS_OTHERCHAR_INFO_PERMIT_REQ.szCharName);
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_OTHERCHAR_INFO_PERMIT_REQ, gS_OTHERCHAR_INFO_PERMIT_REQ);
	}

	public void ClickWhisper(object data)
	{
		bool flag = false;
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null && uI_RightClickMenu.GetTileCharName() != null)
		{
			if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.COMMUNITY_DLG))
			{
				CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
				if (communityUI_DLG != null && communityUI_DLG.RoomUnique != 0)
				{
					GS_WHISPER_INVITE_REQ gS_WHISPER_INVITE_REQ = new GS_WHISPER_INVITE_REQ();
					gS_WHISPER_INVITE_REQ.RoomUnique = communityUI_DLG.RoomUnique;
					TKString.StringChar(uI_RightClickMenu.GetTileCharName(), ref gS_WHISPER_INVITE_REQ.Name);
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_INVITE_REQ, gS_WHISPER_INVITE_REQ);
					flag = true;
				}
			}
			if (!flag)
			{
				GS_WHISPER_REQ gS_WHISPER_REQ = new GS_WHISPER_REQ();
				gS_WHISPER_REQ.RoomUnique = 0;
				TKString.StringChar(uI_RightClickMenu.GetTileCharName(), ref gS_WHISPER_REQ.Name);
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WHISPER_REQ, gS_WHISPER_REQ);
				NrTSingleton<WhisperManager>.Instance.MySendRequest = true;
			}
		}
	}

	public void ClickFollow(object data)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser.IsCharStateAtb(1024L))
		{
			return;
		}
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null && uI_RightClickMenu.GetTileCharName() != null)
		{
			if (nrCharUser.GetCharName().CompareTo(uI_RightClickMenu.GetTileCharName()) == 0)
			{
				string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("358");
				Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
				return;
			}
			if (nrCharUser != null && uI_RightClickMenu != null && this.m_nPersonID > 0L)
			{
				nrCharUser.SetFollowCharPersonID(this.m_nPersonID, uI_RightClickMenu.GetTileCharName());
			}
			else if (!NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bRequestFollowCharPos)
			{
				GS_FOLLOWCHAR_REQ gS_FOLLOWCHAR_REQ = new GS_FOLLOWCHAR_REQ();
				gS_FOLLOWCHAR_REQ.nPersonID = -1L;
				TKString.StringChar(uI_RightClickMenu.GetTileCharName(), ref gS_FOLLOWCHAR_REQ.Name);
				SendPacket.GetInstance().SendObject(914, gS_FOLLOWCHAR_REQ);
				NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_bRequestFollowCharPos = true;
			}
		}
	}

	public void ClickFight(object data)
	{
		NrCharBase @char = this.GetChar();
		if (@char != null)
		{
			if (!@char.IsCharStateAtb(16384L) || !@char.IsCharStateAtb(32768L))
			{
				GS_BATTLE_FIGHT_ALLOW_REQ gS_BATTLE_FIGHT_ALLOW_REQ = new GS_BATTLE_FIGHT_ALLOW_REQ();
				TKString.StringChar(@char.GetCharName(), ref gS_BATTLE_FIGHT_ALLOW_REQ.szCharName);
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_FIGHT_ALLOW_REQ, gS_BATTLE_FIGHT_ALLOW_REQ);
			}
		}
		else
		{
			GS_BATTLE_FIGHT_ALLOW_REQ gS_BATTLE_FIGHT_ALLOW_REQ2 = new GS_BATTLE_FIGHT_ALLOW_REQ();
			TKString.StringChar(this.m_szCharName, ref gS_BATTLE_FIGHT_ALLOW_REQ2.szCharName);
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_FIGHT_ALLOW_REQ, gS_BATTLE_FIGHT_ALLOW_REQ2);
		}
	}

	public void ClickBattleApply(object data)
	{
		NrCharBase @char = this.GetChar();
		if (@char != null && @char.IsCharStateAtb(16384L))
		{
			GS_BATTLE_INTRUSION_REQ gS_BATTLE_INTRUSION_REQ = new GS_BATTLE_INTRUSION_REQ();
			gS_BATTLE_INTRUSION_REQ.i16CharUnique = @char.GetCharUnique();
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_BATTLE_INTRUSION_REQ, gS_BATTLE_INTRUSION_REQ);
		}
	}

	public void ClickBattleWatch(object data)
	{
		NrCharBase @char = this.GetChar();
		if ((@char != null && @char.IsCharStateAtb(16384L)) || @char.IsCharStateAtb(32768L))
		{
			GS_BATTLE_OBSERVER_REQ gS_BATTLE_OBSERVER_REQ = new GS_BATTLE_OBSERVER_REQ();
			gS_BATTLE_OBSERVER_REQ.iCharUnique = @char.GetCharUnique();
			SendPacket.GetInstance().SendObject(231, gS_BATTLE_OBSERVER_REQ);
		}
	}

	public void ClickAddFriend(object data)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null && uI_RightClickMenu.GetTileCharName() != null && nrCharUser.GetCharName().CompareTo(uI_RightClickMenu.GetTileCharName()) != 0)
		{
			GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
			gS_FRIEND_APPLY_REQ.i32WorldID = 0;
			TKString.StringChar(uI_RightClickMenu.GetTileCharName(), ref gS_FRIEND_APPLY_REQ.name);
			SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
		}
	}

	public void ClickReportUser(object data)
	{
		int value = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_CHATLIMIT_MAXNUM);
		string text = string.Empty;
		string message = string.Empty;
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if ((int)kMyCharInfo.GetCharDetailFromUnion(eCHAR_DETAIL_INFO.eCHAR_DETAIL_INFO_LIMIT_COUNT, 2) >= value)
		{
			message = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("700");
			Main_UI_SystemMessage.ADDMessage(message, SYSTEM_MESSAGE_TYPE.NORMAL_MESSAGE);
			return;
		}
		if (this.m_szCharName != string.Empty)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2577");
				string empty = string.Empty;
				text = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("323");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					text,
					"charname",
					this.m_szCharName
				});
				msgBoxUI.SetMsg(new YesDelegate(this.ReportCharYes), null, textFromInterface, empty, eMsgType.MB_OK_CANCEL, 2);
			}
		}
	}

	public void ClickInviteParty(object data)
	{
	}

	public void ClickWarp(object data)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		NrCharBase @char = this.GetChar();
		if (@char.GetCharName().CompareTo(nrCharUser.GetCharName()) == 0)
		{
			return;
		}
	}

	public void ClickPostSend(object data)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null && uI_RightClickMenu.GetTileCharName() != null)
		{
			PostDlg postDlg = (PostDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG);
			postDlg.SetSendCharName(uI_RightClickMenu.GetTileCharName());
		}
	}

	public void ClickFriendDel(object data)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null && uI_RightClickMenu.GetTileCharName() != null)
		{
			MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
			if (msgBoxUI != null)
			{
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("8");
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("328");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					textFromMessageBox,
					"Charname",
					uI_RightClickMenu.GetTileCharName()
				});
				msgBoxUI.SetMsg(new YesDelegate(this.FriendDelYes), null, textFromInterface, empty, eMsgType.MB_OK_CANCEL, 2);
			}
		}
	}

	public void ClickChatBlock(object data)
	{
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu != null && uI_RightClickMenu.GetTileCharName() != null && nrCharUser.GetCharName().CompareTo(uI_RightClickMenu.GetTileCharName()) == 0)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("360");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.CAUTION_MESSAGE);
			return;
		}
	}

	public void FriendDelYes(object a_oObject)
	{
		GS_DEL_FRIEND_REQ gS_DEL_FRIEND_REQ = new GS_DEL_FRIEND_REQ();
		gS_DEL_FRIEND_REQ.i64FriendPersonID = this.m_nPersonID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_DEL_FRIEND_REQ, gS_DEL_FRIEND_REQ);
	}

	public void ReportCharYes(object _Object)
	{
		GS_CHAT_REPORT_USER_REQ gS_CHAT_REPORT_USER_REQ = new GS_CHAT_REPORT_USER_REQ();
		TKString.StringChar(this.m_szCharName, ref gS_CHAT_REPORT_USER_REQ.szCharName);
		SendPacket.GetInstance().SendObject(138, gS_CHAT_REPORT_USER_REQ);
	}

	public void ClickMonDetailInfo(object data)
	{
	}

	public void ClickShowItemInfo(object data)
	{
		Inventory_Dlg inventory_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INVENTORY_DLG) as Inventory_Dlg;
		if (inventory_Dlg != null)
		{
			Protocol_Item.Item_ShowItemInfo((G_ID)inventory_Dlg.WindowID, this.m_oItem as ITEM, this.GetPosition(), null, 0L);
		}
		NrTSingleton<FormsManager>.Instance.Hide(G_ID.SOLSELECT_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DISASSEMBLEITEM_DLG);
	}

	public void ClickItemUseEquip(object data)
	{
		Protocol_Item.Item_Use(this.m_oItem as ITEM);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DISASSEMBLEITEM_DLG);
	}

	public void ClickItemEnhance(object data)
	{
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG) && !NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
		{
			ReforgeMainDlg reforgeMainDlg = (ReforgeMainDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.REFORGEMAIN_DLG);
			reforgeMainDlg.Show();
			reforgeMainDlg.Set_Value(this.m_oItem as ITEM);
			reforgeMainDlg.SetSolID(0L);
		}
	}

	public void ClickItemSell(object data)
	{
		bool flag = false;
		ITEM iTEM = this.m_oItem as ITEM;
		if (iTEM == null)
		{
			return;
		}
		switch (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(iTEM.m_nItemUnique))
		{
		case eITEM_PART.ITEMPART_WEAPON:
		case eITEM_PART.ITEMPART_ARMOR:
		case eITEM_PART.ITEMPART_ACCESSORY:
			flag = true;
			break;
		}
		if (!flag)
		{
			return;
		}
		if (NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(iTEM.m_nItemUnique) == 1000)
		{
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(iTEM.m_nItemUnique);
		if (itemInfo == null)
		{
			return;
		}
		ITEM_SELL itemSellData = NrTSingleton<ITEM_SELL_Manager>.Instance.GetItemSellData(itemInfo.m_nQualityLevel, (int)iTEM.GetRank());
		int nItemSellMoney = itemSellData.nItemSellMoney;
		ItemSellDlg itemSellDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMSELL_DLG) as ItemSellDlg;
		if (itemSellDlg != null)
		{
			itemSellDlg.SetItemSellInfo(iTEM, nItemSellMoney);
		}
	}

	public void OnOKSellItemStart(object a_oObject)
	{
		if (Protocol_Market.s_lsSellItem.Count >= 30)
		{
			string textFromNotify = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("237");
			Main_UI_SystemMessage.ADDMessage(textFromNotify, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			return;
		}
		ITEM iTEM = (ITEM)a_oObject;
		if (iTEM != null)
		{
			Protocol_Item.Send_AutoItemSell(iTEM.m_nItemID);
		}
	}

	public void ClickItemReleaseEquip(object data)
	{
		SolMilitaryGroupDlg solMilitaryGroupDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.SOLMILITARYGROUP_DLG) as SolMilitaryGroupDlg;
		if (solMilitaryGroupDlg == null || !solMilitaryGroupDlg.Visible)
		{
			return;
		}
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			return;
		}
		ITEM pkItem = this.m_oItem as ITEM;
		Protocol_Item.Send_EquipSol_InvenEquip(pkItem);
	}

	public void ClickItemDrop(object data)
	{
		ITEM iTEM = this.m_oItem as ITEM;
		if (iTEM != null)
		{
			Protocol_Item.On_Delete(iTEM);
		}
	}

	public void ClickCancel(object data)
	{
		this.CloseUI(this.m_CurFormKind);
	}

	public void ClickItemRelease(object data)
	{
		G_ID g_ID = (G_ID)((int)data);
		G_ID g_ID2 = g_ID;
		if (g_ID2 != G_ID.POST_DLG)
		{
			if (g_ID2 == G_ID.REFORGEMAIN_DLG)
			{
				ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
				if (reforgeMainDlg != null)
				{
					reforgeMainDlg.ItemSlotClear();
				}
			}
		}
		else
		{
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				postDlg.ItemSlotClear();
			}
		}
	}

	public void OnLEAVE(object data)
	{
	}

	private void OnTROPHY(object data)
	{
	}

	private void OnCHANGELEADER(object data)
	{
	}

	private void OnKICKOUT(object data)
	{
	}

	public bool addReforge()
	{
		bool result = false;
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.REFORGEMAIN_DLG))
		{
			ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
			if (reforgeMainDlg != null)
			{
				ITEM sourceItem = this.m_oItem as ITEM;
				if (reforgeMainDlg.IsItemCheck(sourceItem))
				{
					this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2209"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickItemRelease), G_ID.REFORGEMAIN_DLG);
					this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickCancel));
					result = true;
				}
			}
		}
		return result;
	}

	public bool addPostdlg()
	{
		bool result = false;
		if (NrTSingleton<FormsManager>.Instance.IsShow(G_ID.POST_DLG))
		{
			PostDlg postDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.POST_DLG) as PostDlg;
			if (postDlg != null)
			{
				ITEM sourceItem = this.m_oItem as ITEM;
				if (postDlg.IsItemCheck(sourceItem))
				{
					this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2209"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickItemRelease), G_ID.POST_DLG);
					this.AddList(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"), CRightClickMenu.BASIC_FONT_COLOR, true, new CRightClickMenu._OnClickMenu(this.ClickCancel));
					result = true;
				}
			}
		}
		return result;
	}

	public void ClickNewGuildRankChange(object data)
	{
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(this.m_nPersonID);
		if (memberInfoFromPersonID != null)
		{
			NewGuildRankChangeDlg newGuildRankChangeDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NEWGUILD_RANKCHANGE_DLG) as NewGuildRankChangeDlg;
			if (newGuildRankChangeDlg != null)
			{
				newGuildRankChangeDlg.SetChangeMember(memberInfoFromPersonID);
			}
		}
	}

	public void ClickNewGuildMemberKickOut(object data)
	{
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI != null)
		{
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("159"),
				"targetname",
				this.m_szCharName
			});
			msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKNewGuildMemberKickOut), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("160"), empty, eMsgType.MB_OK_CANCEL, 2);
		}
	}

	public void MsgBoxOKNewGuildMemberKickOut(object Obj)
	{
		if (this.m_nPersonID == NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID)
		{
			return;
		}
		if (NrTSingleton<NewGuildManager>.Instance.IsDischargeMember(NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_PersonID))
		{
			GS_NEWGUILD_MEMBER_DISCHARGE_REQ gS_NEWGUILD_MEMBER_DISCHARGE_REQ = new GS_NEWGUILD_MEMBER_DISCHARGE_REQ();
			gS_NEWGUILD_MEMBER_DISCHARGE_REQ.i64DischargePersonID = this.m_nPersonID;
			SendPacket.GetInstance().SendObject(1815, gS_NEWGUILD_MEMBER_DISCHARGE_REQ);
		}
	}

	public void ClickInviteGuild(object Obj)
	{
		UI_RightClickMenu uI_RightClickMenu = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_RIGHTCLICK_MENU) as UI_RightClickMenu;
		if (uI_RightClickMenu == null)
		{
			return;
		}
		GS_NEWGUILD_INVITE_REQ gS_NEWGUILD_INVITE_REQ = new GS_NEWGUILD_INVITE_REQ();
		TKString.StringChar(uI_RightClickMenu.GetTileCharName(), ref gS_NEWGUILD_INVITE_REQ.strCharName);
		SendPacket.GetInstance().SendObject(1827, gS_NEWGUILD_INVITE_REQ);
	}
}
