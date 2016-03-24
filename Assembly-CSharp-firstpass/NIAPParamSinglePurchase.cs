using SimpleJSON;
using System;

public class NIAPParamSinglePurchase : NIAPParam
{
	protected string paymentSeq;

	public NIAPParamSinglePurchase(string _paymentSeq) : base(NIAPConstant.InvokeMethod.getSinglePurchase)
	{
		this.paymentSeq = _paymentSeq;
	}

	public override string toString()
	{
		JSONClass jSONClass = new JSONClass();
		jSONClass[NIAPConstant.invokeMethod] = this.invokeMethod;
		jSONClass[NIAPConstant.Param.paymentSeq] = this.paymentSeq;
		return jSONClass.ToString();
	}
}
