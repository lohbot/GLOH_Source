using SimpleJSON;
using System;

public class NIAPParamInit : NIAPParam
{
	protected string publicKey;

	public NIAPParamInit(string _publicKey) : base(NIAPConstant.InvokeMethod.init)
	{
		this.publicKey = _publicKey;
	}

	public override string toString()
	{
		JSONClass jSONClass = new JSONClass();
		jSONClass[NIAPConstant.invokeMethod] = this.invokeMethod;
		jSONClass[NIAPConstant.Param.publicKey] = this.publicKey;
		return jSONClass.ToString();
	}
}
