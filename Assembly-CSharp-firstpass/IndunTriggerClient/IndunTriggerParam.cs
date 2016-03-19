using System;
using UnityEngine;

namespace IndunTriggerClient
{
	public class IndunTriggerParam
	{
		public int m_eKind;

		public int m_iD;

		public string m_szName = string.Empty;

		public string[] m_szParam;

		public bool m_bDelete;

		public GameObject m_GameObjcet;

		public IndunTriggerParam()
		{
			this.m_szParam = new string[15];
			for (int i = 0; i < 15; i++)
			{
				this.m_szParam[i] = string.Empty;
			}
			this.m_szName = string.Empty;
			this.m_bDelete = false;
			this.m_iD = 0;
			this.m_GameObjcet = null;
		}

		public string GetParam(int index)
		{
			if (index < 0 || index > 15)
			{
				return null;
			}
			return this.m_szParam[index];
		}

		public void SetParam(int index, string szString)
		{
			if (index < 0 || index > 15)
			{
				return;
			}
			for (int i = 0; i < 50; i++)
			{
				this.m_szParam[index] = string.Empty;
			}
			this.m_szParam[index] = szString;
		}

		public void SetKind(int Kind)
		{
			this.m_eKind = Kind;
		}

		public void Set(int Kind, string[] szParam)
		{
			this.SetKind(Kind);
			for (int i = 0; i < 15; i++)
			{
				this.SetParam(i, szParam[i]);
			}
		}
	}
}
