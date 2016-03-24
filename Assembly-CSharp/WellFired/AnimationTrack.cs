using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class AnimationTrack : ScriptableObject
	{
		[SerializeField]
		private List<AnimationClipData> trackClipList = new List<AnimationClipData>();

		[SerializeField]
		private int layer;

		public int Layer
		{
			get
			{
				return this.layer;
			}
			set
			{
				this.layer = value;
			}
		}

		public List<AnimationClipData> TrackClips
		{
			get
			{
				return this.trackClipList;
			}
			private set
			{
				this.trackClipList = value;
			}
		}

		public void AddClip(AnimationClipData clipData)
		{
			if (this.trackClipList.Contains(clipData))
			{
				throw new Exception("Track already contains Clip");
			}
			this.trackClipList.Add(clipData);
		}

		public void RemoveClip(AnimationClipData clipData)
		{
			if (!this.trackClipList.Contains(clipData))
			{
				throw new Exception("Track doesn't contains Clip");
			}
			this.trackClipList.Remove(clipData);
		}

		private void SortClips()
		{
			this.trackClipList = (from trackClip in this.trackClipList
			orderby trackClip.StartTime
			select trackClip).ToList<AnimationClipData>();
		}

		public void SetClipData(List<AnimationClipData> animationClipData)
		{
			this.trackClipList = animationClipData;
		}
	}
}
