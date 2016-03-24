using System;
using UnityEngine;
using UnityEngine.UI;

namespace WellFired
{
	[RequireComponent(typeof(Button))]
	public class PlaySequenceButton : MonoBehaviour
	{
		[SerializeField]
		private USSequencer sequenceToPlay;

		[SerializeField]
		private bool manageInteractiveState = true;

		private void Start()
		{
			Button button = base.GetComponent<Button>();
			if (!button)
			{
				Debug.LogError("The component Play Sequence button must be added to a Unity UI Button");
				return;
			}
			if (!this.sequenceToPlay)
			{
				Debug.LogError("The Sequence to play field must be hooked up in the Inspector");
				return;
			}
			button.onClick.AddListener(delegate
			{
				this.PlaySequence();
			});
			button.interactable = !this.sequenceToPlay.IsPlaying;
			if (this.manageInteractiveState)
			{
				USSequencer expr_95 = this.sequenceToPlay;
				expr_95.OnRunningTimeSet = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_95.OnRunningTimeSet, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = USRuntimeUtility.CanPlaySequence(sequence);
				}));
				USSequencer expr_BC = this.sequenceToPlay;
				expr_BC.PlaybackStarted = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_BC.PlaybackStarted, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
				USSequencer expr_E3 = this.sequenceToPlay;
				expr_E3.PlaybackPaused = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_E3.PlaybackPaused, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
				USSequencer expr_10A = this.sequenceToPlay;
				expr_10A.PlaybackFinished = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_10A.PlaybackFinished, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = false;
				}));
				USSequencer expr_131 = this.sequenceToPlay;
				expr_131.PlaybackStopped = (USSequencer.PlaybackDelegate)Delegate.Combine(expr_131.PlaybackStopped, new USSequencer.PlaybackDelegate(delegate(USSequencer sequence)
				{
					button.interactable = true;
				}));
			}
		}

		private void RunningTimeUpdated(USSequencer sequence)
		{
			Button component = base.GetComponent<Button>();
			bool flag = USRuntimeUtility.CanPlaySequence(sequence);
			component.interactable = flag;
			Debug.Log(flag);
		}

		private void PlaySequence()
		{
			this.sequenceToPlay.Play();
		}
	}
}
