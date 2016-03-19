using GAME;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ReduceResultDlg : Form
{
	private ItemTexture m_itItem;

	private DrawTexture m_dtItemBG;

	private Label m_lbItem;

	private Label m_lbInfo;

	private Button m_btConfirm;

	private GameObject m_SlotEffect;

	private float m_fStartTime;

	private bool m_bSuccess;

	private ITEM m_Item;

	private string m_strMessage = string.Empty;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "item/dlg_reduceresult", G_ID.REDUCERESULT_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_itItem = (base.GetControl("DrawTexture_equip3") as ItemTexture);
		this.m_itItem.AddMouseOutDelegate(new EZValueChangedDelegate(this.On_Mouse_Out));
		this.m_itItem.AddMouseOverDelegate(new EZValueChangedDelegate(this.On_Mouse_Over));
		this.m_dtItemBG = (base.GetControl("DrawTexture_equip2") as DrawTexture);
		this.m_lbItem = (base.GetControl("Label_equip2") as Label);
		this.m_lbInfo = (base.GetControl("Label_stat2") as Label);
		this.m_btConfirm = (base.GetControl("Button_Confirm") as Button);
		this.m_btConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickConfirm));
		base.SetScreenCenter();
		base.SetupBalckBG("Win_T_BK", 0f, 0f, GUICamera.width * 1.43f, GUICamera.height * 1.43f, 0.5f);
	}

	public void ClickConfirm(IUIObject obj)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REDUCERESULT_DLG);
	}

	public void SetResult(GS_ENHANCEITEM_ACK ACK)
	{
		ITEM itemFromItemID = NkUserInventory.GetInstance().GetItemFromItemID(ACK.i64ItemID);
		if (itemFromItemID == null)
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(ACK.i64SolID);
			if (soldierInfoFromSolID != null)
			{
				itemFromItemID = soldierInfoFromSolID.GetEquipItemInfo().GetItemFromItemID(ACK.i64ItemID);
			}
		}
		if (itemFromItemID != null)
		{
			this.m_Item = itemFromItemID;
			this.m_itItem.SetItemTexture(this.m_Item);
			string name = NrTSingleton<ItemManager>.Instance.GetName(this.m_Item);
			this.m_lbItem.Text = ItemManager.RankTextColor(ACK.nCurRank) + name;
			this.m_dtItemBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(ACK.nCurRank));
		}
		byte i8ReduceSuccess = ACK.i8ReduceSuccess;
		if (i8ReduceSuccess != 1 && i8ReduceSuccess != 2)
		{
			this.m_bSuccess = false;
			this.m_lbInfo.Hide(true);
		}
		else
		{
			if (ACK.nItemType != 10)
			{
				ReduceMainDlg reduceMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REDUCEMAIN_DLG) as ReduceMainDlg;
				if (reduceMainDlg != null)
				{
					reduceMainDlg.UpdateData(ACK.nItemPos, ACK.nItemType, ACK.i64ItemID);
				}
			}
			int num = ACK.i32ITEMUPGRADE[8] - ACK.i32ITEMOPTION[8];
			if (0 > num)
			{
				num = 0;
			}
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref this.m_strMessage, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("982"),
				"Count",
				num
			});
			this.m_lbInfo.SetText(this.m_strMessage);
			this.m_bSuccess = true;
		}
		this.LoadSolComposeSuccessBundle();
	}

	public void LoadSolComposeSuccessBundle()
	{
		string str = string.Format("{0}{1}", "UI/Item/fx_reinforce_result", NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this._funcUIEffectDownloaded), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void _funcUIEffectDownloaded(IDownloadedItem wItem, object obj)
	{
		Main_UI_SystemMessage.CloseUI();
		if (null == wItem.mainAsset)
		{
			TsLog.LogWarning("wItem.mainAsset is null -> Path = {0}", new object[]
			{
				wItem.assetPath
			});
			return;
		}
		GameObject gameObject = wItem.mainAsset as GameObject;
		if (null == gameObject)
		{
			return;
		}
		if (null != this.m_SlotEffect)
		{
			UnityEngine.Object.Destroy(this.m_SlotEffect);
		}
		this.m_SlotEffect = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
		if (null == this.m_SlotEffect)
		{
			return;
		}
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = 300f;
		this.m_SlotEffect.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(this.m_SlotEffect, GUICamera.UILayer);
		if (this.m_bSuccess)
		{
			GameObject gameObject2 = NkUtil.GetChild(this.m_SlotEffect.transform, "fx_sucess").gameObject;
			if (null != gameObject2)
			{
				gameObject2.SetActive(true);
			}
			effectUIPos.x += 2.8f;
			effectUIPos.y += 26f;
			this.m_SlotEffect.transform.position = effectUIPos;
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "EQUIPMENT-UP", "SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			GameObject gameObject3 = NkUtil.GetChild(this.m_SlotEffect.transform, "fx_fail").gameObject;
			if (null != gameObject3)
			{
				gameObject3.SetActive(true);
			}
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "EQUIPMENT-UP", "FAIL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.m_SlotEffect);
		}
		this.m_fStartTime = Time.realtimeSinceStartup;
	}

	public override void OnClose()
	{
		base.OnClose();
		if (null != this.m_SlotEffect)
		{
			UnityEngine.Object.Destroy(this.m_SlotEffect);
		}
	}

	public override void Update()
	{
		if (0f < this.m_fStartTime && Time.realtimeSinceStartup - this.m_fStartTime >= 5f)
		{
			if (null != this.m_SlotEffect)
			{
				UnityEngine.Object.DestroyImmediate(this.m_SlotEffect);
			}
			this.m_fStartTime = 0f;
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.REDUCERESULT_DLG);
		}
	}

	private void On_Mouse_Over(IUIObject a_oObject)
	{
		ImageView imageView = a_oObject as ImageView;
		if (imageView != null)
		{
			IUIListObject mouseItem = imageView.MouseItem;
			if (mouseItem != null)
			{
				ImageSlot imageSlot = mouseItem.Data as ImageSlot;
				if (imageSlot != null && imageSlot.c_oItem != null)
				{
					ITEM iTEM = new ITEM();
					iTEM.Set(imageSlot.c_oItem as ITEM);
					if (iTEM != null && iTEM.m_nItemUnique > 0)
					{
						ItemTooltipDlg itemTooltipDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ITEMTOOLTIP_DLG) as ItemTooltipDlg;
						itemTooltipDlg.Set_Tooltip((G_ID)base.WindowID, iTEM, null, false);
					}
				}
			}
		}
	}

	private void On_Mouse_Out(IUIObject a_oObject)
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.ITEMTOOLTIP_DLG);
	}
}
