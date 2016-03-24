using GAME;
using System;
using System.Collections.Generic;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Item_Box_RareRandom_Dlg : Form
{
	private const int N_RANDOM_TIME = 1500;

	private const int N_HIDE_TIME = 1500;

	private const int N_ITEM_ICON_CHANGE = 60;

	private const int N_ROTATE = -15;

	private Label m_laBoxTitle;

	private DrawTexture m_dtMainBG;

	private DrawTexture m_ivMainBoxItem;

	private Button m_btUseBoxItem;

	private NewListBox m_NlbItemListBox;

	private FlashLabel m_flToolTip1;

	private FlashLabel m_flToolTip2;

	private FlashLabel m_flToolTip3;

	private Button m_btSlide1;

	private Button m_btSlide2;

	private DrawTexture m_dwSlideBG1;

	private DrawTexture m_dwSlideBG2;

	private Label m_laSelectItemName;

	private Label m_laSelectItemCount;

	private FlashLabel m_flToolTip4;

	private FlashLabel m_flToolTip5;

	private FlashLabel m_flToolTip6;

	private bool m_bCompleted;

	private string m_strItemName = string.Empty;

	private int m_BoxitemCount;

	private int m_nTime;

	private int m_nItemChangeTime;

	private int m_nArrayIndex;

	private ITEM m_lMainBoxItem;

	private ITEM m_CompleteItem;

	private int m_nCompleteItemNum;

	private bool m_bButtonOk;

	private Protocol_Item_Box.Roulette_Item[] m_saRouletteItem;

	private GameObject m_goBoxOpenEffect;

	private eITEMMALL_BOXTRADE_TYPE m_eItemMall_BoxType = eITEMMALL_BOXTRADE_TYPE.ITEMMALL_TRADETYPE_GETBOX;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item_Box/DLG_Itembox_Rare", G_ID.ITEM_BOX_RARERANDOM_DLG, true);
		base.ShowBlackBG(0.9f);
	}

	public override void SetComponent()
	{
		this.m_laBoxTitle = (base.GetControl("Label_Title") as Label);
		this.m_dtMainBG = (base.GetControl("DrawTexture_ImageBG") as DrawTexture);
		this.m_dtMainBG.SetTextureFromBundle("UI/Etc/rarebox");
		this.m_dtMainBG.Hide(false);
		this.m_ivMainBoxItem = (base.GetControl("DrawTexture_Item") as DrawTexture);
		if (this.m_ivMainBoxItem != null)
		{
			Transform child = NkUtil.GetChild(this.m_ivMainBoxItem.gameObject.transform, "child_effect");
			if (child == null)
			{
				NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_BOXOPEN", this.m_ivMainBoxItem, this.m_ivMainBoxItem.GetSize());
				this.m_ivMainBoxItem.AddGameObjectDelegate(new EZGameObjectDelegate(this.effectBoxOpen));
			}
		}
		this.m_btUseBoxItem = (base.GetControl("Button_OK") as Button);
		this.m_btUseBoxItem.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickUseBoxItem));
		this.m_btUseBoxItem.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"));
		this.m_NlbItemListBox = (base.GetControl("NewListBox_Item") as NewListBox);
		this.m_NlbItemListBox.Reserve = false;
		this.m_flToolTip1 = (base.GetControl("FlashLabel_Tooltip1") as FlashLabel);
		this.m_flToolTip2 = (base.GetControl("FlashLabel_Tooltip2") as FlashLabel);
		this.m_flToolTip3 = (base.GetControl("FlashLabel_Tooltip3") as FlashLabel);
		this.m_btSlide1 = (base.GetControl("Button_Slide1") as Button);
		this.m_btSlide1.SetLocation(this.m_btSlide1.GetLocation().x, this.m_btSlide1.GetLocationY(), this.m_btSlide1.GetLocation().z - 1.1f);
		this.m_btSlide2 = (base.GetControl("Button_Slide2") as Button);
		this.m_btSlide2.SetLocation(this.m_btSlide2.GetLocation().x, this.m_btSlide2.GetLocationY(), this.m_btSlide2.GetLocation().z - 1.1f);
		BoxCollider boxCollider = (BoxCollider)this.m_btSlide1.gameObject.GetComponent(typeof(BoxCollider));
		if (boxCollider != null)
		{
			boxCollider.size = new Vector3(0f, 0f, 0f);
		}
		boxCollider = (BoxCollider)this.m_btSlide2.gameObject.GetComponent(typeof(BoxCollider));
		if (boxCollider != null)
		{
			boxCollider.size = new Vector3(0f, 0f, 0f);
		}
		this.m_dwSlideBG1 = (base.GetControl("DrawTexture_SlideBG1") as DrawTexture);
		this.m_dwSlideBG2 = (base.GetControl("DrawTexture_SlideBG2") as DrawTexture);
		this.m_dwSlideBG1.SetLocation(this.m_dwSlideBG1.GetLocation().x, this.m_dwSlideBG1.GetLocationY(), this.m_dwSlideBG1.GetLocation().z - 1f);
		this.m_dwSlideBG2.SetLocation(this.m_dwSlideBG2.GetLocation().x, this.m_dwSlideBG2.GetLocationY(), this.m_dwSlideBG2.GetLocation().z - 1f);
		this.m_btSlide1.Hide(true);
		this.m_dwSlideBG1.Hide(true);
		this.m_laSelectItemName = (base.GetControl("Label_Item") as Label);
		this.m_laSelectItemCount = (base.GetControl("Label_Count") as Label);
		this.m_flToolTip4 = (base.GetControl("FlashLabel_Tooltip4") as FlashLabel);
		this.m_flToolTip5 = (base.GetControl("FlashLabel_Tooltip5") as FlashLabel);
		this.m_flToolTip6 = (base.GetControl("FlashLabel_Tooltip6") as FlashLabel);
		base.SetScreenCenter();
	}

	private void SetTitle()
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2544"),
			"itemname",
			this.m_strItemName,
			"count",
			this.m_BoxitemCount
		});
		this.m_laBoxTitle.Text = empty;
	}

	public override void InitData()
	{
	}

	public void effectBoxOpen(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		this.m_goBoxOpenEffect = obj;
		this.m_goBoxOpenEffect.SetActive(false);
		Vector3 localPosition = new Vector3(48f, 110f, 1f);
		this.m_goBoxOpenEffect.transform.localPosition = localPosition;
	}

	public void OnClickUseBoxItem(IUIObject obj)
	{
		if (this.m_BoxitemCount <= 0)
		{
			this.Close();
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_lMainBoxItem.m_nItemUnique);
		if (itemInfo.m_nNeedOpenItemUnique > 0 && itemInfo.m_nNeedOpenItemNum > 0)
		{
			if (!NrTSingleton<ItemManager>.Instance.CheckBoxNeedItem(itemInfo.m_nItemUnique, true, true))
			{
				return;
			}
			ITEMINFO itemInfo2 = NrTSingleton<ItemManager>.Instance.GetItemInfo(itemInfo.m_nNeedOpenItemUnique);
			if (itemInfo2 != null)
			{
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("94");
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("171"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemInfo.m_nNeedOpenItemUnique),
					"count",
					itemInfo.m_nNeedOpenItemNum.ToString(),
					"targetname1",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_lMainBoxItem.m_nItemUnique)
				});
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI != null)
				{
					msgBoxUI.SetMsg(new YesDelegate(this.MsgBoxOKEvent), null, null, null, textFromMessageBox, empty, eMsgType.MB_OK_CANCEL);
				}
			}
		}
		else
		{
			this.Send_Use_RareRandomItem();
		}
	}

	private void MsgBoxOKEvent(object obj)
	{
		this.Send_Use_RareRandomItem();
	}

	private void Send_Use_RareRandomItem()
	{
		this.m_bButtonOk = true;
		this.m_btUseBoxItem.Hide(true);
		this.m_nTime = (this.m_nItemChangeTime = Environment.TickCount);
		this.m_bCompleted = false;
		this.m_nArrayIndex = 0;
		if (this.m_eItemMall_BoxType == eITEMMALL_BOXTRADE_TYPE.ITEMMALL_TRADETYPE_GETBOX)
		{
			Protocol_Item_Box.On_Sead_Box_Use_Random(this.m_lMainBoxItem);
		}
		if (this.m_goBoxOpenEffect != null)
		{
			this.m_goBoxOpenEffect.SetActive(false);
			this.m_goBoxOpenEffect.SetActive(true);
		}
	}

	public override void Update()
	{
		if (this.m_NlbItemListBox != null && this.m_NlbItemListBox.Visible && this.m_NlbItemListBox.changeScrollPos)
		{
			if (0.01f >= this.m_NlbItemListBox.ScrollPosition)
			{
				this.m_btSlide1.Hide(true);
				this.m_dwSlideBG1.Hide(true);
			}
			else if (1f <= this.m_NlbItemListBox.ScrollPosition)
			{
				this.m_btSlide2.Hide(true);
				this.m_dwSlideBG2.Hide(true);
			}
			else
			{
				this.m_btSlide1.Hide(false);
				this.m_dwSlideBG1.Hide(false);
				this.m_btSlide2.Hide(false);
				this.m_dwSlideBG2.Hide(false);
			}
		}
		if (this.m_bButtonOk && !this.m_bCompleted)
		{
			if (Environment.TickCount - this.m_nTime > 1500)
			{
				this.m_bCompleted = true;
				this.SowCompleteItem();
			}
			else if (Environment.TickCount - this.m_nItemChangeTime > 60)
			{
				this.m_nItemChangeTime = Environment.TickCount;
				this.m_ivMainBoxItem.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_saRouletteItem[this.m_nArrayIndex].m_nItemUnique);
				this.m_nArrayIndex++;
				if (this.m_nArrayIndex >= this.m_saRouletteItem.Length)
				{
					this.m_nArrayIndex = 0;
				}
			}
		}
	}

	public override void OnClose()
	{
		base.OnClose();
		if (this.m_goBoxOpenEffect != null)
		{
			UnityEngine.Object.Destroy(this.m_goBoxOpenEffect);
			this.m_goBoxOpenEffect = null;
		}
	}

	private void On_Exit_Button(IUIObject a_cUIObject)
	{
		base.CloseNow();
	}

	public void Set_Item(ITEM a_cItem)
	{
		if (a_cItem == null || !a_cItem.IsValid())
		{
			this.Close();
			return;
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(a_cItem.m_nItemUnique);
		if (itemInfo == null)
		{
			this.Close();
			return;
		}
		bool flag = false;
		ITEM_BOX_GROUP iTEM_BOX_GROUP = null;
		if (itemInfo.IsItemATB(65536L))
		{
			flag = true;
			iTEM_BOX_GROUP = NrTSingleton<ItemManager>.Instance.GetBoxGroup(a_cItem.m_nItemUnique);
			if (iTEM_BOX_GROUP == null)
			{
				this.Close();
				return;
			}
		}
		this.m_lMainBoxItem = a_cItem;
		this.m_strItemName = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_lMainBoxItem.m_nItemUnique);
		this.m_BoxitemCount = this.m_lMainBoxItem.m_nItemNum;
		this.SetTitle();
		this.m_ivMainBoxItem.BaseInfoLoderImage = NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_lMainBoxItem.m_nItemUnique);
		this.m_NlbItemListBox.Clear();
		if (a_cItem != null)
		{
			List<Protocol_Item_Box.Roulette_Item> list = new List<Protocol_Item_Box.Roulette_Item>();
			Protocol_Item_Box.Roulette_Item item = default(Protocol_Item_Box.Roulette_Item);
			for (int i = 0; i < 12; i++)
			{
				int num;
				int num2;
				if (flag)
				{
					num = iTEM_BOX_GROUP.i32GroupItemUnique[i];
					num2 = iTEM_BOX_GROUP.i32GroupItemNum[i];
				}
				else
				{
					num = itemInfo.m_nBoxItemUnique[i];
					num2 = itemInfo.m_nBoxItemNumber[i];
				}
				if (num > 0)
				{
					ITEM boxItemTemp = NrTSingleton<ItemManager>.Instance.GetBoxItemTemp(a_cItem.m_nItemUnique, i);
					if (boxItemTemp != null && boxItemTemp.IsValid())
					{
						item.m_nItemUnique = num;
						item.m_strText = NrTSingleton<UIDataManager>.Instance.GetString(num2.ToString(), " ", NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("442"));
						list.Add(item);
						NewListItem newListItem = new NewListItem(this.m_NlbItemListBox.ColumnNum, true, string.Empty);
						newListItem.SetListItemData(0, true);
						newListItem.SetListItemData(1, boxItemTemp, boxItemTemp, new EZValueChangedDelegate(this.OnItemToolTip), null);
						this.m_NlbItemListBox.Add(newListItem);
					}
				}
			}
			this.m_NlbItemListBox.RepositionItems();
			this.m_saRouletteItem = list.ToArray();
			ItemOption_Text[] array = ItemTooltipDlg.Get_Item_Info(a_cItem, null, false, false, G_ID.NONE);
			if (array.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int j = 0; j < array.Length; j++)
				{
					stringBuilder.Append(array[j].m_OptionName);
					stringBuilder2.Append(array[j].m_OptionValue);
				}
				this.m_flToolTip1.SetFlashLabel(stringBuilder.ToString());
				this.m_flToolTip2.SetFlashLabel(stringBuilder2.ToString());
			}
			else
			{
				this.m_flToolTip1.SetFlashLabel(string.Empty);
				this.m_flToolTip2.SetFlashLabel(string.Empty);
			}
			this.m_flToolTip3.SetLocation(this.m_flToolTip3.GetLocation().x, this.m_flToolTip2.GetLocationY() + this.m_flToolTip2.Height + 10f);
			if (itemInfo.m_strToolTipTextKey != "0")
			{
				string textFromItemHelper = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(itemInfo.m_strToolTipTextKey);
				this.m_flToolTip3.SetFlashLabel(textFromItemHelper);
			}
			else
			{
				this.m_flToolTip3.SetFlashLabel(string.Empty);
			}
		}
		base.ShowLayer(0, 1);
	}

	private void OnItemToolTip(IUIObject a_oObject)
	{
		ItemTexture itemTexture = a_oObject as ItemTexture;
		if (itemTexture != null)
		{
			ITEM iTEM = itemTexture.Data as ITEM;
			if (iTEM != null && iTEM.m_nItemUnique > 0)
			{
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
				NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
				ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
				itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
			}
		}
	}

	public void Set_Item_Complete(ITEM givenItem, int givenItemCount, int curBoxItemCount)
	{
		if (givenItem == null || !givenItem.IsValid() || givenItemCount <= 0)
		{
			return;
		}
		this.m_CompleteItem = givenItem;
		this.m_nCompleteItemNum = givenItemCount;
		this.m_BoxitemCount = curBoxItemCount;
		this.SetTitle();
	}

	public void SowCompleteItem()
	{
		if (this.m_CompleteItem == null || !this.m_CompleteItem.IsValid())
		{
			this.Close();
			return;
		}
		this.m_ivMainBoxItem.SetTexture(NrTSingleton<ItemManager>.Instance.GetItemTexture(this.m_CompleteItem.m_nItemUnique));
		string itemNameByItemUnique = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_CompleteItem.m_nItemUnique);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("572"),
			"Count",
			this.m_nCompleteItemNum
		});
		this.m_laSelectItemName.SetText(itemNameByItemUnique);
		this.m_laSelectItemCount.SetText(empty);
		ItemOption_Text[] array = ItemTooltipDlg.Get_Item_Info(this.m_CompleteItem, null, false, false, G_ID.NONE);
		int num = 10;
		if (array.Length > 0)
		{
			if (array.Length > 2)
			{
				array[1].m_OptionName = "\n" + array[1].m_OptionName;
				array[1].m_OptionValue = "\n" + array[1].m_OptionValue;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].m_OptionName);
				stringBuilder2.Append(array[i].m_OptionValue);
			}
			this.m_flToolTip4.SetFlashLabel(stringBuilder.ToString());
			this.m_flToolTip5.SetFlashLabel(stringBuilder2.ToString());
		}
		else
		{
			this.m_flToolTip4.SetFlashLabel(string.Empty);
			this.m_flToolTip5.SetFlashLabel(string.Empty);
			num = 110;
		}
		if (!Protocol_Item.Is_EquipItem(this.m_CompleteItem.m_nItemUnique))
		{
			this.m_flToolTip6.SetLocation(this.m_flToolTip6.GetLocation().x, this.m_flToolTip5.GetLocationY() + this.m_flToolTip5.Height + (float)num);
			ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_CompleteItem.m_nItemUnique);
			if (itemInfo.m_strToolTipTextKey != "0")
			{
				string textFromItemHelper = NrTSingleton<NrTextMgr>.Instance.GetTextFromItemHelper(itemInfo.m_strToolTipTextKey);
				this.m_flToolTip6.SetFlashLabel(textFromItemHelper);
			}
			else
			{
				this.m_flToolTip6.SetFlashLabel(string.Empty);
			}
		}
		this.m_NlbItemListBox.Visible = false;
		this.m_flToolTip1.Hide(true);
		this.m_flToolTip2.Hide(true);
		this.m_flToolTip3.Hide(true);
		this.m_btSlide1.Hide(true);
		this.m_btSlide2.Hide(true);
		this.m_dwSlideBG1.Hide(true);
		this.m_dwSlideBG2.Hide(true);
		this.m_laSelectItemName.Hide(false);
		this.m_flToolTip4.Hide(false);
		this.m_flToolTip5.Hide(false);
		this.m_flToolTip6.Hide(false);
		if (this.m_BoxitemCount > 0)
		{
			this.m_btUseBoxItem.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2484"));
		}
		else
		{
			this.m_btUseBoxItem.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("415"));
		}
		this.m_btUseBoxItem.Hide(false);
		string itemMaterialCode = NrTSingleton<ItemManager>.Instance.GetItemMaterialCode(this.m_CompleteItem.m_nItemUnique);
		if (!string.IsNullOrEmpty(itemMaterialCode))
		{
			TsAudioManager.Container.RequestAudioClip("UI_ITEM", itemMaterialCode, "SELECT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "ETC", "ROULETTE_POP", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
