using System;
using System.Collections.Generic;

namespace IndunTriggerClient
{
	public class IndunTriggerEditorInfo
	{
		public static IndunTriggerEditorInfo m_Instance;

		private IndunTriggerParamType[] m_TriggerParam;

		private IndunActionParamType[] m_ActionParam;

		private List<IndunCharATB> m_arCharATB;

		private string[] m_szParamName;

		public static IndunTriggerEditorInfo GetInstance()
		{
			if (IndunTriggerEditorInfo.m_Instance == null)
			{
				IndunTriggerEditorInfo.m_Instance = new IndunTriggerEditorInfo();
				IndunTriggerEditorInfo.m_Instance.SetInfo();
			}
			return IndunTriggerEditorInfo.m_Instance;
		}

		protected void SetInfo()
		{
			this.m_TriggerParam = new IndunTriggerParamType[7];
			this.m_ActionParam = new IndunActionParamType[18];
			this.m_arCharATB = new List<IndunCharATB>();
			this.SetParamName();
			this.SetTriggerInfo("NONE", INDUN_TRIGGERKIND.INDUN_TRIGGERKIND_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetTriggerInfo("STARTWAR", INDUN_TRIGGERKIND.INDUN_TRIGGERKIND_START, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetTriggerInfo("AREAENTER", INDUN_TRIGGERKIND.INDUN_TRIGGERKIND_AREAENTER, PARAMTYPE.PARAMTYPE_AREAINDEX, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetTriggerInfo("ISCOMPLETEEVENT", INDUN_TRIGGERKIND.INDUN_TRIGGERKIND_ISCOMPLETEEVENT, PARAMTYPE.PARAMTYPE_VALUE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetTriggerInfo("CHECKTIME", INDUN_TRIGGERKIND.INDUN_TRIGGERKIND_CHECKTIME, PARAMTYPE.PARAMTYPE_TIME, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetTriggerInfo("CHECKCHAR", INDUN_TRIGGERKIND.INDUN_TRIGGERKIND_CHECKCHAR, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_COUNT, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetTriggerInfo("CHECKQUEST", INDUN_TRIGGERKIND.INDUN_TRIGGERKIND_CHECKQUEST, PARAMTYPE.PARAMTYPE_QUESTIDX, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("NONE", INDUN_ACTIONKIND.INDUN_ACTIONKIND_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("CHARMAKE", INDUN_ACTIONKIND.INDUN_ACTIONKIND_CHARMAKE, PARAMTYPE.PARAMTYPE_ECONUM, PARAMTYPE.PARAMTYPE_POSX, PARAMTYPE.PARAMTYPE_POSY, PARAMTYPE.PARAMTYPE_POSZ, PARAMTYPE.PARAMTYPE_VALUE, PARAMTYPE.PARAMTYPE_MOVERANGE, PARAMTYPE.PARAMTYPE_HORIZON, PARAMTYPE.PARAMTYPE_COUNT, PARAMTYPE.PARAMTYPE_POSX, PARAMTYPE.PARAMTYPE_POSY, PARAMTYPE.PARAMTYPE_POSZ, PARAMTYPE.PARAMTYPE_POSX, PARAMTYPE.PARAMTYPE_POSY, PARAMTYPE.PARAMTYPE_POSZ, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("BATTLEMATCH", INDUN_ACTIONKIND.INDUN_ACTIONKIND_BATTLEMATCH, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("LOADEVENT", INDUN_ACTIONKIND.INDUN_ACTIONKIND_LOADEVENT, PARAMTYPE.PARAMTYPE_VALUE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("FAILEVENT", INDUN_ACTIONKIND.INDUN_ACTIONKIND_FAILEVENT, PARAMTYPE.PARAMTYPE_VALUE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("TIMEDELAY", INDUN_ACTIONKIND.INDUN_ACTIONKIND_TIMEDELAY, PARAMTYPE.PARAMTYPE_TIME, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("RESETEVENT", INDUN_ACTIONKIND.INDUN_ACTIONKIND_RESETEVENT, PARAMTYPE.PARAMTYPE_VALUE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("CHARERASE", INDUN_ACTIONKIND.INDUN_ACTIONKIND_CHARERASE, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("EFFECT", INDUN_ACTIONKIND.INDUN_ACTIONKIND_EFFECT, PARAMTYPE.PARAMTYPE_EFFECTCODE, PARAMTYPE.PARAMTYPE_POSX, PARAMTYPE.PARAMTYPE_POSZ, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_NAME, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_HORIZON, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("SHOWTEXT", INDUN_ACTIONKIND.INDUN_ACTIONKIND_SHOWTEXT, PARAMTYPE.PARAMTYPE_TEXTINDEX, PARAMTYPE.PARAMTYPE_TIME, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_ACTIONTIME, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("VICTORYWAR", INDUN_ACTIONKIND.INDUN_ACTIONKIND_VICTORYWAR, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("FAILWAR", INDUN_ACTIONKIND.INDUN_ACTIONKIND_FAILWAR, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("ANICONTROL", INDUN_ACTIONKIND.INDUN_ACTIONKIND_ANICONTROL, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_ANISEQUENCE, PARAMTYPE.PARAMTYPE_COUNT, PARAMTYPE.PARAMTYPE_ANISEQUENCE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("INPUTCONTROL", INDUN_ACTIONKIND.INDUN_ACTIONKIND_INPUTCONTROL, PARAMTYPE.PARAMTYPE_VALUE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("BLOCKAREA", INDUN_ACTIONKIND.INDUN_ACTIONKIND_BLOCKAREA, PARAMTYPE.PARAMTYPE_AREAINDEX, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("INDUNATB", INDUN_ACTIONKIND.INDUN_ACTIONKIND_INDUNATB, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_CHARATB, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("DRAMA", INDUN_ACTIONKIND.INDUN_ACTIONKIND_DRAMA, PARAMTYPE.PARAMTYPE_DRAMACODE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
			this.SetActionInfo("CAMERA", INDUN_ACTIONKIND.INDUN_ACTIONKIND_CAMERA, PARAMTYPE.PARAMTYPE_TYPE, PARAMTYPE.PARAMTYPE_KIND, PARAMTYPE.PARAMTYPE_POSX, PARAMTYPE.PARAMTYPE_POSY, PARAMTYPE.PARAMTYPE_POSZ, PARAMTYPE.PARAMTYPE_VALUE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE.PARAMTYPE_NONE);
		}

		protected void SetTriggerInfo(string szName, INDUN_TRIGGERKIND eKind, PARAMTYPE Param1 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param2 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param3 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param4 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param5 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param6 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param7 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param8 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param9 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param10 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param11 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param12 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param13 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param14 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param15 = PARAMTYPE.PARAMTYPE_NONE)
		{
			this.m_TriggerParam[(int)eKind] = new IndunTriggerParamType(szName, eKind, Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, Param9, Param10, Param11, Param12, Param13, Param14, Param15);
		}

		protected void SetActionInfo(string szName, INDUN_ACTIONKIND eKind, PARAMTYPE Param1 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param2 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param3 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param4 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param5 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param6 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param7 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param8 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param9 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param10 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param11 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param12 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param13 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param14 = PARAMTYPE.PARAMTYPE_NONE, PARAMTYPE Param15 = PARAMTYPE.PARAMTYPE_NONE)
		{
			this.m_ActionParam[(int)eKind] = new IndunActionParamType(szName, eKind, Param1, Param2, Param3, Param4, Param5, Param6, Param7, Param8, Param9, Param10, Param11, Param12, Param13, Param14, Param15);
		}

		protected void SetParamName()
		{
			IndunCharATB indunCharATB = new IndunCharATB();
			indunCharATB.m_eCharATB = eINDUN_CHARSTATEATB.INDUN_CHARSTATEATB_NONE;
			indunCharATB.m_szATBName = "NONE";
			this.m_arCharATB.Add(indunCharATB);
			indunCharATB = new IndunCharATB();
			indunCharATB.m_eCharATB = eINDUN_CHARSTATEATB.INDUN_CHARSTATEATB_ACTION_LOOP;
			indunCharATB.m_szATBName = "ACTION_LOOP";
			this.m_arCharATB.Add(indunCharATB);
			this.m_szParamName = new string[41];
			this.m_szParamName[0] = "NONE";
			this.m_szParamName[1] = "VALUE";
			this.m_szParamName[2] = "ALLY";
			this.m_szParamName[3] = "KIND";
			this.m_szParamName[4] = "AREAINDEX";
			this.m_szParamName[5] = "LEVEL";
			this.m_szParamName[6] = "COUNT";
			this.m_szParamName[7] = "TIME";
			this.m_szParamName[8] = "TEXTINDEX";
			this.m_szParamName[9] = "GRIDPOS";
			this.m_szParamName[10] = "ACTIONTIME";
			this.m_szParamName[11] = "HORIZON";
			this.m_szParamName[12] = "VERTICAL";
			this.m_szParamName[13] = "QEUST_IDX";
			this.m_szParamName[14] = "DIFFICULT";
			this.m_szParamName[15] = "EXP";
			this.m_szParamName[16] = "ITEMUNIQUE";
			this.m_szParamName[17] = "TYPE";
			this.m_szParamName[18] = "MOVECELLPOS";
			this.m_szParamName[19] = "MOVETYPE";
			this.m_szParamName[20] = "PROBABILITY";
			this.m_szParamName[21] = "MONEY";
			this.m_szParamName[22] = "DRAMACODE";
			this.m_szParamName[23] = "EFFECTCODE";
			this.m_szParamName[24] = "NAME";
			this.m_szParamName[25] = "LEFTRIGHT";
			this.m_szParamName[26] = "DOMAIN";
			this.m_szParamName[27] = "CATEGORY";
			this.m_szParamName[28] = "AUDIOKEY";
			this.m_szParamName[29] = "EFFECTLAYER";
			this.m_szParamName[30] = "CHARATB";
			this.m_szParamName[31] = "POS X";
			this.m_szParamName[32] = "POS Z";
			this.m_szParamName[33] = "GRID ID";
			this.m_szParamName[34] = "STARTPOS";
			this.m_szParamName[35] = "X SIZE";
			this.m_szParamName[36] = "Y SIZE";
			this.m_szParamName[37] = "ECO NUM";
			this.m_szParamName[38] = "MOVERANGE";
			this.m_szParamName[39] = "ANISEQUENCE";
			this.m_szParamName[40] = "POS Y";
		}

		public string GetParamName(PARAMTYPE eParamType)
		{
			return this.m_szParamName[(int)eParamType];
		}

		public string[] GetTriggerName()
		{
			string[] array = new string[this.m_TriggerParam.Length];
			for (int i = 0; i < this.m_TriggerParam.Length; i++)
			{
				array[i] = this.m_TriggerParam[i].m_szName;
			}
			return array;
		}

		public int[] GetTriggerKindList()
		{
			int[] array = new int[this.m_TriggerParam.Length];
			for (int i = 0; i < this.m_TriggerParam.Length; i++)
			{
				array[i] = (int)this.m_TriggerParam[i].m_eKind;
			}
			return array;
		}

		public string[] GetActionName()
		{
			string[] array = new string[this.m_ActionParam.Length];
			for (int i = 0; i < this.m_ActionParam.Length; i++)
			{
				array[i] = this.m_ActionParam[i].m_szName;
			}
			return array;
		}

		public int[] GetActionKindList()
		{
			int[] array = new int[this.m_ActionParam.Length];
			for (int i = 0; i < this.m_ActionParam.Length; i++)
			{
				array[i] = (int)this.m_ActionParam[i].m_eKind;
			}
			return array;
		}

		public string GetTriggerName(INDUN_TRIGGERKIND eKind)
		{
			for (int i = 0; i < this.m_TriggerParam.Length; i++)
			{
				if (this.m_TriggerParam[i].m_eKind == eKind)
				{
					return this.m_TriggerParam[i].m_szName;
				}
			}
			return null;
		}

		public string GetActionName(INDUN_ACTIONKIND eKind)
		{
			for (int i = 0; i < this.m_ActionParam.Length; i++)
			{
				if (this.m_ActionParam[i].m_eKind == eKind)
				{
					return this.m_ActionParam[i].m_szName;
				}
			}
			return null;
		}

		public PARAMTYPE[] GetParam(INDUNTRIGGERTYPE eType, int eKind)
		{
			if (eType <= INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_NONE || eType >= INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_MAX)
			{
				return null;
			}
			switch (eType)
			{
			case INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_NONE:
				return null;
			case INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_EVENT:
				return null;
			case INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_TRIGGER:
				return this.m_TriggerParam[eKind].m_eParam;
			case INDUNTRIGGERTYPE.INDUNTRIGGERTYPE_ACTION:
				return this.m_ActionParam[eKind].m_eParam;
			default:
				return null;
			}
		}

		public void SetTriggerHelp(INDUN_TRIGGERKIND eKind, string[] help)
		{
			for (int i = 0; i < this.m_TriggerParam.Length; i++)
			{
				if (this.m_TriggerParam[i].m_eKind == eKind)
				{
					this.m_TriggerParam[i].SetHelp(help);
				}
			}
		}

		public void SetActionHelp(INDUN_ACTIONKIND eKind, string[] help)
		{
			for (int i = 0; i < this.m_ActionParam.Length; i++)
			{
				if (this.m_ActionParam[i].m_eKind == eKind)
				{
					this.m_ActionParam[i].SetHelp(help);
				}
			}
		}

		public string[] GetTriggerHelp(INDUN_TRIGGERKIND eKind)
		{
			for (int i = 0; i < this.m_TriggerParam.Length; i++)
			{
				if (this.m_TriggerParam[i].m_eKind == eKind)
				{
					return this.m_TriggerParam[i].GetHelp();
				}
			}
			return null;
		}

		public string[] GetActionHelp(INDUN_ACTIONKIND eKind)
		{
			for (int i = 0; i < this.m_ActionParam.Length; i++)
			{
				if (this.m_ActionParam[i].m_eKind == eKind)
				{
					return this.m_ActionParam[i].GetHelp();
				}
			}
			return null;
		}

		public string[] GetCharATBToString()
		{
			string[] array = new string[this.m_arCharATB.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.m_arCharATB[i].m_szATBName;
			}
			return array;
		}

		public int GetCharATBFromName(string szType)
		{
			for (int i = 0; i < this.m_arCharATB.Count; i++)
			{
				if (szType == this.m_arCharATB[i].m_szATBName)
				{
					return (int)this.m_arCharATB[i].m_eCharATB;
				}
			}
			return 0;
		}

		public string GetCharATBNameFromATB(int ATB)
		{
			for (int i = 0; i < this.m_arCharATB.Count; i++)
			{
				if (ATB == (int)this.m_arCharATB[i].m_eCharATB)
				{
					return this.m_arCharATB[i].m_szATBName;
				}
			}
			return null;
		}
	}
}
