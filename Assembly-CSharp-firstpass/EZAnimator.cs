using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EZAnimator : MonoBehaviour
{
	private static EZAnimator s_Instance = null;

	protected static Dictionary<EZAnimation.ANIM_TYPE, EZLinkedList<EZAnimation>> freeAnimPool = new Dictionary<EZAnimation.ANIM_TYPE, EZLinkedList<EZAnimation>>();

	protected static EZLinkedList<EZAnimation> animations = new EZLinkedList<EZAnimation>();

	protected static bool pumpIsRunning = false;

	protected static bool pumpIsDone = true;

	protected static float startTime;

	protected static float time;

	protected static float elapsed;

	protected static EZAnimation anim;

	private int i;

	public static EZAnimator instance
	{
		get
		{
			if (EZAnimator.s_Instance == null)
			{
				GameObject gameObject = new GameObject("EZAnimator");
				EZAnimator.s_Instance = (EZAnimator)gameObject.AddComponent(typeof(EZAnimator));
			}
			return EZAnimator.s_Instance;
		}
	}

	public static bool Exists()
	{
		return EZAnimator.s_Instance != null;
	}

	public int GetCount()
	{
		return EZAnimator.animations.Count;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void OnLevelWasLoaded(int level)
	{
	}

	[DebuggerHidden]
	protected static IEnumerator AnimPump()
	{
		return new EZAnimator.<AnimPump>c__Iterator6();
	}

	public void StartAnimationPump()
	{
		if (!EZAnimator.pumpIsRunning && base.gameObject.activeInHierarchy)
		{
			EZAnimator.pumpIsRunning = true;
			base.StartCoroutine(this.PumpStarter());
		}
	}

	[DebuggerHidden]
	protected IEnumerator PumpStarter()
	{
		EZAnimator.<PumpStarter>c__Iterator7 <PumpStarter>c__Iterator = new EZAnimator.<PumpStarter>c__Iterator7();
		<PumpStarter>c__Iterator.<>f__this = this;
		return <PumpStarter>c__Iterator;
	}

	public static void StopAnimationPump()
	{
	}

	protected EZAnimation CreateNewAnimation(EZAnimation.ANIM_TYPE type)
	{
		switch (type)
		{
		case EZAnimation.ANIM_TYPE.AnimClip:
			return new RunAnimClip();
		case EZAnimation.ANIM_TYPE.FadeSprite:
			return new FadeSprite();
		case EZAnimation.ANIM_TYPE.FadeMaterial:
			return new FadeMaterial();
		case EZAnimation.ANIM_TYPE.FadeText:
			return new FadeText();
		case EZAnimation.ANIM_TYPE.Translate:
			return new AnimatePosition();
		case EZAnimation.ANIM_TYPE.PunchPosition:
			return new PunchPosition();
		case EZAnimation.ANIM_TYPE.Crash:
			return new Crash();
		case EZAnimation.ANIM_TYPE.SmoothCrash:
			return new SmoothCrash();
		case EZAnimation.ANIM_TYPE.Shake:
			return new Shake();
		case EZAnimation.ANIM_TYPE.Scale:
			return new AnimateScale();
		case EZAnimation.ANIM_TYPE.PunchScale:
			return new PunchScale();
		case EZAnimation.ANIM_TYPE.Rotate:
			return new AnimateRotation();
		case EZAnimation.ANIM_TYPE.PunchRotation:
			return new PunchRotation();
		case EZAnimation.ANIM_TYPE.ShakeRotation:
			return new ShakeRotation();
		case EZAnimation.ANIM_TYPE.CrashRotation:
			return new CrashRotation();
		case EZAnimation.ANIM_TYPE.FadeAudio:
			return new FadeAudio();
		case EZAnimation.ANIM_TYPE.TuneAudio:
			return new TuneAudio();
		case EZAnimation.ANIM_TYPE.FrameAnimation:
			return new RunFrameAnimation();
		default:
			return null;
		}
	}

	public EZAnimation GetAnimation(EZAnimation.ANIM_TYPE type)
	{
		EZLinkedList<EZAnimation> eZLinkedList;
		if (EZAnimator.freeAnimPool.TryGetValue(type, out eZLinkedList) && !eZLinkedList.Empty)
		{
			EZAnimation head = eZLinkedList.Head;
			eZLinkedList.Remove(head);
			return head;
		}
		return this.CreateNewAnimation(type);
	}

	protected static void ReturnAnimToPool(EZAnimation anim)
	{
		anim.Clear();
		EZLinkedList<EZAnimation> eZLinkedList;
		if (!EZAnimator.freeAnimPool.TryGetValue(anim.type, out eZLinkedList))
		{
			eZLinkedList = new EZLinkedList<EZAnimation>();
			EZAnimator.freeAnimPool.Add(anim.type, eZLinkedList);
		}
		eZLinkedList.Add(anim);
	}

	public void AddAnimation(EZAnimation a)
	{
		if (!a.running)
		{
			EZAnimator.animations.Add(a);
			a.running = true;
		}
		this.StartAnimationPump();
	}

	public void AddTransition(EZTransition t)
	{
		if (t.animationTypes == null)
		{
			return;
		}
		for (int i = 0; i < t.animationTypes.Length; i++)
		{
			EZAnimation.ANIM_TYPE aNIM_TYPE = t.animationTypes[i];
			if (aNIM_TYPE == EZAnimation.ANIM_TYPE.FadeSprite || aNIM_TYPE == EZAnimation.ANIM_TYPE.FadeText || aNIM_TYPE == EZAnimation.ANIM_TYPE.FadeMaterial)
			{
				EZLinkedList<EZLinkedListNode<GameObject>> subSubjects = t.SubSubjects;
				if (subSubjects.Rewind())
				{
					do
					{
						EZAnimation animation = this.GetAnimation(aNIM_TYPE);
						t.animParams[i].transition = t;
						if (!animation.Start(subSubjects.Current.val, t.animParams[i]))
						{
							EZAnimator.ReturnAnimToPool(animation);
						}
						else if (animation.running)
						{
							EZLinkedListNode<EZAnimation> eZLinkedListNode = t.AddRunningAnim();
							eZLinkedListNode.val = animation;
							animation.Data = eZLinkedListNode;
						}
					}
					while (subSubjects.MoveNext());
				}
			}
			if (!(t.MainSubject == null))
			{
				EZAnimation animation = this.GetAnimation(aNIM_TYPE);
				t.animParams[i].transition = t;
				if (!animation.Start(t.MainSubject, t.animParams[i]))
				{
					EZAnimator.ReturnAnimToPool(animation);
				}
				else if (animation.running)
				{
					EZLinkedListNode<EZAnimation> eZLinkedListNode = t.AddRunningAnim();
					eZLinkedListNode.val = animation;
					animation.Data = eZLinkedListNode;
				}
			}
		}
	}

	public void StopAnimation(EZAnimation a)
	{
		this.StopAnimation(a, false);
	}

	public void StopAnimation(EZAnimation a, bool end)
	{
		if (!a.running)
		{
			return;
		}
		if (end)
		{
			a._end();
		}
		else
		{
			a._stop();
		}
		EZAnimator.animations.Remove(a);
		EZAnimator.ReturnAnimToPool(a);
		if (EZAnimator.animations.Empty)
		{
			EZAnimator.StopAnimationPump();
		}
	}

	public void Stop(object obj)
	{
		this.Stop(obj, false);
	}

	public void Stop(object obj, bool end)
	{
		EZLinkedListIterator<EZAnimation> eZLinkedListIterator = EZAnimator.animations.Begin();
		while (!eZLinkedListIterator.Done)
		{
			if (eZLinkedListIterator.Current.GetSubject() == obj)
			{
				EZAnimation current = eZLinkedListIterator.Current;
				if (current.running)
				{
					if (end)
					{
						current._end();
					}
					else
					{
						current._stop();
					}
					EZAnimator.animations.Remove(current);
					EZAnimator.ReturnAnimToPool(current);
					continue;
				}
			}
			eZLinkedListIterator.Next();
		}
		eZLinkedListIterator.End();
	}

	public void Stop(object obj, EZAnimation.ANIM_TYPE type, bool end)
	{
		EZLinkedListIterator<EZAnimation> eZLinkedListIterator = EZAnimator.animations.Begin();
		while (!eZLinkedListIterator.Done)
		{
			if (eZLinkedListIterator.Current.GetSubject() == obj && eZLinkedListIterator.Current.type == type)
			{
				EZAnimation current = eZLinkedListIterator.Current;
				if (current.running)
				{
					if (end)
					{
						current._end();
					}
					else
					{
						current._stop();
					}
					EZAnimator.animations.Remove(current);
					EZAnimator.ReturnAnimToPool(current);
					continue;
				}
			}
			eZLinkedListIterator.Next();
		}
		eZLinkedListIterator.End();
	}

	public void End(object obj)
	{
		this.Stop(obj, true);
	}

	public void EndAll()
	{
		EZLinkedListIterator<EZAnimation> eZLinkedListIterator = EZAnimator.animations.Begin();
		while (!eZLinkedListIterator.Done)
		{
			eZLinkedListIterator.Current.End();
		}
		eZLinkedListIterator.End();
	}

	public void StopAll()
	{
		EZLinkedListIterator<EZAnimation> eZLinkedListIterator = EZAnimator.animations.Begin();
		while (!eZLinkedListIterator.Done)
		{
			eZLinkedListIterator.Current.Stop();
		}
		eZLinkedListIterator.End();
	}

	public void PauseAll()
	{
		EZLinkedListIterator<EZAnimation> eZLinkedListIterator = EZAnimator.animations.Begin();
		while (!eZLinkedListIterator.Done)
		{
			eZLinkedListIterator.Current.Paused = true;
			eZLinkedListIterator.Next();
		}
		eZLinkedListIterator.End();
	}

	public void UnpauseAll()
	{
		EZLinkedListIterator<EZAnimation> eZLinkedListIterator = EZAnimator.animations.Begin();
		while (!eZLinkedListIterator.Done)
		{
			eZLinkedListIterator.Current.Paused = false;
			eZLinkedListIterator.Next();
		}
		eZLinkedListIterator.End();
	}

	public static int GetNumAnimations()
	{
		return EZAnimator.animations.Count;
	}
}
