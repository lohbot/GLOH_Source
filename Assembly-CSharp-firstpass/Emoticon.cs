using System;
using UnityEngine;

public class Emoticon : AutoSpriteControlBase
{
	public enum CONTROL_STATE
	{
		FRAME0,
		FRAME1,
		FRAME2
	}

	private float[] delay = new float[3];

	private float wait;

	private float startTime;

	private int index;

	public string m_strEmoticonKey = string.Empty;

	protected Emoticon.CONTROL_STATE m_ctrlState;

	[HideInInspector]
	public TextureAnim[] states = new TextureAnim[]
	{
		new TextureAnim("Frame0"),
		new TextureAnim("Frame1"),
		new TextureAnim("Frame2")
	};

	public SpriteRoot[] layers = new SpriteRoot[0];

	[HideInInspector]
	public EZTransitionList[] transitions = new EZTransitionList[]
	{
		new EZTransitionList(new EZTransition[]
		{
			new EZTransition("From1"),
			new EZTransition("From2")
		})
	};

	public Emoticon.CONTROL_STATE controlState
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
	}

	public void Update()
	{
		if (this.m_started && this.wait >= 0f && Time.realtimeSinceStartup - this.startTime > this.wait)
		{
			this.index++;
			if (this.index >= this.states.Length)
			{
				this.index = 0;
			}
			base.DoAnim(this.index);
			this.startTime = Time.realtimeSinceStartup;
			this.wait = this.delay[this.index];
		}
	}

	public void SetDelay(float[] delay)
	{
		this.delay = delay;
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
			this.SetState(0);
		}
		if (this.managed && this.m_hidden)
		{
			this.Hide(true);
		}
		this.index = 0;
		this.wait = this.delay[0];
		this.startTime = Time.realtimeSinceStartup;
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
	}

	public override void InitUVs()
	{
		if (this.states[0].spriteFrames.Length != 0)
		{
			this.frameInfo.Copy(this.states[0].spriteFrames[0]);
		}
		base.InitUVs();
	}

	public static Emoticon Create(string name, Vector3 pos)
	{
		return (Emoticon)new GameObject(name)
		{
			transform = 
			{
				position = pos
			}
		}.AddComponent(typeof(Emoticon));
	}

	public static Emoticon Create(string name, Vector3 pos, Quaternion rotation)
	{
		return (Emoticon)new GameObject(name)
		{
			transform = 
			{
				position = pos,
				rotation = rotation
			}
		}.AddComponent(typeof(Emoticon));
	}
}
