using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired.Shared;

namespace WellFired
{
	[ExecuteInEditMode, USequencerEvent("Camera/Transition/Dissolve"), USequencerFriendlyName("Dissolve Transition")]
	public class USCameraDissolveTransition : USEventBase
	{
		private BaseTransition transition;

		[SerializeField]
		private Camera sourceCamera;

		[SerializeField]
		private Camera destinationCamera;

		private void OnGUI()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.ProcessTransitionFromOnGUI();
		}

		public override void FireEvent()
		{
			if (this.transition == null)
			{
				this.transition = new BaseTransition();
			}
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				Debug.LogError("Can't continue this transition with null cameras.");
				return;
			}
			this.transition.InitializeTransition(this.sourceCamera, this.destinationCamera, new List<Camera>(), new List<Camera>(), TypeOfTransition.Dissolve);
		}

		public override void ProcessEvent(float deltaTime)
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.ProcessEventFromNoneOnGUI(deltaTime, base.Duration);
		}

		public override void EndEvent()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.TransitionComplete();
		}

		public override void StopEvent()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.UndoEvent();
		}

		public override void UndoEvent()
		{
			if (this.sourceCamera == null || this.destinationCamera == null || this.transition == null)
			{
				return;
			}
			this.transition.RevertTransition();
		}
	}
}
