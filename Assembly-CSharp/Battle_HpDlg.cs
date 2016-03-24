using GAME;
using System;
using UnityEngine;
using UnityForms;

public class Battle_HpDlg : Form
{
	private const int MAX_SKILL_BUFFICON = 5;

	private const int MAX_IMMUNE_ICON = 1;

	private TsWeakReference<NkBattleChar> m_TargetChar;

	private DrawTexture m_pkDrawTextureHP;

	private DrawTexture[] m_pkDrawTextureBuffSlot;

	private DrawTexture[] m_pkDrawTextureBuffiCon;

	private DrawTexture[] m_pkDrawTextureBuffStroke;

	private float fHpLength;

	private float MAXHP;

	private float fAniStartHP;

	private float fAniEndHP;

	private float fAniStartTime;

	private bool m_bImmuneIcon;

	private int MAX_BUFFICON = 6;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Battle/DLG_Battle_hpmpbuff_activeset", G_ID.BATTLE_HP_GROUP_DLG, false);
		base.DonotDepthChange();
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void SetComponent()
	{
		this.m_pkDrawTextureBuffSlot = new DrawTexture[this.MAX_BUFFICON];
		this.m_pkDrawTextureBuffiCon = new DrawTexture[this.MAX_BUFFICON];
		this.m_pkDrawTextureBuffStroke = new DrawTexture[this.MAX_BUFFICON];
	}

	public override void Update()
	{
		base.Update();
		this.UpdatePosition();
		this.UpdateHPAni();
	}

	public void Set(NkBattleChar pkTarget)
	{
		this.m_TargetChar = pkTarget;
		this.MAXHP = (float)this.m_TargetChar.CastedTarget.GetMaxHP(false);
		this.m_pkDrawTextureHP = (base.GetControl("DrawTexture_GauHpE") as DrawTexture);
		this.fHpLength = this.m_pkDrawTextureHP.GetSize().x;
		if ((Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BABELTOWER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_BOUNTYHUNT || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_MYTHRAID) && this.m_TargetChar.CastedTarget.Ally == Battle.BATTLE.MyAlly && !this.m_TargetChar.CastedTarget.MyChar)
		{
			this.m_pkDrawTextureHP.SetTexture("Com_T_AllyHpPr");
		}
		for (int i = 0; i < this.MAX_BUFFICON; i++)
		{
			this.m_pkDrawTextureBuffSlot[i] = (base.GetControl("DrawTexture_BuffSlot0" + (i + 1).ToString() + "E") as DrawTexture);
			this.m_pkDrawTextureBuffiCon[i] = (base.GetControl("DrawTexture_BuffIcn0" + (i + 1).ToString() + "E") as DrawTexture);
			this.m_pkDrawTextureBuffStroke[i] = (base.GetControl("DrawTexture_Buffstroke0" + (i + 1).ToString() + "E") as DrawTexture);
			this.m_pkDrawTextureBuffSlot[i].Visible = false;
			this.m_pkDrawTextureBuffiCon[i].Visible = false;
			this.m_pkDrawTextureBuffStroke[i].Visible = false;
		}
		this.UpdateHP();
	}

	public void SetImmuneBuffIcon(bool setImmuneIcon, int nImmuneType)
	{
		if (setImmuneIcon)
		{
			string textureKey = "Main_T_Debuffstroke";
			UIBaseInfoLoader battleSkillBuffImmuneIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBuffImmuneIconTexture(nImmuneType);
			this.m_pkDrawTextureBuffiCon[0].Clear();
			this.m_pkDrawTextureBuffStroke[0].Clear();
			this.m_pkDrawTextureBuffiCon[0].SetTexture(battleSkillBuffImmuneIconTexture);
			this.m_pkDrawTextureBuffiCon[0].Visible = true;
			this.m_pkDrawTextureBuffStroke[0].SetTextureKey(textureKey);
			this.m_pkDrawTextureBuffStroke[0].Visible = true;
			this.m_bImmuneIcon = true;
		}
		else if (this.m_bImmuneIcon)
		{
			this.m_pkDrawTextureBuffiCon[0].Clear();
			this.m_pkDrawTextureBuffiCon[0].Visible = false;
			this.m_pkDrawTextureBuffStroke[0].Clear();
			this.m_pkDrawTextureBuffStroke[0].Visible = false;
			this.m_bImmuneIcon = false;
		}
	}

