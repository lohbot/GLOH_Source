using SimpleJSON;
using System;

public class NIAPParamConsume : NIAPParam
{
	protected string purchaseAsJsonText;

	protected string signature;

	public NIAPParamConsume(string _purchaseAsJsonText, string _signature) : base(NIAPConstant.InvokeMethod.requestConsume)
	{
		this.purchaseAsJsonText = _purchaseAsJsonText;
		this.signature = _signature;
	}

	public override string toString()
	{
		JSONClass jSONClass = new JSONClass();
		jSONClass[NIAPConstant.invokeMethod] = this.invokeMethod;
		jSONClass[NIAPConstant.Param.purchaseAsJsonText] = this.purchaseAsJsonText;
		jSONClass[NIAPConstant.Param.signature] = this.signature;
		return jSONClass.ToString();
	}
}
