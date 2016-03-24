using SimpleJSON;
using System;

public class NIAPResult
{
	private string result;

	private string extraValue;

	private string orignalString;

	public NIAPResult(string _result, string _extraValue, string _orignalString)
	{
		this.result = _result;
		this.extraValue = _extraValue;
		this.orignalString = _orignalString;
	}

	public static NIAPResult Build(string resultString)
	{
		JSONNode jSONNode = JSON.Parse(resultString);
		string text = jSONNode[NIAPConstant.result];
		string text2 = jSONNode[NIAPConstant.Param.extraValue];
		return new NIAPResult(text, text2, resultString);
	}

	public string getResult()
	{
		return this.result;
	}

	public string getExtraValue()
	{
		return this.extraValue;
	}

	public string getOrignalString()
	{
		return this.orignalString;
	}

	public override string ToString()
	{
		return "[NIAPResult] result : " + this.result + ", extra : " + this.extraValue;
	}
}
