using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace WellFired.Shared
{
	public static class LinqExts
	{
		public static IEnumerable<T> Empty<T>()
		{
			return new T[0];
		}

		[DebuggerHidden]
		public static IEnumerable<T> FromItems<T>(params T[] items)
		{
			LinqExts.<FromItems>c__Iterator74<T> <FromItems>c__Iterator = new LinqExts.<FromItems>c__Iterator74<T>();
			<FromItems>c__Iterator.items = items;
			<FromItems>c__Iterator.<$>items = items;
			LinqExts.<FromItems>c__Iterator74<T> expr_15 = <FromItems>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static void Each<T>(this IEnumerable<T> source, Action<T> fn)
		{
			foreach (T current in source)
			{
				fn(current);
			}
		}

		public static void Each<T>(this IEnumerable<T> source, Action<T, int> fn)
		{
			int num = 0;
			foreach (T current in source)
			{
				fn(current, num);
				num++;
			}
		}
	}
}
