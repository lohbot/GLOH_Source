using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ItemSkillResult_Dlg : Form
{
	private ItemTexture m_itxItem;

	private Label m_lbItemName;

	private Label m_lbItemStat;

	private Label m_lbItemStat_before;

	private Button m_btnConfirm;

	private Button m_btnUndo;

	private DrawTexture m_txBG;

	private DrawTexture m_txItemSlotBG;

	private Label m_lbTradeCount;

	private Label m_lbTradeCount_before;

	private bool m_bItemSkillSuccess;

	private ITEM m_SelectItem;

	private ITEM m_UndoItem;

	private long m_SelectItemSolID;

	private Label m_lbBefore;

	private Label m_lbAfter;

	private Label m_lbBefore2;

	private Label m_lbAfter2;

	private GameObject SlotEffect;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/itemskill/dlg_Itemskillresult", G_ID.ITEMSKILL_RESULT_DLG, true);
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
	}

	public override void SetComponent()
	{
		this.m_txItemSlotBG = (base.GetControl("DrawTexture_equip2") as DrawTexture);
		this.m_txBG = (base.GetControl("DrawTexture_bg2") as DrawTexture);
		this.m_txBG.SetTextureFromBundle("UI/Etc/reforge_magic");
		this.m_itxItem = (base.GetControl("DrawTexture_equip3") as ItemTexture);
		this.m_lbItemName = (base.GetControl("Label_equip2") as Label);
		this.m_lbItemStat = (base.GetControl("Label_stat2") as Label);
		this.m_lbItemStat_before = (base.GetControl("Label_stat2_before") as Label);
		this.m_btnConfirm = (base.GetControl("Button_Confirm") as Button);
		this.m_btnConfirm.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Click_Confirm));
		this.m_lbTradeCount = (base.GetControl("Label_stat3") as Label);
		this.m_lbTradeCount_before = (base.GetControl("Label_stat3_before") as Label);
		this.m_btnUndo = (base.GetControl("Button_undo") as Button);
		this.m_btnUndo.AddValueChangedDelegate(new EZValueChangedDelegate(this.On_Click_Undo));
		this.m_lbBefore = (base.GetControl("Label_before") as Label);
		this.m_lbAfter = (base.GetControl("Label_after") as Label);
		this.m_lbBefore2 = (base.GetControl("Label_before2") as Label);
		this.m_lbBefore2.Visible = false;
		this.m_lbAfter2 = (base.GetControl("Label_after2") as Label);
		this.m_lbAfter2.Visible = false;
	}

	public void SetItemSkillReinforceData(GS_ITEMSKILL_REINFORCE_ACK pPacket)
	{
		base.SetShowLayer(1, false);
		this.m_lbBefore.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2960"));
		this.m_lbAfter.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2961"));
		this.m_SelectItem = NkUserInventory.GetInstance().GetItemFromItemID(pPacket.i64BaseItemID);
		if (this.m_SelectItem == null)
		{
			ItemSkill_Dlg itemSkill_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMSKILL_DLG) as ItemSkill_Dlg;
			if (itemSkill_Dlg == null)
			{
				this.CloseForm(null);
				return;
			}
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(itemSkill_Dlg.GetItemSelectSolID());
			if (soldierInfoFromSolID != null)
			{
				this.m_SelectItemSolID = itemSkill_Dlg.GetItemSelectSolID();
				this.m_SelectItem = soldierInfoFromSolID.GetEquipItemInfo().GetItemFromItemID(pPacket.i64BaseItemID);
			}
		}
		if (this.m_SelectItem == null)
		{
			return;
		}
		string name = NrTSingleton<ItemManager>.Instance.GetName(this.m_SelectItem);
		this.m_itxItem.SetItemTexture(this.m_SelectItem);
		this.m_lbItemName.SetText(name);
		ItemSkill_Dlg itemSkill_Dlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMSKILL_DLG) as ItemSkill_Dlg;
		if (itemSkill_Dlg2 != null)
		{
			itemSkill_Dlg2.UpdateData(this.m_SelectItem.m_nItemPos, this.m_SelectItem.m_nPosType, 0L);
			itemSkill_Dlg2.CheckSelectItem();
		}
		ITEMINFO itemInfo = NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_SelectItem.m_nItemUnique);
		bool flag = false;
		if (itemInfo.IsItemATB(131072L) || itemInfo.IsItemATB(524288L))
		{
			flag = true;
		}
		int skillUnique = this.m_SelectItem.m_nOption[6];
		int num = this.m_SelectItem.m_nOption[9];
		if (!flag)
		{
			skillUnique = this.m_SelectItem.m_nOption[4];
			num = this.m_SelectItem.m_nOption[5];
		}
		int num2 = 0;
		this.m_bItemSkillSuccess = false;
		string empty = string.Empty;
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		if (battleSkillBase != null)
		{
			if (pPacket.RessultType == 0)
			{
				num2 = num - 1;
				this.m_bItemSkillSuccess = true;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2963"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"skilllevel",
					num
				});
			}
			else if (pPacket.RessultType == -1)
			{
				num2 = num;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2964"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"skilllevel",
					num
				});
			}
			else if (pPacket.RessultType == -2)
			{
				num2 = num + 1;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2965"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"skilllevel",
					num
				});
			}
			else if (pPacket.RessultType == -3)
			{
				num2 = num;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2966"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"skilllevel",
					num
				});
			}
			else if (pPacket.RessultType == -4)
			{
				num2 = num;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2986"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
					"skilllevel",
					num
				});
			}
			this.m_lbAfter2.SetText(empty);
			empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2670"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
				"skilllevel",
				num2
			});
			this.m_lbBefore2.SetText(empty);
		}
		base.SetShowLayer(2, true);
		this.LoadSolComposeSuccessBundle();
	}

	public void SetData(GS_ENHANCEITEM_ACK pPacket)
	{
		base.SetShowLayer(2, false);
		int skillUnique = pPacket.i32ITEMUPGRADE[4];
		int num = pPacket.i32ITEMUPGRADE[5];
		int num2 = pPacket.i32ITEMUPGRADE[2];
		int num3 = pPacket.i32ITEMUPGRADE[2];
		int skillUnique2 = pPacket.i32ITEMOPTION[4];
		int num4 = pPacket.i32ITEMOPTION[5];
		this.m_SelectItem = NkUserInventory.GetInstance().GetItem(pPacket.nItemType, pPacket.nItemPos);
		if (this.m_SelectItem == null)
		{
			ItemSkill_Dlg itemSkill_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMSKILL_DLG) as ItemSkill_Dlg;
			if (itemSkill_Dlg == null)
			{
				this.CloseForm(null);
				return;
			}
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(itemSkill_Dlg.GetItemSelectSolID());
			if (soldierInfoFromSolID != null)
			{
				this.m_SelectItemSolID = itemSkill_Dlg.GetItemSelectSolID();
				this.m_SelectItem = soldierInfoFromSolID.GetEquipItemInfo().GetItem(pPacket.nItemPos);
			}
		}
		this.m_txItemSlotBG.SetTexture("Win_I_Frame" + ItemManager.ChangeRankToString((int)this.m_SelectItem.GetRank()));
		string name = NrTSingleton<ItemManager>.Instance.GetName(this.m_SelectItem);
		this.m_itxItem.SetItemTexture(this.m_SelectItem);
		this.m_lbItemName.SetText(name);
		this.m_lbItemStat.SetText(string.Empty);
		this.m_lbItemStat_before.SetText(string.Empty);
		this.m_bItemSkillSuccess = (pPacket.nAddSkillSuccess == 1);
		string empty = string.Empty;
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		BATTLESKILL_BASE battleSkillBase2 = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique2);
		if (battleSkillBase != null && this.m_bItemSkillSuccess)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1218"),
				"targetname",
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
				"skilllevel",
				num
			});
			this.m_lbItemStat.SetText(empty);
			if (pPacket.nItemType != 10)
			{
				ItemSkill_Dlg itemSkill_Dlg2 = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMSKILL_DLG) as ItemSkill_Dlg;
				if (itemSkill_Dlg2 != null)
				{
					itemSkill_Dlg2.UpdateData(pPacket.nItemPos, pPacket.nItemType, pPacket.i64ItemID);
				}
			}
			if (COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TRADECOUNT_USE) == 1)
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2127"),
					"count",
					pPacket.i32ITEMUPGRADE[7]
				});
				this.m_lbTradeCount.SetText(empty);
			}
			else
			{
				this.m_lbTradeCount.SetText(string.Empty);
			}
			if (battleSkillBase2 != null)
			{
				this.m_btnUndo.Visible = true;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1218"),
					"targetname",
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase2.m_strTextKey),
					"skilllevel",
					num4
				});
				this.m_lbItemStat_before.SetText(empty);
				if (COMMON_CONSTANT_Manager.GetInstance().GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_TRADECOUNT_USE) == 1)
				{
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2127"),
						"count",
						pPacket.i32ITEMOPTION[7]
					});
					this.m_lbTradeCount_before.SetText(empty);
				}
				else
				{
					this.m_lbTradeCount_before.SetText(string.Empty);
				}
			}
			else
			{
				this.m_lbTradeCount_before.SetText(string.Empty);
				this.m_lbItemStat_before.SetText(string.Empty);
				this.m_btnUndo.Visible = false;
			}
		}
		else
		{
			this.m_lbTradeCount.SetText(string.Empty);
			this.m_btnUndo.Visible = false;
		}
		if (num2 != num3)
		{
			this.m_lbItemStat.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1219"));
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("269"));
		}
		this.LoadSolComposeSuccessBundle();
	}

	private void On_Click_Confirm(IUIObject a_oObject)
	{
		this.CloseForm(null);
	}

	private void On_Click_Undo(IUIObject a_oObject)
	{
		string empty = string.Empty;
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(this.m_SelectItem.m_nItemUnique);
		ITEM_REFORGE itemReforgeData = NrTSingleton<ITEM_REFORGE_Manager>.Instance.GetItemReforgeData(itemQuailtyLevel, (int)this.m_SelectItem.GetRank());
		if (itemReforgeData != null)
		{
			this.m_UndoItem = NkUserInventory.instance.GetFirstItemByUnique(itemReforgeData.nUndoUnique);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("225"),
				"itemname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemReforgeData.nUndoUnique),
				"itemnum",
				itemReforgeData.nUndoNum
			});
			NrTSingleton<FormsManager>.Instance.ShowMessageBox(NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("224"), empty, eMsgType.MB_OK_CANCEL, new YesDelegate(this.On_MessageBok_OK), null);
		}
		else
		{
			this.CloseForm(null);
		}
	}

	private void On_MessageBok_OK(object a_oObject)
	{
		int itemQuailtyLevel = NrTSingleton<ItemManager>.Instance.GetItemQuailtyLevel(this.m_SelectItem.m_nItemUnique);
		ITEM_REFORGE itemReforgeData = NrTSingleton<ITEM_REFORGE_Manager>.Instance.GetItemReforgeData(itemQuailtyLevel, (int)this.m_SelectItem.GetRank());
		if (this.m_UndoItem == null || this.m_UndoItem.m_nItemNum < itemReforgeData.nUndoNum)
		{
			LackGold_dlg lackGold_dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GOLDLACK_DLG) as LackGold_dlg;
			if (lackGold_dlg != null && !lackGold_dlg.SetDataShopItem(itemReforgeData.nUndoUnique, eITEMMALL_TYPE.NONE))
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("213"),
					"targetname",
					NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemReforgeData.nUndoUnique)
				});
				Main_UI_SystemMessage.ADDMessage(empty);
			}
			return;
		}
		if (this.m_UndoItem != null)
		{
			GS_ENHANCEITEM_EXTRA_REQ gS_ENHANCEITEM_EXTRA_REQ = default(GS_ENHANCEITEM_EXTRA_REQ);
			gS_ENHANCEITEM_EXTRA_REQ.SolID = this.m_SelectItemSolID;
			gS_ENHANCEITEM_EXTRA_REQ.SrcItemUnique = this.m_SelectItem.m_nItemUnique;
			gS_ENHANCEITEM_EXTRA_REQ.SrcItemPos = this.m_SelectItem.m_nItemPos;
			gS_ENHANCEITEM_EXTRA_REQ.SrcPosType = this.m_SelectItem.m_nPosType;
			gS_ENHANCEITEM_EXTRA_REQ.i8RequestType = 0;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_ENHANCEITEM_EXTRA_REQ, gS_ENHANCEITEM_EXTRA_REQ);
		}
		this.CloseForm(null);
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
		effectUIPos.z = 0f;
		this.SlotEffect.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(this.SlotEffect, GUICamera.UILayer);
		if (this.m_bItemSkillSuccess)
		{
			GameObject gameObject2 = NkUtil.GetChild(this.SlotEffect.transform, "fx_sucess").gameObject;
			if (null != gameObject2)
			{
				gameObject2.SetActive(true);
			}
			effectUIPos.x += 2.8f;
			effectUIPos.y += 26f;
			this.SlotEffect.transform.position = effectUIPos;
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "EQUIPMENT-UP", "SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		}
		else
		{
			GameObject gameObject3 = NkUtil.GetChild(this.SlotEffect.transform, "fx_fail").gameObject;
			if (null != gameObject3)
			{
				gameObject3.SetActive(true);
			}
			TsAudioManager.Container.RequestAudioClip("UI_SFX", "EQUIPMENT-UP", "FAIL", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
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

	public override void OnClose()
	{
		base.OnClose();
		if (null != this.SlotEffect)
		{
			UnityEngine.Object.DestroyImmediate(this.SlotEffect);
		}
		Resources.UnloadUnusedAssets();
	}
}
