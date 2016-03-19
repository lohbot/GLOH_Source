using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EZTransition
{
	public delegate void OnTransitionEndDelegate(EZTransition transition);

	public string name;

	public EZAnimation.ANIM_TYPE[] animationTypes = new EZAnimation.ANIM_TYPE[0];

	public AnimParams[] animParams = new AnimParams[0];

	[NonSerialized]
	protected EZLinkedList<EZLinkedListNode<EZAnimation>> runningAnims = new EZLinkedList<EZLinkedListNode<EZAnimation>>();

	protected EZLinkedList<EZLinkedListNode<EZAnimation>> idleAnims = new EZLinkedList<EZLinkedListNode<EZAnimation>>();

	[NonSerialized]
	protected GameObject mainSubject;

	[NonSerialized]
	protected EZLinkedList<EZLinkedListNode<GameObject>> subSubjects = new EZLinkedList<EZLinkedListNode<GameObject>>();

	[NonSerialized]
	protected EZTransition.OnTransitionEndDelegate onEndDelegates;

	public bool initialized;

	protected bool forcedStop;

	public EZLinkedList<EZLinkedListNode<GameObject>> SubSubjects
	{
		get
		{
			return this.subSubjects;
		}
	}

	public GameObject MainSubject
	{
		get
		{
			return this.mainSubject;
		}
		set
		{
			this.mainSubject = value;
		}
	}

	public EZTransition(string n)
	{
		this.name = n;
		this.runningAnims = null;
	}

	public void AddTransitionEndDelegate(EZTransition.OnTransitionEndDelegate del)
	{
		this.onEndDelegates = (EZTransition.OnTransitionEndDelegate)Delegate.Combine(this.onEndDelegates, del);
	}

	public void RemoveTransitionEndDelegate(EZTransition.OnTransitionEndDelegate del)
	{
		this.onEndDelegates = (EZTransition.OnTransitionEndDelegate)Delegate.Remove(this.onEndDelegates, del);
	}

	public void Copy(EZTransition src)
	{
		this.initialized = false;
		if (src.animationTypes == null)
		{
			return;
		}
		this.animationTypes = new EZAnimation.ANIM_TYPE[src.animationTypes.Length];
		src.animationTypes.CopyTo(this.animationTypes, 0);
		this.animParams = new AnimParams[src.animParams.Length];
		for (int i = 0; i < this.animParams.Length; i++)
		{
			this.animParams[i] = new AnimParams(this);
			this.animParams[i].Copy(src.animParams[i]);
		}
	}

	public void AddSubSubject(GameObject go)
	{
		if (this.subSubjects == null)
		{
			this.subSubjects = new EZLinkedList<EZLinkedListNode<GameObject>>();
		}
		this.subSubjects.Add(new EZLinkedListNode<GameObject>(go));
	}

	public void RemoveSubSubject(GameObject go)
	{
		if (this.subSubjects == null)
		{
			return;
		}
		EZLinkedListNode<GameObject> current = this.subSubjects.Current;
		if (this.subSubjects.Rewind())
		{
			while (!(this.subSubjects.Current.val == go))
			{
				if (!this.subSubjects.MoveNext())
				{
					goto IL_6E;
				}
			}
			this.subSubjects.Remove(this.subSubjects.Current);
		}
		IL_6E:
		this.subSubjects.Current = current;
	}

	public void OnAnimEnd(EZAnimation anim)
	{
		EZLinkedListNode<EZAnimation> eZLinkedListNode = (EZLinkedListNode<EZAnimation>)anim.Data;
		if (eZLinkedListNode == null)
		{
			return;
		}
		if (this.runningAnims == null)
		{
			return;
		}
		eZLinkedListNode.val = null;
		this.runningAnims.Remove(eZLinkedListNode);
		this.idleAnims.Add(eZLinkedListNode);
		if (this.onEndDelegates == null || this.forcedStop)
		{
			return;
		}
		EZLinkedListNode<EZAnimation> current = this.runningAnims.Current;
		if (this.runningAnims.Rewind())
		{
			while (this.runningAnims.Current.val.Duration <= 0f)
			{
				if (!this.runningAnims.MoveNext())
				{
					goto IL_A1;
				}
			}
			return;
		}
		IL_A1:
		this.runningAnims.Current = current;
		this.CallEndDelegates();
	}

	public EZLinkedListNode<EZAnimation> AddRunningAnim()
	{
		if (this.runningAnims == null)
		{
			this.runningAnims = new EZLinkedList<EZLinkedListNode<EZAnimation>>();
			if (this.idleAnims == null)
			{
				this.idleAnims = new EZLinkedList<EZLinkedListNode<EZAnimation>>();
			}
		}
		EZLinkedListNode<EZAnimation> eZLinkedListNode;
		if (this.idleAnims.Count > 0)
		{
			eZLinkedListNode = this.idleAnims.Head;
			this.idleAnims.Remove(eZLinkedListNode);
		}
		else
		{
			eZLinkedListNode = new EZLinkedListNode<EZAnimation>(null);
		}
		this.runningAnims.Add(eZLinkedListNode);
		return eZLinkedListNode;
	}

	public void Start()
	{
		if (this.mainSubject == null)
		{
			if (this.subSubjects == null)
			{
				return;
			}
			if (this.subSubjects.Count < 1)
			{
				return;
			}
		}
		this.StopSafe();
		EZAnimator.instance.AddTransition(this);
		if (this.runningAnims != null)
		{
			if (this.runningAnims.Count >= 1)
			{
				return;
			}
		}
		this.CallEndDelegates();
	}

	public void End()
	{
		if (this.runningAnims == null)
		{
			return;
		}
		if (this.runningAnims.Rewind())
		{
			this.forcedStop = true;
			do
			{
				EZLinkedListNode<EZAnimation> current = this.runningAnims.Current;
				EZAnimation val = current.val;
				if (val != null)
				{
					val.CompletedDelegate = null;
					EZAnimator.instance.StopAnimation(val, true);
				}
				this.runningAnims.Remove(current);
				this.idleAnims.Add(current);
				current.val = null;
			}
			while (this.runningAnims.MoveNext());
			this.forcedStop = false;
			this.CallEndDelegates();
		}
	}

	public void StopSafe()
	{
		if (this.runningAnims == null)
		{
			return;
		}
		EZLinkedListNode<EZAnimation> current = this.runningAnims.Current;
		if (this.runningAnims.Rewind())
		{
			this.forcedStop = true;
			do
			{
				EZLinkedListNode<EZAnimation> current2 = this.runningAnims.Current;
				EZAnimation val = current2.val;
				if (val != null)
				{
					val.CompletedDelegate = null;
					if (val.Mode == EZAnimation.ANIM_MODE.By)
					{
						EZAnimator.instance.StopAnimation(val, true);
					}
					else
					{
						EZAnimator.instance.StopAnimation(val, false);
					}
				}
				this.runningAnims.Remove(current2);
				this.idleAnims.Add(current2);
				current2.val = null;
			}
			while (this.runningAnims.MoveNext());
			this.forcedStop = false;
			this.CallEndDelegates();
		}
		this.runningAnims.Current = current;
	}

	public void Pause()
	{
		EZLinkedListIterator<EZLinkedListNode<EZAnimation>> eZLinkedListIterator = this.runningAnims.Begin();
		while (!eZLinkedListIterator.Done)
		{
			eZLinkedListIterator.Current.val.Paused = true;
			eZLinkedListIterator.Next();
		}
	}

	public void Unpause()
	{
		EZLinkedListIterator<EZLinkedListNode<EZAnimation>> eZLinkedListIterator = this.runningAnims.Begin();
		while (!eZLinkedListIterator.Done)
		{
			eZLinkedListIterator.Current.val.Paused = false;
			eZLinkedListIterator.Next();
		}
	}

	public bool IsRunning()
	{
		if (this.runningAnims == null)
		{
			return false;
		}
		EZLinkedListIterator<EZLinkedListNode<EZAnimation>> eZLinkedListIterator = this.runningAnims.Begin();
		while (!eZLinkedListIterator.Done)
		{
			if (eZLinkedListIterator.Current.val.Duration > 0f)
			{
				return true;
			}
			eZLinkedListIterator.Next();
		}
		return false;
	}

	public bool IsRunningAtAll()
	{
		return this.runningAnims != null && this.runningAnims.Count > 0;
	}

	protected void CallEndDelegates()
	{
		if (this.forcedStop)
		{
			return;
		}
		if (this.onEndDelegates != null)
		{
			this.onEndDelegates(this);
		}
	}

	public int Add()
	{
		this.initialized = true;
		List<EZAnimation.ANIM_TYPE> list = new List<EZAnimation.ANIM_TYPE>();
		if (this.animationTypes.Length > 0)
		{
			list.AddRange(this.animationTypes);
		}
		list.Add(EZAnimation.ANIM_TYPE.Translate);
		this.animationTypes = list.ToArray();
		List<AnimParams> list2 = new List<AnimParams>();
		if (this.animParams.Length > 0)
		{
			list2.AddRange(this.animParams);
		}
		list2.Add(new AnimParams(this));
		this.animParams = list2.ToArray();
		return this.animationTypes.Length - 1;
	}

	public AnimParams AddElement(EZAnimation.ANIM_TYPE type)
	{
		int num = this.Add();
		this.animationTypes[num] = type;
		return this.animParams[num];
	}

	public void Remove(int index)
	{
		this.initialized = true;
		List<EZAnimation.ANIM_TYPE> list = new List<EZAnimation.ANIM_TYPE>();
		if (this.animationTypes.Length > 0)
		{
			list.AddRange(this.animationTypes);
		}
		list.RemoveAt(index);
		this.animationTypes = list.ToArray();
		List<AnimParams> list2 = new List<AnimParams>();
		if (this.animParams.Length > 0)
		{
			list2.AddRange(this.animParams);
		}
		list2.RemoveAt(index);
		this.animParams = list2.ToArray();
	}

	public void SetElementType(int index, EZAnimation.ANIM_TYPE type)
	{
		if (index >= this.animationTypes.Length)
		{
			return;
		}
		if (this.animationTypes[index] != type)
		{
			this.initialized = true;
		}
		this.animationTypes[index] = type;
	}

	public string[] GetNames()
	{
		string[] array = new string[this.animationTypes.Length];
		for (int i = 0; i < this.animationTypes.Length; i++)
		{
			array[i] = i.ToString() + " - " + Enum.GetName(typeof(EZAnimation.ANIM_TYPE), this.animationTypes[i]);
			if (this.animParams[i].transition != this)
			{
				this.animParams[i].transition = this;
			}
		}
		return array;
	}
}
