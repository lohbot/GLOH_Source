using System;
using UnityEngine;

public class TsPositionFollower : MonoBehaviour
{
	[SerializeField]
	private float _offsetY = 1f;

	private Camera m_Camera;

	private Transform m_Target;

	public void SetPositionFollower(Transform Target, Camera TargetCamera)
	{
		this.m_Target = Target;
		this.m_Camera = TargetCamera;
	}

	public void OnChagnedMovePos(Vector3 changedPos, Transform cameraTransForm)
	{
		if (!base.gameObject)
		{
			return;
		}
		changedPos.y += this._offsetY;
		base.transform.position = changedPos;
		base.transform.rotation = cameraTransForm.rotation;
	}

	public void OnDrawGizmos()
	{
		if (Camera.main == null)
		{
			return;
		}
		Color color = Gizmos.color;
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(base.transform.position, new Vector3(1f, 1f, 1f));
		Gizmos.color = color;
	}

	private void Update()
	{
		if (this.m_Camera == null)
		{
			return;
		}
		if (this.m_Target == null)
		{
			return;
		}
		this.OnChagnedMovePos(this.m_Target.transform.position, this.m_Camera.transform);
	}
}
