using System;
using UnityEngine;

public class Behavior_MiniDramaStart : EventTriggerItem_Behavior, IEventTrigger_ActorMake, IEventTrigger_MiniCameraMake
{
	public bool m_GeneralHide;

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

	private EventTriggerMiniCamera _Camera;

	private iTween.EaseType EaseType = iTween.EaseType.linear;

	public override void Init()
	{
		Behavior component = base.gameObject.GetComponent<Behavior>();
		if (component != null)
		{
			NrTSingleton<EventTriggerMiniDrama>.Instance.StartMiniDrama();
			if (this.GetPosition() != Vector3.zero || this.GetAngles() != Vector3.zero || this.m_fieldOfView >= 0f)
			{
				this.EaseType = (iTween.EaseType)((int)Enum.Parse(typeof(iTween.EaseType), this.m_EaseType));
				NrTSingleton<EventTriggerMiniDrama>.Instance.CameraMove(this.GetPosition(), this.m_PosotionMoveTime, this.GetAngles(), this.m_AngleMoveTime, this.m_fieldOfView, this.m_FOVMoveTime, this.m_ActionTime, this.EaseType);
			}
		}
		this.MakeActor(null, 0f, 0f, 0f, this.m_GeneralHide);
	}

	public override bool Excute()
	{
		this.m_Excute = true;
		return NrTSingleton<EventTriggerMiniDrama>.Instance.IsCameraAction() || !NrTSingleton<EventTriggerMiniDrama>.Instance.IsMakeActor(EventTriggerMiniDrama.ActorManager.GeneralName);
	}

	public override bool IsPopNext()
	{
		return true;
	}

	public override string GetComment()
	{
		return string.Format("미니 연출을 시작한다. (카메라 X:{0} Y:{1} Z:{2})", this.m_PositionX, this.m_PositionY, this.m_PositionZ);
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
		return Behavior._BEHAVIORTYPE.DRAMA;
	}

	public override bool IsVaildValue()
	{
		return true;
	}

	public void SetMakeActorCode(string ActorCode)
	{
	}

	public string[] GetMakeActorName()
	{
		return new string[]
		{
			EventTriggerMiniDrama.ActorManager.GeneralName
		};
	}

	public void MakeActor(string ActorName, float x, float y, float Angle, bool Hide)
	{
		NrTSingleton<EventTriggerMiniDrama>.Instance.AddGeneral(EventTriggerMiniDrama.ActorManager.GeneralName, Hide);
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
		return "Main Camera";
	}
}
