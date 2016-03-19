using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/3D Toggle Button")]
public class UIStateToggleBtn3D : ControlBase
{
	protected int curStateIndex;

	public int defaultState;

	[HideInInspector]
	public string[] states = new string[]
	{
		"Unnamed",
		"Disabled"
	};

	[HideInInspector]
	public EZTransitionList[] transitions = new EZTransitionList[]
	{
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Prev")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From State")
		})
	};

	private EZTransition prevTransition;

	[HideInInspector]
	public string[] stateLabels = new string[]
	{
		"[\"]",
		"[\"]",
		"[\"]",
		"[\"]"
	};

	public MonoBehaviour scriptWithMethodToInvoke;

	public string methodToInvoke = string.Empty;

	public POINTER_INFO.INPUT_EVENT whenToInvoke = POINTER_INFO.INPUT_EVENT.TAP;

	public float delay;

	public AudioSource soundToPlay;

	public override bool controlIsEnabled
	{
		get
		{
			return this.m_controlIsEnabled;
		}
		set
		{
			this.m_controlIsEnabled = value;
			if (!value)
			{
				this.DisableMe();
			}
			else
			{
				this.SetToggleState(this.curStateIndex);
			}
		}
	}

	public int StateNum
	{
		get
		{
			return this.curStateIndex;
		}
	}

	public string StateName
	{
		get
		{
			return this.states[this.curStateIndex];
		}
	}

	public override string[] States
	{
		get
		{
			return this.states;
		}
	}

	public override EZTransitionList[] Transitions
	{
		get
		{
			return this.transitions;
		}
		set
		{
			this.transitions = value;
		}
	}

	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	public override EZTransitionList GetTransitions(int index)
	{
		if (index >= this.transitions.Length)
		{
			return null;
		}
		return this.transitions[index];
	}

	public override string GetStateLabel(int index)
	{
		return this.stateLabels[index];
	}

	public override void SetStateLabel(int index, string label)
	{
		this.stateLabels[index] = label;
	}

	public override void OnInput(POINTER_INFO ptr)
	{
		if (this.deleted)
		{
			return;
		}
		if (!this.m_controlIsEnabled)
		{
			base.OnInput(ptr);
			return;
		}
		if (this.inputDelegate != null)
		{
			this.inputDelegate(ref ptr);
		}
		if (!this.m_controlIsEnabled)
		{
			base.OnInput(ptr);
			return;
		}
		if (ptr.evt == POINTER_INFO.INPUT_EVENT.TAP)
		{
			this.ToggleState();
			if (this.soundToPlay != null)
			{
				this.soundToPlay.PlayOneShot(this.soundToPlay.clip);
			}
		}
		if (ptr.evt == this.whenToInvoke && this.scriptWithMethodToInvoke != null)
		{
			this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, this.delay);
		}
		base.OnInput(ptr);
	}

	protected override void Awake()
	{
		base.Awake();
		this.curStateIndex = this.defaultState;
	}

	public override void Start()
	{
		if (Application.isPlaying && base.collider == null)
		{
			this.AddCollider();
		}
	}

	public override void Copy(IControl c)
	{
		this.Copy(c, ControlCopyFlags.All);
	}

	public override void Copy(IControl c, ControlCopyFlags flags)
	{
		base.Copy(c);
		if (!(c is UIStateToggleBtn3D))
		{
			return;
		}
		UIStateToggleBtn3D uIStateToggleBtn3D = (UIStateToggleBtn3D)c;
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.defaultState = uIStateToggleBtn3D.defaultState;
		}
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.prevTransition = uIStateToggleBtn3D.prevTransition;
			if (Application.isPlaying)
			{
				this.SetToggleState(uIStateToggleBtn3D.StateNum);
			}
		}
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.scriptWithMethodToInvoke = uIStateToggleBtn3D.scriptWithMethodToInvoke;
			this.methodToInvoke = uIStateToggleBtn3D.methodToInvoke;
			this.whenToInvoke = uIStateToggleBtn3D.whenToInvoke;
			this.delay = uIStateToggleBtn3D.delay;
		}
		if ((flags & ControlCopyFlags.Sound) == ControlCopyFlags.Sound)
		{
			this.soundToPlay = uIStateToggleBtn3D.soundToPlay;
		}
	}

	public int ToggleState()
	{
		this.SetToggleState(this.curStateIndex + 1);
		if (this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
		return this.curStateIndex;
	}

	public void SetToggleState(int s)
	{
		this.curStateIndex = s % (this.states.Length - 1);
		base.UseStateLabel(this.curStateIndex);
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		this.transitions[this.curStateIndex].list[0].Start();
		this.prevTransition = this.transitions[this.curStateIndex].list[0];
	}

	public void SetToggleState(string stateName)
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			if (this.states[i] == stateName)
			{
				this.SetToggleState(i);
				return;
			}
		}
	}

	protected void DisableMe()
	{
		base.UseStateLabel(this.states.Length - 1);
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		this.transitions[this.states.Length - 1].list[0].Start();
		this.prevTransition = this.transitions[this.states.Length - 1].list[0];
	}

	public override int DrawPreStateSelectGUI(int selState, bool inspector)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		});
		if (GUILayout.Button((!inspector) ? "Add State" : "+", (!inspector) ? "Button" : "ToolbarButton", new GUILayoutOption[0]))
		{
			List<string> list = new List<string>();
			list.AddRange(this.states);
			list.Insert(this.states.Length - 1, "State " + (this.states.Length - 1));
			this.states = list.ToArray();
			List<EZTransitionList> list2 = new List<EZTransitionList>();
			list2.AddRange(this.transitions);
			list2.Insert(this.transitions.Length - 1, new EZTransitionList(new EZTransition[]
			{
				new EZTransition("From Prev")
			}));
			this.transitions = list2.ToArray();
			List<string> list3 = new List<string>();
			list3.AddRange(this.stateLabels);
			list3.Insert(this.stateLabels.Length - 1, "[\"]");
			this.stateLabels = list3.ToArray();
		}
		if (this.states.Length > 2 && selState != this.states.Length - 1)
		{
			if (GUILayout.Button((!inspector) ? "Delete State" : "-", (!inspector) ? "Button" : "ToolbarButton", new GUILayoutOption[0]))
			{
				List<string> list4 = new List<string>();
				list4.AddRange(this.states);
				list4.RemoveAt(selState);
				this.states = list4.ToArray();
				List<EZTransitionList> list5 = new List<EZTransitionList>();
				list5.AddRange(this.transitions);
				list5.RemoveAt(selState);
				this.transitions = list5.ToArray();
				List<string> list6 = new List<string>();
				list6.AddRange(this.stateLabels);
				list6.RemoveAt(selState);
				this.stateLabels = list6.ToArray();
			}
			this.defaultState %= this.states.Length;
		}
		if (inspector)
		{
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
		return 14;
	}

	public override int DrawPostStateSelectGUI(int selState)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		});
		GUILayout.Space(20f);
		GUILayout.Label("State Name:", new GUILayoutOption[0]);
		if (selState < this.states.Length - 1)
		{
			this.states[selState] = GUILayout.TextField(this.states[selState], new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.TextField(this.states[selState], new GUILayoutOption[0]);
		}
		GUILayout.EndHorizontal();
		return 28;
	}

	public override void DrawPreTransitionUI(int selState, IGUIScriptSelector gui)
	{
		this.scriptWithMethodToInvoke = gui.DrawScriptSelection(this.scriptWithMethodToInvoke, ref this.methodToInvoke);
	}

	public static UIStateToggleBtn3D Create(string name, Vector3 pos)
	{
		return (UIStateToggleBtn3D)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIStateToggleBtn3D));
	}

	public static UIStateToggleBtn3D Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIStateToggleBtn3D)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIStateToggleBtn3D));
	}
}
