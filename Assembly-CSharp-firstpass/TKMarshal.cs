using System;
using TKCORE;
using UnityEngine;

public class TKMarshal
{
	public static byte[] Serialize(object _Target)
	{
		return PacketSerialize.Serialize(_Target);
	}

	public static int DeSerialize(byte[] _Buffer, int _Index, object _OBJ)
	{
		return PacketDeSerialize.DeSerialize(_OBJ, _Buffer, _Index) - _Index;
	}

	public static object DeSerializeType(byte[] _Buffer, int _Index, out int _Size, Type _Type)
	{
		int num = _Index;
		object result = PacketDeSerialize.DeSerializeType(_Type, _Buffer, ref _Index);
		_Size = _Index - num;
		return result;
	}

	public static char[] StringChar(string _Src, ref char[] _Des)
	{
		char[] array = _Src.ToCharArray();
		int num = Mathf.Min(_Des.Length, array.Length);
		for (int i = 0; i < num; i++)
		{
			_Des[i] = _Src[i];
		}
		return _Des;
	}
}
