using GAME;
using Global;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class DLG_OtherCharDetailInfo : Form
{
	private enum eINFO
	{
		eINFO_OTHERPERSON,
		eINFO_FRIEND,
		eINFO_MAX
	}

	private enum eFRIEND_DETAIL
	{
		eFRIEND_DETAIL_HELPSOLCOUNT,
		eFRIEND_DETAIL_COLOSSEUMGRADE,
		eFRIEND_DETAIL_PLUNDERRANK,
		eFRIEND_DETAIL_BABELTOPFLOOR,
		eFRIEND_DETAIL_MAX
	}

	private const int SLOT_MAX = 6;

	private const string EMPTY_SOL_TEX = "NULL";

	private const int LIST_MAX = 14;

	private long m_PersonID = -1L;

	private string m_PersonCharName = string.Empty;

	private int LOADED_PAGE = -1;

	private List<COMMUNITY_USER_INFO> m_CommunityUserList = new List<COMMUNITY_USER_INFO>();

	private NrPage m_Page;

	private int m_SelectIndex;

	private NrSoldierList m_SolList;

	private Label m_lbTitle;

	private Toolbar m_TB;

	private DrawTexture[] SolLeader = new DrawTexture[2];

	private DrawTexture[] SolSelect = new DrawTexture[2];

	private ItemTexture[][] m_dtSlot = new ItemTexture[2][];

	private Button[][] m_btSlot = new Button[2][];

	private Label[] m_lbChaName = new Label[2];

	private Label[] m_lbIntro = new Label[2];

	private Button m_btAddFriend;

	private Button m_btClose;

	private NewListBox m_lbFriend_DetailInfo;

	private DrawTexture m_dtFriendFaceBookImg;

	private Label m_laFriendFaceBookID;

	private NewListBox m_lxList;

	private Button m_btPagePre;

	private Box m_bxCurrentPage;

	private Button m_btPageNext;

	private DrawTexture[] m_DrawTexture_Slot = new DrawTexture[6];

	private DrawTexture[] m_DrawTexture_Slot2 = new DrawTexture[6];

	private int Info_mode;

	private int[] Friend_Detail_Value = new int[4];

	public string GetCharNameBySoldierInfo(NkSoldierInfo kSolInfo)
	{
		string text = kSolInfo.GetName();
		if (text == string.Empty)
		{
			text = this.m_PersonCharName;
		}
		return text;
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Community/dlg_otherdetailinfo", G_ID.DLG_OTHER_CHAR_DETAIL, true);
	}

	public override void SetComponent()
	{
		this.m_lbTitle = (base.GetControl("Label_Title") as Label);
		this.m_TB = (base.GetControl("ToolBar_ToolBar") as Toolbar);
		if (null != this.m_TB)
		{
			UIPanelTab expr_4A = this.m_TB.Control_Tab[0];
			expr_4A.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_4A.ButtonClick, new EZValueChangedDelegate(this.OnClickToolBar));
			UIPanelTab expr_78 = this.m_TB.Control_Tab[1];
			expr_78.ButtonClick = (EZValueChangedDelegate)Delegate.Combine(expr_78.ButtonClick, new EZValueChangedDelegate(this.OnClickToolBar));
			this.m_TB.Control_Tab[0].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1657");
			this.m_TB.Control_Tab[1].Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1656");
			this.m_TB.FirstSetting();
		}
		string name = string.Empty;
		for (int i = 0; i < 2; i++)
		{
			this.m_dtSlot[i] = new ItemTexture[6];
			this.m_btSlot[i] = new Button[6];
			for (int j = 0; j < 6; j++)
			{
				name = string.Format("ItemTexture_SolIcn{0}{1}", i, j + 1);
				this.m_dtSlot[i][j] = (base.GetControl(name) as ItemTexture);
				name = string.Format("Button_SolBtn{0}{1}", i, j + 1);
				this.m_btSlot[i][j] = (base.GetControl(name) as Button);
				this.m_btSlot[i][j].TabIndex = j;
			}
			this.SolLeader[i] = (base.GetControl(string.Format("DrawTexture_SolLeaderSelect{0}", i)) as DrawTexture);
			this.SolSelect[i] = (base.GetControl(string.Format("DrawTexture_SolLeader{0}", i)) as DrawTexture);
			this.m_lbIntro[i] = (base.GetControl(string.Format("Label_Introduce{0}", i)) as Label);
			this.m_lbChaName[i] = (base.GetControl(string.Format("Label_CharName{0}", i)) as Label);
		}
		this.m_btAddFriend = (base.GetControl("Button_AddFriend") as Button);
		Button expr_242 = this.m_btAddFriend;
		expr_242.Click = (EZValueChangedDelegate)Delegate.Combine(expr_242.Click, new EZValueChangedDelegate(this.OnAddFriend));
		this.m_lxList = (base.GetControl("NLB_connection") as NewListBox);
		if (null != this.m_lxList)
		{
			this.m_lxList.AddValueChangedDelegate(new EZValueChangedDelegate(this.BtnClickListBox));
		}
		this.m_bxCurrentPage = (base.GetControl("Box_Page") as Box);
		this.m_btPagePre = (base.GetControl("Button_Pagepre") as Button);
		Button expr_2D3 = this.m_btPagePre;
		expr_2D3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_2D3.Click, new EZValueChangedDelegate(this.OnPrePage));
		this.m_btPageNext = (base.GetControl("Button_Pagenext") as Button);
		Button expr_310 = this.m_btPageNext;
		expr_310.Click = (EZValueChangedDelegate)Delegate.Combine(expr_310.Click, new EZValueChangedDelegate(this.OnNextPage));
		for (int k = 0; k < 6; k++)
		{
			this.m_DrawTexture_Slot[k] = (base.GetControl(string.Format("DrawTexture_Slot{0}", 11 + k)) as DrawTexture);
			this.m_DrawTexture_Slot2[k] = (base.GetControl(string.Format("DrawTexture_Slot{0:00}", 1 + k)) as DrawTexture);
		}
		this.m_lbFriend_DetailInfo = (base.GetControl("NLB_Info") as NewListBox);
		this.m_dtFriendFaceBookImg = (base.GetControl("DrawTexture_fb02") as DrawTexture);
		this.m_dtFriendFaceBookImg.Hide(true);
		this.m_laFriendFaceBookID = (base.GetControl("LB_fbname02") as Label);
		this.m_laFriendFaceBookID.SetText(string.Empty);
		this.m_btClose = (base.GetControl("Button_Close") as Button);
		this.m_btClose.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		this.m_Page = new NrPage(this.m_btPagePre, this.m_btPageNext, new REFRESH_VOIDFUC(this.OnPageList));
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		this.OnClickToolBar(this.m_TB.Control_Tab[0]);
	}

	private void InitDetailInfo()
	{
		this.m_CommunityUserList.Clear();
		this.LOADED_PAGE = -1;
		this.m_Page.CURRENT_PAGE = 1;
		this.OnClickToolBar(this.m_TB.Control_Tab[0]);
		this.m_TB.SetSelectTabIndex(0);
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.m_SolList != null)
		{
			this.m_SolList.Init();
			this.m_SolList = null;
		}
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_OTHER_CHAR_EQUIPMENT);
	}

	private void OnClickToolBar(IUIObject obj)
	{
		UIPanelTab uIPanelTab = (UIPanelTab)obj;
		int index = uIPanelTab.panel.index;
		base.AllHideLayer();
		base.ShowLayer(0);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (index == 1)
		{
			base.ShowLayer(2);
			this.UpdateList();
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_OTHER_CHAR_EQUIPMENT);
		}
		else
		{
			if (kMyCharInfo.m_kFriendInfo.IsFriend(this.m_PersonID))
			{
				base.ShowLayer(3);
			}
			else
			{
				base.ShowLayer(1);
			}
			this.SetSoldierList(this.m_SolList);
		}
	}

	private void OnClickSol(IUIObject obj)
	{
		Button button = (Button)obj;
		if (null != button)
		{
			NkSoldierInfo soldierInfo = this.m_SolList.GetSoldierInfo(button.TabIndex);
			if (soldierInfo != null)
			{
				DLG_OtherCharEquipment dLG_OtherCharEquipment = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_OTHER_CHAR_EQUIPMENT) as DLG_OtherCharEquipment;
				if (dLG_OtherCharEquipment != null)
				{
					dLG_OtherCharEquipment.SetSolStatInfo(soldierInfo);
				}
			}
			this.UpdateSoldieInfo(button.TabIndex);
		}
	}

	private void BtnClickListBox(IUIObject obj)
	{
		IUIListObject selectItem = this.m_lxList.GetSelectItem();
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = (COMMUNITY_USER_INFO)selectItem.Data;
		int num = this.m_lxList.SelectedItem.GetIndex();
		num = Mathf.Clamp(num, 0, this.m_CommunityUserList.Count);
		cOMMUNITY_USER_INFO = this.m_CommunityUserList[num];
		bool flag = false;
		if (cOMMUNITY_USER_INFO != null)
		{
			if (cOMMUNITY_USER_INFO.byLocation <= 0 || !cOMMUNITY_USER_INFO.bConnect)
			{
				flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(cOMMUNITY_USER_INFO.i64PersonID, 0, cOMMUNITY_USER_INFO.strName, CRightClickMenu.KIND.OTHER_FRIEND_LOGOFF, CRightClickMenu.TYPE.SIMPLE_SECTION_1, false);
			}
			else if (Client.m_MyWS != (long)cOMMUNITY_USER_INFO.i32WorldID || Client.m_MyCH != cOMMUNITY_USER_INFO.byLocation)
			{
				Debug.Log(string.Concat(new object[]
				{
					Client.m_MyWS,
					"!=",
					cOMMUNITY_USER_INFO.i32WorldID,
					",",
					Client.m_MyCH,
					"!=",
					cOMMUNITY_USER_INFO.byLocation
				}));
				flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(cOMMUNITY_USER_INFO.i64PersonID, 0, cOMMUNITY_USER_INFO.strName, CRightClickMenu.KIND.OTHER_FRIEND_DIFF_SV_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_3, false);
			}
			else
			{
				flag = NrTSingleton<CRightClickMenu>.Instance.CreateUI(cOMMUNITY_USER_INFO.i64PersonID, 0, cOMMUNITY_USER_INFO.strName, CRightClickMenu.KIND.OTHER_FRIEND_SAME_SV_CLICK, CRightClickMenu.TYPE.SIMPLE_SECTION_3, false);
			}
		}
		else
		{
			NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
		}
		if (flag)
		{
			float x = this.m_lxList.GetSize().x;
			float height = 28f;
			float left = base.GetLocation().x + this.m_lxList.GetLocation().x + this.m_lxList.GetSelectItem().gameObject.transform.localPosition.x;
			float top = base.GetLocationY() + this.m_lxList.GetLocationY() + -this.m_lxList.GetSelectItem().gameObject.transform.localPosition.y;
			Rect windowRect = new Rect(left, top, x, height);
			NrTSingleton<CRightClickMenu>.Instance.SetWindowRect(windowRect);
		}
	}

	private void UpdateSoldieInfo(int solIdx)
	{
		if (this.m_SolList != null)
		{
			NkSoldierInfo soldierInfo = this.m_SolList.GetSoldierInfo(solIdx);
			string text = string.Empty;
			if (soldierInfo != null && soldierInfo.IsValid())
			{
				text = soldierInfo.GetName();
				if (text == string.Empty)
				{
					text = this.m_PersonCharName;
				}
				if (!soldierInfo.IsItemReceiveData())
				{
					this.SOLDIER_EQUIPINFO_REQ(this.m_PersonID, soldierInfo.GetSolID());
				}
				this.m_SelectIndex = solIdx;
				Button button = this.m_btSlot[this.Info_mode][this.m_SelectIndex];
				if (null != button)
				{
					float num = 2f;
					this.SolLeader[this.Info_mode].SetLocation(button.GetLocationX() - num, button.GetLocationY() - num);
				}
			}
			this.m_lbChaName[this.Info_mode].Text = text;
		}
	}

	private void OnAddFriend(IUIObject obj)
	{
		GS_FRIEND_APPLY_REQ gS_FRIEND_APPLY_REQ = new GS_FRIEND_APPLY_REQ();
		gS_FRIEND_APPLY_REQ.i32WorldID = 0;
		TKString.StringChar(this.m_PersonCharName, ref gS_FRIEND_APPLY_REQ.name);
		SendPacket.GetInstance().SendObject(904, gS_FRIEND_APPLY_REQ);
	}

	private void OnPrePage(IUIObject obj)
	{
	}

	private void OnNextPage(IUIObject obj)
	{
	}

	public void OnPageList()
	{
		if (this.LOADED_PAGE < this.m_Page.CURRENT_PAGE)
		{
			this.OTHER_FRIEND_LIST_PAGE_REQ(this.m_PersonID, this.m_Page.CURRENT_PAGE, 14);
		}
		this.UpdateList();
	}

	public void SetPersonID(long PersonID, string szCharName, string szIntroMsg)
	{
		base.AllHideLayer();
		base.ShowLayer(0);
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (0L < PersonID)
		{
			if (this.m_CommunityUserList != null)
			{
				this.m_CommunityUserList.Clear();
			}
			if (this.m_SolList != null)
			{
				this.m_SolList.Init();
			}
			this.m_PersonID = PersonID;
			this.m_PersonCharName = szCharName;
			long num = 0L;
			string arg = string.Empty;
			if (kMyCharInfo.m_kFriendInfo.IsFriend(this.m_PersonID))
			{
				this.Info_mode = 1;
				this.FRIEND_DETAILINFO_REQ(this.m_PersonID);
				base.ShowLayer(1);
			}
			else
			{
				this.Info_mode = 0;
				base.ShowLayer(3);
			}
			if (szIntroMsg == string.Empty)
			{
				string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("124");
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref arg, new object[]
				{
					textFromInterface,
					"username",
					szCharName
				});
			}
			else
			{
				arg = szIntroMsg;
			}
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1658"),
				"count",
				string.Format("{0:####,####,####,####}", num)
			});
			this.m_lbTitle.Text = string.Format("{0}", this.m_PersonCharName);
			this.m_lbIntro[this.Info_mode].Text = string.Format("{0}", arg);
			this.InitDetailInfo();
			this.SOLDIER_BASEINFO_REQ(this.m_PersonID);
			this.OTHER_FRIEND_LIST_PAGE_REQ(this.m_PersonID, this.m_Page.CURRENT_PAGE - 1, 14);
		}
		else
		{
			Debug.LogError("Fail PersonID" + PersonID);
		}
	}

	public void SetSoldierEquipItem(long SolID, NrEquipItemInfo kEquipInfo)
	{
		if (this.m_SolList != null)
		{
			NkSoldierInfo soldierInfoBySolID = this.m_SolList.GetSoldierInfoBySolID(SolID);
			if (soldierInfoBySolID != null)
			{
				NrEquipItemInfo equipItemInfo = soldierInfoBySolID.GetEquipItemInfo();
				equipItemInfo.Set(kEquipInfo);
				soldierInfoBySolID.UpdateSoldierStatInfo();
				this.UpdateSoldieInfo(this.m_SelectIndex);
			}
		}
		else
		{
			Debug.LogError("m_SolList null ");
		}
	}

	public void SetSolierTexutre(EVENT_HERODATA _EventHero, int iSolIndex, NkSoldierInfo pkSolInfo)
	{
		if (_EventHero != null)
		{
			if (this.Info_mode == 1)
			{
				this.m_DrawTexture_Slot[iSolIndex].SetTexture("Win_I_EventSol");
			}
			if (this.Info_mode == 0)
			{
				this.m_DrawTexture_Slot2[iSolIndex].SetTexture("Win_I_EventSol");
			}
			this.m_dtSlot[this.Info_mode][iSolIndex].SetSolImageTexure(eCharImageType.SMALL, this.GetListSolInfo(pkSolInfo));
		}
		else
		{
			UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(pkSolInfo.GetCharKind(), (int)pkSolInfo.GetGrade());
			if (legendFrame != null)
			{
				if (this.Info_mode == 1)
				{
					this.m_DrawTexture_Slot[iSolIndex].SetTexture(legendFrame);
				}
				if (this.Info_mode == 0)
				{
					this.m_DrawTexture_Slot2[iSolIndex].SetTexture(legendFrame);
				}
			}
			this.m_dtSlot[this.Info_mode][iSolIndex].SetSolImageTexure(eCharImageType.SMALL, this.GetListSolInfo(pkSolInfo));
		}
	}

	public void UpsateSoldierList()
	{
		if (this.m_SolList != null)
		{
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo soldierInfo = this.m_SolList.GetSoldierInfo(i);
				if (soldierInfo != null && soldierInfo.IsValid())
				{
					EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(soldierInfo.GetCharKind(), soldierInfo.GetGrade());
					if (i == 0)
					{
						CommunityUI_DLG communityUI_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COMMUNITY_DLG) as CommunityUI_DLG;
						if (communityUI_DLG != null)
						{
							COMMUNITY_USER_INFO community_User = communityUI_DLG.GetCommunity_User(this.m_PersonID);
							if (community_User != null && community_User.UserPortrait != null)
							{
								this.m_dtSlot[this.Info_mode][i].SetTexture(community_User.UserPortrait);
							}
							else
							{
								this.SetSolierTexutre(eventHeroCharCode, i, soldierInfo);
							}
						}
						else
						{
							this.SetSolierTexutre(eventHeroCharCode, i, soldierInfo);
						}
					}
					else
					{
						this.SetSolierTexutre(eventHeroCharCode, i, soldierInfo);
					}
					string empty = string.Empty;
					string charNameBySoldierInfo = this.GetCharNameBySoldierInfo(soldierInfo);
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1639"),
						"count",
						soldierInfo.GetLevel(),
						"targetname",
						charNameBySoldierInfo
					});
					this.m_btSlot[this.Info_mode][i].ToolTip = empty;
					Button expr_153 = this.m_btSlot[this.Info_mode][i];
					expr_153.Click = (EZValueChangedDelegate)Delegate.Combine(expr_153.Click, new EZValueChangedDelegate(this.OnClickSol));
					this.SOLDIER_EQUIPINFO_REQ(this.m_PersonID, soldierInfo.GetSolID());
				}
				else
				{
					this.m_dtSlot[this.Info_mode][i].SetTexture("NULL");
					this.m_btSlot[this.Info_mode][i].ShowToolTip = false;
				}
			}
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo.m_PersonID == this.m_PersonID || kMyCharInfo.m_kFriendInfo.IsFriend(kMyCharInfo.m_PersonID))
			{
				this.m_btAddFriend.Visible = false;
			}
		}
	}

	public void SetSoldierList(NrSoldierList solList)
	{
		this.m_SolList = solList;
		this.UpsateSoldierList();
	}

	public List<COMMUNITY_USER_INFO> GetFriendList()
	{
		return this.m_CommunityUserList;
	}

	public void UpdateFriendList(USER_FRIEND_INFO info)
	{
		COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = new COMMUNITY_USER_INFO();
		cOMMUNITY_USER_INFO.Set(info);
		foreach (COMMUNITY_USER_INFO current in this.m_CommunityUserList)
		{
			if (current.i64PersonID == info.nPersonID)
			{
				current.Update(info);
				break;
			}
		}
		if (cOMMUNITY_USER_INFO != null)
		{
			COMMUNITY_USER_INFO cOMMUNITY_USER_INFO2 = new COMMUNITY_USER_INFO();
			cOMMUNITY_USER_INFO2.Set(info);
			this.m_CommunityUserList.Add(cOMMUNITY_USER_INFO2);
		}
		int a = this.m_CommunityUserList.Count / 14 + 1;
		this.LOADED_PAGE = Mathf.Max(a, this.LOADED_PAGE);
	}

	public void UpdateList(int FriendMaxCount)
	{
		this.m_Page.MAX_PAGE = FriendMaxCount / 14 + 1;
		this.UpdateList();
	}

	public void UpdateList()
	{
		this.m_lxList.Clear();
		string empty = string.Empty;
		string empty2 = string.Empty;
		int num = (this.m_Page.CURRENT_PAGE - 1) * 14;
		int num2 = Mathf.Min(num + 14, this.m_CommunityUserList.Count);
		for (int i = num; i < num2; i++)
		{
			COMMUNITY_USER_INFO cOMMUNITY_USER_INFO = this.m_CommunityUserList[i];
			NewListItem newListItem = new NewListItem(this.m_lxList.ColumnNum, true, string.Empty);
			CommunityUI_DLG.CurrentLocationName(cOMMUNITY_USER_INFO, ref empty, ref empty2);
			newListItem.SetListItemData(0, this.GetLoaderImg(CommunityUI_DLG.CommunityIcon(cOMMUNITY_USER_INFO)), null, null, null);
			newListItem.SetListItemData(1, cOMMUNITY_USER_INFO.strName, null, null, null);
			newListItem.SetListItemData(2, cOMMUNITY_USER_INFO.i16Level.ToString(), null, null, null);
			newListItem.SetListItemData(3, NrTSingleton<CTextParser>.Instance.GetTextColor(empty2) + empty, null, null, null);
			newListItem.Data = cOMMUNITY_USER_INFO;
			this.m_lxList.Add(newListItem);
		}
		this.m_lxList.RepositionItems();
		this.m_bxCurrentPage.Text = string.Format("{0}/{1}", this.m_Page.CURRENT_PAGE, this.m_Page.MAX_PAGE);
	}

	private UIBaseInfoLoader GetLoaderImg(string Key)
	{
		UIBaseInfoLoader uIBaseInfoLoader = new UIBaseInfoLoader();
		uIBaseInfoLoader.Tile = SpriteTile.SPRITE_TILE_MODE.STM_3x3;
		NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(Key, ref uIBaseInfoLoader);
		return uIBaseInfoLoader;
	}

	public void SetFriendDetailInfo(GS_FRIEND_DETAILINFO_ACK ACK, NkDeserializePacket kDeserializePacket)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		USER_FRIEND_INFO friend = kMyCharInfo.m_kFriendInfo.GetFriend(this.m_PersonID);
		this.Friend_Detail_Value[0] = ACK.FriendHelpCount;
		this.Friend_Detail_Value[1] = (int)ACK.ColosseumGrade;
		this.Friend_Detail_Value[2] = ACK.i32InfiRank;
		this.Friend_Detail_Value[3] = (int)ACK.i16BabelClearFloor;
		this.m_lbFriend_DetailInfo.Clear();
		string text = string.Empty;
		string text2 = string.Empty;
		for (int i = 0; i < 4; i++)
		{
			NewListItem newListItem = new NewListItem(this.m_lbFriend_DetailInfo.ColumnNum, true, string.Empty);
			text2 = this.GetFriendDetailTitleText(i);
			newListItem.SetListItemData(0, text2, null, null, null);
			text = this.GetFriendDetailinfoText(i);
			if (i == 1)
			{
				text2 = NrTSingleton<NrTable_ColosseumRankReward_Manager>.Instance.GetGradeTextKey((short)this.Friend_Detail_Value[i]);
			}
			else if (i == 2)
			{
				if ((short)this.Friend_Detail_Value[i] == 0)
				{
					text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2225");
				}
				else
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
					{
						text,
						"count",
						this.Friend_Detail_Value[i]
					});
				}
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text2, new object[]
				{
					text,
					"count",
					this.Friend_Detail_Value[i]
				});
			}
			newListItem.SetListItemData(1, text2, null, null, null);
			this.m_lbFriend_DetailInfo.Add(newListItem);
		}
		this.m_lbFriend_DetailInfo.RepositionItems();
		if (friend != null && TKString.NEWString(friend.szPlatformName) != string.Empty)
		{
			this.m_dtFriendFaceBookImg.Hide(false);
			this.m_laFriendFaceBookID.SetText(TKString.NEWString(friend.szPlatformName));
		}
	}

	public string GetFriendDetailTitleText(int detailinfo)
	{
		string result = string.Empty;
		switch (detailinfo)
		{
		case 0:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2167");
			break;
		case 1:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2521");
			break;
		case 2:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2217");
			break;
		case 3:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2169");
			break;
		}
		return result;
	}

	public string GetFriendDetailinfoText(int detailinfo)
	{
		string result = string.Empty;
		switch (detailinfo)
		{
		case 0:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1185");
			break;
		case 2:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1186");
			break;
		case 3:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1187");
			break;
		}
		return result;
	}

	private NkListSolInfo GetListSolInfo(NkSoldierInfo solInfo)
	{
		if (solInfo == null)
		{
			return null;
		}
		return new NkListSolInfo
		{
			ShowCombat = false,
			ShowLevel = false,
			ShowGrade = true,
			SolGrade = (int)solInfo.GetGrade(),
			SolCharKind = solInfo.GetCharKind(),
			SolCostumePortraitPath = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(solInfo)
		};
	}

	private void FRIEND_DETAILINFO_REQ(long personid)
	{
		GS_FRIEND_DETAILINFO_REQ gS_FRIEND_DETAILINFO_REQ = new GS_FRIEND_DETAILINFO_REQ();
		gS_FRIEND_DETAILINFO_REQ.nFriendPersonID = personid;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_DETAILINFO_REQ, gS_FRIEND_DETAILINFO_REQ);
	}

	private void SOLDIER_BASEINFO_REQ(long PersonID)
	{
		GS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_REQ gS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_REQ = new GS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_REQ();
		gS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_REQ.nPersonID = PersonID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_REQ, gS_LOAD_OTHERCHAR_SOLDIER_BASEINFO_REQ);
	}

	private void SOLDIER_EQUIPINFO_REQ(long PersonID, long SolID)
	{
		GS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_REQ gS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_REQ = default(GS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_REQ);
		gS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_REQ.nPersonID = PersonID;
		gS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_REQ.i64SolID = SolID;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_REQ, gS_LOAD_OTHERCHAR_SOLDIER_EQUIPINFO_REQ);
	}

	private void OTHER_FRIEND_LIST_PAGE_REQ(long PersonID, int i8Page, int i8ListNum)
	{
		GS_OTHER_FRIEND_LIST_PAGE_REQ gS_OTHER_FRIEND_LIST_PAGE_REQ = new GS_OTHER_FRIEND_LIST_PAGE_REQ();
		gS_OTHER_FRIEND_LIST_PAGE_REQ.nPersonID = PersonID;
		gS_OTHER_FRIEND_LIST_PAGE_REQ.ui8Page = (byte)i8Page;
		gS_OTHER_FRIEND_LIST_PAGE_REQ.ui8ListNum = (byte)i8ListNum;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_OTHER_FRIEND_LIST_PAGE_REQ, gS_OTHER_FRIEND_LIST_PAGE_REQ);
	}
}