	public void SetBuffIcon(BATTLESKILL_BUF[] BuffData)
	{
		string text = string.Empty;
		string textureKey = string.Empty;
		int num = 0;
		if (this.m_bImmuneIcon)
		{
			num++;
		}
		for (int i = 0; i < 12; i++)
		{
			if (BuffData[i].BSkillBufSkillUnique > 0)
			{
				BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(BuffData[i].BSkillBufSkillUnique);
				if (battleSkillBase == null)
				{
					return;
				}
				if (BuffData[i].BSkillDeBuff)
				{
					textureKey = "Main_T_buffstroke";
				}
				else
				{
					textureKey = "Main_T_Debuffstroke";
				}
				for (int j = 0; j < 2; j++)
				{
					if (num >= this.MAX_BUFFICON)
					{
						return;
					}
					text = battleSkillBase.m_nBuffIconCode[j];
					if (!(text == string.Empty) && !(text == "0"))
					{
						UIBaseInfoLoader battleSkillBuffIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBuffIconTexture(text);
						this.m_pkDrawTextureBuffiCon[num].Clear();
						this.m_pkDrawTextureBuffStroke[num].Clear();
						this.m_pkDrawTextureBuffiCon[num].SetTexture(battleSkillBuffIconTexture);
						this.m_pkDrawTextureBuffiCon[num].Visible = true;
						this.m_pkDrawTextureBuffStroke[num].SetTextureKey(textureKey);
						this.m_pkDrawTextureBuffStroke[num].Visible = true;
						num++;
					}
				}
			}
		}
	}

	public void ClealBuffIcon()
	{
		for (int i = 0; i < this.MAX_BUFFICON; i++)
		{
			this.m_pkDrawTextureBuffiCon[i].Clear();
			this.m_pkDrawTextureBuffiCon[i].Visible = false;
			this.m_pkDrawTextureBuffStroke[i].Clear();
			this.m_pkDrawTextureBuffStroke[i].Visible = false;
		}
	}

	public void UpdatePosition()
	{
		if (this.m_TargetChar == null)
		{
			return;
		}
		if (!this.m_TargetChar.CastedTarget.IsReady3DModel())
		{
			return;
		}
		if (this.MAXHP != (float)this.m_TargetChar.CastedTarget.GetMaxHP(false))
		{
			this.MAXHP = (float)this.m_TargetChar.CastedTarget.GetMaxHP(false);
		}
		Vector3 pos = Vector3.zero;
		pos = this.m_TargetChar.CastedTarget.Get3DChar().GetRootGameObject().transform.position;
		pos = GUICamera.WorldToEZ(pos);
		pos.x -= base.GetSizeX() / 2f;
		base.SetLocation(pos.x, pos.y);
	}

	public void UpdateHP()
	{
		float num3;
		if (NrTSingleton<MythRaidManager>.Instance.IsMythRaidBossCharKind(this.m_TargetChar.CastedTarget.GetCharKindInfo().GetCharKind()))
		{
			float num = (float)Battle.BATTLE.BossCurrentHP;
			float num2 = (float)Battle.BATTLE.BossMaxHP;
			if (num > num2)
			{
				num = num2;
			}
			num3 = num / num2;
		}
		else
		{
			float num4 = (float)this.m_TargetChar.CastedTarget.GetSoldierInfo().GetHP();
			if (num4 > this.MAXHP)
			{
				num4 = this.MAXHP;
			}
			num3 = num4 / this.MAXHP;
		}
		this.m_pkDrawTextureHP.SetSize(this.fHpLength * num3, this.m_pkDrawTextureHP.GetSize().y);
		if (this.m_TargetChar.CastedTarget.MyChar)
		{
			Battle_CharinfoDlg battle_CharinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CHARINFO_DLG) as Battle_CharinfoDlg;
			if (battle_CharinfoDlg != null)
			{
				battle_CharinfoDlg.UpdateHP((int)this.m_TargetChar.CastedTarget.GetSolIdx());
			}
		}
	}

	public void UpdateHpFromRestart(int nHP)
	{
		float num = (float)this.m_TargetChar.CastedTarget.GetSoldierInfo().GetHP();
		if (num > this.MAXHP)
		{
			num = this.MAXHP;
		}
		this.fAniStartHP = num / this.MAXHP;
		num = (float)nHP;
		this.fAniEndHP = num / this.MAXHP;
		this.fAniStartTime = Time.time;
	}

	public void ChangeColor()
	{
		this.m_pkDrawTextureHP.SetTexture("Com_T_AllyHpPr");
	}

	public void UpdateHPAni()
	{
		if (this.fAniStartTime != 0f)
		{
			if (Time.time - this.fAniStartTime < 1.5f)
			{
				float t = (Time.time - this.fAniStartTime) / 1.5f;
				float num = Mathf.Lerp(this.fAniStartHP, this.fAniEndHP, t);
				this.m_pkDrawTextureHP.SetSize(this.fHpLength * num, this.m_pkDrawTextureHP.GetSize().y);
				if (this.m_TargetChar.CastedTarget.MyChar)
				{
					Battle_CharinfoDlg battle_CharinfoDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CHARINFO_DLG) as Battle_CharinfoDlg;
					if (battle_CharinfoDlg != null)
					{
						battle_CharinfoDlg.UpdateHP((int)this.m_TargetChar.CastedTarget.GetSolIdx());
					}
				}
			}
			else
			{
				this.fAniEndHP = 0f;
				this.fAniStartHP = 0f;
				this.fAniStartTime = 0f;
				this.UpdateHP();
			}
		}
	}
}
