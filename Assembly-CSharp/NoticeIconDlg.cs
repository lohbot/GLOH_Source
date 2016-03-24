using GAME;
using Ndoors.Framework.Stage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityForms;

public class NoticeIconDlg : Form
{
	private const float BLINK_TIME = 0.5f;

	private const float VISIBLE_BLINK_ON = 1f;

	private const float VISIBLE_BLINK_OFF = 0f;

	public static int[] tempicontype = new int[]
	{
		-1,
		-1,
		-1,
		-1,
		-1
	};

	private Button[] m_BtnIcon;

	private float[] m_BlinkValue;

	private bool[] m_bIconStatus;

	private float mTime;

	private List<ICON_TYPE> mList = new List<ICON_TYPE>();

	public int COUNT_MAX
	{
		get
		{
			return 5;
		}
	}

	public static void SetIcon(ICON_TYPE type, bool bOn)
	{
		if (Scene.IsCurScene(Scene.Type.WORLD))
		{
			NoticeIconDlg noticeIconDlg = (NoticeIconDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_ICON);
			if (noticeIconDlg != null)
			{
				if (type == ICON_TYPE.ATTEND_REWARD)
				{
					noticeIconDlg.SetIconOnOff(type, false);
					noticeIconDlg.SetIconStatus(type, false);
				}
				else
				{
					noticeIconDlg.SetIconOnOff(type, bOn);
					noticeIconDlg.SetIconStatus(type, bOn);
				}
				if (!NrTSingleton<NkQuestManager>.Instance.IsCompletedFirstQuest())
				{
					noticeIconDlg.Hide();
				}
			}
		}
		else if (bOn)
		{
			NoticeIconDlg.AddTempNotice(type);
		}
	}

