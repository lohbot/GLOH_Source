using GameMessage;
using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Controls/Radio Button")]
public class UIRadioBtn : AutoSpriteControlBase, IRadioButton
{
	protected enum CONTROL_STATE
	{
		True,
		False,
		Disabled,
		Over,
		Active
	}

	private UIRadioBtn.CONTROL_STATE state;

	private UIRadioBtn.CONTROL_STATE layerState;

	protected bool btnValue;

	public bool useParentForGrouping = true;

	public int radioGroup;

	protected RadioBtnGroup group;

	public bool defaultValue;

	protected bool stateChangeWhileDeactivated;

	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
	{
		new TextureAnim("True"),
		new TextureAnim("False"),
		new TextureAnim("Disabled")
	};

	[HideInInspector]
	public string[] stateLabels = new string[]
	{
		"[\"]",
		"[\"]",
		"[\"]"
	};

	[HideInInspector]
	public EZTransitionList[] transitions = new EZTransitionList[]
	{
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From False"),
			new EZTransition("From Disabled")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From True"),
			new EZTransition("From Disabled")
		}),
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From True"),
			new EZTransition("From False")
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
				this.SetButtonState();
			}
		}
	}

	public virtual bool Value
	{
		get
		{
			return this.btnValue;
		}
		set
		{
			this.SetValue(value);
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
			int num = (!this.btnValue) ? 1 : 0;
			if (this.States[num].spriteFrames.Length != 0)
			{
				return this.States[num].spriteFrames[0];
			}
			return null;
		}
	}

	public override TextureAnim DefaultState
	{
		get
		{
			int num = (!this.btnValue) ? 1 : 0;
			return this.States[num];
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

	public override string GetStateLabel(int index)
	{
		return this.stateLabels[index];
	}

	public override void SetStateLabel(int index, string label)
	{
		this.stateLabels[index] = label;
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
		if (!this.m_controlIsEnabled || base.IsHidden())
		{
			base.OnInput(ref ptr);
			return;
		}
		if (ptr.evt == this.whenToInvoke)
		{
			this.Value = true;
			if (this.soundToPlay != null)
			{
				this.soundToPlay.PlayOneShot(this.soundToPlay.clip);
			}
			if (this.scriptWithMethodToInvoke != null)
			{
				this.scriptWithMethodToInvoke.Invoke(this.methodToInvoke, this.delay);
			}
			MsgHandler.Handle("ToolbarSound", new object[0]);
		}
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.PRESS:
		case POINTER_INFO.INPUT_EVENT.DRAG:
			if (!this.disableHoverEffect)
			{
				this.SetLayerState(UIRadioBtn.CONTROL_STATE.Active);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && ptr.hitInfo.collider == base.collider && !this.disableHoverEffect)
			{
				this.SetLayerState(UIRadioBtn.CONTROL_STATE.Over);
			}
			else
			{
				this.SetLayerState(this.state);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE:
			if (!this.disableHoverEffect)
			{
				this.SetLayerState(UIRadioBtn.CONTROL_STATE.Over);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			this.SetLayerState(this.state);
			break;
		}
		base.OnInput(ref ptr);
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIRadioBtn))
		{
			return;
		}
		UIRadioBtn uIRadioBtn = (UIRadioBtn)s;
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.state = uIRadioBtn.state;
			this.prevTransition = uIRadioBtn.prevTransition;
			if (Application.isPlaying)
			{
				this.Value = uIRadioBtn.Value;
			}
		}
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.group = uIRadioBtn.group;
			this.defaultValue = uIRadioBtn.defaultValue;
		}
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.scriptWithMethodToInvoke = uIRadioBtn.scriptWithMethodToInvoke;
			this.methodToInvoke = uIRadioBtn.methodToInvoke;
			this.whenToInvoke = uIRadioBtn.whenToInvoke;
			this.delay = uIRadioBtn.delay;
		}
		if ((flags & ControlCopyFlags.Sound) == ControlCopyFlags.Sound)
		{
			this.soundToPlay = uIRadioBtn.soundToPlay;
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.group == null)
		{
			return;
		}
		this.group.buttons.Remove(this);
		this.group = null;
	}

	public void SetGroup(GameObject parent)
	{
		this.SetGroup(parent.transform.GetHashCode());
	}

	public void SetGroup(Transform parent)
	{
		this.SetGroup(parent.GetHashCode());
	}

	public void SetGroup(int groupID)
	{
		if (this.group != null)
		{
			this.group.buttons.Remove(this);
			this.group = null;
		}
		this.radioGroup = groupID;
		this.group = RadioBtnGroup.GetGroup(groupID);
		this.group.buttons.Add(this);
		if (this.btnValue)
		{
			this.PopOtherButtonsInGroup();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.btnValue = this.defaultValue;
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
		this.state = ((!this.controlIsEnabled) ? UIRadioBtn.CONTROL_STATE.Disabled : ((!this.btnValue) ? UIRadioBtn.CONTROL_STATE.False : UIRadioBtn.CONTROL_STATE.True));
		if (Application.isPlaying)
		{
			this.stateIndices = new int[this.layers.Length, 5];
			int num = (!this.btnValue) ? 1 : 0;
			num = ((!this.m_controlIsEnabled) ? 2 : num);
			for (int i = 0; i < this.layers.Length; i++)
			{
				if (!(this.layers[i] == null))
				{
					this.stateIndices[i, 0] = 0;
					this.stateIndices[i, 1] = 0;
					this.stateIndices[i, 2] = this.layers[i].GetStateIndex("disabled");
					this.stateIndices[i, 3] = this.layers[i].GetStateIndex("over");
					this.stateIndices[i, 4] = this.layers[i].GetStateIndex("active");
					if (this.stateIndices[i, num] != -1)
					{
						this.layers[i].SetState(this.stateIndices[i, num]);
					}
					else
					{
						this.layers[i].Hide(true);
					}
				}
			}
			if (base.collider == null)
			{
				this.AddCollider();
			}
			this.SetValue(this.btnValue, true);
			if (this.useParentForGrouping && base.transform.parent != null)
			{
				this.SetGroup(base.transform.parent.GetHashCode());
			}
			else
			{
				this.SetGroup(this.radioGroup);
			}
		}
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
	}

	protected void PopOtherButtonsInGroup()
	{
		if (this.group == null)
		{
			return;
		}
		for (int i = 0; i < this.group.buttons.Count; i++)
		{
			if (this.group.buttons[i] != this)
			{
				this.group.buttons[i].Value = false;
			}
		}
	}

	protected virtual void SetValue(bool val)
	{
		this.SetValue(val, false);
	}

	protected virtual void SetValue(bool val, bool suppressTransition)
	{
		bool flag = this.btnValue;
		this.btnValue = val;
		if (this.btnValue)
		{
			this.PopOtherButtonsInGroup();
		}
		this.SetButtonState(suppressTransition);
		if (flag != this.btnValue && this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
	}

	protected virtual void SetButtonState()
	{
		this.SetButtonState(false);
	}

	protected virtual void SetButtonState(bool suppressTransition)
	{
		if (base.spriteMesh == null)
		{
			return;
		}
		if (!this.m_started)
		{
			return;
		}
		int num = (int)this.state;
		this.state = ((!this.controlIsEnabled) ? UIRadioBtn.CONTROL_STATE.Disabled : ((!this.btnValue) ? UIRadioBtn.CONTROL_STATE.False : UIRadioBtn.CONTROL_STATE.True));
		int newState = Mathf.Clamp((int)this.state, 0, 2);
		if (!base.gameObject.activeInHierarchy)
		{
			this.stateChangeWhileDeactivated = true;
			return;
		}
		this.SetState(newState);
		this.UpdateCollider();
		this.SetLayerState(this.state);
		if (!suppressTransition)
		{
			if (this.prevTransition != null && num != (int)this.state)
			{
				this.prevTransition.StopSafe();
			}
			this.StartTransition(newState, num);
		}
	}

	protected void SetLayerState(UIRadioBtn.CONTROL_STATE s)
	{
		if (s == this.layerState)
		{
			return;
		}
		this.layerState = s;
		int num = (int)this.layerState;
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.stateIndices[i, num] != -1)
			{
				this.layers[i].Hide(base.IsHidden());
				this.layers[i].SetState(this.stateIndices[i, num]);
			}
			else
			{
				this.layers[i].Hide(true);
			}
		}
	}

	protected void StartTransition(int newState, int prevState)
	{
		int num = 0;
		switch (newState)
		{
		case 0:
			switch (prevState)
			{
			case 0:
				this.prevTransition = null;
				return;
			case 1:
				num = 0;
				break;
			case 2:
				num = 1;
				break;
			}
			break;
		case 1:
			switch (prevState)
			{
			case 0:
				num = 0;
				break;
			case 1:
				this.prevTransition = null;
				return;
			case 2:
				num = 1;
				break;
			}
			break;
		case 2:
			switch (prevState)
			{
			case 0:
				num = 0;
				break;
			case 1:
				num = 1;
				break;
			case 2:
				this.prevTransition = null;
				return;
			}
			break;
		}
		this.transitions[newState].list[num].Start();
		this.prevTransition = this.transitions[newState].list[num];
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.stateChangeWhileDeactivated)
		{
			this.SetButtonState();
			this.stateChangeWhileDeactivated = false;
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
		this.StartTransition(2, (int)this.state);
		this.state = UIRadioBtn.CONTROL_STATE.Disabled;
	}

	public override void InitUVs()
	{
		int num;
		if (!this.m_controlIsEnabled)
		{
			num = this.states.Length - 1;
		}
		else
		{
			num = ((!this.defaultValue) ? 1 : 0);
		}
		if (this.states[num].spriteFrames.Length != 0)
		{
			this.frameInfo.Copy(this.states[num].spriteFrames[0]);
		}
		base.InitUVs();
	}

	public override void DrawPreTransitionUI(int selState, IGUIScriptSelector gui)
	{
		this.scriptWithMethodToInvoke = gui.DrawScriptSelection(this.scriptWithMethodToInvoke, ref this.methodToInvoke);
	}

	public static UIRadioBtn Create(string name, Vector3 pos)
	{
		return (UIRadioBtn)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIRadioBtn));
	}

	public static UIRadioBtn Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIRadioBtn)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIRadioBtn));
	}

	virtual string get_name()
	{
		return base.name;
	}

	virtual void set_name(string value)
	{
		base.name = value;
	}
}
