using System;
using System.Collections.Generic;

namespace SERVICE
{
	public static class ServiceCode
	{
		private static Dictionary<eSERVICE_AREA, string> m_dicMobileCode;

		static ServiceCode()
		{
			ServiceCode.m_dicMobileCode = new Dictionary<eSERVICE_AREA, string>();
			ServiceCode.Init();
		}

		public static void Init()
		{
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL, "korlocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_KORQA, "korqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_KORTSTORE, "kortstore");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_KORGOOGLE, "korgoogle");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_KORNAVER, "kornaver");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_BANDNAVER, "bandnaver");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_BANDGOOGLE, "bandgoogle");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_KORKAKAO, "korkakao");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_KAKAOTSTORE, "kakaotstore");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGLOCAL, "globalenglocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALENGQA, "globalengqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALJPNLOCAL, "globaljpnlocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALJPNQA, "globaljpnqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNLOCAL, "globalchnlocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALCHNQA, "globalchnqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALTESTLOCAL, "globaltestlocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_GLOBALTESTQA, "globaltestqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_USLOCAL, "uslocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_USQA, "usqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_USGOOGLE, "usios");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_USAMAZON, "usios");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_CNLOCAL, "cnlocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_CNQA, "cnqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_CNTEST, "test");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_CNREVIEW, "review");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_JPLOCAL, "jplocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_JPQA, "LGLOH");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_JPQALINE, "LGLOH");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_ANDROID_JPLINE, "LGLOH");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_KORLOCAL, "korlocal");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_KORQA, "korqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_KORAPPSTORE, "korappstore");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_KORKAKAO, "korkakao");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_USQA, "usqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_USIOS, "usios");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_CNQA, "cnqa");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_CNTEST, "test");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_JPQA, "lgloh");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_JPQALINE, "lgloh");
			ServiceCode.m_dicMobileCode.Add(eSERVICE_AREA.SERVICE_IOS_JPLINE, "lgloh");
		}

		public static eSERVICE_AREA GetServiceArea(string servicecode)
		{
			if (ServiceCode.m_dicMobileCode == null)
			{
				return eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL;
			}
			Dictionary<eSERVICE_AREA, string>.Enumerator enumerator = ServiceCode.m_dicMobileCode.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<eSERVICE_AREA, string> current = enumerator.Current;
				if (current.Key.ToString().Equals(servicecode))
				{
					KeyValuePair<eSERVICE_AREA, string> current2 = enumerator.Current;
					return current2.Key;
				}
			}
			return eSERVICE_AREA.SERVICE_ANDROID_KORLOCAL;
		}

		public static string GetMobileCode(string servicecode)
		{
			if (ServiceCode.m_dicMobileCode != null)
			{
				Dictionary<eSERVICE_AREA, string>.Enumerator enumerator = ServiceCode.m_dicMobileCode.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<eSERVICE_AREA, string> current = enumerator.Current;
					if (current.Key.ToString().Equals(servicecode))
					{
						KeyValuePair<eSERVICE_AREA, string> current2 = enumerator.Current;
						return current2.Value;
					}
				}
			}
			return "korlocal";
		}

		public static string GetMobileCode(int servicearea)
		{
			return ServiceCode.GetMobileCode((eSERVICE_AREA)servicearea);
		}

		public static string GetMobileCode(eSERVICE_AREA servicearea)
		{
			string result = "korlocal";
			if (ServiceCode.m_dicMobileCode != null && ServiceCode.m_dicMobileCode.ContainsKey(servicearea))
			{
				string empty = string.Empty;
				if (ServiceCode.m_dicMobileCode.TryGetValue(servicearea, out empty))
				{
					result = empty;
				}
			}
			return result;
		}
	}
}
