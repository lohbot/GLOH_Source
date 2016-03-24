using GAME;
using Ndoors.Framework.Stage;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class Main_UI_LevelUpAlarmSoldier : Form
{
	private DrawTexture m_DtSoldierImg;

	private Label m_LblSoldierName;

	private Label m_ClbTalkMessage;

	private Button m_BtnExit;

	private float m_fRenderTime;

	private int m_nCharKind;

	private short m_iLevel;

	private int m_nGrade;

	private int m_nCostumeUnique;

	private int m_nEventType;

	private int m_nEventTitleText;

	private int m_nEventExplainText;

	private string m_strEventExplainText = string.Empty;

	private bool m_bSlideHide;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "DLG_SoldierTalk", G_ID.MAIN_UI_LEVELUP_ALARM_SOLDIER, false);
		base.DonotDepthChange(1000f);
	}

	public override void SetComponent()
	{
		base.SetComponent();
		this.m_DtSoldierImg = (base.GetControl("SoldierTalk_portrait") as DrawTexture);
		this.m_LblSoldierName = (base.GetControl("SoldierTalk_Name") as Label);
		this.m_ClbTalkMessage = (base.GetControl("SoldierTalk_Message") as Label);
		this.m_BtnExit = (base.GetControl("SoldierTalk_btn") as Button);
		this.m_BtnExit.Visible = false;
		base.Draggable = false;
		base.SetLocation(base.GetLocation().x, GUICamera.height - base.GetSize().y - 80f);
		base.SetShowLayer(1, false);
	}

	public override void Update()
	{
		if (this == null || !base.Visible)
		{
			return;
		}
		base.Update();
		if (Scene.CurScene == Scene.Type.BATTLE)
		{
			base.CloseNow();
			return;
		}
		this.FadeInOut();
	}

	public override void InitData()
	{
		base.InitData();
		this.m_fRenderTime = 0f;
		this.m_bSlideHide = false;
	}

	private void FadeInOut()
	{
		float num = Time.deltaTime * 250f;
		if (this.m_bSlideHide && this.m_fRenderTime < (float)Main_UI_LevelUpAlarmSoldier.GetTick())
		{
			num *= 3f * (base.GetSize().x - base.GetLocation().x) / base.GetSize().x;
			base.SetLocation(base.GetLocation().x - num, (float)((int)(GUICamera.height - base.GetSize().y - 80f)));
			if (base.GetLocation().x + base.GetSize().x <= 1f)
			{
				this.Hide();
			}
		}
		if (!this.m_bSlideHide)
		{
			if (base.GetLocation().x <= -1f)
			{
				num *= 3f * (-base.GetLocation().x / base.GetSize().x);
				base.SetLocation(base.GetLocation().x + num, (float)((int)(GUICamera.height - base.GetSize().y - 80f)));
			}
			else
			{
				this.m_bSlideHide = true;
				this.m_fRenderTime = (float)(Main_UI_LevelUpAlarmSoldier.GetTick() + 1000L);
			}
		}
	}

	public void SetInfo(NkSoldierInfo solInfo)
	{
		this.m_nCharKind = 0;
		this.m_iLevel = 0;
		this.m_nGrade = 0;
		this.m_nCostumeUnique = 0;
		if (solInfo == null)
		{
			return;
		}
		this.m_nCharKind = solInfo.GetCharKind();
		this.m_iLevel = solInfo.GetLevel();
		this.m_nGrade = (int)solInfo.GetGrade();
		this.m_nCostumeUnique = (int)solInfo.GetSolSubData(eSOL_SUBDATA.SOL_SUBDATA_COSTUME);
		this.SetImage();
		this.SetMessage();
	}

	private void SetImage()
	{
		this.m_DtSoldierImg.SetTexture(eCharImageType.LARGE, this.m_nCharKind, this.m_nGrade, NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumePortraitPath(this.m_nCostumeUnique));
	}

	private void SetMessage()
	{
		string text = string.Empty;
		string empty = string.Empty;
		string text2 = string.Empty;
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromhelper("Help_GLevel_Default");
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(this.m_nCharKind);
		if (charKindInfo == null)
		{
			return;
		}
		text2 = charKindInfo.GetName();
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"targetname",
			text2,
			"count",
			this.m_iLevel.ToString()
		});
		this.m_LblSoldierName.Text = text2;
		this.m_ClbTalkMessage.SetText(empty);
		base.SetLocation(-base.GetSize().x, (float)((int)(GUICamera.height - base.GetSize().y - 80f)));
	}

	public void SetEventInfo(int nEventType, int nEventTitleText, int nEventExplainText)
	{
		this.m_nEventType = nEventType;
		this.m_nEventTitleText = nEventTitleText;
		this.m_nEventExplainText = nEventExplainText;
		this.SetEventImage();
		this.SetEventMessage(false);
	}

	public void SetEventInfo(int nEventType, int nEventTitleText, string strEventExplainText)
	{
		this.m_nEventType = nEventType;
		this.m_nEventTitleText = nEventTitleText;
		this.m_strEventExplainText = strEventExplainText;
		this.SetEventImage();
		this.SetEventMessage(true);
	}

	private void SetEventImage()
	{
		BUNNING_EVENT_INFO value = NrTSingleton<NrTable_BurnningEvent_Manager>.Instance.GetValue((eBUNNING_EVENT)this.m_nEventType);
		string bundlepath = string.Format("{0}", "UI/etc/" + value.m_strSolTalkImg);
		NrTSingleton<UIImageBundleManager>.Instance.RequestBundleImage(bundlepath, new PostProcPerItem(this.SetImage));
	}

	private void SetImage(WWWItem _item, object _param)
	{
		if (this == null)
		{
			return;
		}
		if (_item.isCanceled)
		{
			return;
		}
		if (_item.GetSafeBundle() != null)
		{
			Texture2D texture2D = _item.GetSafeBundle().mainAsset as Texture2D;
			if (null != texture2D)
			{
				this.m_DtSoldierImg.SetTexture(texture2D);
				string imageKey = string.Empty;
				if (_param is string)
				{
					imageKey = (string)_param;
					NrTSingleton<UIImageBundleManager>.Instance.AddTexture(imageKey, texture2D);
					return;
				}
			}
		}
	}

	private void SetEventMessage(bool isStr = false)
	{
		this.m_LblSoldierName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(this.m_nEventTitleText.ToString()));
		int nEventExplainText = this.m_nEventExplainText;
		if (isStr)
		{
			this.m_ClbTalkMessage.SetText(this.m_strEventExplainText);
		}
		else
		{
			this.m_ClbTalkMessage.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(nEventExplainText.ToString()));
		}
		base.SetLocation(-base.GetSize().x, (float)((int)(GUICamera.height - base.GetSize().y - 80f)));
	}

	public static long GetTick()
	{
		return (long)Environment.TickCount;
	}
}
