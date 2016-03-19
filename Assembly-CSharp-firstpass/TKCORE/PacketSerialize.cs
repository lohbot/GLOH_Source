using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TKCORE
{
	public class PacketSerialize
	{
		public static bool btestLog;

		public static byte[] Serialize(object _OBJ)
		{
			Type type = _OBJ.GetType();
			ByteBlock byteBlock = new ByteBlock();
			byte[] array = null;
			byte[] array2 = null;
			byte[] array3 = null;
			if (type.IsArray)
			{
				array2 = PacketSerialize.SerializeFieldArray(_OBJ);
			}
			else if (PacketSerialize.IsFieldsType(type))
			{
				array = PacketSerialize.SerializeClass(_OBJ);
			}
			else
			{
				array3 = PacketSerialize.SerializeFieldBase(_OBJ);
			}
			byteBlock.Add(array);
			byteBlock.Add(array2);
			byteBlock.Add(array3);
			string text = string.Concat(new object[]
			{
				"SerializedType::::::",
				_OBJ.ToString(),
				" T: ",
				type
			});
			if (array != null)
			{
				text = text + " C:" + array.Length;
			}
			if (array2 != null)
			{
				text = text + " A:" + array2.Length;
			}
			if (array3 != null)
			{
				text = text + " B:" + array3.Length;
			}
			byte[] alloc = byteBlock.GetAlloc();
			if (PacketSerialize.btestLog)
			{
				byte[] array4 = alloc;
				text += "BufferS:";
				byte[] array5 = array4;
				for (int i = 0; i < array5.Length; i++)
				{
					byte b = array5[i];
					text = text + " " + b;
				}
				Debug.Log(text);
			}
			return alloc;
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

		private static byte[] SerializeClass(object _OBJ)
		{
			Type type = _OBJ.GetType();
			FieldInfo[] fields = type.GetFields();
			ByteBlock byteBlock = new ByteBlock();
			if (PacketSerialize.btestLog)
			{
				Debug.Log(string.Concat(new object[]
				{
					"-------Serialize GetFields------",
					fields.Length,
					" NAME:",
					type.Name
				}));
			}
			int num = 0;
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				object value = fieldInfo.GetValue(_OBJ);
				Type type2 = value.GetType();
				byte[] array2;
				if (PacketSerialize.IsFieldsType(type2))
				{
					array2 = PacketSerialize.Serialize(value);
				}
				else
				{
					array2 = PacketSerialize.SerializeFieldBase(value);
				}
				byteBlock.Add(array2);
				if (PacketSerialize.btestLog)
				{
					string text = string.Empty;
					text += num++;
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"Class:",
						fieldInfo.DeclaringType,
						" Name:",
						fieldInfo.Name,
						" Type:",
						fieldInfo.FieldType,
						" Value:",
						value.ToString()
					});
					if (array2 != null && 0 < array2.Length)
					{
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							" Buffer:",
							array2.Length,
							"--"
						});
						byte[] array3 = array2;
						for (int j = 0; j < array3.Length; j++)
						{
							byte b = array3[j];
							text = text + b.ToString() + " ";
						}
					}
					Debug.Log(text);
				}
			}
			return byteBlock.GetAlloc();
		}

		private static byte[] SerializeFieldArray(object _Context)
		{
			byte[] result;
			if (typeof(char[]) == _Context.GetType())
			{
				result = PacketSerialize.SerializeFieldBaseArrayChar(_Context as char[]);
			}
			else
			{
				result = PacketSerialize.SerializeFieldBaseArray(_Context as Array);
			}
			return result;
		}

		private static byte[] SerializeFieldBaseArray(Array _Contexts)
		{
			ByteBlock byteBlock = new ByteBlock();
			foreach (object current in _Contexts)
			{
				byte[] buffer = PacketSerialize.Serialize(current);
				byteBlock.Add(buffer);
			}
			return byteBlock.GetAlloc();
		}

		private static byte[] SerializeFieldBaseArrayChar(char[] _szChar)
		{
			byte[] array = new byte[_szChar.Length * 2];
			Encoding unicode = Encoding.Unicode;
			unicode.GetBytes(_szChar, 0, _szChar.Length, array, 0);
			return array;
		}

		private static byte[] SerializeFieldBase(object _Context)
		{
			byte[] result = null;
			Type type = _Context.GetType();
			if (PacketSerialize.btestLog)
			{
				Debug.Log(string.Concat(new object[]
				{
					"ContextType:",
					type,
					" _Context:",
					_Context
				}));
			}
			if (type == typeof(byte))
			{
				result = new byte[]
				{
					(byte)_Context
				};
			}
			else if (type == typeof(sbyte))
			{
				sbyte b = (sbyte)_Context;
				result = new byte[]
				{
					(byte)b
				};
			}
			else if (type == typeof(bool))
			{
				result = BitConverter.GetBytes((bool)_Context);
			}
			else if (type == typeof(char))
			{
				result = BitConverter.GetBytes((char)_Context);
			}
			else if (type == typeof(double))
			{
				result = BitConverter.GetBytes((double)_Context);
			}
			else if (type == typeof(float))
			{
				result = BitConverter.GetBytes((float)_Context);
			}
			else if (type == typeof(int))
			{
				result = BitConverter.GetBytes((int)_Context);
			}
			else if (type == typeof(long))
			{
				result = BitConverter.GetBytes((long)_Context);
			}
			else if (type == typeof(short))
			{
				result = BitConverter.GetBytes((short)_Context);
			}
			else if (type == typeof(uint))
			{
				result = BitConverter.GetBytes((uint)_Context);
			}
			else if (type == typeof(ulong))
			{
				result = BitConverter.GetBytes((ulong)_Context);
			}
			else if (type == typeof(ushort))
			{
				result = BitConverter.GetBytes((ushort)_Context);
			}
			else if (type.IsEnum)
			{
				result = new byte[]
				{
					(byte)_Context
				};
			}
			else
			{
				Debug.LogWarning("SerializeFieldBase:not Surpport!!!!" + type.ToString());
				if (type.IsEnum)
				{
					Debug.LogWarning(type + "USE Enum : byte ");
				}
			}
			return result;
		}
	}
}
