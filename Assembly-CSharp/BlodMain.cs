using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class BlodMain : MonoBehaviour
{
	public int skipFrameBillb = 5;

	public int skipFrameLOD = 3;

	private BlodLOD[] LODCounter;

	[NonSerialized]
	public Transform LODTarget;

	[NonSerialized]
	public Transform BilbCamera;

	private Vector3 LodTagertOldPos = Vector3.zero;

	public static BlodMain Instance
	{
		get;
		private set;
	}

	public int CountLOD
	{
		get
		{
			return (this.LODCounter == null) ? 0 : this.LODCounter.Length;
		}
	}

	private void Awake()
	{
		BlodMain.Instance = this;
	}

	private void Start()
	{
		BlodMain.Instance = this;
		BlodBillboard.skipFrameLimit = this.skipFrameBillb;
		BlodLOD.skipFrameLimit = this.skipFrameLOD;
		base.StartCoroutine("RoughUpdate");
		if (Application.isPlaying)
		{
			Transform transform = null;
			GameObject gameObject = GameObject.FindWithTag("Player");
			if (gameObject)
			{
				transform = gameObject.transform;
			}
			if (transform == null && Camera.main)
			{
				transform = Camera.main.transform;
			}
			if (transform != null)
			{
				BlodLOD[] lODCounter = this.LODCounter;
				for (int i = 0; i < lODCounter.Length; i++)
				{
					BlodLOD blodLOD = lODCounter[i];
					blodLOD.UpdateLOD(transform.position);
				}
			}
		}
	}

	private void OnDestroy()
	{
		this.LODCounter = null;
		base.StopAllCoroutines();
	}

	public void CollectLOD()
	{
		this.LODCounter = (UnityEngine.Object.FindObjectsOfType(typeof(BlodLOD)) as BlodLOD[]);
		UnityEngine.Debug.Log("LODCounter : " + this.LODCounter.Length);
	}

	[DebuggerHidden]
	private IEnumerator RoughUpdate()
	{
		BlodMain.<RoughUpdate>c__Iterator6B <RoughUpdate>c__Iterator6B = new BlodMain.<RoughUpdate>c__Iterator6B();
		<RoughUpdate>c__Iterator6B.<>f__this = this;
		return <RoughUpdate>c__Iterator6B;
	}
}
