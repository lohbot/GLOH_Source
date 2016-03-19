using System;
using System.Collections.Generic;
using UnityEngine;

public class BlodLOD : MonoBehaviour
{
	[Serializable]
	private class Item : IComparable<BlodLOD.Item>
	{
		public float distance = 10f;

		public Transform target;

		public int CompareTo(BlodLOD.Item rhand)
		{
			if (this.distance == rhand.distance)
			{
				return 0;
			}
			if (this.distance > rhand.distance)
			{
				return -1;
			}
			return 1;
		}
	}

	[SerializeField]
	private List<BlodLOD.Item> lodItems = new List<BlodLOD.Item>();

	[NonSerialized]
	private int _curIdxLOD;

	public static int skipFrameLimit = 5;

	private int skipFrame;

	private void Start()
	{
		this.lodItems.Sort();
		this.skipFrame = UnityEngine.Random.Range(0, BlodLOD.skipFrameLimit);
		if (BlodMain.Instance != null && BlodMain.Instance.LODTarget != null)
		{
			this.UpdateLOD(BlodMain.Instance.LODTarget.position);
		}
	}

	public void UpdateLODRough(Vector3 camPos)
	{
		if (--this.skipFrame > 0)
		{
			return;
		}
		this.skipFrame = BlodLOD.skipFrameLimit;
		this.UpdateLOD(camPos);
	}

	public void UpdateLOD(Vector3 camPos)
	{
		float num = Vector3.Distance(new Vector3(base.transform.position.x, camPos.y, base.transform.position.z), camPos);
		int i;
		for (i = this.lodItems.Count - 1; i >= 0; i--)
		{
			if (num < this.lodItems[i].distance)
			{
				break;
			}
		}
		if (i == this._curIdxLOD)
		{
			this._curIdxLOD = i;
		}
		else if (i == -1)
		{
			this.CullOut();
		}
		else
		{
			this.SwitchLOD(i, this._curIdxLOD == -1);
		}
	}

	private void SwitchLOD(int idxLOD, bool isCullIn)
	{
		for (int i = 0; i < this.lodItems.Count; i++)
		{
			GameObject gameObject = this.lodItems[i].target.gameObject;
			if (isCullIn)
			{
				gameObject.SetActive(true);
			}
			if (i == idxLOD)
			{
				gameObject.renderer.enabled = true;
			}
			else
			{
				gameObject.renderer.enabled = false;
			}
		}
		this._curIdxLOD = idxLOD;
	}

	private void CullOut()
	{
		this._curIdxLOD = -1;
		foreach (BlodLOD.Item current in this.lodItems)
		{
			current.target.gameObject.renderer.enabled = false;
			current.target.gameObject.SetActive(false);
		}
	}
}
