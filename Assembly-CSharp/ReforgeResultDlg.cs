using GAME;
using PROTOCOL.GAME;
using System;
using System.Text;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ReforgeResultDlg : Form
{
	private ItemTexture m_txItemAfter;

	private DrawTexture m_txItemBG;

	private DrawTexture m_txBG;

	private Label m_lbItemAfterName;

	private Label m_lbItemAfterStat;

	private Label m_lbItemGrade;

	private Button m_btOK;

	private ITEM m_Item;

	private bool m_bFacebookBtn;

	private bool m_bItemUpgradeSuccess;

	private GameObject SlotEffect;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Reforge/DLG_ReforgeResult", G_ID.REFORGERESULT_DLG, true);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_txItemAfter = (base.GetControl("DrawTexture_equip3") as ItemTexture);
		this.m_lbItemAfterName = (base.GetControl("Label_equip2") as Label);
		this.m_lbItemAfterStat = (base.GetControl("Label_stat2") as Label);
		this.m_lbItemGrade = (base.GetControl("Label_grade") as Label);
		this.m_btOK = (base.GetControl("BT_Feed") as Button);
		this.m_btOK.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickConfirm));
		this.m_txItemBG = (base.GetControl("DrawTexture_equip2") as DrawTexture);
		this.m_txBG = (base.GetControl("DrawTexture_bg2") as DrawTexture);
		this.m_txBG.SetTextureFromBundle("UI/Etc/reforge");
	}

	public void SetData(GS_ENHANCEITEM_ACK pPacket)
	{
		NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_Money = pPacket.LeftMoney;
		ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
		ITEM item = NkUserInventory.GetInstance().GetItem(pPacket.nItemType, pPacket.nItemPos);
		if (item == null)
		{
			if (reforgeMainDlg == null)
			{
				return;
			}
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(reforgeMainDlg.GetSolID());
			if (soldierInfoFromSolID != null)
			{
				item = soldierInfoFromSolID.GetEquipItemInfo().GetItem(pPacket.nItemPos);
			}
		}
		this.m_Item = item;
		this.m_txItemAfter.SetItemTexture(item);
		string name = NrTSingleton<ItemManager>.Instance.GetName(item);
		this.m_lbItemAfterName.Text = ItemManager.RankTextColor(pPacket.nCurRank) + name;
		this.m_lbItemGrade.Text = ItemManager.RankTextColor(pPacket.nCurRank) + ItemManager.RankText(pPacket.nCurRank);
		this.m_txItemBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString(pPacket.nCurRank));
		this.GetStatString(item.m_nItemUnique, pPacket);
		this.LoadSolComposeSuccessBundle();
		if (pPacket.i8ItemEnchantSuccess == 1 && pPacket.nLastRank < pPacket.nCurRank)
		{
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "EQUIPMENT-UP", "SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			this.m_bItemUpgradeSuccess = true;
			this.FacebookButtonSet(false);
			if (pPacket.nItemType != 10)
			{
				ReforgeSelectDlg reforgeSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGESELECT_DLG) as ReforgeSelectDlg;
				if (reforgeSelectDlg != null)
				{
					reforgeSelectDlg.UpdateData(pPacket.nItemPos, pPacket.nItemType, pPacket.i64ItemID);
				}
			}
		}
		else
		{
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "EQUIPMENT-UP", "FAIL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			this.m_bItemUpgradeSuccess = false;
			this.FacebookButtonSet(false);
		}
		TsLog.Log("Packet SolID ={0}", new object[]
		{
			pPacket.i64SolID
		});
		if (reforgeMainDlg != null)
		{
			reforgeMainDlg.bSendRequest = false;
		}
	}

	private void FacebookButtonSet(bool bShow)
	{
		if (!TsPlatform.IsBand && !NrTSingleton<ContentsLimitManager>.Instance.IsFacebookLimit())
		{
			this.m_bFacebookBtn = bShow;
		}
		if (bShow && !TsPlatform.IsBand && !NrTSingleton<ContentsLimitManager>.Instance.IsFacebookLimit())
		{
			this.m_btOK.SetButtonTextureKey("Win_B_FaceBook");
			this.m_btOK.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("183"));
		}
		else
		{
			this.m_btOK.SetButtonTextureKey("Win_B_BasicBtn01");
			this.m_btOK.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
		}
	}

	private void GetStatString(int ItemUnique, GS_ENHANCEITEM_ACK pPacket)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = string.Empty;
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1011");
		string textColor2 = NrTSingleton<CTextParser>.Instance.GetTextColor("1401");
		string textColor3 = NrTSingleton<CTextParser>.Instance.GetTextColor("1304");
		if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(ItemUnique) == eITEM_PART.ITEMPART_WEAPON)
		{
			int num = Protocol_Item.Get_Min_Damage(ItemUnique, 0);
			int optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[0], num, 1);
			int nValue = Protocol_Item.Get_Max_Damage(ItemUnique, 0);
			int optionValue2 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[0], nValue, 1);
			stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("242") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString(), " ~ ", optionValue2.ToString()));
			int num2 = Protocol_Item.Get_Min_Damage(ItemUnique, 0);
			int optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[0], num2, 1);
			int nValue2 = Protocol_Item.Get_Max_Damage(ItemUnique, 0);
			int optionValue4 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[0], nValue2, 1);
			int num3 = optionValue3 - optionValue;
			int num4 = optionValue4 - optionValue2;
			if (num4 != 0)
			{
				text = ((num4 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", ((num3 <= 0) ? string.Empty : "+") + num3.ToString(), " ~ ", ((num4 <= 0) ? string.Empty : "+") + num4.ToString(), ")");
			}
			stringBuilder2.AppendLine(string.Concat(new string[]
			{
				textColor,
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("242"),
				NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue3.ToString(), " ~ ", optionValue4.ToString()),
				" ",
				text
			}));
			text = string.Empty;
			num = Protocol_Item.Get_Critical_Plus(ItemUnique, 0);
			optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[1], num, 3);
			stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("256") + " " + optionValue.ToString());
			num2 = Protocol_Item.Get_Critical_Plus(ItemUnique, 0);
			optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[1], num2, 3);
			num3 = optionValue3 - optionValue;
			if (num3 != 0)
			{
				text = ((num3 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", (num3 <= 0) ? string.Empty : "+", num3.ToString(), ")");
			}
			stringBuilder2.AppendLine(string.Concat(new string[]
			{
				textColor,
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("256"),
				" ",
				optionValue3.ToString(),
				" ",
				text
			}));
		}
		else if (NrTSingleton<ItemManager>.Instance.GetItemPartByItemUnique(ItemUnique) == eITEM_PART.ITEMPART_ARMOR)
		{
			int num = Protocol_Item.Get_Defense(ItemUnique, 0);
			int optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[0], num, 2);
			stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("243") + NrTSingleton<UIDataManager>.Instance.GetString(" ", optionValue.ToString()));
			int num2 = Protocol_Item.Get_Defense(ItemUnique, 0);
			int optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[0], num2, 2);
			int num3 = optionValue3 - optionValue;
			if (num3 != 0)
			{
				text = ((num3 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", (num3 <= 0) ? string.Empty : "+", num3.ToString(), ")");
			}
			stringBuilder2.AppendLine(string.Concat(new string[]
			{
				textColor,
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("243"),
				" ",
				optionValue3.ToString(),
				" ",
				text
			}));
			text = string.Empty;
			num = Protocol_Item.Get_Evasion_Plus(ItemUnique, 0);
			if (num != 0)
			{
				optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[1], num, 7);
				stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("255") + " " + optionValue.ToString());
				num2 = Protocol_Item.Get_Evasion_Plus(ItemUnique, 0);
				optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[1], num2, 7);
				num3 = optionValue3 - optionValue;
				if (num3 != 0)
				{
					text = ((num3 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", (num3 <= 0) ? string.Empty : "+", num3.ToString(), ")");
				}
				if (num2 != 0)
				{
					stringBuilder2.AppendLine(string.Concat(new string[]
					{
						textColor,
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("255"),
						" ",
						optionValue3.ToString(),
						" ",
						text
					}));
				}
			}
			num = Protocol_Item.Get_ADDHP(ItemUnique, 0);
			if (num != 0)
			{
				optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[1], num, 4);
				stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1216") + " " + optionValue.ToString());
				num2 = Protocol_Item.Get_ADDHP(ItemUnique, 0);
				optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[1], num2, 4);
				num3 = optionValue3 - optionValue;
				if (num3 != 0)
				{
					text = ((num3 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", (num3 <= 0) ? string.Empty : "+", num3.ToString(), ")");
				}
				if (num2 != 0)
				{
					stringBuilder2.AppendLine(string.Concat(new string[]
					{
						textColor,
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1216"),
						" ",
						optionValue3.ToString(),
						" ",
						text
					}));
				}
			}
			num = Protocol_Item.Get_Hitrate_Plus(ItemUnique, 0);
			if (num != 0)
			{
				optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[1], num, 6);
				stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("254") + " " + optionValue.ToString());
				num2 = Protocol_Item.Get_Hitrate_Plus(ItemUnique, 0);
				optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[1], num2, 6);
				num3 = optionValue3 - optionValue;
				if (num3 != 0)
				{
					text = ((num3 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", (num3 <= 0) ? string.Empty : "+", num3.ToString(), ")");
				}
				if (num2 != 0)
				{
					stringBuilder2.AppendLine(string.Concat(new string[]
					{
						textColor,
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("254"),
						" ",
						optionValue3.ToString(),
						" ",
						text
					}));
				}
			}
			num = Protocol_Item.Get_Critical_Plus(ItemUnique, 0);
			if (num != 0)
			{
				optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[1], num, 3);
				stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("256") + " " + optionValue.ToString());
				num2 = Protocol_Item.Get_Critical_Plus(ItemUnique, 0);
				optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[1], num2, 3);
				num3 = optionValue3 - optionValue;
				if (num3 != 0)
				{
					text = ((num3 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", (num3 <= 0) ? string.Empty : "+", num3.ToString(), ")");
				}
				if (num2 != 0)
				{
					stringBuilder2.AppendLine(string.Concat(new string[]
					{
						textColor,
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("256"),
						" ",
						optionValue3.ToString(),
						" ",
						text
					}));
				}
			}
			num = Protocol_Item.Get_INT(ItemUnique, 0);
			if (num != 0)
			{
				optionValue = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMOPTION[1], num, 5);
				stringBuilder.AppendLine(textColor + NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1211") + " " + optionValue.ToString());
				num2 = Protocol_Item.Get_INT(ItemUnique, 0);
				optionValue3 = Tooltip_Dlg.GetOptionValue(ItemUnique, pPacket.i32ITEMUPGRADE[1], num2, 5);
				num3 = optionValue3 - optionValue;
				if (num3 != 0)
				{
					text = ((num3 <= 0) ? textColor2 : textColor3) + NrTSingleton<UIDataManager>.Instance.GetString("(", (num3 <= 0) ? string.Empty : "+", num3.ToString(), ")");
				}
				if (num2 != 0)
				{
					stringBuilder2.AppendLine(string.Concat(new string[]
					{
						textColor,
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1211"),
						" ",
						optionValue3.ToString(),
						" ",
						text
					}));
				}
			}
		}
		this.m_lbItemAfterStat.Text = stringBuilder2.ToString();
	}

	private void OnClickConfirm(IUIObject a_oObject)
	{
		if (this.m_bFacebookBtn && this.m_Item != null)
		{
			Facebook_Feed_Dlg facebook_Feed_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.FACEBOOK_FEED_DLG) as Facebook_Feed_Dlg;
			if (facebook_Feed_Dlg != null)
			{
				facebook_Feed_Dlg.SetType(eFACEBOOK_FEED_TYPE.ENCHANT_ITEM, this.m_Item);
			}
		}
		this.Close();
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
		this.SlotEffect = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
		if (null == this.SlotEffect)
		{
			return;
		}
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = 300f;
		this.SlotEffect.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(this.SlotEffect, GUICamera.UILayer);
		if (this.m_bItemUpgradeSuccess)
		{
			GameObject gameObject2 = NkUtil.GetChild(this.SlotEffect.transform, "fx_sucess").gameObject;
			if (null != gameObject2)
			{
				gameObject2.SetActive(true);
			}
			effectUIPos.x += 2.8f;
			effectUIPos.y += 26f;
			this.SlotEffect.transform.position = effectUIPos;
		}
		else
		{
			GameObject gameObject3 = NkUtil.GetChild(this.SlotEffect.transform, "fx_fail").gameObject;
			if (null != gameObject3)
			{
				gameObject3.SetActive(true);
			}
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.SlotEffect);
		}
		if (null != this.SlotEffect)
		{
			UnityEngine.Object.DestroyObject(this.SlotEffect, 5f);
		}
	}

	public override void Hide()
	{
		ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
		if (reforgeMainDlg != null)
		{
			reforgeMainDlg.ReFreshItem();
		}
		base.OnClose();
		if (null != this.SlotEffect)
		{
			UnityEngine.Object.DestroyImmediate(this.SlotEffect);
		}
		Resources.UnloadUnusedAssets();
		base.Hide();
	}

	public override void OnClose()
	{
		ReforgeMainDlg reforgeMainDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGEMAIN_DLG) as ReforgeMainDlg;
		if (reforgeMainDlg != null)
		{
			reforgeMainDlg.ReFreshItem();
		}
		ReforgeSelectDlg reforgeSelectDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.REFORGESELECT_DLG) as ReforgeSelectDlg;
		if (reforgeSelectDlg != null)
		{
			reforgeSelectDlg.closeButton.Visible = true;
		}
		base.OnClose();
		if (null != this.SlotEffect)
		{
			UnityEngine.Object.DestroyImmediate(this.SlotEffect);
		}
		Resources.UnloadUnusedAssets();
	}
}
