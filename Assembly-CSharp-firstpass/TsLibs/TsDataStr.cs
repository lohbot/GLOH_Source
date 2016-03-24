using System;

namespace TsLibs
{
	public struct TsDataStr
	{
		public static TsDataStr Empty;

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

		public static implicit operator string(TsDataStr dstr)
		{
			return dstr.str;
		}

		public static implicit operator int(TsDataStr dstr)
		{
			int num;
			if (!int.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator uint(TsDataStr dstr)
		{
			uint num;
			if (!uint.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator short(TsDataStr dstr)
		{
			short num;
			if (!short.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator ushort(TsDataStr dstr)
		{
			ushort num;
			if (!ushort.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator long(TsDataStr dstr)
		{
			long num;
			if (!long.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator ulong(TsDataStr dstr)
		{
			ulong num;
			if (!ulong.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator decimal(TsDataStr dstr)
		{
			decimal num;
			if (!decimal.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator bool(TsDataStr dstr)
		{
			bool flag;
			if (!bool.TryParse(dstr.str, out flag))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, flag.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return flag;
		}

		public static implicit operator byte(TsDataStr dstr)
		{
			byte b;
			if (!byte.TryParse(dstr.str, out b))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, b.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return b;
		}

		public static implicit operator float(TsDataStr dstr)
		{
			float num;
			if (!float.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator double(TsDataStr dstr)
		{
			double num;
			if (!double.TryParse(dstr.str, out num))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, num.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return num;
		}

		public static implicit operator DateTime(TsDataStr dstr)
		{
			DateTime dateTime;
			if (!DateTime.TryParse(dstr.str, out dateTime))
			{
				string message = string.Format("InvalidCastException: NDataStr({0}) to {1}.", dstr.str, dateTime.GetType().ToString());
				throw new InvalidCastException(message);
			}
			return dateTime;
		}
	}
}
