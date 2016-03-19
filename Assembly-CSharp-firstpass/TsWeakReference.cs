using System;

public class TsWeakReference<T> : WeakReference where T : class
{
	public T CastedTarget
	{
		get
		{
			return (T)((object)this.Target);
		}
	}

	private TsWeakReference(T target) : base(target)
	{
	}

	private TsWeakReference(T target, bool trackResurrection) : base(target, trackResurrection)
	{
	}

	public void KeepAlive()
	{
		GC.KeepAlive(this.Target);
	}

	public static implicit operator TsWeakReference<T>(T target)
	{
		return (target != null) ? new TsWeakReference<T>(target) : null;
	}

	public static implicit operator T(TsWeakReference<T> source)
	{
		return (source != null) ? ((T)((object)source.Target)) : ((T)((object)null));
	}
}
