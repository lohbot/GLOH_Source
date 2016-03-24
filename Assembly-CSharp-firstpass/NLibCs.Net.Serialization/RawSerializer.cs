using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace NLibCs.Net.Serialization
{
	public class RawSerializer
	{
		private struct OwnerFieldPair
		{
			public object owner;

			public object field;

			public OwnerFieldPair(object owner, object field)
			{
				this.owner = owner;
				this.field = field;
			}
		}

		private static byte[] mRawData = new byte[32768];

		private Type mTargetType;

		private int mObjectSize;

		public static int BufferSize
		{
			get
			{
				return RawSerializer.mRawData.Length;
			}
			set
			{
				byte[] obj = RawSerializer.mRawData;
				lock (obj)
				{
					int num = RawSerializer.mRawData.Length;
					if (value != num)
					{
						byte[] dst = new byte[value];
						Buffer.BlockCopy(RawSerializer.mRawData, 0, dst, 0, Math.Min(num, value));
						RawSerializer.mRawData = dst;
					}
				}
			}
		}

		public bool UsingFixedSize
		{
			get;
			set;
		}

		public RawSerializer(Type targetType) : this(targetType, false)
		{
		}

		public RawSerializer(Type targetType, short targetSize)
		{
			this.mTargetType = targetType;
			this.mObjectSize = (int)targetSize;
			this.UsingFixedSize = false;
		}

		public RawSerializer(Type targetType, bool useFixedBufferSize)
		{
			this.mTargetType = targetType;
			this.mObjectSize = RawSerializer.GetObjectSize(this.mTargetType);
			this.UsingFixedSize = useFixedBufferSize;
		}

		private bool Deserialize(byte[] source, int baseIndex, object target, out int writtenBytes)
		{
			if (target == null)
			{
				writtenBytes = 0;
				return false;
			}
			Type type = target.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			int num = baseIndex;
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (!RawSerializer.IsIngoreField(fieldInfo))
				{
					Type fieldType = fieldInfo.FieldType;
					object obj = null;
					if (fieldType.IsArray)
					{
						Array array2 = null;
						LengthAttribute lengthAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(LengthAttribute)) as LengthAttribute;
						if (lengthAttribute != null)
						{
							if (lengthAttribute.LengthField != null)
							{
								FieldInfo lengthField = this.GetLengthField(fields, lengthAttribute.LengthField);
								if (lengthField != null)
								{
									object value = lengthField.GetValue(target);
									Type type2 = value.GetType();
									if (type2 == typeof(byte))
									{
										lengthAttribute.Length = (int)((byte)value);
									}
									else if (type2 == typeof(short))
									{
										lengthAttribute.Length = (int)((short)value);
									}
									else if (type2 == typeof(int))
									{
										lengthAttribute.Length = (int)value;
									}
								}
							}
							array2 = (fieldInfo.GetValue(target) as Array);
							lengthAttribute.ResizeArray(fieldType, ref array2);
						}
						if (fieldType == typeof(bool[]))
						{
							obj = this.BytesToArray<bool>(source, ref num, array2, 1, new Func<byte[], int, bool>(BitConverter.ToBoolean));
						}
						else if (fieldType == typeof(char[]))
						{
							obj = this.BytesToChars(source, ref num, array2);
						}
						else if (fieldType == typeof(byte[]))
						{
							obj = this.BytesToArray<byte>(source, ref num, array2, 1, (byte[] buf, int off) => buf[off]);
						}
						else if (fieldType == typeof(sbyte[]))
						{
							obj = this.BytesToArray<byte>(source, ref num, array2, 1, (byte[] buf, int off) => buf[off]);
						}
						else if (fieldType == typeof(short[]))
						{
							obj = this.BytesToArray<short>(source, ref num, array2, 2, new Func<byte[], int, short>(BitConverter.ToInt16));
						}
						else if (fieldType == typeof(ushort[]))
						{
							obj = this.BytesToArray<ushort>(source, ref num, array2, 2, new Func<byte[], int, ushort>(BitConverter.ToUInt16));
						}
						else if (fieldType == typeof(int[]))
						{
							obj = this.BytesToArray<int>(source, ref num, array2, 4, new Func<byte[], int, int>(BitConverter.ToInt32));
						}
						else if (fieldType == typeof(uint[]))
						{
							obj = this.BytesToArray<uint>(source, ref num, array2, 4, new Func<byte[], int, uint>(BitConverter.ToUInt32));
						}
						else if (fieldType == typeof(long[]))
						{
							obj = this.BytesToArray<long>(source, ref num, array2, 8, new Func<byte[], int, long>(BitConverter.ToInt64));
						}
						else if (fieldType == typeof(ulong[]))
						{
							obj = this.BytesToArray<ulong>(source, ref num, array2, 8, new Func<byte[], int, ulong>(BitConverter.ToUInt64));
						}
						else if (fieldType == typeof(float[]))
						{
							obj = this.BytesToArray<float>(source, ref num, array2, 4, new Func<byte[], int, float>(BitConverter.ToSingle));
						}
						else if (fieldType == typeof(double[]))
						{
							obj = this.BytesToArray<double>(source, ref num, array2, 8, new Func<byte[], int, double>(BitConverter.ToDouble));
						}
						else if (!fieldType.GetElementType().IsPrimitive)
						{
							Array array3 = fieldInfo.GetValue(target) as Array;
							if (array3 == null || array3.Length != array2.Length)
							{
								array3 = array2;
							}
							int num2 = 0;
							foreach (object current in array3)
							{
								int num3;
								this.Deserialize(source, num, current, out num3);
								array3.SetValue(current, num2++);
								num += num3;
							}
							fieldInfo.SetValue(target, array3);
						}
					}
					else if (fieldType == typeof(bool))
					{
						obj = BitConverter.ToBoolean(source, num);
						num++;
					}
					else if (fieldType == typeof(int))
					{
						obj = BitConverter.ToInt32(source, num);
						num += 4;
					}
					else if (fieldType == typeof(uint))
					{
						obj = BitConverter.ToUInt32(source, num);
						num += 4;
					}
					else if (fieldType == typeof(short))
					{
						obj = BitConverter.ToInt16(source, num);
						num += 2;
					}
					else if (fieldType == typeof(ushort))
					{
						obj = BitConverter.ToUInt16(source, num);
						num += 2;
					}
					else if (fieldType == typeof(long))
					{
						obj = BitConverter.ToInt64(source, num);
						num += 8;
					}
					else if (fieldType == typeof(ulong))
					{
						obj = BitConverter.ToUInt64(source, num);
						num += 8;
					}
					else if (fieldType == typeof(byte))
					{
						obj = source[num++];
					}
					else if (fieldType == typeof(sbyte))
					{
						obj = (sbyte)source[num++];
					}
					else if (fieldType == typeof(char))
					{
						obj = BitConverter.ToChar(source, num);
						num += 2;
					}
					else if (fieldType == typeof(float))
					{
						obj = BitConverter.ToSingle(source, num);
						num += 4;
					}
					else if (fieldType == typeof(double))
					{
						obj = BitConverter.ToDouble(source, num);
						num += 8;
					}
					else if (fieldType == typeof(string))
					{
						char[] orgVal = null;
						LengthAttribute lengthAttribute2 = Attribute.GetCustomAttribute(fieldInfo, typeof(LengthAttribute)) as LengthAttribute;
						if (lengthAttribute2 != null)
						{
							lengthAttribute2.ResizeArray<char>(ref orgVal);
						}
						char[] array4 = this.BytesToChars(source, ref num, orgVal);
						string text = new string(array4, 0, this.GetCompactLength(array4));
						obj = text;
					}
					else if (!fieldType.IsPrimitive)
					{
						object value2 = fieldInfo.GetValue(target);
						int num4;
						this.Deserialize(source, num, value2, out num4);
						fieldInfo.SetValue(target, value2);
						num += num4;
					}
					if (obj != null)
					{
						fieldInfo.SetValue(target, obj);
					}
				}
			}
			writtenBytes = num - baseIndex;
			return writtenBytes > 0;
		}

		private FieldInfo GetLengthField(FieldInfo[] fieldInfoList, string lengthField)
		{
			for (int i = 0; i < fieldInfoList.Length; i++)
			{
				FieldInfo fieldInfo = fieldInfoList[i];
				if (fieldInfo.Name == lengthField)
				{
					return fieldInfo;
				}
			}
			return null;
		}

		private int GetCompactLength(char[] chars)
		{
			for (int i = 0; i < chars.Length; i++)
			{
				if (chars[i] == '\0')
				{
					return i;
				}
			}
			return chars.Length;
		}

		private char[] BytesToChars(byte[] buffer, ref int offset, Array orgVal)
		{
			ushort num;
			char[] array;
			if (orgVal == null)
			{
				num = BitConverter.ToUInt16(buffer, offset);
				offset += 2;
				array = new char[(int)num];
			}
			else
			{
				num = (ushort)orgVal.Length;
				array = (orgVal as char[]);
			}
			Decoder decoder = Encoding.Unicode.GetDecoder();
			decoder.GetChars(buffer, offset, (int)(num * 2), array, 0);
			offset += (int)(num * 2);
			return array;
		}

		private T[] BytesToArray<T>(byte[] buffer, ref int offset, Array orgVal, int typeSize, Func<byte[], int, T> convertor) where T : new()
		{
			ushort num;
			T[] array;
			if (orgVal == null)
			{
				num = BitConverter.ToUInt16(buffer, offset);
				offset += 2;
				array = new T[(int)num];
			}
			else
			{
				num = (ushort)orgVal.Length;
				array = (orgVal as T[]);
			}
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = convertor(buffer, offset);
				offset += typeSize;
			}
			return array;
		}

		public bool Deserialize(object target, Stream source)
		{
			if (target.GetType() != this.mTargetType)
			{
				return false;
			}
			bool result;
			try
			{
				int num = (!this.UsingFixedSize) ? this.mObjectSize : -1;
				if (num < 0)
				{
					num = RawSerializer.mRawData.Length;
				}
				bool flag = false;
				byte[] obj = RawSerializer.mRawData;
				lock (obj)
				{
					source.Read(RawSerializer.mRawData, 0, num);
					int num2;
					flag = this.Deserialize(RawSerializer.mRawData, 0, target, out num2);
				}
				result = flag;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool Deserialize<T>(ref T target, Stream source) where T : struct
		{
			if (target.GetType() != this.mTargetType)
			{
				return false;
			}
			object obj = target;
			if (this.Deserialize(obj, source))
			{
				target = (T)((object)obj);
				return true;
			}
			return false;
		}

		public object Deserialize(Stream source)
		{
			ConstructorInfo constructor = this.mTargetType.GetConstructor(Type.EmptyTypes);
			object obj = (constructor == null) ? Activator.CreateInstance(this.mTargetType) : constructor.Invoke(null);
			return (!this.Deserialize(obj, source)) ? null : obj;
		}

		public T Deserialize<T>(Stream source) where T : new()
		{
			if (typeof(T) != this.mTargetType)
			{
				return default(T);
			}
			T t = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
			object obj = t;
			return (!this.Deserialize(obj, source)) ? default(T) : ((T)((object)obj));
		}

		public new static bool Equals(object left, object right)
		{
			return left.GetType() == right.GetType() && RawSerializer.EqualFields(new RawSerializer.OwnerFieldPair(left, null), new RawSerializer.OwnerFieldPair(right, null));
		}

		private static bool EqualFields(RawSerializer.OwnerFieldPair left, RawSerializer.OwnerFieldPair right)
		{
			if (left.field != null && right.field != null)
			{
				if (!object.Equals(left.field, right.field))
				{
					return false;
				}
			}
			else
			{
				Type type = left.owner.GetType();
				Type type2 = right.owner.GetType();
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo[] fields2 = type2.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					FieldInfo fieldInfo = fields[i];
					FieldInfo fieldInfo2 = fields2[i];
					if (fieldInfo == null || fieldInfo2 == null)
					{
						return false;
					}
					if (!RawSerializer.IsIngoreField(fieldInfo))
					{
						if (fieldInfo.FieldType.IsArray)
						{
							Array array = fieldInfo.GetValue(left.owner) as Array;
							Array array2 = fieldInfo2.GetValue(right.owner) as Array;
							int num = Math.Min(array.Length, array2.Length);
							for (int j = 0; j < num; j++)
							{
								RawSerializer.OwnerFieldPair left2 = new RawSerializer.OwnerFieldPair(left.owner, array.GetValue(j));
								RawSerializer.OwnerFieldPair right2 = new RawSerializer.OwnerFieldPair(right.owner, array2.GetValue(j));
								if (!RawSerializer.EqualFields(left2, right2))
								{
									return false;
								}
							}
						}
						else
						{
							object value = fieldInfo.GetValue(left.owner);
							object value2 = fieldInfo2.GetValue(right.owner);
							RawSerializer.OwnerFieldPair left3;
							RawSerializer.OwnerFieldPair right3;
							if (type.IsPrimitive || type == typeof(string))
							{
								left3 = new RawSerializer.OwnerFieldPair(left.owner, value);
								right3 = new RawSerializer.OwnerFieldPair(right.owner, value2);
							}
							else
							{
								left3 = new RawSerializer.OwnerFieldPair(value, null);
								right3 = new RawSerializer.OwnerFieldPair(value2, null);
							}
							if (!RawSerializer.EqualFields(left3, right3))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		private static bool TryGetPrimitiveSize(Type type, out int size)
		{
			size = 0;
			if (type == typeof(int))
			{
				size = 4;
			}
			else if (type == typeof(uint))
			{
				size = 4;
			}
			else if (type == typeof(short))
			{
				size = 2;
			}
			else if (type == typeof(ushort))
			{
				size = 2;
			}
			else if (type == typeof(long))
			{
				size = 8;
			}
			else if (type == typeof(ulong))
			{
				size = 8;
			}
			else if (type == typeof(byte))
			{
				size = 1;
			}
			else if (type == typeof(sbyte))
			{
				size = 1;
			}
			else if (type == typeof(float))
			{
				size = 4;
			}
			else if (type == typeof(double))
			{
				size = 8;
			}
			else if (type == typeof(char))
			{
				size = 2;
			}
			else
			{
				if (type != typeof(bool))
				{
					return false;
				}
				size = 1;
			}
			return true;
		}

		private static int GetFieldSize(Type type, FieldInfo ownerFieldInfo, object instance)
		{
			int num = 0;
			if (type.IsPrimitive)
			{
				if (!RawSerializer.TryGetPrimitiveSize(type, out num))
				{
					throw new UnknownLengthException(ownerFieldInfo);
				}
			}
			else if (type == typeof(string))
			{
				if (instance == null)
				{
					throw new UnknownLengthException(ownerFieldInfo);
				}
				num += 2;
				num += (instance as string).Length * 2;
			}
			else
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo[] array = fields;
				for (int i = 0; i < array.Length; i++)
				{
					FieldInfo fieldInfo = array[i];
					if (!RawSerializer.IsIngoreField(fieldInfo))
					{
						Type fieldType = fieldInfo.FieldType;
						LengthAttribute lengthAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(LengthAttribute)) as LengthAttribute;
						if (fieldType.IsArray)
						{
							if (lengthAttribute != null)
							{
								num += lengthAttribute.Length * RawSerializer.GetFieldSize(fieldType.GetElementType(), fieldInfo, instance);
							}
							else
							{
								if (instance == null)
								{
									throw new UnknownLengthException(fieldInfo);
								}
								int fieldSize = RawSerializer.GetFieldSize(fieldType.GetElementType(), fieldInfo, instance);
								Array array2 = fieldInfo.GetValue(instance) as Array;
								num += 2;
								num += array2.Length * fieldSize;
							}
						}
						else if (lengthAttribute != null && fieldType == typeof(string))
						{
							num += lengthAttribute.Length * 2;
						}
						else
						{
							num += RawSerializer.GetFieldSize(fieldType, fieldInfo, instance);
						}
					}
				}
			}
			return num;
		}

		public static int GetObjectSize(Type type)
		{
			return RawSerializer.GetObjectSize(type, null);
		}

		public static int GetObjectSize(Type type, object instance)
		{
			try
			{
				return RawSerializer.GetFieldSize(type, null, instance);
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			return -1;
		}

		private bool Serialize(object obj, int rawDataBaseIndex, out int writtenLength)
		{
			if (obj == null)
			{
				Console.WriteLine("Serialize() >> input object is null");
				writtenLength = 0;
				return false;
			}
			Type type = obj.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fields == null)
			{
				Console.WriteLine("Serialize() >> GetFileds() is null");
				writtenLength = 0;
				return false;
			}
			int num = rawDataBaseIndex;
			FieldInfo[] array = fields;
			for (int i = 0; i < array.Length; i++)
			{
				FieldInfo fieldInfo = array[i];
				if (!RawSerializer.IsIngoreField(fieldInfo))
				{
					Type fieldType = fieldInfo.FieldType;
					object value = fieldInfo.GetValue(obj);
					byte[] array2 = null;
					if (fieldType.IsArray)
					{
						Array array3 = value as Array;
						LengthAttribute lengthAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(LengthAttribute)) as LengthAttribute;
						if (lengthAttribute != null)
						{
							lengthAttribute.ResizeArray(fieldType, ref array3);
						}
						bool isWriteLength = lengthAttribute == null;
						if (fieldType == typeof(bool[]))
						{
							array2 = this.ArrayToBytes<bool>(array3 as bool[], isWriteLength, 1, new Func<bool, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(char[]))
						{
							array2 = this.CharsToBytes(array3 as char[], isWriteLength);
						}
						else if (fieldType == typeof(byte[]))
						{
							array2 = this.ArrayToBytes<byte>(array3 as byte[], isWriteLength, 1, (byte v) => new byte[]
							{
								v
							});
						}
						else if (fieldType == typeof(sbyte[]))
						{
							array2 = this.ArrayToBytes<sbyte>(array3 as sbyte[], isWriteLength, 1, (sbyte v) => new byte[]
							{
								(byte)v
							});
						}
						else if (fieldType == typeof(short[]))
						{
							array2 = this.ArrayToBytes<short>(array3 as short[], isWriteLength, 2, new Func<short, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(ushort[]))
						{
							array2 = this.ArrayToBytes<ushort>(array3 as ushort[], isWriteLength, 2, new Func<ushort, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(int[]))
						{
							array2 = this.ArrayToBytes<int>(array3 as int[], isWriteLength, 4, new Func<int, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(uint[]))
						{
							array2 = this.ArrayToBytes<uint>(array3 as uint[], isWriteLength, 4, new Func<uint, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(long[]))
						{
							array2 = this.ArrayToBytes<long>(array3 as long[], isWriteLength, 8, new Func<long, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(ulong[]))
						{
							array2 = this.ArrayToBytes<ulong>(array3 as ulong[], isWriteLength, 8, new Func<ulong, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(float[]))
						{
							array2 = this.ArrayToBytes<float>(array3 as float[], isWriteLength, 4, new Func<float, byte[]>(BitConverter.GetBytes));
						}
						else if (fieldType == typeof(double[]))
						{
							array2 = this.ArrayToBytes<double>(array3 as double[], isWriteLength, 8, new Func<double, byte[]>(BitConverter.GetBytes));
						}
						else if (!fieldType.GetElementType().IsPrimitive)
						{
							foreach (object current in array3)
							{
								int num2;
								this.Serialize(current, num, out num2);
								num += num2;
							}
						}
					}
					else if (fieldType == typeof(int))
					{
						array2 = BitConverter.GetBytes((int)value);
					}
					else if (fieldType == typeof(uint))
					{
						array2 = BitConverter.GetBytes((uint)value);
					}
					else if (fieldType == typeof(short))
					{
						array2 = BitConverter.GetBytes((short)value);
					}
					else if (fieldType == typeof(ushort))
					{
						array2 = BitConverter.GetBytes((ushort)value);
					}
					else if (fieldType == typeof(long))
					{
						array2 = BitConverter.GetBytes((long)value);
					}
					else if (fieldType == typeof(ulong))
					{
						array2 = BitConverter.GetBytes((ulong)value);
					}
					else if (fieldType == typeof(byte))
					{
						array2 = new byte[]
						{
							(byte)value
						};
					}
					else if (fieldType == typeof(sbyte))
					{
						sbyte b = (sbyte)value;
						array2 = new byte[]
						{
							(byte)b
						};
					}
					else if (fieldType == typeof(float))
					{
						array2 = BitConverter.GetBytes((float)value);
					}
					else if (fieldType == typeof(double))
					{
						array2 = BitConverter.GetBytes((double)value);
					}
					else if (fieldType == typeof(bool))
					{
						array2 = BitConverter.GetBytes((bool)value);
					}
					else if (fieldType == typeof(char))
					{
						array2 = BitConverter.GetBytes((char)value);
					}
					else if (fieldType == typeof(string))
					{
						string text = value as string;
						char[] chars = text.ToCharArray();
						LengthAttribute lengthAttribute2 = Attribute.GetCustomAttribute(fieldInfo, typeof(LengthAttribute)) as LengthAttribute;
						if (lengthAttribute2 != null)
						{
							lengthAttribute2.ResizeArray<char>(ref chars);
						}
						array2 = this.CharsToBytes(chars, lengthAttribute2 == null);
					}
					else if (!fieldType.IsPrimitive)
					{
						int num3;
						this.Serialize(value, num, out num3);
						num += num3;
					}
					if (array2 != null)
					{
						Buffer.BlockCopy(array2, 0, RawSerializer.mRawData, num, array2.Length);
						num += array2.Length;
					}
				}
			}
			writtenLength = num - rawDataBaseIndex;
			return writtenLength > 0;
		}

		private byte[] CharsToBytes(char[] chars, bool isWriteLength)
		{
			if (chars == null)
			{
				return null;
			}
			ushort num = (ushort)chars.Length;
			int num2 = (int)(num * 2);
			int num3 = 0;
			if (isWriteLength)
			{
				num2 += 2;
				num3 += 2;
			}
			byte[] array = new byte[num2];
			if (isWriteLength)
			{
				Buffer.BlockCopy(BitConverter.GetBytes(num), 0, array, 0, 2);
			}
			Encoder encoder = Encoding.Unicode.GetEncoder();
			encoder.GetBytes(chars, 0, chars.Length, array, num3, false);
			return array;
		}

		private byte[] ArrayToBytes<T>(T[] array, bool isWriteLength, int typeSize, Func<T, byte[]> convertor) where T : new()
		{
			if (array == null)
			{
				return null;
			}
			ushort num = (ushort)array.Length;
			int num2 = (int)num * typeSize;
			int num3 = 0;
			if (isWriteLength)
			{
				num2 += 2;
				num3 += 2;
			}
			byte[] array2 = new byte[num2];
			for (int i = 0; i < (int)num; i++)
			{
				Buffer.BlockCopy(convertor(array[i]), 0, array2, i * typeSize + num3, typeSize);
			}
			if (isWriteLength)
			{
				Buffer.BlockCopy(BitConverter.GetBytes(num), 0, array2, 0, 2);
			}
			return array2;
		}

		public bool Serialize(object source, Stream target)
		{
			int num;
			return this.Serialize(source, target, out num);
		}

		public bool Serialize(object source, Stream target, out int writtenLength)
		{
			if (source.GetType() != this.mTargetType)
			{
				writtenLength = 0;
				return false;
			}
			try
			{
				byte[] obj = RawSerializer.mRawData;
				lock (obj)
				{
					if (this.Serialize(source, 0, out writtenLength))
					{
						target.Write(RawSerializer.mRawData, 0, (!this.UsingFixedSize) ? writtenLength : RawSerializer.mRawData.Length);
						return true;
					}
				}
			}
			catch (IOException value)
			{
				Console.WriteLine(value);
			}
			writtenLength = 0;
			return false;
		}

		private static bool IsIngoreField(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsPrivate)
			{
				if (Attribute.GetCustomAttribute(fieldInfo, typeof(SerializedFieldAttribute)) == null)
				{
					return true;
				}
			}
			else if (Attribute.GetCustomAttribute(fieldInfo, typeof(NonSerializedFieldAttribute)) != null)
			{
				return true;
			}
			return false;
		}
	}
}
