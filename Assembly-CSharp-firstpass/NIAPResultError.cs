using SimpleJSON;
using System;

public class NIAPResultError
{
	private string code;

	private string message;

	public NIAPResultError(string _code, string _message)
	{
		this.code = _code;
		this.message = _message;
	}

	public static NIAPResultError Build(string errorResult)
	{
		JSONNode jSONNode = JSON.Parse(errorResult);
		string text = jSONNode[NIAPConstant.Param.code];
		string text2 = jSONNode[NIAPConstant.Param.message];
		return new NIAPResultError(text, text2);
	}

	public string getCode()
	{
		return this.code;
	}

	public string getMessage()
	{
		return this.message;
	}

	public override string ToString()
	{
		return string.Format("[NIAPError] code : " + this.code + ", message : " + this.message, new object[0]);
	}
}
