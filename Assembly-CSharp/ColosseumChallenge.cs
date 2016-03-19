using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class ColosseumChallenge : Form
{
	public class ChallengeControl
	{
		public Button m_ChallengeButton;

		public DrawTexture m_LeagerFaceImg;

		public DrawTexture m_ClearImage;

		public DrawTexture m_DisableMark;

		public DrawTexture m_DisableBG;

		public Label m_Name;

		public ChallengeControl()
		{
			this.m_ChallengeButton = null;
			this.m_LeagerFaceImg = null;
			this.m_ClearImage = null;
			this.m_DisableMark = null;
			this.m_DisableBG = null;
			this.m_Name = null;
		}
	}

	private DrawTexture m_BackImage;

	private Label m_ExplainText;

	private Button m_PrevButton;

	private Button m_NextButton;

	private ColosseumChallenge.ChallengeControl[] m_ChallengeControl;

	private static int m_CurrentIndex;

	private Dictionary<int, ECO> m_dicEcoGroupInfo = new Dictionary<int, ECO>();

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Colosseum/DLG_Colosseum_Challenge", G_ID.COLOSSEUM_CHALLENGE_DLG, false);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
		this.closeButton.SetButtonTextureKey("Win_B_Close01");
	}

	public override void SetComponent()
	{
		this.m_BackImage = (base.GetControl("DrawTexture_BGIMG01") as DrawTexture);
		this.m_ExplainText = (base.GetControl("Label_PageTitleLabel01") as Label);
		this.m_PrevButton = (base.GetControl("Button_PrePageBtn01") as Button);
		this.m_PrevButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickPrev));
		this.m_NextButton = (base.GetControl("Button_NextPageBtn01") as Button);
		this.m_NextButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickNext));
		this.m_ChallengeControl = new ColosseumChallenge.ChallengeControl[8];
		for (int i = 0; i < 8; i++)
		{
			this.m_ChallengeControl[i] = new ColosseumChallenge.ChallengeControl();
			string name = string.Empty;
			string str = (i + 1).ToString();
			name = "DrawTexture_LeaderFace0" + str;
			this.m_ChallengeControl[i].m_LeagerFaceImg = (base.GetControl(name) as DrawTexture);
			this.m_ChallengeControl[i].m_LeagerFaceImg.EffectAni = true;
			name = "DrawTexture_ClearMark0" + str;
			this.m_ChallengeControl[i].m_ClearImage = (base.GetControl(name) as DrawTexture);
			this.m_ChallengeControl[i].m_ClearImage.Visible = false;
			name = "DrawTexture_DisableMark0" + str;
			this.m_ChallengeControl[i].m_DisableMark = (base.GetControl(name) as DrawTexture);
			name = "DrawTexture_DisableBG0" + str;
			this.m_ChallengeControl[i].m_DisableBG = (base.GetControl(name) as DrawTexture);
			name = "Button_ChallengeBtn0" + str;
			this.m_ChallengeControl[i].m_ChallengeButton = (base.GetControl(name) as Button);
			this.m_ChallengeControl[i].m_ChallengeButton.EffectAni = false;
			this.m_ChallengeControl[i].m_ChallengeButton.AddValueChangedDelegate(new EZValueChangedDelegate(this.ClickOK));
			this.m_ChallengeControl[i].m_ChallengeButton.Data = i;
			name = "Label_Name0" + str;
			this.m_ChallengeControl[i].m_Name = (base.GetControl(name) as Label);
		}
		base.SetLayerZ(1, -0.2f);
		this.m_PrevButton.SetLocationZ(this.m_PrevButton.GetLocation().z - 0.5f);
		base.SetScreenCenter();
		this.ShowChallengeList();
		ColosseumChallenge.m_CurrentIndex = 0;
	}

	public override void Show()
	{
		base.Show();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void OnClose()
	{
		base.OnClose();
		ColosseumChallenge.m_CurrentIndex = 0;
		GS_COLOSSEUM_START_REQ gS_COLOSSEUM_START_REQ = new GS_COLOSSEUM_START_REQ();
		gS_COLOSSEUM_START_REQ.byMode = 3;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLOSSEUM_START_REQ, gS_COLOSSEUM_START_REQ);
	}

	private void ClickPrev(IUIObject obj)
	{
		ColosseumChallenge.m_CurrentIndex--;
		if (0 > ColosseumChallenge.m_CurrentIndex)
		{
			ColosseumChallenge.m_CurrentIndex = 0;
			return;
		}
		this.ShowChallengeList();
	}

	private void ClickNext(IUIObject obj)
	{
		ColosseumChallenge.m_CurrentIndex++;
		if (ColosseumChallenge.m_CurrentIndex >= NrTSingleton<NkAdventureManager>.Instance.TotalCount())
		{
			ColosseumChallenge.m_CurrentIndex = NrTSingleton<NkAdventureManager>.Instance.TotalCount() - 1;
			return;
		}
		this.ShowChallengeList();
	}

	private void ShowChallengeList()
	{
		if (ColosseumChallenge.m_CurrentIndex == 0)
		{
			this.m_PrevButton.Visible = false;
		}
		else
		{
			this.m_PrevButton.Visible = true;
		}
		int totalCount = COLOSSEUM_CHALLENGE_DATA.GetTotalCount();
		int num = totalCount / 8;
		if (num - 1 == ColosseumChallenge.m_CurrentIndex)
		{
			this.m_NextButton.Visible = false;
		}
		else
		{
			this.m_NextButton.Visible = true;
		}
		switch (ColosseumChallenge.m_CurrentIndex)
		{
		case 0:
		{
			this.m_ExplainText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1407"));
			string str = string.Format("UI/pvp/{0}{1}", "practice_01", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem = Holder.TryGetOrCreateBundle(str + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem.SetItemType(ItemType.USER_ASSETB);
			wWWItem.SetCallback(new PostProcPerItem(this.SetBackImage), "practice_01");
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem, DownGroup.RUNTIME, true);
			break;
		}
		case 1:
		{
			this.m_ExplainText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1408"));
			string str2 = string.Format("UI/pvp/{0}{1}", "practice_02", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem2 = Holder.TryGetOrCreateBundle(str2 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem2.SetItemType(ItemType.USER_ASSETB);
			wWWItem2.SetCallback(new PostProcPerItem(this.SetBackImage), "practice_02");
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem2, DownGroup.RUNTIME, true);
			break;
		}
		case 2:
		{
			this.m_ExplainText.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1409"));
			string str3 = string.Format("UI/pvp/{0}{1}", "practice_03", NrTSingleton<UIDataManager>.Instance.AddFilePath);
			WWWItem wWWItem3 = Holder.TryGetOrCreateBundle(str3 + Option.extAsset, NkBundleCallBack.UIBundleStackName);
			wWWItem3.SetItemType(ItemType.USER_ASSETB);
			wWWItem3.SetCallback(new PostProcPerItem(this.SetBackImage), "practice_03");
			TsImmortal.bundleService.RequestDownloadCoroutine(wWWItem3, DownGroup.RUNTIME, true);
			break;
		}
		}
		this.m_dicEcoGroupInfo.Clear();
		int num2 = ColosseumChallenge.m_CurrentIndex + 1;
		int num3 = 8 * num2;
		int num4 = num3 - 8;
		for (int i = num4; i < num3; i++)
		{
			BASE_COLOSSEUM_CHALLENGE_DATA colosseumChallengeData = COLOSSEUM_CHALLENGE_DATA.GetColosseumChallengeData(i);
			if (colosseumChallengeData != null)
			{
				int num5 = i % 8;
				this.SetEcoHeroinfo(num5, colosseumChallengeData.m_i32EcoIndex, i);
				this.m_ChallengeControl[num5].m_Name.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(colosseumChallengeData.m_i32InterfaceKey.ToString()));
			}
		}
		NrMyCharInfo myCharInfo = NrTSingleton<NkCharManager>.Instance.GetMyCharInfo();
		long charSubData = myCharInfo.GetCharSubData(eCHAR_SUBDATA.CHAR_SUBDATA_COLOSSEUM_CHALLENGE);
		int num6;
		if (charSubData < 8L)
		{
			num6 = 0;
		}
		else if (charSubData < 16L)
		{
			num6 = 1;
		}
		else
		{
			num6 = 2;
		}
		int num7 = (int)charSubData % 8;
		if (num7 < 0)
		{
			return;
		}
		if (num6 == ColosseumChallenge.m_CurrentIndex)
		{
			this.m_ChallengeControl[num7].m_DisableMark.Visible = false;
			this.m_ChallengeControl[num7].m_DisableBG.Visible = false;
		}
	}

	public void SetEcoHeroinfo(int index, int groupunique, int nStepIndex)
	{
		if (index < 0)
		{
			return;
		}
		ECO eco = NrTSingleton<NrBaseTableManager>.Instance.GetEco(groupunique.ToString());
		if (eco != null)
		{
			this.m_ChallengeControl[index].m_LeagerFaceImg.SetTexture(eCharImageType.SMALL, NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindByCode(eco.szCharCode[0]), -1);
			if (NrTSingleton<NkCharManager>.Instance.GetMyCharInfo().IsColosseumChallengeClear(nStepIndex))
			{
				this.m_ChallengeControl[index].m_ClearImage.Visible = true;
				this.m_ChallengeControl[index].m_DisableMark.Visible = false;
				this.m_ChallengeControl[index].m_DisableBG.Visible = false;
			}
			else
			{
				this.m_ChallengeControl[index].m_ClearImage.Visible = false;
				this.m_ChallengeControl[index].m_DisableMark.Visible = true;
				this.m_ChallengeControl[index].m_DisableBG.Visible = true;
			}
			this.m_dicEcoGroupInfo.Add(index, eco);
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

	private void ClickOK(IUIObject obj)
	{
		if (obj == null)
		{
			return;
		}
		int num = (int)obj.Data;
		int episode = ColosseumChallenge.m_CurrentIndex * 8 + num;
		ColosseumChallengeCheck colosseumChallengeCheck = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_CHALLENGE_CHECK_DLG) as ColosseumChallengeCheck;
		if (colosseumChallengeCheck != null)
		{
			colosseumChallengeCheck.SetEpisode(episode);
		}
	}
}
