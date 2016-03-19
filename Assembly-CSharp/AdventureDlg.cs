using NPatch;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class AdventureDlg : Form
{
	public class AdventureControl
	{
		public DrawTexture m_MonsterImage;

		public Label m_MonsterLevel;

		public Button m_MonsterButton;

		public DrawTexture m_NpcImage;

		public Button m_NpcButton;

		public Box m_QuestMark;

		public DrawTexture m_ClearImage;

		public DrawTexture m_DisableMark;

		public DrawTexture m_DisableBG;

		public AdventureControl()
		{
			this.m_MonsterImage = null;
			this.m_MonsterLevel = null;
			this.m_MonsterButton = null;
			this.m_NpcImage = null;
			this.m_NpcButton = null;
			this.m_ClearImage = null;
			this.m_DisableMark = null;
			this.m_DisableBG = null;
		}
	}

	private Label m_Name;

	private DrawTexture m_BackImage;

	private AdventureDlg.AdventureControl[] m_AdventureControl;

	private Button m_PrevButton;

	private Button m_NextButton;

	private DrawTexture m_NpcFace;

	private Label m_BaloonText;

	private Button m_SubStory;

	private Box m_SubStoryNum;

	private static bool m_PageSetting;

	private static Adventure m_CurrentAdventure;

	private CQuest m_CurrentQuest;

	private static int m_CurrentIndex;

	private int m_MaxControlNum;

	private float oldZ;

	private int m_nWinID;

	public void ShowUIGuide(string param1, string param2, int winID)
	{
		int num;
		if (param1 == "10102_010")
		{
			num = 1;
		}
		else
		{
			num = 2;
		}
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = false;
		}
		this.oldZ = base.GetLocation().z;
		this.m_nWinID = winID;
		base.SetLocation(base.GetLocationX(), base.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() + 1f);
		this.m_AdventureControl[num].m_NpcButton.SetLocationZ(-2f);
		this.m_AdventureControl[num].m_NpcImage.SetLocationZ(-2.5f);
		this.m_AdventureControl[num].m_QuestMark.SetLocationZ(-3f);
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide == null)
		{
			return;
		}
		Vector2 x = new Vector2(base.GetLocationX() + this.m_AdventureControl[num].m_NpcImage.GetLocationX() + 80f, base.GetLocationY() + this.m_AdventureControl[num].m_NpcImage.GetLocationY() + this.m_AdventureControl[num].m_NpcImage.GetSize().y / 2f);
		Vector2 x2 = new Vector2(base.GetLocationX() + this.m_AdventureControl[num].m_NpcImage.GetLocationX() + this.m_AdventureControl[num].m_NpcImage.GetSize().x + 30f, base.GetLocationY() + this.m_AdventureControl[num].m_NpcImage.GetLocationY() + 55f);
		uI_UIGuide.Move(x, x2);
	}

	public override void InitializeComponent()
	{
		if (!AdventureDlg.m_PageSetting)
		{
			AdventureDlg.m_CurrentAdventure = NrTSingleton<NkAdventureManager>.Instance.GetCurrentAdventure(ref AdventureDlg.m_CurrentIndex);
		}
		else
		{
			AdventureDlg.m_CurrentAdventure = NrTSingleton<NkAdventureManager>.Instance.GetAdventureFromIndex(AdventureDlg.m_CurrentIndex);
		}
		if (AdventureDlg.m_CurrentAdventure == null)
		{
			AdventureDlg.m_CurrentIndex = 0;
			AdventureDlg.m_CurrentAdventure = NrTSingleton<NkAdventureManager>.Instance.GetAdventureFromIndex(AdventureDlg.m_CurrentIndex);
		}
		string str = "DLG_Adventure_" + AdventureDlg.m_CurrentAdventure.GetAdventureUnique().ToString();
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "QuestList/" + str, G_ID.ADVENTURE_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
		this.closeButton.SetButtonTextureKey("Win_B_Close01");
		AdventureDlg.m_PageSetting = false;
	}

	public override void SetComponent()
	{
		this.m_Name = (base.GetControl("Label_PageTitleLabel01") as Label);
		this.m_BackImage = (base.GetControl("DrawTexture_BGIMG01") as DrawTexture);
		this.m_MaxControlNum = AdventureDlg.m_CurrentAdventure.GetAdventureInfoCount();
		this.m_AdventureControl = new AdventureDlg.AdventureControl[this.m_MaxControlNum];
		for (int i = 0; i < this.m_MaxControlNum; i++)
		{
			this.m_AdventureControl[i] = new AdventureDlg.AdventureControl();
			string name = string.Empty;
			string str = (i + 1).ToString();
			name = "DrawTexture_MonFace0" + str;
			this.m_AdventureControl[i].m_MonsterImage = (base.GetControl(name) as DrawTexture);
			this.m_AdventureControl[i].m_MonsterImage.EffectAni = true;
			name = "Label_Lv0" + str;
			this.m_AdventureControl[i].m_MonsterLevel = (base.GetControl(name) as Label);
			name = "Button_EpisodeBtn0" + str;
			this.m_AdventureControl[i].m_MonsterButton = (base.GetControl(name) as Button);
			this.m_AdventureControl[i].m_MonsterButton.EffectAni = false;
			this.m_AdventureControl[i].m_MonsterButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickMonster));
			this.m_AdventureControl[i].m_MonsterButton.Data = i;
			name = "DrawTexture_NPCFace0" + str;
			this.m_AdventureControl[i].m_NpcImage = (base.GetControl(name) as DrawTexture);
			this.m_AdventureControl[i].m_NpcImage.EffectAni = true;
			this.m_AdventureControl[i].m_NpcImage.Visible = false;
			name = "Button_NPCFace0" + str;
			this.m_AdventureControl[i].m_NpcButton = (base.GetControl(name) as Button);
			this.m_AdventureControl[i].m_NpcButton.Visible = false;
			this.m_AdventureControl[i].m_NpcButton.EffectAni = false;
			this.m_AdventureControl[i].m_NpcButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNpc));
			this.m_AdventureControl[i].m_NpcButton.Data = i;
			name = "Box_QuestMark0" + str;
			this.m_AdventureControl[i].m_QuestMark = (base.GetControl(name) as Box);
			this.m_AdventureControl[i].m_QuestMark.Visible = false;
			name = "DrawTexture_ClearMark0" + str;
			this.m_AdventureControl[i].m_ClearImage = (base.GetControl(name) as DrawTexture);
			this.m_AdventureControl[i].m_ClearImage.Visible = false;
			name = "DrawTexture_DisableMark0" + str;
			this.m_AdventureControl[i].m_DisableMark = (base.GetControl(name) as DrawTexture);
			name = "DrawTexture_DisableBG0" + str;
			this.m_AdventureControl[i].m_DisableBG = (base.GetControl(name) as DrawTexture);
		}
		this.m_PrevButton = (base.GetControl("Button_PrePageBtn01") as Button);
		this.m_PrevButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_NextButton = (base.GetControl("Button_NextPageBtn01") as Button);
		this.m_NextButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_NpcFace = (base.GetControl("NPC_Face") as DrawTexture);
		this.m_NpcFace.SetTexture(eCharImageType.LARGE, 242, -1);
		this.m_BaloonText = (base.GetControl("BaloonText") as Label);
		this.m_SubStory = (base.GetControl("Button_SubStory") as Button);
		if (null != this.m_SubStory)
		{
			this.m_SubStory.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickSubStory));
		}
		this.m_SubStoryNum = (base.GetControl("Box_StoryNum") as Box);
		if (null != this.m_SubStoryNum)
		{
			int subQuestCount = NrTSingleton<NkQuestManager>.Instance.GetSubQuestCount();
			if (0 < subQuestCount)
			{
				this.m_SubStoryNum.Text = subQuestCount.ToString();
			}
			else
			{
				this.m_SubStoryNum.Visible = false;
			}
		}
		base.SetLayerZ(1, -0.2f);
		this.m_PrevButton.SetLocationZ(this.m_PrevButton.GetLocation().z - 0.5f);
		base.SetShowLayer(1, false);
		base.SetScreenCenter();
	}

	private void ClickSubStory(IUIObject obj)
	{
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(0, 0))
		{
			return;
		}
		NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.QUEST_SUBSTORY_DLG);
		this.Close();
	}

	public void DrawAdventure()
	{
		if (AdventureDlg.m_CurrentIndex == 0)
		{
			this.m_PrevButton.Visible = false;
		}
		if (Launcher.Instance.LocalPatchLevel != Launcher.Instance.PatchLevelMax)
		{
			if (0 < AdventureDlg.m_CurrentIndex)
			{
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				if (msgBoxUI == null)
				{
					return;
				}
				msgBoxUI.SetMsg(new YesDelegate(this.OnOKDownStart), null, NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2458"), NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("213"), eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("10"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("11"));
			}
			this.m_NextButton.Visible = false;
		}
		if (NrTSingleton<NkAdventureManager>.Instance.TotalCount() - 1 == AdventureDlg.m_CurrentIndex)
		{
			this.m_NextButton.Visible = false;
		}
		else
		{
			int index = AdventureDlg.m_CurrentIndex + 1;
			Adventure adventureFromIndex = NrTSingleton<NkAdventureManager>.Instance.GetAdventureFromIndex(index);
			if (adventureFromIndex != null && adventureFromIndex.GetAdventureUnique() == NrTSingleton<ContentsLimitManager>.Instance.GetLimitAdventure())
			{
				this.m_NextButton.Visible = false;
			}
		}
		this.ShowBackImage(AdventureDlg.m_CurrentAdventure);
		this.ShowNpcImage(AdventureDlg.m_CurrentAdventure);
	}

	public void OnOKDownStart(object a_oObject)
	{
		Launcher.Instance.SavePatchLevel(Launcher.Instance.PatchLevelMax);
		NrTSingleton<NrMainSystem>.Instance.ReLogin(true);
	}

	private void ShowBackImage(Adventure adventure)
	{
		if (adventure != null)
		{
			this.m_Name.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(adventure.GetAdventureName());
			string backImage = adventure.GetBackImage();
			Texture2D texture = NrTSingleton<UIImageBundleManager>.Instance.GetTexture(backImage);
			if (null != texture)
			{
				this.m_BackImage.SetTexture(texture);
			}
			else
			{
				string str = string.Format("UI/Adventure/{0}{1}", backImage, NrTSingleton<UIDataManager>.Instance.AddFilePath);
				WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
				wWWItem.SetItemType(ItemType.USER_ASSETB);
				wWWItem.SetCallback(new PostProcPerItem(this.SetBackImage), backImage);
				TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			}
		}
	}

	private void ShowNewAdventure()
	{
		AdventureDlg.m_PageSetting = true;
		this.Close();
		AdventureDlg adventureDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.ADVENTURE_DLG) as AdventureDlg;
		if (adventureDlg != null)
		{
			adventureDlg.DrawAdventure();
			AdventureDlg.m_PageSetting = false;
		}
	}

	private void ClickPrev(IUIObject obj)
	{
		AdventureDlg.m_CurrentIndex--;
		if (0 > AdventureDlg.m_CurrentIndex)
		{
			AdventureDlg.m_CurrentIndex = 0;
			return;
		}
		this.ShowNewAdventure();
	}

	private void ClickNext(IUIObject obj)
	{
		AdventureDlg.m_CurrentIndex++;
		if (AdventureDlg.m_CurrentIndex >= NrTSingleton<NkAdventureManager>.Instance.TotalCount())
		{
			AdventureDlg.m_CurrentIndex = NrTSingleton<NkAdventureManager>.Instance.TotalCount() - 1;
			return;
		}
		Adventure adventureFromIndex = NrTSingleton<NkAdventureManager>.Instance.GetAdventureFromIndex(AdventureDlg.m_CurrentIndex);
		if (adventureFromIndex == null)
		{
			return;
		}
		if (adventureFromIndex.GetAdventureUnique() == NrTSingleton<ContentsLimitManager>.Instance.GetLimitAdventure())
		{
			AdventureDlg.m_CurrentIndex--;
			return;
		}
		this.ShowNewAdventure();
	}

	private void ClickMonster(IUIObject obj)
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		if ((double)this.oldZ != 0.0)
		{
			base.SetLocation(base.GetLocationX(), base.GetLocationY(), this.oldZ);
		}
		if (obj == null)
		{
			return;
		}
		int index = (int)obj.Data;
		Adventure.AdventureInfo adventureInfo = AdventureDlg.m_CurrentAdventure.GetAdventureInfo(index);
		if (adventureInfo == null)
		{
			return;
		}
		EpisodeCheckDlg episodeCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EPISODECHECK_DLG) as EpisodeCheckDlg;
		if (episodeCheckDlg != null)
		{
			episodeCheckDlg.SetEpisode(adventureInfo, null);
		}
	}

	private void ClickNpc(IUIObject obj)
	{
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		if ((double)this.oldZ != 0.0)
		{
			base.SetLocation(base.GetLocationX(), base.GetLocationY(), this.oldZ);
		}
		if (this.m_CurrentQuest == null)
		{
			return;
		}
		if (obj == null)
		{
			return;
		}
		int index = (int)obj.Data;
		Adventure.AdventureInfo adventureInfo = AdventureDlg.m_CurrentAdventure.GetAdventureInfo(index);
		if (adventureInfo == null)
		{
			return;
		}
		EpisodeCheckDlg episodeCheckDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.EPISODECHECK_DLG) as EpisodeCheckDlg;
		if (episodeCheckDlg != null)
		{
			episodeCheckDlg.SetEpisode(adventureInfo, this.m_CurrentQuest);
		}
	}

	private void ShowMonsterImage(Adventure adventure)
	{
	}

	private void ShowNpcImage(Adventure adventure)
	{
		if (adventure == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.m_MaxControlNum; i++)
		{
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(adventure.GetMonsterKind(i));
			if (charKindInfoFromCode != null)
			{
				this.m_AdventureControl[i].m_MonsterImage.SetTexture(eCharImageType.SMALL, charKindInfoFromCode.GetCharKind(), -1);
				this.m_AdventureControl[i].m_ClearImage.Visible = false;
				if (NrTSingleton<NkQuestManager>.Instance.QuestGroupClearCheck(adventure.GetQuestGroupUnique(i)) == QUEST_CONST.E_QUEST_GROUP_STATE.E_QUEST_GROUP_STATE_NONE)
				{
					this.m_AdventureControl[i].m_ClearImage.Visible = true;
					this.m_AdventureControl[i].m_DisableMark.Visible = false;
					this.m_AdventureControl[i].m_DisableBG.Visible = false;
					this.m_AdventureControl[i].m_MonsterLevel.Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152"), charKindInfoFromCode.GetCHARKIND_MONSTERINFO().MINLEVEL.ToString());
				}
				else if (!flag)
				{
					CQuestGroup questGroupByGroupUnique = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(adventure.GetQuestGroupUnique(i));
					if (questGroupByGroupUnique != null)
					{
						CQuest cQuest = questGroupByGroupUnique.FindCurrentQuest();
						if (cQuest != null)
						{
							QUEST_CONST.eQUESTSTATE questState = NrTSingleton<NkQuestManager>.Instance.GetQuestState(cQuest.GetQuestUnique());
							if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ACCEPTABLE)
							{
								this.m_CurrentQuest = cQuest;
								this.m_AdventureControl[i].m_NpcButton.Visible = true;
								this.m_AdventureControl[i].m_NpcImage.Visible = true;
								this.m_AdventureControl[i].m_QuestMark.Visible = true;
								this.m_AdventureControl[i].m_NpcImage.SetTexture(eCharImageType.SMALL, cQuest.GetQuestCommon().i32QuestCharKind, -1);
								this.m_AdventureControl[i].m_MonsterLevel.Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152"), charKindInfoFromCode.GetCHARKIND_MONSTERINFO().MINLEVEL.ToString());
							}
							else if (questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_ONGOING || questState == QUEST_CONST.eQUESTSTATE.QUESTSTATE_COMPLETE)
							{
								this.m_AdventureControl[i].m_DisableMark.Visible = false;
								this.m_AdventureControl[i].m_DisableBG.Visible = false;
								this.m_AdventureControl[i].m_MonsterLevel.Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1002"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152"), charKindInfoFromCode.GetCHARKIND_MONSTERINFO().MINLEVEL.ToString());
							}
							else
							{
								this.m_AdventureControl[i].m_MonsterLevel.Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1102"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152"), charKindInfoFromCode.GetCHARKIND_MONSTERINFO().MINLEVEL.ToString());
							}
							flag = true;
						}
					}
				}
				else
				{
					this.m_AdventureControl[i].m_MonsterLevel.Text = NrTSingleton<UIDataManager>.Instance.GetString(NrTSingleton<CTextParser>.Instance.GetTextColor("1102"), NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("152"), charKindInfoFromCode.GetCHARKIND_MONSTERINFO().MINLEVEL.ToString());
				}
			}
		}
		CQuestGroup questGroupByGroupUnique2 = NrTSingleton<NkQuestManager>.Instance.GetQuestGroupByGroupUnique(adventure.GetQuestGroupUnique(0));
		if (questGroupByGroupUnique2 == null)
		{
			return;
		}
		CQuest firstQuest = questGroupByGroupUnique2.GetFirstQuest();
		if (firstQuest == null)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo == null)
		{
			return;
		}
		if ((int)firstQuest.GetQuestLevel(0) > kMyCharInfo.GetLevel())
		{
			base.SetShowLayer(1, true);
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("622"),
				"count",
				firstQuest.GetQuestLevel(0)
			});
			this.m_BaloonText.Text = empty;
			this.m_NextButton.controlIsEnabled = false;
		}
		else
		{
			base.SetShowLayer(1, false);
			this.m_NextButton.controlIsEnabled = true;
		}
	}

	private void SetBackImage(WWWItem _item, object _param)
	{
		if (_item.GetSafeBundle() != null && null != _item.GetSafeBundle().mainAsset)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.m_BackImage.SetTexture(texture2D);
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
				}
			}
		}
	}

	public override void OnClose()
	{
		if (null != base.InteractivePanel)
		{
			base.InteractivePanel.depthChangeable = true;
		}
		UI_UIGuide uI_UIGuide = NrTSingleton<FormsManager>.Instance.GetForm((G_ID)this.m_nWinID) as UI_UIGuide;
		if (uI_UIGuide != null)
		{
			uI_UIGuide.CloseUI = true;
		}
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "ADVENTURE", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
