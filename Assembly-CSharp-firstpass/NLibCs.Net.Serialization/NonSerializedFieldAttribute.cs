using System;

namespace NLibCs.Net.Serialization
{
	[AttributeUsage(AttributeTargets.Field)]
	public class NonSerializedFieldAttribute : Attribute
	{
	}
}
