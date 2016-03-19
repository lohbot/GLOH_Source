using System;
using UnityEngine;

public class BlodBillboard : MonoBehaviour
{
	public bool lockUpAxis = true;

	public static int skipFrameLimit = 13;

	private int skipFrame;

	private void Start()
	{
		this.skipFrame = UnityEngine.Random.Range(0, BlodBillboard.skipFrameLimit);
	}

	private void Update()
	{
		Transform bilbCamera = BlodMain.Instance.BilbCamera;
		if (!base.renderer.enabled || BlodMain.Instance == null || bilbCamera == null)
		{
			return;
		}
		if (--this.skipFrame > 0)
		{
			return;
		}
		this.skipFrame = BlodBillboard.skipFrameLimit;
		if (this.lockUpAxis)
		{
			base.transform.LookAt(new Vector3(bilbCamera.position.x, base.transform.position.y, bilbCamera.position.z), Vector3.up);
		}
		else
		{
			base.transform.LookAt(bilbCamera.position, Vector3.up);
		}
	}
}
