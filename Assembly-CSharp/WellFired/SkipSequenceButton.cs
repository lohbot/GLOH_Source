using System;
using UnityEngine;
using UnityEngine.UI;

namespace WellFired
{
	[RequireComponent(typeof(Button))]
	public class SkipSequenceButton : MonoBehaviour
	{
		[SerializeField]
		private USSequencer sequenceToSkip;

		[SerializeField]
		private bool manageInteractiveState = true;

		private void Start()
		{
			Button button = base.GetComponent<Button>();
			if (!button)
			{
				Debug.LogError("The component Skip Sequence button must be added to a Unity UI Button");
				return;
			}
			if (!this.sequenceToSkip)
			{
				Debug.LogError("The Sequence to skip field must be hooked up in the Inspector");
				return;
			}
			button.onClick.AddListener(delegate
			{
				this.SkipSequence();
			});
			button.interactable = !this.sequenceToSkip.IsPlaying;
			if (this.manageInteractiveState)
			{
				USSequencer expr_95 = this.sequenceToSkip;
				expr_95.OnRunningTimeSet = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_95.OnRunningTimeSet, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = USRuntimeUtility.CanSkipSequence(sequence);
				}));
				USSequencer expr_BC = this.sequenceToSkip;
				expr_BC.PlaybackStarted = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_BC.PlaybackStarted, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_E3 = this.sequenceToSkip;
				expr_E3.PlaybackPaused = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_E3.PlaybackPaused, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_10A = this.sequenceToSkip;
				expr_10A.PlaybackFinished = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_10A.PlaybackFinished, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
				USSequencer expr_131 = this.sequenceToSkip;
				expr_131.PlaybackStopped = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_131.PlaybackStopped, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
			}
		}

		private void SkipSequence()
		{
			this.sequenceToSkip.SkipTimelineTo(this.sequenceToSkip.Duration);
		}
	}
}
