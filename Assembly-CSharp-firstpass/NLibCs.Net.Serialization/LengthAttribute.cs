using System;

namespace NLibCs.Net.Serialization
{
	public class LengthAttribute : Attribute
	{
		private int length;

		private string fieldName;

		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		public string LengthField
		{
			get
			{
				return this.fieldName;
			}
		}

		public LengthAttribute(int length)
		{
			this.length = length;
		}

		public LengthAttribute(string fieldName)
		{
			this.fieldName = fieldName;
		}

		public void ResizeArray(Type arrayType, ref Array val)
		{
			if (!arrayType.IsArray)
			{
				return;
			}
			Array array = val;
			if (array == null)
			{
				array = Array.CreateInstance(arrayType.GetElementType(), this.length);
				val = array;
			}
			else if (array.Length != this.length)
			{
				Array array2 = Array.CreateInstance(arrayType.GetElementType(), this.length);
				int num = Math.Min(array.Length, this.length);
				Array.Copy(array, 0, array2, 0, num);
				Array.Clear(array2, num, this.length - num);
				val = array2;
			}
		}

		public void ResizeArray<T>(ref T[] array) where T : new()
		{
			if (array == null)
			{
				array = new T[this.length];
			}
			else if (array.Length != this.length)
			{
				T[] array2 = new T[this.length];
				int num = Math.Min(array.Length, this.length);
				Array.Copy(array, 0, array2, 0, num);
				Array.Clear(array2, num, this.length - num);
				array = array2;
			}
		}
	}
}
