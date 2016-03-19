using Ndoors.Framework.Stage;
using System;
using UnityEngine;
using UnityForms;

public class Main_UI_LevelUpAlarmMonarch : Form
{
	private DrawTexture m_DrawTexture_DrawTexture0;

	private DrawTexture m_DrawTexture_DrawTexture1;

	private Button m_Button_Button7;

	private Label m_Label_Label2;

	private Label m_Label_Label3;

	private FlashLabel m_FlashLabel_Label1;

	private Button m_Button_Button12;

	private Box m_Box_Box11;

	private Button m_Button_Button13;

	private int m_nCharKind;

	private byte m_byLevel;

	private int m_nPageNum;

	private int m_nPageMax;

	private bool m_bSlideHide;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Main/dlg_generallevelup", G_ID.MAIN_UI_LEVELUP_ALARM_MONARCH, false);
	}

	public override void SetComponent()
	{
		base.SetComponent();
		this.m_DrawTexture_DrawTexture0 = (base.GetControl("DrawTexture_DrawTexture0") as DrawTexture);
		this.m_DrawTexture_DrawTexture0.AddBoxCollider();
		this.m_DrawTexture_DrawTexture0.SetUseBoxCollider(true);
		this.m_DrawTexture_DrawTexture1 = (base.GetControl("DrawTexture_DrawTexture1") as DrawTexture);
		this.m_DrawTexture_DrawTexture1.AddBoxCollider();
		this.m_DrawTexture_DrawTexture1.SetUseBoxCollider(true);
		this.m_Button_Button7 = (base.GetControl("Button_Button7") as Button);
		Button expr_7C = this.m_Button_Button7;
		expr_7C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_7C.Click, new EZValueChangedDelegate(this.OnButtonClickExit));
		this.m_Label_Label2 = (base.GetControl("Label_Label2") as Label);
		this.m_Label_Label3 = (base.GetControl("Label_Label3") as Label);
		this.m_FlashLabel_Label1 = (base.GetControl("FlashLabel_Lable1") as FlashLabel);
		this.m_FlashLabel_Label1.lineSpacing = 1.4f;
		this.m_Button_Button12 = (base.GetControl("Button_Button12") as Button);
		Button expr_10B = this.m_Button_Button12;
		expr_10B.Click = (EZValueChangedDelegate)Delegate.Combine(expr_10B.Click, new EZValueChangedDelegate(this.OnButtonClickPrev));
		this.m_Box_Box11 = (base.GetControl("Box_Box11") as Box);
		this.m_Button_Button13 = (base.GetControl("Button_Button13") as Button);
		Button expr_15E = this.m_Button_Button13;
		expr_15E.Click = (EZValueChangedDelegate)Delegate.Combine(expr_15E.Click, new EZValueChangedDelegate(this.OnButtonClickNext));
		base.SetSize(base.GetSize().x, base.GetSize().y);
		base.Draggable = false;
		base.SetLocation(base.GetLocation().x, GUICamera.height - base.GetSize().y, 100f);
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
		this.m_bSlideHide = false;
	}

	private void FadeInOut()
	{
		if (this.m_nCharKind == 0)
		{
			return;
		}
		float num = Time.deltaTime * 250f;
		if (this.m_bSlideHide)
		{
			num *= 3f * (base.GetSize().x - base.GetLocation().x) / base.GetSize().x;
			base.SetLocation(base.GetLocation().x - num, GUICamera.height - base.GetSize().y);
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
				base.SetLocation(base.GetLocation().x + num, GUICamera.height - base.GetSize().y);
			}
			else
			{
				base.SetLocation(0f, GUICamera.height - base.GetSize().y);
			}
		}
	}

	public void SetInfo(int nCharKind, byte byLevel)
	{
		this.m_nPageNum = 1;
		this.m_nCharKind = nCharKind;
		this.m_byLevel = byLevel;
		this.CalcMaxPageNum();
		this.SetPageNum();
		this.SetImage();
		this.SetMessage();
		base.SetLocation(-base.GetSize().x, GUICamera.height - base.GetSize().y);
	}

	private void CalcMaxPageNum()
	{
		int num = 1;
		while (true)
		{
			string strTextKey = string.Empty;
			string text = string.Empty;
			strTextKey = string.Format("Help_Level_{0:000}_{1:00}", this.m_byLevel, num);
			text = NrTSingleton<NrTextMgr>.Instance.GetTextFromhelper(strTextKey);
			if (string.IsNullOrEmpty(text) || " " == text || num == 100 || text.StartsWith("(xxx:"))
			{
				break;
			}
			num++;
		}
		this.m_nPageMax = num - 1;
		if (this.m_nPageMax < 1)
		{
			this.m_nPageMax = 1;
		}
	}

	private void SetPageNum()
	{
		string textColor = NrTSingleton<CTextParser>.Instance.GetTextColor("1107");
		this.m_Box_Box11.Text = textColor + this.m_nPageNum.ToString() + " / " + this.m_nPageMax.ToString();
	}

	private void SetImage()
	{
		this.m_DrawTexture_DrawTexture0.SetTexture(eCharImageType.LARGE, this.m_nCharKind, -1);
	}

	private void SetMessage()
	{
		string text = string.Empty;
		string empty = string.Empty;
		string strTextKey = string.Empty;
		string text2 = string.Empty;
		if (NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1) == null)
		{
			return;
		}
		text = NrTSingleton<NrTextMgr>.Instance.GetTextFromhelper("Help_Level_Default");
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			text,
			"count",
			this.m_byLevel.ToString()
		});
		this.m_Label_Label2.Text = string.Empty;
		this.m_Label_Label3.Text = string.Empty;
		if (!string.IsNullOrEmpty(empty))
		{
			string[] separator = new string[]
			{
				"\r\n"
			};
			string[] array = empty.Split(separator, StringSplitOptions.None);
			if (array.Length > 0 && array[0].Length > 0)
			{
				this.m_Label_Label2.Text = array[0];
			}
			if (array.Length > 1 && array[1].Length > 0)
			{
				this.m_Label_Label3.Text = array[1];
			}
		}
		strTextKey = string.Format("Help_Level_{0:000}_{1:00}", this.m_byLevel, this.m_nPageNum);
		text2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromhelper(strTextKey);
		this.m_FlashLabel_Label1.SetFlashLabel(string.Empty);
		if (!string.IsNullOrEmpty(text2))
		{
			this.m_FlashLabel_Label1.SetFlashLabel(text2);
		}
		base.SetLocation(0f, GUICamera.height - base.GetSize().y);
	}

	public static long GetTick()
	{
		return (long)Environment.TickCount;
	}

	private void OnButtonClickExit(IUIObject obj)
	{
		Debug.Log("--- OnButtonClickExit ---");
		this.m_bSlideHide = true;
		AlarmManager.GetInstance().CloseLevelUpAlarm();
	}

	private void OnButtonClickPrev(IUIObject obj)
	{
		if (this.m_nPageNum > 1)
		{
			this.m_nPageNum--;
			this.SetMessage();
			this.SetPageNum();
		}
	}

	private void OnButtonClickNext(IUIObject obj)
	{
		if (this.m_nPageNum < this.m_nPageMax)
		{
			this.m_nPageNum++;
			this.SetMessage();
			this.SetPageNum();
		}
	}
}
