using PROTOCOL;
using SERVICE;
using System;

namespace Global
{
	public class Client
	{
		private static Client instance;

		public static WorldServer_INFO[] m_WorldServer_Infos;

		public static long m_MyWS;

		public static byte m_MyCH;

		public static int m_WorldServer_Count;

		public static int WORLD_SERVER_MAX = 50;

		public static int m_MyOriginWS;

		public static string m_WorldType = "A";

		public static Client GetInstance()
		{
			if (Client.instance == null)
			{
				Client.instance = new Client();
			}
			return Client.instance;
		}

		public bool IsUnionWorldServer()
		{
			return this.IsUnionWorldServer((int)Client.m_MyWS);
		}

		public bool IsUnionWorldServer(int WSID)
		{
			WorldServer_INFO[] worldServer_Infos = Client.m_WorldServer_Infos;
			for (int i = 0; i < worldServer_Infos.Length; i++)
			{
				WorldServer_INFO worldServer_INFO = worldServer_Infos[i];
				if (worldServer_INFO.byIsUnionServer == 1 && worldServer_INFO.i32SVID == WSID)
				{
					return true;
				}
			}
			return false;
		}

		public bool Get_UnionWorldServer_Info(ref char[] szIP, ref int _i32Port)
		{
			WorldServer_INFO[] worldServer_Infos = Client.m_WorldServer_Infos;
			for (int i = 0; i < worldServer_Infos.Length; i++)
			{
				WorldServer_INFO worldServer_INFO = worldServer_Infos[i];
				if (worldServer_INFO.byIsUnionServer == 1)
				{
					TKString.CharChar(worldServer_INFO.szIP, ref szIP);
					_i32Port = worldServer_INFO.i32_Port;
					return true;
				}
			}
			return false;
		}

		public bool Get_WorldServer_InfoFromID(int WSID, ref char[] szIP, ref int _i32Port)
		{
			WorldServer_INFO[] worldServer_Infos = Client.m_WorldServer_Infos;
			for (int i = 0; i < worldServer_Infos.Length; i++)
			{
				WorldServer_INFO worldServer_INFO = worldServer_Infos[i];
				if (worldServer_INFO.i32SVID == WSID)
				{
					TKString.CharChar(worldServer_INFO.szIP, ref szIP);
					_i32Port = worldServer_INFO.i32_Port;
					return true;
				}
			}
			return false;
		}

		public string Get_WorldServerName_InfoFromID(int WSID)
		{
			WorldServer_INFO[] worldServer_Infos = Client.m_WorldServer_Infos;
			for (int i = 0; i < worldServer_Infos.Length; i++)
			{
				WorldServer_INFO worldServer_INFO = worldServer_Infos[i];
				if (worldServer_INFO.i32SVID == WSID)
				{
					return new string(worldServer_INFO.szName);
				}
			}
			return null;
		}

		public static int GetLoginServerPort()
		{
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (TsPlatform.IsIPhone && (currentServiceArea == eSERVICE_AREA.SERVICE_IOS_KORQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_KORKAKAO))
			{
				return 9600;
			}
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_JPQALINE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_JPLINE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_JPQALINE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_JPLINE)
			{
				return 10600;
			}
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_USQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_USIOS)
			{
				return 9600;
			}
			return 6000;
		}

		public static int GetWorldServerPort()
		{
			eSERVICE_AREA currentServiceArea = NrTSingleton<NrGlobalReference>.Instance.GetCurrentServiceArea();
			if (TsPlatform.IsIPhone && (currentServiceArea == eSERVICE_AREA.SERVICE_IOS_KORQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_KORKAKAO))
			{
				return 9400;
			}
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_JPQALINE || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_JPLINE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_JPQALINE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_JPLINE)
			{
				return 10400;
			}
			if (currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USQA || currentServiceArea == eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_USQA || currentServiceArea == eSERVICE_AREA.SERVICE_IOS_USIOS)
			{
				return 9400;
			}
			return 4000;
		}
	}
}
