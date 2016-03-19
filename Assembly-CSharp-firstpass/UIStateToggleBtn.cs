using GameMessage;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Toggle Button")]
public class UIStateToggleBtn : AutoSpriteControlBase
{
	protected int curStateIndex;

	protected bool stateChangeWhileDeactivated;

	public int defaultState;

	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
	{
		new TextureAnim("Unnamed"),
		new TextureAnim("Disabled")
	};

	[HideInInspector]
	public string[] stateLabels = new string[]
	{
		"[\"]",
		"[\"]",
		"[\"]",
		"[\"]"
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

	public SpriteRoot[] layers = new SpriteRoot[0];

	public MonoBehaviour scriptWithMethodToInvoke;

	public string methodToInvoke = string.Empty;

	public POINTER_INFO.INPUT_EVENT whenToInvoke = POINTER_INFO.INPUT_EVENT.TAP;

	public float delay;

	public AudioSource soundToPlay;

	public bool disableHoverEffect;

	protected int[,] stateIndices;

	protected int overLayerState;

	protected int activeLayerState;

	protected int layerState;

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
			return this.states[this.curStateIndex].name;
		}
	}

	public override TextureAnim[] States
	{
		get
		{
			return this.states;
		}
		set
		{
			this.states = value;
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

	public override CSpriteFrame DefaultFrame
	{
		get
		{
			if (this.States[this.defaultState].spriteFrames.Length != 0)
			{
				return this.States[this.defaultState].spriteFrames[0];
			}
			return null;
		}
	}

	public override TextureAnim DefaultState
	{
		get
		{
			return this.States[this.defaultState];
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
			bool flag = this.spriteText == null;
			base.Text = value;
			if (flag && this.spriteText != null && Application.isPlaying)
			{
				for (int i = 0; i < this.transitions.Length; i++)
				{
					for (int j = 0; j < this.transitions[i].list.Length; j++)
					{
						this.transitions[i].list[j].AddSubSubject(this.spriteText.gameObject);
					}
				}
			}
		}
	}

	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
			base.gameObject.SetActive(value);
		}
	}

	public override string GetStateLabel(int index)
	{
		return this.stateLabels[index];
	}

	public override void SetStateLabel(int index, string label)
	{
		this.stateLabels[index] = label;
		if (index == this.curStateIndex)
		{
			base.UseStateLabel(index);
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

	public override void OnInput(ref POINTER_INFO ptr)
	{
		if (this.deleted)
		{
			return;
		}
		if (!this.m_controlIsEnabled || base.IsHidden())
		{
			base.OnInput(ref ptr);
			return;
		}
		if (this.inputDelegate != null)
		{
			this.inputDelegate(ref ptr);
		}
		if (!this.m_controlIsEnabled || base.IsHidden())
		{
			base.OnInput(ref ptr);
			return;
		}
		if (ptr.evt == this.whenToInvoke)
		{
			this.ToggleState();
			if (this.soundToPlay != null)
			{
				this.soundToPlay.PlayOneShot(this.soundToPlay.clip);
			}
			MsgHandler.Handle("CheckBoxSound", new object[0]);
			if (this.scriptWithMethodToInvoke != null)
			{
				this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, this.delay);
			}
		}
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.PRESS:
		case POINTER_INFO.INPUT_EVENT.DRAG:
			if (!this.disableHoverEffect)
			{
				this.SetLayerState(this.activeLayerState);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && ptr.hitInfo.collider == base.collider && !this.disableHoverEffect)
			{
				this.SetLayerState(this.overLayerState);
			}
			else
			{
				this.SetLayerState(this.curStateIndex);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE:
			if (!this.disableHoverEffect)
			{
				this.SetLayerState(this.overLayerState);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			this.SetLayerState(this.curStateIndex);
			break;
		}
		base.OnInput(ref ptr);
	}

	protected override void Awake()
	{
		base.Awake();
		this.curStateIndex = this.defaultState;
	}

	public override void Start()
	{
		if (this.m_started)
		{
			return;
		}
		base.Start();
		this.aggregateLayers = new SpriteRoot[1][];
		this.aggregateLayers[0] = this.layers;
		if (Application.isPlaying)
		{
			this.stateIndices = new int[this.layers.Length, this.states.Length + 3];
			this.overLayerState = this.states.Length + 1;
			this.activeLayerState = this.states.Length + 2;
			for (int i = 0; i < this.states.Length; i++)
			{
				this.transitions[i].list[0].MainSubject = base.gameObject;
				for (int j = 0; j < this.layers.Length; j++)
				{
					if (this.layers[j] == null)
					{
						TsLog.LogError("A null layer sprite was encountered on control \"" + base.name + "\". Please fill in the layer reference, or remove the empty element.", new object[0]);
					}
					else
					{
						this.stateIndices[j, i] = this.layers[j].GetStateIndex(this.states[i].name);
						if (this.stateIndices[j, this.curStateIndex] != -1)
						{
							this.layers[j].SetState(this.stateIndices[j, this.curStateIndex]);
						}
						else
						{
							this.layers[j].Hide(true);
						}
					}
				}
			}
			for (int j = 0; j < this.layers.Length; j++)
			{
				this.stateIndices[j, this.overLayerState] = this.layers[j].GetStateIndex("Over");
				this.stateIndices[j, this.activeLayerState] = this.layers[j].GetStateIndex("Active");
			}
			if (base.collider == null)
			{
				this.AddCollider();
			}
			this.SetToggleState(this.curStateIndex, true);
		}
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIStateToggleBtn))
		{
			return;
		}
		UIStateToggleBtn uIStateToggleBtn = (UIStateToggleBtn)s;
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.defaultState = uIStateToggleBtn.defaultState;
		}
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.prevTransition = uIStateToggleBtn.prevTransition;
			if (Application.isPlaying)
			{
				this.SetToggleState(uIStateToggleBtn.StateNum);
			}
		}
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.scriptWithMethodToInvoke = uIStateToggleBtn.scriptWithMethodToInvoke;
			this.methodToInvoke = uIStateToggleBtn.methodToInvoke;
			this.whenToInvoke = uIStateToggleBtn.whenToInvoke;
			this.delay = uIStateToggleBtn.delay;
		}
		if ((flags & ControlCopyFlags.Sound) == ControlCopyFlags.Sound)
		{
			this.soundToPlay = uIStateToggleBtn.soundToPlay;
		}
	}

	public int ToggleState()
	{
		this.SetToggleState(this.curStateIndex + 1);
		return this.curStateIndex;
	}

	public virtual void SetToggleState(int s, bool suppressTransition)
	{
		this.curStateIndex = s % (this.states.Length - 1);
		if (!base.gameObject.activeInHierarchy)
		{
			this.stateChangeWhileDeactivated = true;
			if (this.changeDelegate != null)
			{
				this.changeDelegate(this);
			}
			return;
		}
		this.SetState(this.curStateIndex);
		base.UseStateLabel(this.curStateIndex);
		this.UpdateCollider();
		this.SetLayerState(this.curStateIndex);
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.stateIndices[i, this.curStateIndex] != -1)
			{
				this.layers[i].Hide(base.IsHidden());
				this.layers[i].SetState(this.stateIndices[i, this.curStateIndex]);
			}
			else
			{
				this.layers[i].Hide(true);
			}
		}
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		if (!suppressTransition)
		{
			this.transitions[this.curStateIndex].list[0].Start();
			this.prevTransition = this.transitions[this.curStateIndex].list[0];
		}
		if (this.changeDelegate != null && !this.stateChangeWhileDeactivated)
		{
			this.changeDelegate(this);
		}
	}

	public virtual void SetToggleState(int s)
	{
		this.SetToggleState(s, false);
	}

	public virtual void SetToggleState(string stateName, bool suppressTransition)
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			if (this.states[i].name == stateName)
			{
				this.SetToggleState(i, suppressTransition);
				return;
			}
		}
	}

	public virtual void SetToggleState(string stateName)
	{
		this.SetToggleState(stateName, false);
	}

	protected void SetLayerState(int s)
	{
		if (s == this.layerState)
		{
			return;
		}
		this.layerState = s;
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.stateIndices[i, this.layerState] != -1)
			{
				this.layers[i].Hide(false);
				this.layers[i].SetState(this.stateIndices[i, this.layerState]);
			}
			else
			{
				this.layers[i].Hide(true);
			}
		}
	}

	protected void DisableMe()
	{
		this.SetState(this.states.Length - 1);
		base.UseStateLabel(this.states.Length - 1);
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.stateIndices[i, this.states.Length - 1] != -1)
			{
				this.layers[i].SetState(this.stateIndices[i, this.states.Length - 1]);
			}
		}
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		this.transitions[this.states.Length - 1].list[0].Start();
		this.prevTransition = this.transitions[this.states.Length - 1].list[0];
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.stateChangeWhileDeactivated)
		{
			this.SetToggleState(this.curStateIndex);
			this.stateChangeWhileDeactivated = false;
		}
	}

	public override void InitUVs()
	{
		if (this.states != null && this.defaultState <= this.states.Length - 1 && this.states[this.defaultState].spriteFrames.Length != 0)
		{
			this.frameInfo.Copy(this.states[this.defaultState].spriteFrames[0]);
		}
		base.InitUVs();
	}

	public override int DrawPreStateSelectGUI(int selState, bool inspector)
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.MaxWidth(50f)
		});
		if (GUILayout.Button((!inspector) ? "Add State" : "+", (!inspector) ? "Button" : "ToolbarButton", new GUILayoutOption[0]))
		{
			List<TextureAnim> list = new List<TextureAnim>();
			list.AddRange(this.states);
			list.Insert(this.states.Length - 1, new TextureAnim("State " + (this.states.Length - 1)));
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
				List<TextureAnim> list4 = new List<TextureAnim>();
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
			this.states[selState].name = GUILayout.TextField(this.states[selState].name, new GUILayoutOption[0]);
		}
		else
		{
			GUILayout.TextField(this.states[selState].name, new GUILayoutOption[0]);
		}
		GUILayout.EndHorizontal();
		return 28;
	}

	public override void DrawPreTransitionUI(int selState, IGUIScriptSelector gui)
	{
		this.scriptWithMethodToInvoke = gui.DrawScriptSelection(this.scriptWithMethodToInvoke, ref this.methodToInvoke);
	}

	public static UIStateToggleBtn Create(string name, Vector3 pos)
	{
		return (UIStateToggleBtn)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIStateToggleBtn));
	}

	public static UIStateToggleBtn Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIStateToggleBtn)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIStateToggleBtn));
	}

	public void SetCheckState(int s)
	{
		this.curStateIndex = s % (this.states.Length - 1);
		if (!base.gameObject.activeInHierarchy)
		{
			this.stateChangeWhileDeactivated = true;
			if (this.changeDelegate != null)
			{
				this.changeDelegate(this);
			}
			return;
		}
		this.SetState(this.curStateIndex);
		base.UseStateLabel(this.curStateIndex);
		this.UpdateCollider();
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.stateIndices[i, this.curStateIndex] != -1)
			{
				this.layers[i].Hide(false);
				this.layers[i].SetState(this.stateIndices[i, this.curStateIndex]);
			}
			else
			{
				this.layers[i].Hide(true);
			}
		}
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		this.transitions[this.curStateIndex].list[0].Start();
		this.prevTransition = this.transitions[this.curStateIndex].list[0];
	}
}
