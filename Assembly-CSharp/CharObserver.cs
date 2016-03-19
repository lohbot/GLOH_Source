using Ndoors.Framework.Stage;
using System;
using System.Collections;
using System.Diagnostics;

public static class CharObserver
{
	private static bool m_Moving;

	private static Action<bool> m_ActionChangeMoving;

	public static event Action<bool> OnChangeMoving
	{
		add
		{
			CharObserver.m_ActionChangeMoving = (Action<bool>)Delegate.Combine(CharObserver.m_ActionChangeMoving, value);
		}
		remove
		{
			CharObserver.m_ActionChangeMoving = (Action<bool>)Delegate.Remove(CharObserver.m_ActionChangeMoving, value);
		}
	}

	static CharObserver()
	{
		CharObserver.m_Moving = false;
		CharObserver.m_ActionChangeMoving = delegate(bool T)
		{
		};
		StageSystem.AddCommonPararellTask(CharObserver.Action());
	}

	[DebuggerHidden]
	private static IEnumerator Action()
	{
		return new CharObserver.<Action>c__Iterator8();
	}
}
