using GameMessage;
using System;
using UnityEngine;
using UnityForms;

[AddComponentMenu("EZ GUI/Controls/Button")]
public class UIButton : AutoSpriteControlBase
{
	public enum CONTROL_STATE
	{
		NORMAL,
		OVER,
		ACTIVE,
		DISABLED
	}

	protected UIButton.CONTROL_STATE m_ctrlState;

	private string oldColorText = string.Empty;

	private string oldText = string.Empty;

	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
	{
		new TextureAnim("Normal"),
		new TextureAnim("Over"),
		new TextureAnim("Active"),
		new TextureAnim("Disabled")
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

	public string NormalColorText = string.Empty;

	public string OverColorText = string.Empty;

	public string AvtiveColorText = string.Empty;

	public string BaseString = string.Empty;

	public SpriteRoot[] layers = new SpriteRoot[0];

	public MonoBehaviour scriptWithMethodToInvoke;

	public string methodToInvoke = string.Empty;

	public POINTER_INFO.INPUT_EVENT whenToInvoke = POINTER_INFO.INPUT_EVENT.TAP;

	public float delay;

	public AudioSource soundOnOver;

	public AudioSource soundOnClick;

	public bool repeat;

	public bool alwaysFinishActiveTransition;

	protected bool transitionQueued;

	protected EZTransition nextTransition;

	protected int[,] stateIndices;

	private bool isListButton;

	private int index;

	private float startTime;

	private bool bPlayAni;

	public UIButton.CONTROL_STATE controlState
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
			base.UpdateText = true;
			this.m_controlIsEnabled = value;
			if (!value)
			{
				this.SetControlState(UIButton.CONTROL_STATE.DISABLED);
				if (base.ColorText != "[#7F7F7FFF]")
				{
					this.oldColorText = base.ColorText;
				}
				this.oldText = this.Text;
				base.ColorText = "[#7F7F7FFF]";
				this.Text = this.oldText;
			}
			else
			{
				this.SetControlState(UIButton.CONTROL_STATE.NORMAL);
				if (string.Empty != this.oldText)
				{
					base.ColorText = this.oldColorText;
					this.Text = this.oldText;
				}
			}
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

	public bool IsListButton
	{
		get
		{
			return this.isListButton;
		}
		set
		{
			this.isListButton = value;
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
			if (!this.m_controlIsEnabled && string.Empty != this.oldText)
			{
				this.oldText = this.Text;
			}
		}
	}

	public override void SetClippingRect(Rect3D value)
	{
		if (this.ignoreClipping)
		{
			return;
		}
		base.SetClippingRect(value);
		this.PlayAnim((int)this.m_ctrlState);
	}

	public void SetEnabled(bool value)
	{
		this.controlIsEnabled = value;
	}

	public bool GetEnableD()
	{
		return this.m_controlIsEnabled;
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
		switch (ptr.evt)
		{
		case POINTER_INFO.INPUT_EVENT.PRESS:
		case POINTER_INFO.INPUT_EVENT.DRAG:
			if (!this.IsListButton)
			{
				this.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
			if (this.AvtiveColorText != string.Empty && this.BaseString != string.Empty && this.AvtiveColorText != base.ColorText)
			{
				base.ColorText = this.AvtiveColorText;
				this.Text = this.BaseString;
			}
			if (ptr.evt == POINTER_INFO.INPUT_EVENT.PRESS)
			{
				if (this.mouseDownDelegate != null)
				{
					this.mouseDownDelegate(this);
				}
				if (this.bEffectAni && !this.bStartAni)
				{
					Vector3 one = Vector3.one;
					if (0f > base.transform.localScale.x)
					{
						one.x *= -1f;
					}
					if (0f > base.transform.localScale.y)
					{
						one.y *= -1f;
					}
					if (0f > base.transform.localScale.z)
					{
						one.z *= -1f;
					}
					AnimateScale.Do(base.gameObject, EZAnimation.ANIM_MODE.FromTo, this.width, this.height, base.transform.localScale, new Vector3(0.85f * one.x, 0.85f * one.y, 0.85f * one.z), EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.BackOut), 0.2f, 0f, new EZAnimation.CompletionDelegate(base.EffectAniStartDelegate), new EZAnimation.CompletionDelegate(base.EffectAniCompletionDelegate));
				}
			}
			break;
		case POINTER_INFO.INPUT_EVENT.DOUBLE_PRESS:
			if (!this.IsListButton)
			{
				this.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
			if (this.doubleClickDelegate != null)
			{
				this.doubleClickDelegate(this);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.RIGHT_PRESS:
			if (!this.IsListButton)
			{
				this.SetControlState(UIButton.CONTROL_STATE.ACTIVE);
			}
			if (this.rightMouseDelegate != null)
			{
				this.rightMouseDelegate(this);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.TAP:
		case POINTER_INFO.INPUT_EVENT.LONG_TAP:
			if (!this.IsListButton)
			{
				this.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.RIGHT_RELEASE:
		case POINTER_INFO.INPUT_EVENT.RIGHT_TAP:
			if (ptr.type != POINTER_INFO.POINTER_TYPE.TOUCHPAD && ptr.hitInfo.collider == base.collider)
			{
				if (!this.IsListButton)
				{
					this.SetControlState(UIButton.CONTROL_STATE.OVER);
				}
			}
			else if (!this.IsListButton)
			{
				this.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE:
			if (this.m_ctrlState != UIButton.CONTROL_STATE.OVER)
			{
				if (!this.IsListButton)
				{
					this.SetControlState(UIButton.CONTROL_STATE.OVER);
				}
				if (this.OverColorText != string.Empty && this.OverColorText != base.ColorText)
				{
					this.NormalColorText = base.ColorText;
					base.ColorText = this.OverColorText;
					this.Text = this.BaseString;
				}
				if (this.soundOnOver != null)
				{
					this.soundOnOver.PlayOneShot(this.soundOnOver.clip);
				}
				if (this.mouseOverDelegate != null)
				{
					this.mouseOverDelegate(this);
				}
			}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			if (this.m_ctrlState != UIButton.CONTROL_STATE.NORMAL && this.mouseOutDelegate != null)
			{
				this.mouseOutDelegate(this);
			}
			if (!this.IsListButton)
			{
				this.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			}
			if (this.NormalColorText != string.Empty && this.BaseString != string.Empty && this.NormalColorText != base.ColorText)
			{
				base.ColorText = this.NormalColorText;
				this.Text = this.BaseString;
			}
			break;
		}
		base.OnInput(ref ptr);
		if (this.repeat)
		{
			if (this.m_ctrlState == UIButton.CONTROL_STATE.ACTIVE)
			{
				goto IL_4A2;
			}
		}
		else if (ptr.evt == this.whenToInvoke)
		{
			goto IL_4A2;
		}
		return;
		IL_4A2:
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
			if (this.bUseDefaultSound)
			{
				MsgHandler.Handle("ButtonSound", new object[0]);
			}
		}
	}

	public void CallChangeDelegate()
	{
		if (this.changeDelegate != null)
		{
			this.changeDelegate(this);
		}
	}

	public override void Start()
	{
		if (this.m_started)
		{
			return;
		}
		base.Start();
		if (Application.isPlaying)
		{
			this.aggregateLayers = new SpriteRoot[1][];
			this.aggregateLayers[0] = this.layers;
			this.stateIndices = new int[this.layers.Length, 4];
			for (int i = 0; i < this.layers.Length; i++)
			{
				if (this.layers[i] == null)
				{
					TsLog.LogError("A null layer sprite was encountered on control \"" + base.name + "\". Please fill in the layer reference, or remove the empty element.", new object[0]);
				}
				else
				{
					this.stateIndices[i, 0] = this.layers[i].GetStateIndex("normal");
					this.stateIndices[i, 1] = this.layers[i].GetStateIndex("over");
					this.stateIndices[i, 2] = this.layers[i].GetStateIndex("active");
					this.stateIndices[i, 3] = this.layers[i].GetStateIndex("disabled");
					if (this.stateIndices[i, (int)this.m_ctrlState] != -1)
					{
						this.layers[i].SetState(this.stateIndices[i, (int)this.m_ctrlState]);
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
			this.SetState((int)this.m_ctrlState);
		}
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.transitionQueued)
		{
			this.nextTransition.RemoveTransitionEndDelegate(new EZTransition.OnTransitionEndDelegate(this.RunFollowupTrans));
			this.transitionQueued = false;
		}
		if (EZAnimator.Exists() && !this.deleted)
		{
			bool flag = this.alwaysFinishActiveTransition;
			this.alwaysFinishActiveTransition = false;
			if (this.controlState == UIButton.CONTROL_STATE.DISABLED)
			{
				this.SetControlState(UIButton.CONTROL_STATE.DISABLED);
			}
			else if (!this.isListButton)
			{
				this.SetControlState(UIButton.CONTROL_STATE.NORMAL);
			}
			if (this.prevTransition != null && this.prevTransition.IsRunning())
			{
				this.prevTransition.End();
			}
			this.alwaysFinishActiveTransition = flag;
		}
		this.prevTransition = null;
	}

	public override void Copy(SpriteRoot s)
	{
		this.Copy(s, ControlCopyFlags.All);
	}

	public override void Copy(SpriteRoot s, ControlCopyFlags flags)
	{
		base.Copy(s, flags);
		if (!(s is UIButton))
		{
			return;
		}
		UIButton uIButton = (UIButton)s;
		if ((flags & ControlCopyFlags.State) == ControlCopyFlags.State)
		{
			this.prevTransition = uIButton.prevTransition;
			if (Application.isPlaying)
			{
				this.SetControlState(uIButton.controlState);
			}
		}
		if ((flags & ControlCopyFlags.Invocation) == ControlCopyFlags.Invocation)
		{
			this.scriptWithMethodToInvoke = uIButton.scriptWithMethodToInvoke;
			this.methodToInvoke = uIButton.methodToInvoke;
			this.whenToInvoke = uIButton.whenToInvoke;
			this.delay = uIButton.delay;
		}
		if ((flags & ControlCopyFlags.Sound) == ControlCopyFlags.Sound)
		{
			this.soundOnOver = uIButton.soundOnOver;
			this.soundOnClick = uIButton.soundOnClick;
		}
		if ((flags & ControlCopyFlags.Settings) == ControlCopyFlags.Settings)
		{
			this.repeat = uIButton.repeat;
		}
	}

	public void SetControlState(UIButton.CONTROL_STATE s)
	{
		if (this.m_ctrlState == s)
		{
			return;
		}
		int ctrlState = (int)this.m_ctrlState;
		this.m_ctrlState = s;
		if (this.animations[(int)s].GetFrameCount() > 0)
		{
			this.SetState((int)s);
		}
		base.UseStateLabel((int)s);
		if (s == UIButton.CONTROL_STATE.DISABLED)
		{
			this.m_controlIsEnabled = false;
		}
		else
		{
			this.m_controlIsEnabled = true;
		}
		this.UpdateCollider();
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (this.stateIndices[i, (int)s] != -1)
			{
				this.layers[i].Hide(false);
				this.layers[i].SetState(this.stateIndices[i, (int)s]);
			}
			else
			{
				this.layers[i].Hide(true);
			}
		}
		if (!this.alwaysFinishActiveTransition || (this.prevTransition != this.transitions[2].list[0] && this.prevTransition != this.transitions[2].list[1]))
		{
			if (this.prevTransition != null)
			{
				this.prevTransition.StopSafe();
			}
			this.StartTransition((int)s, ctrlState);
		}
		else
		{
			this.QueueTransition((int)s, 2);
		}
	}

	protected int DetermineNextTransition(int newState, int prevState)
	{
		int result = 0;
		switch (newState)
		{
		case 0:
			switch (prevState)
			{
			case 1:
				result = 0;
				break;
			case 2:
				result = 1;
				break;
			case 3:
				result = 2;
				break;
			}
			break;
		case 1:
			switch (prevState)
			{
			case 0:
				result = 0;
				break;
			case 2:
				result = 1;
				break;
			}
			break;
		case 2:
			if (prevState != 0)
			{
				if (prevState == 1)
				{
					result = 1;
				}
			}
			else
			{
				result = 0;
			}
			break;
		case 3:
			switch (prevState)
			{
			case 0:
				result = 0;
				break;
			case 1:
				result = 1;
				break;
			case 2:
				result = 2;
				break;
			}
			break;
		}
		return result;
	}

	protected void StartTransition(int newState, int prevState)
	{
		int num = this.DetermineNextTransition(newState, prevState);
		this.prevTransition = this.transitions[newState].list[num];
		this.prevTransition.Start();
	}

	protected void QueueTransition(int newState, int prevState)
	{
		if (this.deleted)
		{
			return;
		}
		this.nextTransition = this.transitions[newState].list[this.DetermineNextTransition(newState, prevState)];
		if (!this.transitionQueued)
		{
			this.prevTransition.AddTransitionEndDelegate(new EZTransition.OnTransitionEndDelegate(this.RunFollowupTrans));
		}
		this.transitionQueued = true;
	}

	protected void RunFollowupTrans(EZTransition trans)
	{
		if (this.deleted)
		{
			trans.RemoveTransitionEndDelegate(new EZTransition.OnTransitionEndDelegate(this.RunFollowupTrans));
			return;
		}
		this.prevTransition = this.nextTransition;
		this.nextTransition = null;
		trans.RemoveTransitionEndDelegate(new EZTransition.OnTransitionEndDelegate(this.RunFollowupTrans));
		this.transitionQueued = false;
		if (this.prevTransition != null)
		{
			this.prevTransition.Start();
		}
	}

	public override void InitUVs()
	{
		if (this.states[0].spriteFrames.Length != 0)
		{
			this.frameInfo.Copy(this.states[0].spriteFrames[0]);
		}
		base.InitUVs();
	}

	public static UIButton Create(string name, Vector3 pos)
	{
		return (UIButton)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(UIButton));
	}

	public static UIButton Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (UIButton)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(UIButton));
	}

	public override void DrawPreTransitionUI(int selState, IGUIScriptSelector gui)
	{
		this.scriptWithMethodToInvoke = gui.DrawScriptSelection(this.scriptWithMethodToInvoke, ref this.methodToInvoke);
	}

	public void PlayAni(bool flag)
	{
		this.bPlayAni = flag;
		if (!this.bPlayAni)
		{
			this.SetControlState(UIButton.CONTROL_STATE.NORMAL);
		}
	}

	public virtual void Update()
	{
		if (null == this)
		{
			return;
		}
		if (this.m_started && this.bPlayAni && Time.realtimeSinceStartup - this.startTime > 0.3f)
		{
			this.index++;
			if (this.index >= this.states.Length - 1)
			{
				this.index = 0;
			}
			base.DoAnim(this.index);
			this.startTime = Time.realtimeSinceStartup;
		}
	}

	public void SetButtonTextureKey(UIBaseInfoLoader kBaseInfo)
	{
		if (kBaseInfo == null)
		{
			return;
		}
		this.SetButtonImage(kBaseInfo);
	}

	public void SetButtonTextureKey(string key)
	{
		UIBaseInfoLoader uIBaseInfoLoader = NrTSingleton<UIImageInfoManager>.Instance.FindUIImageDictionary(key);
		if (uIBaseInfoLoader == null)
		{
			return;
		}
		this.SetButtonImage(uIBaseInfoLoader);
	}

	private float GetPixelToUVsWidth(Material mat, float width)
	{
		float num = 1f / (float)mat.mainTexture.width;
		return width * num;
	}

	private float GetPixelToUVsHeight(Material mat, float height)
	{
		float num = 1f / (float)mat.mainTexture.height;
		return height * num;
	}

	private void SetButtonImage(UIBaseInfoLoader kBaseInfo)
	{
		base.SetSpriteTile(kBaseInfo.Tile, kBaseInfo.UVs.width / (float)kBaseInfo.ButtonCount, kBaseInfo.UVs.height);
		this.m_bPattern = kBaseInfo.Pattern;
		Material material = (Material)CResources.Load(kBaseInfo.Material);
		base.Setup(this.width, this.height, material);
		float pixelToUVsWidth = this.GetPixelToUVsWidth(material, kBaseInfo.UVs.width / (float)kBaseInfo.ButtonCount);
		Rect rect = new Rect(this.GetPixelToUVsWidth(material, kBaseInfo.UVs.x) - pixelToUVsWidth, 1f - this.GetPixelToUVsHeight(material, kBaseInfo.UVs.y + kBaseInfo.UVs.height), pixelToUVsWidth, this.GetPixelToUVsHeight(material, kBaseInfo.UVs.height));
		Rect uvs = new Rect(rect);
		uvs.x += pixelToUVsWidth;
		for (int i = 0; i < 4; i++)
		{
			this.States[i].spriteFrames = new CSpriteFrame[1];
			this.States[i].spriteFrames[0] = new CSpriteFrame();
			rect.x += pixelToUVsWidth;
			if ((int)kBaseInfo.ButtonCount <= i)
			{
				this.States[i].spriteFrames[0].uvs = uvs;
			}
			else
			{
				this.States[i].spriteFrames[0].uvs = rect;
			}
			this.animations[i] = new UVAnimation();
			this.animations[i].SetAnim(this.States[i], i);
		}
		base.PlayAnim(0, 0);
	}

	public void AlphaAni(float startA, float destA, float time)
	{
		SpriteRoot[] componentsInChildren = base.transform.GetComponentsInChildren<SpriteRoot>(true);
		if (componentsInChildren != null)
		{
			SpriteRoot[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SpriteRoot spriteRoot = array[i];
				if (null != spriteRoot)
				{
					FadeSprite.Do(spriteRoot, EZAnimation.ANIM_MODE.FromTo, new Color(spriteRoot.color.r, spriteRoot.color.g, spriteRoot.color.b, startA), new Color(spriteRoot.color.r, spriteRoot.color.g, spriteRoot.color.b, destA), new EZAnimation.Interpolator(EZAnimation.linear), time, 0f, null, null);
				}
			}
		}
		SpriteText[] componentsInChildren2 = base.transform.GetComponentsInChildren<SpriteText>(true);
		if (componentsInChildren2 != null)
		{
			SpriteText[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				SpriteText spriteText = array2[j];
				if (null != spriteText)
				{
					FadeText.Do(spriteText, EZAnimation.ANIM_MODE.FromTo, new Color(spriteText.color.r, spriteText.color.g, spriteText.color.b, startA), new Color(spriteText.color.r, spriteText.color.g, spriteText.color.b, destA), new EZAnimation.Interpolator(EZAnimation.linear), time, 0f, null, null);
				}
			}
		}
	}

	public void StopAni()
	{
		SpriteRoot[] componentsInChildren = base.transform.GetComponentsInChildren<SpriteRoot>(true);
		if (componentsInChildren != null)
		{
			SpriteRoot[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				SpriteRoot spriteRoot = array[i];
				if (null != spriteRoot)
				{
					EZAnimator.instance.Stop(spriteRoot.gameObject);
					EZAnimator.instance.Stop(spriteRoot);
				}
			}
		}
		SpriteText[] componentsInChildren2 = base.transform.GetComponentsInChildren<SpriteText>(true);
		if (componentsInChildren2 != null)
		{
			SpriteText[] array2 = componentsInChildren2;
			for (int j = 0; j < array2.Length; j++)
			{
				SpriteText spriteText = array2[j];
				if (null != spriteText)
				{
					EZAnimator.instance.Stop(spriteText.gameObject);
					EZAnimator.instance.Stop(spriteText);
				}
			}
		}
	}
}
