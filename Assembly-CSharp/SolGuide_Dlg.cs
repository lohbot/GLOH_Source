using GAME;
using PROTOCOL;
using System;
using System.Collections.Generic;
using UnityForms;

public class SolGuide_Dlg : Form
{
	private const int MAX_SLOT_COUNT = 27;

	private DropDownList m_DropDownList_Season;

	private DropDownList m_DropDownList_Setorder;

	private SOLGUIDE_SLOT[] m_Sol_Slot = new SOLGUIDE_SLOT[27];

	private CheckBox m_CheckBox_Alchemy;

	private Label m_Label_Alchemy;

	private bool bElementSolCheck;

	private Box m_Box_Box19;

	private Button m_Button_LeftArr;

	private Button m_Button_RightArr;

	private byte bCurrentSeason;

	private byte bCurrentPage;

	private byte bMaxPage;

	private Dictionary<byte, List<SolSlotData>> dicSlotData = new Dictionary<byte, List<SolSlotData>>();

	private SolGuideSlot m_eSolSortType;

	private NrCharKindInfo kCharKindInfo;

	private List<SOLGUIDE_DATA> m_SolGuideList = new List<SOLGUIDE_DATA>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "SolGuide/DLG_SolGuide", G_ID.SOLGUIDE_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_DropDownList_Season = (base.GetControl("DDL_Season") as DropDownList);
		this.m_DropDownList_Season.Reserve = false;
		this.m_DropDownList_Setorder = (base.GetControl("DDL_Setorder") as DropDownList);
		this.m_DropDownList_Setorder.Reserve = false;
		for (int i = 0; i < 27; i++)
		{
			this.m_Sol_Slot[i] = new SOLGUIDE_SLOT();
			this.m_Sol_Slot[i].DT_Slot = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DT_SlotBg", (i + 1).ToString())) as DrawTexture);
			this.m_Sol_Slot[i].IT_Slot = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("IT_SOL", (i + 1).ToString())) as ItemTexture);
			this.m_Sol_Slot[i].BT_Slot = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("BT_Slot", (i + 1).ToString())) as Button);
			this.m_Sol_Slot[i].DT_Event = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DT_Event", (i + 1).ToString())) as DrawTexture);
			this.m_Sol_Slot[i].DT_BlackSlot = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("DT_DarkIcon", (i + 1).ToString())) as DrawTexture);
			if (i < 9)
			{
				this.m_Sol_Slot[i].Box_New = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("BOX_NEW0", (i + 1).ToString())) as Box);
			}
			else
			{
				this.m_Sol_Slot[i].Box_New = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("BOX_NEW", (i + 1).ToString())) as Box);
			}
			this.m_Sol_Slot[i].Box_New.DeleteSpriteText();
			this.m_Sol_Slot[i].BT_Slot.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnSelectSolSlot));
			this.m_Sol_Slot[i].BT_Slot.data = i;
			this.m_Sol_Slot[i].BT_Slot.DeleteSpriteText();
		}
		this.m_CheckBox_Alchemy = (base.GetControl("CheckBox_Alchemy") as CheckBox);
		CheckBox expr_249 = this.m_CheckBox_Alchemy;
		expr_249.CheckedChanged = (EZValueChangedDelegate)Delegate.Combine(expr_249.CheckedChanged, new EZValueChangedDelegate(this.CheckChange));
		this.m_Label_Alchemy = (base.GetControl("Label_Alchemy") as Label);
		this.m_Box_Box19 = (base.GetControl("Box_Box19") as Box);
		this.m_Button_LeftArr = (base.GetControl("Bt_LeftArr") as Button);
		this.m_Button_RightArr = (base.GetControl("BT_RightArr") as Button);
		this.m_DropDownList_Season.AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_Season));
		this.m_DropDownList_Setorder.AddValueChangedDelegate(new EZValueChangedDelegate(this.Change_Setorder));
		this.m_Button_LeftArr.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickLeftArr));
		this.m_Button_RightArr.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_ClickRightArr));
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsAlchemy())
		{
			this.m_CheckBox_Alchemy.Visible = false;
			this.m_Label_Alchemy.Visible = false;
		}
		else
		{
			this.m_CheckBox_Alchemy.Visible = true;
			this.m_Label_Alchemy.Visible = true;
		}
	}

	public void SetGuideGuiSet(bool bElementMark)
	{
		this.InitGuideDataSet();
		this.SetDropDownList_Season();
		this.SetDropDownList_Setorder();
		this.m_eSolSortType = SolGuideSlot.SLOTTYPE_GRADE_ASCENDING;
		this.SetGuideDataSort(this.bCurrentSeason, this.m_eSolSortType);
		this.SetViewSolGuide_Data(this.bCurrentSeason, this.bCurrentPage);
		if (!bElementMark)
		{
			this.bElementSolCheck = true;
			this.m_CheckBox_Alchemy.SetCheckState(0);
		}
		else
		{
			this.bElementSolCheck = false;
			this.m_CheckBox_Alchemy.SetCheckState(1);
		}
	}

	private void OnSelectSolSlot(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		int num = (int)obj.Data;
		SolSlotData slotListData = this.GetSlotListData(num);
		if (slotListData == null)
		{
			TsLog.LogWarning(" Sol Guide Slot Not Data Button {0}", new object[]
			{
				num
			});
			return;
		}
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.SOLDETAIL_DLG) && slotListData != null && slotListData.i32KindInfo != 0)
		{
			SolDetail_Info_Dlg solDetail_Info_Dlg = (SolDetail_Info_Dlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.SOLDETAIL_DLG);
			if (!solDetail_Info_Dlg.Visible)
			{
				solDetail_Info_Dlg.Show();
			}
			solDetail_Info_Dlg.SetSolKind(slotListData);
		}
	}

	private void SetDropDownList_Season()
	{
		this.m_DropDownList_Season.SetViewArea(this.dicSlotData.Count);
		this.m_DropDownList_Season.Clear();
		this.bCurrentSeason = 0;
		this.bCurrentPage = 1;
		this.bMaxPage = 1;
		string str = string.Empty;
		foreach (byte current in this.dicSlotData.Keys)
		{
			ListItem listItem = new ListItem();
			listItem.Key = current;
			if (current == 0)
			{
				str = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1943");
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref str, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2302"),
					"count",
					current.ToString()
				});
			}
			listItem.SetColumnStr(0, str);
			this.m_DropDownList_Season.Add(listItem);
		}
		this.m_DropDownList_Season.RepositionItems();
		this.m_DropDownList_Season.SetFirstItem();
	}

	private void SetDropDownList_Setorder()
	{
		this.m_DropDownList_Setorder.SetViewArea(3);
		this.m_DropDownList_Setorder.Clear();
		ListItem listItem = new ListItem();
		listItem.Key = SolGuideSlot.SLOTTYPE_GRADE_ASCENDING;
		listItem.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1891"));
		this.m_DropDownList_Setorder.Add(listItem);
		ListItem listItem2 = new ListItem();
		listItem2.Key = SolGuideSlot.SLOTTYPE_GRADE_DESCENDING;
		listItem2.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1892"));
		this.m_DropDownList_Setorder.Add(listItem2);
		ListItem listItem3 = new ListItem();
		listItem3.Key = SolGuideSlot.SLOTTYPE_NAME;
		listItem3.SetColumnStr(0, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1890"));
		this.m_DropDownList_Setorder.Add(listItem3);
		this.m_DropDownList_Setorder.RepositionItems();
		this.m_DropDownList_Setorder.SetFirstItem();
		this.m_eSolSortType = SolGuideSlot.SLOTTYPE_GRADE_ASCENDING;
	}

	private void Change_Season(IUIObject obj)
	{
		ListItem listItem = this.m_DropDownList_Season.SelectedItem.Data as ListItem;
		if (this.bCurrentSeason != (byte)listItem.Key)
		{
			this.bCurrentSeason = (byte)listItem.Key;
			this.bCurrentPage = 1;
			this.SetGuideDataSort(this.bCurrentSeason, this.m_eSolSortType);
			this.SetViewSolGuide_Data(this.bCurrentSeason, this.bCurrentPage);
		}
	}

	private void Change_Setorder(IUIObject obj)
	{
		ListItem listItem = this.m_DropDownList_Setorder.SelectedItem.Data as ListItem;
		if (this.m_eSolSortType != (SolGuideSlot)((int)listItem.Key))
		{
			this.m_eSolSortType = (SolGuideSlot)((int)listItem.Key);
			this.SetGuideDataSort(this.bCurrentSeason, this.m_eSolSortType);
			this.SetViewSolGuide_Data(this.bCurrentSeason, this.bCurrentPage);
		}
	}

	public void On_ClickLeftArr(IUIObject a_cObject)
	{
		if (this.bCurrentPage > 1)
		{
			this.bCurrentPage -= 1;
		}
		this.SetViewSolGuide_Data(this.bCurrentSeason, this.bCurrentPage);
	}

	public void On_ClickRightArr(IUIObject a_cObject)
	{
		if (this.bCurrentPage < this.bMaxPage)
		{
			this.bCurrentPage += 1;
		}
		this.SetViewSolGuide_Data(this.bCurrentSeason, this.bCurrentPage);
	}

	public void ChageClickText()
	{
		string @string = NrTSingleton<UIDataManager>.Instance.GetString(this.bCurrentPage.ToString(), "/", this.bMaxPage.ToString());
		this.m_Box_Box19.SetText(@string);
	}

	private void CheckChange(IUIObject a_cUIObject)
	{
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsAlchemy())
		{
			this.m_Label_Alchemy.Visible = false;
			return;
		}
		this.bElementSolCheck = !this.bElementSolCheck;
		this.m_CheckBox_Alchemy.SetCheckState((!this.bElementSolCheck) ? 0 : 1);
		this.SetViewSolGuide_Data(this.bCurrentSeason, this.bCurrentPage);
	}

	public override void CloseForm(IUIObject obj)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		myCharInfo.InitCharSolGuide();
		base.CloseForm(obj);
	}

	private void InitGuideDataSet()
	{
		this.dicSlotData.Clear();
		this.dicSlotData = new Dictionary<byte, List<SolSlotData>>();
		List<SOL_GUIDE> value = NrTSingleton<NrTableSolGuideManager>.Instance.GetValue();
		if (value == null)
		{
			return;
		}
		for (int i = 0; i < value.Count; i++)
		{
			SOL_GUIDE sOL_GUIDE = value[i];
			if (sOL_GUIDE != null)
			{
				this.kCharKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(sOL_GUIDE.m_i32CharKind);
				if (this.kCharKindInfo != null)
				{
					if (sOL_GUIDE.m_bSeason != 0)
					{
						if (!NrTSingleton<ContentsLimitManager>.Instance.IsSolGuide_Season((int)sOL_GUIDE.m_bSeason))
						{
							if (!this.IsSolGuideData(sOL_GUIDE.m_i32CharKind))
							{
								if (!NrTSingleton<ContentsLimitManager>.Instance.IsSolGuideCharKindInfo(sOL_GUIDE.m_i32CharKind))
								{
									SolSlotData slotData;
									if (this.kCharKindInfo != null)
									{
										slotData = new SolSlotData(this.kCharKindInfo.GetName(), sOL_GUIDE.m_i32CharKind, (byte)sOL_GUIDE.m_iSolGrade, sOL_GUIDE.m_bFlagSet, sOL_GUIDE.m_bFlagSetCount - 1, sOL_GUIDE.m_bSeason, sOL_GUIDE.m_i32SkillUnique, sOL_GUIDE.m_i32SkillText);
									}
									else
									{
										slotData = new SolSlotData(" ", 0, 0, 0, 0, 0, 0, 0);
									}
									this.SetGuideTableData(0, slotData);
									this.SetGuideTableData(sOL_GUIDE.m_bSeason, slotData);
								}
							}
						}
					}
				}
			}
		}
	}

	private void SetGuideTableData(byte bSeason, SolSlotData SlotData)
	{
		List<SolSlotData> list = null;
		if (this.dicSlotData.ContainsKey(bSeason))
		{
			this.dicSlotData.TryGetValue(bSeason, out list);
			if (list == null)
			{
				list = new List<SolSlotData>();
				list.Add(SlotData);
				this.dicSlotData.Add(bSeason, list);
			}
			else
			{
				list.Add(SlotData);
			}
		}
		else
		{
			list = new List<SolSlotData>();
			list.Add(SlotData);
			this.dicSlotData.Add(bSeason, list);
		}
	}

	private void SetViewSolGuide_Data(byte bSeason, byte bPage)
	{
		if (bSeason == 0 && bPage == 0)
		{
			return;
		}
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsAlchemy())
		{
			this.SetViewSolGuide(bSeason, bPage);
		}
		else if (this.bElementSolCheck)
		{
			this.SetViewSolElement(bSeason, bPage);
		}
		else
		{
			this.SetViewSolGuide(bSeason, bPage);
		}
	}

	private void SetGuideDataSort(byte bSeason, SolGuideSlot eSortType)
	{
		List<SolSlotData> list = null;
		if (this.dicSlotData.ContainsKey(bSeason))
		{
			this.dicSlotData.TryGetValue(bSeason, out list);
			list.Sort(new Comparison<SolSlotData>(this.CompareSolName));
			if (list == null)
			{
				TsLog.LogWarning("!!!!!!!!!!!!!!!!!!! Not Data List {0}, {1}", new object[]
				{
					bSeason,
					eSortType
				});
			}
			else
			{
				switch (eSortType)
				{
				case SolGuideSlot.SLOTTYPE_NAME:
					break;
				case SolGuideSlot.SLOTTYPE_GRADE_ASCENDING:
					list.Sort(new Comparison<SolSlotData>(this.CompareSolGradeAscend));
					break;
				case SolGuideSlot.SLOTTYPE_GRADE_DESCENDING:
					list.Sort(new Comparison<SolSlotData>(this.CompareSolGradeDescend));
					break;
				default:
					TsLog.LogWarning("!!!!!!!!!!!!! Sort Type Error {0}", new object[]
					{
						eSortType
					});
					break;
				}
			}
		}
	}

	private int CompareSolName(SolSlotData a, SolSlotData b)
	{
		return a.strSolName.CompareTo(b.strSolName);
	}

	private int CompareSolGradeAscend(SolSlotData a, SolSlotData b)
	{
		if (a.bSolGrade < b.bSolGrade)
		{
			return 1;
		}
		if (a.bSolGrade != b.bSolGrade)
		{
			return -1;
		}
		if (a.bBitFlagCount < b.bBitFlagCount)
		{
			return 1;
		}
		return 0;
	}

	private int CompareSolGradeDescend(SolSlotData a, SolSlotData b)
	{
		if (a.bSolGrade > b.bSolGrade)
		{
			return 1;
		}
		if (a.bSolGrade != b.bSolGrade)
		{
			return -1;
		}
		if (a.bBitFlagCount < b.bBitFlagCount)
		{
			return 1;
		}
		return 0;
	}

	public SolSlotData GetSlotListData(int nClickIndex)
	{
		List<SolSlotData> list = null;
		SolSlotData result = null;
		if (this.dicSlotData.ContainsKey(this.bCurrentSeason))
		{
			this.dicSlotData.TryGetValue(this.bCurrentSeason, out list);
			if (list == null)
			{
				return null;
			}
			int num = (int)((this.bCurrentPage - 1) * 27) + nClickIndex;
			if (list.Count > num)
			{
				result = list[num];
			}
		}
		return result;
	}

	private void SetViewSolGuide(byte bSeason, byte bPage)
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		List<SolSlotData> list = null;
		if (this.dicSlotData.ContainsKey(bSeason))
		{
			this.dicSlotData.TryGetValue(bSeason, out list);
			int num = (int)((bPage - 1) * 27);
			int count = list.Count;
			this.bMaxPage = (byte)(count / 27);
			if (count % 27 > 0)
			{
				this.bMaxPage += 1;
			}
			for (int i = 0; i < 27; i++)
			{
				if (count > num)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(list[num].i32KindInfo);
					long charSubData = myCharInfo.GetCharSubData((int)list[num].bBitFlag + eCHAR_SUBDATA.CHAR_SUBDATA_SOLGUIDE_0 - 1);
					if (charKindInfo != null)
					{
						this.m_Sol_Slot[i].IT_Slot.SetSolImageTexure(eCharImageType.SMALL, charKindInfo.GetCharKind(), (int)(list[num].bSolGrade - 1));
					}
					else
					{
						this.m_Sol_Slot[i].IT_Slot.SetTexture("Win_T_ItemEmpty");
					}
					this.m_Sol_Slot[i].DT_Event.Hide(true);
					this.m_Sol_Slot[i].Box_New.Hide(true);
					if ((charSubData & 1L << (int)list[num].bBitFlagCount) != 0L)
					{
						this.m_Sol_Slot[i].DT_BlackSlot.Hide(true);
						long charSolGuide = myCharInfo.GetCharSolGuide((int)(list[num].bBitFlag - 1));
						if ((charSolGuide & 1L << (int)list[num].bBitFlagCount) != 0L)
						{
							this.m_Sol_Slot[i].Box_New.Hide(false);
						}
						else
						{
							this.m_Sol_Slot[i].Box_New.Hide(true);
						}
					}
					else
					{
						this.m_Sol_Slot[i].DT_BlackSlot.Hide(false);
						this.m_Sol_Slot[i].Box_New.Hide(true);
					}
					if (NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(charKindInfo.GetCharKind(), list[num].bSolGrade) == null)
					{
						UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(charKindInfo.GetCharKind(), (int)list[num].bSolGrade);
						if (legendFrame != null)
						{
							this.m_Sol_Slot[i].DT_Slot.SetTexture(legendFrame);
						}
						else
						{
							this.m_Sol_Slot[i].DT_Slot.SetTexture("Win_T_ItemEmpty");
						}
					}
					else
					{
						this.m_Sol_Slot[i].DT_Event.Hide(false);
						this.m_Sol_Slot[i].DT_Slot.SetTexture("Win_I_EventSol");
					}
				}
				else
				{
					this.m_Sol_Slot[i].IT_Slot.ClearData();
					this.m_Sol_Slot[i].IT_Slot.SetTexture("Win_T_ItemEmpty");
					this.m_Sol_Slot[i].DT_BlackSlot.Hide(false);
					this.m_Sol_Slot[i].Box_New.Hide(true);
					this.m_Sol_Slot[i].DT_Event.Hide(true);
					this.m_Sol_Slot[i].DT_Slot.SetTexture("Win_T_ItemEmpty");
				}
				num++;
			}
		}
		else
		{
			TsLog.LogWarning("!!!!!!!!!!!!!!! Not NDT File", new object[0]);
		}
		this.ChageClickText();
	}

	private void SetViewSolElement(byte bSeason, byte bPage)
	{
		List<SolSlotData> list = null;
		if (this.dicSlotData.ContainsKey(bSeason))
		{
			this.dicSlotData.TryGetValue(bSeason, out list);
			int num = (int)((bPage - 1) * 27);
			int count = list.Count;
			this.bMaxPage = (byte)(count / 27);
			if (count % 27 > 0)
			{
				this.bMaxPage += 1;
			}
			for (int i = 0; i < 27; i++)
			{
				if (count > num)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(list[num].i32KindInfo);
					if (charKindInfo != null)
					{
						this.m_Sol_Slot[i].IT_Slot.SetSolImageTexure(eCharImageType.SMALL, charKindInfo.GetCharKind(), (int)(list[num].bSolGrade - 1));
					}
					else
					{
						this.m_Sol_Slot[i].IT_Slot.SetTexture("Win_T_ItemEmpty");
					}
					this.m_Sol_Slot[i].DT_Event.Hide(true);
					this.m_Sol_Slot[i].Box_New.Hide(true);
					if (NrTSingleton<ContentsLimitManager>.Instance.IsElementKind(list[num].i32KindInfo))
					{
						this.m_Sol_Slot[i].DT_BlackSlot.Hide(false);
					}
					else
					{
						bool tf = false;
						if (NrTSingleton<NrTableSolGuideManager>.Instance.GetCharKindAlchemy(list[num].i32KindInfo))
						{
							tf = true;
						}
						if (NrTSingleton<NrTableSolGuideManager>.Instance.GetCharKindLegend(list[num].i32KindInfo))
						{
							tf = true;
						}
						this.m_Sol_Slot[i].DT_BlackSlot.Hide(tf);
					}
					if (NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(charKindInfo.GetCharKind(), list[num].bSolGrade) == null)
					{
						UIBaseInfoLoader legendFrame = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendFrame(charKindInfo.GetCharKind(), (int)list[num].bSolGrade);
						if (legendFrame != null)
						{
							this.m_Sol_Slot[i].DT_Slot.SetTexture(legendFrame);
						}
						else
						{
							this.m_Sol_Slot[i].DT_Slot.SetTexture("Win_T_ItemEmpty");
						}
					}
					else
					{
						this.m_Sol_Slot[i].DT_Event.Hide(false);
						this.m_Sol_Slot[i].DT_Slot.SetTexture("Win_I_EventSol");
					}
				}
				else
				{
					this.m_Sol_Slot[i].IT_Slot.ClearData();
					this.m_Sol_Slot[i].IT_Slot.SetTexture("Win_T_ItemEmpty");
					this.m_Sol_Slot[i].DT_BlackSlot.Hide(false);
					this.m_Sol_Slot[i].Box_New.Hide(true);
					this.m_Sol_Slot[i].DT_Event.Hide(true);
					this.m_Sol_Slot[i].DT_Slot.SetTexture("Win_T_ItemEmpty");
				}
				num++;
			}
		}
		else
		{
			TsLog.LogWarning("!!!!!!!!!!!!!!! Not NDT File", new object[0]);
		}
		this.ChageClickText();
	}

	public void ClearSolGuideInfo()
	{
		this.m_SolGuideList.Clear();
	}

	public void AddSolGuideInfo(SOLGUIDE_DATA Data)
	{
		for (int i = 0; i < this.m_SolGuideList.Count; i++)
		{
			if (this.m_SolGuideList[i].i32CharKind == Data.i32CharKind)
			{
				this.m_SolGuideList[i] = Data;
				return;
			}
		}
		this.m_SolGuideList.Add(Data);
	}

	public bool IsSolGuideData(int iCharKind)
	{
		for (int i = 0; i < this.m_SolGuideList.Count; i++)
		{
			if (this.m_SolGuideList[i].i32CharKind == iCharKind)
			{
				return true;
			}
		}
		return false;
	}
}
