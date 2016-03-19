using System;
using UnityForms;

public static class Protocol_Supply
{
	public enum E_BUFF_TYPE
	{
		ATTACK,
		DEFENSE,
		MAGIC_DEFENSE,
		HIT,
		DODGE,
		CRITICAL,
		MULTI_HIT,
		END
	}

	public struct Supply_Buff
	{
		public int m_nSupplyFunction;

		public int m_nBuffType;

		public int m_nBuffValue;

		public long m_lBuffTime;

		public void Init()
		{
			this.m_nSupplyFunction = 0;
			this.m_nBuffType = 0;
			this.m_nBuffValue = 0;
			this.m_lBuffTime = 0L;
		}
	}

	public const int N_BUFF_MAX = 21;

	private static Protocol_Supply.Supply_Buff[] s_saBuff = new Protocol_Supply.Supply_Buff[7];

	public static void Supply_Buff_Show()
	{
		bool flag = false;
		for (int i = 0; i < Protocol_Supply.s_saBuff.Length; i++)
		{
			if (Protocol_Supply.s_saBuff[i].m_nSupplyFunction > 0)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.BUFF_DLG);
		}
	}

	public static void Set_Buff(Protocol_Supply.Supply_Buff a_sBuff)
	{
		Protocol_Supply.s_saBuff[a_sBuff.m_nBuffType] = a_sBuff;
	}

	public static void Remove_Buff(Protocol_Supply.E_BUFF_TYPE a_eBuffType)
	{
		Protocol_Supply.s_saBuff[(int)a_eBuffType].Init();
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.BUFF_DLG);
	}

	public static Protocol_Supply.Supply_Buff Get_Buff(Protocol_Supply.E_BUFF_TYPE a_eBuffType)
	{
		return Protocol_Supply.s_saBuff[(int)a_eBuffType];
	}

	public static int Get_Buff_Vaule(Protocol_Supply.E_BUFF_TYPE a_eBuffType)
	{
		return Protocol_Supply.s_saBuff[(int)a_eBuffType].m_nBuffValue;
	}

	public static long Get_Buff_Tick_Count(long a_nTime)
	{
		return a_nTime * 60L * 1000L;
	}
}
