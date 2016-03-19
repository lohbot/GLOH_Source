using GAME;
using System;
using System.Text;
using UnityForms;

public class DailyGift_Dlg : Form
{
	private const int DAY_COUNT_MAX = 7;

	private DrawTexture m_dtBg;

	private DrawTexture[] m_dtReward = new DrawTexture[7];

	private Label[] m_lbReward = new Label[7];

	private DrawTexture[] m_dtRewardClear = new DrawTexture[7];

	private Button m_btOK;

	private int[] m_ItemToolTipUnique = new int[7];

	private int[] m_ItemToolTipCount = new int[7];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "System/dlg_dailygift", G_ID.EVENT_DAILY_GIFT_DLG, false, true);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_dtBg = (base.GetControl("DrawTexture_DrawTexture16") as DrawTexture);
		this.m_dtBg.SetTextureFromBundle("ui/etc/dailygift");
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 7; i++)
		{
			stringBuilder.Length = 0;
			stringBuilder.Append("DrawTexture_GiftItem");
			stringBuilder.Append(string.Format("{0:00}", i + 1));
			this.m_dtReward[i] = (base.GetControl(stringBuilder.ToString()) as DrawTexture);
			this.m_dtReward[i].UsedCollider(true);
			this.m_dtReward[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickItemDT));
			stringBuilder.Length = 0;
			stringBuilder.Append("Label_ItemName");
			stringBuilder.Append(string.Format("{0:00}", i + 1));
			this.m_lbReward[i] = (base.GetControl(stringBuilder.ToString()) as Label);
			stringBuilder.Length = 0;
			stringBuilder.Append("DrawTexture_Clear");
			stringBuilder.Append(string.Format("{0:00}", i + 1));
			this.m_dtRewardClear[i] = (base.GetControl(stringBuilder.ToString()) as DrawTexture);
		}
		this.m_btOK = (base.GetControl("Button_ok") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickOK));
		this.m_btOK.Visible = false;
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		if (myCharInfo == null)
		{
			return;
		}
		long charWeekData = myCharInfo.GetCharWeekData(0);
		string empty = string.Empty;
		int num = 0;
		short num2 = 0;
		string empty2 = string.Empty;
		for (int j = 0; j < 7; j++)
		{
			if (NrTSingleton<NrDailyGiftManager>.Instance.GetDailyGiftItemInfo((byte)(j + 1), out num, out num2, out empty2))
			{
				this.m_dtReward[j].SetTextureFromBundle(empty2);
				ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(num);
				if (itemInfo != null)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1078"),
						"count",
						num2
					});
					this.m_ItemToolTipUnique[j] = num;
					this.m_ItemToolTipCount[j] = (int)num2;
					this.m_dtReward[j].Data = j;
				}
				this.m_lbReward[j].SetText(empty);
				this.m_dtRewardClear[j].Visible = false;
			}
			else
			{
				this.Close();
			}
		}
	}

	public override void Update()
	{
	}

	public void OnClickOK(IUIObject obj)
	{
		this.Close();
	}

	public void OnClickItemDT(IUIObject obj)
	{
		int num = (int)obj.Data;
		ITEM iTEM = new ITEM();
		iTEM.m_nItemUnique = this.m_ItemToolTipUnique[num];
		iTEM.m_nItemNum = this.m_ItemToolTipCount[num];
		ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
		itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
	}

	public void CheckDailyEventDay(int WeekDayCount)
	{
		int num = WeekDayCount - 1;
		if (num >= 0 && num < 7)
		{
			for (int i = 0; i < 7; i++)
			{
				if (num > i)
				{
					this.m_dtRewardClear[i].Visible = true;
				}
				else
				{
					this.m_dtRewardClear[i].Visible = false;
				}
			}
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("ATTENDANCECHECK", this.m_dtReward[num], this.m_dtReward[num].GetSize());
			this.m_btOK.Visible = true;
		}
	}
}
