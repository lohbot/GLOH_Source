using SimpleJSON;
using System;
using System.Collections.Generic;

public class NIAPParamProductInfos : NIAPParam
{
	protected List<string> productCodes;

	public NIAPParamProductInfos(List<string> _productCodes) : base(NIAPConstant.InvokeMethod.getProductDetails)
	{
		this.productCodes = _productCodes;
	}

	public override string toString()
	{
		JSONClass jSONClass = new JSONClass();
		jSONClass[NIAPConstant.invokeMethod] = this.invokeMethod;
		for (int i = 0; i < this.productCodes.Count; i++)
		{
			jSONClass[NIAPConstant.Param.productCodes][i] = this.productCodes[i];
		}
		return jSONClass.ToString();
	}
}
