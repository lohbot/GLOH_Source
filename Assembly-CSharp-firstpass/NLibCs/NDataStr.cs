using System;
using UnityEngine;

namespace NLibCs
{
	public struct NDataStr
	{
		public static NDataStr Empty;

		public string str;

		public string ToLower()
		{
			return this.str.ToLower();
		}

		private static string[] __SplitCoord(string data)
		{
			int num = 0;
			int num2 = data.Length;
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] == '(')
				{
					num = i + 1;
				}
				else if (data[i] == ')')
				{
					num2 = i;
					break;
				}
			}
			data = data.Substring(num, num2 - num);
			return data.Split(new char[]
			{
				','
			});
		}

		public static implicit operator string(NDataStr dstr)
		{
			return dstr.str;
		}

		public static implicit operator int(NDataStr dstr)
		{
			int num;
			if (!int.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator uint(NDataStr dstr)
		{
			uint num;
			if (!uint.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator short(NDataStr dstr)
		{
			short num;
			if (!short.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator ushort(NDataStr dstr)
		{
			ushort num;
			if (!ushort.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator long(NDataStr dstr)
		{
			long num;
			if (!long.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator ulong(NDataStr dstr)
		{
			ulong num;
			if (!ulong.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator decimal(NDataStr dstr)
		{
			decimal num;
			if (!decimal.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator bool(NDataStr dstr)
		{
			bool flag;
			if (!bool.TryParse(dstr.str, out flag))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, flag.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return flag;
		}

		public static implicit operator byte(NDataStr dstr)
		{
			byte b;
			if (!byte.TryParse(dstr.str, out b))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, b.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return b;
		}

		public static implicit operator float(NDataStr dstr)
		{
			float num;
			if (!float.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator double(NDataStr dstr)
		{
			double num;
			if (!double.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator DateTime(NDataStr dstr)
		{
			DateTime dateTime;
			if (!DateTime.TryParse(dstr.str, out dateTime))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, dateTime.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return dateTime;
		}

		public static implicit operator Vector2(NDataStr dstr)
		{
			Vector2 vector = default(Vector2);
			string[] array = NDataStr.__SplitCoord(dstr.str);
			if (array.Length == 2)
			{
				float new_x = 0f;
				float new_y = 0f;
				if (float.TryParse(array[0], out new_x) && float.TryParse(array[1], out new_y))
				{
					vector.Set(new_x, new_y);
					return vector;
				}
			}
			string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, vector.GetType().ToString());
			throw new InvalidCastException(message);
		}

		public static implicit operator Vector3(NDataStr dstr)
		{
			Vector3 result = default(Vector3);
			string[] array = NDataStr.__SplitCoord(dstr.str);
			if (array.Length == 3)
			{
				float new_x = 0f;
				float new_y = 0f;
				float new_z = 0f;
				if (float.TryParse(array[0], out new_x) && float.TryParse(array[1], out new_y) && float.TryParse(array[2], out new_z))
				{
					result.Set(new_x, new_y, new_z);
					return result;
				}
			}
			string message = string.Format("InvalidCastException: NDataStr({0}) to Vector3", dstr.str);
			throw new InvalidCastException(message);
		}

		public static implicit operator Vector4(NDataStr dstr)
		{
			Vector4 vector = default(Vector4);
			string[] array = NDataStr.__SplitCoord(dstr.str);
			if (array.Length == 4)
			{
				float new_x = 0f;
				float new_y = 0f;
				float new_z = 0f;
				float new_w = 0f;
				if (float.TryParse(array[0], out new_x) && float.TryParse(array[1], out new_y) && float.TryParse(array[2], out new_z) && float.TryParse(array[3], out new_w))
				{
					vector.Set(new_x, new_y, new_z, new_w);
					return vector;
				}
			}
			string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, vector.GetType().ToString());
			throw new InvalidCastException(message);
		}
	}
}
