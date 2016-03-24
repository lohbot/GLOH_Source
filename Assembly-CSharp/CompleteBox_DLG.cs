using GAME;
using PROTOCOL.GAME;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class CompleteBox_DLG : Form
{
	private ItemTexture m_IT_ItemIcon;

	private Label m_Label_title;

	private Label m_LB_Notice;

	private Label m_LB_ItemRank;

	private Label m_LB_ItemSkillLv;

	private Button m_Btn_Ok;

	private ITEM m_SelectItem;

	private GameObject SlotEffect;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "Item/itemskill/DLG_CompleteBox", G_ID.COMPLETEBOX_DLG, true);
	}

	public override void SetComponent()
	{
		this.m_IT_ItemIcon = (base.GetControl("IT_ItemIcon") as ItemTexture);
		this.m_Label_title = (base.GetControl("Label_title") as Label);
		this.m_LB_Notice = (base.GetControl("LB_Notice") as Label);
		this.m_LB_ItemRank = (base.GetControl("LB_ItemRank") as Label);
		this.m_LB_ItemSkillLv = (base.GetControl("LB_ItemSkillLv") as Label);
		this.m_Btn_Ok = (base.GetControl("Btn_Ok") as Button);
		this.m_Btn_Ok.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnConfirm));
		base.ShowBlackBG(0.5f);
		base.SetScreenCenter();
		this.initData();
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

	public void initData()
	{
		this.m_Label_title.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3429"));
		this.m_LB_Notice.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3471"));
		this.m_Btn_Ok.SetEnabled(false);
	}

	public void SetEvolutionResultData(GS_ITEMEVOLUTION_ACK pPacket)
	{
		this.m_SelectItem = NkUserInventory.GetInstance().GetItemFromItemID(pPacket.i64BaseItemID);
		if (this.m_SelectItem == null)
		{
			ItemEvolution_Dlg itemEvolution_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.ITEMEVOLUTION_DLG) as ItemEvolution_Dlg;
			if (itemEvolution_Dlg == null)
			{
				this.CloseForm(null);
				return;
			}
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser == null)
			{
				return;
			}
			NkSoldierInfo soldierInfoFromSolID = nrCharUser.GetPersonInfo().GetSoldierInfoFromSolID(itemEvolution_Dlg.GetItemSelectSolID());
			if (soldierInfoFromSolID != null)
			{
				this.m_SelectItem = soldierInfoFromSolID.GetEquipItemInfo().GetItemFromItemID(pPacket.i64BaseItemID);
			}
		}
		if (this.m_SelectItem == null)
		{
			return;
		}
		this.m_IT_ItemIcon.SetItemTexture(this.m_SelectItem);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2931"),
			"grade",
			NrTSingleton<ItemManager>.Instance.GetItemInfo(this.m_SelectItem.m_nItemUnique).m_nStarGrade,
			"target",
			NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(this.m_SelectItem.m_nItemUnique)
		});
		this.m_LB_ItemRank.SetText(empty);
		empty = string.Empty;
		int num = this.m_SelectItem.m_nOption[5];
		int skillUnique = this.m_SelectItem.m_nOption[4];
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
		if (battleSkillBase == null)
		{
			return;
		}
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1292"),
			"skillname",
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(battleSkillBase.m_strTextKey),
			"skilllevel",
			num
		});
		this.m_LB_ItemSkillLv.SetText(empty);
		this.LoadSolComposeSuccessBundle();
		this.m_Btn_Ok.SetEnabled(true);
	}

	public void OnConfirm(IUIObject obj)
	{
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
		GameObject gameObject2 = NkUtil.GetChild(this.SlotEffect.transform, "fx_sucess").gameObject;
		if (null != gameObject2)
		{
			gameObject2.SetActive(true);
		}
		effectUIPos.x += 2.8f;
		effectUIPos.y -= 21f;
		this.SlotEffect.transform.position = effectUIPos;
		TsAudioManager.Container.RequestAudioClip("UI_SFX", "EQUIPMENT-UP", "SUCCESS", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this.SlotEffect);
		}
		if (null != this.SlotEffect)
		{
			UnityEngine.Object.DestroyObject(this.SlotEffect, 5f);
		}
	}
}
