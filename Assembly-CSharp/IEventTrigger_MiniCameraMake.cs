using System;
using UnityEngine;

public interface IEventTrigger_MiniCameraMake
{
	void SetMiniCameraInfo(Vector3 Position, Vector3 Angles, float FieldOfView);

	void SetPosition(Vector3 Position);

	void SetAngles(Vector3 Angles);

	void SetFieldOfView(float FieldOfView);

	Vector3 GetPosition();

	Vector3 GetAngles();

	float GetFieldOfView();

	string GetCameraName();
}
