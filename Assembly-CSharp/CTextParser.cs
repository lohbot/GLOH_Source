using GAME;
using System;
using System.Collections.Generic;
using System.Text;
using UnityForms;

public class CTextParser : NrTSingleton<CTextParser>
{
	private enum E_TAG_TYPE
	{
		OPEN_ANGLE,
		CLOSE_ANGLE,
		OPEN_BIGANGLE,
		CLOSE_BIGANGLE,
		A,
		B,
		C,
		CROSSHATCH,
		SLASH,
		BACKSLASH_N,
		N,
		BACKSLASH,
		R,
		AT,
		AMPERSAND,
		UNDER_BAR,
		PLUS,
		G
	}

	private const int MAX_LENGTH = 128;

	private char[] m_szTag = new char[]
	{
		'{',
		'}',
		'[',
		']',
		'a',
		'b',
		'C',
		'#',
		'/',
		'\n',
		'n',
		'\\',
		'r',
		'@',
		'$',
		'_',
		'+',
		'G'
	};

	private Dictionary<string, string> m_ColorMap = new Dictionary<string, string>();

	private StringBuilder replacedContent = new StringBuilder(128);

	private StringBuilder ParamTypeBuilder = new StringBuilder(128);

	private StringBuilder ParamCodeBuilder = new StringBuilder(128);

	private StringBuilder reaplacedParam = new StringBuilder(128);

	private StringBuilder willReplaceParamBuilder = new StringBuilder(128);

	private StringBuilder contentBuilder = new StringBuilder(128);

	private StringBuilder paramValueBuilder = new StringBuilder(128);

	private CTextParser()
	{
	}

	internal bool SetData(FONT_COLOR fontColor)
	{
		if (this.m_ColorMap.ContainsKey(fontColor.ColorCode))
		{
			return false;
		}
		this.m_ColorMap.Add(fontColor.ColorCode, fontColor.ColorText);
		return true;
	}

	public void ReplaceBattleSkillParam(ref string refReplaceText, string BaseString, BATTLESKILL_DETAIL BSkillDetail, NkSoldierInfo solInfo, int costumeUnique = -1)
	{
		if (BSkillDetail == null)
		{
			return;
		}
		refReplaceText = string.Empty;
		int i = 0;
		this.replacedContent.Length = 0;
		while (i < BaseString.Length)
		{
			if (BaseString[i] == this.m_szTag[2])
			{
				if (BaseString[i + 1] == this.m_szTag[7])
				{
					this.replacedContent.Append(BaseString[i]);
					this.replacedContent.Append(BaseString[i + 1]);
					i++;
				}
				else
				{
					this.replacedContent.Append(BaseString[i]);
				}
			}
			else if (BaseString[i] == this.m_szTag[13])
			{
				this.ReplaceParamValBattleSkill(ref i, ref this.replacedContent, BaseString, BSkillDetail, solInfo, costumeUnique);
			}
			else if (BaseString[i] == this.m_szTag[14])
			{
				this.ReplaceName(ref i, ref this.replacedContent, BaseString);
			}
			else if (BaseString[i] == this.m_szTag[0])
			{
				if (BaseString[i + 1] == this.m_szTag[4])
				{
					this.CheckAuxiliaryWord(ref i, ref this.replacedContent, BaseString);
				}
				else if (BaseString[i + 1] == this.m_szTag[5])
				{
					this.CheckAuxiliaryWord2(ref i, ref this.replacedContent, BaseString);
				}
				else if (BaseString[i + 1] == this.m_szTag[17])
				{
					this.ReplaceGender(ref i, ref this.replacedContent, BaseString);
				}
				else if (BaseString[i + 1] == this.m_szTag[13])
				{
					this.replacedContent.Append(BaseString[i]);
					this.replacedContent.Append(BaseString[i + 1]);
					this.replacedContent.Append(BaseString[i + 2]);
					i++;
					i++;
					i++;
					this.ReplaceParamValBattleSkill(ref i, ref this.replacedContent, BaseString, BSkillDetail, solInfo, costumeUnique);
				}
				else
				{
					this.replacedContent.Append(BaseString[i]);
				}
			}
			else if (BaseString[i] == this.m_szTag[8])
			{
				if (BaseString[i + 1] == this.m_szTag[8])
				{
					this.replacedContent.Append(BaseString[i + 1]);
					i++;
				}
				else
				{
					this.replacedContent.Append(BaseString[i + 1]);
					i++;
				}
			}
			else
			{
				this.replacedContent.Append(BaseString[i]);
			}
			i++;
		}
		refReplaceText = this.replacedContent.ToString();
	}

