using System;
using UnityEngine;

namespace WellFired
{
	[USequencerEvent("Spawn/Spawn Prefab(NDOORS)"), USequencerEventHideDuration, USequencerFriendlyName("Spawn Prefab(NDOORS)")]
	public class NUSSpawnPrefabEvent : USSpawnPrefabEvent
	{
		[SerializeField]
		private GameObject affectedObject;

		[SerializeField]
		private string affectedObjectPath;

		private GameObject instantiatedObject;

		public new GameObject AffectedObject
		{
			get
			{
				if (this.affectedObject == null && this.affectedObjectPath != string.Empty)
				{
					GameObject gameObject = GameObject.Find(this.affectedObjectPath);
					this.affectedObject = gameObject;
				}
				return this.affectedObject;
			}
			set
			{
				this.affectedObject = value;
			}
		}

		public override void FireEvent()
		{
			if (!this.spawnPrefab)
			{
				this.spawnPrefab = this.AffectedObject;
			}
			GameObject gameObject;
			if (this.spawnTransform)
			{
				gameObject = (UnityEngine.Object.Instantiate(this.spawnPrefab, this.spawnTransform.position, this.spawnTransform.rotation) as GameObject);
			}
			else
			{
				gameObject = (UnityEngine.Object.Instantiate(this.spawnPrefab, Vector3.zero, Quaternion.identity) as GameObject);
			}
			this.instantiatedObject = gameObject;
		}

		public void SetSpawnPrefabByObjectPath()
		{
			if (this.affectedObjectPath == string.Empty)
			{
				this.affectedObjectPath = this.affectedObject.transform.GetFullHierarchyPath();
				this.spawnPrefab = this.AffectedObject;
			}
		}

		public override void StopEvent()
		{
			UnityEngine.Object.DestroyImmediate(this.instantiatedObject);
		}
	}
}
