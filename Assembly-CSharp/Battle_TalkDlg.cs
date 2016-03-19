using System;
using UnityEngine;
using UnityForms;

public class Battle_TalkDlg : Form
{
	private Label m_lbCharName;

	private FlashLabel m_lbTalk;

	private DrawTexture m_dwCharFace;

	private DrawTexture m_dwBG;

	private DrawTexture m_dwBGLine1;

	private DrawTexture m_dwBGLine2;

	private float m_fEndTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_Talk", G_ID.BATTLE_TALK_DLG, false);
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_lbCharName = (base.GetControl("CharTalk_Charname") as Label);
		this.m_lbTalk = (base.GetControl("CharTalk_talklabel") as FlashLabel);
		this.m_dwCharFace = (base.GetControl("DrawTexture_CharFace01") as DrawTexture);
		this.m_dwBG = (base.GetControl("CharTalk_BG") as DrawTexture);
		this.m_dwBGLine1 = (base.GetControl("CharTalk_BG_line") as DrawTexture);
		this.m_dwBGLine2 = (base.GetControl("CharTalk_BG_line2") as DrawTexture);
		base.SetSize(GUICamera.width, base.GetSizeY());
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
		this.m_dwBG.SetSize(GUICamera.width, this.m_dwBG.GetSize().y);
		this.m_dwBGLine1.SetSize(GUICamera.width, this.m_dwBGLine1.GetSize().y);
		this.m_dwBGLine2.SetSize(GUICamera.width, this.m_dwBGLine2.GetSize().y);
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
		this.m_lbCharName.SetText(strName);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromTBS(nTextIndex.ToString())
		});
		this.m_lbTalk.SetFlashLabel(empty);
		this.m_dwCharFace.SetTexture(eCharImageType.LARGE, pkTarget.GetCharKindInfo().GetCharKind(), (int)pkTarget.GetSoldierInfo().GetGrade());
		this.m_fEndTime = Time.time + fTime;
		this.Show();
	}

	public void UpdatePosition()
	{
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
