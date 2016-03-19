using PROTOCOL.GAME;
using System;
using UnityForms;

public class Battle_BossAggro_DLG : Form
{
	private enum eAGGRO_SOL_COUNT
	{
		eAGGRO_SOL_COUNT_1,
		eAGGRO_SOL_COUNT_2,
		eAGGRO_SOL_COUNT_3,
		eAGGRO_SOL_COUNT_MAX
	}

	private ItemTexture m_itBossMon;

	private DrawTexture m_itBossMonHP;

	private Label m_lbBossMonName;

	private ItemTexture[] m_itAggroSolIcon;

	private Label[] m_lbAggroSolVal;

	private DrawTexture[] m_dwAggroSol_bg1;

	private DrawTexture[] m_dwAggroSol_bg2;

	private DrawTexture m_dtGuildBossHpBG;

	private DrawTexture m_dtGuildBossHp;

	private Label m_lbGuildBossHp;

	private NkBattleChar m_pkBossBattleChar;

	private float fHpLength;

	private float fGuildBossHpLength;

	private float MAXHP;

	private int m_nBossImmuneCount;

	private int m_nGuildBossHpCurLevel = -1;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_battle_Bossaggro", G_ID.BATTLE_BOSSAGGRO_DLG, false);
		form.AlwaysUpdate = true;
		form.TopMost = true;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_itBossMon = (base.GetControl("ItemTexture_monster") as ItemTexture);
		this.m_itBossMonHP = (base.GetControl("DrawTexture_hp") as DrawTexture);
		this.fHpLength = this.m_itBossMonHP.GetSize().x;
		this.m_lbBossMonName = (base.GetControl("Label_monstername") as Label);
		this.m_itAggroSolIcon = new ItemTexture[3];
		this.m_lbAggroSolVal = new Label[3];
		this.m_dwAggroSol_bg1 = new DrawTexture[3];
		this.m_dwAggroSol_bg2 = new DrawTexture[3];
		for (int i = 0; i < 3; i++)
		{
			string name = string.Format("ItemTexture_{0}_sol", (i + 1).ToString());
			this.m_itAggroSolIcon[i] = (base.GetControl(name) as ItemTexture);
			this.m_itAggroSolIcon[i].Visible = false;
			string name2 = string.Format("Label_{0}_label", (i + 1).ToString());
			this.m_lbAggroSolVal[i] = (base.GetControl(name2) as Label);
			this.m_lbAggroSolVal[i].Visible = false;
			string name3 = string.Format("DrawTexture_{0}_bg01", (i + 1).ToString());
			this.m_dwAggroSol_bg1[i] = (base.GetControl(name3) as DrawTexture);
			this.m_dwAggroSol_bg1[i].Visible = false;
			string name4 = string.Format("DrawTexture_{0}_bg02", (i + 1).ToString());
			this.m_dwAggroSol_bg2[i] = (base.GetControl(name4) as DrawTexture);
			this.m_dwAggroSol_bg2[i].Visible = false;
		}
		this.m_dtGuildBossHpBG = (base.GetControl("DrawTexture_gbooshp2") as DrawTexture);
		this.m_dtGuildBossHp = (base.GetControl("DrawTexture_gbosshp") as DrawTexture);
		this.fGuildBossHpLength = this.m_dtGuildBossHp.GetSize().x;
		this.m_lbGuildBossHp = (base.GetControl("Label_gaugetext") as Label);
		this.Hide();
	}

	public void _SetDialogPos()
	{
		base.SetLocation(0f, 0f);
	}

	public void SetBossData(NkBattleChar nBossChar)
	{
		if (nBossChar != null && nBossChar.IsCharKindATB(128L))
		{
			this.m_pkBossBattleChar = nBossChar;
		}
		if (this.m_pkBossBattleChar != null)
		{
			this.MAXHP = (float)this.m_pkBossBattleChar.GetMaxHP(false);
			this.m_itBossMon.SetSolImageTexure(eCharImageType.SMALL, this.m_pkBossBattleChar.GetCharKindInfo().GetCharKind(), -1);
			this.m_lbBossMonName.SetText(this.m_pkBossBattleChar.GetCharName());
			this.Show();
			this.UpdateBossHP();
		}
	}

	public void UpdateBossInfo()
	{
		if (this.m_pkBossBattleChar == null)
		{
			return;
		}
		string empty = string.Empty;
		if (this.m_pkBossBattleChar.IsBattleCharATB(1024) && this.m_nBossImmuneCount > 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2156"),
				"targetname",
				this.m_pkBossBattleChar.GetCharName(),
				"count",
				this.m_nBossImmuneCount.ToString()
			});
			this.m_lbBossMonName.SetText(empty);
		}
		else if (this.m_pkBossBattleChar.IsBattleCharATB(32768) && this.m_nBossImmuneCount > 0)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2772"),
				"targetname",
				this.m_pkBossBattleChar.GetCharName(),
				"count",
				this.m_nBossImmuneCount.ToString()
			});
			this.m_lbBossMonName.SetText(empty);
		}
		else
		{
			this.m_lbBossMonName.SetText(this.m_pkBossBattleChar.GetCharName());
		}
	}

	public void SetBossImmuneCount(int immuneCount)
	{
		this.m_nBossImmuneCount = immuneCount;
	}

	public void DecBossImmuneCount()
	{
		if (this.m_nBossImmuneCount > 0)
		{
			this.m_nBossImmuneCount--;
		}
	}

	public void UpdateBossHP()
	{
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS)
		{
			this.UpdateGuildBossHP();
		}
		else
		{
			this.UpdateNormalBossHP();
		}
	}

	public void UpdateNormalBossHP()
	{
		if (this.m_pkBossBattleChar == null)
		{
			return;
		}
		base.SetShowLayer(2, false);
		float num = (float)this.m_pkBossBattleChar.GetSoldierInfo().GetHP();
		if (num > this.MAXHP)
		{
			num = this.MAXHP;
		}
		float num2 = num / this.MAXHP;
		this.m_itBossMonHP.SetSize(this.fHpLength * num2, this.m_itBossMonHP.GetSize().y);
	}

	public void UpdateAggroSolInfo(GS_BATTLE_BOSS_AGGRO_NFY AggroSolData)
	{
		if (this.m_pkBossBattleChar == null)
		{
			return;
		}
		if (this.m_pkBossBattleChar.GetBUID() != AggroSolData.i16BUID)
		{
			return;
		}
		string empty = string.Empty;
		for (int i = 0; i < 3; i++)
		{
			if (AggroSolData.i8AggroValue[i] == 0)
			{
				this.m_itAggroSolIcon[i].Visible = false;
				this.m_lbAggroSolVal[i].Visible = false;
				this.m_dwAggroSol_bg1[i].Visible = false;
				this.m_dwAggroSol_bg2[i].Visible = false;
			}
			else
			{
				NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(AggroSolData.i16AggroTargetBUID[i]);
				if (charByBUID != null)
				{
					this.m_itAggroSolIcon[i].SetSolImageTexure(eCharImageType.SMALL, charByBUID.GetCharKindInfo().GetCharKind(), -1);
				}
				CTextParser arg_E7_0 = NrTSingleton<CTextParser>.Instance;
				object[] expr_BA = new object[3];
				expr_BA[0] = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("672");
				expr_BA[1] = "Count";
				int arg_E6_1 = 2;
				int num = (int)AggroSolData.i8AggroValue[i];
				expr_BA[arg_E6_1] = num.ToString();
				arg_E7_0.ReplaceParam(ref empty, expr_BA);
				this.m_lbAggroSolVal[i].SetText(empty);
				this.m_itAggroSolIcon[i].Visible = true;
				this.m_lbAggroSolVal[i].Visible = true;
				this.m_dwAggroSol_bg1[i].Visible = true;
				this.m_dwAggroSol_bg2[i].Visible = true;
			}
		}
	}

	public void UpdateGuildBossHP()
	{
		if (this.m_pkBossBattleChar == null)
		{
			return;
		}
		int num = (int)(this.MAXHP / 10f);
		if (0 >= num)
		{
			num = 1;
		}
		int num2 = this.m_pkBossBattleChar.GetSoldierInfo().GetHP() / num;
		if (num2 != this.m_nGuildBossHpCurLevel)
		{
			this.m_nGuildBossHpCurLevel = num2;
			int num3 = 10 - num2;
			if (num3 <= 0)
			{
				num3 = 1;
			}
			string texture = "Win_T_Gauge" + num3.ToString("00");
			this.m_dtGuildBossHp.SetTexture(texture);
			if (num3 + 1 <= 10)
			{
				texture = "Win_T_Gauge" + (num3 + 1).ToString("00");
				this.m_dtGuildBossHpBG.SetTexture(texture);
			}
			else
			{
				this.m_dtGuildBossHpBG.Visible = false;
			}
			string empty = string.Empty;
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("411"),
				"count",
				num2.ToString()
			});
			this.m_lbGuildBossHp.SetText(empty);
		}
		int num4 = this.m_pkBossBattleChar.GetSoldierInfo().GetHP() % num;
		if (num4 > num)
		{
			num4 = num;
		}
		float num5 = (float)num4 / (float)num;
		this.m_dtGuildBossHp.SetSize(this.fGuildBossHpLength * num5, this.m_dtGuildBossHpBG.GetSize().y);
	}
}
