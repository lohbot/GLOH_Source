using GAME;
using System;
using UnityEngine;
using UnityForms;

public class ItemMallProductDetailDlg : Form
{
	private const int MAX_PRODUCT_COUNT = 4;

	private FunDelegate m_BuyDelegate;

	private ITEM_MALL_ITEM m_Item;

	private Label m_lbProductName;

	private DrawTexture m_dtProductImg;

	private DrawTexture m_dtProduct_Effect;

	private DrawTexture m_dt_won;

	private Label m_lbDecription;

	private Button[] m_btSol = new Button[4];

	private DrawTexture[] m_dtSol = new DrawTexture[4];

	private Button m_btPrice;

	private Label m_lbPrice;

	private Label m_lbGiftHelp;

	private Label m_lbSaleNum;

	private Label m_lbSaleNumHelp;

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
		this.m_dtProduct_Effect = (base.GetControl("DT_Product_Effect") as DrawTexture);
		this.m_dt_won = (base.GetControl("DrawTexture_won") as DrawTexture);
		this.m_lbDecription = (base.GetControl("Label_Description") as Label);
		this.m_btSol[0] = (base.GetControl("Button_Sol1") as Button);
		this.m_btSol[1] = (base.GetControl("Button_Sol2") as Button);
		this.m_btSol[2] = (base.GetControl("Button_Sol3") as Button);
		this.m_btSol[3] = (base.GetControl("Button_Sol4") as Button);
		for (int i = 0; i < this.m_btSol.Length; i++)
		{
			this.m_btSol[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickedSolGuid));
		}
		this.m_dtSol[0] = (base.GetControl("DT_Sol1") as DrawTexture);
		this.m_dtSol[1] = (base.GetControl("DT_Sol2") as DrawTexture);
		this.m_dtSol[2] = (base.GetControl("DT_Sol3") as DrawTexture);
		this.m_dtSol[3] = (base.GetControl("DT_Sol4") as DrawTexture);
		this.VisibleContent(false);
		this.m_btPrice = (base.GetControl("Button_buy") as Button);
		this.m_lbPrice = (base.GetControl("Label_Price") as Label);
		this.m_lbGiftHelp = (base.GetControl("Label_GiftHelp") as Label);
		this.m_lbGiftHelp.Visible = false;
		this.m_lbSaleNum = (base.GetControl("Label_SaleNum") as Label);
		this.m_lbSaleNum.Visible = false;
		this.m_lbSaleNumHelp = (base.GetControl("Label_SaleNumHelp") as Label);
		this.m_lbSaleNumHelp.Visible = false;
	}

	private void VisibleContent(bool bVisible)
	{
		for (int i = 0; i < 4; i++)
		{
			this.m_btSol[i].Visible = bVisible;
			this.m_dtSol[i].Visible = bVisible;
		}
	}

	public void SetItem(ITEM_MALL_ITEM Item)
	{
		this.m_Item = Item;
		this.m_lbProductName.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromItem(Item.m_strTextKey);
		this.m_lbDecription.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(Item.m_strItemTooltip);
		this.m_dtProductImg.SetFadeTextureFromBundle(Item.m_strIconPath);
		this.m_lbPrice.Text = ItemMallItemManager.GetCashPrice(Item);
		this.m_btPrice.Data = Item;
		this.m_btPrice.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClicked));
		this.m_dt_won.SetTexture(ItemMallItemManager.GetCashTextureName((eITEMMALL_MONEY_TYPE)Item.m_nMoneyType));
		this.VisibleContent(false);
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
					this.m_dtSol[num].Visible = true;
					this.m_btSol[num].Data = charKindByCode;
					this.m_btSol[num].Text = string.Format("{0} {1}", charKindInfo.GetName(), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("788"));
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
}
