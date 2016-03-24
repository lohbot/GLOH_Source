using CostumeDlg;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class CostumeGuide_Dlg : Form
{
	public const int MAX_SLOT_COUNT = 27;

	public const int NOT_EXIST = -1;

	private DropDownList _DDL_Season;

	private DropDownList _DDL_Setorder;

	private Box _Box_Box19;

	private COSTUMEGUIDE_SLOT[] _slotList;

	private byte _currentSeason;

	private byte _currentPage;

	public COSTUME_SORT _currentSort;

	public byte _maxPage;

	public byte ENTIRE_SEASON;

	private Dictionary<byte, List<SolSlotData>> _slotDataDic;

	private ComponentSetter _componentSetter;

	private InitProcessor _initProcessor;

	private SlotDataProcessor _slotDataProcessor;

	public override void InitializeComponent()
	{
		Form form = this;
		base.Scale = true;
		NrTSingleton<UIBaseFileManager>.Instance.LoadFileAll(ref form, "SolGuide/DLG_CostumeBook", G_ID.COSTUMEGUIDE_DLG, false, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		base.Draggable = false;
		this.Init();
	}

	public override void SetComponent()
	{
		this._DDL_Season = this._componentSetter.SetDDLSeason(this, new EZValueChangedDelegate(this.Change_Season));
		this._DDL_Setorder = this._componentSetter.SetDDLOrder(this, new EZValueChangedDelegate(this.Change_Setorder));
		this._slotList = this._componentSetter.SetSlotListComponent(this, 27, new EZValueChangedDelegate(this.OnSelectSolSlot));
		this._Box_Box19 = (base.GetControl("Box_Box19") as Box);
	}

	public void SetCostumeGuide()
	{
		this._initProcessor.InitCostumeSlotData(this, ref this._slotDataDic);
		this._initProcessor.InitDropDownList_Season(this, ref this._DDL_Season);
		this._initProcessor.InitDropDownList_Setorder(this, ref this._DDL_Setorder);
		this._currentSort = COSTUME_SORT.SLOTTYPE_GRADE_ASCENDING;
		List<SolSlotData> solSlotData = this._slotDataProcessor.GetSolSlotData(ref this._slotDataDic, this._currentSeason, this._currentSort);
		this._initProcessor.InitViewCostumeGuide(this, this._slotList, solSlotData, this._currentPage);
	}

	public void ChangePageText()
	{
		string @string = NrTSingleton<UIDataManager>.Instance.GetString(this._currentPage.ToString(), "/", this._maxPage.ToString());
		this._Box_Box19.SetText(@string);
	}

	private void On_ClickLeftArr(IUIObject a_cObject)
	{
		int currentPage = (int)this._currentPage;
		if (1 < this._currentPage)
		{
			this._currentPage -= 1;
		}
		if (currentPage == (int)this._currentPage)
		{
			return;
		}
		List<SolSlotData> solSlotData = this._slotDataProcessor.GetSolSlotData(ref this._slotDataDic, this._currentSeason, this._currentSort);
		this._initProcessor.InitViewCostumeGuide(this, this._slotList, solSlotData, this._currentPage);
	}

	private void On_ClickRightArr(IUIObject a_cObject)
	{
		int currentPage = (int)this._currentPage;
		if (this._currentPage < this._maxPage)
		{
			this._currentPage += 1;
		}
		if (currentPage == (int)this._currentPage)
		{
			return;
		}
		List<SolSlotData> solSlotData = this._slotDataProcessor.GetSolSlotData(ref this._slotDataDic, this._currentSeason, this._currentSort);
		this._initProcessor.InitViewCostumeGuide(this, this._slotList, solSlotData, this._currentPage);
	}

	private void OnSelectSolSlot(IUIObject obj)
	{
		if (obj == null || obj.Data == null)
		{
			return;
		}
		int num = (int)obj.Data;
		if (num == -1)
		{
			return;
		}
		CostumeRoom_Dlg costumeRoom_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COSTUMEROOM_DLG) as CostumeRoom_Dlg;
		if (costumeRoom_Dlg == null)
		{
			return;
		}
		costumeRoom_Dlg.InitCostumeRoom(num, null);
		costumeRoom_Dlg.Show();
	}

	private void Change_Season(IUIObject obj)
	{
		if (this._DDL_Season == null || this._DDL_Season.SelectedItem == null || this._DDL_Season.SelectedItem.Data == null)
		{
			Debug.LogError("ERROR, CostumeGuide_Dlg.cs, Change_Season(), _DDL_Season is Null");
			return;
		}
		ListItem listItem = this._DDL_Season.SelectedItem.Data as ListItem;
		if (listItem == null)
		{
			return;
		}
		byte b = (byte)listItem.Key;
		if (this._currentSeason == b)
		{
			return;
		}
		this._currentPage = 1;
		this._currentSeason = b;
		List<SolSlotData> solSlotData = this._slotDataProcessor.GetSolSlotData(ref this._slotDataDic, this._currentSeason, this._currentSort);
		this._initProcessor.InitViewCostumeGuide(this, this._slotList, solSlotData, this._currentPage);
	}

	private void Change_Setorder(IUIObject obj)
	{
		if (this._DDL_Setorder == null || this._DDL_Setorder.SelectedItem == null || this._DDL_Setorder.SelectedItem.data == null)
		{
			Debug.LogError("ERROR, CostumeGuide_Dlg.cs, Change_Setorder(), _DDL_Season is Null");
			return;
		}
		ListItem listItem = this._DDL_Setorder.SelectedItem.Data as ListItem;
		if (listItem == null)
		{
			return;
		}
		COSTUME_SORT cOSTUME_SORT = (COSTUME_SORT)((int)listItem.Key);
		if (this._currentSort == cOSTUME_SORT)
		{
			return;
		}
		this._currentSort = cOSTUME_SORT;
		List<SolSlotData> solSlotData = this._slotDataProcessor.GetSolSlotData(ref this._slotDataDic, this._currentSeason, cOSTUME_SORT);
		this._initProcessor.InitViewCostumeGuide(this, this._slotList, solSlotData, this._currentPage);
	}

	private void Init()
	{
		this._slotList = new COSTUMEGUIDE_SLOT[27];
		this._slotDataDic = new Dictionary<byte, List<SolSlotData>>();
		this._componentSetter = new ComponentSetter();
		this._initProcessor = new InitProcessor();
		this._slotDataProcessor = new SlotDataProcessor();
		this.ENTIRE_SEASON = 0;
		this._currentPage = 1;
		this._maxPage = 1;
	}
}
