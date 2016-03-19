using System;
using System.Reflection;
using UnityEngine;

public class NrCharInfoAdaptor : MonoBehaviour
{
	private INrCharInfo mi_CharInfo;

	private static INrCharInfo mi_NullCharInfo;

	public int charunique;

	public int charkind;

	public Vector3 makepos = Vector3.zero;

	public INrCharInfo CharInfo
	{
		get
		{
			return this.mi_CharInfo;
		}
	}

	private void Start()
	{
		if (NrCharInfoAdaptor.mi_NullCharInfo == null)
		{
			NrCharInfoAdaptor.mi_NullCharInfo = new NrCharInfoDefault();
		}
		if (this.mi_CharInfo == null)
		{
			this.mi_CharInfo = NrCharInfoAdaptor.mi_NullCharInfo;
		}
	}

	public void SetCharInfoInterface(INrCharInfo charInfo)
	{
		this.mi_CharInfo = charInfo;
		if (this.mi_CharInfo == null)
		{
			this.mi_CharInfo = NrCharInfoAdaptor.mi_NullCharInfo;
		}
	}

	public string GetMethodValue(string methodName)
	{
		Type type = this.mi_CharInfo.GetType();
		MethodInfo method = type.GetMethod(methodName);
		if (method == null)
		{
			return string.Empty;
		}
		return (string)method.Invoke(this.mi_CharInfo, null);
	}
}
