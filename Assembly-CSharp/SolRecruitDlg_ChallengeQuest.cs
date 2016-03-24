using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolRecruitDlg_ChallengeQuest : SolRecruitDlg
{
	private GameObject _directionObj;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/Recruit/DLG_SolRecruit_New", G_ID.SOLRECRUIT_CHALLENGEQUEST_DLG, true);
		base.InteractivePanel.draggable = false;
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		base.SetComponent();
		string textureFromBundle = string.Format("UI/SolRecruit/solrecruit_baner01", new object[0]);
		this.m_BackImage.SetTextureFromBundle(textureFromBundle);
	}

	public override void SetTicketList()
	{
		COMMON_CONSTANT_Manager instance = COMMON_CONSTANT_Manager.GetInstance();
		if (instance == null)
		{
			Debug.LogError("ERROR, SolRecruitDlg_ChallengeQuest.cs, SetTicketList(), pkConstant is null");
			return;
		}
		string text = string.Empty;
		string empty = string.Empty;
		this.m_TicketList.Clear();
		if (!NrTSingleton<ContentsLimitManager>.Instance.IsLegendHire())
		{
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2862");
			NewListItem newListItem = new NewListItem(this.m_TicketList.ColumnNum, true, string.Empty);
			int value = instance.GetValue(eCOMMON_CONSTANT.eCOMMON_CONSTANT_ESSENCE_ITEMUNIQUE);
			int needItemNum = SolRecruitDlg.GetNeedItemNum(13);
			newListItem.SetListItemData(1, NrTSingleton<ItemManager>.Instance.GetItemTexture(value), null, null, null);
			newListItem.SetListItemEnable(3, true);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				text,
				"targetname",
				NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(value),
				"count1",
				needItemNum,
				"count2",
				SolRecruitDlg.GetSolCount(13)
			});
			newListItem.SetListItemData(2, empty, null, null, null);
			newListItem.SetListItemData(3, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("573"), eSolRecruitType.SOLRECRUIT_ESSENCE_ONE, new EZValueChangedDelegate(this.ClickTicketList), null);
			newListItem.SetListItemData(4, false);
			newListItem.SetListItemData(6, false);
			newListItem.Data = eSolRecruitType.SOLRECRUIT_ESSENCE_ONE;
			this.m_TicketList.Add(newListItem);
		}
		this.m_TicketList.RepositionItems();
	}

	protected override void ClickPrimiumMall(IUIObject obj)
	{
	}

	private void SolRecruitDummySuccess(WWWItem _item, object _param)
	{
		Main_UI_SystemMessage.CloseUI();
		if (this == null)
		{
			return;
		}
		if (_item.GetSafeBundle() == null || _item.GetSafeBundle().mainAsset == null)
		{
			return;
		}
		GameObject gameObject = _item.GetSafeBundle().mainAsset as GameObject;
		if (gameObject == null)
		{
			return;
		}
		this._directionObj = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
		this._directionObj.tag = NrTSingleton<UIDataManager>.Instance.UIBundleTag;
		Vector2 screenPos = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
		Vector3 effectUIPos = base.GetEffectUIPos(screenPos);
		effectUIPos.z = base.InteractivePanel.transform.position.z - 10f;
		this._directionObj.transform.position = effectUIPos;
		NkUtil.SetAllChildLayer(this._directionObj, GUICamera.UILayer);
		GameObject gameObject2 = NkUtil.GetChild(this._directionObj.transform, "fx_direct_dragonhero").gameObject;
		if (gameObject2 != null)
		{
			gameObject2.SetActive(true);
		}
		if (TsPlatform.IsMobile && TsPlatform.IsEditor)
		{
			NrTSingleton<NkClientLogic>.Instance.SetEditorShaderConvert(ref this._directionObj);
		}
		Animation[] componentsInChildren = this._directionObj.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Animation animation = array[i];
			if (!(animation == null))
			{
				if (animation.cullingType != AnimationCullingType.AlwaysAnimate)
				{
					animation.cullingType = AnimationCullingType.AlwaysAnimate;
				}
			}
		}
		this.CreateCloseButton();
		this.SetDirectionDummySoldier();
	}

	private void SetSolFaceImage(WWWItem item, object param)
	{
		if (this._directionObj == null)
		{
			return;
		}
		Texture2D texture = this.SaveBundleImage(item, param);
		this.SetImageAtTarget(texture, "face");
	}

	private void SetImage(WWWItem item, object param)
	{
		if (this._directionObj == null || param == null)
		{
			return;
		}
		object[] array = param as object[];
		if (array == null || array.Length != 2)
		{
			return;
		}
		string param2 = array[0] as string;
		string childObjName = array[1] as string;
		Texture2D texture = this.SaveBundleImage(item, param2);
		this.SetImageAtTarget(texture, childObjName);
	}

	protected override void ClickTicketList(IUIObject obj)
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		else
		{
			NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.UIGUIDE_DLG);
		}
		if (obj.Data == null)
		{
			Debug.LogError("ERROR, SolRecruitDlg_ChallengeQuest.cs, ClickTicketList(), obj.Data is Null");
			return;
		}
		eSolRecruitType recruittype = (eSolRecruitType)((int)obj.Data);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1699");
		string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("296");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref textFromMessageBox, new object[]
		{
			textFromMessageBox,
			"count",
			SolRecruitDlg.GetNeedItemNum((int)recruittype)
		});
		MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
		if (msgBoxUI == null)
		{
			return;
		}
		msgBoxUI.SetMsg(new YesDelegate(this.OkTickekLegendHero), null, null, null, textFromInterface, textFromMessageBox, eMsgType.MB_OK_CANCEL);
	}

	private void OnClickDirectionCloseBtn(IUIObject obj)
	{
		Debug.Log("Click Close Button");
		if (this._directionObj == null)
		{
			return;
		}
		Animation componentInChildren = this._directionObj.GetComponentInChildren<Animation>();
		if (componentInChildren == null || componentInChildren.isPlaying)
		{
			return;
		}
		this.ShowGameGuideDlg();
		UnityEngine.Object.Destroy(this._directionObj);
	}

	private void OkTickekLegendHero(object obj)
	{
		this.ShowRecruitDummyDirection();
	}

	public static void OnLegendRecruitGuideEnd()
	{
		SolRecruitDlg_ChallengeQuest.SendSuccessPacket(ChallengeManager.eCHALLENGECODE.CHALLENGECODE_LEGEND_ESSENCE_RECRUIT);
	}

	private void ShowRecruitDummyDirection()
	{
		string str = string.Format("{0}", "UI/Soldier/fx_direct_dragonhero" + NrTSingleton<UIDataManager>.Instance.AddFilePath);
		WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
		wWWItem.SetItemType(ItemType.USER_ASSETB);
		wWWItem.SetCallback(new PostProcPerItem(this.SolRecruitDummySuccess), null);
		TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
	}

	private void CreateCloseButton()
	{
		if (this._directionObj == null)
		{
			return;
		}
		Transform child = NkUtil.GetChild(this._directionObj.transform, "fx_white");
		if (child == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(child.gameObject) as GameObject;
		gameObject.name = "closebox(Clone)";
		gameObject.transform.parent = child;
		gameObject.transform.localScale = child.transform.localScale;
		gameObject.AddComponent<BoxCollider>();
		gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 10f, gameObject.transform.position.z);
		Button button = gameObject.AddComponent<Button>();
		button.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickDirectionCloseBtn));
	}

	private void SetDirectionDummySoldier()
	{
		NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode("sol_ereina");
		if (charKindInfoFromCode == null)
		{
			return;
		}
		string str = string.Empty;
		if (UIDataManager.IsUse256Texture())
		{
			str = "_256";
		}
		else
		{
			str = "_512";
		}
		string imageKey = charKindInfoFromCode.GetPortraitFile1(6, string.Empty) + str;
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey);
		if (texture == null)
		{
			NrTSingleton<UIImageBundleManager>.Instance.RequestCharImage(imageKey, eCharImageType.LARGE, new PostProcPerItem(this.SetSolFaceImage));
		}
		else
		{
			this.SetImageAtTarget(texture, "face");
		}
		string imageKey2 = "font_number" + (charKindInfoFromCode.GetSeason(1) + 1).ToString();
		this.SetDummyImage(imageKey2, "fx_font_number");
		this.SetDummyImage("card_legend", "back");
		this.SetDummyImage("rankl7", "rank");
	}

	private void SetDummyImage(string imageKey, string targetChildName)
	{
		object[] callbackParam = new object[]
		{
			imageKey,
			targetChildName
		};
		Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey);
		if (texture == null)
		{
			string str = string.Format("{0}", "UI/Soldier/" + imageKey + NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetImage), callbackParam);
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
		}
		else
		{
			this.SetImageAtTarget(texture, targetChildName);
		}
	}

	private void SetImageAtTarget(Texture2D texture, string childObjName)
	{
		if (texture == null || string.IsNullOrEmpty(childObjName))
		{
			return;
		}
		GameObject gameObject = NkUtil.GetChild(this._directionObj.transform, childObjName).gameObject;
		if (gameObject == null)
		{
			return;
		}
		Material material = new Material(Shader.Find("Transparent/Vertex Colored" + NrTSingleton<UIDataManager>.Instance.AddFilePath));
		if (material == null)
		{
			return;
		}
		material.mainTexture = texture;
		if (gameObject.renderer == null)
		{
			return;
		}
		gameObject.renderer.sharedMaterial = material;
	}

	private Texture2D SaveBundleImage(WWWItem item, object param)
	{
		if (item.GetSafeBundle() == null || item.GetSafeBundle().mainAsset == null)
		{
			return null;
		}
		if (item.GetSafeBundle().mainAsset == null)
		{
			return null;
		}
		Texture2D texture2D = item.GetSafeBundle().mainAsset as Texture2D;
		if (texture2D == null)
		{
			return null;
		}
		if (!(param is string))
		{
			return null;
		}
		string imageKey = (string)param;
		if (NrTSingleton<UIImageBundleManager>.Instance.GetTexture(imageKey) == null)
		{
			NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
		}
		return texture2D;
	}

	private void ShowGameGuideDlg()
	{
		NrTSingleton<GameGuideManager>.Instance.Update(GameGuideCheck.NONE, GameGuideType.CHALLENGE_LEGEND_RECRUIT);
		NrTSingleton<GameGuideManager>.Instance.Update();
		GameGuideDlg gameGuideDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.GAMEGUIDE_DLG) as GameGuideDlg;
		gameGuideDlg.RegistCloseCallback(new OnCloseCallback(SolRecruitDlg_ChallengeQuest.OnLegendRecruitGuideEnd));
	}

	private static void SendSuccessPacket(ChallengeManager.eCHALLENGECODE type)
	{
		GS_RECOMMEND_CHALLENGE_CLEAR_REQ gS_RECOMMEND_CHALLENGE_CLEAR_REQ = new GS_RECOMMEND_CHALLENGE_CLEAR_REQ();
		gS_RECOMMEND_CHALLENGE_CLEAR_REQ.i32RecommendChallengeUnique = (int)type;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_RECOMMEND_CHALLENGE_CLEAR_REQ, gS_RECOMMEND_CHALLENGE_CLEAR_REQ);
	}
}
