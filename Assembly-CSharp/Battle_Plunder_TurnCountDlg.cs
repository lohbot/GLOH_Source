using System;
using UnityForms;

public class Battle_Plunder_TurnCountDlg : Form
{
	private DrawTexture m_dwTurnCount;

	private DrawTexture m_dwTurnCount2;

	private int m_nMaxTurnCount;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		base.TopMost = true;
		instance.LoadFileAll(ref form, "Plunder/DLG_pvp_turncount", G_ID.BATTLE_PLUNDER_TURNCOUNT_DLG, false);
		if (base.InteractivePanel != null)
		{
			base.Draggable = false;
			base.AlwaysUpdate = true;
		}
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_dwTurnCount = (base.GetControl("DrawTexture_TurnCount") as DrawTexture);
		this.m_dwTurnCount2 = (base.GetControl("DrawTexture_TurnCount2") as DrawTexture);
		this.Hide();
	}

	public override void InitData()
	{
		base.InitData();
		float x = GUICamera.width - base.GetSizeX();
		base.SetLocation(x, 0f);
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		float x = GUICamera.width - base.GetSizeX();
		base.SetLocation(x, 0f);
	}

	public void SetData(int nTurnCount)
	{
		this.m_nMaxTurnCount = nTurnCount;
		this.UpdateData();
		this.Show();
		PlunderGoldDlg plunderGoldDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDER_GOLD_DLG) as PlunderGoldDlg;
		if (plunderGoldDlg != null)
		{
			float x = plunderGoldDlg.GetLocationX() - base.GetSizeX() + 120f;
			base.SetLocation(x, 0f);
		}
	}

	public void UpdateData()
	{
		int nTurnCount = Battle.BATTLE.m_nTurnCount;
		int num = this.m_nMaxTurnCount - nTurnCount;
		int num2 = num / 10;
		if (num2 == 0)
		{
			this.m_dwTurnCount2.Visible = false;
		}
		else
		{
			string texture = "Win_Number_" + num2.ToString();
			this.m_dwTurnCount2.SetTexture(texture);
		}
		string texture2 = "Win_Number_" + (num % 10).ToString();
		this.m_dwTurnCount.SetTexture(texture2);
	}
}
