using System;
using System.Collections.Generic;
using UnityEngine;

public class NkBattleCharMove
{
	private NkBattleChar m_pkChar;

	private Vector3 m_vCharPos = Vector3.zero;

	private Vector3 m_vTargetPos = Vector3.zero;

	private Vector3 m_vFinalTargetPos = Vector3.zero;

	public eBATTLE_MOVE_STATUS m_eMoveStatus = eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_MAX;

	private Vector3 m_vFastMoveNextTargetPos = Vector3.zero;

	private bool m_bArrived;

	private bool m_bFastMove;

	private List<Vector3> m_AStarPath;

	private List<eBATTLE_MOVE_STATUS> m_MoveStatus;

	private List<GameObject> m_lsMoveArrow = new List<GameObject>();

	public NkBattleCharMove(NkBattleChar pkChar)
	{
		this.m_pkChar = pkChar;
		this.m_AStarPath = new List<Vector3>();
		this.m_MoveStatus = new List<eBATTLE_MOVE_STATUS>();
		this.Init();
	}

	public NkBattleChar GetBattleChar()
	{
		return this.m_pkChar;
	}

	public void Init()
	{
		this.m_vCharPos = new Vector3(0f, 0f, 0f);
		this.m_vTargetPos = new Vector3(0f, 0f, 0f);
		this.m_vFinalTargetPos = new Vector3(0f, 0f, 0f);
		this.m_vFastMoveNextTargetPos = new Vector3(0f, 0f, 0f);
		this.ClearPath();
		this.m_bArrived = true;
		this.ClearArrowMarksAll();
	}

	public void SetCharPos(GameObject pkChar)
	{
		this.m_vCharPos = pkChar.transform.position;
		this.m_pkChar.GetPersonInfo().SetCharPos(this.m_vCharPos);
	}

	public void SetForceCharPos(float x, float z)
	{
		Vector3 vector = new Vector3(x, 0f, z);
		NrBattleMap battleMap = Battle.BATTLE.GetBattleMap();
		if (battleMap == null)
		{
			return;
		}
		vector.y = battleMap.GetBattleMapHeight(vector) + 0.3f;
		this.m_pkChar.m_k3DChar.GetRootGameObject().transform.position = vector;
		this.m_pkChar.m_k3DChar.GetCharController().transform.position = vector;
		this.SetCharPos(this.m_pkChar.m_k3DChar.GetRootGameObject());
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
	}

	public Vector3 GetTargetPos()
	{
		return this.m_vTargetPos;
	}

	public void SetFinalTargetPos(float x, float y, float z)
	{
		this.m_vTargetPos.x = x;
		this.m_vTargetPos.y = y;
		this.m_vTargetPos.z = z;
	}

	public Vector3 GetFinalTargetPos()
	{
		return this.m_vFinalTargetPos;
	}

	public Vector3 GetFastMoveNextTargetPos()
	{
		return this.m_vFastMoveNextTargetPos;
	}

	public bool IsFastMoving()
	{
		return this.m_bFastMove;
	}

	public bool IsMoving()
	{
		return !this.m_bArrived;
	}

	public bool IsAutoMove()
	{
		return NrTSingleton<NrAutoPath>.Instance.IsAutoMoving();
	}

	public void ClearPath()
	{
		this.m_AStarPath.Clear();
	}

	public void AddPath(Vector3 pos)
	{
		this.m_AStarPath.Add(pos);
		this.m_bArrived = false;
	}

	public bool MoveTo(float x, float y, float z)
	{
		this.SetTargetPos(x, y, z);
		this.ClearPath();
		this.m_bArrived = false;
		this.AddPath(this.m_vTargetPos);
		this.ProcessCharMove(true);
		return true;
	}

	public bool StraightMoveTo(float x, float y, float z)
	{
		this.SetTargetPos(x, y, z);
		this.ClearPath();
		this.m_bArrived = false;
		this.AddPath(this.m_vTargetPos);
		this.ProcessCharMove(true);
		return true;
	}

