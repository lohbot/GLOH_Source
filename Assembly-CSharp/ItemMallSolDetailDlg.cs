using GAME;
using System;
using TsBundle;
using UnityForms;

public class ItemMallSolDetailDlg : Form
{
	private const byte SHOW_SOL_GRADE = 5;

	private const int SHOW_SOL_LEVEL = 50;

	private FunDelegate m_BuyDelegate;

	private SolDetailDlgTool m_SolInterfaceTool = new SolDetailDlgTool();

	protected Label m_Label_Stats_str2;

	protected Label m_Label_Stats_dex2;

	protected Label m_Label_Stats_vit2;

	protected Label m_Label_Stats_int2;

	private Button m_Button_MovieBtn;

	private Label m_Label_Movie;

	private ScrollLabel m_ScrollLabel_SolInfo;

	private Label m_Label_Price;

	private Button m_Button_Price;

	private DrawTexture m_DrawTexture_Won;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Item/dlg_itemmall_soldetail", G_ID.ITEMMALL_SOL_DETAIL, true);
	}

	public override void SetComponent()
	{
		this.m_SolInterfaceTool.m_DrawTextureWeapon = (base.GetControl("DT_Weapon") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_Character_Name = (base.GetControl("Label_character_name") as Label);
		this.m_SolInterfaceTool.m_Label_SeasonNum = (base.GetControl("Label_SeasonNum") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_rank = (base.GetControl("DrawTexture_rank") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_Rank2 = (base.GetControl("Label_rank2") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_Character = (base.GetControl("DrawTexture_character") as DrawTexture);
		this.m_Button_MovieBtn = (base.GetControl("Button_Movie") as Button);
		this.m_Button_MovieBtn.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnIntroMovieButton));
		this.m_Label_Movie = (base.GetControl("LB_Movie") as Label);
		this.m_Label_Stats_str2 = (base.GetControl("Label_stats_str2") as Label);
		this.m_Label_Stats_dex2 = (base.GetControl("Label_stats_dex2") as Label);
		this.m_Label_Stats_vit2 = (base.GetControl("Label_stats_vit2") as Label);
		this.m_Label_Stats_int2 = (base.GetControl("Label_stats_int2") as Label);
		this.m_SolInterfaceTool.m_DrawTexture_Event = (base.GetControl("DrawTexture_Event") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_EventDate = (base.GetControl("Label_EventDate") as Label);
		for (int i = 0; i < 2; i++)
		{
			this.m_SolInterfaceTool.m_Lebel_EventHero[i] = (base.GetControl(NrTSingleton<UIDataManager>.Instance.GetString("Label_EventStat", (i + 1).ToString())) as Label);
		}
		this.m_SolInterfaceTool.m_DrawTextureSkillIcon = (base.GetControl("DT_SkillIcon") as DrawTexture);
		this.m_SolInterfaceTool.m_Label_SkillAnger = (base.GetControl("Label_Anger") as Label);
		this.m_SolInterfaceTool.m_Label_SkillName = (base.GetControl("Label_SkillName") as Label);
		this.m_SolInterfaceTool.m_ScrollLabel_SkillInfo = (base.GetControl("ScrollLabel_SkillInfo") as ScrollLabel);
		this.m_ScrollLabel_SolInfo = (base.GetControl("ScrollLabel_SolInfo") as ScrollLabel);
		this.m_DrawTexture_Won = (base.GetControl("DrawTexture_Won") as DrawTexture);
		if (TsPlatform.IsIPhone || NrGlobalReference.strLangType.Equals("eng"))
		{
			this.m_DrawTexture_Won.SetTexture("Win_I_Dolla");
		}
		else
		{
			this.m_DrawTexture_Won.SetTexture("Win_I_Won");
		}
		this.m_Label_Price = (base.GetControl("Label_price") as Label);
		this.m_Button_Price = (base.GetControl("Button_Buy") as Button);
		this.m_Button_Price.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickedPriceBtn));
		this.m_Button_Price.Visible = false;
		this.m_Label_Price.Visible = false;
		this.m_DrawTexture_Won.Visible = false;
		base.Draggable = false;
		base.SetScreenCenter();
		base.ShowBlackBG(0.5f);
	}

	private void OnClickedPriceBtn(IUIObject obj)
	{
		if (this.m_BuyDelegate != null)
		{
			this.m_BuyDelegate(obj);
		}
		this.Close();
	}

	private void OnIntroMovieButton(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		int num = (int)obj.Data;
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(num);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + num + " !!");
			return;
		}
		string sOLINTRO = charKindInfo.GetCHARKIND_INFO().SOLINTRO;
		if (NrTSingleton<NrGlobalReference>.Instance.localWWW)
		{
			if (NrTSingleton<NrGlobalReference>.Instance.useCache)
			{
				string str = string.Format("{0}SOLINTRO/", Option.GetProtocolRootPath(Protocol.HTTP));
				NmMainFrameWork.PlayMovieURL(str + sOLINTRO + ".mp4", true, false);
			}
			else
			{
				NmMainFrameWork.PlayMovieURL("http://klohw.ndoors.com/at2mobile_android/SOLINTRO/" + sOLINTRO + ".mp4", true, false);
			}
		}
		else
		{
			string str2 = string.Format("{0}SOLINTRO/", NrTSingleton<NrGlobalReference>.Instance.basePath);
			NmMainFrameWork.PlayMovieURL(str2 + sOLINTRO + ".mp4", true, false);
		}
	}

	public void SetButtonEvent(FunDelegate delBuy)
	{
		if (delBuy != null)
		{
			this.m_BuyDelegate = (FunDelegate)Delegate.Combine(this.m_BuyDelegate, new FunDelegate(delBuy.Invoke));
		}
	}

	public void SetMallItem(ITEM_MALL_ITEM MallItem, int i32CharKind)
	{
		this.m_DrawTexture_Won.SetTexture(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)MallItem.m_nMoneyType));
		this.m_Label_Price.Text = ItemMallItemManager.GetCashPrice(MallItem);
		this.m_Button_Price.Data = MallItem;
		this.m_Label_Price.Visible = true;
		this.m_Button_Price.Visible = true;
		this.m_DrawTexture_Won.Visible = true;
		this.SetSolKind(i32CharKind, 5);
	}

	public void SetSolKind(int i32CharKind, byte bGrade)
	{
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(i32CharKind);
		if (charKindInfo == null)
		{
			TsLog.LogOnlyEditor("!!!! SOL CHARKIND ERROR {0}" + i32CharKind + " !!");
			return;
		}
		this.m_SolInterfaceTool.m_kSelectCharKindInfo = charKindInfo;
		this.m_SolInterfaceTool.SetCharImg(bGrade);
		this.m_SolInterfaceTool.SetHeroEventLabel(bGrade);
		this.m_SolInterfaceTool.SetSkillInfo_MaxLevel();
		this.m_Button_MovieBtn.data = i32CharKind;
		if (this.m_SolInterfaceTool.m_kSelectCharKindInfo.GetCHARKIND_INFO().SOLINTRO != "0")
		{
			this.m_Button_MovieBtn.Visible = true;
			this.m_Label_Movie.Visible = true;
		}
		else
		{
			this.m_Button_MovieBtn.Visible = false;
			this.m_Label_Movie.Visible = false;
		}
		int num = charKindInfo.GetGradePlusSTR((int)(bGrade - 1));
		int num2 = charKindInfo.GetGradePlusDEX((int)(bGrade - 1));
		int num3 = charKindInfo.GetGradePlusINT((int)(bGrade - 1));
		int num4 = charKindInfo.GetGradePlusVIT((int)(bGrade - 1));
		num += charKindInfo.GetIncSTR((int)(bGrade - 1), 50);
		num2 += charKindInfo.GetIncDEX((int)(bGrade - 1), 50);
		num3 += charKindInfo.GetIncINT((int)(bGrade - 1), 50);
		num4 += charKindInfo.GetIncVIT((int)(bGrade - 1), 50);
		this.m_Label_Stats_str2.SetText(num.ToString());
		this.m_Label_Stats_dex2.SetText(num2.ToString());
		this.m_Label_Stats_vit2.SetText(num4.ToString());
		this.m_Label_Stats_int2.SetText(num3.ToString());
		this.m_ScrollLabel_SolInfo.SetScrollLabel(charKindInfo.GetDesc());
	}
}
