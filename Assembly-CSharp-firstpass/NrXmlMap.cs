using System;
using System.Collections.Generic;
using UnityEngine;

public class NrXmlMap
{
	public List<string> kXmlValList = new List<string>();

	public NrXmlMap()
	{
		this.Init();
	}

	public void Init()
	{
		this.kXmlValList.Clear();
	}

	public void SetData(int _index, out byte _Variable)
	{
		_Variable = 0;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = byte.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError("NrXmlMap SetData Overflow Byte Value : " + this.kXmlValList[_index] + "\r\nMessage : " + ex.Message);
			}
		}
	}

	public void SetData(int _index, out short _Variable)
	{
		_Variable = 0;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = short.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"NrXmlMap SetData Overflow ",
					_Variable.GetType(),
					" Value : ",
					this.kXmlValList[_index],
					"\r\nMessage : ",
					ex.Message
				}));
			}
		}
	}

	public void SetData(int _index, out int _Variable)
	{
		_Variable = 0;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = int.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"NrXmlMap SetData Overflow ",
					_Variable.GetType(),
					" Value : ",
					this.kXmlValList[_index],
					"\r\nMessage : ",
					ex.Message
				}));
			}
			catch (Exception ex2)
			{
				Debug.Log(ex2);
				Debug.Log(ex2.InnerException);
			}
		}
	}

	public void SetData(int _index, out long _Variable)
	{
		_Variable = 0L;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = long.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"NrXmlMap SetData Overflow ",
					_Variable.GetType(),
					" Value : ",
					this.kXmlValList[_index],
					"\r\nMessage : ",
					ex.Message
				}));
			}
		}
	}

	public void SetData(int _index, out float _Variable)
	{
		_Variable = 0f;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = float.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"NrXmlMap SetData Overflow ",
					_Variable.GetType(),
					" Value : ",
					this.kXmlValList[_index],
					"\r\nMessage : ",
					ex.Message
				}));
			}
		}
	}

	public void SetData(int _index, out string _Variable)
	{
		if (_index < this.kXmlValList.Count)
		{
			_Variable = this.kXmlValList[_index];
		}
		else
		{
			_Variable = string.Empty;
		}
	}

	public void SetData(int _index, out ushort _Variable)
	{
		_Variable = 0;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = ushort.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"NrXmlMap SetData Overflow ",
					_Variable.GetType(),
					" Value : ",
					this.kXmlValList[_index],
					"\r\nMessage : ",
					ex.Message
				}));
			}
		}
	}

	public void SetData(int _index, out uint _Variable)
	{
		_Variable = 0u;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = uint.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"NrXmlMap SetData Overflow ",
					_Variable.GetType(),
					" Value : ",
					this.kXmlValList[_index],
					"\r\nMessage : ",
					ex.Message
				}));
			}
		}
	}

	public void SetData(int _index, out ulong _Variable)
	{
		_Variable = 0uL;
		if (_index < this.kXmlValList.Count && string.Empty != this.kXmlValList[_index])
		{
			try
			{
				_Variable = ulong.Parse(this.kXmlValList[_index]);
			}
			catch (OverflowException ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"NrXmlMap SetData Error ",
					_Variable.GetType(),
					"Value : ",
					this.kXmlValList[_index],
					"/",
					ex.Message
				}));
			}
		}
	}
}
