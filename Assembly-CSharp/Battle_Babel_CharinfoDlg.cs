using PROTOCOL.GAME;
using System;
using UnityEngine;
using UnityForms;

public class Battle_Babel_CharinfoDlg : Form
{
	private Label m_lbTurnTime;

	private Label m_lbTurnInfo;

	private Label m_lbEmoticon;

	private BATTLE_BABELTOWER_CHARINFO[] m_stCharinfo;

	private bool m_bUpdate;

	private int m_nAdvantageIndex = -1;

	private int m_nMyIndex = -1;

	private float[] m_fTime;

	private int[] m_nRemainTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_babel_charinfo", G_ID.BATTLE_BABEL_CHARINFO_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_stCharinfo = new BATTLE_BABELTOWER_CHARINFO[4];
		this.m_fTime = new float[4];
		this.m_nRemainTime = new int[4];
		this.m_lbTurnTime = (base.GetControl("LB_TurnTime") as Label);
		this.m_lbTurnInfo = (base.GetControl("LB_TurnInfoText") as Label);
		this.m_lbEmoticon = (base.GetControl("Label_Label6") as Label);
		this.m_lbEmoticon.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("925"));
		base.SetShowLayer(1, false);
		base.SetShowLayer(2, false);
		this.m_bUpdate = false;
		this._SetDialogPos();
	}

	public void SetData(BATTLE_BABELTOWER_CHARINFO[] arInfo)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			this.m_stCharinfo[i] = arInfo[i];
			if (arInfo[i].nCharUnique > 0)
			{
				num++;
			}
		}
		if (num <= 1)
		{
			this.Close();
			return;
		}
		this.m_bUpdate = true;
		if (!NrTSingleton<FormsManager>.Instance.IsShow(G_ID.BATTLE_EMOTICON_DLG))
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BATTLE_EMOTICON_DLG);
		}
	}

	public override void InitData()
	{
		this._SetDialogPos();
	}

	public void _SetDialogPos()
	{
		float x = (GUICamera.width - base.GetSizeX()) / 2f;
		float y = 5f;
		base.SetLocation(x, y);
	}

	public override void ChangedResolution()
	{
		this._SetDialogPos();
	}

	public void ChangeTurn(eBATTLE_ALLY eCurrentTurnAlly)
	{
		for (int i = 0; i < 4; i++)
		{
			if (eCurrentTurnAlly == eBATTLE_ALLY.eBATTLE_ALLY_0)
			{
				float num = BATTLE_CONSTANT_Manager.GetInstance().GetValue(eBATTLE_CONSTANT.eBATTLE_CONSTANT_BATTLE_TURN_CONTROL_DELAY);
				this.m_fTime[i] = Time.realtimeSinceStartup + (float)i * num;
				this.m_nRemainTime[i] = -1;
				NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
				if (this.m_stCharinfo[i].nCharUnique == nrCharUser.GetCharUnique())
				{
					Battle.BATTLE.EnableOrderTime = this.m_fTime[i];
					Battle.BATTLE.BabelAdvantageCharUnique = 0;
					this.m_nAdvantageIndex = -1;
					this.m_nMyIndex = i;
					base.SetShowLayer(1, true);
					string empty = string.Empty;
					NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
					{
						NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("924"),
						"charname",
						TKString.NEWString(this.m_stCharinfo[0].szCharName)
					});
					this.m_lbTurnInfo.SetText(empty);
				}
			}
			else
			{
				this.m_lbTurnTime.SetText("-");
				Battle.BATTLE.BabelAdvantageCharUnique = 0;
				base.SetShowLayer(1, false);
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (!this.m_bUpdate)
		{
			return;
		}
		if (Battle.BATTLE == null)
		{
			return;
		}
		if (Battle.BATTLE.CurrentTurnAlly != eBATTLE_ALLY.eBATTLE_ALLY_0)
		{
			return;
		}
		if (this.m_nAdvantageIndex == this.m_nMyIndex)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (this.m_stCharinfo[i].nCharUnique > 0)
			{
				float num = this.m_fTime[i] - Time.realtimeSinceStartup;
				if (num < -1f)
				{
					num = -1f;
					this.m_nAdvantageIndex = i;
				}
				if (this.m_nRemainTime[i] != (int)num)
				{
					this.m_nRemainTime[i] = (int)num + 1;
					if (this.m_nMyIndex == i)
					{
						this.m_lbTurnTime.SetText(this.m_nRemainTime[i].ToString());
					}
				}
			}
		}
		if (this.m_nAdvantageIndex >= 0 && this.m_nAdvantageIndex < this.m_nMyIndex && Battle.BATTLE.BabelAdvantageCharUnique != this.m_stCharinfo[this.m_nAdvantageIndex].nCharUnique)
		{
			Battle.BATTLE.BabelAdvantageCharUnique = this.m_stCharinfo[this.m_nAdvantageIndex].nCharUnique;
			Battle.BATTLE.GRID_MANAGER.BabelTower_Battle_Grid_Update();
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("924"),
				"charname",
				TKString.NEWString(this.m_stCharinfo[this.m_nAdvantageIndex].szCharName)
			});
			this.m_lbTurnInfo.SetText(empty);
		}
		if (this.m_nAdvantageIndex == this.m_nMyIndex)
		{
			base.SetShowLayer(1, false);
			Battle.BATTLE.GRID_MANAGER.BabelTower_Battle_Grid_Update();
		}
	}

	public void SetEmoticonText(bool bShow)
	{
		base.SetShowLayer(2, bShow);
	}
}
