using GAME;
using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using System.Collections.Generic;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NrCharMove
{
	private const short MovableDestRange = 10;

	private NrCharBase m_pkChar;

	private NrCharBase m_pkTargetChar;

	private int m_nFollowPersonID;

	private Vector3 m_vFollowCharLastPos = Vector3.zero;

	private Vector3 m_vCharPos = Vector3.zero;

	private Vector3 m_vLastCharPos = Vector3.zero;

	private Vector3 m_vDirection = Vector3.zero;

	private Vector3 m_vTargetPos = Vector3.zero;

	private Vector3 m_vFastMoveNextTargetPos = Vector3.zero;

	private GameObject m_objMoveMark;

	private bool m_bHideMoveMark;

	private bool m_bArrived;

	private bool m_bFastMove;

	private bool m_bJoyStickMove;

	private bool m_bKeyboardMove;

	private bool m_bMouseMove;

	private float m_fCharSpeed = 10f;

	private float m_fIncreaseCharSpeed;

	private float m_fMinSpeed;

	private bool m_bMoveDepartTime;

	private bool m_bMoveArriveTime;

	private float m_fMoveDuringTime;

	private float m_fKeyMovePacketSendTimer;

	private Vector2 m_vCheckFrom = Vector2.zero;

	private Vector2 m_vCheckTo = Vector2.zero;

	private LinkedList<Vector3> m_AStarPath;

	public List<Vector3> m_NavPath = new List<Vector3>();

	private bool m_bLastMovePosCheck;

	public bool HideMoveMark
	{
		set
		{
			this.m_bHideMoveMark = value;
		}
	}

	public NrCharMove(NrCharBase pkChar)
	{
		this.m_pkChar = pkChar;
		this.m_AStarPath = new LinkedList<Vector3>();
		this.m_objMoveMark = null;
		this.Init();
	}

	public void Init()
	{
		this.m_pkTargetChar = null;
		this.m_vCharPos = new Vector3(0f, 0f, 0f);
		this.m_vLastCharPos = new Vector3(0f, 0f, 0f);
		this.m_vDirection = new Vector3(0f, 0f, 0f);
		this.m_vTargetPos = new Vector3(0f, 0f, 0f);
		this.m_vFastMoveNextTargetPos = new Vector3(0f, 0f, 0f);
		this.ClearPath();
		this.m_bArrived = true;
		this.m_bJoyStickMove = false;
		this.m_bKeyboardMove = false;
		this.m_bMouseMove = false;
		this.m_fCharSpeed = 10f;
		this.m_fIncreaseCharSpeed = 0f;
		this.m_fMinSpeed = 0f;
		this.m_bMoveDepartTime = false;
		this.m_bMoveArriveTime = false;
		this.m_fKeyMovePacketSendTimer = 0f;
		this.DestroyMoveMark();
	}

	private float CheckDistance(Vector3 vFrom, Vector3 vTo)
	{
		this.m_vCheckFrom.x = vFrom.x;
		this.m_vCheckFrom.y = vFrom.z;
		this.m_vCheckTo.x = vTo.x;
		this.m_vCheckTo.y = vTo.z;
		return Vector2.Distance(this.m_vCheckFrom, this.m_vCheckTo);
	}

	public void SetCharPos(GameObject pkChar)
	{
		this.m_vCharPos = pkChar.transform.position;
		this.m_vDirection = pkChar.transform.forward;
		this.m_pkChar.GetPersonInfo().SetCharPos(this.m_vCharPos);
		if (this.m_vCharPos.x <= 1f || this.m_vCharPos.z <= 1f)
		{
			Debug.LogWarning("!!! CharMove SetTargetPos Problem ==> " + this.m_vCharPos.ToString());
		}
	}

	public void SetCharPos(Vector3 pos, Vector3 dir)
	{
		this.m_vCharPos = pos;
		this.m_vDirection = dir;
		this.m_pkChar.GetPersonInfo().SetCharPos(this.m_vCharPos);
		if (this.m_vCharPos.x <= 1f || this.m_vCharPos.z <= 1f)
		{
			Debug.LogWarning("!!! CharMove SetTargetPos Problem ==> " + this.m_vCharPos.ToString());
		}
	}

	public Vector3 GetCharPos()
	{
		return this.m_vCharPos;
	}

	public void SetTargetPos(float x, float y, float z)
	{
		this.m_vTargetPos.x = x;
		this.m_vTargetPos.y = y;
		this.m_vTargetPos.z = z;
		if (x <= 1f || z <= 1f)
		{
			Debug.LogWarning("!!! CharMove SetTargetPos Problem ==> " + this.m_vTargetPos.ToString());
		}
	}

	public Vector3 GetTargetPos()
	{
		return this.m_vTargetPos;
	}

	public Vector3 GetFastMoveNextTargetPos()
	{
		return this.m_vFastMoveNextTargetPos;
	}

	public bool IsKeyboardOrMouseMove()
	{
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			return this.IsJoystickMove();
		}
		return this.m_bKeyboardMove || this.m_bMouseMove;
	}

	public bool IsJoystickMove()
	{
		return this.m_bJoyStickMove;
	}

	public void SetTargetChar(NrCharBase pkChar)
	{
		this.m_pkTargetChar = pkChar;
	}

	public NrCharBase GetTargetChar()
	{
		return this.m_pkTargetChar;
	}

	public bool IsFastMoving()
	{
		return this.m_bFastMove;
	}

	public bool IsMoving()
	{
		return !this.m_bArrived;
	}

	public void SetIncreaseSpeed(float fSpeed)
	{
		this.m_fIncreaseCharSpeed = fSpeed;
		this.SetSpeed(this.m_fCharSpeed);
	}

	public void SetSpeed(float fSpeed)
	{
		this.m_fCharSpeed = fSpeed;
		this.m_fMinSpeed = 1f - fSpeed;
		fSpeed += this.m_fIncreaseCharSpeed;
		if (this.m_pkChar.m_k3DChar != null)
		{
			this.m_pkChar.m_k3DChar.SetSpeed(fSpeed);
		}
	}

	public float GetSpeed()
	{
		return this.m_fCharSpeed;
	}

	private float GetCurrentSpeed()
	{
		return this.GetSpeed() + this.m_fIncreaseCharSpeed;
	}

	public bool IsAutoMove()
	{
		return NrTSingleton<NrAutoPath>.Instance.IsAutoMoving();
	}

	public void ClearPath()
	{
		this.m_AStarPath.Clear();
		this.m_NavPath.Clear();
	}

	public void AddPath(Vector3 pos)
	{
		this.m_AStarPath.AddLast(pos);
		this.m_NavPath.Add(pos);
		this.m_bArrived = false;
	}

	public bool MoveTo(float x, float y, float z)
	{
		this.SetTargetPos(x, y, z);
		Nr3DCharBase nr3DCharBase = this.m_pkChar.Get3DChar();
		if (nr3DCharBase == null)
		{
			return false;
		}
		if (!nr3DCharBase.IsCreated() || !nr3DCharBase.Is3DCharActive())
		{
			return false;
		}
		NrCharDefine.eMoveTargetReason eMoveTargetReason = this.IsMovableArea(x, z);
		if (eMoveTargetReason != NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
		{
			Vector3 lhs = Vector3.zero;
			lhs = this.FindMovableDestination(this.m_vTargetPos, eMoveTargetReason);
			if (lhs == Vector3.zero)
			{
				this.SetTargetPos(0f, 0f, 0f);
				return false;
			}
			this.SetTargetPos(lhs.x, lhs.y, lhs.z);
		}
		if (this.m_vCharPos.x == this.m_vTargetPos.x && this.m_vCharPos.z == this.m_vTargetPos.z)
		{
			return true;
		}
		this.MoveStart();
		return true;
	}

	public bool StraightMoveTo(float x, float y, float z)
	{
		this.SetTargetPos(x, y, z);
		NrCharDefine.eMoveTargetReason eMoveTargetReason = this.IsMovableArea(x, z);
		if (eMoveTargetReason != NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
		{
			Vector3 lhs = Vector3.zero;
			lhs = this.FindMovableDestination(this.m_vTargetPos, eMoveTargetReason);
			if (lhs == Vector3.zero)
			{
				this.SetTargetPos(0f, 0f, 0f);
				return false;
			}
			this.SetTargetPos(lhs.x, lhs.y, lhs.z);
		}
		if (this.m_vCharPos.x == this.m_vTargetPos.x && this.m_vCharPos.z == this.m_vTargetPos.z)
		{
			return true;
		}
		this.MoveStart();
		return true;
	}

	public bool MoveToFast(float x, float y, float z, float next_x, float next_y, float next_z)
	{
		this.SetTargetPos(x, y, z);
		NrCharDefine.eMoveTargetReason eMoveTargetReason = this.IsMovableArea(x, z);
		if (eMoveTargetReason != NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
		{
			Vector3 lhs = Vector3.zero;
			lhs = this.FindMovableDestination(this.m_vTargetPos, eMoveTargetReason);
			if (lhs == Vector3.zero)
			{
				this.SetTargetPos(0f, 0f, 0f);
				return false;
			}
			this.SetTargetPos(lhs.x, lhs.y, lhs.z);
		}
		if (this.m_vCharPos.x == this.m_vTargetPos.x && this.m_vCharPos.z == this.m_vTargetPos.z)
		{
			return true;
		}
		this.m_vFastMoveNextTargetPos.x = next_x;
		this.m_vFastMoveNextTargetPos.y = next_y;
		this.m_vFastMoveNextTargetPos.z = next_z;
		NrCharDefine.eMoveTargetReason eMoveTargetReason2 = this.IsMovableArea(next_x, next_z);
		if (eMoveTargetReason2 != NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
		{
			Vector3 lhs2 = Vector3.zero;
			lhs2 = this.FindMovableDestination(this.m_vFastMoveNextTargetPos, eMoveTargetReason2);
			if (lhs2 == Vector3.zero)
			{
				this.m_vFastMoveNextTargetPos.x = 0f;
				this.m_vFastMoveNextTargetPos.y = 0f;
				this.m_vFastMoveNextTargetPos.z = 0f;
				return false;
			}
			this.m_vFastMoveNextTargetPos.x = lhs2.x;
			this.m_vFastMoveNextTargetPos.y = lhs2.y;
			this.m_vFastMoveNextTargetPos.z = lhs2.z;
		}
		this.m_pkChar.SetIncreaseSpeed(50f);
		this.m_bFastMove = true;
		this.MoveStart();
		return true;
	}

	public bool AutoMoveTo(int DestMapIndex, short DestX, short DestY)
	{
		return this.AutoMoveTo(DestMapIndex, DestX, DestY, false);
	}

	public bool AutoMoveTo(int DestMapIndex, short DestX, short DestY, bool bFollowChar)
	{
		if (this.m_pkChar == null || this.m_pkChar.GetID() != 1)
		{
			return false;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return false;
		}
		if (!bFollowChar && this.m_pkChar != null && this.m_pkChar.IsCharKindATB(1L))
		{
			NrCharUser nrCharUser = (NrCharUser)this.m_pkChar;
			if (nrCharUser.GetFollowCharPersonID() > 0L)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
				return false;
			}
		}
		DestMapIndex = NrTSingleton<MapManager>.Instance.GetAutoMoveDestMapIndex(DestMapIndex);
		if (NrTSingleton<NrAutoPath>.Instance.Generate(DestMapIndex, DestX, DestY, bFollowChar) == 0)
		{
			return false;
		}
		Vector3 lhs = NrTSingleton<NrAutoPath>.Instance.PopMovePath();
		if (lhs != Vector3.zero)
		{
			this.MoveTo(lhs.x, lhs.y, lhs.z);
		}
		return true;
	}

	public Vector3 FindFirstPath(int DestMapIndex, short DestX, short DestY, bool bFollowChar)
	{
		Vector3 zero = Vector3.zero;
		if (this.m_pkChar == null || this.m_pkChar.GetID() != 1)
		{
			return zero;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return zero;
		}
		if (!bFollowChar && this.m_pkChar != null && this.m_pkChar.IsCharKindATB(1L))
		{
			NrCharUser nrCharUser = (NrCharUser)this.m_pkChar;
			if (nrCharUser.GetFollowCharPersonID() > 0L)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
				return zero;
			}
		}
		DestMapIndex = NrTSingleton<MapManager>.Instance.GetAutoMoveDestMapIndex(DestMapIndex);
		if (NrTSingleton<NrAutoPath>.Instance.Generate(DestMapIndex, DestX, DestY, bFollowChar) == 0)
		{
			return zero;
		}
		if (NrTSingleton<NrAutoPath>.Instance.MovePathCount() >= 2)
		{
			NrTSingleton<NrAutoPath>.Instance.PopMovePath();
			return NrTSingleton<NrAutoPath>.Instance.PopMovePath();
		}
		return NrTSingleton<NrAutoPath>.Instance.PopMovePath();
	}

	public void MoveToByJoystick(RaycastHit _rayCastHit)
	{
		RaycastHit raycastHit = _rayCastHit;
		Vector3 point = raycastHit.point;
		if (this.MoveTo(point.x, point.y, point.z))
		{
			this.m_bJoyStickMove = true;
		}
	}

	private void MoveStart()
	{
		this.SetIncreaseMove();
		this.ClearPath();
		this.m_bArrived = false;
		this.AddPath(this.m_vTargetPos);
		this.MakeMoveMark(this.m_vTargetPos);
		this.ProcessCharMove(true);
	}

	public void MoveStop(bool bSetAni, bool bOptionMove)
	{
		if (this.m_pkChar == null)
		{
			return;
		}
		if (this.m_pkChar.m_k3DChar == null)
		{
			return;
		}
		if (this.m_pkChar.m_k3DChar.GetRootGameObject() == null)
		{
			return;
		}
		if (this.m_pkChar.IsReadyCharAction())
		{
			this.SetCharPos(this.m_pkChar.m_k3DChar.GetRootGameObject());
			this.m_pkChar.m_k3DChar.MoveStop(bSetAni);
		}
		this.ClearPath();
		this.m_bArrived = true;
		this.m_bKeyboardMove = false;
		this.m_bMouseMove = false;
		this.m_bJoyStickMove = false;
		this.m_bFastMove = false;
		this.SetIncreaseSpeed(0f);
		this.m_bMoveDepartTime = false;
		this.DestroyMoveMark();
		this.SetTargetChar(null);
		if (bOptionMove)
		{
			this.SetDecreaseMove();
		}
		this.m_pkChar.SyncBillboardRotate(false);
		if (this.m_pkChar.GetID() == 1)
		{
			if (this.IsAutoMove() && NrTSingleton<NkClientLogic>.Instance.GetWarpGateIndex() == 0)
			{
				NrTSingleton<NrAutoPath>.Instance.ResetData();
			}
			if (NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState() || NrTSingleton<NkClientLogic>.Instance.IsMovable())
			{
				this.SendCharMovePacketForKeyBoardMove(true);
			}
		}
	}

	public void SetIncreaseMove()
	{
		if (!this.IsAutoMove() && !this.m_bFastMove && this.m_pkChar.GetCharKindType() == eCharKindType.CKT_USER && !this.IsMoving())
		{
			this.SetIncreaseSpeed(this.m_fMinSpeed);
			this.m_bMoveDepartTime = true;
			this.m_fMoveDuringTime = Time.time;
		}
	}

	private void ProcessIncreaseMove()
	{
		if (this.m_bMoveDepartTime)
		{
			if (this.m_fIncreaseCharSpeed < 0f)
			{
				float currentSpeed = this.GetCurrentSpeed();
				float speed = this.GetSpeed();
				float num = Mathf.Lerp(currentSpeed / 6f, speed, Time.deltaTime);
				float increaseSpeed = Mathf.Min(this.m_fIncreaseCharSpeed + num, 0f);
				this.SetIncreaseSpeed(increaseSpeed);
			}
			else
			{
				this.SetIncreaseSpeed(0f);
				this.m_bMoveDepartTime = false;
			}
		}
		else if (this.IsAutoMove() && this.m_fIncreaseCharSpeed < 0f)
		{
			this.SetIncreaseSpeed(0f);
		}
	}

	private void SetDecreaseMove()
	{
		if (!this.IsAutoMove() && !this.m_bFastMove && this.m_pkChar.GetCharKindType() == eCharKindType.CKT_USER)
		{
			if (this.m_fMoveDuringTime != 0f && Time.time - this.m_fMoveDuringTime >= 1f)
			{
				GameObject rootGameObject = this.m_pkChar.m_k3DChar.GetRootGameObject();
				if (rootGameObject != null)
				{
					Vector3 vector = rootGameObject.transform.TransformDirection(Vector3.forward);
					vector.y = 0f;
					Vector3 vector2 = this.m_vCharPos + vector.normalized * 1f;
					this.m_pkChar.MoveTo(vector2.x, vector2.y, vector2.z, true);
					this.m_bMoveArriveTime = true;
				}
			}
			this.m_fMoveDuringTime = 0f;
		}
	}

	private void ProcessDecreaseMove()
	{
		if (this.m_bMoveArriveTime)
		{
			if (this.m_fIncreaseCharSpeed > this.m_fMinSpeed)
			{
				float currentSpeed = this.GetCurrentSpeed();
				float speed = this.GetSpeed();
				float num = (speed - Mathf.Lerp(currentSpeed - 1f, speed, Time.deltaTime)) / 2f;
				float increaseSpeed = Mathf.Max(this.m_fIncreaseCharSpeed - num, this.m_fMinSpeed);
				if (this.m_fIncreaseCharSpeed - this.m_fMinSpeed <= 1f)
				{
					this.m_fIncreaseCharSpeed = this.m_fMinSpeed;
				}
				this.SetIncreaseSpeed(increaseSpeed);
			}
			else
			{
				this.SetIncreaseSpeed(0f);
				this.m_bMoveArriveTime = false;
			}
		}
	}

	public bool SetCurrentPosInfo()
	{
		if (this.m_pkChar == null)
		{
			return false;
		}
		if (this.m_pkChar.m_k3DChar == null)
		{
			return false;
		}
		if (!this.m_bKeyboardMove && !this.m_bMouseMove && !this.IsJoystickMove())
		{
			if (this.m_bArrived)
			{
				this.ProcessDecreaseMove();
				return false;
			}
			bool flag = false;
			if (this.m_pkChar.GetID() == 1 && this.IsAutoMove())
			{
				this.SendCharMovePacketForKeyBoardMove(false);
				flag = true;
			}
			if (!this.m_pkChar.m_k3DChar.IsMoveToTarget() && this.m_AStarPath.Count == 0)
			{
				this.SetTargetPos(this.m_vTargetPos.x, this.m_vTargetPos.y, this.m_vTargetPos.z);
				this.ClearPath();
				this.m_bArrived = true;
				this.m_fKeyMovePacketSendTimer = 0f;
				this.DestroyMoveMark();
				if (this.m_bFastMove)
				{
					this.m_bFastMove = false;
					this.m_pkChar.SetIncreaseSpeed(0f);
					this.MoveTo(this.m_vFastMoveNextTargetPos.x, this.m_vFastMoveNextTargetPos.y, this.m_vFastMoveNextTargetPos.z);
				}
				this.ProcessByTargetChar();
				if (flag)
				{
					Vector3 lhs = NrTSingleton<NrAutoPath>.Instance.PopMovePath();
					if (lhs != Vector3.zero)
					{
						this.MoveTo(lhs.x, lhs.y, lhs.z);
					}
					else if (NrTSingleton<NrAutoPath>.Instance.MapPathCount() == 0)
					{
						NrTSingleton<NrAutoPath>.Instance.ResetData();
						this.CheckAutoMoveKind();
						GS_CHAR_LASTMOVE_CHECK_REQ gS_CHAR_LASTMOVE_CHECK_REQ = new GS_CHAR_LASTMOVE_CHECK_REQ();
						gS_CHAR_LASTMOVE_CHECK_REQ.Pos.x = this.m_pkChar.m_k3DChar.GetRootGameObject().transform.position.x;
						gS_CHAR_LASTMOVE_CHECK_REQ.Pos.y = this.m_pkChar.m_k3DChar.GetRootGameObject().transform.position.y;
						gS_CHAR_LASTMOVE_CHECK_REQ.Pos.z = this.m_pkChar.m_k3DChar.GetRootGameObject().transform.position.z;
						gS_CHAR_LASTMOVE_CHECK_REQ.Direction.x = this.m_vDirection.x;
						gS_CHAR_LASTMOVE_CHECK_REQ.Direction.y = this.m_vDirection.y;
						gS_CHAR_LASTMOVE_CHECK_REQ.Direction.z = this.m_vDirection.z;
						SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_LASTMOVE_CHECK_REQ, gS_CHAR_LASTMOVE_CHECK_REQ);
						this.m_bLastMovePosCheck = false;
					}
				}
				this.SetDecreaseMove();
			}
		}
		this.SetCharPos(this.m_pkChar.m_k3DChar.GetRootGameObject());
		return true;
	}

	public NrCharDefine.eMoveTargetReason IsMovableArea(float x, float y)
	{
		if (this.m_pkChar.IsCharKindATB(1L))
		{
			Vector2 pos = new Vector2(x, y);
			bool flag = NrTSingleton<NrNpcPosManager>.Instance.IsWideCollArea(pos);
			if (flag)
			{
				if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nIndunUnique != -1)
				{
					return NrCharDefine.eMoveTargetReason.MTR_WIDECOLL;
				}
				pos.x = this.m_vCharPos.x;
				pos.y = this.m_vCharPos.z;
				if (!NrTSingleton<NrNpcPosManager>.Instance.IsWideCollArea(pos))
				{
					return NrCharDefine.eMoveTargetReason.MTR_WIDECOLL;
				}
			}
		}
		return NrCharDefine.eMoveTargetReason.MTR_SUCCESS;
	}

	private bool FindMovableAreaByKey(ref Vector3 MoveDirection)
	{
		float x = this.m_vCharPos.x;
		float z = this.m_vCharPos.z;
		Vector3 a = MoveDirection;
		Vector3 a2 = -Vector3.Cross(MoveDirection, Vector3.up);
		int num = 1;
		Vector3 a3 = Vector3.zero;
		Vector3 a4 = Vector3.zero;
		Vector3 b = Vector3.zero;
		Vector3 b2 = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)(i + 1);
			a3 = a2 * num2;
			a4 = a2 * -num2;
			b = a * num2;
			b2 = a * -num2;
			Vector3 vector = a3 + b;
			if (this.IsMovableArea(x + vector.x, z + vector.z) == NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
			{
				MoveDirection.x = vector.x;
				MoveDirection.z = vector.z;
				return true;
			}
			Vector3 vector2 = a4 + b;
			if (this.IsMovableArea(x + vector2.x, z + vector2.z) == NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
			{
				MoveDirection.x = vector2.x;
				MoveDirection.z = vector2.z;
				return true;
			}
			if (this.IsMovableArea(x + a3.x, z) == NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
			{
				MoveDirection.x = a3.x;
				MoveDirection.z = a3.z;
				return true;
			}
			if (this.IsMovableArea(x + a4.x, z) == NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
			{
				MoveDirection.x = a4.x;
				MoveDirection.z = a4.z;
				return true;
			}
			Vector3 vector3 = a3 + b2;
			if (this.IsMovableArea(x + vector3.x, z + vector3.z) == NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
			{
				MoveDirection.x = vector3.x;
				MoveDirection.z = vector3.z;
				return true;
			}
			Vector3 vector4 = a4 + b2;
			if (this.IsMovableArea(x + vector4.x, z + vector4.z) == NrCharDefine.eMoveTargetReason.MTR_SUCCESS)
			{
				MoveDirection.x = vector4.x;
				MoveDirection.z = vector4.z;
				return true;
			}
		}
		return false;
	}

	public Vector2 FindMovableNearWideColl(Vector3 vDest, bool bFromUser, float fAngleRef, float fOldFactor)
	{
		Vector2 vector = new Vector2(vDest.x, vDest.z);
		Vector2 vector2 = default(Vector2);
		if (!NrTSingleton<NrNpcPosManager>.Instance.FindWideCollArea(vector, ref vector2))
		{
			return vector;
		}
		Vector2 vector3 = vector - vector2;
		this.m_vCheckFrom.x = this.m_vCharPos.x;
		this.m_vCheckFrom.y = this.m_vCharPos.z;
		this.m_vCheckTo.x = vDest.x;
		this.m_vCheckTo.y = vDest.z;
		if (bFromUser)
		{
			vector3 = this.m_vCheckFrom - vector2;
		}
		if (fAngleRef != 0f)
		{
			float num = fAngleRef * Mathf.Pow(-1f, fAngleRef + 1f);
			fOldFactor += num;
			float f = 30f * fOldFactor * 0.0174532924f;
			float x = vector3.x * Mathf.Cos(f) - vector3.y * Mathf.Sin(f);
			float y = vector3.x * Mathf.Sin(f) + vector3.y * Mathf.Cos(f);
			vector3.x = x;
			vector3.y = y;
		}
		vector3.Normalize();
		vector3 *= 2f;
		vector3 *= 1.1f;
		Vector2 result = vector2 + vector3;
		if (fAngleRef < 12f && this.IsMovableArea(result.x, result.y) == NrCharDefine.eMoveTargetReason.MTR_WIDECOLL)
		{
			result = this.FindMovableNearWideColl(vDest, bFromUser, fAngleRef + 1f, fOldFactor);
		}
		return result;
	}

	public Vector3 FindMovableDestination(Vector3 vDest, NrCharDefine.eMoveTargetReason movereason)
	{
		Vector3 zero = Vector3.zero;
		if (movereason == NrCharDefine.eMoveTargetReason.MTR_WIDECOLL)
		{
			Vector2 vector = this.FindMovableNearWideColl(vDest, true, 0f, 0f);
			if (vector.x == 0f && vector.y == 0f)
			{
				return zero;
			}
			zero.x = vector.x;
			zero.z = vector.y;
		}
		zero.y = NrCharMove.CalcHeight(zero);
		return zero;
	}

	public void MakeMoveMark(Vector3 objPos)
	{
		if (this.m_pkTargetChar != null || this.m_pkChar.GetID() > 1)
		{
			return;
		}
		if (this.m_bHideMoveMark)
		{
			this.DestroyMoveMark();
			return;
		}
		if (this.m_objMoveMark == null)
		{
			this.m_objMoveMark = new GameObject("CLICK_MOVE");
			NrTSingleton<NkEffectManager>.Instance.AddEffect("CLICK_MOVE", this.m_objMoveMark);
		}
		this.m_objMoveMark.transform.position = new Vector3(objPos.x, objPos.y, objPos.z);
		this.m_objMoveMark.transform.localScale = new Vector3(1f, 1f, 1f);
		Animation[] componentsInChildren = this.m_objMoveMark.GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Animation animation = array[i];
			animation.Rewind();
		}
		NkUtil.SetAllChildActive(this.m_objMoveMark, true);
	}

	public void DestroyMoveMark()
	{
		if (this.m_objMoveMark != null)
		{
			this.m_bHideMoveMark = false;
			NkUtil.SetAllChildActive(this.m_objMoveMark, false);
		}
	}

	public bool CheckAutoMoveKind()
	{
		QuestAutoPathInfo autoPathInfo = NrTSingleton<NkQuestManager>.Instance.GetAutoPathInfo();
		if (autoPathInfo != null)
		{
			if (!autoPathInfo.m_bComplete)
			{
				autoPathInfo.m_kQuest.AfterAutoPath();
			}
			else if (autoPathInfo.m_nCharKind > 0)
			{
				NrCharBase charByCharKind = NrTSingleton<NkCharManager>.Instance.GetCharByCharKind(autoPathInfo.m_nCharKind);
				if (charByCharKind != null)
				{
					if (!this.m_pkChar.IsCloseToTalkNPC(ref charByCharKind, 2f))
					{
						return false;
					}
					NrCharKindInfo charKindInfo = charByCharKind.GetCharKindInfo();
					if (charKindInfo != null)
					{
						NpcTalkUI_DLG npcTalkUI_DLG = (NpcTalkUI_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.NPCTALK_DLG);
						if (npcTalkUI_DLG != null)
						{
							npcTalkUI_DLG.SetNpcID(charKindInfo.GetCharKind(), charByCharKind.GetCharUnique());
							npcTalkUI_DLG.Show();
						}
						CHARKIND_NPCINFO cHARKIND_NPCINFO = charKindInfo.GetCHARKIND_NPCINFO();
						if (cHARKIND_NPCINFO != null && !string.IsNullOrEmpty(cHARKIND_NPCINFO.SOUND_GREETING))
						{
							TsAudioManager.Instance.AudioContainer.RequestAudioClip("NPC", cHARKIND_NPCINFO.SOUND_GREETING, "GREETING", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
						}
					}
					if (charByCharKind.GetCharObject() != null)
					{
						Vector3 position = charByCharKind.GetCharObject().transform.position;
						this.m_pkChar.m_k3DChar.RequestLookAt(position.x, position.z);
					}
					return true;
				}
				else
				{
					Debug.Log("null == npc");
				}
			}
			NrTSingleton<NkQuestManager>.Instance.SetAutoPathInfo(null);
		}
		return false;
	}

	public bool CheckTargetPositionChanged()
	{
		if (this.m_bJoyStickMove)
		{
			return false;
		}
		if (this.m_pkTargetChar == null)
		{
			return false;
		}
		float num = this.CheckDistance(this.m_vCharPos, this.m_pkTargetChar.GetPersonInfo().GetCharPos());
		if (num > 2.5f)
		{
			return true;
		}
		this.SetTargetChar(null);
		return false;
	}

	public void MoveToFollowChar()
	{
		if (this.m_pkChar.GetID() == 1)
		{
			NrCharUser nrCharUser = (NrCharUser)this.m_pkChar;
			if (nrCharUser.GetFollowCharPersonID() > 0L)
			{
				NrCharBase followChar = this.GetFollowChar(nrCharUser.GetFollowCharPersonID());
				if (followChar != null && followChar.GetCharObject() != null)
				{
					float num = 5f;
					float num2 = Vector3.Distance(this.m_vCharPos, followChar.GetCharObject().transform.position);
					if (num2 > num)
					{
						int nMapIndex = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.m_nMapIndex;
						this.m_vFollowCharLastPos = new Vector3(followChar.GetCharObject().transform.position.x, followChar.GetCharObject().transform.position.y, followChar.GetCharObject().transform.position.z);
						float d = num + 1f;
						Vector3 lhs = followChar.GetCharObject().transform.position + d * -followChar.GetCharObject().transform.forward;
						if (lhs != Vector3.zero)
						{
							this.m_pkChar.m_kCharMove.AutoMoveTo(nMapIndex, (short)lhs.x, (short)lhs.z, true);
							NrTSingleton<NkQuestManager>.Instance.SetAutoPathInfo(null);
						}
						return;
					}
					float num3 = this.CheckDistance(this.m_vLastCharPos, this.m_vCharPos);
					float num4 = 1f;
					if (num3 >= num4)
					{
						this.SendCharMovePacketForKeyBoardMove(true);
					}
				}
				else
				{
					nrCharUser.RefreshFollowCharPos();
				}
			}
		}
	}

	public void ProcessByTargetChar()
	{
		if (this.m_pkChar.GetID() == 1)
		{
			NrCharUser nrCharUser = (NrCharUser)this.m_pkChar;
			if (nrCharUser.GetFollowCharPersonID() > 0L)
			{
				NrCharBase followChar = this.GetFollowChar(nrCharUser.GetFollowCharPersonID());
				if (followChar != null && followChar.GetCharObject() != null && Vector3.Distance(this.m_vFollowCharLastPos, followChar.GetCharObject().transform.position) > 60f)
				{
					int nMapIndex = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.m_nMapIndex;
					this.m_vFollowCharLastPos = new Vector3(followChar.GetCharObject().transform.position.x, followChar.GetCharObject().transform.position.y, followChar.GetCharObject().transform.position.z);
					float d = 6f;
					Vector3 lhs = followChar.GetCharObject().transform.position + d * -followChar.GetCharObject().transform.forward;
					if (lhs != Vector3.zero)
					{
						this.m_pkChar.m_kCharMove.AutoMoveTo(nMapIndex, (short)lhs.x, (short)lhs.z, true);
						NrTSingleton<NkQuestManager>.Instance.SetAutoPathInfo(null);
					}
					return;
				}
			}
		}
		if (this.CheckTargetPositionChanged())
		{
			Vector3 charPos = this.m_pkTargetChar.GetPersonInfo().GetCharPos();
			this.MoveTo(charPos.x, charPos.y, charPos.z);
		}
	}

	public void ProcessCharMove(bool bStartMove)
	{
		if (this.m_bArrived)
		{
			return;
		}
		if (!this.m_pkChar.IsMovingAnimation())
		{
			return;
		}
		bool flag = false;
		if (bStartMove)
		{
			flag = true;
		}
		else if (!this.m_pkChar.m_k3DChar.IsMoveToTarget())
		{
			flag = true;
		}
		this.ProcessIncreaseMove();
		if (this.m_bKeyboardMove || this.m_bMouseMove || this.IsJoystickMove())
		{
			flag = false;
		}
		if (flag && this.m_AStarPath.Count > 0)
		{
			Vector3 value = this.m_AStarPath.First.Value;
			if (NrTSingleton<NrAutoPath>.Instance.MapPathCount() == 0 && NrTSingleton<NrAutoPath>.Instance.MovePathCount() == 0 && this.m_pkChar.GetID() == 1 && this.IsAutoMove() && !this.m_bLastMovePosCheck)
			{
				this.m_bLastMovePosCheck = true;
			}
			this.SendCharMovePacket(this.m_vCharPos, value, this.m_vDirection);
			this.m_AStarPath.RemoveFirst();
		}
	}

	public void SendCharMovePacketForKeyBoardMove(bool bForce)
	{
		if (!bForce)
		{
			if (Time.time - this.m_fKeyMovePacketSendTimer <= 0.8f)
			{
				return;
			}
			float num = this.CheckDistance(this.m_vLastCharPos, this.m_vCharPos);
			if (num <= 1f)
			{
				return;
			}
		}
		Vector3 vLastCharPos = this.m_vLastCharPos;
		Vector3 vCharPos = this.m_vCharPos;
		if (this.m_vCharPos == Vector3.zero)
		{
			Debug.Log("KEYBOARD MOVE POS WRONG : " + this.m_vLastCharPos.ToString() + " " + this.m_vCharPos.ToString());
			return;
		}
		GS_CHAR_MOVE_REQ gS_CHAR_MOVE_REQ = new GS_CHAR_MOVE_REQ();
		gS_CHAR_MOVE_REQ.PosStart.x = vLastCharPos.x;
		gS_CHAR_MOVE_REQ.PosStart.y = vLastCharPos.y;
		gS_CHAR_MOVE_REQ.PosStart.z = vLastCharPos.z;
		gS_CHAR_MOVE_REQ.PosDest.x = vCharPos.x;
		gS_CHAR_MOVE_REQ.PosDest.y = vCharPos.y;
		gS_CHAR_MOVE_REQ.PosDest.z = vCharPos.z;
		gS_CHAR_MOVE_REQ.Direction.x = this.m_vDirection.x;
		gS_CHAR_MOVE_REQ.Direction.y = this.m_vDirection.y;
		gS_CHAR_MOVE_REQ.Direction.z = this.m_vDirection.z;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_CHAR_MOVE_REQ, gS_CHAR_MOVE_REQ);
		this.m_fKeyMovePacketSendTimer = Time.time;
		this.m_vLastCharPos.x = vCharPos.x;
		this.m_vLastCharPos.y = vCharPos.y;
		this.m_vLastCharPos.z = vCharPos.z;
	}

	public void SendCharMovePacket(Vector3 vFrom, Vector3 vTo, Vector3 vDir)
	{
		if (this.m_pkChar != null && !this.m_bKeyboardMove && !this.m_bMouseMove && !this.IsJoystickMove())
		{
			this.m_pkChar.MoveTo(vTo.x, vTo.y, vTo.z, true);
		}
	}

	public void KeyboardMove()
	{
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		if (!this.m_pkChar.IsMovingAnimation())
		{
			return;
		}
		NrCharUser nrCharUser = this.m_pkChar as NrCharUser;
		if (nrCharUser == null || !nrCharUser.IsReady3DModel())
		{
			return;
		}
		Nr3DCharActor nr3DCharActor = nrCharUser.Get3DChar() as Nr3DCharActor;
		if (nr3DCharActor == null)
		{
			return;
		}
		Transform transform = Camera.main.transform;
		Vector3 a = transform.TransformDirection(Vector3.forward);
		a.y = 0f;
		a = a.normalized;
		Vector3 a2 = new Vector3(a.z, 0f, -a.x);
		float axisRaw = NkInputManager.GetAxisRaw("Vertical");
		float axisRaw2 = NkInputManager.GetAxisRaw("Horizontal");
		Vector3 vector = axisRaw2 * a2 + axisRaw * a;
		Vector3 vector2 = Vector3.zero;
		if (vector != Vector3.zero)
		{
			float num = 0.3f;
			vector2 = Vector3.RotateTowards(vector2, vector, num * 0.0174532924f * Time.deltaTime, 1f);
			if (this.IsMovableArea(this.m_vCharPos.x + vector2.x, this.m_vCharPos.z + vector2.z) != NrCharDefine.eMoveTargetReason.MTR_SUCCESS && !this.FindMovableAreaByKey(ref vector2))
			{
				return;
			}
			vector2 = vector2.normalized;
		}
		if (vector2 == Vector3.zero)
		{
			return;
		}
		this.ProcessIncreaseMove();
		nr3DCharActor.KeyboardMove(vector2, true);
		this.SetCharPos(nr3DCharActor.GetRootGameObject());
		this.SendCharMovePacketForKeyBoardMove(false);
		this.m_bArrived = false;
		this.m_bKeyboardMove = true;
		this.m_bMouseMove = false;
	}

	public void MouseMove()
	{
		if (this.m_bKeyboardMove)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		if (!this.m_pkChar.IsMovingAnimation())
		{
			return;
		}
		NrCharUser nrCharUser = this.m_pkChar as NrCharUser;
		if (nrCharUser == null || !nrCharUser.IsReady3DModel())
		{
			return;
		}
		Nr3DCharActor nr3DCharActor = nrCharUser.Get3DChar() as Nr3DCharActor;
		if (nr3DCharActor == null)
		{
			return;
		}
		GameObject rootGameObject = nr3DCharActor.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		Transform transform = Camera.main.transform;
		Vector3 vector = transform.TransformDirection(Vector3.forward);
		vector.y = 0f;
		vector = vector.normalized;
		Vector3 vector2 = vector;
		Vector3 vector3 = Vector3.zero;
		if (vector2 != Vector3.zero)
		{
			float num = 0.3f;
			vector3 = Vector3.RotateTowards(vector3, vector2, num * 0.0174532924f * Time.deltaTime, 1f);
			if (this.IsMovableArea(this.m_vCharPos.x + vector3.x, this.m_vCharPos.z + vector3.z) != NrCharDefine.eMoveTargetReason.MTR_SUCCESS && !this.FindMovableAreaByKey(ref vector3))
			{
				return;
			}
			vector3 = vector3.normalized;
		}
		if (vector3 == Vector3.zero)
		{
			return;
		}
		this.ProcessIncreaseMove();
		nr3DCharActor.KeyboardMove(vector3, true);
		this.SetCharPos(nr3DCharActor.GetRootGameObject());
		this.SendCharMovePacketForKeyBoardMove(false);
		this.m_bArrived = false;
		this.m_bKeyboardMove = false;
		this.m_bMouseMove = true;
	}

	public void SetTabMoveInfoForMobile(bool bStart, eCharAnimationType anitype)
	{
		NrCharUser nrCharUser = this.m_pkChar as NrCharUser;
		Nr3DCharActor nr3DCharActor = nrCharUser.Get3DChar() as Nr3DCharActor;
		if (nr3DCharActor == null)
		{
			return;
		}
		if (bStart)
		{
			this.ProcessIncreaseMove();
			this.SetCharPos(nr3DCharActor.GetRootGameObject());
			this.SendCharMovePacketForKeyBoardMove(false);
			this.m_bArrived = false;
			this.m_bKeyboardMove = false;
			this.m_bMouseMove = false;
			this.m_bJoyStickMove = true;
		}
		else
		{
			this.MoveStop(false, false);
		}
		nrCharUser.SetAnimation(anitype);
	}

	public void MobileTabDragMove()
	{
		if (this.m_bKeyboardMove)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		if (!this.m_pkChar.IsMovingAnimation())
		{
			return;
		}
		NrCharUser nrCharUser = this.m_pkChar as NrCharUser;
		if (nrCharUser == null || !nrCharUser.IsReady3DModel())
		{
			return;
		}
		Nr3DCharActor nr3DCharActor = nrCharUser.Get3DChar() as Nr3DCharActor;
		if (nr3DCharActor == null)
		{
			return;
		}
		GameObject rootGameObject = nr3DCharActor.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		Transform transform = Camera.main.transform;
		Vector3 vector = transform.TransformDirection(Vector3.forward);
		vector.y = 0f;
		vector = vector.normalized;
		Vector3 vector2 = vector;
		Vector3 vector3 = Vector3.zero;
		if (vector2 != Vector3.zero)
		{
			float num = 0.3f;
			vector3 = Vector3.RotateTowards(vector3, vector2, num * 0.0174532924f * Time.deltaTime, 1f);
			if (this.IsMovableArea(this.m_vCharPos.x + vector3.x, this.m_vCharPos.z + vector3.z) != NrCharDefine.eMoveTargetReason.MTR_SUCCESS && !this.FindMovableAreaByKey(ref vector3))
			{
				return;
			}
			vector3 = vector3.normalized;
		}
		if (vector3 == Vector3.zero)
		{
			return;
		}
		this.ProcessIncreaseMove();
		nr3DCharActor.KeyboardMove(vector3, true);
		this.SetCharPos(nr3DCharActor.GetRootGameObject());
		this.SendCharMovePacketForKeyBoardMove(false);
		this.m_bArrived = false;
		this.m_bKeyboardMove = false;
		this.m_bMouseMove = true;
	}

	public void MobileTabDragMoveToDirection(ref Vector3 vToDir)
	{
		if (this.m_bKeyboardMove)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		if (!this.m_pkChar.IsMovingAnimation())
		{
			return;
		}
		NrCharUser nrCharUser = this.m_pkChar as NrCharUser;
		if (nrCharUser == null || !nrCharUser.IsReady3DModel())
		{
			return;
		}
		Nr3DCharActor nr3DCharActor = nrCharUser.Get3DChar() as Nr3DCharActor;
		if (nr3DCharActor == null)
		{
			return;
		}
		GameObject rootGameObject = nr3DCharActor.GetRootGameObject();
		if (rootGameObject == null)
		{
			return;
		}
		Transform transform = Camera.main.transform;
		Vector3 a = transform.TransformDirection(Vector3.forward);
		a.y = 0f;
		a = a.normalized;
		Vector3 a2 = new Vector3(a.z, 0f, -a.x);
		vToDir = vToDir.normalized;
		float y = vToDir.y;
		float x = vToDir.x;
		Vector3 vector = x * a2 + y * a;
		Vector3 vector2 = Vector3.zero;
		if (vector != Vector3.zero)
		{
			float num = 0.3f;
			vector2 = Vector3.RotateTowards(vector2, vector, num * 0.0174532924f * Time.deltaTime, 1f);
			NrCharDefine.eMoveTargetReason eMoveTargetReason = this.IsMovableArea(this.m_vCharPos.x + vector2.x, this.m_vCharPos.z + vector2.z);
			if (eMoveTargetReason != NrCharDefine.eMoveTargetReason.MTR_SUCCESS && !this.FindMovableAreaByKey(ref vector2))
			{
				return;
			}
			vector2 = vector2.normalized;
		}
		if (vector2 == Vector3.zero)
		{
			return;
		}
		this.ProcessIncreaseMove();
		nr3DCharActor.KeyboardMove(vector2, true);
		this.SetCharPos(nr3DCharActor.GetRootGameObject());
		this.SendCharMovePacketForKeyBoardMove(false);
		this.m_bArrived = false;
		this.m_bKeyboardMove = false;
		this.m_bMouseMove = true;
	}

	public static float CalcHeight(Vector3 pos)
	{
		float result = 0f;
		pos.y = 200f;
		Ray ray = new Ray(pos, new Vector3(0f, -1f, 0f));
		if (NkRaycast.Raycast(ray))
		{
			RaycastHit hIT = NkRaycast.HIT;
			if (hIT.transform != null)
			{
				if (hIT.transform.position == Vector3.zero)
				{
					result = NrTSingleton<NrTerrain>.Instance.SampleHeight(pos);
				}
				else
				{
					result = hIT.point.y;
				}
			}
		}
		return result;
	}

	private NrCharBase GetFollowChar(long nPersonID)
	{
		if (nPersonID <= 0L)
		{
			return null;
		}
		NrCharBase nrCharBase;
		if (this.m_nFollowPersonID > 0)
		{
			nrCharBase = NrTSingleton<NkCharManager>.Instance.GetChar(this.m_nFollowPersonID);
			if (nrCharBase != null && nrCharBase.GetPersonInfo().GetPersonID() == nPersonID)
			{
				return nrCharBase;
			}
			nrCharBase = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(nPersonID);
			if (nrCharBase != null)
			{
				this.m_nFollowPersonID = nrCharBase.GetID();
			}
		}
		else
		{
			nrCharBase = NrTSingleton<NkCharManager>.Instance.GetCharByPersonID(nPersonID);
			if (nrCharBase != null)
			{
				this.m_nFollowPersonID = nrCharBase.GetID();
			}
		}
		return nrCharBase;
	}

	public void ShowPath(bool bShow)
	{
		if (bShow)
		{
			GameObject gameObject = GameObject.Find("NAVPATH_" + this.m_pkChar.GetCharUnique());
			if (gameObject == null)
			{
				gameObject = new GameObject("NAVPATH_" + this.m_pkChar.GetCharUnique());
			}
			Vector3 position = Vector3.zero;
			for (int i = 0; i < this.m_NavPath.Count; i++)
			{
				GameObject gameObject2 = GameObject.Find(string.Concat(new object[]
				{
					"Nav_Path_",
					this.m_pkChar.GetCharUnique(),
					"_",
					i.ToString()
				}));
				if (!(gameObject2 != null))
				{
					gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					gameObject2.name = string.Concat(new object[]
					{
						"Nav_Path_",
						this.m_pkChar.GetCharUnique(),
						"_",
						i.ToString()
					});
					position = this.m_NavPath[i];
					gameObject2.transform.position = position;
					gameObject2.transform.parent = gameObject.transform;
					gameObject2.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
					MeshRenderer component = gameObject2.GetComponent<MeshRenderer>();
					if (component != null)
					{
						component.material = new Material(component.sharedMaterial)
						{
							color = Color.black
						};
					}
					LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
					if (lineRenderer == null)
					{
						lineRenderer = gameObject.AddComponent<LineRenderer>();
						lineRenderer.material = new Material(Shader.Find("AT2/Effect/Particle_Standard_mobile"));
						lineRenderer.SetColors(Color.red, Color.blue);
						lineRenderer.SetWidth(0.4f, 0.4f);
						lineRenderer.SetVertexCount(this.m_NavPath.Count);
					}
					lineRenderer.SetPosition(i, position);
				}
			}
		}
		else
		{
			GameObject gameObject3 = GameObject.Find("NAVPATH_" + this.m_pkChar.GetCharUnique());
			if (gameObject3 != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject3);
			}
		}
	}
}
