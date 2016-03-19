using System;
using System.Collections;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TKCORE
{
	public class PacketDeSerialize
	{
		public static bool btestLog;

		public static int DeSerialize(object _OBJ, byte[] _Buffer, int _Index)
		{
			try
			{
				Type type = _OBJ.GetType();
				if (type.IsArray)
				{
					_Index = PacketDeSerialize.DeSerializeFieldArray(_OBJ, _Buffer, _Index);
				}
				else
				{
					_Index = PacketDeSerialize.DeSerializeClass(_OBJ, _Buffer, _Index);
				}
			}
			catch (Exception ex)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Type:",
					_OBJ.GetType(),
					" Index:",
					_Index,
					" Buffer:",
					_Buffer.Length
				}));
				Debug.Log("DeSerialize Exception " + ex.Message);
			}
			return _Index;
		}

		public static object DeSerializeType(Type _Type, byte[] _Buffer, ref int _Index)
		{
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(_Type);
				_Index = PacketDeSerialize.DeSerialize(obj, _Buffer, _Index);
			}
			catch (Exception ex)
			{
				Debug.Log("ex" + ex.Message);
			}
			return obj;
		}

		private static int DeSerializeFieldArray(object _OBJ, byte[] _Buffer, int _Index)
		{
			Type type = _OBJ.GetType();
			Array array = (Array)_OBJ;
			if (typeof(char[]) == type)
			{
				_Index = PacketDeSerialize.DeSerializeFieldArrayChar(array, _Buffer, _Index);
			}
			else
			{
				_Index = PacketDeSerialize.DeSerializeFieldArrayBase(array, _Buffer, _Index);
			}
			if (PacketDeSerialize.btestLog)
			{
				string text = string.Concat(new object[]
				{
					"Class:",
					type.DeclaringType,
					" Name:",
					type.Name,
					" Type:",
					type,
					" AllayLength:",
					array.Length,
					":"
				});
				foreach (object current in (_OBJ as IList))
				{
					text = text + " " + current;
				}
				Debug.Log(text);
			}
			return _Index;
		}

		private static int DeSerializeFieldArrayBase(Array _Contexts, byte[] _Buffer, int _Index)
		{
			int num = 0;
			foreach (object current in _Contexts)
			{
				Type type = current.GetType();
				object value;
				if (PacketDeSerialize.IsFieldsType(type))
				{
					_Index = PacketDeSerialize.DeSerialize(current, _Buffer, _Index);
					value = current;
				}
				else
				{
					value = PacketDeSerialize.DeSerializeValue(_Buffer, ref _Index, current);
				}
				_Contexts.SetValue(value, num++);
			}
			return _Index;
		}

		private static int DeSerializeFieldArrayChar(Array _Contexts, byte[] _Buffer, int _Index)
		{
			int num = _Contexts.Length * 2;
			Encoding unicode = Encoding.Unicode;
			Array chars = unicode.GetChars(_Buffer, _Index, num);
			Array.Copy(chars, _Contexts, chars.Length);
			return _Index + num;
		}

		private static bool IsFieldsType(Type _Type)
		{
			return _Type.IsClass || (_Type.IsValueType && !_Type.IsPrimitive && !_Type.IsEnum);
		}

		private static object GetFieldValue(FieldInfo fInfo, object _OBJ)
		{
			object value = fInfo.GetValue(_OBJ);
			if (value == null)
			{
				Debug.LogError("Alloc:NULL:T:" + fInfo.FieldType);
			}
			return value;
		}

		private static int DeSerializeClass(object _OBJ, byte[] _Buffer, int _Index)
		{
			Type type = _OBJ.GetType();
			FieldInfo[] fields = type.GetFields();
			if (PacketDeSerialize.btestLog)
			{
				Debug.Log(string.Concat(new object[]
				{
					"-------DeSerialize Fields------",
					fields.Length,
					" ",
					type.Name
				}));
			}
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				object value = fieldInfo.GetValue(_OBJ);
				Type type2 = value.GetType();
				if (PacketDeSerialize.IsFieldsType(type2))
				{
					_Index = PacketDeSerialize.DeSerialize(value, _Buffer, _Index);
				}
				else
				{
					_Index = PacketDeSerialize.DeSerializeFieldBase(_OBJ, _Buffer, _Index, fieldInfo);
				}
			}
			return _Index;
		}

		private static int DeSerializeFieldBase(object _OBJ, byte[] _Buffer, int _Index, FieldInfo _Field)
		{
			object value = _Field.GetValue(_OBJ);
			object obj = null;
			if (value != null)
			{
				obj = PacketDeSerialize.DeSerializeValue(_Buffer, ref _Index, value);
				_Field.SetValue(_OBJ, obj);
			}
			if (PacketDeSerialize.btestLog)
			{
				string message = string.Concat(new object[]
				{
					"Class:",
					_Field.DeclaringType,
					" Name:",
					_Field.Name,
					" Type:",
					_Field.FieldType,
					" Value:",
					obj.ToString()
				});
				Debug.Log(message);
			}
			return _Index;
		}

		private static object DeSerializeValue(byte[] _Buffer, ref int _Index, object _Context)
		{
			Type type = _Context.GetType();
			object result = null;
			if (type == typeof(byte))
			{
				result = _Buffer[_Index];
				_Index++;
			}
			else if (type == typeof(sbyte))
			{
				result = (sbyte)_Buffer[_Index];
				_Index++;
			}
			else if (type == typeof(bool))
			{
				result = BitConverter.ToBoolean(_Buffer, _Index);
				_Index++;
			}
			else if (type == typeof(double))
			{
				result = BitConverter.ToDouble(_Buffer, _Index);
				_Index += 8;
			}
			else if (type == typeof(float))
			{
				result = BitConverter.ToSingle(_Buffer, _Index);
				_Index += 4;
			}
			else if (type == typeof(int))
			{
				result = BitConverter.ToInt32(_Buffer, _Index);
				_Index += 4;
			}
			else if (type == typeof(long))
			{
				result = BitConverter.ToInt64(_Buffer, _Index);
				_Index += 8;
			}
			else if (type == typeof(short))
			{
				result = BitConverter.ToInt16(_Buffer, _Index);
				_Index += 2;
			}
			else if (type == typeof(uint))
			{
				result = BitConverter.ToUInt32(_Buffer, _Index);
				_Index += 4;
			}
			else if (type == typeof(ulong))
			{
				result = BitConverter.ToUInt64(_Buffer, _Index);
				_Index += 8;
			}
			else if (type == typeof(ushort))
			{
				result = BitConverter.ToUInt16(_Buffer, _Index);
				_Index += 2;
			}
			else if (type.IsEnum)
			{
				result = _Buffer[_Index];
				_Index++;
			}
			else
			{
				Debug.LogWarning("DeSerializeValue:not Surpport!!!!" + type.ToString());
			}
			return result;
		}
	}
}
