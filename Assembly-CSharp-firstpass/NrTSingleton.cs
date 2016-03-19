using System;
using System.Reflection;

public abstract class NrTSingleton<T> where T : class
{
	internal static class SingletonAllocator
	{
		internal static T ms_kInstance;

		static SingletonAllocator()
		{
			NrTSingleton<T>.SingletonAllocator.CreateInstance(typeof(T));
			NrTSingleton<T>.SingletonAllocator.Initialize(typeof(T));
		}

		public static T CreateInstance(Type type)
		{
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[0], new ParameterModifier[0]);
			ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			if (constructors.Length > 0)
			{
				throw new Exception(type.FullName + " has one more public constructors so the property cannot be enforced.");
			}
			if (constructor == null)
			{
				throw new Exception(type.FullName + " doesn't have a private/protected constructor so the property cannnot be enforced.");
			}
			T result;
			try
			{
				result = (NrTSingleton<T>.SingletonAllocator.ms_kInstance = (T)((object)constructor.Invoke(new object[0])));
			}
			catch (Exception innerException)
			{
				throw new Exception("The singleton couldnt be constructed, check if " + type.FullName + " has a default constructor", innerException);
			}
			return result;
		}

		public static bool Initialize(Type type)
		{
			MethodInfo method = type.GetMethod("Initialize");
			return method == null || (bool)method.Invoke(NrTSingleton<T>.SingletonAllocator.ms_kInstance, null);
		}
	}

	public static T Instance
	{
		get
		{
			return NrTSingleton<T>.SingletonAllocator.ms_kInstance;
		}
	}
}
