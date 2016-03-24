using System;
using UnityEngine;
using UnityEngine.UI;

namespace WellFired
{
	[RequireComponent(typeof(Button))]
	public class StopSequenceButton : MonoBehaviour
	{
		[SerializeField]
		private USSequencer sequenceToStop;

		[SerializeField]
		private bool manageInteractiveState = true;

		private void Start()
		{
			Button button = base.GetComponent<Button>();
			if (!button)
			{
				Debug.LogError("The component Stop Sequence button must be added to a Unity UI Button");
				return;
			}
			if (!this.sequenceToStop)
			{
				Debug.LogError("The Sequence to stop field must be hooked up in the Inspector");
				return;
			}
			button.onClick.AddListener(delegate
			{
				this.StopSequence();
			});
			button.interactable = this.sequenceToStop.IsPlaying;
			if (this.manageInteractiveState)
			{
				USSequencer expr_92 = this.sequenceToStop;
				expr_92.OnRunningTimeSet = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_92.OnRunningTimeSet, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = USRuntimeUtility.CanStopSequence(sequence);
				}));
				USSequencer expr_B9 = this.sequenceToStop;
				expr_B9.PlaybackStarted = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_B9.PlaybackStarted, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_E0 = this.sequenceToStop;
				expr_E0.PlaybackPaused = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_E0.PlaybackPaused, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_107 = this.sequenceToStop;
				expr_107.PlaybackFinished = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_107.PlaybackFinished, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_12E = this.sequenceToStop;
				expr_12E.PlaybackStopped = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_12E.PlaybackStopped, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
			}
		}

		private void StopSequence()
		{
			this.sequenceToStop.Stop();
		}
	}
}
