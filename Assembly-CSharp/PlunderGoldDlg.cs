using System;
using UnityEngine;
using UnityForms;

public class PlunderGoldDlg : Form
{
	private Label m_lbGold;

	private Label m_lbCharName;

	private Label m_lbCharLevel;

	private TsWeakReference<NkBattleChar>[] m_pkTreasureChar;

	private int[] m_nCurHP;

	private float[] m_fMaxHP;

	private long m_nStartGold;

	private long m_nEndGold;

	private long m_nPlunderGold;

	private float m_fStartRate;

	private float m_fAniStartTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		base.Scale = true;
		instance.LoadFileAll(ref form, "Plunder/DLG_plunder_gold", G_ID.PLUNDER_GOLD_DLG, false);
		this.Hide();
	}

	public override void SetComponent()
	{
		this.m_lbGold = (base.GetControl("Label_gold2") as Label);
		this.m_lbGold.SetText("0");
		this.m_lbCharName = (base.GetControl("Label_CharName") as Label);
		this.m_lbCharLevel = (base.GetControl("Label_CharLevel") as Label);
		this.m_pkTreasureChar = new TsWeakReference<NkBattleChar>[3];
		this.m_nCurHP = new int[3];
		this.m_fMaxHP = new float[3];
		this.UpdatePosition();
	}

	public void SetTreasureChar(byte nStartPosindex, NkBattleChar pkChar)
	{
		if (nStartPosindex < 0 || nStartPosindex >= 3)
		{
			return;
		}
		this.m_pkTreasureChar[(int)nStartPosindex] = pkChar;
		this.m_fMaxHP[(int)nStartPosindex] = (float)this.m_pkTreasureChar[(int)nStartPosindex].CastedTarget.GetMaxHP(false);
		this.m_nCurHP[(int)nStartPosindex] = this.m_pkTreasureChar[(int)nStartPosindex].CastedTarget.GetSoldierInfo().GetHP();
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		if (kMyCharInfo != null && this.m_nPlunderGold == 0L)
		{
			this.m_nPlunderGold = kMyCharInfo.PlunderMoney;
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1435"),
				"charname",
				kMyCharInfo.PlunderCharName
			});
			this.m_lbCharName.SetText(empty);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1469"),
				"count",
				kMyCharInfo.PlunderCharLevel.ToString()
			});
			this.m_lbCharLevel.SetText(empty);
		}
	}

	public override void Update()
	{
		base.Update();
		this.UpdateGold();
		this.GoldAni();
	}

	public void UpdateGold()
	{
		float num = 0f;
		for (int i = 0; i < 3; i++)
		{
			if (this.m_pkTreasureChar[i].CastedTarget == null)
			{
				num += 1f;
			}
			else
			{
				int hP = this.m_pkTreasureChar[i].CastedTarget.GetSoldierInfo().GetHP();
				this.m_nCurHP[i] = hP;
				num += 1f - (float)this.m_nCurHP[i] / this.m_fMaxHP[i];
			}
		}
		num /= 3f;
		if (num != this.m_fStartRate)
		{
			this.m_nStartGold = (long)(this.m_fStartRate * 100f) * this.m_nPlunderGold / 100L;
			this.m_nEndGold = (long)(num * 100f) * this.m_nPlunderGold / 100L;
			if (this.m_fStartRate != num)
			{
				this.m_fStartRate = num;
			}
			this.m_fAniStartTime = Time.realtimeSinceStartup;
		}
	}

	public void GoldAni()
	{
		if (this.m_fAniStartTime != 0f)
		{
			if (Time.realtimeSinceStartup - this.m_fAniStartTime < 1.5f)
			{
				float num = (Time.realtimeSinceStartup - this.m_fAniStartTime) / 1.5f;
				long num2 = this.m_nStartGold + (long)((float)(this.m_nEndGold - this.m_nStartGold) * num);
				this.m_lbGold.SetText(ANNUALIZED.Convert(num2));
			}
			else
			{
				this.m_lbGold.SetText(ANNUALIZED.Convert(this.m_nEndGold));
				this.m_nStartGold = 0L;
				this.m_fAniStartTime = 0f;
			}
		}
	}

	public void UpdatePosition()
	{
		Battle_Plunder_TurnCountDlg battle_Plunder_TurnCountDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_PLUNDER_TURNCOUNT_DLG) as Battle_Plunder_TurnCountDlg;
		float x = GUICamera.width - base.GetSizeX();
		float y = 0f;
		if (battle_Plunder_TurnCountDlg != null)
		{
			y = battle_Plunder_TurnCountDlg.GetLocationY() + battle_Plunder_TurnCountDlg.GetSize().y;
		}
		base.SetLocation(x, y);
	}
}
