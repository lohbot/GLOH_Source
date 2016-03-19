using System;

public class SREWARD_PRODUCT
{
	public int m_nRewardType;

	public int m_nRewardRate;

	public int m_nRewardValue1;

	public int m_nRewardValue2;

	public string m_stRewardTexture;

	public string m_strParserRewardType;

	public SREWARD_PRODUCT()
	{
		this.Init();
	}

	public void Init()
	{
		this.m_nRewardType = 0;
		this.m_nRewardRate = 0;
		this.m_nRewardValue1 = 0;
		this.m_nRewardValue2 = 0;
		this.m_stRewardTexture = string.Empty;
		this.m_strParserRewardType = string.Empty;
	}
}
