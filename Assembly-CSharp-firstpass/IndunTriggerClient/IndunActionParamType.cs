using System;

namespace IndunTriggerClient
{
	public class IndunActionParamType
	{
		public INDUN_ACTIONKIND m_eKind;

		public PARAMTYPE[] m_eParam;

		public string[] m_szHelp;

		public string m_szName = string.Empty;

		public IndunActionParamType()
		{
			this.m_eKind = INDUN_ACTIONKIND.INDUN_ACTIONKIND_NONE;
			this.m_eParam = new PARAMTYPE[15];
			this.m_szHelp = new string[15];
		}

		public IndunActionParamType(string szName, INDUN_ACTIONKIND eKind, PARAMTYPE Param1, PARAMTYPE Param2, PARAMTYPE Param3, PARAMTYPE Param4, PARAMTYPE Param5, PARAMTYPE Param6, PARAMTYPE Param7, PARAMTYPE Param8, PARAMTYPE Param9, PARAMTYPE Param10, PARAMTYPE Param11, PARAMTYPE Param12, PARAMTYPE Param13, PARAMTYPE Param14, PARAMTYPE Param15)
		{
			this.m_eKind = eKind;
			this.m_eParam = new PARAMTYPE[15];
			this.m_szHelp = new string[15];
			this.m_szName = szName;
			this.m_eParam[0] = Param1;
			this.m_eParam[1] = Param2;
			this.m_eParam[2] = Param3;
			this.m_eParam[3] = Param4;
			this.m_eParam[4] = Param5;
			this.m_eParam[5] = Param6;
			this.m_eParam[6] = Param7;
			this.m_eParam[7] = Param8;
			this.m_eParam[8] = Param9;
			this.m_eParam[9] = Param10;
			this.m_eParam[10] = Param11;
			this.m_eParam[11] = Param12;
			this.m_eParam[12] = Param13;
			this.m_eParam[13] = Param14;
			this.m_eParam[14] = Param15;
		}

		public void SetHelp(string[] help)
		{
			for (int i = 0; i < 15; i++)
			{
				this.m_szHelp[i] = help[i];
			}
		}

		public string[] GetHelp()
		{
			return this.m_szHelp;
		}
	}
}
