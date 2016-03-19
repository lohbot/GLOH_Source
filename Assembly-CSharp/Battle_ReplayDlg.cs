using GameMessage;
using System;
using UnityEngine;
using UnityForms;

public class Battle_ReplayDlg : Form
{
	private Button m_btStop;

	private Button m_btReduce;

	private Button m_btAccel;

	private Button m_bOff;

	private Label m_lbSpeed;

	private float m_fSpeed = 1f;

	public float Speed
	{
		get
		{
			return this.m_fSpeed;
		}
		set
		{
			this.m_fSpeed = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_replay", G_ID.BATTLE_REPLAY_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_btStop = (base.GetControl("btn_stop") as Button);
		Button expr_1C = this.m_btStop;
		expr_1C.Click = (EZValueChangedDelegate)Delegate.Combine(expr_1C.Click, new EZValueChangedDelegate(this.OnClickStop));
		this.m_btReduce = (base.GetControl("btn_reduce") as Button);
		Button expr_59 = this.m_btReduce;
		expr_59.Click = (EZValueChangedDelegate)Delegate.Combine(expr_59.Click, new EZValueChangedDelegate(this.OnClickReduce));
		this.m_btAccel = (base.GetControl("btn_accel") as Button);
		Button expr_96 = this.m_btAccel;
		expr_96.Click = (EZValueChangedDelegate)Delegate.Combine(expr_96.Click, new EZValueChangedDelegate(this.OnClickAccel));
		this.m_bOff = (base.GetControl("btn_off") as Button);
		Button expr_D3 = this.m_bOff;
		expr_D3.Click = (EZValueChangedDelegate)Delegate.Combine(expr_D3.Click, new EZValueChangedDelegate(this.OnClickOff));
		this.m_lbSpeed = (base.GetControl("Label_speed") as Label);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("177"),
			"count",
			Time.timeScale.ToString("f1")
		});
		this.m_lbSpeed.SetText(empty);
		this._SetDialogPos();
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(GUICamera.width - base.GetSizeX(), GUICamera.height - base.GetSizeY());
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void OnClickStop(IUIObject obj)
	{
		if (Time.timeScale != 0f)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("177"),
				"count",
				Time.timeScale.ToString("f1")
			});
			this.m_lbSpeed.SetText(empty);
		}
		this.m_fSpeed = Time.timeScale;
	}

	public void OnClickReduce(IUIObject obj)
	{
		Time.timeScale -= 0.5f;
		if (Time.timeScale <= 0.5f)
		{
			Time.timeScale = 0.5f;
		}
		this.m_fSpeed = Time.timeScale;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("177"),
			"count",
			Time.timeScale.ToString("f1")
		});
		this.m_lbSpeed.SetText(empty);
	}

	public void OnClickAccel(IUIObject obj)
	{
		Time.timeScale += 0.5f;
		if (Time.timeScale >= 3f)
		{
			Time.timeScale = 3f;
		}
		this.m_fSpeed = Time.timeScale;
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("177"),
			"count",
			Time.timeScale.ToString("f1")
		});
		this.m_lbSpeed.SetText(empty);
	}

	public void OnClickOff(IUIObject obj)
	{
		Battle.BATTLE.Observer = true;
		MsgHandler.Handle("Rcv_BATTLE_RESULT", new object[0]);
	}
}
