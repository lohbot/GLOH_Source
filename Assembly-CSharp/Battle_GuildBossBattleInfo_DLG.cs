using System;
using UnityEngine;
using UnityForms;

public class Battle_GuildBossBattleInfo_DLG : Form
{
	private Label m_lbRoundNum;

	private Label m_lbRoundNum2;

	private Label m_lbDamageNum;

	private Label m_lbUpperPlayer;

	private Label m_lbUpperDamage;

	private Label m_lBestDamageText;

	private long bossMaxHp;

	private NkBattleChar bossBattleChar;

	private long m_TotalMythRaidDamage;

	private long upperBestDamage;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		base.Scale = true;
		Form form = this;
		instance.LoadFileAll(ref form, "GuildBoss/dlg_guildboss_battleinfo", G_ID.GUILDBOSS_BATTLEINFO_DLG, false);
		form.AlwaysUpdate = true;
		form.TopMost = true;
		base.ShowSceneType = FormsManager.FORM_TYPE_MAIN;
	}

	public override void Show()
	{
		base.Show();
	}

	public override void SetComponent()
	{
		this.m_lbRoundNum = (base.GetControl("LB_Round_Info") as Label);
		this.m_lbRoundNum2 = (base.GetControl("LB_Round_Info_2") as Label);
		this.m_lbDamageNum = (base.GetControl("LB_Damage_Info") as Label);
		this.m_lbRoundNum.SetText("1");
		this.m_lbRoundNum2.SetText("1");
		this.m_lbDamageNum.SetText("0");
		this.TextAniSetting(this.m_lbDamageNum);
		this.m_lbUpperPlayer = (base.GetControl("LB_BestPlayer") as Label);
		this.m_lbUpperDamage = (base.GetControl("LB_BestDamage") as Label);
		this.m_lBestDamageText = (base.GetControl("LB_BestDamage_Text") as Label);
		this.m_lbUpperPlayer.SetText(string.Empty);
		this.m_lbUpperDamage.SetText(string.Empty);
		this.TextAniSetting(this.m_lbUpperDamage);
		if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_GUILD_BOSS || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_NEWEXPLORATION)
		{
			base.SetShowLayer(0, true);
			base.SetShowLayer(1, false);
			base.SetShowLayer(2, false);
		}
		else
		{
			this.Hide();
		}
	}

	public void UpdateRoundInfo(int roundCount)
	{
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2158"),
			"count",
			roundCount.ToString()
		});
		this.m_lbRoundNum.SetText(empty);
	}

	public void UpdateDamageInfo(long addDamageData)
	{
		if (addDamageData > 0L)
		{
			string empty = string.Empty;
			this.m_TotalMythRaidDamage = addDamageData;
			string text = ANNUALIZED.Convert(this.m_TotalMythRaidDamage);
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3133"),
				"damage",
				text
			});
			this.TextUpdateAndPlayAni(this.m_lbDamageNum, empty);
			if (addDamageData >= this.upperBestDamage)
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null)
				{
					this.m_lbUpperPlayer.SetText(@char.GetCharName());
				}
				this.TextUpdateAndPlayAni(this.m_lbUpperDamage, addDamageData.ToString());
			}
		}
	}

	public void UpdateUpperRankerInfo(int _upperRank, long bestDamage, long _upperRankerPersonID)
	{
		base.SetShowLayer(1, true);
		string textFromInterface = NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("3097");
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			textFromInterface,
			"count",
			_upperRank.ToString()
		});
		this.m_lBestDamageText.SetText(empty);
		if (this.upperBestDamage >= bestDamage)
		{
			return;
		}
		NewGuildMember memberInfoFromPersonID = NrTSingleton<NewGuildManager>.Instance.GetMemberInfoFromPersonID(_upperRankerPersonID);
		if (memberInfoFromPersonID == null)
		{
			return;
		}
		this.m_lbUpperPlayer.SetText(memberInfoFromPersonID.GetCharName());
		string damage = string.Empty;
		if (bestDamage <= 0L)
		{
			damage = "-";
		}
		else
		{
			damage = bestDamage.ToString();
		}
		this.upperBestDamage = bestDamage;
		this.TextUpdateAndPlayAni(this.m_lbUpperDamage, damage);
	}

	public void SetBossData(NkBattleChar nBossChar)
	{
		if (nBossChar != null && nBossChar.IsCharKindATB(128L))
		{
			this.bossBattleChar = nBossChar;
		}
		if (this.bossBattleChar != null)
		{
			this.bossMaxHp = (long)this.bossBattleChar.GetSoldierInfo().GetHP();
			this.Show();
		}
	}

	public void UpdateDamage()
	{
		if (this.bossBattleChar == null)
		{
			return;
		}
		long addDamageData = this.bossMaxHp - (long)this.bossBattleChar.GetSoldierInfo().GetHP();
		this.UpdateDamageInfo(addDamageData);
	}

	private void TextAniSetting(Label text)
	{
		if (text == null)
		{
			Debug.LogError("ERROR, Battle_MythRaidBattleInfo_DLG.cs, TextAniSetting(), text is Null");
			return;
		}
		UILabelStepByStepAni uILabelStepByStepAni = text.transform.gameObject.AddComponent<UILabelStepByStepAni>();
		uILabelStepByStepAni._loopTime = -1f;
		uILabelStepByStepAni._loopInterval = 0.01f;
		uILabelStepByStepAni._nextValueStopInterval = 0.5f;
		uILabelStepByStepAni._reverse = true;
		uILabelStepByStepAni._changePartUpdate = true;
		uILabelStepByStepAni._useComma = true;
	}

	private void TextUpdateAndPlayAni(Label text, string damage)
	{
		UILabelStepByStepAni component = text.GetComponent<UILabelStepByStepAni>();
		if (component == null)
		{
			Debug.LogError("ERROR, Battle_MythRaidBattleInfo_DLG.cs, TextUpdateAndPlayAni(), textAni is Null");
			return;
		}
		component.Clear();
		text.SetText(damage);
	}
}
