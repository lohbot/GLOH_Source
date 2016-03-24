using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	[ExecuteInEditMode]
	[Serializable]
	public class USTimelineObserver : USTimelineBase
	{
		[Serializable]
		private class SnapShotEntry
		{
			public Camera camera;

			public AudioListener listener;

			public RenderTexture target;

			public bool state;
		}

		[SerializeField]
		public List<USObserverKeyframe> observerKeyframes = new List<USObserverKeyframe>();

		[SerializeField]
		private List<USTimelineObserver.SnapShotEntry> currentSnapshots = new List<USTimelineObserver.SnapShotEntry>();

		private List<Camera> camerasAtLastSnapShot;

		public LayerMask layersToIgnore = 0;

		public USObserverKeyframe CurrentlyActiveKeyframe
		{
			get;
			set;
		}

		private int KeyframeComparer(USObserverKeyframe a, USObserverKeyframe b)
		{
			if (a == null && b == null)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			return a.FireTime.CompareTo(b.FireTime);
		}

		public override void StopTimeline()
		{
			this.RevertToSnapshot();
			this.currentSnapshots.Clear();
			if (this.camerasAtLastSnapShot != null)
			{
				this.camerasAtLastSnapShot.Clear();
			}
		}

		public override void PauseTimeline()
		{
		}

		public override void ResumeTimeline()
		{
		}

		public override void StartTimeline()
		{
			this.SortKeyframes();
			this.CreateSnapshot();
			if (this.observerKeyframes.Count > 0)
			{
				this.DisableAllCameras();
			}
			this.Process(0f, base.Sequence.PlaybackRate);
		}

		private void DisableAllCameras()
		{
			List<Camera> list = this.AllValidCameras();
			foreach (Camera current in list)
			{
				if (NrTSingleton<UIManager>.Instance.uiCameras[0].camera != null && NrTSingleton<UIManager>.Instance.uiCameras[0].camera != current)
				{
					current.enabled = false;
				}
				AudioListener component = current.gameObject.GetComponent<AudioListener>();
				if (component)
				{
					component.enabled = false;
				}
			}
		}

		public override void ManuallySetTime(float sequencerTime)
		{
			this.Process(sequencerTime, 1f);
		}

		public override void SkipTimelineTo(float time)
		{
			this.Process(time, 1f);
		}

		public override void Process(float sequencerTime, float playbackRate)
		{
			USObserverKeyframe uSObserverKeyframe = null;
			for (int i = 0; i < this.observerKeyframes.Count; i++)
			{
				USObserverKeyframe uSObserverKeyframe2 = this.observerKeyframes[i];
				if (uSObserverKeyframe2.FireTime <= sequencerTime)
				{
					if (uSObserverKeyframe == null)
					{
						uSObserverKeyframe = uSObserverKeyframe2;
					}
					if (uSObserverKeyframe2.FireTime > uSObserverKeyframe.FireTime)
					{
						uSObserverKeyframe = uSObserverKeyframe2;
					}
				}
			}
			if (this.CurrentlyActiveKeyframe != null && sequencerTime >= this.CurrentlyActiveKeyframe.FireTime + this.CurrentlyActiveKeyframe.TransitionDuration && this.CurrentlyActiveKeyframe.Fired)
			{
				this.CurrentlyActiveKeyframe.Process(sequencerTime - this.CurrentlyActiveKeyframe.FireTime);
				this.CurrentlyActiveKeyframe.End();
				USObserverKeyframe uSObserverKeyframe3 = this.KeyframeBefore(this.CurrentlyActiveKeyframe);
				if (uSObserverKeyframe3 && uSObserverKeyframe3.AudioListener)
				{
					uSObserverKeyframe3.AudioListener.enabled = false;
				}
			}
			if (uSObserverKeyframe != this.CurrentlyActiveKeyframe)
			{
				USObserverKeyframe currentlyActiveKeyframe = this.CurrentlyActiveKeyframe;
				this.CurrentlyActiveKeyframe = uSObserverKeyframe;
				List<USObserverKeyframe> list = this.CollectAllKeyframesBetween(currentlyActiveKeyframe, this.CurrentlyActiveKeyframe);
				bool flag = (currentlyActiveKeyframe == null && this.CurrentlyActiveKeyframe != null) || (this.CurrentlyActiveKeyframe != null && currentlyActiveKeyframe.FireTime < this.CurrentlyActiveKeyframe.FireTime);
				if (flag)
				{
					for (int j = 0; j < list.Count<USObserverKeyframe>(); j++)
					{
						if (!(currentlyActiveKeyframe != null) || j != 0)
						{
							if (j - 1 >= 0)
							{
								list[j - 1].UnFire();
							}
							list[j].Fire((j <= 0) ? null : list[j - 1].KeyframeCamera);
							if (j != list.Count<USObserverKeyframe>() - 1)
							{
								list[j].Process(sequencerTime - this.CurrentlyActiveKeyframe.FireTime);
								list[j].End();
							}
						}
					}
				}
				else
				{
					for (int k = list.Count<USObserverKeyframe>() - 1; k >= 0; k--)
					{
						if (this.CurrentlyActiveKeyframe != null && k == 0)
						{
							USObserverKeyframe uSObserverKeyframe4 = this.KeyframeBefore(this.CurrentlyActiveKeyframe);
							list[k].Fire((!uSObserverKeyframe4) ? null : uSObserverKeyframe4.KeyframeCamera);
						}
						else
						{
							bool flag2 = true;
							if (base.Sequence.IsPingPonging && list[k].FireTime <= 0f)
							{
								flag2 = false;
							}
							if (flag2)
							{
								list[k].Revert();
							}
						}
					}
				}
			}
			if (this.CurrentlyActiveKeyframe)
			{
				this.CurrentlyActiveKeyframe.Process(sequencerTime - this.CurrentlyActiveKeyframe.FireTime);
			}
		}

		private USObserverKeyframe KeyframeBefore(USObserverKeyframe currentlyActiveKeyframe)
		{
			if (currentlyActiveKeyframe == null)
			{
				return null;
			}
			USObserverKeyframe uSObserverKeyframe = null;
			foreach (USObserverKeyframe current in this.observerKeyframes)
			{
				if (current.FireTime < currentlyActiveKeyframe.FireTime && (uSObserverKeyframe == null || uSObserverKeyframe.FireTime < current.FireTime))
				{
					uSObserverKeyframe = current;
				}
			}
			return uSObserverKeyframe;
		}

		private List<USObserverKeyframe> CollectAllKeyframesBetween(USObserverKeyframe prevKeyframe, USObserverKeyframe currentKeyframe)
		{
			USObserverKeyframe uSObserverKeyframe;
			if (prevKeyframe == null)
			{
				uSObserverKeyframe = this.observerKeyframes[0];
			}
			else if (currentKeyframe == null)
			{
				uSObserverKeyframe = this.observerKeyframes[0];
			}
			else
			{
				uSObserverKeyframe = ((prevKeyframe.FireTime >= currentKeyframe.FireTime) ? currentKeyframe : prevKeyframe);
			}
			USObserverKeyframe uSObserverKeyframe2;
			if (prevKeyframe == null)
			{
				uSObserverKeyframe2 = currentKeyframe;
			}
			else if (currentKeyframe == null)
			{
				uSObserverKeyframe2 = prevKeyframe;
			}
			else
			{
				uSObserverKeyframe2 = ((!(uSObserverKeyframe == prevKeyframe)) ? prevKeyframe : currentKeyframe);
			}
			List<USObserverKeyframe> list = new List<USObserverKeyframe>();
			foreach (USObserverKeyframe current in this.observerKeyframes)
			{
				if (current.FireTime >= uSObserverKeyframe.FireTime && current.FireTime <= uSObserverKeyframe2.FireTime)
				{
					list.Add(current);
				}
			}
			return list;
		}

		private void Update()
		{
			if (Application.isEditor && this.observerKeyframes.Count > 0 && base.Sequence.RunningTime > 0f && !this.ValidatePreviousSnapshot())
			{
				this.RevertToSnapshot();
				this.CreateSnapshot();
				this.Process(base.Sequence.RunningTime, 1f);
			}
		}

		public USObserverKeyframe AddKeyframe(USObserverKeyframe keyframe)
		{
			keyframe.observer = this;
			this.observerKeyframes.Add(keyframe);
			this.SortKeyframes();
			if (this.observerKeyframes.Count == 1 && base.Sequence.HasSequenceBeenStarted)
			{
				this.DisableAllCameras();
			}
			return keyframe;
		}

		private void SortKeyframes()
		{
			this.observerKeyframes.Sort(new Comparison<USObserverKeyframe>(this.KeyframeComparer));
		}

		public void RemoveKeyframe(USObserverKeyframe keyframe)
		{
			if (keyframe == null)
			{
				return;
			}
			if (!this.observerKeyframes.Contains(keyframe))
			{
				return;
			}
			this.observerKeyframes.Remove(keyframe);
		}

		public void SetKeyframes(List<USObserverKeyframe> keyframes)
		{
			this.observerKeyframes = keyframes;
		}

		public void OnEditorUndo()
		{
		}

		public void OnGUI()
		{
			if (this.CurrentlyActiveKeyframe)
			{
				this.CurrentlyActiveKeyframe.ProcessFromOnGUI();
			}
		}

		public bool IsValidCamera(Camera testCamera)
		{
			if ((1 << testCamera.gameObject.layer & this.layersToIgnore) > 0)
			{
				return false;
			}
			bool flag = (testCamera.gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable;
			bool flag2 = (testCamera.gameObject.hideFlags & HideFlags.HideAndDontSave) == HideFlags.HideAndDontSave;
			bool flag3 = (testCamera.gameObject.hideFlags & HideFlags.HideInHierarchy) == HideFlags.HideInHierarchy;
			return !flag && !flag2 && !flag3;
		}

		private bool ValidatePreviousSnapshot()
		{
			List<Camera> list = this.AllValidCameras();
			return list.Count == this.camerasAtLastSnapShot.Count && !list.Except(this.camerasAtLastSnapShot).Any<Camera>();
		}

		public List<Camera> AllValidCameras()
		{
			Camera[] source = Resources.FindObjectsOfTypeAll(typeof(Camera)) as Camera[];
			return (from camera in source
			where this.IsValidCamera(camera)
			select camera).ToList<Camera>();
		}

		private void CreateSnapshot()
		{
			this.currentSnapshots.Clear();
			if (this.camerasAtLastSnapShot != null)
			{
				this.camerasAtLastSnapShot.Clear();
			}
			this.camerasAtLastSnapShot = this.AllValidCameras();
			foreach (Camera current in this.camerasAtLastSnapShot)
			{
				this.currentSnapshots.Add(new USTimelineObserver.SnapShotEntry
				{
					camera = current,
					target = current.targetTexture,
					listener = current.gameObject.GetComponent<AudioListener>(),
					state = current.enabled
				});
			}
		}

		private void RevertToSnapshot()
		{
			foreach (USTimelineObserver.SnapShotEntry current in this.currentSnapshots)
			{
				if (current.camera != null)
				{
					current.camera.enabled = current.state;
				}
				if (current.listener != null)
				{
					current.listener.enabled = current.state;
				}
				if (current.camera != null)
				{
					current.camera.targetTexture = current.target;
				}
			}
			this.CurrentlyActiveKeyframe = null;
		}
	}
}
