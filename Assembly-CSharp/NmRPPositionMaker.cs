using System;
using UnityEngine;

public class NmRPPositionMaker : MonoBehaviour
{
	public ROAD_POINT m_kRP;

	public GameObject m_goMove;

	public void InitRPPositionMaker()
	{
		this.m_kRP = null;
		this.m_goMove = null;
	}

	public void SetRPPositionMaker(ROAD_POINT kRP, GameObject goMove)
	{
		this.m_kRP = kRP;
		this.m_goMove = goMove;
	}

	public void ChangePosition(Vector3 v3Position)
	{
		this.m_goMove.transform.localPosition = new Vector3(v3Position.x, v3Position.y, v3Position.z);
		this.m_kRP.POSX = v3Position.x;
		this.m_kRP.POSY = v3Position.y;
		this.m_kRP.POSZ = v3Position.z;
	}
}
