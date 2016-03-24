using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ItemComposeDlg : Form
{
	public const int COMPOSITEM_COUNT_MIN = 1;

	private DrawTexture m_dtMainBG;

	private ItemTexture m_itSelectItem;

	private Label m_lbSelectItem;

	private Label m_lbSelectItemCount;

	private Label m_lbItemResult;

	private Button m_btNext_add;

	private Button m_btNext_minus;

	private Button m_btNext_max;

	private Button m_btNext_ok;

	private NewListBox m_nlbNeedItem;

	private ITEM_COMPOSE m_BaseComposeData;

	public int m_NowSelectItemCount = 1;

	private ITEM_COMPOSE_Manager.Compose_Material_Item[] m_NeedItemList;

	private bool bSendServerOK = true;

	private GameObject m_RootGameObject;

	private bool m_bLoadActionReforge;

	private float m_fStartTime;

	private bool m_bRequest;

	private int m_iComposeResultItemUnique;

	private int m_iComposeResultItemNum;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "item/dlg_Compose", G_ID.ITEMCOMPOSE_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_dtMainBG = (base.GetControl("DrawTexture_SubBG") as DrawTexture);
		this.m_dtMainBG.SetTextureFromBundle("UI/Etc/reforge");
		this.m_itSelectItem = (base.GetControl("ItemTexture_Item") as ItemTexture);
		this.m_lbSelectItem = (base.GetControl("Label_Item") as Label);
		this.m_lbSelectItemCount = (base.GetControl("Label_Count") as Label);
		this.m_lbItemResult = (base.GetControl("Label_Result") as Label);
		this.m_lbItemResult.Visible = false;
		this.m_btNext_minus = (base.GetControl("Button_Next1") as Button);
		this.m_btNext_minus.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMinus));
		this.m_btNext_add = (base.GetControl("Button_Next2") as Button);
		this.m_btNext_add.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickAdd));
		this.m_btNext_max = (base.GetControl("Button_Max") as Button);
		this.m_btNext_max.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMax));
		this.m_btNext_ok = (base.GetControl("Button_OK") as Button);
		this.m_btNext_ok.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
		this.m_nlbNeedItem = (base.GetControl("NewListBox_Item") as NewListBox);
		this.m_nlbNeedItem.Reserve = false;
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void InitData()
	{
		int p_nCharKind = base.p_nCharKind;
		if (p_nCharKind <= 0)
		{
			return;
		}
		if (this.m_BaseComposeData == null)
		{
			this.m_BaseComposeData = NrTSingleton<ITEM_COMPOSE_Manager>.Instance.GetComposeItemByNPCKIND(p_nCharKind);
			if (this.m_BaseComposeData == null)
			{
				return;
			}
		}
		this.m_itSelectItem.SetItemTexture(this.m_BaseComposeData.m_nComposeItemUnique);
		this.m_lbSelectItem.SetText(NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_BaseComposeData.m_nComposeItemUnique));
		this.m_NowSelectItemCount = 1;
		this.ReloadSelectItemCount();
		this.ReloadMaterialList();
	}

	public void ReloadSelectItemCount()
	{
		this.m_lbSelectItemCount.SetText(this.m_NowSelectItemCount.ToString());
	}

	public void ReloadMaterialList()
	{
		if (this.m_NeedItemList == null)
		{
			this.m_NeedItemList = NrTSingleton<ITEM_COMPOSE_Manager>.Instance.ComposeItem_MaterialList(this.m_BaseComposeData);
		}
		if (this.m_NeedItemList != null)
		{
			this.m_nlbNeedItem.Clear();
			this.bSendServerOK = true;
			for (int i = 0; i < this.m_NeedItemList.Length; i++)
			{
				int nItemUnique = this.m_NeedItemList[i].m_nItemUnique;
				int itemCnt = NkUserInventory.GetInstance().GetItemCnt(nItemUnique);
				if (nItemUnique > 0)
				{
					NewListItem newListItem = new NewListItem(this.m_nlbNeedItem.ColumnNum, true, string.Empty);
					newListItem.SetListItemData(0, true);
					newListItem.SetListItemData(1, this.m_NeedItemList[i].m_t2ItemIcon, null, null, null);
					newListItem.SetListItemData(2, this.m_NeedItemList[i].m_strItemName, null, null, null);
					string text = this.ReCountMaterialItem(this.m_NeedItemList[i].m_shItemNumber, itemCnt);
					newListItem.SetListItemData(3, text, null, null, null);
					this.m_nlbNeedItem.Add(newListItem);
				}
			}
			this.m_nlbNeedItem.RepositionItems();
		}
	}

	public string ReCountMaterialItem(int NeedMaterialNum, int UserItemNum)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1105");
		int num = this.m_NowSelectItemCount * NeedMaterialNum;
		if (num > UserItemNum)
		{
			textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1401");
			this.bSendServerOK = false;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty2, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1984"),
			"count1",
			num,
			"count2",
			UserItemNum
		});
		return string.Format("{0}{1}", textColor, empty2);
	}

	public void ClickMinus(IUIObject obj)
	{
		if (this.m_BaseComposeData == null)
		{
			return;
		}
		if (this.m_NowSelectItemCount == 1)
		{
			return;
		}
		if (this.m_NowSelectItemCount < 1)
		{
			this.m_NowSelectItemCount = 1;
		}
		else
		{
			this.m_NowSelectItemCount--;
		}
		this.ReloadSelectItemCount();
		this.ReloadMaterialList();
	}

	public void ClickAdd(IUIObject obj)
	{
		if (this.m_BaseComposeData == null)
		{
			return;
		}
		this.m_NowSelectItemCount++;
		this.ReloadSelectItemCount();
		this.ReloadMaterialList();
	}

	public void ClickMax(IUIObject obj)
	{
		if (this.m_BaseComposeData == null)
		{
			return;
		}
		int num = NrTSingleton<ITEM_COMPOSE_Manager>.Instance.CanComposeItemNum(this.m_BaseComposeData, -1);
		if (num <= 0)
		{
			this.m_NowSelectItemCount = 1;
		}
		else
		{
			this.m_NowSelectItemCount = num;
		}
		this.ReloadSelectItemCount();
		this.ReloadMaterialList();
	}

	public void ClickOK(IUIObject obj)
	{
		if (this.m_BaseComposeData != null && this.m_NowSelectItemCount > 0 && !this.m_bRequest)
		{
			if (this.bSendServerOK)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("181"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_BaseComposeData.m_nComposeItemUnique)
				});
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.OnSendComposeOk), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("1983"), empty, eMsgType.MB_OK_CANCEL, 2);
			}
			else
			{
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("606"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
			}
		}
	}

	public void OnSendComposeOk(object a_oObject)
	{
		GS_ITEM_COMPOSE_REQ gS_ITEM_COMPOSE_REQ = new GS_ITEM_COMPOSE_REQ();
		gS_ITEM_COMPOSE_REQ.m_nComposeProductionID = this.m_BaseComposeData.m_nComposeProductionID;
		gS_ITEM_COMPOSE_REQ.m_nComposeItemNum = this.m_NowSelectItemCount;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ITEM_COMPOSE_REQ, gS_ITEM_COMPOSE_REQ);
	}

	public void ActionCompose(int ItemUnique, int ItemNum)
	{
		if (!this.m_bRequest && ItemUnique > 0 && ItemNum > 0)
		{
			this.m_iComposeResultItemUnique = ItemUnique;
			this.m_iComposeResultItemNum = ItemNum;
			string str = string.Format("{0}", "UI/Item/fx_reinforcestone_ui" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetActionCompose), null);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			this.m_bRequest = true;
		}
	}

	private void SetActionCompose(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null != _item.GetSafeBundle() && null != _item.GetSafeBundle().mainAsset)
		{
			GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
			if (null != gameObject)
			{
				this.m_RootGameObject = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
				Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
				Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
				effectUIPos.z = 300f;
				this.m_RootGameObject.transform.position = effectUIPos;
				NkUtil.SetAllChildLayer(this.m_RootGameObject, GUICamera.UILayer);
				base.InteractivePanel.MakeChild(this.m_RootGameObject);
				this.m_fStartTime = Time.time;
				this.m_bLoadActionReforge = true;
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("607"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_iComposeResultItemUnique),
					"count",
					this.m_iComposeResultItemNum.ToString()
				});
				this.m_lbItemResult.SetText(empty);
				this.m_lbItemResult.SetLocationZ(this.m_RootGameObject.transform.localPosition.z - 4f);
				this.m_lbItemResult.Visible = true;
				if (TsPlatform.IsMobile && TsPlatform.IsEditor)
				{
					NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_RootGameObject);
				}
			}
		}
	}

	public override void Update()
	{
		if (this.m_bLoadActionReforge && Time.time - this.m_fStartTime > 1.7f)
		{
			UnityEngine.Object.DestroyImmediate(this.m_RootGameObject);
			this.m_bLoadActionReforge = false;
			this.m_bRequest = false;
			this.m_lbItemResult.Visible = false;
			this.m_iComposeResultItemUnique = 0;
			this.m_iComposeResultItemNum = 0;
		}
	}
}
