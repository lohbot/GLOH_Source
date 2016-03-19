using System;
using UnityEngine;

[Serializable]
public class AnimParams
{
	public EZAnimation.ANIM_MODE mode = EZAnimation.ANIM_MODE.To;

	public float delay;

	public float duration;

	public bool pingPong;

	public bool restartOnRepeat;

	public bool repeatDelay;

	public EZAnimation.EASING_TYPE easing;

	[NonSerialized]
	protected EZTransition m_transition;

	public Color color = Color.white;

	public Color color2 = Color.white;

	public Vector3 vec;

	public Vector3 vec2;

	public Vector3 axis;

	public float floatVal;

	public float floatVal2;

	public string strVal = string.Empty;

	public EZTransition transition
	{
		get
		{
			return this.m_transition;
		}
		set
		{
			this.m_transition = value;
		}
	}

	public AnimParams(EZTransition trans)
	{
		this.m_transition = trans;
	}

	public void Copy(AnimParams src)
	{
		this.mode = src.mode;
		this.delay = src.delay;
		this.duration = src.duration;
		this.easing = src.easing;
		this.color = src.color;
		this.vec = src.vec;
		this.axis = src.axis;
		this.floatVal = src.floatVal;
		this.color2 = src.color2;
		this.vec2 = src.vec2;
		this.floatVal2 = src.floatVal2;
		this.strVal = src.strVal;
		this.pingPong = src.pingPong;
		this.repeatDelay = src.repeatDelay;
		this.restartOnRepeat = src.restartOnRepeat;
	}

