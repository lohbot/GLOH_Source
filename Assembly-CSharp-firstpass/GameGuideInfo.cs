using System;

public class GameGuideInfo
{
	public int m_nUnique;

	public GameGuideType m_eType;

	public GameGuideCheck m_eCheck;

	public int m_nPriority;

	public int m_nMinLevel;

	public int m_nMaxLevel;

	public string m_strBaloonTextKey;

	public string m_strTalkKey;

	public string m_strButtonKey;

	public float m_nDelayTime;

	public object m_objData;

	public float m_nCheckTime;

	public GameGuideInfo()
	{
		this.Init();
	}

	public virtual void Init()
	{
		this.m_nUnique = 0;
		this.m_eType = GameGuideType.DEFAULT;
		this.m_eCheck = GameGuideCheck.NONE;
		this.m_nPriority = 0;
		this.m_nMinLevel = 0;
		this.m_nMaxLevel = 0;
		this.m_strBaloonTextKey = string.Empty;
		this.m_strTalkKey = string.Empty;
		this.m_strButtonKey = string.Empty;
		this.m_nDelayTime = 0f;
		this.m_objData = null;
		this.m_nCheckTime = 0f;
	}

	public virtual void InitData()
	{
	}

	public void Set(GameGuideInfo gameGuideInfo)
	{
		this.m_nUnique = gameGuideInfo.m_nUnique;
		this.m_eType = gameGuideInfo.m_eType;
		this.m_eCheck = gameGuideInfo.m_eCheck;
		this.m_nPriority = gameGuideInfo.m_nPriority;
		this.m_nMinLevel = gameGuideInfo.m_nMinLevel;
		this.m_nMaxLevel = gameGuideInfo.m_nMaxLevel;
		this.m_strBaloonTextKey = gameGuideInfo.m_strBaloonTextKey;
		this.m_strTalkKey = gameGuideInfo.m_strTalkKey;
		this.m_strButtonKey = gameGuideInfo.m_strButtonKey;
		this.m_nDelayTime = gameGuideInfo.m_nDelayTime;
		this.m_objData = gameGuideInfo.m_objData;
		this.m_nCheckTime = gameGuideInfo.m_nCheckTime;
	}

	public virtual void ExcuteGameGuide()
	{
	}

	public virtual bool CheckGameGuide()
	{
		return false;
	}

	public virtual bool CheckGameGuideOnce()
	{
		return false;
	}

	public virtual string GetGameGuideText()
	{
		return string.Empty;
	}

	public int GetMinLevel()
	{
		return this.m_nMinLevel;
	}

	public int GetMaxLevel()
	{
		return this.m_nMaxLevel;
	}
}
