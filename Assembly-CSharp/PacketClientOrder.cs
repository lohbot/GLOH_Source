using System;
using System.Reflection;
using UnityEngine;

public class PacketClientOrder
{
	public string DEBUG_FUNC_NAME = string.Empty;

	public MethodInfo Method;

	public object[] Parameters;

	public PacketClientOrder(string _FuncName, object[] _parameters)
	{
		this.Create(_FuncName, _parameters);
	}

	public PacketClientOrder(string _FuncName, object _parameters)
	{
		object[] parameters = new object[]
		{
			_parameters
		};
		this.Create(_FuncName, parameters);
	}

	public PacketClientOrder(object _parameters)
	{
		string funcName = _parameters.GetType().ToString();
		object[] parameters = new object[]
		{
			_parameters
		};
		this.Create(funcName, parameters);
	}

	private void Create(string _FuncName, object[] _parameters)
	{
		int num = _parameters.Length;
		Type[] array = new Type[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = _parameters[i].GetType();
		}
		this.DEBUG_FUNC_NAME = _FuncName;
		this.Method = typeof(Battle).GetMethod(this.DEBUG_FUNC_NAME, array);
		this.Parameters = _parameters;
	}

	public void DEBUG_LOG()
	{
		Debug.LogWarning(string.Concat(new object[]
		{
			"---PopPacket METHOD:",
			this.DEBUG_FUNC_NAME,
			": ",
			Time.time,
			" :",
			this.Parameters.Length
		}));
		Debug.Log(string.Concat(new object[]
		{
			" Method  N?",
			null == this.Method,
			" Parameters ",
			this.Parameters.Length,
			" ",
			this.Parameters.GetType()
		}));
		object[] parameters = this.Parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			object obj = parameters[i];
			Debug.Log(string.Concat(new object[]
			{
				"PopPacket PARAMAS:",
				obj,
				" T: ",
				obj.GetType(),
				" :N:",
				null == obj
			}));
		}
		if (this.Method != null)
		{
			Debug.Log(string.Concat(new object[]
			{
				" Method NAME :: ",
				this.Method.Name,
				" N?",
				null == this.Method
			}));
		}
	}
}
