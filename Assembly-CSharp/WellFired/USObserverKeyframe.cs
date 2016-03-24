using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	[Serializable]
	public class USObserverKeyframe : ScriptableObject
	{
		public USTimelineObserver observer;

		public bool prevActiveState;

		private AudioListener cachedListener;

		private BaseTransition transition;

		[SerializeField]
		private TypeOfTransition transitionType;

		[SerializeField]
		private float transitionDuration;

		[SerializeField]
		private Camera camera;

		[SerializeField]
		public string cameraPath = string.Empty;

		[SerializeField]
		private float fireTime;

		[SerializeField]
		private List<Camera> additionalSourceCameras = new List<Camera>();

		[SerializeField]
		private List<Camera> additionalDestinationCameras = new List<Camera>();

		public bool Fired
		{
			get;
			private set;
		}

		public float FireTime
		{
			get
			{
				return this.fireTime;
			}
			set
			{
				if (this.observer)
				{
					this.fireTime = Mathf.Min(Mathf.Max(value, 0f), this.observer.Sequence.Duration);
				}
				else
				{
					this.fireTime = value;
				}
			}
		}

		public float TransitionDuration
		{
			get
			{
				return (this.transitionType != TypeOfTransition.Cut) ? this.transitionDuration : 0f;
			}
			set
			{
				this.transitionDuration = value;
			}
		}

		public TypeOfTransition TransitionType
		{
			get
			{
				return this.transitionType;
			}
			set
			{
				this.transitionType = value;
			}
		}

		public BaseTransition ActiveTransition
		{
			get
			{
				return this.transition;
			}
			set
			{
				this.transition = value;
			}
		}

		public Camera KeyframeCamera
		{
			get
			{
				return this.camera;
			}
			set
			{
				this.camera = value;
			}
		}

		public AudioListener AudioListener
		{
			get
			{
				if (!this.KeyframeCamera)
				{
					return null;
				}
				this.cachedListener = this.KeyframeCamera.GetComponent<AudioListener>();
				return this.cachedListener;
			}
			set
			{
			}
		}

		public void Fire(Camera previousCamera)
		{
			this.Fired = true;
			if (this.transitionType != TypeOfTransition.Cut)
			{
				if (previousCamera != null)
				{
					this.ActiveTransition = new BaseTransition();
					this.ActiveTransition.InitializeTransition(previousCamera, this.KeyframeCamera, this.additionalSourceCameras, this.additionalDestinationCameras, this.transitionType);
				}
				else
				{
					Debug.LogWarning("Cannot use a transition as the first cut in a sequence.");
				}
			}
			if (this.ActiveTransition == null && this.KeyframeCamera)
			{
				this.KeyframeCamera.enabled = true;
			}
			if (this.AudioListener)
			{
				this.AudioListener.enabled = true;
			}
		}

		public void UnFire()
		{
			this.Fired = false;
			if (this.ActiveTransition == null && this.KeyframeCamera)
			{
				this.KeyframeCamera.enabled = false;
			}
		}

		public void End()
		{
			this.Fired = false;
			if (this.ActiveTransition != null)
			{
				this.ActiveTransition.TransitionComplete();
			}
			this.ActiveTransition = null;
		}

		public void Revert()
		{
			this.Fired = false;
			if (this.ActiveTransition != null)
			{
				this.ActiveTransition.RevertTransition();
				if (this.KeyframeCamera)
				{
					this.KeyframeCamera.enabled = false;
				}
				if (this.AudioListener)
				{
					this.AudioListener.enabled = false;
				}
			}
			else
			{
				if (this.KeyframeCamera)
				{
					this.KeyframeCamera.enabled = false;
				}
				if (this.AudioListener)
				{
					this.AudioListener.enabled = false;
				}
			}
			this.ActiveTransition = null;
		}

		public void ProcessFromOnGUI()
		{
			if (this.ActiveTransition != null)
			{
				this.ActiveTransition.ProcessTransitionFromOnGUI();
			}
		}

		public void Process(float time)
		{
			if (this.transitionType == TypeOfTransition.Cut)
			{
				return;
			}
			if (time > this.TransitionDuration)
			{
				return;
			}
			if (this.ActiveTransition != null)
			{
				this.ActiveTransition.ProcessEventFromNoneOnGUI(time, this.TransitionDuration);
			}
			this.Fired = true;
		}
	}
}
