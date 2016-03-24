using SimpleJSON;
using System;
using UnityEngine;

public class NIAPParam : MonoBehaviour
{
	protected string invokeMethod;

	public NIAPParam(string _invokeMethod)
	{
		this.invokeMethod = _invokeMethod;
	}

	public string getInvokeMethod()
	{
		return this.invokeMethod;
	}

	public virtual string toString()
	{
		JSONClass jSONClass = new JSONClass();
		jSONClass[NIAPConstant.invokeMethod] = this.invokeMethod;
		return jSONClass.ToString();
	}
}
