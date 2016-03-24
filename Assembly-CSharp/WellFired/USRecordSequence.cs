using System;
using UnityEngine;

namespace WellFired
{
	public class USRecordSequence : MonoBehaviour
	{
		private bool isRecording;

		private bool recordOnStart;

		private int imageCount;

		private int captureFrameRate = 60;

		private int upscaleAmount = 1;

		private string capturePath = string.Empty;

		public int CaptureFrameRate
		{
			set
			{
				this.captureFrameRate = Mathf.Clamp(value, 1, 60);
			}
		}

		public int UpscaleAmount
		{
			set
			{
				this.upscaleAmount = Mathf.Clamp(value, 1, 8);
			}
		}

		public string CapturePath
		{
			set
			{
				this.capturePath = value;
			}
		}

		public bool RecordOnStart
		{
			set
			{
				this.recordOnStart = value;
			}
		}

		private void Start()
		{
			this.imageCount = 0;
			this.isRecording = this.recordOnStart;
		}

		private void LateUpdate()
		{
			Time.captureFramerate = this.captureFrameRate;
			if (this.isRecording)
			{
				Application.CaptureScreenshot(string.Concat(new object[]
				{
					this.capturePath,
					"/Screenshot_",
					this.imageCount,
					".png"
				}), this.upscaleAmount);
				this.imageCount++;
			}
		}

		public void StartRecording()
		{
			this.isRecording = true;
		}

		public void PauseRecording()
		{
			this.isRecording = false;
		}

		public void StopRecording()
		{
			this.isRecording = false;
			this.imageCount = 0;
		}
	}
}
