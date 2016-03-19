using GAME;
using System;
using UnityEngine;

namespace PROTOCOL
{
	public class SOLDIER_INFO_EXTEND : SOLDIER_INFO
	{
		public string Name = string.Empty;

		public string Sol_Name = string.Empty;

		public string City_Name = string.Empty;

		public string workType = string.Empty;

		public int WEAPON;

		public int STR;

		public int INT;

		public int CMD;

		public int POL;

		public int CLASS;

		public bool NeedRepairItem;

		public UIBaseInfoLoader m_FaceImg;

		public Texture2D m_WorkImg;

		public int SolMAX;

		public SOLDIER_INFO_EXTEND(NkSoldierInfo d)
		{
			this.ResetData(d);
		}

		public SOLDIER_INFO_EXTEND(SOLDIER_INFO d)
		{
			this.ResetData(d);
		}

		public void ResetData(SOLDIER_INFO SolData)
		{
			base.Set(ref SolData);
		}

		public void ResetData(NkSoldierInfo pkSolinfo)
		{
			base.Set(ref pkSolinfo.m_kBase);
		}
	}
}
