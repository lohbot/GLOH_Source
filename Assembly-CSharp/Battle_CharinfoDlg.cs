using System;
using UnityForms;

public class Battle_CharinfoDlg : Form
{
	private ItemTexture[] m_itSolPortrait;

	private Button[] m_btSol;

	private DrawTexture[] m_pbBG;

	private ProgressBar[] m_pbSolHP;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_CharInfo", G_ID.BATTLE_CHARINFO_DLG, false);
		if (base.InteractivePanel != null)
		{
			base.Draggable = false;
			base.AlwaysUpdate = true;
		}
	}

	public override void SetComponent()
	{
		this.m_itSolPortrait = new ItemTexture[6];
		this.m_btSol = new Button[6];
		this.m_pbSolHP = new ProgressBar[6];
		this.m_pbBG = new DrawTexture[6];
		for (int i = 0; i < 6; i++)
		{
			string name = string.Format("character0{0}", (i + 1).ToString());
			this.m_itSolPortrait[i] = (base.GetControl(name) as ItemTexture);
			name = string.Format("btn0{0}", (i + 1).ToString());
			this.m_btSol[i] = (base.GetControl(name) as Button);
			Button expr_93 = this.m_btSol[i];
			expr_93.Click = (EZValueChangedDelegate)Delegate.Combine(expr_93.Click, new EZValueChangedDelegate(this.OnClickSol));
			name = string.Format("ProgressBar_0{0}", (i + 1).ToString());
			this.m_pbSolHP[i] = (base.GetControl(name) as ProgressBar);
			name = string.Format("guage_bg0{0}", (i + 1).ToString());
			this.m_pbBG[i] = (base.GetControl(name) as DrawTexture);
			this.m_pbSolHP[i].Visible = false;
			this.m_pbBG[i].Visible = false;
		}
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
	}

	public override void InitData()
	{
		base.InitData();
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_CHARINFO);
		if (form != null)
		{
			form.Hide();
		}
		form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG);
		if (form != null)
		{
			form.Hide();
		}
		if (!TsPlatform.IsMobile)
		{
			form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
			if (form != null)
			{
				form.Hide();
			}
		}
		else
		{
			form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
			if (form != null)
			{
				form.Hide();
			}
		}
		this.SetSolderinfo();
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetLocation(0f, GUICamera.height - base.GetSizeY());
	}

	public override void OnClose()
	{
		Form form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_CHARINFO);
		if (form != null)
		{
			form.Show();
			form.InitData();
		}
		form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.MYCHARINFO_DLG);
		if (form != null)
		{
			form.Show();
			form.InitData();
		}
		if (!TsPlatform.IsMobile)
		{
			form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
			if (form != null)
			{
				form.Show();
			}
		}
		else
		{
			form = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.CHAT_MAIN_DLG);
			if (form != null)
			{
				form.Show();
			}
		}
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Show()
	{
		base.Show();
		this.SetSolderinfo();
	}

	public void SetSolderinfo()
	{
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(i);
			if (!soldierInfo.IsValid())
			{
				base.SetShowLayer(i, false);
			}
			else
			{
				this.m_itSolPortrait[i].SetSolImageTexure(eCharImageType.SMALL, soldierInfo.GetCharKind(), (int)soldierInfo.GetGrade());
				this.m_pbSolHP[i].Value = (float)soldierInfo.GetHP() / (float)soldierInfo.GetMaxHP();
				this.SetToolTip(i, soldierInfo);
				this.m_pbSolHP[i].Visible = false;
				this.m_pbBG[i].Visible = false;
			}
		}
	}

	public void UpdateHP(int nSolIndex)
	{
		if (nSolIndex < 0 || nSolIndex >= 6)
		{
			return;
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(nSolIndex);
		if (soldierInfo != null && soldierInfo.IsValid())
		{
			this.m_pbSolHP[nSolIndex].Value = (float)soldierInfo.GetHP() / (float)soldierInfo.GetMaxHP();
			this.SetToolTip(nSolIndex, soldierInfo);
		}
	}

	public void OnClickSol(IUIObject obj)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_btSol[i] == obj)
			{
				break;
			}
		}
	}

	private void SetToolTip(int solposindex, NkSoldierInfo pkSolinfo)
	{
		if (pkSolinfo == null)
		{
			this.m_btSol[solposindex].ToolTip = null;
		}
		else
		{
			NrTSingleton<UIDataManager>.Instance.InitStringBuilder();
			string value = string.Empty;
			NrTSingleton<UIDataManager>.Instance.GetStringBuilder().Append(pkSolinfo.GetName());
			NrTSingleton<UIDataManager>.Instance.GetStringBuilder().AppendLine();
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref value, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("167"),
				"count1",
				pkSolinfo.GetLevel().ToString(),
				"count2",
				pkSolinfo.GetSolMaxLevel().ToString()
			});
			NrTSingleton<UIDataManager>.Instance.GetStringBuilder().Append(value);
			NrTSingleton<UIDataManager>.Instance.GetStringBuilder().AppendLine();
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref value, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1854"),
				"curhp",
				pkSolinfo.GetHP(),
				"maxhp",
				pkSolinfo.GetMaxHP()
			});
			NrTSingleton<UIDataManager>.Instance.GetStringBuilder().Append(value);
			NrTSingleton<UIDataManager>.Instance.GetStringBuilder().AppendLine();
			if (pkSolinfo.IsMaxLevel())
			{
				value = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("286");
			}
			else
			{
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref value, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1855"),
					"curexp",
					pkSolinfo.GetExp(),
					"nextexp",
					pkSolinfo.GetNextExp()
				});
			}
			NrTSingleton<UIDataManager>.Instance.GetStringBuilder().Append(value);
			this.m_btSol[solposindex].ToolTip = NrTSingleton<UIDataManager>.Instance.GetString();
		}
	}
}
