using GAME;
using PROTOCOL;
using PROTOCOL.GAME.ID;
using System;
using UnityForms;

public class Myth_Evolution_Time_DLG : Form
{
	private Label m_LB_MythEvolutionTime;

	private Label m_LB_LegendEvolutionTime;

	private Button m_Button_MINUS;

	private Button m_Button_PLUS;

	private Button m_Button_Button01;

	private Button m_Button_Button02;

	private Button m_Button_NumPad;

	private HorizontalSlider m_HSlider_HSlider1;

	private Label m_Label_Count;

	private DrawTexture m_DT_elixir;

	private int m_CurItemUseNum = 1;

	private int m_MaxItemUseNum = 1;

	private float m_fSliderValue;

	private MYTH_TYPE m_MythType;

	private int m_i32CharKind;

	private long m_i64SolID;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Soldier/Evolution/DLG_SolEvolutionTimeWarning", G_ID.MYTH_EVOLUTION_TIME_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_LB_MythEvolutionTime = (base.GetControl("LB_MythEvolutionTime") as Label);
		this.m_LB_LegendEvolutionTime = (base.GetControl("LB_LegendEvolutionTime") as Label);
		this.m_Button_MINUS = (base.GetControl("Button_MINUS") as Button);
		this.m_Button_MINUS.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnValueMinus));
		this.m_Button_PLUS = (base.GetControl("Button_PLUS") as Button);
		this.m_Button_PLUS.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnValueAdd));
		this.m_Button_Button01 = (base.GetControl("Button_Button01") as Button);
		this.m_Button_Button01.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnCanCelClose));
		this.m_Button_Button02 = (base.GetControl("Button_Button02") as Button);
		this.m_Button_Button02.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnItemUse));
		this.m_Button_NumPad = (base.GetControl("Button_NumPad") as Button);
		this.m_Button_NumPad.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickNumPad));
		this.m_Label_Count = (base.GetControl("Label_Count") as Label);
		this.m_DT_elixir = (base.GetControl("DT_elixir") as DrawTexture);
		this.m_HSlider_HSlider1 = (base.GetControl("HSlider_HSlider1") as HorizontalSlider);
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
		if (null != base.BLACK_BG)
		{
			base.BLACK_BG.RemoveValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
		}
	}

	public void InitSet(MYTH_TYPE e_MythType, int i32CharKind, long i64SolID)
	{
		this.m_MythType = e_MythType;
		this.m_i32CharKind = i32CharKind;
		this.m_i64SolID = i64SolID;
		string textureFromBundle = "ui/itemshop/itemshop_elixir01";
		this.m_DT_elixir.SetTextureFromBundle(textureFromBundle);
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long num = 0L;
			if (this.m_MythType == MYTH_TYPE.MYTHTYPE_LEGEND)
			{
				num = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LEGENDMAKETIME);
				base.SetShowLayer(1, true);
				base.SetShowLayer(2, false);
			}
			else if (this.m_MythType == MYTH_TYPE.MYTHTYPE_EVOLUTION)
			{
				num = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MYTH_EVOLUTION_TIME);
				base.SetShowLayer(1, false);
				base.SetShowLayer(2, true);
			}
			long curTime = PublicMethod.GetCurTime();
			if (curTime > num)
			{
				base.CloseNow();
				return;
			}
			long iSec = num - curTime;
			int num2 = NkUserInventory.GetInstance().Get_First_ItemCnt(50317);
			long num3 = PublicMethod.GetTotalDayFromSec(iSec);
			long hourFromSec = PublicMethod.GetHourFromSec(iSec);
			if (this.m_MythType == MYTH_TYPE.MYTHTYPE_LEGEND)
			{
				base.SetShowLayer(1, true);
				base.SetShowLayer(2, false);
				if (hourFromSec > 0L)
				{
					num3 += 1L;
				}
				if ((long)num2 < num3)
				{
					this.m_MaxItemUseNum = num2;
				}
				else
				{
					this.m_MaxItemUseNum = (int)num3;
				}
			}
			else if (this.m_MythType == MYTH_TYPE.MYTHTYPE_EVOLUTION)
			{
				num3 *= 2L;
				base.SetShowLayer(1, false);
				base.SetShowLayer(2, true);
				if (hourFromSec > 0L)
				{
					if (hourFromSec > 12L)
					{
						num3 += 2L;
					}
					else
					{
						num3 += 1L;
					}
				}
				this.m_MaxItemUseNum = (int)num3;
			}
			this.SetItemText();
		}
	}

	private void SetItemText()
	{
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo != null)
		{
			long num = 0L;
			if (this.m_MythType == MYTH_TYPE.MYTHTYPE_LEGEND)
			{
				num = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_LEGENDMAKETIME);
			}
			else if (this.m_MythType == MYTH_TYPE.MYTHTYPE_EVOLUTION)
			{
				num = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_MYTH_EVOLUTION_TIME);
			}
			long num2 = num - PublicMethod.GetCurTime();
			if (num2 <= 0L)
			{
				base.CloseNow();
				return;
			}
			long totalDayFromSec = PublicMethod.GetTotalDayFromSec(num2);
			long hourFromSec = PublicMethod.GetHourFromSec(num2);
			long minuteFromSec = PublicMethod.GetMinuteFromSec(num2);
			string empty = string.Empty;
			if (this.m_MythType == MYTH_TYPE.MYTHTYPE_LEGEND)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3449"),
					"day",
					totalDayFromSec,
					"hour",
					hourFromSec,
					"min",
					minuteFromSec,
					"count",
					this.m_CurItemUseNum,
					"minusday",
					this.m_CurItemUseNum
				});
				this.m_LB_LegendEvolutionTime.SetText(empty);
			}
			else if (this.m_MythType == MYTH_TYPE.MYTHTYPE_EVOLUTION)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3450"),
					"day",
					totalDayFromSec,
					"hour",
					hourFromSec,
					"min",
					minuteFromSec,
					"count",
					this.m_CurItemUseNum,
					"minusday",
					(float)this.m_CurItemUseNum * 0.5f
				});
				this.m_LB_MythEvolutionTime.SetText(empty);
			}
			this.m_Label_Count.SetText(this.m_CurItemUseNum.ToString());
		}
	}

	private void OnValueMinus(IUIObject obj)
	{
		if (this.m_CurItemUseNum > 1)
		{
			this.m_CurItemUseNum--;
		}
		this.Set_GetItemNum();
		this.SetItemText();
	}

	private void OnValueAdd(IUIObject obj)
	{
		if (this.m_MaxItemUseNum >= this.m_CurItemUseNum + 1)
		{
			this.m_CurItemUseNum++;
		}
		this.Set_GetItemNum();
		this.SetItemText();
	}

	private void OnItemUse(IUIObject obj)
	{
		ITEM firstItemByUnique = NkUserInventory.instance.GetFirstItemByUnique(50317);
		if (firstItemByUnique != null && this.m_CurItemUseNum <= firstItemByUnique.m_nItemNum)
		{
			GS_ITEM_MATERIAL_USE_REQ gS_ITEM_MATERIAL_USE_REQ = new GS_ITEM_MATERIAL_USE_REQ();
			gS_ITEM_MATERIAL_USE_REQ.m_bType = (byte)this.m_MythType;
			gS_ITEM_MATERIAL_USE_REQ.i32CharKind = this.m_i32CharKind;
			gS_ITEM_MATERIAL_USE_REQ.i64SolID = this.m_i64SolID;
			gS_ITEM_MATERIAL_USE_REQ.m_i32ItemUnique = 50317;
			gS_ITEM_MATERIAL_USE_REQ.m_i32ItemNum = this.m_CurItemUseNum;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_MATERIAL_USE_REQ, gS_ITEM_MATERIAL_USE_REQ);
			this.Close();
		}
		else
		{
			string empty = string.Empty;
			string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(50317);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1256"),
				"itemname",
				itemNameByItemUnique,
				"count",
				this.m_CurItemUseNum
			});
			Main_UI_SystemMessage.ADDMessage(empty, SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
		}
	}

	private void OnCanCelClose(IUIObject obj)
	{
		this.Close();
	}

	private void OnClickNumPad(IUIObject obj)
	{
		InputNumberDlg inputNumberDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.DLG_INPUTNUMBER) as InputNumberDlg;
		inputNumberDlg.SetCallback(new Action<InputNumberDlg, object>(this.On_InputData), null, new Action<InputNumberDlg, object>(this.OnClose_InputNumber), null);
		inputNumberDlg.SetMinMax(1L, (long)this.m_MaxItemUseNum);
		inputNumberDlg.SetNum((long)this.m_CurItemUseNum);
		inputNumberDlg.SetLocation(inputNumberDlg.GetLocationX(), inputNumberDlg.GetLocationY(), base.GetLocation().z - 2f);
	}

	public void On_InputData(InputNumberDlg a_cForm, object a_oObject)
	{
		int num = (int)a_cForm.GetNum();
		if (num > this.m_MaxItemUseNum)
		{
			num = this.m_MaxItemUseNum;
		}
		this.m_CurItemUseNum = num;
		this.Set_GetItemNum();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void OnClose_InputNumber(InputNumberDlg a_cForm, object a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.DLG_INPUTNUMBER);
	}

	public void Set_GetItemNum()
	{
		float value = 0f;
		if (this.m_MaxItemUseNum > 0)
		{
			value = (float)this.m_CurItemUseNum / (float)this.m_MaxItemUseNum;
		}
		this.SetItemText();
		this.m_HSlider_HSlider1.Value = value;
		this.m_fSliderValue = this.m_HSlider_HSlider1.Value;
	}

	public override void Update()
	{
		base.Update();
		if (this.m_fSliderValue != this.m_HSlider_HSlider1.Value)
		{
			int num = (int)(this.m_HSlider_HSlider1.Value * 100f / (100f / (float)this.m_MaxItemUseNum));
			this.m_CurItemUseNum = num + 1;
			if (this.m_CurItemUseNum >= this.m_MaxItemUseNum)
			{
				this.m_CurItemUseNum = this.m_MaxItemUseNum;
			}
			if (this.m_MaxItemUseNum <= 1)
			{
				this.m_CurItemUseNum = 1;
			}
			this.m_fSliderValue = this.m_HSlider_HSlider1.Value;
			this.SetItemText();
		}
	}
}
