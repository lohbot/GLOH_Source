using System;
using UnityEngine;
using UnityForms;

public class UI_UIGuide : Form
{
	private DrawTexture _BG;

	private FlashLabel _Text;

	private float _ScreenWidth;

	private float _ScreenHeight;

	public DrawTexture m_HelpTip;

	public DrawTexture m_HelpBack;

	public Label m_HelpText;

	public Button m_Touch;

	private TsWeakReference<NkBattleChar> m_TargetChar;

	private bool m_bClose;

	private bool bAutoClose;

	private float fCloseTime;

	public G_ID m_nOtherWinID;

	public bool CloseUI
	{
		get
		{
			return this.m_bClose;
		}
		set
		{
			this.m_bClose = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.TopMost = true;
		instance.LoadFileAll(ref form, "System/DLG_UIGuide", G_ID.UIGUIDE_DLG, false);
		base.Draggable = false;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
		this.Show();
		base.DonotDepthChange(NrTSingleton<FormsManager>.Instance.GetTopMostZ());
	}

	public override void SetComponent()
	{
		this._SetComponetBasic();
		this.ResizeDlg();
	}

	public override void InitData()
	{
		base.InitData();
	}

	public override void Update()
	{
		base.Update();
		if (this.m_TargetChar != null && this.m_TargetChar.CastedTarget != null)
		{
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector3 pos = Vector3.zero;
			pos = this.m_TargetChar.CastedTarget.Get3DChar().GetRootGameObject().transform.position;
			pos = GUICamera.WorldToEZ(pos);
			zero = new Vector2(pos.x, pos.y - 150f);
			zero2 = new Vector2(pos.x - 20f, pos.y - 190f);
			this.Move(zero, zero2);
		}
		if (this._ScreenWidth != GUICamera.width || this._ScreenHeight != GUICamera.height)
		{
			this.ResizeDlg();
		}
		if (this.m_nOtherWinID != G_ID.NONE)
		{
			Form form = NrTSingleton<FormsManager>.Instance.GetForm(this.m_nOtherWinID);
			if (form != null)
			{
				form.SetLocation(form.GetLocationX(), form.GetLocationY(), NrTSingleton<FormsManager>.Instance.GetTopMostZ() - 1f);
			}
		}
		if (!this.bAutoClose)
		{
			return;
		}
		if (Time.realtimeSinceStartup > this.fCloseTime)
		{
			this.Close();
		}
	}

	public void _SetComponetBasic()
	{
		this._BG = (base.GetControl("Talk_BG") as DrawTexture);
		this._Text = (base.GetControl("Talk_label") as FlashLabel);
		this.m_HelpTip = (base.GetControl("DrawTexture_Helpicon") as DrawTexture);
		this.m_HelpBack = (base.GetControl("DrawTexture_HelpText") as DrawTexture);
		this.m_HelpText = (base.GetControl("Label_HelpText") as Label);
		this.m_HelpText.UpdateText = true;
		this.m_Touch = (base.GetControl("DrawTexture_Point") as Button);
		this.m_Touch.PlayAni(true);
		BoxCollider component = this.m_Touch.gameObject.GetComponent<BoxCollider>();
		if (null != component)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	public void ResizeDlg()
	{
		base.SetLocation(0f, 0f);
		this._ScreenWidth = GUICamera.width;
		this._ScreenHeight = GUICamera.height;
		this._BG.SetLocation(0f, 0f);
		this._BG.SetSize(this._ScreenWidth, this._ScreenHeight);
	}

	public override void Close()
	{
		base.Close();
	}

	public void SetText(string textkey)
	{
		this._Text.SetFlashLabel(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify(textkey));
		this.m_HelpText.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify(textkey);
		this.m_HelpText.SetSize(this.m_HelpText.spriteText.TotalWidth, this.m_HelpText.spriteText.TotalHeight + 20f);
		this.m_HelpText.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify(textkey);
		this.m_HelpBack.SetSize(this.m_HelpText.GetWidth() + 20f, this.m_HelpText.height + 40f);
	}

	public void SetTextBattle(string textkey)
	{
		this.m_HelpText.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(textkey);
		this.m_HelpText.SetSize(this.m_HelpText.spriteText.TotalWidth, this.m_HelpText.spriteText.TotalHeight + 20f);
		this.m_HelpText.Text = NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(textkey);
		this.m_HelpBack.SetSize(this.m_HelpText.GetWidth() + 20f, this.m_HelpText.height + 20f);
	}

	public void Move(Vector2 x1, Vector2 x2)
	{
		this.m_Touch.SetLocation(x1.x - this.m_Touch.width, x1.y, -2f);
		this.m_HelpTip.SetLocation(x2.x - 17f, x2.y - 66f, -3f);
		this.m_HelpText.SetLocation(x2.x, x2.y - this.m_HelpText.spriteText.TotalHeight - 5f + 20f, -2f);
		this.m_HelpBack.SetLocation(x2.x - 10f, x2.y - 5f - this.m_HelpText.spriteText.TotalHeight - 5f, -1.9f);
	}

	public void Move(Vector2 x1, Vector2 x2, float time)
	{
		this.Move(x1, x2);
		this.bAutoClose = true;
		this.fCloseTime = Time.realtimeSinceStartup + time;
	}

	public void SetBattleTutorial(int value1, int value2, NkBattleChar pkBattleChar)
	{
		base.SetShowLayer(0, false);
		base.SetShowLayer(1, true);
		this.SetTextBattle(value1.ToString());
		if (value1 == 101010703)
		{
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (pkBattleChar != null)
			{
				this.m_TargetChar = pkBattleChar;
				Vector3 pos = Vector3.zero;
				pos = this.m_TargetChar.CastedTarget.Get3DChar().GetRootGameObject().transform.position;
				pos = GUICamera.WorldToEZ(pos);
				zero = new Vector2(pos.x, pos.y - 150f);
				zero2 = new Vector2(pos.x - 20f, pos.y - 190f);
			}
			else
			{
				zero = new Vector2(GUICamera.width / 2f + this.m_Touch.width, GUICamera.height / 2f - 150f);
				zero2 = new Vector2(GUICamera.width / 2f, GUICamera.height / 2f - 190f);
			}
			this.Move(zero, zero2);
			this._BG.UsedCollider(false);
		}
		else if (value1 == 101010701)
		{
			Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg == null)
			{
				return;
			}
			Vector2 skillButtonPos = battle_Control_Dlg.GetSkillButtonPos();
			Vector3 v = new Vector2(skillButtonPos.x + 60f, skillButtonPos.y - 25f);
			Vector2 x = new Vector2(skillButtonPos.x - this.m_HelpText.GetSize().x - 30f, skillButtonPos.y - 55f);
			this.Move(v, x);
			this._BG.UsedCollider(false);
		}
	}
}
