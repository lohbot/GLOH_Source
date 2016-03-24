using System;
using UnityEngine;
using UnityEngine.UI;

namespace WellFired
{
	[RequireComponent(typeof(Button))]
	public class PauseSequenceButton : MonoBehaviour
	{
		[SerializeField]
		private USSequencer sequenceToPause;

		[SerializeField]
		private bool manageInteractiveState = true;

		private void Start()
		{
			Button button = base.GetComponent<Button>();
			if (!button)
			{
				Debug.LogError("The component Pause Sequence button must be added to a Unity UI Button");
				return;
			}
			if (!this.sequenceToPause)
			{
				Debug.LogError("The Sequence to pause field must be hooked up in the Inspector");
				return;
			}
			button.onClick.AddListener(delegate
			{
				this.PauseSequence();
			});
			button.interactable = this.sequenceToPause.IsPlaying;
			if (this.manageInteractiveState)
			{
				USSequencer expr_92 = this.sequenceToPause;
				expr_92.OnRunningTimeSet = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_92.OnRunningTimeSet, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = USRuntimeUtility.CanPauseSequence(sequence);
				}));
				USSequencer expr_B9 = this.sequenceToPause;
				expr_B9.PlaybackStarted = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_B9.PlaybackStarted, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_E0 = this.sequenceToPause;
				expr_E0.PlaybackPaused = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_E0.PlaybackPaused, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
				USSequencer expr_107 = this.sequenceToPause;
				expr_107.PlaybackFinished = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_107.PlaybackFinished, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
				USSequencer expr_12E = this.sequenceToPause;
				expr_12E.PlaybackStopped = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_12E.PlaybackStopped, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
			}
		}

		private void PauseSequence()
		{
			this.sequenceToPause.Pause();
		}
	}
}
