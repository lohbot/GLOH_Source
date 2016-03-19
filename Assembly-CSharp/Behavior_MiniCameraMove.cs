using System;
using UnityEngine;

public class Behavior_MiniCameraMove : EventTriggerItem_Behavior, IEventTrigger_MiniCameraMake
{
	public float m_PositionX;

	public float m_PositionY;

	public float m_PositionZ;

	public float m_PosotionMoveTime = -1f;

	public float m_AngleX;

	public float m_AngleY;

	public float m_AngleZ;

	public float m_AngleMoveTime = -1f;

	public float m_fieldOfView = -1f;

	public float m_FOVMoveTime = -1f;

	public string m_EaseType = string.Empty;

	public float m_ActionTime;

	private iTween.EaseType EaseType = iTween.EaseType.linear;

	[HideInInspector]
	private EventTriggerMiniCamera _Camera;

	private float _StartTime;

	public override void Init()
	{
		this.EaseType = (iTween.EaseType)((int)Enum.Parse(typeof(iTween.EaseType), this.m_EaseType));
		NrTSingleton<EventTriggerMiniDrama>.Instance.CameraMove(this.GetPosition(), this.m_PosotionMoveTime, this.GetAngles(), this.m_AngleMoveTime, this.m_fieldOfView, this.m_FOVMoveTime, this.m_ActionTime, this.EaseType);
		this._StartTime = Time.time;
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return Math.Abs(this._StartTime - Time.time) < this.m_ActionTime;
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("카메라를 X:{0}Y:{1}Z:{2}으로 {3}초 동안 움직니다.", new object[]
		{
			this.m_PositionX,
			this.m_PositionY,
			this.m_PositionZ,
			this.m_ActionTime
		});
	}

	public override float ExcuteTiemSecond()
	{
		return 0f;
	}

	public override void Draw()
	{
	}

	public override Behavior._BEHAVIORTYPE GetBehaviorType()
	{
		return Behavior._BEHAVIORTYPE.CAMERA;
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public EventTriggerMiniCamera GetCamera()
	{
		return this._Camera;
	}

	public void SetMiniCameraInfo(Vector3 Position, Vector3 Angles, float FieldOfView)
	{
		this.SetPosition(Position);
		this.SetAngles(Angles);
		this.SetFieldOfView(FieldOfView);
	}

	public void SetPosition(Vector3 Position)
	{
		this.m_PositionX = Position.x;
		this.m_PositionY = Position.y;
		this.m_PositionZ = Position.z;
	}

	public Vector3 GetPosition()
	{
		return new Vector3(this.m_PositionX, this.m_PositionY, this.m_PositionZ);
	}

	public void SetAngles(Vector3 Angles)
	{
		this.m_AngleX = Angles.x;
		this.m_AngleY = Angles.y;
		this.m_AngleZ = Angles.z;
	}

	public Vector3 GetAngles()
	{
		return new Vector3(this.m_AngleX, this.m_AngleY, this.m_AngleZ);
	}

	public void SetFieldOfView(float FieldOfView)
	{
		this.m_fieldOfView = FieldOfView;
	}

	public float GetFieldOfView()
	{
		return this.m_fieldOfView;
	}

	public string GetCameraName()
	{
		return base.name;
	}

	public void OnDrawGizmos()
	{
		if (this._Camera != null)
		{
			Color color = Gizmos.color;
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(this._Camera.transform.position, new Vector3(1f, 1f, 1f));
			Gizmos.color = color;
		}
	}
}
