using System;

namespace PROTOCOL
{
	public class SCENARIO_USER_INFO
	{
		public byte i8Ally;

		public byte i8User_num;

		public char[] szUser_name = new char[21];

		public SCENARIO_SOLDIER_INFO[] general_infos = new SCENARIO_SOLDIER_INFO[6];

		public SCENARIO_USER_INFO()
		{
			for (int i = 0; i < 6; i++)
			{
				this.general_infos[i] = new SCENARIO_SOLDIER_INFO();
			}
		}
	}
}
