using System;
using UnityEngine;

public class CutScene_Camera
{
	public string CameraName = string.Empty;

	public int _fov;

	public float _fDuration;

	public float _fFireTime;

	public Vector3 _StartPosition = Vector3.zero;

	public Quaternion _StartRotation = Quaternion.identity;

	public Vector3 _updatePosition = Vector3.zero;

	public Quaternion _updateRotation = Quaternion.identity;

	public AnimationCurve[] _curvePosition = new AnimationCurve[3];

	public AnimationCurve[] _curveRotation = new AnimationCurve[4];

	public float Duration
	{
		get
		{
			return this._fDuration;
		}
		set
		{
			this._fDuration = value;
		}
	}

	public bool Update(float time)
	{
		if (time < this._fFireTime)
		{
			return false;
		}
		this.GetPosition(ref this._updatePosition, time);
		this.GetRotation(ref this._updateRotation, time);
		Camera.main.transform.localPosition = this._updatePosition;
		Camera.main.transform.localRotation = this._updateRotation;
		if (Camera.main.fieldOfView != (float)this._fov)
		{
			Camera.main.fieldOfView = (float)this._fov;
		}
		return true;
	}

	public void GetPosition(ref Vector3 position, float time)
	{
		position.x = this._curvePosition[0].Evaluate(time);
		position.y = this._curvePosition[1].Evaluate(time);
		position.z = this._curvePosition[2].Evaluate(time);
	}

	public void GetRotation(ref Quaternion rotation, float time)
	{
		rotation.x = this._curveRotation[0].Evaluate(time);
		rotation.y = this._curveRotation[1].Evaluate(time);
		rotation.z = this._curveRotation[2].Evaluate(time);
		rotation.w = this._curveRotation[3].Evaluate(time);
	}
}
