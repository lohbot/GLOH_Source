using System;
using System.Reflection;

namespace NLibCs.Net.Serialization
{
	public class UnknownLengthException : Exception
	{
		public UnknownLengthException(FieldInfo info) : base(string.Format("{2}.{0} ({1}) => Unknown length, Please use the [ArrayLength] attribute.", info.Name, info.FieldType, info.ReflectedType))
		{
		}
	}
}
