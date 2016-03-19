using System;
using System.Collections.Generic;

namespace Ndoors.Memory
{
	public class StackObjectPool : IObjectPoolContainer
	{
		private ObjectPoolAttribute m_opAttribute;

		private Stack<object> m_reuseStack;

		private int m_curAlocCount;

		private int m_maxAlocCount;

		private int m_AcquireCount;

		ObjectPoolAttribute IObjectPoolContainer.objectPoolAttr
		{
			get
			{
				return this.m_opAttribute;
			}
		}

		private StackObjectPool(ObjectPoolAttribute opa)
		{
			this.m_opAttribute = opa;
			this.m_reuseStack = new Stack<object>(this.m_opAttribute.initCapacity);
			this.GrowPool_I();
		}

		object IObjectPoolContainer.Acquire()
		{
			this.m_AcquireCount++;
			this.m_curAlocCount++;
			if (this.m_curAlocCount > this.m_maxAlocCount)
			{
				this.m_maxAlocCount = this.m_curAlocCount;
				if (this.m_maxAlocCount > this.m_opAttribute.limitCount)
				{
					TsLog.LogWarning("StackObjectPool<{0}> creation over limit count({1}).", new object[]
					{
						this.m_opAttribute.typeName,
						this.m_opAttribute.limitCount
					});
				}
			}
			return this.AcquireOrGrow_I();
		}

		void IObjectPoolContainer.Release(object obj)
		{
			this.m_curAlocCount--;
			this.m_reuseStack.Push(obj);
		}

		void IObjectPoolContainer.Clear()
		{
			while (this.m_reuseStack.Count > 0)
			{
				this.m_reuseStack.Pop();
			}
		}

		private static IObjectPoolContainer CreateInstanceStaicPrivate(ObjectPoolAttribute opa)
		{
			return new StackObjectPool(opa);
		}

		public override string ToString()
		{
			int num = (int)((float)(this.m_AcquireCount - this.m_maxAlocCount) / (float)this.m_AcquireCount * 100f);
			return string.Format("StackObjectPool<{0}>: Cur/Max({1}/{2}) Init({3}) Capacity({4}) Limit({5}) Acqure({6}) ReUse({7}%)", new object[]
			{
				this.m_opAttribute.typeName,
				this.m_curAlocCount,
				this.m_maxAlocCount,
				this.m_opAttribute.initCount,
				this.m_opAttribute.initCapacity,
				this.m_opAttribute.limitCount,
				this.m_AcquireCount,
				num
			});
		}

		private void GrowPool_I()
		{
			int initCount = this.m_opAttribute.initCount;
			while (initCount-- > 0)
			{
				this.m_reuseStack.Push(this.m_opAttribute.CreateObjectStaticPrivate());
			}
		}

		private object AcquireOrGrow_I()
		{
			if (this.m_reuseStack.Count <= 0)
			{
				this.GrowPool_I();
			}
			TsLog.Assert(this.m_reuseStack.Count > 0, "{0}<{1}>: Fail! Growing.", new object[]
			{
				base.GetType().Name,
				this.m_opAttribute.typeName
			});
			return this.m_reuseStack.Pop();
		}
	}
}
