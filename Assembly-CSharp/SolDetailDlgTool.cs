using GAME;
using System;
using UnityEngine;
using UnityForms;

public class SolDetailDlgTool
{
	public const int EventSTATISTICS_NUM = 2;

	public NrCharKindInfo m_kSelectCharKindInfo;

	public DrawTexture m_DrawTexture_Character;

	public DrawTexture m_DrawTextureWeapon;

	public Label m_Label_Character_Name;

	public Label m_Label_SeasonNum;

	public DrawTexture m_DrawTexture_rank;

	public Label m_Label_Rank2;

	public DrawTexture m_DrawTexture_Event;

	public Label m_Label_EventDate;

	public Label[] m_Lebel_EventHero = new Label[2];

	public DrawTexture m_DrawTextureSkillIcon;

	public Label m_Label_SkillAnger;

	public Label m_Label_SkillName;

	public ScrollLabel m_ScrollLabel_SkillInfo;

	public void SetSeason(byte bGrade)
	{
		if (this.m_kSelectCharKindInfo == null)
		{
			return;
		}
		int season = this.m_kSelectCharKindInfo.GetSeason(bGrade);
		string empty = string.Empty;
		NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
		{
			NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1208"),
			"count",
			season + 1
		});
		this.m_Label_SeasonNum.SetText(empty);
	}

	public void SetCharImg(byte bGrade, string costumePortraitPath = "")
	{
		if (this.m_kSelectCharKindInfo == null)
		{
			return;
		}
		this.SetSeason(bGrade);
		string legendName = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendName(this.m_kSelectCharKindInfo.GetCharKind(), (int)bGrade, this.m_kSelectCharKindInfo.GetName());
		this.m_Label_Character_Name.SetText(legendName);
		short legendType = NrTSingleton<NrCharKindInfoManager>.Instance.GetLegendType(this.m_kSelectCharKindInfo.GetCharKind(), (int)bGrade);
		this.m_DrawTexture_Character.SetTextureEffect(eCharImageType.LARGE, this.m_kSelectCharKindInfo.GetCharKind(), (int)bGrade, costumePortraitPath);
		UIBaseInfoLoader solLargeGradeImg = NrTSingleton<NrCharKindInfoManager>.Instance.GetSolLargeGradeImg(this.m_kSelectCharKindInfo.GetCharKind(), (int)bGrade);
		if (0 < legendType)
		{
			this.m_DrawTexture_rank.SetSize(solLargeGradeImg.UVs.width, solLargeGradeImg.UVs.height);
		}
		this.m_DrawTexture_rank.SetTexture(solLargeGradeImg);
		if (this.m_DrawTextureWeapon != null)
		{
			int weaponType = this.m_kSelectCharKindInfo.GetWeaponType();
			this.m_DrawTextureWeapon.SetTexture(string.Format("Win_I_Weapon{0}", weaponType.ToString()));
		}
	}

	public BATTLESKILL_BASE SetSkillIcon()
	{
		if (this.m_kSelectCharKindInfo == null)
		{
			return null;
		}
		SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(this.m_kSelectCharKindInfo.GetCharKind());
		BATTLESKILL_BASE battleSkillBase = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(solGuild.m_i32SkillUnique);
		if (this.m_DrawTextureSkillIcon != null)
		{
			if (battleSkillBase != null)
			{
				UIBaseInfoLoader battleSkillIconTexture = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillIconTexture(battleSkillBase.m_nSkillUnique);
				this.m_DrawTextureSkillIcon.SetTexture(battleSkillIconTexture);
			}
			else
			{
				this.m_DrawTextureSkillIcon.SetTexture(string.Empty);
			}
		}
		return battleSkillBase;
	}

	public void SetSkillInfo_MaxLevel()
	{
		BATTLESKILL_BASE bATTLESKILL_BASE = this.SetSkillIcon();
		if (bATTLESKILL_BASE == null)
		{
			return;
		}
		if (this.m_Label_SkillName != null)
		{
			this.m_Label_SkillName.SetText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface(bATTLESKILL_BASE.m_strTextKey));
		}
		if (this.m_Label_SkillAnger != null)
		{
			BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(bATTLESKILL_BASE.m_nSkillUnique, bATTLESKILL_BASE.m_nSkillMaxLevel);
			if (battleSkillDetail != null)
			{
				string empty = string.Empty;
				NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
				{
					NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2153"),
					"count",
					battleSkillDetail.m_nSkillNeedAngerlyPoint.ToString()
				});
				this.m_Label_SkillAnger.SetText(empty);
			}
		}
		if (this.m_ScrollLabel_SkillInfo != null)
		{
			SOL_GUIDE solGuild = NrTSingleton<NrTableSolGuideManager>.Instance.GetSolGuild(this.m_kSelectCharKindInfo.GetCharKind());
			this.m_ScrollLabel_SkillInfo.SetScrollLabel(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface((solGuild.m_i32SkillText + 30000).ToString()));
		}
	}

	public void SetHeroEventLabel(byte bGrade)
	{
		if (this.m_kSelectCharKindInfo == null)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			this.m_Lebel_EventHero[i].Hide(true);
		}
		EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(this.m_kSelectCharKindInfo.GetCharKind(), bGrade - 1);
		if (eventHeroCharCode == null)
		{
			this.m_Label_EventDate.Visible = false;
			this.m_DrawTexture_Event.Visible = false;
			return;
		}
		string arg = string.Format("?/?", new object[0]);
		if (eventHeroCharCode.tEndTime > eventHeroCharCode.tStartTime)
		{
			arg = string.Format("{0}/{1}", eventHeroCharCode.tEndTime.Month.ToString(), eventHeroCharCode.tEndTime.Day.ToString());
		}
		string text = string.Format("{0}/{1} ~ {2}", eventHeroCharCode.tStartTime.Month.ToString(), eventHeroCharCode.tStartTime.Day.ToString(), arg);
		this.m_Label_EventDate.Visible = false;
		int num = 0;
		if (eventHeroCharCode.i32Attack != 100)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2191"),
				"count",
				eventHeroCharCode.i32Attack.ToString()
			});
			this.m_Lebel_EventHero[num].SetText(text);
			this.m_Lebel_EventHero[num].Hide(false);
			num++;
		}
		if (eventHeroCharCode.i32Hp != 100)
		{
			NrTSingleton<CTextParser>.Instance.ReplaceParam(ref text, new object[]
			{
				NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("2192"),
				"count",
				eventHeroCharCode.i32Hp.ToString()
			});
			text = NrTSingleton<UIDataManager>.Instance.GetString("H P : ", eventHeroCharCode.i32Hp.ToString(), "%");
			this.m_Lebel_EventHero[num].SetText(text);
			this.m_Lebel_EventHero[num].Hide(false);
		}
		this.m_DrawTexture_Event.Visible = true;
		Transform child = NkUtil.GetChild(this.m_DrawTexture_Event.gameObject.transform, "child_effect");
		if (child == null)
		{
			NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_EVENTFONT", this.m_DrawTexture_Event, this.m_DrawTexture_Event.GetSize());
		}
	}
}
