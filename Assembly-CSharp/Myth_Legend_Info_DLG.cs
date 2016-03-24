using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityForms;

public class Myth_Legend_Info_DLG : Form
{
	protected DrawTexture m_DT_WEAPON;

	protected Label m_Label_character_name;

	protected Label m_Label_SeasonNum;

	protected DrawTexture m_DrawTexture_rank;

	protected DrawTexture m_DrawTexture_character;

	protected DrawTexture m_DT_SKILLICON;

	private Button m_DT_SKILLBUTTON;

	private Button m_Button_MovieBtn;

	protected NewListBox m_NewListBox_Reincarnate;

	private Button m_Close_Button;

	private Button m_Button_Help;

	protected Label m_Label_Gold2;

	protected Label m_Label_Essence2;

	protected Button m_btn_Legend;

	protected Label m_Label_LegendTime;

	private Button m_btn_LegendTime;

	protected CHARKIND_LEGENDINFO m_CharKind_Legendinfo;

	protected eElement_MsgType[] m_eElement_Msg = new eElement_MsgType[5];

	private ElementSol m_Element_SolID = new ElementSol();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/Evolution/DLG_SolEvolution_Legend_Info", G_ID.MYTH_LEGEND_INFO_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_DT_WEAPON = (base.GetControl("DT_WEAPON") as DrawTexture);
		this.m_Label_character_name = (base.GetControl("Label_character_name") as Label);
		this.m_Label_SeasonNum = (base.GetControl("Label_SeasonNum") as Label);
		this.m_DrawTexture_rank = (base.GetControl("DrawTexture_rank") as DrawTexture);
		this.m_DrawTexture_character = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.m_DT_SKILLICON = (base.GetControl("DT_SKILLICON") as DrawTexture);
		this.m_DT_SKILLBUTTON = (base.GetControl("DT_SKILLBUTTON") as Button);
		this.m_DT_SKILLBUTTON.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_SkillInfo));
		this.m_Button_MovieBtn = (base.GetControl("Button_MovieBtn") as Button);
		this.m_Button_MovieBtn.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_PreViewHero));
		this.m_NewListBox_Reincarnate = (base.GetControl("NewListBox_Reincarnate") as NewListBox);
		this.m_NewListBox_Reincarnate.touchScroll = false;
		this.m_Close_Button = (base.GetControl("Close_Button") as Button);
		this.m_Close_Button.AddValueChangedDelegate(new EZValueChangedDelegate(this.Click_Close));
		this.m_Button_Help = (base.GetControl("Button_Help") as Button);
		this.m_Button_Help.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickHelp));
		this.m_Label_Gold2 = (base.GetControl("Label_Gold2") as Label);
		this.m_Label_Essence2 = (base.GetControl("Label_Essence2") as Label);
		this.m_btn_Legend = (base.GetControl("btn_Legend") as Button);
		this.m_btn_Legend.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickReincarnate));
		this.m_Label_LegendTime = (base.GetControl("Label_LegendTime") as Label);
		this.m_btn_LegendTime = (base.GetControl("BTN_LegendTime") as Button);
		this.m_btn_LegendTime.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickTimeEnd));
		base.SetScreenCenter();
		base.ShowBlackBG(0.8f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public virtual void InitSetCharKind(int i32CharKind)
	{
		this.GetTimeToString();
		this.InitLegendDataSet(i32CharKind);
	}

	public virtual bool GetTimeToString()
	{
		bool result = false;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LEGENDMAKETIME);
			long curTime = PublicMethod.GetCurTime();
			if (curTime < charSubData)
			{
				base.SetShowLayer(1, true);
				string empty = string.Empty;
				long iSec = charSubData - curTime;
				long totalDayFromSec = PublicMethod.GetTotalDayFromSec(iSec);
				long hourFromSec = PublicMethod.GetHourFromSec(iSec);
				long minuteFromSec = PublicMethod.GetMinuteFromSec(iSec);
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2858"),
					"day",
					totalDayFromSec,
					"hour",
					hourFromSec,
					"min",
					minuteFromSec
				});
				this.m_Label_LegendTime.SetText(empty);
			}
			else
			{
				base.SetShowLayer(1, false);
				result = true;
			}
		}
		return result;
	}

	private void InitLegendDataSet(int i32CharKind)
	{
		this.m_CharKind_Legendinfo = NrTSingleton<NrBaseTableManager>.Instance.GetLegendGuide_Col(i32CharKind);
		if (this.m_CharKind_Legendinfo == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_CharKind_Legendinfo.i32Element_LegendCharkind);
		if (charKindInfo == null)
		{
			return;
		}
		this.m_Element_SolID.Init();
		this.m_Label_character_name.SetText(charKindInfo.GetName());
		byte b = this.m_CharKind_Legendinfo.ui8Element_LegendGrade - 1;
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(i32CharKind, (int)b);
		this.m_DrawTexture_character.SetTextureEffect(eCharImageType.LARGE, i32CharKind, (int)b, string.Empty);
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(i32CharKind, (int)b);
		if (0 < legendType)
		{
			this.m_DrawTexture_rank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
		}
		this.m_DrawTexture_rank.SetTexture(solLargeGradeImg);
		int weaponType = charKindInfo.GetWeaponType();
		this.m_DT_WEAPON.SetTexture(string.Format("Win_I_Weapon{0}", weaponType.ToString()));
		int season = charKindInfo.GetSeason(b);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
			"count",
			season + 1
		});
		this.m_Label_SeasonNum.SetText(empty);
		SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(this.m_CharKind_Legendinfo.i32Element_LegendCharkind);
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(solGuild.m_i32SkillUnique);
		if (battleSkillBase != null)
		{
			UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique);
			this.m_DT_SKILLICON.SetTexture(battleSkillIconTexture);
			string empty2 = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1293"),
				"skillname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey)
			});
		}
		else
		{
			this.m_DT_SKILLICON.SetTexture(string.Empty);
		}
		this.m_Label_Gold2.SetText(ANNUALIZED.Convert(this.m_CharKind_Legendinfo.i64NeedMoney));
		this.m_Label_Essence2.SetText(ANNUALIZED.Convert(this.m_CharKind_Legendinfo.i32NeedEssence));
		this.m_NewListBox_Reincarnate.Clear();
		for (int i = 0; i < 5; i++)
		{
			this.m_eElement_Msg[i] = eElement_MsgType.eElement_NONE;
			if (i32CharKind != 0)
			{
				NewListItem item = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true, string.Empty);
				this.m_eElement_Msg[i] = eElement_MsgType.eElement_NOTSOL;
				this.SetLegendReincarnateListBox(ref item, i, this.m_CharKind_Legendinfo.i32Base_CharKind[i], this.m_CharKind_Legendinfo.ui8Base_LegendGrade[i], false);
				this.m_NewListBox_Reincarnate.Add(item);
			}
		}
		this.m_NewListBox_Reincarnate.RepositionItems();
		bool flag = this.SetButtonLegendReincarnate();
		if (!flag)
		{
			flag = this.SetButtonLegendTime();
			if (!flag)
			{
				this.m_btn_Legend.SetEnabled(flag);
			}
		}
		else
		{
			this.m_btn_Legend.SetEnabled(flag);
			this.SetButtonLegendTime();
		}
		this.GetTimeToString();
	}

	private void SetLegendReincarnateListBox(ref NewListItem item, int i, int i32CharKind, byte bCharRank, bool bElement)
	{
		string text = string.Empty;
		NkSoldierInfo legendBattleSolDataCheck = this.GetLegendBattleSolDataCheck(ref this.m_eElement_Msg[i], i32CharKind, bCharRank);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!!!!!!!! SolGuild - Element CharKind " + i32CharKind + " Error");
			return;
		}
		item.SetListItemData(8, false);
		item.SetListItemData(9, false);
		if (i == 0)
		{
			item.SetListItemData(10, false);
		}
		else
		{
			item.SetListItemData(10, string.Empty, i, new EZValueChangedDelegate(this.OnClickUseSol), null);
		}
		NkListSolInfo nkListSolInfo = new NkListSolInfo();
		nkListSolInfo.SolCharKind = i32CharKind;
		nkListSolInfo.SolGrade = (int)(bCharRank - 1);
		item.SetListItemData(3, nkListSolInfo, null, null, null);
		item.SetListItemData(4, charKindInfo.GetName(), null, null, null);
		text = this.GetElementSolMsg(this.m_eElement_Msg[i]);
		item.SetListItemData(5, text, null, null, null);
		if (this.m_eElement_Msg[i] == eElement_MsgType.eElement_OK || this.m_eElement_Msg[i] == eElement_MsgType.eElement_SOLHEIGHT)
		{
			item.SetListItemData(0, true);
			item.SetListItemData(1, false);
			if (legendBattleSolDataCheck == null)
			{
				item.SetListItemData(6, false);
				item.SetListItemData(7, false);
				item.SetListItemData(11, false);
				return;
			}
			item.SetListItemData(6, true);
			item.SetListItemData(7, legendBattleSolDataCheck.GetListSolInfo(false), null, null, null);
			item.SetListItemData(11, false);
			this.m_Element_SolID.SetLegendSol(legendBattleSolDataCheck, i);
		}
		else
		{
			item.SetListItemData(0, false);
			item.SetListItemData(1, true);
			if (legendBattleSolDataCheck == null)
			{
				item.SetListItemData(6, false);
				item.SetListItemData(7, false);
				item.SetListItemData(11, false);
				return;
			}
			item.SetListItemData(6, true);
			item.SetListItemData(7, legendBattleSolDataCheck.GetListSolInfo(false), null, null, null);
			item.SetListItemData(11, true);
			this.m_Element_SolID.SetLegendSol(legendBattleSolDataCheck, i);
		}
		bool flag = false;
		for (int j = 0; j < 6; j++)
		{
			if (legendBattleSolDataCheck.GetEquipItemInfo() != null)
			{
				ITEM item2 = legendBattleSolDataCheck.GetEquipItemInfo().m_kItem[j].GetItem();
				if (item2 == null)
				{
					TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Item pos{1}  ==  ITEM NULL ", new object[]
					{
						i32CharKind,
						j
					});
				}
				else if (item2.m_nItemUnique != 0)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			item.SetListItemData(5, false);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627");
			item.SetListItemData(8, true);
			item.SetListItemData(9, true);
			item.SetListItemData(9, text, legendBattleSolDataCheck, new EZValueChangedDelegate(this.OnClickReleaseEquip), null);
		}
		else if (legendBattleSolDataCheck.GetFriendPersonID() != 0L)
		{
			item.SetListItemData(5, false);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("74");
			item.SetListItemData(8, true);
			item.SetListItemData(9, true);
			item.SetListItemData(9, text, legendBattleSolDataCheck, new EZValueChangedDelegate(this.OnClickUnsetSolHelp), null);
		}
		item.Data = legendBattleSolDataCheck.GetSolID();
	}

	private void OnClickTimeEnd(IUIObject obj)
	{
		Myth_Evolution_Time_DLG myth_Evolution_Time_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_TIME_DLG) as Myth_Evolution_Time_DLG;
		if (myth_Evolution_Time_DLG != null)
		{
			myth_Evolution_Time_DLG.InitSet(MYTH_TYPE.MYTHTYPE_LEGEND, this.m_CharKind_Legendinfo.i32Element_LegendCharkind, 0L);
		}
	}

	protected virtual void OnClickReincarnate(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LEGENDMAKETIME);
			long curTime = PublicMethod.GetCurTime();
			if (curTime < charSubData)
			{
				Myth_Evolution_Time_DLG myth_Evolution_Time_DLG = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MYTH_EVOLUTION_TIME_DLG) as Myth_Evolution_Time_DLG;
				if (myth_Evolution_Time_DLG != null)
				{
					myth_Evolution_Time_DLG.InitSet(MYTH_TYPE.MYTHTYPE_LEGEND, this.m_CharKind_Legendinfo.i32Element_LegendCharkind, 0L);
				}
				return;
			}
		}
		this.SetButtonLegendReincarnate();
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3437");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("369");
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		msgBoxUI.SetMsg(new YesDelegate(this.OnReincarnateOK), null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL, 2);
	}

	public virtual void OnReincarnateOK(object a_oObject)
	{
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		long[] array = new long[5];
		for (int i = 0; i < 5; i++)
		{
			this.m_eElement_Msg[i] = eElement_MsgType.eElement_NOTSOL;
			if (this.m_CharKind_Legendinfo.i32Base_CharKind[i] == 0)
			{
				this.m_eElement_Msg[i] = eElement_MsgType.eElement_OK;
				array[i] = 0L;
			}
			else
			{
				NkSoldierInfo legendSolInfo = this.m_Element_SolID.GetLegendSolInfo(i);
				if (legendSolInfo == null)
				{
					Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1649"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
					return;
				}
				if (legendSolInfo.GetSolPosType() != 0)
				{
					if (legendSolInfo.GetSolPosType() == 2 || legendSolInfo.GetSolPosType() == 6)
					{
						Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("384"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
						return;
					}
					TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : SOL Pos{1}  ==  Char Sol Pos Type", new object[]
					{
						this.m_CharKind_Legendinfo.i32Base_CharKind[i],
						legendSolInfo.GetSolPosType()
					});
					return;
				}
				else
				{
					for (int j = 0; j < 6; j++)
					{
						ITEM item = legendSolInfo.GetEquipItemInfo().m_kItem[j].GetItem();
						if (item == null)
						{
							TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Item pos{1}  ==  ITEM NULL ", new object[]
							{
								this.m_CharKind_Legendinfo.i32Base_CharKind[i],
								j
							});
							return;
						}
						if (item.m_nItemUnique != 0)
						{
							Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("383"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
							return;
						}
					}
					if (legendSolInfo.GetFriendPersonID() != 0L)
					{
						TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Set FriendSOLHELP ", new object[]
						{
							legendSolInfo.GetName()
						});
						return;
					}
					array[i] = legendSolInfo.GetSolID();
				}
			}
		}
		long i64NeedMoney = this.m_CharKind_Legendinfo.i64NeedMoney;
		int itemCnt = NkUserInventory.GetInstance().GetItemCnt(this.m_CharKind_Legendinfo.i32EssenceUnique);
		if (i64NeedMoney > kMyCharInfo.m_Money)
		{
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("89"), SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			return;
		}
		if (itemCnt < this.m_CharKind_Legendinfo.i32NeedEssence)
		{
			string empty = string.Empty;
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_CharKind_Legendinfo.i32EssenceUnique);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("780"),
				"target",
				itemNameByItemUnique
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NORMAL_SYSTEM_MESSAGE);
			return;
		}
		GS_ELEMENT_LEGENDSOL_GET_REQ gS_ELEMENT_LEGENDSOL_GET_REQ = new GS_ELEMENT_LEGENDSOL_GET_REQ();
		gS_ELEMENT_LEGENDSOL_GET_REQ.i64PersonID = kMyCharInfo.m_PersonID;
		gS_ELEMENT_LEGENDSOL_GET_REQ.i32CharKind = this.m_CharKind_Legendinfo.i32Element_LegendCharkind;
		gS_ELEMENT_LEGENDSOL_GET_REQ.i64SolID = array;
		SendPacket.GetInstance().SendObject(1840, gS_ELEMENT_LEGENDSOL_GET_REQ);
	}

	public void AddLegendElement(NkSoldierInfo kSoldierInfo, int iCount)
	{
		byte b = this.m_CharKind_Legendinfo.ui8Base_LegendGrade[iCount] - 1;
		NkSoldierInfo nkSoldierInfo = null;
		eElement_MsgType eElement_MsgType = eElement_MsgType.eElement_NONE;
		this.GetLegendSolGradeCheck(ref nkSoldierInfo, ref eElement_MsgType, kSoldierInfo, b);
		if (nkSoldierInfo == null)
		{
			return;
		}
		NewListItem newListItem = new NewListItem(this.m_NewListBox_Reincarnate.ColumnNum, true, string.Empty);
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!!!!!!!! SolGuild - Element CharKind " + nkSoldierInfo.GetCharKind() + " Error");
			return;
		}
		int num = COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_LEGEND_ADVENT_HERO);
		if (num < 0)
		{
			num = 0;
		}
		this.m_eElement_Msg[iCount] = eElement_MsgType;
		if (kSoldierInfo.GetCharKind() == num)
		{
			this.m_eElement_Msg[iCount] = eElement_MsgType.eElement_OK;
		}
		newListItem.SetListItemData(8, false);
		newListItem.SetListItemData(9, false);
		newListItem.SetListItemData(3, new NkListSolInfo
		{
			SolCharKind = this.m_CharKind_Legendinfo.i32Base_CharKind[iCount],
			SolGrade = (int)b
		}, null, null, null);
		newListItem.SetListItemData(4, charKindInfo.GetName(), null, null, null);
		string text = this.GetElementSolMsg(this.m_eElement_Msg[iCount]);
		newListItem.SetListItemData(5, text, null, null, null);
		if (nkSoldierInfo == null)
		{
			UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_B_Addlist");
			if (uIBaseInfoLoader != null)
			{
				newListItem.SetListItemData(10, uIBaseInfoLoader, iCount, new EZValueChangedDelegate(this.OnClickUseSol), null);
			}
			else
			{
				newListItem.SetListItemData(10, string.Empty, iCount, new EZValueChangedDelegate(this.OnClickUseSol), null);
			}
		}
		else
		{
			UIBaseInfoLoader uIBaseInfoLoader2 = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary("Win_B_Addlist");
			if (uIBaseInfoLoader2 != null)
			{
				newListItem.SetListItemData(10, uIBaseInfoLoader2, iCount, new EZValueChangedDelegate(this.OnClickUseSol), null);
			}
		}
		if (this.m_eElement_Msg[iCount] == eElement_MsgType.eElement_OK || this.m_eElement_Msg[iCount] == eElement_MsgType.eElement_SOLHEIGHT)
		{
			newListItem.SetListItemData(0, true);
			newListItem.SetListItemData(1, false);
			newListItem.SetListItemData(11, false);
			if (nkSoldierInfo == null)
			{
				newListItem.SetListItemData(6, false);
				newListItem.SetListItemData(7, false);
				return;
			}
			newListItem.SetListItemData(6, true);
			newListItem.SetListItemData(7, nkSoldierInfo.GetListSolInfo(false), null, null, null);
			this.m_Element_SolID.SetLegendSol(nkSoldierInfo, iCount);
		}
		else
		{
			newListItem.SetListItemData(0, false);
			newListItem.SetListItemData(1, true);
			if (nkSoldierInfo == null)
			{
				newListItem.SetListItemData(6, false);
				newListItem.SetListItemData(7, false);
				newListItem.SetListItemData(11, false);
				return;
			}
			newListItem.SetListItemData(6, true);
			newListItem.SetListItemData(7, nkSoldierInfo.GetListSolInfo(false), null, null, null);
			newListItem.SetListItemData(11, true);
			this.m_Element_SolID.SetLegendSol(nkSoldierInfo, iCount);
		}
		bool flag = false;
		for (int i = 0; i < 6; i++)
		{
			if (nkSoldierInfo.GetEquipItemInfo() != null)
			{
				ITEM item = nkSoldierInfo.GetEquipItemInfo().m_kItem[i].GetItem();
				if (item == null)
				{
					TsLog.LogWarning("!!!!!!!!!!!!!! CharKind {0} : Item pos{1}  ==  ITEM NULL ", new object[]
					{
						nkSoldierInfo.GetCharKind(),
						i
					});
				}
				else if (item.m_nItemUnique != 0)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			newListItem.SetListItemData(5, false);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("627");
			newListItem.SetListItemData(8, true);
			newListItem.SetListItemData(9, true);
			newListItem.SetListItemData(9, text, nkSoldierInfo, new EZValueChangedDelegate(this.OnClickReleaseEquip), null);
		}
		else if (nkSoldierInfo.GetFriendPersonID() != 0L)
		{
			newListItem.SetListItemData(5, false);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("74");
			newListItem.SetListItemData(8, true);
			newListItem.SetListItemData(9, true);
			newListItem.SetListItemData(9, text, nkSoldierInfo, new EZValueChangedDelegate(this.OnClickUnsetSolHelp), null);
		}
		newListItem.Data = nkSoldierInfo.GetSolID();
		this.m_NewListBox_Reincarnate.RemoveAdd(iCount, newListItem);
		this.m_NewListBox_Reincarnate.RepositionItems();
		this.SetButtonLegendReincarnate();
		bool flag2 = this.SetButtonLegendTime();
		if (!flag2)
		{
			this.m_btn_Legend.SetEnabled(flag2);
		}
		this.GetTimeToString();
	}

	private bool SetButtonLegendReincarnate()
	{
		bool result = true;
		for (int i = 0; i < 5; i++)
		{
			if (this.m_eElement_Msg[i] >= eElement_MsgType.eElement_NOTSOL && this.m_eElement_Msg[i] <= eElement_MsgType.eElement_NEEDGRADE)
			{
				result = false;
			}
		}
		for (int j = 0; j < 5; j++)
		{
			NkSoldierInfo legendSolInfo = this.m_Element_SolID.GetLegendSolInfo(j);
			if (legendSolInfo == null)
			{
				result = false;
				break;
			}
			if (legendSolInfo.GetFriendPersonID() != 0L)
			{
				result = false;
				break;
			}
			for (int k = 0; k < 6; k++)
			{
				if (legendSolInfo.GetEquipItemInfo() != null)
				{
					ITEM item = legendSolInfo.GetEquipItemInfo().m_kItem[k].GetItem();
					if (item != null)
					{
						if (item.m_nItemUnique != 0)
						{
							result = false;
							break;
						}
					}
				}
			}
		}
		return result;
	}

	private bool SetButtonLegendTime()
	{
		bool result = true;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LEGENDMAKETIME);
			long curTime = PublicMethod.GetCurTime();
			if (curTime < charSubData)
			{
				result = false;
			}
		}
		return result;
	}

	private void OnClickUseSol(IUIObject obj)
	{
		int num = (int)obj.Data;
		if (num < 0 || num > 5)
		{
			return;
		}
		long[] array = new long[5];
		for (int i = 0; i < 5; i++)
		{
			NkSoldierInfo legendSolInfo = this.m_Element_SolID.GetLegendSolInfo(i);
			if (legendSolInfo != null)
			{
				array[i] = this.m_Element_SolID.GetLegendSolInfo(i).GetSolID();
			}
			else
			{
				array[i] = 0L;
			}
		}
		SolMilitarySelectDlg solMilitarySelectDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLMILITARYSELECT_DLG) as SolMilitarySelectDlg;
		if (solMilitarySelectDlg != null)
		{
			solMilitarySelectDlg.SetLocationByForm(this);
			solMilitarySelectDlg.SetFocus();
			solMilitarySelectDlg.SetLegendElement(this.m_CharKind_Legendinfo.i32Element_LegendCharkind, array, (byte)num);
			solMilitarySelectDlg.SetSortList();
			TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "MERCENARY", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
	}

	private void Click_SkillInfo(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(this.m_CharKind_Legendinfo.i32Element_LegendCharkind);
		if (solGuild == null)
		{
			return;
		}
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(solGuild.m_i32SkillUnique);
		if (battleSkillBase != null)
		{
			SolDetail_Skill_Dlg solDetail_Skill_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_SKILLICON_DLG) as SolDetail_Skill_Dlg;
			if (solDetail_Skill_Dlg != null)
			{
				solDetail_Skill_Dlg.SetSkillData(solGuild.m_i32SkillUnique, solGuild.m_i32SkillUnique, false);
			}
		}
	}

	protected virtual void Click_PreViewHero(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_CharKind_Legendinfo.i32Element_LegendCharkind);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor(" [Click_PreViewHero] == SOL CHARKIND ERROR {0}" + this.m_CharKind_Legendinfo.i32Element_LegendCharkind + " !!");
			return;
		}
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.MessageBox_PreviewHero), charKindInfo.GetCharKind(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3293"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("438"), eMsgType.MB_OK_CANCEL, 2);
		msgBoxUI.Show();
	}

	private void MessageBox_PreviewHero(object a_oObject)
	{
		int num = (int)a_oObject;
		if (num < 0)
		{
			return;
		}
		GS_PREVIEW_HERO_START_REQ gS_PREVIEW_HERO_START_REQ = new GS_PREVIEW_HERO_START_REQ();
		gS_PREVIEW_HERO_START_REQ.i32CharKind = num;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_PREVIEW_HERO_START_REQ, gS_PREVIEW_HERO_START_REQ);
		HeroCollect_DLG heroCollect_DLG = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.HEROCOLLECT_DLG) as HeroCollect_DLG;
		if (heroCollect_DLG != null)
		{
			NrTSingleton<NkClientLogic>.Instance.GidPrivewHero = 433;
		}
	}

	protected virtual void ClickHelp(IUIObject obj)
	{
		GameHelpList_Dlg gameHelpList_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAME_HELP_LIST) as GameHelpList_Dlg;
		if (gameHelpList_Dlg != null)
		{
			gameHelpList_Dlg.SetViewType(eHELP_LIST.Soldier_Descent.ToString());
		}
	}

	private void Click_Close(IUIObject obj)
	{
		this.Close();
	}

	private NkSoldierInfo GetLegendBattleSolDataCheck(ref eElement_MsgType eElement_Msg, int i32CharKind, byte bCharGrade)
	{
		NkSoldierInfo result = null;
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null || NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo == null)
		{
			return result;
		}
		NkReadySolList readySolList = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetReadySolList();
		foreach (NkSoldierInfo current in readySolList.GetList().Values)
		{
			if (current != null)
			{
				if (current.GetSolID() != NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().GetFaceSolID())
				{
					if (current.GetSolPosType() == 0 && current.GetCharKind() == i32CharKind && current.GetLegendType() == 0 && !current.IsAtbCommonFlag(1L) && !this.m_Element_SolID.GetLegendSolCheck(i32CharKind, current.m_kBase.SolID))
					{
						this.GetLegendSolGradeCheck(ref result, ref eElement_Msg, current, bCharGrade - 1);
					}
				}
			}
		}
		return result;
	}

	private void GetLegendSolGradeCheck(ref NkSoldierInfo pkSolinfo, ref eElement_MsgType eElement_Msg, NkSoldierInfo pkReadySolinfo, byte bCharGrade)
	{
		if (NrTSingleton<ContentsLimitManager>.Instance.IsSolGuideCharKindInfo(pkReadySolinfo.GetCharKind()))
		{
			eElement_Msg = eElement_MsgType.eElement_NOTSOL;
			TsLog.LogOnlyEditor("!!!! CONTENTLIMIT SOL CHARKIND ERROR {0}" + this.m_CharKind_Legendinfo.i32Element_LegendCharkind + " !!");
			return;
		}
		if (pkReadySolinfo == null)
		{
			return;
		}
		if (pkReadySolinfo.IsCostumeEquip())
		{
			return;
		}
		if (pkReadySolinfo.GetMaxSkillLevel())
		{
			if (pkReadySolinfo.GetGrade() >= bCharGrade)
			{
				if (pkReadySolinfo.GetGrade() == bCharGrade)
				{
					if (eElement_Msg == eElement_MsgType.eElement_OK)
					{
						if (pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
						{
							pkSolinfo = pkReadySolinfo;
						}
					}
					else
					{
						eElement_Msg = eElement_MsgType.eElement_OK;
						pkSolinfo = pkReadySolinfo;
					}
				}
				else if (eElement_Msg < eElement_MsgType.eElement_NEEDGRADE)
				{
					eElement_Msg = eElement_MsgType.eElement_SOLHEIGHT;
					pkSolinfo = pkReadySolinfo;
				}
				else if (eElement_Msg == eElement_MsgType.eElement_SOLHEIGHT && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
				{
					pkSolinfo = pkReadySolinfo;
				}
			}
			else if (eElement_Msg < eElement_MsgType.eElement_NEEDGRADE)
			{
				eElement_Msg = eElement_MsgType.eElement_NEEDGRADE;
				pkSolinfo = pkReadySolinfo;
			}
			else if (eElement_Msg == eElement_MsgType.eElement_NEEDGRADE && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
			{
				pkSolinfo = pkReadySolinfo;
			}
		}
		else if (eElement_Msg < eElement_MsgType.eElement_NEEDEXP)
		{
			eElement_Msg = eElement_MsgType.eElement_NEEDEXP;
			pkSolinfo = pkReadySolinfo;
		}
		else if (eElement_Msg == eElement_MsgType.eElement_NEEDEXP && pkSolinfo.GetEvolutionExp() > pkReadySolinfo.GetEvolutionExp())
		{
			pkSolinfo = pkReadySolinfo;
		}
	}

	protected void OnClickUnsetSolHelp(IUIObject obj)
	{
		NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)obj.Data;
		if (nkSoldierInfo == null)
		{
			return;
		}
		GS_FRIEND_HELPSOL_UNSET_REQ gS_FRIEND_HELPSOL_UNSET_REQ = new GS_FRIEND_HELPSOL_UNSET_REQ();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64FriendPersonID = nkSoldierInfo.GetFriendPersonID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64SolID = nkSoldierInfo.GetSolID();
		gS_FRIEND_HELPSOL_UNSET_REQ.i64AddExp = nkSoldierInfo.AddHelpExp;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_FRIEND_HELPSOL_UNSET_REQ, gS_FRIEND_HELPSOL_UNSET_REQ);
	}

	protected void OnClickReleaseEquip(IUIObject obj)
	{
		UIButton uIButton = obj as UIButton;
		if (uIButton == null)
		{
			return;
		}
		NkSoldierInfo nkSoldierInfo = uIButton.Data as NkSoldierInfo;
		if (nkSoldierInfo != null)
		{
			Protocol_Item.Send_EquipSol_InvenEquip_All(nkSoldierInfo);
		}
	}

	protected string GetElementSolMsg(eElement_MsgType eType)
	{
		string result = string.Empty;
		switch (eType)
		{
		case eElement_MsgType.eElement_NOTSOL:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1649");
			break;
		case eElement_MsgType.eElement_NEEDEXP:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1650");
			break;
		case eElement_MsgType.eElement_NEEDGRADE:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1651");
			break;
		case eElement_MsgType.eElement_SOLHEIGHT:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1691");
			break;
		case eElement_MsgType.eElement_OK:
			result = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1575");
			break;
		}
		return result;
	}

	public void ElementSolSuccess(SOLDIER_INFO pkSolInfo)
	{
		SolElementSuccessDlg solElementSuccessDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLELEMENTSUCCESS_DLG) as SolElementSuccessDlg;
		if (solElementSuccessDlg != null)
		{
			solElementSuccessDlg.LoadLegendSolCompleteElement(pkSolInfo);
		}
	}
}
