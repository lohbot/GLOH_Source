using System;
using UnityEngine;
using UnityForms;

public class Battle_HeadUpTalk : Form
{
	private TsWeakReference<NkBattleChar> m_TargetChar;

	private Label m_lbCharName;

	private Label m_lbTalk;

	private float m_fEndTime;

	private int m_nBattleCharID = -1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Headuptalk", G_ID.BATTLE_HEADUP_TALK, false);
	}

	public override void SetComponent()
	{
		this.m_lbCharName = (base.GetControl("Label_ChaName") as Label);
		this.m_lbTalk = (base.GetControl("Label_ChaTalk") as Label);
		this.m_lbTalk.MultiLine = true;
		this.m_lbTalk.MaxWidth = 180f;
		this.m_lbTalk.UpdateText = true;
		this.Hide();
	}

	public override void Update()
	{
		base.Update();
		this.UpdatePosition();
		if (Time.time > this.m_fEndTime)
		{
			this.Close();
		}
	}

	public void Set(NkBattleChar pkTarget, string strName, float fTime, int nTextIndex)
	{
		this.m_TargetChar = pkTarget;
		this.m_nBattleCharID = pkTarget.GetID();
		this.m_lbCharName.SetText(strName);
		string textFromTBS = NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(nTextIndex.ToString());
		this.m_lbTalk.Text = textFromTBS;
		float width = this.m_lbTalk.GetWidth();
		float totalHeight = this.m_lbTalk.spriteText.TotalHeight;
		base.SetSize(base.GetSizeX(), this.m_lbTalk.GetLocationY() + totalHeight + 14f);
		this.m_lbTalk.Setup(width, totalHeight);
		this.m_lbTalk.Text = textFromTBS;
		this.m_fEndTime = Time.time + fTime;
		this.Show();
	}

	public void UpdatePosition()
	{
		if (NrTSingleton<NkBattleCharManager>.Instance.GetChar(this.m_nBattleCharID) == null)
		{
			this.Close();
			return;
		}
		if (this.m_TargetChar == null)
		{
			this.Close();
			return;
		}
		if (this.m_TargetChar.CastedTarget.GetNameDummy() == null)
		{
			this.Close();
			return;
		}
		Vector3 pos = Vector3.zero;
		pos = this.m_TargetChar.CastedTarget.GetNameDummy().position;
		pos = this.WorldToEZ(pos);
		pos.x -= base.GetSizeX() / 2f;
		pos.y -= base.GetSizeY();
		base.SetLocation(pos.x, pos.y);
	}

	public Vector3 WorldToEZ(Vector3 Pos)
	{
		Camera main = Camera.main;
		if (null != main)
		{
			Pos = main.WorldToScreenPoint(Pos);
		}
		Pos.y = (float)Screen.height - Pos.y;
		Pos = GUICamera.ScreenToGUIPoint(Pos);
		return Pos;
	}
}
