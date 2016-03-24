using SimpleJSON;
using System;

public class NIAPParamPayment : NIAPParam
{
	protected string productCode;

	protected int niapRequestCode;

	protected string payLoad;

	public NIAPParamPayment(string _productCode, int _niapRequestCode, string _payLoad) : base(NIAPConstant.InvokeMethod.requestPayment)
	{
		this.productCode = _productCode;
		this.niapRequestCode = _niapRequestCode;
		this.payLoad = _payLoad;
	}

	public override string toString()
	{
		JSONClass jSONClass = new JSONClass();
		jSONClass[NIAPConstant.invokeMethod] = this.invokeMethod;
		jSONClass[NIAPConstant.Param.productCode] = this.productCode;
		jSONClass[NIAPConstant.Param.niapRequestCode] = this.niapRequestCode.ToString();
		jSONClass[NIAPConstant.Param.payLoad] = this.payLoad;
		return jSONClass.ToString();
	}
}