	public void ReplaceParam(ref string refReplaceText, params object[] aParam)
	{
		refReplaceText = string.Empty;
		string text = aParam[0] as string;
		int i = 0;
		this.replacedContent.Length = 0;
		while (i < text.Length)
		{
			if (text[i] == this.m_szTag[2])
			{
				if (text[i + 1] == this.m_szTag[7])
				{
					this.replacedContent.Append(text[i]);
					this.replacedContent.Append(text[i + 1]);
					i++;
				}
				else
				{
					this.replacedContent.Append(text[i]);
				}
			}
			else if (text[i] == this.m_szTag[13])
			{
				this.ReplaceParamVal(ref i, ref this.replacedContent, text, aParam);
			}
			else if (text[i] == this.m_szTag[14])
			{
				this.ReplaceName(ref i, ref this.replacedContent, text);
			}
			else if (text[i] == this.m_szTag[0])
			{
				if (text[i + 1] == this.m_szTag[4])
				{
					this.CheckAuxiliaryWord(ref i, ref this.replacedContent, text);
				}
				else if (text[i + 1] == this.m_szTag[5])
				{
					this.CheckAuxiliaryWord2(ref i, ref this.replacedContent, text);
				}
				else if (text[i + 1] == this.m_szTag[17])
				{
					this.ReplaceGender(ref i, ref this.replacedContent, text);
				}
				else if (text[i + 1] == this.m_szTag[13])
				{
					this.replacedContent.Append(text[i]);
					this.replacedContent.Append(text[i + 1]);
					this.replacedContent.Append(text[i + 2]);
					i++;
					i++;
					i++;
					this.ReplaceParamVal(ref i, ref this.replacedContent, text, aParam);
				}
				else
				{
					this.replacedContent.Append(text[i]);
				}
			}
			else if (text[i] == this.m_szTag[8])
			{
				if (text[i + 1] == this.m_szTag[8])
				{
					this.replacedContent.Append(text[i + 1]);
					i++;
				}
				else
				{
					this.replacedContent.Append(text[i + 1]);
					i++;
				}
			}
			else
			{
				this.replacedContent.Append(text[i]);
			}
			i++;
		}
		refReplaceText = this.replacedContent.ToString();
	}