	public bool MoveToFast(float x, float y, float z, float next_x, float next_y, float next_z)
	{
		this.SetTargetPos(x, y, z);
		this.m_vFastMoveNextTargetPos.x = next_x;
		this.m_vFastMoveNextTargetPos.y = next_y;
		this.m_vFastMoveNextTargetPos.z = next_z;
		this.ClearPath();
		this.m_bArrived = false;
		this.m_pkChar.SetIncreaseSpeed(10f);
		this.m_bFastMove = true;
		this.AddPath(this.m_vTargetPos);
		this.ProcessCharMove(true);
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
		if (NrTSingleton<NrAutoPath>.Instance.Generate(DestMapIndex, DestX, DestY) == 0)
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

	public void RefreshFindPath()
	{
		this.MoveTo(this.m_vTargetPos.x, this.m_vTargetPos.y, this.m_vTargetPos.z);
	}

	public void ForceStopChar(bool bSetAni, float x, float z)
	{
		if (this.m_bArrived)
		{
			return;
		}
		if (this.m_pkChar == null)
		{
			return;
		}
		if (this.m_pkChar.m_k3DChar == null)
		{
			return;
		}
		if (this.m_pkChar.IsReadyCharAction())
		{
			if (x <= 0f || z > 0f)
			{
			}
			this.SetCharPos(this.m_pkChar.m_k3DChar.GetRootGameObject());
			this.m_pkChar.m_k3DChar.MoveStop(bSetAni);
		}
		this.ClearPath();
		this.m_bArrived = true;
		this.m_bFastMove = false;
		this.m_pkChar.SetIncreaseSpeed(0f);
		this.ClearArrowMarksAll();
		NrTSingleton<NkClientLogic>.Instance.InitPickChar();
		if (this.m_pkChar.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_MOVE)
		{
			this.m_pkChar.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
		}
		this.m_eMoveStatus = eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_MAX;
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
		if (Battle.BATTLE == null)
		{
			return false;
		}
		if (this.m_bArrived)
		{
			return false;
		}
		if (!this.m_pkChar.m_k3DChar.IsMoveToTarget() && this.m_AStarPath.Count == 0)
		{
			this.SetTargetPos(0f, 0f, 0f);
			this.ClearPath();
			this.m_bArrived = true;
			this.ClearArrowMarksAll();
			if (this.m_pkChar.GetCurrentOrder() == eBATTLE_ORDER.eBATTLE_ORDER_MOVE)
			{
				this.m_pkChar.SetCurrentOrder(eBATTLE_ORDER.eBATTLE_ORDER_NONE);
				if (this.m_pkChar.GetComeBackRotate())
				{
					this.m_pkChar.SetRotate(this.m_pkChar.GetGridRotate());
					this.m_pkChar.SetComeBackRotate(false);
				}
				this.m_pkChar.SetAnimation(this.m_pkChar.GetStayAni());
			}
			if (this.m_bFastMove)
			{
				this.m_bFastMove = false;
				this.m_pkChar.SetIncreaseSpeed(0f);
				this.MoveTo(this.m_vFastMoveNextTargetPos.x, this.m_vFastMoveNextTargetPos.y, this.m_vFastMoveNextTargetPos.z);
			}
		}
		this.SetCharPos(this.m_pkChar.m_k3DChar.GetRootGameObject());
		this.UpdateMoveArrow();
		return true;
	}

	public void ProcessCharMove(bool bStartMove)
	{
		if (this.m_bArrived)
		{
			return;
		}
		if (this.m_eMoveStatus == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_WAIT)
		{
			return;
		}
		if (this.m_pkChar.m_k3DChar == null)
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
		if (flag && this.m_AStarPath.Count > 0)
		{
			Vector3 vector = this.m_AStarPath[0];
			this.m_AStarPath.RemoveAt(0);
			this.SetTargetPos(vector.x, vector.y, vector.z);
			this.m_eMoveStatus = this.m_MoveStatus[0];
			this.m_MoveStatus.RemoveAt(0);
			this.m_pkChar.m_k3DChar.SetLookAt(new Vector3(vector.x, vector.y, vector.z));
			this.m_pkChar.m_k3DChar.MoveTo(vector.x, vector.y, vector.z);
		}
	}

	public static float CalcHeight(Vector3 pos)
	{
		float result = 0f;
		float y = pos.y;
		pos.y = 200f;
		Vector3 vector = Vector3.zero;
		Ray ray = new Ray(pos, new Vector3(0f, -1f, 0f));
		if (NkRaycast.Raycast(ray))
		{
			vector = NkRaycast.POINT;
			RaycastHit hIT = NkRaycast.HIT;
			if (hIT.transform != null)
			{
				if (hIT.transform.position == Vector3.zero)
				{
					result = NrTSingleton<NrTerrain>.Instance.SampleHeight(pos);
				}
				else
				{
					result = hIT.transform.collider.ClosestPointOnBounds(pos).y;
				}
			}
		}
		pos.y = y;
		return result;
	}

	public void AddAstarPath(float x, float y, float z, eBATTLE_MOVE_STATUS eSTATUS)
	{
		Vector3 item = new Vector3(x, y, z);
		if (eSTATUS == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_STOP)
		{
			this.ClearPath();
			this.m_MoveStatus.Clear();
			this.m_eMoveStatus = eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_MAX;
		}
		this.m_AStarPath.Add(item);
		this.m_MoveStatus.Add(eSTATUS);
	}

	public void MoveServerAStar()
	{
		this.m_bArrived = false;
		this.ProcessCharMove(true);
	}

	public void RenewMovePath(Vector3 path, eBATTLE_MOVE_STATUS eMoveStatus)
	{
		this.m_bArrived = false;
		if (this.m_eMoveStatus == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL)
		{
			this.m_AStarPath.Insert(0, this.m_vTargetPos);
			this.m_MoveStatus.Insert(0, eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL);
		}
		if (this.m_eMoveStatus == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_CHANGEPOS)
		{
			this.SetTargetPos(path.x, path.y, path.z);
			this.m_pkChar.m_k3DChar.SetLookAt(path.x, path.y, path.z, true);
			this.m_pkChar.m_k3DChar.MoveTo(path.x, path.y, path.z);
		}
		else
		{
			int num = this.m_MoveStatus.IndexOf(eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_NORMAL);
			if (num >= 0)
			{
				this.m_AStarPath.Insert(num, path);
				this.m_MoveStatus.Insert(num, eMoveStatus);
			}
			if (this.m_eMoveStatus == eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_WAIT)
			{
				this.m_eMoveStatus = eBATTLE_MOVE_STATUS.eBATTLE_MOVE_STATUS_MAX;
			}
			this.ProcessCharMove(true);
		}
	}

	public void ClearArrowMarksAll()
	{
		foreach (GameObject current in this.m_lsMoveArrow)
		{
			UnityEngine.Object.Destroy(current);
		}
		this.m_lsMoveArrow.Clear();
	}

	public void MakeMoveArrow()
	{
		if (this.m_AStarPath.Count <= 0)
		{
			return;
		}
		Vector3 vFrom = this.GetCharPos();
		foreach (Vector3 current in this.m_AStarPath)
		{
			this.DrawLine(vFrom, current);
			vFrom = current;
		}
	}

	public void DrawLine(Vector3 vFrom, Vector3 vTo)
	{
		vTo.y = 0f;
		vFrom.y = 0f;
		float num = 0.5f;
		Vector3 vector = vTo - vFrom;
		vector.Normalize();
		Quaternion localRotation = Quaternion.LookRotation(vector);
		vector *= num;
		float num2 = Vector3.Distance(vTo, vFrom);
		num2 -= num;
		float num3 = 0f;
		while (num2 > 0f)
		{
			vFrom += vector;
			Vector3 vector2 = vFrom;
			if (num3 == 0f)
			{
				num3 = NkBattleCharMove.CalcHeight(vector2);
			}
			vector2.y = num3 + 0.1f;
			num2 -= num;
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Battle.BATTLE.MoveArrow, Vector3.zero, Quaternion.identity);
			if (gameObject == null)
			{
				return;
			}
			this.m_lsMoveArrow.Add(gameObject);
			gameObject.transform.localPosition = vector2;
			gameObject.transform.localRotation = localRotation;
		}
	}

	public void UpdateMoveArrow()
	{
		Vector3 vector = Vector3.zero;
		GameObject y = null;
		foreach (GameObject current in this.m_lsMoveArrow)
		{
			vector = current.transform.position - this.GetCharPos();
			if (null == y && vector.magnitude <= 1f)
			{
				y = current;
			}
		}
		if (null != y)
		{
			foreach (GameObject current2 in this.m_lsMoveArrow)
			{
				if (!(current2 != y))
				{
					break;
				}
				current2.transform.GetChild(0).localPosition = new Vector3(1000f, 1000f, 1000f);
			}
		}
	}
}
