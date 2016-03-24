using System;
using UnityEngine;
using UnityForms;

public class UI_MiniDramaTalk : Form
{
	private DrawTexture dtFace1;

	private Label flbTalk1;

	private TsWeakReference<NrCharBase> _kChar;

	private float _Showtime;

	private bool _Show;

	public bool m_ShowUI
	{
		get
		{
			return this._Show;
		}
		set
		{
			this._Show = value;
			this.SetShowControll(this._Show);
			base.InteractivePanel.gameObject.SetActive(this._Show);
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "MiniDrama/DLG_MiniDramaTalk", G_ID.MINIDIRECTONTALK_DLG, true);
	}

	public override void SetComponent()
	{
		this.dtFace1 = (base.GetControl("DrawTexture_DrawTexture1") as DrawTexture);
		this.flbTalk1 = (base.GetControl("Label_Talk") as Label);
		this.flbTalk1.MaxWidth = 200f;
		this.flbTalk1.UpdateText = true;
	}

	private void SetShowControll(bool bShow)
	{
		if (bShow)
		{
			this.Show();
		}
		else
		{
			this.Hide();
		}
	}

	public override void InitData()
	{
		base.InitData();
		base.Draggable = false;
		base.ShowLayer(1);
		base.InteractivePanel.depthChangeable = true;
	}

	public void Init(NrCharBase kChar)
	{
		this._kChar = kChar;
	}

	public void SetTalk(string text, float fShowTime)
	{
		NrCharBase nrCharBase = this._kChar;
		if (nrCharBase == null)
		{
			return;
		}
		this._Showtime = Time.time + fShowTime;
		string szColorNum = "1101";
		if (nrCharBase.GetCharKindInfo() != null)
		{
			int cHARKIND = nrCharBase.GetCharKindInfo().GetCHARKIND_INFO().CHARKIND;
			this.dtFace1.SetTexture(eCharImageType.SMALL, cHARKIND, -1, string.Empty);
		}
		string strText = NrTSingleton<CTextParser>.Instance.GetTextColor(szColorNum) + text;
		this.SetTalkControl(strText, this.flbTalk1);
		this.m_ShowUI = true;
	}

	private void SetTalkControl(string strText, Label flbTalk)
	{
		if (strText != null)
		{
			Vector2 a = Vector2.zero;
			flbTalk.SetAnchorText(SpriteText.Anchor_Pos.Upper_Left);
			flbTalk.SetText(strText);
			float totalWidth = flbTalk.spriteText.TotalWidth;
			flbTalk.Setup(totalWidth, flbTalk.spriteText.TotalHeight);
			flbTalk.SetSize(totalWidth, flbTalk.spriteText.TotalHeight);
			flbTalk.SetText(strText);
			flbTalk.FindOuterEdges();
			a.y = flbTalk.BottomRightEdge().y;
			float num = flbTalk.spriteText.characterSize;
			num = 0f;
			Vector2 b = new Vector2(num, -num);
			Vector2 b2 = new Vector2(flbTalk.GetLocation().x, flbTalk.GetLocation().y);
			Vector2 b3 = new Vector2(22f, -13f);
			if (a.x < flbTalk.BottomRightEdge().x)
			{
				a = flbTalk.BottomRightEdge();
			}
			Vector2 vector = a + b2 + b + b3;
			base.SetSize(vector.x, -vector.y);
		}
	}

	public override void Update()
	{
		if (this._Showtime != 0f && Time.time > this._Showtime)
		{
			this._Showtime = 0f;
			this.m_ShowUI = false;
		}
	}

	public void UpatePotition()
	{
		if (this._kChar != null)
		{
			NrCharBase nrCharBase = this._kChar;
			if (nrCharBase == null)
			{
				return;
			}
			Vector3 pos = nrCharBase.GetNameDummy().position;
			pos = this.WorldToEZ(pos);
			pos.x -= base.GetSize().x / 2f;
			pos.y -= base.GetSize().y / 2f;
			base.SetLocation(pos.x, pos.y);
		}
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
