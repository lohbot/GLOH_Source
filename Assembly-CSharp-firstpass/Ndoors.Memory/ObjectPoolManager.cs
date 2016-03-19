using System;
using System.Collections.Generic;
using System.Text;

namespace Ndoors.Memory
{
	public static class ObjectPoolManager
	{
		private static SortedDictionary<ObjectPoolKey, IObjectPoolContainer> ms_ObjPools = new SortedDictionary<ObjectPoolKey, IObjectPoolContainer>(new ObjectPoolKey.Comparer());

		public static T Acquire<T>() where T : class, IPoolObject
		{
			ObjectPoolKey data = new ObjectPoolKey(typeof(T));
			return ObjectPoolManager.Acquire<T>(data, null);
		}

		public static T Acquire<T>(object param) where T : class, IPoolObject
		{
			ObjectPoolKey data = new ObjectPoolKey(typeof(T));
			return ObjectPoolManager.Acquire<T>(data, param);
		}

		public static T Acquire<T>(int size) where T : class, IPoolObject
		{
			ObjectPoolKey data = new ObjectPoolKey(typeof(T), size);
			return ObjectPoolManager.Acquire<T>(data, null);
		}

		public static T Acquire<T>(int size, object param) where T : class, IPoolObject
		{
			ObjectPoolKey data = new ObjectPoolKey(typeof(T), size);
			return ObjectPoolManager.Acquire<T>(data, param);
		}

		private static T Acquire<T>(ObjectPoolKey data, object param) where T : class, IPoolObject
		{
			Type typeFromHandle = typeof(T);
			IObjectPoolContainer objectPoolContainer = ObjectPoolManager.I_GetObjectPoolContainer(data);
			TsLog.Assert(objectPoolContainer != null, "ObjectPoolConatiner is not found for {0} class.", new object[]
			{
				typeFromHandle.Name
			});
			T t = (T)((object)null);
			if (objectPoolContainer == null)
			{
				TsLog.Assert(false, "ObjectPoolManager. Class {0} poolContainer is null", new object[]
				{
					typeFromHandle.Name
				});
			}
			else if (objectPoolContainer.objectPoolAttr == null)
			{
				TsLog.Assert(false, "ObjectPoolManager. ObjectPoolAttribute for class {0} is null", new object[]
				{
					typeFromHandle.Name
				});
			}
			else if (objectPoolContainer.objectPoolAttr.CreateObjectStaticPrivate == null)
			{
				TsLog.Assert(false, "{0} class ObjectPoolAttribute.CreateInstance is not found!", new object[]
				{
					typeFromHandle.Name
				});
			}
			else
			{
				t = (objectPoolContainer.Acquire() as T);
				if (t == null)
				{
					TsLog.LogError("ObjectPoolManager.Acquire<T>(). Fail creation {0}.", new object[]
					{
						typeFromHandle.Name
					});
				}
				else
				{
					t.OnCreate(param);
				}
			}
			return t;
		}

		public static void Release<T>(IPoolObject iobj) where T : class, IPoolObject
		{
			T t = iobj as T;
			if (t != null)
			{
				TsLog.LogWarning("Release T", new object[0]);
				ObjectPoolManager.Release<T>(t);
			}
		}

		public static void Release<T>(T tobj) where T : class, IPoolObject
		{
			ObjectPoolManager.Release<T>(tobj, new ObjectPoolKey(typeof(T)));
		}

		public static void Release<T>(T tobj, int size) where T : class, IPoolObject
		{
			ObjectPoolManager.Release<T>(tobj, new ObjectPoolKey(typeof(T), size));
		}

		private static void Release<T>(T tobj, ObjectPoolKey data) where T : class, IPoolObject
		{
			IObjectPoolContainer objectPoolContainer;
			if (ObjectPoolManager.ms_ObjPools.TryGetValue(data, out objectPoolContainer))
			{
				tobj.OnDelete();
				objectPoolContainer.Release(tobj);
			}
		}

		public static string Print()
		{
			StringBuilder stringBuilder = new StringBuilder(4096);
			stringBuilder.AppendFormat("Use Pool : {0}\n", NrTSingleton<NrGlobalReference>.Instance.usePool);
			foreach (KeyValuePair<ObjectPoolKey, IObjectPoolContainer> current in ObjectPoolManager.ms_ObjPools)
			{
				ObjectPoolKey key = current.Key;
				IObjectPoolContainer value = current.Value;
				stringBuilder.AppendFormat("{0}, {1}\n", key.ToString(), value.ToString());
			}
			return stringBuilder.ToString();
		}

		private static IObjectPoolContainer I_GetObjectPoolContainer(ObjectPoolKey _key)
		{
			IObjectPoolContainer objectPoolContainer;
			if (!ObjectPoolManager.ms_ObjPools.TryGetValue(_key, out objectPoolContainer))
			{
				bool flag = true;
				object[] customAttributes = _key.GetObjType().GetCustomAttributes(false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					ObjectPoolAttribute objectPoolAttribute = (ObjectPoolAttribute)customAttributes[i];
					ObjectPoolAttribute objectPoolAttribute2 = objectPoolAttribute;
					if (objectPoolAttribute2 != null)
					{
						objectPoolContainer = objectPoolAttribute2.CreatePoolContainerStaticPrivate(objectPoolAttribute2);
						ObjectPoolManager.ms_ObjPools.Add(_key, objectPoolContainer);
						flag = false;
						break;
					}
				}
				if (flag)
				{
					TsLog.LogError("ObjectPoolManager.I_GetObjectPoolContainer(...). Fail creation {0}", new object[]
					{
						_key.GetObjType()
					});
				}
			}
			return objectPoolContainer;
		}

		public static void Clear()
		{
			foreach (IObjectPoolContainer current in ObjectPoolManager.ms_ObjPools.Values)
			{
				current.Clear();
			}
			ObjectPoolManager.ms_ObjPools.Clear();
		}
	}
}
