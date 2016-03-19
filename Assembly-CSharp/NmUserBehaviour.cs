using System;
using UnityEngine;

public class NmUserBehaviour : MonoBehaviour
{
	public bool preRunState = true;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		this.UpdateWalkEffect(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		this.UpdateWalkEffect(true);
	}

	private void OnTriggerExit(Collider other)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			@char.OnWalkEffect(string.Empty, Vector3.zero);
		}
	}

	private void UpdateWalkEffect(bool bDirectUpdate)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			bool flag = @char.m_kCharMove.IsMoving();
			bool flag2 = this.preRunState != flag;
			if (!bDirectUpdate && !flag2)
			{
				return;
			}
			this.preRunState = flag;
			Vector3 centerPosition = @char.GetCenterPosition();
			Vector3 vPos = @char.m_kCharMove.GetCharPos();
			string effectKey = string.Empty;
			Ray ray = new Ray(centerPosition, Vector3.down);
			if (NkRaycast.Raycast(ray))
			{
				effectKey = this.GetEffectState(NkRaycast.HIT.transform, flag);
				vPos = NkRaycast.HIT.point;
				Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.black);
			}
			@char.OnWalkEffect(effectKey, vPos);
		}
	}

	private string GetEffectState(Transform TFM, bool bRunState)
	{
		if (!(null != TFM.gameObject) || TFM.gameObject.layer != TsLayer.WATER)
		{
			return string.Empty;
		}
		if (bRunState)
		{
			return "FX_RUN_WATER";
		}
		return "FX_STAY_WATER";
	}
}