	public static void AddTempNotice(ICON_TYPE type)
	{
		if (type < ICON_TYPE.WHISPER)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (NoticeIconDlg.tempicontype[i] < 0)
			{
				NoticeIconDlg.tempicontype[i] = (int)type;
				break;
			}
		}
	}

	public void InitTempNotice()
	{
		for (int i = 0; i < 5; i++)
		{
			NoticeIconDlg.tempicontype[i] = -1;
		}
	}

	public bool IsTempNotice()
	{
		for (int i = 0; i < 5; i++)
		{
			if (NoticeIconDlg.tempicontype[i] >= 0)
			{
				return true;
			}
		}
		return false;
	}

	public void ShowTempNotice()
	{
		if (!this.IsTempNotice())
		{
			return;
		}
		NoticeIconDlg noticeIconDlg = (NoticeIconDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_ICON);
		for (int i = 0; i < 5; i++)
		{
			if (NoticeIconDlg.tempicontype[i] >= 0)
			{
				if (noticeIconDlg != null)
				{
					noticeIconDlg.SetIconOnOff((ICON_TYPE)NoticeIconDlg.tempicontype[i], true);
					noticeIconDlg.SetIconStatus((ICON_TYPE)NoticeIconDlg.tempicontype[i], true);
					if (!NrTSingleton<NkQuestManager>.Instance.IsCompletedFirstQuest())
					{
						noticeIconDlg.Hide();
					}
				}
			}
		}
		this.InitTempNotice();
	}

	public static bool IsShowIcon(ICON_TYPE type)
	{
		NoticeIconDlg noticeIconDlg = (NoticeIconDlg)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MAIN_UI_ICON);
		return noticeIconDlg.ShowIcon(type);
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Main/Dlg_MiniIcn", G_ID.MAIN_UI_ICON, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		base.DonotDepthChange(UIPanelManager.UI_DEPTH - 1f);
	}

	public float GetButtonLocationX(ICON_TYPE type)
	{
		if (ICON_TYPE.POST <= type && ICON_TYPE.END > type)
		{
			return this.m_BtnIcon[(int)type].GetLocationX();
		}
		return 0f;
	}

	public override void SetComponent()
	{
		this.m_BtnIcon = new Button[this.COUNT_MAX];
		this.m_BlinkValue = new float[this.COUNT_MAX];
		this.m_bIconStatus = new bool[this.COUNT_MAX];
		this.m_BtnIcon[0] = (base.GetControl("Button_Whisper") as Button);
		this.m_BtnIcon[1] = (base.GetControl("Button_NewMail") as Button);
		this.m_BtnIcon[2] = (base.GetControl("Button_GameGuide") as Button);
		this.m_BtnIcon[3] = (base.GetControl("Button_Dailygift") as Button);
		this.m_BtnIcon[4] = (base.GetControl("Button_MineRecord") as Button);
		for (int i = 0; i < this.COUNT_MAX; i++)
		{
			this.m_BtnIcon[i].AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickIcon));
			this.SetIconOnOff((ICON_TYPE)i, false);
			this.m_bIconStatus[i] = false;
			ICON_TYPE iCON_TYPE = (ICON_TYPE)i;
			string toolTip = string.Empty;
			ICON_TYPE iCON_TYPE2 = iCON_TYPE;
			if (iCON_TYPE2 != ICON_TYPE.POST)
			{
				if (iCON_TYPE2 != ICON_TYPE.GAMEGUIDE)
				{
				}
			}
			else
			{
				toolTip = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("4");
			}
			this.m_BtnIcon[i].ToolTip = toolTip;
			this.m_BtnIcon[i].DeleteSpriteText();
		}
		base.ChangeSceneDestory = false;
	}

	public override void InitData()
	{
		this.UpdateRePosition();
		BubbleGameGuideDlg bubbleGameGuideDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BUBBLEGAMEGUIDE_DLG) as BubbleGameGuideDlg;
		if (bubbleGameGuideDlg != null)
		{
			bubbleGameGuideDlg.UpdatePosition();
		}
		if (Scene.CurScene == Scene.Type.BATTLE || Scene.CurScene == Scene.Type.SOLDIER_BATCH)
		{
			for (int i = 0; i < this.COUNT_MAX; i++)
			{
				this.SetIconOnOff((ICON_TYPE)i, false);
			}
		}
		else
		{
			for (int j = 0; j < this.COUNT_MAX; j++)
			{
				this.SetIconOnOff((ICON_TYPE)j, this.m_bIconStatus[j]);
			}
		}
	}

	public bool UpdateVisibleState()
	{
		if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState())
		{
			this.Hide();
			return false;
		}
		return true;
	}

	public override void Update()
	{
		if (Scene.CurScene != Scene.Type.WORLD)
		{
			return;
		}
		base.Update();
		if (!this.UpdateVisibleState())
		{
			return;
		}
		if (this.mTime < Time.time)
		{
			float num = 0f;
			for (int i = 0; i < this.COUNT_MAX; i++)
			{
				if (0f < this.m_BlinkValue[i])
				{
					if (0f >= num)
					{
						num = ((this.m_BlinkValue[i] < 1f) ? 1f : 0.5f);
					}
					this.SetBlinkValue(i, num);
					if (i == 2)
					{
						NrTSingleton<GameGuideManager>.Instance.SetBubbleGameGuide();
					}
				}
			}
			this.mTime = Time.time + 0.5f;
		}
	}

	public override void ChangedResolution()
	{
		this.UpdateRePosition();
	}

	public void UpdateRePosition()
	{
		if (!Scene.IsCurScene(Scene.Type.WORLD))
		{
			return;
		}
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MENUICON_DLG);
		if (form != null)
		{
			base.SetLocation(form.GetLocationX() - base.GetSizeX() - 5f, GUICamera.height - base.GetSizeY());
		}
		float num = base.GetSizeX();
		foreach (ICON_TYPE current in this.mList)
		{
			int num2 = (int)current;
			if (this.m_BtnIcon[num2].Visible)
			{
				num -= this.m_BtnIcon[num2].GetSize().x;
				this.m_BtnIcon[num2].SetLocation(num, this.m_BtnIcon[num2].GetLocationY());
			}
		}
	}

	private ICON_TYPE GetType(IUIObject obj)
	{
		for (int i = 0; i < this.COUNT_MAX; i++)
		{
			if (this.m_BtnIcon[i] == (Button)obj)
			{
				return (ICON_TYPE)i;
			}
		}
		return ICON_TYPE.END;
	}

	private bool ShowHiteSwitch(G_ID ID)
	{
		if (NrTSingleton<FormsManager>.Instance.IsShow(ID))
		{
			NrTSingleton<FormsManager>.Instance.Hide(ID);
			return false;
		}
		NrTSingleton<FormsManager>.Instance.ShowForm(ID);
		return true;
	}

	public void SetIconStatus(ICON_TYPE iconType, bool OnOff)
	{
		this.m_bIconStatus[(int)iconType] = OnOff;
	}

	public bool ShowIcon(ICON_TYPE iconType)
	{
		return ICON_TYPE.WHISPER <= iconType && iconType < (ICON_TYPE)this.COUNT_MAX && 0.0 < (double)this.m_BlinkValue[(int)iconType];
	}

	public void SetIconOnOff(ICON_TYPE iconType, bool OnOff)
	{
		if (ICON_TYPE.WHISPER <= iconType && iconType < (ICON_TYPE)this.COUNT_MAX)
		{
			this.SetBlinkValue((int)iconType, (!OnOff) ? 0f : 1f);
			this.m_BtnIcon[(int)iconType].Visible = OnOff;
			if (OnOff)
			{
				if (!this.mList.Contains(iconType))
				{
					this.mList.Add(iconType);
				}
			}
			else if (this.mList.Contains(iconType))
			{
				this.mList.Remove(iconType);
			}
			this.UpdateRePosition();
		}
	}

	private void SetBlinkValue(int index, float fValue)
	{
		if (0 <= index && index < this.COUNT_MAX)
		{
			this.m_BlinkValue[index] = fValue;
			float num = fValue;
			if (0f >= num)
			{
				num = 1f;
			}
			this.m_BtnIcon[index].SetAlphaBG(num);
		}
	}

	private void OnClickIcon(IUIObject obj)
	{
		ICON_TYPE type = this.GetType(obj);
		this.SetBlinkValue((int)type, 0f);
		switch (type)
		{
		case ICON_TYPE.WHISPER:
			this.ShowHiteSwitch(G_ID.WHISPER_DLG);
			break;
		case ICON_TYPE.POST:
			if (this.ShowHiteSwitch(G_ID.POST_DLG))
			{
				PostDlg postDlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.POST_DLG) as PostDlg;
				if (postDlg != null)
				{
					postDlg.ChangeTab_RecvList();
				}
			}
			break;
		case ICON_TYPE.GAMEGUIDE:
			this.ShowHiteSwitch(G_ID.GAMEGUIDE_DLG);
			break;
		case ICON_TYPE.MINE_RECORED:
			this.ShowHiteSwitch(G_ID.MINE_RECORD_DLG);
			break;
		}
	}

	private void OnMouseOverIcon(IUIObject obj)
	{
		ICON_TYPE type = this.GetType(obj);
		string text = string.Empty;
		switch (type)
		{
		case ICON_TYPE.WHISPER:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromToolTip("51");
			break;
		case ICON_TYPE.POST:
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("4");
			break;
		}
		Button button = obj as Button;
		if (null != button && string.Empty != text)
		{
			button.ToolTip = text;
		}
	}

	private void OnMouseOutIcon(IUIObject obj)
	{
		Button button = obj as Button;
		if (null != button)
		{
			button.ShowToolTip = false;
		}
	}

	public override void Hide()
	{
		base.Hide();
	}

	public override void Show()
	{
		base.Show();
		this.UpdateRePosition();
	}
}
