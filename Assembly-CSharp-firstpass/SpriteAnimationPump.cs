using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class SpriteAnimationPump : MonoBehaviour
{
	private static SpriteAnimationPump instance;

	protected static SpriteBase head;

	protected static SpriteBase cur;

	protected static bool pumpIsRunning;

	protected static bool pumpIsDone = true;

	public static float animationPumpInterval = 0.03333f;

	public bool IsRunning
	{
		get
		{
			return SpriteAnimationPump.pumpIsRunning;
		}
	}

	public static SpriteAnimationPump Instance
	{
		get
		{
			if (SpriteAnimationPump.instance == null)
			{
				GameObject gameObject = new GameObject("SpriteAnimationPump");
				SpriteAnimationPump.instance = (SpriteAnimationPump)gameObject.AddComponent(typeof(SpriteAnimationPump));
			}
			return SpriteAnimationPump.instance;
		}
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void StartAnimationPump()
	{
		if (!SpriteAnimationPump.pumpIsRunning)
		{
			SpriteAnimationPump.pumpIsRunning = true;
			base.StartCoroutine(this.PumpStarter());
		}
	}

	[DebuggerHidden]
	protected IEnumerator PumpStarter()
	{
		SpriteAnimationPump.<PumpStarter>c__Iterator8 <PumpStarter>c__Iterator = new SpriteAnimationPump.<PumpStarter>c__Iterator8();
		<PumpStarter>c__Iterator.<>f__this = this;
		return <PumpStarter>c__Iterator;
	}

	public static void StopAnimationPump()
	{
	}

	[DebuggerHidden]
	protected static IEnumerator AnimationPump()
	{
		return new SpriteAnimationPump.<AnimationPump>c__Iterator9();
	}

	public static void Add(SpriteBase s)
	{
		if (SpriteAnimationPump.head != null)
		{
			s.next = SpriteAnimationPump.head;
			SpriteAnimationPump.head.prev = s;
			SpriteAnimationPump.head = s;
		}
		else
		{
			SpriteAnimationPump.head = s;
			SpriteAnimationPump.Instance.StartAnimationPump();
		}
	}

	public static void Remove(SpriteBase s)
	{
		if (SpriteAnimationPump.head == s)
		{
			SpriteAnimationPump.head = (SpriteBase)s.next;
			if (SpriteAnimationPump.head == null)
			{
				SpriteAnimationPump.StopAnimationPump();
			}
		}
		else if (s.next != null)
		{
			s.prev.next = s.next;
			s.next.prev = s.prev;
		}
		else if (s.prev != null)
		{
			s.prev.next = null;
		}
		s.next = null;
		s.prev = null;
	}
}
