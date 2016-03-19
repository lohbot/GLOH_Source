using System;
using System.Collections.Generic;
using UnityEngine;

public class FS_ShadowManager : MonoBehaviour
{
	private static FS_ShadowManager _manager;

	private Dictionary<Material, FS_ShadowManagerMesh> shadowMeshes = new Dictionary<Material, FS_ShadowManagerMesh>();

	private Dictionary<Material, FS_ShadowManagerMesh> shadowMeshesStatic = new Dictionary<Material, FS_ShadowManagerMesh>();

	private int frameCalcedFustrum;

	private Plane[] fustrumPlanes;

	private void Start()
	{
		FS_ShadowManager[] array = (FS_ShadowManager[])UnityEngine.Object.FindObjectsOfType(typeof(FS_ShadowManager));
		if (array.Length > 1)
		{
			Debug.LogWarning("There should only be one FS_ShadowManger in the scene. Found " + array.Length);
		}
	}

	private void OnApplicationQuit()
	{
		this.shadowMeshes.Clear();
		this.shadowMeshesStatic.Clear();
	}

	public static FS_ShadowManager Manager()
	{
		if (FS_ShadowManager._manager == null)
		{
			FS_ShadowManager fS_ShadowManager = (FS_ShadowManager)UnityEngine.Object.FindObjectOfType(typeof(FS_ShadowManager));
			if (fS_ShadowManager == null)
			{
				GameObject gameObject = new GameObject("FS_ShadowManager");
				FS_ShadowManager._manager = gameObject.AddComponent<FS_ShadowManager>();
			}
			else
			{
				FS_ShadowManager._manager = fS_ShadowManager;
			}
		}
		return FS_ShadowManager._manager;
	}

	public void RecalculateStaticGeometry(FS_ShadowSimple removeShadow)
	{
		FS_MeshKey meshKey = new FS_MeshKey(removeShadow.shadowMaterial, true);
		this.RecalculateStaticGeometry(removeShadow, meshKey);
	}

	public void RecalculateStaticGeometry(FS_ShadowSimple removeShadow, FS_MeshKey meshKey)
	{
		if (this.shadowMeshesStatic.ContainsKey(meshKey.mat))
		{
			FS_ShadowManagerMesh fS_ShadowManagerMesh = this.shadowMeshesStatic[meshKey.mat];
			if (removeShadow != null)
			{
				fS_ShadowManagerMesh.removeShadow(removeShadow);
			}
			fS_ShadowManagerMesh.recreateStaticGeometry();
		}
	}

	public void registerGeometry(FS_ShadowSimple s, FS_MeshKey meshKey)
	{
		FS_ShadowManagerMesh fS_ShadowManagerMesh;
		if (meshKey.isStatic)
		{
			if (!this.shadowMeshesStatic.ContainsKey(meshKey.mat))
			{
				fS_ShadowManagerMesh = new GameObject("ShadowMeshStatic_" + meshKey.mat.name)
				{
					transform = 
					{
						parent = base.transform
					}
				}.AddComponent<FS_ShadowManagerMesh>();
				fS_ShadowManagerMesh.shadowMaterial = s.shadowMaterial;
				fS_ShadowManagerMesh.isStatic = true;
				this.shadowMeshesStatic.Add(meshKey.mat, fS_ShadowManagerMesh);
			}
			else
			{
				fS_ShadowManagerMesh = this.shadowMeshesStatic[meshKey.mat];
			}
		}
		else if (!this.shadowMeshes.ContainsKey(meshKey.mat))
		{
			fS_ShadowManagerMesh = new GameObject("ShadowMesh_" + meshKey.mat.name)
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<FS_ShadowManagerMesh>();
			fS_ShadowManagerMesh.shadowMaterial = s.shadowMaterial;
			fS_ShadowManagerMesh.isStatic = false;
			this.shadowMeshes.Add(meshKey.mat, fS_ShadowManagerMesh);
		}
		else
		{
			fS_ShadowManagerMesh = this.shadowMeshes[meshKey.mat];
		}
		fS_ShadowManagerMesh.registerGeometry(s);
	}

	public Plane[] getCameraFustrumPlanes()
	{
		if (Time.frameCount != this.frameCalcedFustrum || this.fustrumPlanes == null)
		{
			Camera main = Camera.main;
			if (main == null)
			{
				Debug.LogWarning("No main camera could be found for visibility culling.");
				this.fustrumPlanes = null;
			}
			else
			{
				this.fustrumPlanes = GeometryUtility.CalculateFrustumPlanes(main);
				this.frameCalcedFustrum = Time.frameCount;
			}
		}
		return this.fustrumPlanes;
	}
}
