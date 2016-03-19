using System;
using System.Reflection;

namespace Ndoors.Memory
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ObjectPoolAttribute : Attribute
	{
		public string typeName
		{
			get;
			private set;
		}

		public int initCapacity
		{
			get;
			set;
		}

		public int initCount
		{
			get;
			set;
		}

		public int limitCount
		{
			get;
			set;
		}

		public Func<object> CreateObjectStaticPrivate
		{
			get;
			private set;
		}

		public Func<ObjectPoolAttribute, IObjectPoolContainer> CreatePoolContainerStaticPrivate
		{
			get;
			private set;
		}

		public ObjectPoolAttribute(Type classType, int initCount, int initCapacity, int limitCount)
		{
			this.typeName = classType.Name;
			this.initCapacity = initCapacity;
			this.initCount = initCount;
			this.limitCount = limitCount;
			this.CheckPoolCounter_I();
			MethodInfo method = classType.GetMethod("CreateInstanceStaicPrivate", BindingFlags.Static | BindingFlags.NonPublic);
			if (method == null)
			{
				TsLog.Assert(method != null, "{0} ObjectPoolAttribute. Create callback not found", new object[]
				{
					this.typeName
				});
			}
			else
			{
				Delegate @delegate = Delegate.CreateDelegate(typeof(Func<object>), classType, "CreateInstanceStaicPrivate");
				this.CreateObjectStaticPrivate = (@delegate as Func<object>);
				TsLog.Assert(this.CreateObjectStaticPrivate != null, "{0} ObjectPoolAttribute. static private create method({1}) not found.", new object[]
				{
					this.typeName,
					"CreateInstanceStaicPrivate"
				});
				Type typeFromHandle = typeof(StackObjectPool);
				method = typeFromHandle.GetMethod("CreateInstanceStaicPrivate", BindingFlags.Static | BindingFlags.NonPublic);
				if (method == null)
				{
					TsLog.Assert(method != null, "{0} ObjectPoolAttribute. static private method not found in the class {1}", new object[]
					{
						typeFromHandle.Name,
						"CreateInstanceStaicPrivate"
					});
				}
				else
				{
					@delegate = Delegate.CreateDelegate(typeof(Func<ObjectPoolAttribute, IObjectPoolContainer>), typeFromHandle, "CreateInstanceStaicPrivate");
					this.CreatePoolContainerStaticPrivate = (@delegate as Func<ObjectPoolAttribute, IObjectPoolContainer>);
				}
			}
		}

		private void CheckPoolCounter_I()
		{
			if (this.initCapacity <= 0)
			{
				TsLog.LogWarning("{0} ObjectPoolAttribute.initCapacity is invalid value({1}). use default value 16", new object[]
				{
					this.typeName,
					this.initCapacity
				});
				this.initCapacity = 16;
			}
			else if (this.initCapacity > 1024)
			{
				TsLog.LogWarning("{0} ObjectPoolAttribute.initCapacity is too big ({1}).", new object[]
				{
					this.initCapacity
				});
			}
			if (this.initCount < 0)
			{
				TsLog.LogWarning("{0} ObjectPoolAttribute.initCapacity is invalid value({1}). use default value 16", new object[]
				{
					this.typeName,
					this.initCapacity
				});
				this.initCount = ((this.initCapacity <= 16) ? this.initCapacity : 16);
			}
			else if (this.initCount > this.initCapacity)
			{
				TsLog.LogWarning("{0} ObjectPoolAttribute.initCount is bigger than initCapacity", new object[]
				{
					this.typeName
				});
				this.initCapacity = this.initCount;
			}
			if (this.limitCount < this.initCapacity)
			{
				TsLog.LogWarning("{0} ObjectPoolAttribute.limitCount is smaller than initCapacity", new object[]
				{
					this.typeName
				});
				this.limitCount = this.initCapacity;
			}
			if (TsPlatform.IsMobile && this.initCount > 16)
			{
				this.initCount = 16;
			}
		}
	}
}