	private void ReplaceName(ref int m_iTextCount, ref StringBuilder refReplaceText, string szContent)
	{
		int num = 0;
		int i = m_iTextCount + 1;
		int length = szContent.Length;
		if (i >= length)
		{
			TsLog.LogWarning("TextParser.ReplaceName({0},$ReplaceText,\"{1}\") => szContent size over!", new object[]
			{
				m_iTextCount,
				szContent
			});
			return;
		}
		this.ParamTypeBuilder.Length = 0;
		while (i < length)
		{
			num++;
			if (szContent[i] == this.m_szTag[15])
			{
				break;
			}
			this.ParamTypeBuilder.Append(szContent[i]);
			i++;
		}
		this.ParamCodeBuilder.Length = 0;
		while (i < length)
		{
			num++;
			if (szContent[i] == this.m_szTag[14])
			{
				break;
			}
			this.ParamCodeBuilder.Append(szContent[i]);
			i++;
		}
		string text = this.ParamTypeBuilder.ToString();
		string text2 = this.ParamCodeBuilder.ToString();
		string text3 = string.Empty;
		string text4 = text;
		switch (text4)
		{
		case "CHARCODE":
		{
			NrCharKindInfo charKindInfoFromCode = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(text2);
			if (charKindInfoFromCode == null)
			{
				text3 = string.Empty;
			}
			else
			{
				text3 = charKindInfoFromCode.GetName();
			}
			break;
		}
		case "CHARCODEMONSTER":
		{
			NrCharKindInfo charKindInfoFromCode2 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(text2);
			if (charKindInfoFromCode2 == null)
			{
				text3 = string.Empty;
			}
			else
			{
				string textColor = this.GetTextColor("1108");
				text3 = NrTSingleton<UIDataManager>.Instance.GetString(textColor, charKindInfoFromCode2.GetName());
			}
			break;
		}
		case "CHARCODENPC":
		{
			NrCharKindInfo charKindInfoFromCode3 = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfoFromCode(text2);
			if (charKindInfoFromCode3 == null)
			{
				text3 = string.Empty;
			}
			else
			{
				string textColor2 = this.GetTextColor("1105");
				text3 = NrTSingleton<UIDataManager>.Instance.GetString(textColor2, charKindInfoFromCode3.GetName());
			}
			break;
		}
		case "ITEM":
		{
			int itemunique;
			int.TryParse(text2, out itemunique);
			text3 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemunique);
			break;
		}
		case "USERNAME":
		{
			NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
			if (nrCharUser != null)
			{
				string charName = nrCharUser.GetCharName();
				text3 = charName;
			}
			else
			{
				text3 = "NoName";
			}
			break;
		}
		case "ITEMUNIQUE":
		{
			int itemunique2;
			int.TryParse(text2, out itemunique2);
			text3 = NrTSingleton<ItemManager>.Instance.GetItemNameByItemUnique(itemunique2);
			string textColor3 = this.GetTextColor("1104");
			text3 = NrTSingleton<UIDataManager>.Instance.GetString(textColor3, text3);
			break;
		}
		}
		refReplaceText.Append(text3);
		m_iTextCount += num - 1;
	}

	public void ReplaceParamColor(out string outReplaceText, params string[] stringParams)
	{
		if (stringParams.Length < 1)
		{
			TsLog.LogWarning("TextParser.ReplaceParamColor() => failed : aParam length is zero.", new object[0]);
			outReplaceText = string.Empty;
			return;
		}
		string text = stringParams[0];
		this.reaplacedParam.Length = 0;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == this.m_szTag[0] && text[i + 1] == this.m_szTag[6])
			{
				this.ChangeColor(ref i, ref this.reaplacedParam, text);
			}
			else
			{
				this.reaplacedParam.Append(text[i]);
			}
		}
		outReplaceText = this.reaplacedParam.ToString();
	}

	private void ReplaceParamValBattleSkill(ref int m_iTextCount, ref StringBuilder refReplaceText, string szContent, BATTLESKILL_DETAIL BSkillDetail, NkSoldierInfo solInfo, int costumeUnique = -1)
	{
		int length = szContent.Length;
		int i = m_iTextCount + 1;
		this.willReplaceParamBuilder.Length = 0;
		while (i < length)
		{
			if (szContent[i] == this.m_szTag[13])
			{
				break;
			}
			this.willReplaceParamBuilder.Append(szContent[i]);
			i++;
		}
		string text = this.willReplaceParamBuilder.ToString();
		int paramType = NrTSingleton<BATTLE_SKILL_PARSER>.Instance.GetParamType(text);
		int skillDetalParamValue = BSkillDetail.GetSkillDetalParamValue(paramType);
		float num = 0f;
		long num2 = 0L;
		if (skillDetalParamValue != 0)
		{
			eBATTLESKILL_DETAIL_CODE eBATTLESKILL_DETAIL_CODE = (eBATTLESKILL_DETAIL_CODE)paramType;
			switch (eBATTLESKILL_DETAIL_CODE)
			{
			case eBATTLESKILL_DETAIL_CODE.ADD_CRITICAL:
			case eBATTLESKILL_DETAIL_CODE.ADD_PHY_DEFENSE_P:
			case eBATTLESKILL_DETAIL_CODE.ADD_MAG_DEFENSE_P:
			case eBATTLESKILL_DETAIL_CODE.ADD_HIT_RATE_P:
			case eBATTLESKILL_DETAIL_CODE.ADD_EVASION_P:
			case eBATTLESKILL_DETAIL_CODE.ADD_CRITICAL_P:
			case eBATTLESKILL_DETAIL_CODE.ADD_MIN_DAMAGE_P:
			case eBATTLESKILL_DETAIL_CODE.ADD_MAX_DAMAGE_P:
			case eBATTLESKILL_DETAIL_CODE.ADD_MAX_HP_P:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MIN_DAMAGE_P:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MAX_DAMAGE_P:
			case eBATTLESKILL_DETAIL_CODE.MINUS_PHY_DEFENSE_P:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MAG_DEFENSE_P:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MAX_HP_P:
			case eBATTLESKILL_DETAIL_CODE.MINUS_CRITICAL:
			case eBATTLESKILL_DETAIL_CODE.MINUS_HEAL_P:
			case eBATTLESKILL_DETAIL_CODE.PLUS_HEAL_P:
			case eBATTLESKILL_DETAIL_CODE.MINUS_DAMAGE_P:
			case eBATTLESKILL_DETAIL_CODE.PLUS_DAMAGE_P:
				goto IL_2C3;
			case eBATTLESKILL_DETAIL_CODE.ADD_MIN_DAMAGE:
			case eBATTLESKILL_DETAIL_CODE.ADD_MAX_DAMAGE:
			case eBATTLESKILL_DETAIL_CODE.ADD_MAX_HP:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MIN_DAMAGE:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MAX_DAMAGE:
			case eBATTLESKILL_DETAIL_CODE.MINUS_PHY_DEFENSE:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MAG_DEFENSE:
			case eBATTLESKILL_DETAIL_CODE.MINUS_MAX_HP:
			case eBATTLESKILL_DETAIL_CODE.NOMOVE_ON:
			case eBATTLESKILL_DETAIL_CODE.NOTURN_ON:
			case eBATTLESKILL_DETAIL_CODE.NOSKILL_ON:
			case eBATTLESKILL_DETAIL_CODE.NOATTACK_ON:
			case eBATTLESKILL_DETAIL_CODE.IMMORTAL_ON:
			case eBATTLESKILL_DETAIL_CODE.NOCONTROL_ON:
			case eBATTLESKILL_DETAIL_CODE.NOCONTROL_AND_NOTARGETING:
			case eBATTLESKILL_DETAIL_CODE.NONORMAL_ATTACK_ON:
			case eBATTLESKILL_DETAIL_CODE.MINUS_HEAL:
			case eBATTLESKILL_DETAIL_CODE.PLUS_HEAL:
			case eBATTLESKILL_DETAIL_CODE.MINUS_DAMAGE:
			case eBATTLESKILL_DETAIL_CODE.PLUS_DAMAGE:
				IL_13A:
				switch (eBATTLESKILL_DETAIL_CODE)
				{
				case eBATTLESKILL_DETAIL_CODE.EMPTY_P:
				case eBATTLESKILL_DETAIL_CODE.INCREASE_WEPON_ACTIVE:
				case eBATTLESKILL_DETAIL_CODE.DECREASE_PVP_DAMAGE:
				case eBATTLESKILL_DETAIL_CODE.INCREASE_PVP_DAMAGE:
				case eBATTLESKILL_DETAIL_CODE.INCREASE_BOSS_DAMAGE:
				case eBATTLESKILL_DETAIL_CODE.DECREASE_MONSTER_DAMAGE:
				case eBATTLESKILL_DETAIL_CODE.INCREASE_EXP_P:
				case eBATTLESKILL_DETAIL_CODE.INCREASE_GOLD_P:
				case eBATTLESKILL_DETAIL_CODE.INCREASE_ITEM_P:
				case eBATTLESKILL_DETAIL_CODE.PHOENIX:
				case eBATTLESKILL_DETAIL_CODE.EMPTY_P2:
					goto IL_2C3;
				case eBATTLESKILL_DETAIL_CODE.EMPTY_DAMAGE_P:
				case eBATTLESKILL_DETAIL_CODE.EMPTY_DAMAGE_P2:
					goto IL_230;
				case eBATTLESKILL_DETAIL_CODE.IMMUNE_EQUAL_BOSS:
				case eBATTLESKILL_DETAIL_CODE.INCREASE_DEFENCE_ACTIVE:
				case eBATTLESKILL_DETAIL_CODE.DECREASE_CRITICAL_HIT:
				case eBATTLESKILL_DETAIL_CODE.MAGIC_RESIST:
				case eBATTLESKILL_DETAIL_CODE.PHYSICAL_RESIST:
				case eBATTLESKILL_DETAIL_CODE.EMPTY_VALUE2:
					IL_190:
					switch (eBATTLESKILL_DETAIL_CODE)
					{
					case eBATTLESKILL_DETAIL_CODE.ENDURE_DAMAGE_POISON_P:
					case eBATTLESKILL_DETAIL_CODE.ENDURE_DAMAGE_FIRE_P:
					case eBATTLESKILL_DETAIL_CODE.ENDURE_DAMAGE_ICE_P:
					case eBATTLESKILL_DETAIL_CODE.ENDURE_DAMAGE_LIGHTNING_P:
					case eBATTLESKILL_DETAIL_CODE.ENDURE_DAMAGE_BLEEDING_P:
					case eBATTLESKILL_DETAIL_CODE.ENDURE_HEAL_P:
					case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE_P:
						goto IL_230;
					case eBATTLESKILL_DETAIL_CODE.ENDURE_HEAL:
					case eBATTLESKILL_DETAIL_CODE.ENDURE_ANGERLYPOINT:
					case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE:
					case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE_TYPE:
					case eBATTLESKILL_DETAIL_CODE.AFTER_USED_DAMAGE_TARGET:
					case eBATTLESKILL_DETAIL_CODE.AFTER_USED_HEAL:
					case eBATTLESKILL_DETAIL_CODE.AFTER_USED_SKILL_LEVEL:
					case eBATTLESKILL_DETAIL_CODE.DEL_BUFF_ALL:
					case eBATTLESKILL_DETAIL_CODE.DEL_BUFF_TYPE:
						IL_1E2:
						switch (eBATTLESKILL_DETAIL_CODE)
						{
						case eBATTLESKILL_DETAIL_CODE.BLOOD_SUCKING_PER:
						case eBATTLESKILL_DETAIL_CODE.ADD_BLOOD_SUCKING_PER:
						case eBATTLESKILL_DETAIL_CODE.BLOOD_SUCKING_ALL_PER:
							goto IL_2C3;
						case eBATTLESKILL_DETAIL_CODE.BLOOD_SUCKING_VALUE:
						case eBATTLESKILL_DETAIL_CODE.ADD_BLOOD_SUCKING_VALUE:
							IL_200:
							if (eBATTLESKILL_DETAIL_CODE == eBATTLESKILL_DETAIL_CODE.DAMAGE_P || eBATTLESKILL_DETAIL_CODE == eBATTLESKILL_DETAIL_CODE.HEAL_P)
							{
								goto IL_230;
							}
							if (eBATTLESKILL_DETAIL_CODE == eBATTLESKILL_DETAIL_CODE.SUMMON_LEVEL_P)
							{
								goto IL_2C3;
							}
							if (eBATTLESKILL_DETAIL_CODE == eBATTLESKILL_DETAIL_CODE.PROTECT_SHIELD_VALUE_P)
							{
								goto IL_230;
							}
							if (eBATTLESKILL_DETAIL_CODE != eBATTLESKILL_DETAIL_CODE.PLUNDER_ANGERLYPOINT_P)
							{
								goto IL_385;
							}
							goto IL_2C3;
						}
						goto IL_200;
					case eBATTLESKILL_DETAIL_CODE.AFTER_USED_SKILL:
					{
						int skillDetalParamValue2 = BSkillDetail.GetSkillDetalParamValue(74);
						BATTLESKILL_DETAIL battleSkillDetail = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillDetail(skillDetalParamValue, skillDetalParamValue2);
						if (battleSkillDetail != null)
						{
							skillDetalParamValue = BSkillDetail.GetSkillDetalParamValue(3);
							if (skillDetalParamValue > 0)
							{
								if (solInfo != null)
								{
									int num3 = (solInfo.GetMaxDamage() + solInfo.GetMinDamage()) / 2;
									EVENT_HERODATA eventHeroCharCode = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(solInfo.GetCharKind(), solInfo.GetGrade());
									if (eventHeroCharCode != null)
									{
										int num4 = (int)((float)num3 * ((float)eventHeroCharCode.i32Attack * 0.01f));
										num3 = num4;
									}
									num2 = (long)((int)((float)num3 * ((float)skillDetalParamValue / 10000f)));
								}
								else
								{
									num = (float)skillDetalParamValue / 100f;
								}
							}
						}
						goto IL_385;
					}
					case eBATTLESKILL_DETAIL_CODE.ACTION_HP_P:
						goto IL_2C3;
					}
					goto IL_1E2;
				}
				goto IL_190;
			case eBATTLESKILL_DETAIL_CODE.PLUS_MIN_DAMAGE_P:
			case eBATTLESKILL_DETAIL_CODE.PLUS_MAX_DAMAGE_P:
				goto IL_230;
			}
			goto IL_13A;
			IL_230:
			if (solInfo != null)
			{
				int num5 = (solInfo.GetMaxDamage() + solInfo.GetMinDamage()) / 2;
				if (0 <= costumeUnique)
				{
					num5 = this.GetCostumeDamage(solInfo, costumeUnique);
				}
				EVENT_HERODATA eventHeroCharCode2 = NrTSingleton<NrTableEvnetHeroManager>.Instance.GetEventHeroCharCode(solInfo.GetCharKind(), solInfo.GetGrade());
				if (eventHeroCharCode2 != null)
				{
					int num6 = (int)((float)num5 * ((float)eventHeroCharCode2.i32Attack * 0.01f));
					num5 = num6;
				}
				num2 = (long)num5 * (long)skillDetalParamValue;
				num2 /= 10000L;
			}
			else
			{
				num = (float)skillDetalParamValue / 100f;
			}
			goto IL_385;
			IL_2C3:
			num = (float)skillDetalParamValue / 100f;
			IL_385:
			string value;
			if (num2 > 0L)
			{
				value = num2.ToString();
			}
			else if (num > 0f)
			{
				value = num.ToString();
			}
			else
			{
				value = skillDetalParamValue.ToString();
			}
			refReplaceText.Append(value);
		}
		m_iTextCount += text.Length + 1;
	}

	private void ReplaceParamVal(ref int m_iTextCount, ref StringBuilder refReplaceText, string szContent, params object[] aParam)
	{
		int length = szContent.Length;
		int i = m_iTextCount + 1;
		this.willReplaceParamBuilder.Length = 0;
		while (i < length)
		{
			if (szContent[i] == this.m_szTag[13])
			{
				break;
			}
			this.willReplaceParamBuilder.Append(szContent[i]);
			i++;
		}
		string text = this.willReplaceParamBuilder.ToString();
		for (int j = 1; j < aParam.Length; j++)
		{
			string a;
			if (aParam[j] == null)
			{
				a = "null";
			}
			else if (aParam[j] is string)
			{
				a = (aParam[j] as string);
			}
			else
			{
				a = aParam[j].ToString();
			}
			if (string.Equals(a, text))
			{
				string value;
				if (aParam[j + 1] == null)
				{
					value = "null";
				}
				else if (aParam[j + 1] is string)
				{
					value = (aParam[j + 1] as string);
				}
				else
				{
					value = aParam[j + 1].ToString();
				}
				refReplaceText.Append(value);
				break;
			}
		}
		m_iTextCount += text.Length + 1;
	}

	private void ChangeColor(ref int m_iTextCount, ref StringBuilder refReplaceText, string content)
	{
		int num = 0;
		int num2 = m_iTextCount + 2;
		int startIndex = num2;
		int length = content.Length;
		while (num2 < length && num < 4)
		{
			if (content[num2] == this.m_szTag[1])
			{
				break;
			}
			num++;
			num2++;
		}
		string szColorNum = new string(content.ToCharArray(startIndex, num));
		string textColor = this.GetTextColor(szColorNum);
		refReplaceText.Append(textColor);
		m_iTextCount += num + 2;
	}

	private void CheckAuxiliaryWord(ref int m_iTextCount, ref StringBuilder refReplaceText, string szContent)
	{
		if (0 >= refReplaceText.Length)
		{
			m_iTextCount += 5;
			refReplaceText.Append(szContent[m_iTextCount]);
			return;
		}
		char c = refReplaceText[refReplaceText.Length - 1];
		if (c < '가' || c > '힟')
		{
			m_iTextCount += 5;
			refReplaceText.Append(szContent[m_iTextCount]);
			return;
		}
		uint num = Convert.ToUInt32((int)(c - '가'));
		if (num % 28u == 0u)
		{
			m_iTextCount += 5;
			refReplaceText.Append(szContent[m_iTextCount]);
		}
		else
		{
			m_iTextCount += 3;
			refReplaceText.Append(szContent[m_iTextCount]);
			m_iTextCount += 2;
		}
	}

	private void CheckAuxiliaryWord2(ref int m_iTextCount, ref StringBuilder refReplaceText, string szContent)
	{
		if (0 >= refReplaceText.Length)
		{
			m_iTextCount += 5;
			refReplaceText.Append(szContent[m_iTextCount]);
			return;
		}
		char c = refReplaceText[refReplaceText.Length - 1];
		if (c < '가' || c > '힟')
		{
			m_iTextCount += 5;
			refReplaceText.Append(szContent[m_iTextCount]);
			return;
		}
		uint num = Convert.ToUInt32((int)(c - '가'));
		int num2 = (int)(num % 28u);
		if (num2 == 0 || num2 == 8)
		{
			m_iTextCount += 6;
			refReplaceText.Append(szContent[m_iTextCount]);
		}
		else
		{
			m_iTextCount += 3;
			refReplaceText.Append(szContent[m_iTextCount]);
			m_iTextCount += 2;
		}
	}

	public string GetTextColor(string szColorNum)
	{
		if (this.m_ColorMap.ContainsKey(szColorNum))
		{
			return this.m_ColorMap[szColorNum];
		}
		return "[#D6D0C5FF]";
	}

	public bool ChangeQuestConditionText(string strConText, out string refTextValue)
	{
		if (strConText.Length <= 40)
		{
			refTextValue = strConText;
			return false;
		}
		this.contentBuilder.Length = 0;
		string text = string.Empty;
		int num = 0;
		bool flag = false;
		bool result = false;
		while (strConText.Length > num)
		{
			if (strConText[num] == '(' && !flag)
			{
				text = this.contentBuilder.ToString();
				this.contentBuilder.Length = 0;
				flag = true;
			}
			this.contentBuilder.Append(strConText[num]);
			num++;
		}
		if (flag)
		{
			bool flag2 = false;
			if (text.Length > 40)
			{
				text.Remove(40);
				flag2 = true;
			}
			refTextValue = NrTSingleton<UIDataManager>.Instance.GetString(text, (!flag2) ? string.Empty : "…", this.contentBuilder.ToString());
		}
		else
		{
			if (this.contentBuilder.Length > 40)
			{
				this.contentBuilder.Remove(40, this.contentBuilder.Length - 40);
				this.contentBuilder.Append("…");
			}
			refTextValue = this.contentBuilder.ToString();
		}
		return result;
	}

	public bool ParsingQuestDlg(ref QUEST_DLG_INFO info, string strInfo)
	{
		int i = 0;
		int num = 0;
		this.paramValueBuilder.Length = 0;
		bool flag = false;
		while (i < strInfo.Length)
		{
			if (strInfo[i] == this.m_szTag[16])
			{
				string text = this.paramValueBuilder.ToString();
				switch (num)
				{
				case 0:
					info.strDialogUnique = NrTSingleton<UIDataManager>.Instance.GetString(info.strDialogUnique, text);
					break;
				case 1:
					info.strDialogUnique = NrTSingleton<UIDataManager>.Instance.GetString(info.strDialogUnique, text);
					break;
				case 2:
					int.TryParse(text, out info.i32OrderUnique);
					break;
				case 3:
					if (text.Contains("&"))
					{
						int startIndex = text.IndexOf("&");
						text = text.Remove(startIndex);
						info.bTalkUser = true;
					}
					if (text.Contains("*"))
					{
						char[] separator = new char[]
						{
							'*'
						};
						string[] array = text.Split(separator);
						if (2 <= array.Length)
						{
							info.nNpcName = int.Parse(array[1]);
						}
						text = array[0];
					}
					if (text.Contains("#"))
					{
						char[] separator2 = new char[]
						{
							'#'
						};
						string[] array2 = text.Split(separator2);
						if (1 <= array2.Length)
						{
							char[] separator3 = new char[]
							{
								'%'
							};
							string[] array3 = array2[0].Split(separator3);
							if (1 <= array3.Length)
							{
								info.QuestDlgCharCode = array3[0];
								if (info.QuestDlgCharCode == "player")
								{
									info.bTalkUser = true;
								}
							}
							if (2 <= array3.Length)
							{
								QUEST_DLG_INFO.Alignment ePosition = QUEST_DLG_INFO.Alignment.CENTER;
								if (array3[1] == "l")
								{
									ePosition = QUEST_DLG_INFO.Alignment.LEFT;
								}
								else if (array3[1] == "r")
								{
									ePosition = QUEST_DLG_INFO.Alignment.RIGHT;
								}
								info.ePosition = ePosition;
							}
							if (3 <= array3.Length)
							{
								if (array3[2] == "x")
								{
									info.bShowName = false;
								}
								else
								{
									info.bShowName = true;
								}
							}
							if (4 <= array3.Length)
							{
								if (array3[3] == "x")
								{
									info.bShowImage = false;
								}
								else
								{
									info.bShowImage = true;
								}
							}
						}
						if (2 <= array2.Length)
						{
							char[] separator4 = new char[]
							{
								'%'
							};
							string[] array4 = array2[1].Split(separator4);
							if (1 <= array4.Length)
							{
								info.QuestDlgCharCode2 = array4[0];
								if (info.QuestDlgCharCode2 == "player")
								{
									info.bTalkUser = true;
								}
							}
							if (2 <= array4.Length)
							{
								QUEST_DLG_INFO.Alignment ePosition2 = QUEST_DLG_INFO.Alignment.NONE;
								if (array4[1] == "l")
								{
									ePosition2 = QUEST_DLG_INFO.Alignment.LEFT;
								}
								else if (array4[1] == "r")
								{
									ePosition2 = QUEST_DLG_INFO.Alignment.RIGHT;
								}
								info.ePosition2 = ePosition2;
							}
							if (3 <= array4.Length)
							{
								if (array4[2] == "x")
								{
									info.bShowName2 = false;
								}
								else
								{
									info.bShowName2 = true;
								}
							}
							else
							{
								info.bShowName2 = true;
							}
							if (3 <= array4.Length)
							{
								if (array4[2] == "x")
								{
									info.bShowImage2 = false;
								}
								else
								{
									info.bShowImage2 = true;
								}
							}
							else
							{
								info.bShowImage2 = true;
							}
						}
					}
					else if (text.Contains("%"))
					{
						char[] separator5 = new char[]
						{
							'%'
						};
						string[] array5 = text.Split(separator5);
						if (1 <= array5.Length)
						{
							info.QuestDlgCharCode = NrTSingleton<UIDataManager>.Instance.GetString(info.QuestDlgCharCode, array5[0]);
							if (info.QuestDlgCharCode == "player")
							{
								info.bTalkUser = true;
							}
						}
						if (2 <= array5.Length)
						{
							QUEST_DLG_INFO.Alignment ePosition3 = QUEST_DLG_INFO.Alignment.CENTER;
							if (array5[1] == "l")
							{
								ePosition3 = QUEST_DLG_INFO.Alignment.LEFT;
							}
							else if (array5[1] == "r")
							{
								ePosition3 = QUEST_DLG_INFO.Alignment.RIGHT;
							}
							info.ePosition = ePosition3;
						}
						if (3 <= array5.Length)
						{
							if (array5[2] == "x")
							{
								info.bShowName = false;
							}
							else
							{
								info.bShowName = true;
							}
						}
						if (4 <= array5.Length)
						{
							if (array5[3] == "x")
							{
								info.bShowImage = false;
							}
							else
							{
								info.bShowImage = true;
							}
						}
					}
					else
					{
						info.QuestDlgCharCode = NrTSingleton<UIDataManager>.Instance.GetString(info.QuestDlgCharCode, text);
					}
					break;
				case 4:
					info.strLang_Idx = NrTSingleton<UIDataManager>.Instance.GetString(info.strLang_Idx, text);
					break;
				case 5:
					goto IL_4E5;
				case 6:
					info.strUserAnswer = NrTSingleton<UIDataManager>.Instance.GetString(info.strUserAnswer, text);
					break;
				default:
					goto IL_4E5;
				}
				IL_661:
				this.paramValueBuilder.Length = 0;
				num++;
				i++;
				continue;
				IL_4E5:
				if (text.Equals("s"))
				{
					info.AddDLGOption(2L);
				}
				else if (text.Equals("b"))
				{
					info.AddDLGOption(1L);
				}
				else if (text.Equals("sb"))
				{
					info.AddDLGOption(2L);
					info.AddDLGOption(1L);
				}
				else if (text.Equals("c"))
				{
					info.AddDLGOption(4L);
				}
				else if (text.Equals("sound"))
				{
					info.AddDLGOption(8L);
					flag = true;
				}
				else if (flag)
				{
					info.strSound = text;
					flag = false;
				}
				else if (text.Equals("i"))
				{
					info.MakeLoadImage();
				}
				else if (text.Equals("ir"))
				{
					info.MakeLoadImage();
					info.AddDLGOption(16L);
				}
				else if (text.Equals("il"))
				{
					info.MakeLoadImage();
					info.AddDLGOption(32L);
				}
				else if (text.Equals("ic"))
				{
					info.bImageClose = true;
				}
				else if (text.Equals("fw"))
				{
					info.AddDLGOption(64L);
				}
				else if (text.Equals("fb"))
				{
					info.AddDLGOption(128L);
				}
				goto IL_661;
			}
			this.paramValueBuilder.Append(strInfo[i]);
			i++;
		}
		return true;
	}

	public string GetBaseColor()
	{
		return "[#D6D0C5FF]";
	}

	private void ReplaceGender(ref int m_iTextCount, ref StringBuilder refReplaceText, string content)
	{
		char[] separator = new char[]
		{
			'/'
		};
		int num = 2;
		int i = m_iTextCount + 2;
		int length = content.Length;
		NrTSingleton<UIDataManager>.Instance.InitStringBuilder();
		while (i < length)
		{
			i++;
			num++;
			if (content[i] == this.m_szTag[0])
			{
				break;
			}
			NrTSingleton<UIDataManager>.Instance.AppendString(content[i]);
		}
		string[] array = NrTSingleton<UIDataManager>.Instance.GetString().Split(separator);
		if (2 <= array.Length)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				if (@char.GetCharKindInfo().GetGender() == 1)
				{
					refReplaceText.Append(array[0]);
				}
				else if (@char.GetCharKindInfo().GetGender() == 2)
				{
					refReplaceText.Append(array[1]);
				}
			}
		}
		m_iTextCount += num + 2;
	}

	private int GetCostumeDamage(NkSoldierInfo solInfo, int costumeUnique)
	{
		if (solInfo == null)
		{
			return 0;
		}
		int result = (solInfo.GetMinDamage_NotAdjustCostume() + solInfo.GetMaxDamage_NotAdjustCostume()) / 2;
		CharCostumeInfo_Data costumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeData(costumeUnique);
		if (costumeData == null)
		{
			return result;
		}
		if (costumeData.IsNormalCostume())
		{
			return result;
		}
		double num = (double)((float)solInfo.GetMinDamage_NotAdjustCostume() + (float)(solInfo.GetMinDamage_NotAdjustCostume() * costumeData.m_ATKBonusRate) / 100f);
		double num2 = (double)((float)solInfo.GetMaxDamage_NotAdjustCostume() + (float)(solInfo.GetMaxDamage_NotAdjustCostume() * costumeData.m_ATKBonusRate) / 100f);
		return (int)((num + num2) / 2.0);
	}
}
