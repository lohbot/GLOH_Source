using System;

public class NIAPResult
{
	private string requestType;

	private string resultType;

	private string result;

	private string extraValue;

	public NIAPResult(string requestType, string resultType, string result, string extraValue)
	{
		this.requestType = requestType;
		this.resultType = resultType;
		this.result = result;
		this.extraValue = extraValue;
	}

	public string getResult()
	{
		return this.result;
	}

	public string getExtraValue()
	{
		return this.extraValue;
	}

	public string getRequestType()
	{
		return this.requestType;
	}

	public string getResultType()
	{
		return this.resultType;
	}
}
