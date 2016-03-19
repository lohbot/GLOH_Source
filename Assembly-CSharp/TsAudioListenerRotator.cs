using System;
using UnityEngine;

public class TsAudioListenerRotator : MonoBehaviour
{
	private GameObject _targetGO;

	[SerializeField]
	private bool _showCubeForDebug;

	private GameObject _debugCube;

	public void ChangeTartgetGO(GameObject targetGO)
	{
		this._targetGO = targetGO;
	}

	public void Update()
	{
		if (!this._targetGO)
		{
			if (!Camera.main)
			{
				return;
			}
			this._targetGO = Camera.main.gameObject;
			if (!this._targetGO)
			{
				return;
			}
		}
		Vector3 forward = this._targetGO.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		base.transform.rotation = Quaternion.LookRotation(forward);
		if (this._showCubeForDebug)
		{
			if (this._debugCube == null)
			{
				this._debugCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				this._debugCube.transform.position = base.transform.position;
				this._debugCube.transform.rotation = base.transform.rotation;
				this._debugCube.transform.parent = base.transform;
				this._debugCube.collider.enabled = false;
			}
		}
		else if (this._debugCube != null)
		{
			UnityEngine.Object.DestroyImmediate(this._debugCube);
		}
	}
}
