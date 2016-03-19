using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/3D Button")]
public class UIButton3D : ControlBase
{
	public enum CONTROL_STATE
	{
		NORMAL,
		OVER,
		ACTIVE,
		DISABLED
	}

	protected UIButton3D.CONTROL_STATE m_ctrlState;

	protected string[] states = new string[]
	{
		"Normal",
		"Over",
		"Active",
		"Disabled"
	};

	[HideInInspector]
	public EZTransitionList[] transitions = new EZTransitionList[]
	{
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Over"),
			new EZTransition("From Active"),
			new EZTransition("From Disabled")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Normal"),
			new EZTransition("From Active")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Normal"),
			new EZTransition("From Over")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From Normal"),
			new EZTransition("From Over"),
			new EZTransition("From Active")
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

	public AudioSource soundOnOver;

	public AudioSource soundOnClick;

	public bool repeat;

	public UIButton3D.CONTROL_STATE controlState
	{
		get
		{
			return this.m_ctrlState;
		}
	}

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
				this.SetControlState(UIButton3D.CONTROL_STATE.DISABLED);
			}
			else
			{
				this.SetControlState(UIButton3D.CONTROL_STATE.NORMAL);
			}
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
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.PRESS:
		case POINTER_INFO.INPUT_EVENT.DRAG:
			this.SetControlState(UIButton3D.CONTROL_STATE.ACTIVE);
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && ptr.hitInfo.collider == base.collider)
			{
				this.SetControlState(UIButton3D.CONTROL_STATE.OVER);
			}
			else
			{
				this.SetControlState(UIButton3D.CONTROL_STATE.NORMAL);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE:
			if (this.m_ctrlState != UIButton3D.CONTROL_STATE.OVER)
			{
				this.SetControlState(UIButton3D.CONTROL_STATE.OVER);
				if (this.soundOnOver != null)
				{
					this.soundOnOver.PlayOneShot(this.soundOnOver.clip);
				}
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			this.SetControlState(UIButton3D.CONTROL_STATE.NORMAL);
			break;
		}
		base.OnInput(ptr);
		if (this.repeat)
		{
			if (this.m_ctrlState == UIButton3D.CONTROL_STATE.ACTIVE)
			{
				goto IL_167;
			}
		}
		else if (ptr.evt == this.whenToInvoke)
		{
			goto IL_167;
		}
		return;
		IL_167:
		if (ptr.evt == this.whenToInvoke && this.soundOnClick != null)
		{
			this.soundOnClick.PlayOneShot(this.soundOnClick.clip);
		}
		if (this.scriptWithMethodToInvoke != null)
		{
			this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, this.delay);
		}
		if (this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
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
		base.Copy(c, flags);
		if (!(c is UIButton3D))
		{
			return;
		}
		UIButton3D uIButton3D = (UIButton3D)c;
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.prevTransition = uIButton3D.prevTransition;
			if (Application.isPlaying)
			{
				this.SetControlState(uIButton3D.controlState);
			}
		}
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.scriptWithMethodToInvoke = uIButton3D.scriptWithMethodToInvoke;
			this.methodToInvoke = uIButton3D.methodToInvoke;
			this.whenToInvoke = uIButton3D.whenToInvoke;
			this.delay = uIButton3D.delay;
		}
		if ((flags & ControlCopyFlags.Sound) == ControlCopyFlags.Sound)
		{
			this.soundOnOver = uIButton3D.soundOnOver;
			this.soundOnClick = uIButton3D.soundOnClick;
		}
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.repeat = uIButton3D.repeat;
		}
	}

	protected void SetControlState(UIButton3D.CONTROL_STATE s)
	{
		if (this.m_ctrlState == s)
		{
			return;
		}
		int ctrlState = (int)this.m_ctrlState;
		this.m_ctrlState = s;
		base.UseStateLabel((int)s);
		if (this.prevTransition != null)
		{
			this.prevTransition.StopSafe();
		}
		this.StartTransition((int)s, ctrlState);
	}

	protected void StartTransition(int newState, int prevState)
	{
		int num = 0;
		switch (newState)
		{
		case 0:
			switch (prevState)
			{
			case 1:
				num = 0;
				break;
			case 2:
				num = 1;
				break;
			case 3:
				num = 2;
				break;
			}
			break;
		case 1:
			switch (prevState)
			{
			case 0:
				num = 0;
				break;
			case 2:
				num = 1;
				break;
			}
			break;
		case 2:
			if (prevState != 0)
			{
				if (prevState == 1)
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
			break;
		case 3:
			switch (prevState)
			{
			case 0:
				num = 0;
				break;
			case 1:
				num = 1;
				break;
			case 2:
				num = 2;
				break;
			}
			break;
		}
		this.prevTransition = this.transitions[newState].list[num];
		this.prevTransition.Start();
	}

	public override void DrawPreTransitionUI(int selState, IGUIScriptSelector gui)
	{
		this.scriptWithMethodToInvoke = gui.DrawScriptSelection(this.scriptWithMethodToInvoke, ref this.methodToInvoke);
	}

	public static UIButton3D Create(string name, Vector3 pos)
	{
		return (UIButton3D)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIButton3D));
	}

	public static UIButton3D Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIButton3D)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIButton3D));
	}
}
