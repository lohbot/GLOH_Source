using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class USInternalCurve : ScriptableObject
	{
		[SerializeField]
		private AnimationCurve animationCurve;

		[SerializeField]
		private List<USInternalKeyframe> internalKeyframes;

		[SerializeField]
		private bool useCurrentValue;

		[SerializeField]
		private float duration;

		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
			}
		}

		public float FirstKeyframeTime
		{
			get
			{
				if (this.internalKeyframes.Count == 0)
				{
					return 0f;
				}
				return this.internalKeyframes[0].Time;
			}
		}

		public float LastKeyframeTime
		{
			get
			{
				if (this.internalKeyframes.Count == 0)
				{
					return this.Duration;
				}
				return this.internalKeyframes[this.internalKeyframes.Count - 1].Time;
			}
		}

		public AnimationCurve UnityAnimationCurve
		{
			get
			{
				return this.animationCurve;
			}
			set
			{
				this.animationCurve = new AnimationCurve();
				Keyframe[] keys = value.keys;
				for (int i = 0; i < keys.Length; i++)
				{
					Keyframe key = keys[i];
					this.animationCurve.AddKey(key);
				}
				this.BuildInternalCurveFromAnimationCurve();
			}
		}

		public List<USInternalKeyframe> Keys
		{
			get
			{
				return this.internalKeyframes;
			}
		}

		public bool UseCurrentValue
		{
			get
			{
				return this.useCurrentValue;
			}
			set
			{
				this.useCurrentValue = value;
			}
		}

		public static int KeyframeComparer(USInternalKeyframe a, USInternalKeyframe b)
		{
			return a.Time.CompareTo(b.Time);
		}

		private void OnEnable()
		{
			if (this.internalKeyframes == null)
			{
				this.internalKeyframes = new List<USInternalKeyframe>();
				this.Duration = 10f;
			}
			else if (this.internalKeyframes.Count > 0)
			{
				this.Duration = (from keyframe in this.internalKeyframes
				orderby keyframe.Time
				select keyframe).LastOrDefault<USInternalKeyframe>().Time;
			}
			if (this.UnityAnimationCurve == null)
			{
				this.UnityAnimationCurve = new AnimationCurve();
			}
		}

		public float Evaluate(float time)
		{
			return this.animationCurve.Evaluate(time);
		}

		public void BuildInternalCurveFromAnimationCurve()
		{
			Keyframe[] keys = this.animationCurve.keys;
			for (int i = 0; i < keys.Length; i++)
			{
				Keyframe keyframe = keys[i];
				USInternalKeyframe uSInternalKeyframe = null;
				foreach (USInternalKeyframe current in this.internalKeyframes)
				{
					if (Mathf.Approximately(keyframe.time, current.Time))
					{
						uSInternalKeyframe = current;
						break;
					}
				}
				if (uSInternalKeyframe)
				{
					uSInternalKeyframe.ConvertFrom(keyframe);
				}
				else
				{
					USInternalKeyframe uSInternalKeyframe2 = ScriptableObject.CreateInstance<USInternalKeyframe>();
					uSInternalKeyframe2.ConvertFrom(keyframe);
					uSInternalKeyframe2.curve = this;
					this.internalKeyframes.Add(uSInternalKeyframe2);
					this.internalKeyframes.Sort(new Comparison<USInternalKeyframe>(USInternalCurve.KeyframeComparer));
				}
			}
		}

		public void BuildAnimationCurveFromInternalCurve()
		{
			while (this.animationCurve.keys.Length > 0)
			{
				this.animationCurve.RemoveKey(0);
			}
			Keyframe key = default(Keyframe);
			foreach (USInternalKeyframe current in this.Keys)
			{
				key.value = current.Value;
				key.time = current.Time;
				key.inTangent = current.InTangent;
				key.outTangent = current.OutTangent;
				this.animationCurve.AddKey(key);
			}
			this.internalKeyframes.Sort(new Comparison<USInternalKeyframe>(USInternalCurve.KeyframeComparer));
		}

		public void ValidateKeyframeTimes()
		{
			for (int i = this.Keys.Count - 1; i >= 0; i--)
			{
				if (i != this.Keys.Count - 1)
				{
					if (Mathf.Approximately(this.Keys[i].Time, this.Keys[i + 1].Time))
					{
						this.internalKeyframes.Remove(this.Keys[i]);
					}
				}
			}
		}

		public USInternalKeyframe AddKeyframe(float time, float value)
		{
			USInternalKeyframe uSInternalKeyframe = null;
			foreach (USInternalKeyframe current in this.internalKeyframes)
			{
				if (Mathf.Approximately(current.Time, time))
				{
					uSInternalKeyframe = current;
				}
				if (uSInternalKeyframe != null)
				{
					break;
				}
			}
			if (!uSInternalKeyframe)
			{
				uSInternalKeyframe = ScriptableObject.CreateInstance<USInternalKeyframe>();
				this.internalKeyframes.Add(uSInternalKeyframe);
			}
			this.Duration = Mathf.Max((from keyframe in this.internalKeyframes
			orderby keyframe.Time
			select keyframe).LastOrDefault<USInternalKeyframe>().Time, time);
			uSInternalKeyframe.curve = this;
			uSInternalKeyframe.Time = time;
			uSInternalKeyframe.Value = value;
			uSInternalKeyframe.InTangent = 0f;
			uSInternalKeyframe.OutTangent = 0f;
			this.internalKeyframes.Sort(new Comparison<USInternalKeyframe>(USInternalCurve.KeyframeComparer));
			this.BuildAnimationCurveFromInternalCurve();
			return uSInternalKeyframe;
		}

		public void RemoveKeyframe(USInternalKeyframe internalKeyframe)
		{
			for (int i = this.internalKeyframes.Count - 1; i >= 0; i--)
			{
				if (this.internalKeyframes[i] == internalKeyframe)
				{
					this.internalKeyframes.RemoveAt(i);
				}
			}
			this.internalKeyframes.Sort(new Comparison<USInternalKeyframe>(USInternalCurve.KeyframeComparer));
			this.Duration = (from keyframe in this.internalKeyframes
			orderby keyframe.Time
			select keyframe).LastOrDefault<USInternalKeyframe>().Time;
			this.BuildAnimationCurveFromInternalCurve();
		}

		public USInternalKeyframe GetNextKeyframe(USInternalKeyframe keyframe)
		{
			if (this.internalKeyframes.Count == 0)
			{
				return null;
			}
			if (this.internalKeyframes[this.internalKeyframes.Count - 1] == keyframe)
			{
				return null;
			}
			int num = -1;
			for (int i = 0; i < this.internalKeyframes.Count; i++)
			{
				if (this.internalKeyframes[i] == keyframe)
				{
					num = i;
				}
				if (num != -1)
				{
					break;
				}
			}
			return this.internalKeyframes[num + 1];
		}

		public USInternalKeyframe GetPrevKeyframe(USInternalKeyframe keyframe)
		{
			if (this.internalKeyframes.Count == 0)
			{
				return null;
			}
			if (this.internalKeyframes[0] == keyframe)
			{
				return null;
			}
			int num = -1;
			for (int i = 0; i < this.internalKeyframes.Count; i++)
			{
				if (this.internalKeyframes[i] == keyframe)
				{
					num = i;
				}
				if (num != -1)
				{
					break;
				}
			}
			return this.internalKeyframes[num - 1];
		}

		public bool Contains(USInternalKeyframe keyframe)
		{
			for (int i = this.Keys.Count - 1; i >= 0; i--)
			{
				if (this.Keys[i] == keyframe)
				{
					return true;
				}
			}
			return false;
		}

		public float FindNextKeyframeTime(float time)
		{
			float time2 = this.Duration;
			for (int i = this.Keys.Count - 1; i >= 0; i--)
			{
				if (this.Keys[i].Time < time2 && this.Keys[i].Time > time)
				{
					time2 = this.Keys[i].Time;
				}
			}
			return time2;
		}

		public float FindPrevKeyframeTime(float time)
		{
			float num = 0f;
			for (int i = 0; i < this.Keys.Count; i++)
			{
				if (this.Keys[i].Time > num && this.Keys[i].Time < time)
				{
					num = this.Keys[i].Time;
				}
			}
			return num;
		}

		public bool CanSetKeyframeToTime(float newTime)
		{
			foreach (USInternalKeyframe current in this.internalKeyframes)
			{
				if (Mathf.Approximately(current.Time, newTime))
				{
					return false;
				}
			}
			return true;
		}
	}
}
