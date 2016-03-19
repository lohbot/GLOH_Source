using System;
using UnityEngine;

public class NkRaycast
{
	private static RaycastHit m_Hit;

	private static float m_fDistance;

	private static Ray m_Ray;

	private static TsLayerMask mc_kPickLayer = TsLayer.EVERYTHING - TsLayer.DEFAULT - TsLayer.IGNORE_RAYCAST - TsLayer.FADE_OBJECT - TsLayer.PC - TsLayer.PC_OTHER - TsLayer.IGNORE_PICK;

	private static bool m_bHit = false;

	public static RaycastHit HIT
	{
		get
		{
			return NkRaycast.m_Hit;
		}
	}

	public static Vector3 POINT
	{
		get
		{
			return NkRaycast.HIT.point;
		}
	}

	public static string DebugOut()
	{
		return string.Format("HIT:{0}, T{1}", NkRaycast.m_Hit, Time.frameCount);
	}

	private static bool SetHitFaile()
	{
		NkRaycast.m_Hit.point = Vector3.zero;
		return false;
	}

	public static bool Raycast()
	{
		return NkRaycast.Raycast(3.40282347E+38f, NkRaycast.mc_kPickLayer);
	}

	public static bool Raycast(Ray ray)
	{
		return NkRaycast.Raycast(ray, 3.40282347E+38f, NkRaycast.mc_kPickLayer);
	}

	public static bool Raycast(int Mask)
	{
		return NkRaycast.Raycast(3.40282347E+38f, Mask);
	}

	public static bool Raycast(float fDistance, int Mask)
	{
		if (null == Camera.main)
		{
			return NkRaycast.SetHitFaile();
		}
		Ray ray = Camera.main.ScreenPointToRay(NkInputManager.mousePosition);
		return NkRaycast.UpdateRayCast(ray, fDistance, Mask);
	}

	public static bool Raycast(Ray ray, float fDistance, int Mask)
	{
		return NkRaycast.UpdateRayCast(ray, fDistance, Mask);
	}

	private static bool UpdateRayCast(Ray ray, float fDistance, int Mask)
	{
		if (NkRaycast.UpdateRay(ray, fDistance, Mask))
		{
			if (Physics.Raycast(ray, out NkRaycast.m_Hit, fDistance, Mask))
			{
				NkRaycast.m_bHit = true;
			}
			else
			{
				NkRaycast.m_bHit = false;
				NkRaycast.SetHitFaile();
			}
		}
		return NkRaycast.m_bHit;
	}

	private static bool UpdateRay(Ray ray, float fDistance, int Mask)
	{
		if (NkRaycast.m_Ray.direction == ray.direction && NkRaycast.m_Ray.origin == ray.origin && fDistance == NkRaycast.m_fDistance)
		{
			return false;
		}
		NkRaycast.m_Ray = ray;
		NkRaycast.m_fDistance = fDistance;
		return true;
	}
}
