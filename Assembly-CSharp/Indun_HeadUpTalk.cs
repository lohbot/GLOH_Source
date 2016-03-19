using System;
using UnityEngine;
using UnityForms;

public class Indun_HeadUpTalk : Form
{
	private TsWeakReference<NrCharBase> m_TargetChar;

	private Label m_lbCharName;

	private Label m_lbTalk;

	private float m_fEndTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Indun/DLG_INDUN_Headuptalk", G_ID.INDUN_HEADUP_TALK, false);
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

	public void Set(NrCharBase pkTarget, string strName, float fTime, int nTextIndex)
	{
		this.m_TargetChar = pkTarget;
		this.m_lbCharName.SetText(strName);
		string textFromTBS = NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(nTextIndex.ToString());
		this.m_lbTalk.Text = textFromTBS;
		float width = this.m_lbTalk.GetWidth();
		float totalHeight = this.m_lbTalk.spriteText.TotalHeight;
		base.SetSize(base.GetSize().x, this.m_lbTalk.GetLocationY() + totalHeight + 14f);
		this.m_lbTalk.Setup(width, totalHeight);
		this.m_lbTalk.Text = textFromTBS;
		this.m_fEndTime = Time.time + fTime;
		NrTSingleton<ChatManager>.Instance.PushMsg(CHAT_TYPE.BATTLE, strName, NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(nTextIndex.ToString()));
		this.Show();
	}

	public void UpdatePosition()
	{
		if (this.m_TargetChar == null)
		{
			return;
		}
		if (this.m_TargetChar.CastedTarget.GetNameDummy() == null)
		{
			return;
		}
		Vector3 pos = Vector3.zero;
		pos = this.m_TargetChar.CastedTarget.GetNameDummy().position;
		pos = this.WorldToEZ(pos);
		pos.x -= base.GetSize().x / 2f;
		pos.y -= base.GetSize().y;
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
