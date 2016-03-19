using System;
using UnityEngine;

public class FX_ETC_CameraScreen : MonoBehaviour
{
	private Transform cameraTrasform;

	[SerializeField]
	private float Screen_distace;

	[SerializeField]
	private Camera targetCamera;

	[SerializeField]
	private bool usebilboard = true;

	private void Start()
	{
		if (this.targetCamera == null)
		{
			GameObject gameObject = TsSceneSwitcher.Instance._GetBundle_RootSceneGO(TsSceneSwitcher.Instance.CurrentSceneType);
			if (gameObject == null)
			{
				return;
			}
			Transform child = NkUtil.GetChild(gameObject.transform, "Main Camera");
			this.targetCamera = child.GetComponent<Camera>();
		}
		if (this.targetCamera == null)
		{
			return;
		}
		this.cameraTrasform = this.targetCamera.transform;
	}

	private void FixedUpdate()
	{
		if (this.cameraTrasform == null)
		{
			return;
		}
		Vector3 eulerAngles = this.cameraTrasform.TransformDirection(Vector3.forward);
		Vector3 normalized = eulerAngles.normalized;
		base.transform.position = new Vector3(this.cameraTrasform.position.x + normalized.x * this.Screen_distace, this.cameraTrasform.position.y + normalized.y * this.Screen_distace, this.cameraTrasform.position.z + normalized.z * this.Screen_distace);
		if (this.usebilboard)
		{
			base.transform.Rotate(eulerAngles);
			base.transform.LookAt(this.cameraTrasform);
		}
	}
}
