using GAME;
using PROTOCOL;
using System;
using UnityEngine;
using UnityForms;

public class ItemMallProductDetailDlg : Form
{
	private const int MAX_PRODUCT_COUNT = 6;

	private FunDelegate m_BuyDelegate;

	private ITEM_MALL_ITEM m_Item;

	private TIMESHOP_DATA m_pTimeShopItem;

	private Label m_lbProductName;

	private DrawTexture m_dtProductImg;

	private DrawTexture m_dtProduct_Effect;

	private DrawTexture m_dt_won;

	private DrawTexture m_dtProductLine;

	private Label m_lbDecription;

	private Button[] m_btSol = new Button[6];

	private Button[] m_btSol2 = new Button[6];

	private Button m_btPrice;

	private Label m_lbPrice;

	private Label m_lbGiftHelp;

	private Label m_lbSaleNum;

	private Label m_lbSaleNumHelp;

	private Button m_bRateOpenUrl;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/dlg_itemmall_Product_detail", G_ID.ITEMMALL_PRODUCTDETAIL_DLG, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_lbProductName = (base.GetControl("Label_ProductName") as Label);
		this.m_dtProductImg = (base.GetControl("DrawTexture_ProductImg") as DrawTexture);
		this.m_dtProductLine = (base.GetControl("DT_ProductImgLine") as DrawTexture);
		this.m_dtProduct_Effect = (base.GetControl("DT_Product_Effect") as DrawTexture);
		this.m_dt_won = (base.GetControl("DrawTexture_won") as DrawTexture);
		this.m_lbDecription = (base.GetControl("Label_Description") as Label);
		for (int i = 0; i < 6; i++)
		{
			this.m_btSol[i] = (base.GetControl(string.Format("Button_Sol{0}", (i + 1).ToString())) as Button);
			this.m_btSol[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickedSolGuid));
			this.m_btSol[i].EffectAni = false;
			this.m_btSol2[i] = (base.GetControl(string.Format("Btn_ItemInfo_0{0}", (i + 1).ToString())) as Button);
			this.m_btSol2[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickedSolGuid));
		}
		this.VisibleContent(false);
		this.m_btPrice = (base.GetControl("Button_buy") as Button);
		this.m_lbPrice = (base.GetControl("Label_Price") as Label);
		this.m_lbGiftHelp = (base.GetControl("Label_GiftHelp") as Label);
		this.m_lbGiftHelp.Visible = false;
		this.m_lbSaleNum = (base.GetControl("Label_SaleNum") as Label);
		this.m_lbSaleNum.Visible = false;
		this.m_lbSaleNumHelp = (base.GetControl("Label_SaleNumHelp") as Label);
		this.m_lbSaleNumHelp.Visible = false;
		this.m_bRateOpenUrl = (base.GetControl("Button_rateInfo") as Button);
		this.m_bRateOpenUrl.Visible = false;
		this.m_bRateOpenUrl.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickRateOpenUrl));
	}

	private void VisibleContent(bool bVisible)
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_btSol[i].Visible = bVisible;
			this.m_btSol2[i].Visible = bVisible;
		}
	}

	public void SetItem(ITEM_MALL_ITEM Item)
	{
		this.m_Item = Item;
		this.m_lbProductName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(Item.m_strTextKey);
		this.m_lbDecription.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(Item.m_strItemTooltip);
		this.m_dtProductImg.SetFadeTextureFromBundle(Item.m_strIconPath);
		this.m_lbPrice.Text = ItemMallItemManager.GetCashPrice(Item);
		ITEM_VOUCHER_DATA itemVoucherDataFromItemID = NrTSingleton<ItemMallItemManager>.Instance.GetItemVoucherDataFromItemID(Item.m_Idx);
		if (itemVoucherDataFromItemID != null)
		{
			NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
			if (kMyCharInfo == null)
			{
				return;
			}
			eVOUCHER_TYPE ui8VoucherType = (eVOUCHER_TYPE)itemVoucherDataFromItemID.ui8VoucherType;
			if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HEARTS_HERO)
			{
				long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_HEARTS_TIME);
				if (PublicMethod.GetCurTime() > charSubData)
				{
					this.m_lbPrice.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3344");
				}
			}
			else if (ui8VoucherType == eVOUCHER_TYPE.eVOUCHER_TYPE_RECRUIT_HERO)
			{
				long charSubData = kMyCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_FREETICKET_PREMINUM_TIME);
				if (PublicMethod.GetCurTime() > charSubData)
				{
					this.m_lbPrice.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3344");
				}
			}
			else
			{
				long voucherRemainTime = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetVoucherRemainTime(ui8VoucherType, itemVoucherDataFromItemID.i64ItemMallID);
				if (voucherRemainTime > 0L)
				{
					this.m_btPrice.controlIsEnabled = false;
				}
			}
		}
		this.m_btPrice.Data = Item;
		this.m_btPrice.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClicked));
		this.m_dt_won.SetTexture(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
		this.VisibleContent(false);
		if (NrTSingleton<ContentsLimitManager>.Instance.IsRateUrl() && (this.m_Item.m_nGroup == 50 || this.m_Item.m_nGroup == 51 || this.m_Item.m_nGroup == 5))
		{
			this.m_bRateOpenUrl.Visible = true;
		}
		char[] separator = new char[]
		{
			'+'
		};
		string[] array = Item.m_strSolKind.Split(separator);
		if (array == null)
		{
			return;
		}
		int num = 0;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			string text2 = text.Trim();
			if (!(text2 == string.Empty))
			{
				int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(text2);
				if (charKindByCode != 0)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charKindByCode);
					this.m_btSol[num].Visible = true;
					this.m_btSol2[num].Visible = true;
					this.m_btSol[num].Data = charKindByCode;
					this.m_btSol[num].Text = string.Format("{0} {1}", charKindInfo.GetName(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("788"));
					this.m_btSol2[num].Data = charKindByCode;
					num++;
				}
			}
		}
		if (TsPlatform.IsAndroid)
		{
			if (Item.m_nGift == 1)
			{
				this.m_lbGiftHelp.Visible = true;
			}
			else
			{
				this.m_lbGiftHelp.Visible = false;
			}
		}
		if (0 < Item.m_nSaleNum)
		{
			int num2 = Item.m_nSaleNum - NrTSingleton<ItemMallItemManager>.Instance.GetBuyCount(Item.m_Idx);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2611"),
				"Count",
				num2
			});
			this.m_lbSaleNum.SetText(empty);
			this.m_lbSaleNum.Visible = true;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2639"),
				"count",
				Item.m_nSaleNum
			});
			this.m_lbSaleNumHelp.SetText(empty);
			this.m_lbSaleNumHelp.Visible = true;
		}
		else
		{
			this.m_lbSaleNum.Visible = false;
			this.m_lbSaleNumHelp.Visible = false;
			this.m_dtProductLine.Visible = false;
		}
	}

	public void SetEffect()
	{
		Transform child = NkUtil.GetChild(this.m_dtProduct_Effect.gameObject.transform, "child_effect");
		if (child == null)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey(EffectDefine.FX_ITEMMALL_PRODUCT, this.m_dtProduct_Effect, this.m_dtProduct_Effect.GetSize());
		}
	}

	public void AddBlackBgClickCloseForm()
	{
		base.BLACK_BG.AddValueChangedDelegate(new EZValueChangedDelegate(this.CloseForm));
	}

	public void SetButtonEvent(FunDelegate delBuy)
	{
		if (delBuy != null)
		{
			this.m_BuyDelegate = (FunDelegate)Delegate.Combine(this.m_BuyDelegate, new FunDelegate(delBuy.Invoke));
		}
	}

	private void OnClicked(IUIObject obj)
	{
		if (this.m_BuyDelegate != null)
		{
			this.m_BuyDelegate(obj);
		}
		this.Close();
	}

	private void OnClickedSolGuid(IUIObject obj)
	{
		int num = (int)obj.Data;
		if (num == 0)
		{
			return;
		}
		ItemMallSolDetailDlg itemMallSolDetailDlg = (ItemMallSolDetailDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_SOL_DETAIL);
		if (!itemMallSolDetailDlg.Visible)
		{
			itemMallSolDetailDlg.Show();
		}
		itemMallSolDetailDlg.SetMallItem(this.m_Item, num);
		itemMallSolDetailDlg.SetButtonEvent(this.m_BuyDelegate);
	}

	public void OnClickRateOpenUrl(IUIObject obj)
	{
		ITEM_RATE_OPENURL_DATA itemRateOpenUrl = BASE_RATE_OPENURL_DATA.GetItemRateOpenUrl();
		NrMobileNoticeWeb nrMobileNoticeWeb = new NrMobileNoticeWeb();
		nrMobileNoticeWeb.OnRateOpenUrl(itemRateOpenUrl.strUrl);
	}

	public void SetTimeShopItem(TIMESHOP_DATA _pData, int _nIndex)
	{
		if (_pData == null)
		{
			return;
		}
		this.m_pTimeShopItem = _pData;
		this.m_lbProductName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(_pData.m_strProductTextKey);
		if (_pData.m_strItemToolTip.Equals("0"))
		{
			this.m_lbDecription.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(_pData.m_nItemUnique.ToString());
		}
		else
		{
			this.m_lbDecription.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(_pData.m_strItemToolTip);
		}
		this.m_dtProductImg.SetFadeTextureFromBundle(_pData.m_strIconPath);
		this.m_lbPrice.Text = _pData.m_lPrice.ToString();
		this.m_btPrice.Data = _pData;
		this.m_btPrice.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClicked));
		this.m_dt_won.SetTexture(NrTSingleton<NrTableTimeShopManager>.Instance.Get_MoneyTypeTextureName((eTIMESHOP_MONEYTYPE)_pData.m_nMoneyType));
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.IsBuy_TimeShopItemByIDX(_pData.m_lIdx))
		{
			this.m_btPrice.SetEnabled(false);
			this.m_lbPrice.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3341");
			this.m_dt_won.Visible = false;
		}
		eTIMESHOP_TYPE type_ByIDX = NrTSingleton<NrTableTimeShopManager>.Instance.GetType_ByIDX(_pData.m_lIdx);
		if (NrTSingleton<NrTableTimeShopManager>.Instance.IsRecommend((byte)type_ByIDX, _pData.m_lIdx))
		{
			this.SetEffect();
		}
		this.VisibleContent(false);
		char[] separator = new char[]
		{
			'+'
		};
		string[] array = _pData.m_strSolKind.Split(separator);
		if (array == null)
		{
			return;
		}
		int num = 0;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			string text2 = text.Trim();
			if (!(text2 == string.Empty))
			{
				int charKindByCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(text2);
				if (charKindByCode != 0)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(charKindByCode);
					this.m_btSol[num].Visible = true;
					this.m_btSol2[num].Visible = true;
					this.m_btSol[num].Data = charKindByCode;
					this.m_btSol[num].Text = string.Format("{0} {1}", charKindInfo.GetName(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("788"));
					this.m_btSol[num].SetValueChangedDelegate(new EZValueChangedDelegate(this.Click_SolGuide));
					this.m_btSol2[num].Data = charKindByCode;
					this.m_btSol2[num].SetValueChangedDelegate(new EZValueChangedDelegate(this.Click_SolGuide));
					num++;
				}
			}
		}
		this.m_lbGiftHelp.Visible = false;
		this.m_lbSaleNum.Visible = false;
		this.m_lbSaleNumHelp.Visible = false;
		this.m_dtProductLine.Visible = false;
	}

	private void Click_SolGuide(IUIObject _obj)
	{
		int num = (int)_obj.Data;
		if (num == 0)
		{
			return;
		}
		ItemMallSolDetailDlg itemMallSolDetailDlg = (ItemMallSolDetailDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMMALL_SOL_DETAIL);
		if (!itemMallSolDetailDlg.Visible)
		{
			itemMallSolDetailDlg.Show();
		}
		itemMallSolDetailDlg.SetTimeShopItem(this.m_pTimeShopItem, num);
		itemMallSolDetailDlg.SetButtonEvent(this.m_BuyDelegate);
	}
}
