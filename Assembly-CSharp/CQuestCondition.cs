using System;

public abstract class CQuestCondition
{
	protected long m_i64Param;

	protected long m_i64ParamVal;

	public long m_i64PreParamVal;

	protected string m_szTextKey = string.Empty;

	public string LagnType
	{
		get
		{
			return this.m_szTextKey;
		}
		set
		{
			this.m_szTextKey = value;
		}
	}

	public void SetConditionInfo(long i64Param, long i64ParamVal, string szTextKey)
	{
		this.m_i64Param = i64Param;
		this.m_i64ParamVal = i64ParamVal;
		this.m_szTextKey = szTextKey;
	}

	public long GetParam()
	{
		return this.m_i64Param;
	}

	public long GetParamVal()
	{
		return this.m_i64ParamVal;
	}

	public virtual bool CheckCondition(long i64Param, ref long i64ParamVal)
	{
		return i64Param == this.GetParam() && i64ParamVal >= this.GetParamVal();
	}

	public virtual void AfterAccept()
	{
	}

	public virtual void AfterOnGoing()
	{
	}

	public virtual void AfterAutoPath()
	{
	}

	public virtual bool IsServerCheck()
	{
		return true;
	}

	public abstract string GetConditionText(long i64ParamVal);
}
