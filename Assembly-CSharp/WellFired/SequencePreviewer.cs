using System;
using UnityEngine;
using UnityEngine.UI;

namespace WellFired
{
	[RequireComponent(typeof(Slider))]
	public class SequencePreviewer : MonoBehaviour
	{
		[SerializeField]
		private USSequencer sequenceToPreview;

		private void Start()
		{
			Slider component = base.GetComponent<Slider>();
			if (!component)
			{
				Debug.LogError("The component SequenceSlider button must be added to a Unity Slider");
				return;
			}
			if (!this.sequenceToPreview)
			{
				Debug.LogError("The Sequence to Preview field must be hooked up in the Inspector");
				return;
			}
			component.onValueChanged.AddListener(delegate(float value)
			{
				this.SetRunningTime(value);
			});
		}

		private void SetRunningTime(float runningTime)
		{
			this.sequenceToPreview.RunningTime = runningTime * this.sequenceToPreview.Duration;
		}

		private void Update()
		{
			Slider component = base.GetComponent<Slider>();
			component.value = this.sequenceToPreview.RunningTime / this.sequenceToPreview.Duration;
		}
	}
}
