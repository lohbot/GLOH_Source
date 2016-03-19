using System;

public static class INDUN_DEFINE
{
	public const int INVALID_INDUN_UNIQUE = -1;

	public const int INVALID_INDUN_IDX = -1;

	private static NkValueParse<eINDUN_TYPE> m_kIndunTypeCode;

	static INDUN_DEFINE()
	{
		INDUN_DEFINE.m_kIndunTypeCode = new NkValueParse<eINDUN_TYPE>();
		INDUN_DEFINE._RegisterIndunType();
	}

	private static void _RegisterIndunType()
	{
		INDUN_DEFINE.m_kIndunTypeCode.InsertCodeValue("NONE", eINDUN_TYPE.eINDUN_TYPE_NONE);
		INDUN_DEFINE.m_kIndunTypeCode.InsertCodeValue("SOLO", eINDUN_TYPE.eINDUN_TYPE_SOLO);
		INDUN_DEFINE.m_kIndunTypeCode.InsertCodeValue("PARTY", eINDUN_TYPE.eINDUN_TYPE_PARTY);
		INDUN_DEFINE.m_kIndunTypeCode.InsertCodeValue("GUILD", eINDUN_TYPE.eINDUN_TYPE_GUILD);
		INDUN_DEFINE.m_kIndunTypeCode.InsertCodeValue("ALL", eINDUN_TYPE.eINDUN_TYPE_ALL);
	}

	public static eINDUN_TYPE GetIndunType(string szType)
	{
		return INDUN_DEFINE.m_kIndunTypeCode.GetValue(szType);
	}
}
