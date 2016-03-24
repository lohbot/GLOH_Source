using System;

namespace NLibCs.Net.Serialization
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class SerializableAttribute : Attribute
	{
		private int objectSize;

		public int ObjectSize
		{
			get
			{
				return this.objectSize;
			}
		}

		public SerializableAttribute(Type type)
		{
			this.objectSize = RawSerializer.GetObjectSize(type);
		}
	}
}