	public virtual void DrawGUI(EZAnimation.ANIM_TYPE type, GameObject go, IGUIHelper gui, bool inspector)
	{
		float pixels = 0f;
		float pixels2 = 0f;
		bool changed = GUI.changed;
		GUI.changed = false;
		this.delay = gui.FloatField("Delay:", this.delay);
		if (!inspector)
		{
			GUILayout.Space(pixels);
		}
		this.duration = gui.FloatField("Duration:", this.duration);
		if (this.duration < 0f)
		{
			this.repeatDelay = GUILayout.Toggle(this.repeatDelay, new GUIContent("Rep. Delay", "Repeats the delay on each loop iteration"), new GUILayoutOption[0]);
			if (type != EZAnimation.ANIM_TYPE.AnimClip || type != EZAnimation.ANIM_TYPE.Crash || type != EZAnimation.ANIM_TYPE.CrashRotation || type != EZAnimation.ANIM_TYPE.PunchPosition || type != EZAnimation.ANIM_TYPE.PunchRotation || type != EZAnimation.ANIM_TYPE.PunchScale || type != EZAnimation.ANIM_TYPE.Shake || type != EZAnimation.ANIM_TYPE.ShakeRotation || type != EZAnimation.ANIM_TYPE.SmoothCrash)
			{
				this.pingPong = GUILayout.Toggle(this.pingPong, new GUIContent("PingPong", "Ping-Pong: Causes the animated value to go back and forth as it loops."), new GUILayoutOption[0]);
			}
		}
		if (!inspector)
		{
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(pixels2);
		}
		if (type == EZAnimation.ANIM_TYPE.FadeMaterial || type == EZAnimation.ANIM_TYPE.FadeSprite || type == EZAnimation.ANIM_TYPE.FadeAudio || type == EZAnimation.ANIM_TYPE.TuneAudio || type == EZAnimation.ANIM_TYPE.FadeText || type == EZAnimation.ANIM_TYPE.Rotate || type == EZAnimation.ANIM_TYPE.Scale || type == EZAnimation.ANIM_TYPE.Translate)
		{
			this.easing = (EZAnimation.EASING_TYPE)gui.EnumField("Easing:", this.easing);
		}
		if (!inspector)
		{
			GUILayout.Space(pixels);
		}
		if (type == EZAnimation.ANIM_TYPE.FadeMaterial || type == EZAnimation.ANIM_TYPE.FadeSprite || type == EZAnimation.ANIM_TYPE.FadeAudio || type == EZAnimation.ANIM_TYPE.TuneAudio || type == EZAnimation.ANIM_TYPE.FadeText || type == EZAnimation.ANIM_TYPE.Rotate || type == EZAnimation.ANIM_TYPE.Scale || type == EZAnimation.ANIM_TYPE.Translate)
		{
			this.mode = (EZAnimation.ANIM_MODE)gui.EnumField("Mode:", this.mode);
		}
		if (this.duration < 0f && (type == EZAnimation.ANIM_TYPE.FadeMaterial || type == EZAnimation.ANIM_TYPE.FadeSprite || type == EZAnimation.ANIM_TYPE.FadeAudio || type == EZAnimation.ANIM_TYPE.TuneAudio || type == EZAnimation.ANIM_TYPE.FadeText || type == EZAnimation.ANIM_TYPE.Rotate || type == EZAnimation.ANIM_TYPE.Scale || type == EZAnimation.ANIM_TYPE.Translate))
		{
			this.restartOnRepeat = GUILayout.Toggle(this.restartOnRepeat, new GUIContent("Restart on Loop", "Resets the starting value on each loop iteration. Set this to false if you want something like continuous movement in the same direction without going back to the starting point."), new GUILayoutOption[0]);
		}
		if (!inspector)
		{
			GUILayout.EndHorizontal();
		}
		switch (type)
		{
		case EZAnimation.ANIM_TYPE.AnimClip:
			this.strVal = gui.TextField("Anim Clip:", this.strVal);
			if (!inspector)
			{
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				this.floatVal = Mathf.Clamp01(gui.FloatField("Blend Weight:", this.floatVal));
				GUILayout.Space(15f);
				this.floatVal = GUILayout.HorizontalSlider(this.floatVal, 0f, 1f, new GUILayoutOption[]
				{
					GUILayout.Width(200f)
				});
				GUILayout.EndHorizontal();
			}
			else
			{
				this.floatVal = Mathf.Clamp01(gui.FloatField("Blend Weight:", this.floatVal));
			}
			break;
		case EZAnimation.ANIM_TYPE.FadeSprite:
		case EZAnimation.ANIM_TYPE.FadeMaterial:
		case EZAnimation.ANIM_TYPE.FadeText:
			if (this.mode == EZAnimation.ANIM_MODE.FromTo)
			{
				this.color = gui.ColorField("Start Color:", this.color);
				this.color2 = gui.ColorField("End Color:", this.color2);
			}
			else
			{
				this.color = gui.ColorField("Color:", this.color);
			}
			break;
		case EZAnimation.ANIM_TYPE.Translate:
			if (this.mode == EZAnimation.ANIM_MODE.FromTo)
			{
				this.vec = gui.Vector3Field("Start Pos:", this.vec);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
				{
					this.vec = go.transform.localPosition;
					GUI.changed = true;
				}
				if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
				{
					go.transform.localPosition = this.vec;
				}
				GUILayout.EndHorizontal();
				this.vec2 = gui.Vector3Field("End Pos:", this.vec2);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
				{
					this.vec2 = go.transform.localPosition;
					GUI.changed = true;
				}
				if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
				{
					go.transform.localPosition = this.vec2;
				}
				GUILayout.EndHorizontal();
			}
			else if (this.mode == EZAnimation.ANIM_MODE.By)
			{
				this.vec = gui.Vector3Field("Vector:", this.vec);
			}
			else
			{
				this.vec = gui.Vector3Field("Pos:", this.vec);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
				{
					this.vec = go.transform.localPosition;
					GUI.changed = true;
				}
				if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
				{
					go.transform.localPosition = this.vec;
				}
				GUILayout.EndHorizontal();
			}
			break;
		case EZAnimation.ANIM_TYPE.PunchPosition:
		case EZAnimation.ANIM_TYPE.Crash:
		case EZAnimation.ANIM_TYPE.PunchScale:
		case EZAnimation.ANIM_TYPE.PunchRotation:
			this.vec = gui.Vector3Field("Magnitude:", this.vec);
			break;
		case EZAnimation.ANIM_TYPE.SmoothCrash:
		case EZAnimation.ANIM_TYPE.CrashRotation:
			this.vec = gui.Vector3Field("Magnitude:", this.vec);
			this.vec2 = gui.Vector3Field("Oscillations:", this.vec2);
			break;
		case EZAnimation.ANIM_TYPE.Shake:
		case EZAnimation.ANIM_TYPE.ShakeRotation:
			this.vec = gui.Vector3Field("Magnitude:", this.vec);
			this.floatVal = gui.FloatField("Oscillations:", this.floatVal);
			break;
		case EZAnimation.ANIM_TYPE.Scale:
			if (this.mode == EZAnimation.ANIM_MODE.FromTo)
			{
				this.vec = gui.Vector3Field("Start Scale:", this.vec);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
				{
					this.vec = go.transform.localScale;
					GUI.changed = true;
				}
				if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
				{
					go.transform.localScale = this.vec;
				}
				GUILayout.EndHorizontal();
				this.vec2 = gui.Vector3Field("End Scale:", this.vec2);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
				{
					this.vec2 = go.transform.localScale;
					GUI.changed = true;
				}
				if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
				{
					go.transform.localScale = this.vec2;
				}
				GUILayout.EndHorizontal();
			}
			else
			{
				this.vec = gui.Vector3Field("Scale:", this.vec);
				if (this.mode == EZAnimation.ANIM_MODE.To)
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
					{
						this.vec = go.transform.localScale;
						GUI.changed = true;
					}
					if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
					{
						go.transform.localScale = this.vec;
					}
					GUILayout.EndHorizontal();
				}
			}
			break;
		case EZAnimation.ANIM_TYPE.Rotate:
			if (this.mode == EZAnimation.ANIM_MODE.FromTo)
			{
				this.vec = gui.Vector3Field("Start Angles:", this.vec);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
				{
					this.vec = go.transform.localEulerAngles;
					GUI.changed = true;
				}
				if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
				{
					go.transform.localEulerAngles = this.vec;
				}
				GUILayout.EndHorizontal();
				this.vec2 = gui.Vector3Field("End Angles:", this.vec2);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
				{
					this.vec2 = go.transform.localEulerAngles;
					GUI.changed = true;
				}
				if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
				{
					go.transform.localEulerAngles = this.vec2;
				}
				GUILayout.EndHorizontal();
			}
			else
			{
				this.vec = gui.Vector3Field("Angles:", this.vec);
				if (this.mode == EZAnimation.ANIM_MODE.To)
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					if (GUILayout.Button(new GUIContent("Use Current", "Uses the object's current value for this field"), new GUILayoutOption[0]))
					{
						this.vec = go.transform.localEulerAngles;
						GUI.changed = true;
					}
					if (GUILayout.Button(new GUIContent("Set as Current", "Applies the displayed values to the current object."), new GUILayoutOption[0]))
					{
						go.transform.localEulerAngles = this.vec;
					}
					GUILayout.EndHorizontal();
				}
			}
			break;
		case EZAnimation.ANIM_TYPE.FadeAudio:
			if (this.mode == EZAnimation.ANIM_MODE.FromTo)
			{
				this.floatVal = gui.FloatField("Start Volume:", this.floatVal);
				this.floatVal2 = gui.FloatField("End Volume:", this.floatVal2);
			}
			else
			{
				this.floatVal = gui.FloatField("Volume:", this.floatVal);
			}
			break;
		case EZAnimation.ANIM_TYPE.TuneAudio:
			if (this.mode == EZAnimation.ANIM_MODE.FromTo)
			{
				this.floatVal = gui.FloatField("Start pitch:", this.floatVal);
				this.floatVal2 = gui.FloatField("End pitch:", this.floatVal2);
			}
			else
			{
				this.floatVal = gui.FloatField("Pitch:", this.floatVal);
			}
			break;
		}
		if (GUI.changed)
		{
			this.m_transition.initialized = true;
		}
		GUI.changed = (changed || GUI.changed);
	}
}
