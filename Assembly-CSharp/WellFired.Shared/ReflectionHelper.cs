using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace WellFired.Shared
{
	public class ReflectionHelper : IReflectionHelper
	{
		public bool IsAssignableFrom(Type first, Type second)
		{
			return first.IsAssignableFrom(second);
		}

		public bool IsEnum(Type type)
		{
			return type.IsEnum;
		}

		[DebuggerHidden]
		private IEnumerable GetBaseTypes(Type type)
		{
			ReflectionHelper.<GetBaseTypes>c__Iterator76 <GetBaseTypes>c__Iterator = new ReflectionHelper.<GetBaseTypes>c__Iterator76();
			<GetBaseTypes>c__Iterator.type = type;
			<GetBaseTypes>c__Iterator.<$>type = type;
			<GetBaseTypes>c__Iterator.<>f__this = this;
			ReflectionHelper.<GetBaseTypes>c__Iterator76 expr_1C = <GetBaseTypes>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		public PropertyInfo GetProperty(Type type, string name)
		{
			return type.GetProperty(name);
		}

		public MethodInfo GetMethod(Type type, string name)
		{
			return type.GetMethod(name);
		}

		public FieldInfo GetField(Type type, string name)
		{
			return type.GetField(name);
		}

		public bool IsValueType(Type type)
		{
			return type.IsValueType;
		}

		public MethodInfo GetNonPublicStaticMethod(Type type, string name)
		{
			return type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
		}

		public MethodInfo GetNonPublicMethod(Type type, string name)
		{
			return type.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public PropertyInfo GetNonPublicInstanceProperty(Type type, string name)
		{
			return type.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public FieldInfo GetNonPublicInstanceField(Type type, string name)
		{
			return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
		}
	}
}
