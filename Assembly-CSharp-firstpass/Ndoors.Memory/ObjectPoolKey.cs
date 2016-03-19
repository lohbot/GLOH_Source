using System;
using System.Collections.Generic;

namespace Ndoors.Memory
{
	public struct ObjectPoolKey
	{
		public class EqualityComparer : IEqualityComparer<ObjectPoolKey>
		{
			public bool Equals(ObjectPoolKey x, ObjectPoolKey y)
			{
				return x.m_Type == y.m_Type && x.m_iSize == y.m_iSize;
			}

			public int GetHashCode(ObjectPoolKey x)
			{
				return x.m_Type.GetHashCode() + x.m_iSize;
			}
		}

		public class Comparer : IComparer<ObjectPoolKey>
		{
			public int Compare(ObjectPoolKey x, ObjectPoolKey y)
			{
				if (x.m_iSize < y.m_iSize)
				{
					return -1;
				}
				if (x.m_iSize > y.m_iSize)
				{
					return 1;
				}
				return x.m_Type.Name.CompareTo(y.m_Type.Name);
			}
		}

		private const int DEFAULT_SIZE_KEY = 0;

		private Type m_Type;

		private int m_iSize;

		public ObjectPoolKey(Type type)
		{
			this.m_Type = type;
			this.m_iSize = 0;
		}

		public ObjectPoolKey(Type type, int size)
		{
			TsLog.Assert(size > 0, "ObjectPool size invalide val={0}", new object[]
			{
				size
			});
			this.m_Type = type;
			this.m_iSize = size;
		}

		public Type GetObjType()
		{
			return this.m_Type;
		}

		public override string ToString()
		{
			return string.Format("Type({0}) Size({1})", this.m_Type, this.m_iSize);
		}
	}
}
