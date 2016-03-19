using System;
using System.Collections.Generic;
using UnityEngine;

public class TsMeshCombine : MonoBehaviour
{
	private class Combiner
	{
		private Transform root;

		private int layer;

		private string nameTag;

		private Material material;

		public List<TsMeshUtil.CombineData> list;

		public string Name
		{
			get
			{
				return string.Format("#Combined mesh (Material=\"{0}\", {1})", this.material.name, (!string.IsNullOrEmpty(this.nameTag)) ? this.nameTag : "?");
			}
		}

		public Combiner(Material material, int layer, Transform root, string tag = "")
		{
			this.root = root;
			this.material = material;
			this.layer = layer;
			this.list = new List<TsMeshUtil.CombineData>();
			this.nameTag = tag;
		}

		public void Add(TsMeshUtil.CombineData data)
		{
			this.list.Add(data);
		}

		public void Combine()
		{
			Mesh mesh = TsMeshUtil.Combine(this.list, this.material.name);
			if (mesh != null)
			{
				mesh.name = string.Format("Combined ({0})", this.root.name);
				GameObject gameObject = new GameObject(this.Name);
				gameObject.transform.parent = this.root;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.AddComponent(typeof(MeshFilter));
				gameObject.AddComponent(typeof(MeshRenderer));
				gameObject.renderer.material = this.material;
				gameObject.layer = this.layer;
				MeshFilter component = gameObject.GetComponent<MeshFilter>();
				component.mesh = mesh;
			}
			else
			{
				int count = this.list.Count;
				int num = count / 2;
				TsMeshCombine.Combiner combiner = new TsMeshCombine.Combiner(this.material, this.layer, this.root, this.nameTag);
				TsMeshCombine.Combiner combiner2 = new TsMeshCombine.Combiner(this.material, this.layer, this.root, this.nameTag);
				for (int i = 0; i < num; i++)
				{
					combiner.list.Add(this.list[i]);
				}
				for (int j = num; j < count; j++)
				{
					combiner2.list.Add(this.list[j]);
				}
				combiner.Combine();
				combiner2.Combine();
			}
		}
	}

	private static class Cells
	{
		public static Vector2 OFFSET;

		public static float CELL_WIDTH;

		public static float CELL_HEIGHT;

		public static int COLUMS;

		public static int ROWS;

		public static string BOUND_ROOT_NAME;

		static Cells()
		{
			TsMeshCombine.Cells.OFFSET = Vector2.zero;
			TsMeshCombine.Cells.CELL_WIDTH = 10000f;
			TsMeshCombine.Cells.CELL_HEIGHT = 10000f;
			TsMeshCombine.Cells.COLUMS = 1;
			TsMeshCombine.Cells.ROWS = 1;
			TsMeshCombine.Cells.BOUND_ROOT_NAME = string.Empty;
			if (TsPlatform.IsMobile)
			{
				TsMeshUtil.MAX_VERTICES = 2000;
			}
			TsMeshCombine.Cells.COLUMS = 3;
			TsMeshCombine.Cells.ROWS = 3;
			TsMeshCombine.Cells.BOUND_ROOT_NAME = "world_terrain";
		}

		public static void ResetBound()
		{
			try
			{
				Terrain terrain = Terrain.activeTerrain;
				GameObject gameObject = null;
				if (terrain == null)
				{
					gameObject = GameObject.Find(TsMeshCombine.Cells.BOUND_ROOT_NAME);
					terrain = gameObject.GetComponent<Terrain>();
				}
				if (terrain != null)
				{
					Collider collider = terrain.collider;
					if (collider != null)
					{
						TsMeshCombine.Cells.CELL_WIDTH = collider.bounds.size.x / (float)TsMeshCombine.Cells.COLUMS;
						TsMeshCombine.Cells.CELL_HEIGHT = collider.bounds.size.z / (float)TsMeshCombine.Cells.ROWS;
						Transform transform = terrain.transform;
						Vector3 position = transform.position;
						TsMeshCombine.Cells.OFFSET.x = position.x;
						TsMeshCombine.Cells.OFFSET.y = position.z;
					}
					else
					{
						Debug.LogError(string.Format("[TsMeshCombine] TerrainCollider에 접근할 수 없습니다. (\"{0}\")", terrain.name));
					}
				}
				else if (gameObject != null)
				{
					Vector3 vector = new Vector3(3.40282347E+38f, 3.40282347E+38f, 3.40282347E+38f);
					Vector3 vector2 = new Vector3(-3.40282347E+38f, -3.40282347E+38f, -3.40282347E+38f);
					MeshFilter[] componentsInChildren = gameObject.GetComponentsInChildren<MeshFilter>();
					MeshFilter[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						MeshFilter meshFilter = array[i];
						Transform transform2 = meshFilter.transform;
						Bounds bounds = meshFilter.sharedMesh.bounds;
						Vector3 min = bounds.min;
						Vector3 max = bounds.max;
						min.Scale(transform2.lossyScale);
						max.Scale(transform2.lossyScale);
						Vector3 vector3 = transform2.rotation * min + transform2.position;
						Vector3 vector4 = transform2.rotation * max + transform2.position;
						vector.x = Mathf.Min(vector.x, vector3.x);
						vector.y = Mathf.Min(vector.y, vector3.y);
						vector.z = Mathf.Min(vector.z, vector3.z);
						vector2.x = Mathf.Max(vector2.x, vector4.x);
						vector2.y = Mathf.Max(vector2.y, vector4.y);
						vector2.z = Mathf.Max(vector2.z, vector4.z);
					}
					TsMeshCombine.Cells.CELL_WIDTH = (vector2.x - vector.x) / (float)TsMeshCombine.Cells.COLUMS;
					TsMeshCombine.Cells.CELL_HEIGHT = (vector2.z - vector.z) / (float)TsMeshCombine.Cells.ROWS;
					TsMeshCombine.Cells.OFFSET.x = vector.x;
					TsMeshCombine.Cells.OFFSET.y = vector.z;
				}
				else
				{
					Debug.LogError(string.Format("[TsMeshCombine] 터레인으로 예상되는 매쉬를 찾을 수 없습니다. (\"{0}\")", TsMeshCombine.Cells.BOUND_ROOT_NAME));
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("[TsMeshCombine] bouding 크기 계산중 예외 발생 : {0}", arg));
			}
		}
	}

	private struct Key : IComparer<TsMeshCombine.Key>, IEqualityComparer<TsMeshCombine.Key>, IEquatable<TsMeshCombine.Key>
	{
		private ulong mID;

		public Key(Material mat, int layer, Vector3 worldPosition)
		{
			float num = Mathf.Clamp(worldPosition.x - TsMeshCombine.Cells.OFFSET.x, 0f, TsMeshCombine.Cells.CELL_WIDTH * (float)TsMeshCombine.Cells.COLUMS);
			float num2 = Mathf.Clamp(worldPosition.z - TsMeshCombine.Cells.OFFSET.y, 0f, TsMeshCombine.Cells.CELL_HEIGHT * (float)TsMeshCombine.Cells.ROWS);
			int num3 = (int)(num / TsMeshCombine.Cells.CELL_WIDTH);
			int num4 = (int)(num2 / TsMeshCombine.Cells.CELL_HEIGHT);
			int num5 = num3 + num4 * TsMeshCombine.Cells.COLUMS;
			this.mID = (ulong)((long)layer << 48 | (long)num5 << 32 | ((long)mat.GetHashCode() & (long)((ulong)-1)));
		}

		public override bool Equals(object obj)
		{
			return obj is TsMeshCombine.Key && this.Equals((TsMeshCombine.Key)obj);
		}

		public bool Equals(TsMeshCombine.Key obj)
		{
			return obj.mID == this.mID;
		}

		public bool Equals(TsMeshCombine.Key x, TsMeshCombine.Key y)
		{
			return x.Equals(y);
		}

		public int Compare(TsMeshCombine.Key x, TsMeshCombine.Key y)
		{
			return (x.mID != y.mID) ? ((x.mID >= y.mID) ? 1 : -1) : 0;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public int GetHashCode(TsMeshCombine.Key obj)
		{
			return obj.GetHashCode();
		}

		public override string ToString()
		{
			int num = (int)(this.mID >> 32 & 65535uL);
			int layer = (int)(this.mID >> 48 & 255uL);
			return string.Format("Cell=({0},{1}), Layer=\"{2}\"", num % TsMeshCombine.Cells.COLUMS, num / TsMeshCombine.Cells.COLUMS, LayerMask.LayerToName(layer));
		}
	}

	public void Combine()
	{
		if (base.gameObject.isStatic)
		{
			Debug.LogWarning(string.Format("이미 StaticBatching이 동작 중이므로 컴바인할 수 없습니다. (\"{0}\")", base.gameObject.name), base.gameObject);
			return;
		}
		TsMeshCombine.Cells.ResetBound();
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Dictionary<TsMeshCombine.Key, TsMeshCombine.Combiner> dictionary = new Dictionary<TsMeshCombine.Key, TsMeshCombine.Combiner>();
		MeshFilter[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			MeshFilter meshFilter = array[i];
			GameObject gameObject = meshFilter.gameObject;
			Renderer renderer = meshFilter.renderer;
			Mesh sharedMesh = meshFilter.sharedMesh;
			if (!sharedMesh.name.Contains("Combined Mesh (root: scene)"))
			{
				if (renderer != null && renderer.enabled && sharedMesh != null && !gameObject.isStatic)
				{
					if (gameObject.collider is MeshCollider)
					{
						Debug.LogWarning(string.Format("[CombinedMesh] 오브젝트에 MeshCollider가 있어서 combine 할 수 없습니다. (Object=\"{0}\", Mesh=\"{1}\")", gameObject.name, meshFilter.sharedMesh.name), gameObject);
					}
					else
					{
						TsMeshUtil.CombineData data = default(TsMeshUtil.CombineData);
						data.mesh = sharedMesh;
						data.transform = worldToLocalMatrix * meshFilter.transform.localToWorldMatrix;
						Vector3 position = meshFilter.transform.position;
						Material[] sharedMaterials = renderer.sharedMaterials;
						for (int j = 0; j < sharedMaterials.Length; j++)
						{
							Material material = sharedMaterials[j];
							data.subMeshIndex = Math.Min(j, data.mesh.subMeshCount - 1);
							TsMeshCombine.Key key = new TsMeshCombine.Key(material, gameObject.layer, position);
							TsMeshCombine.Combiner combiner;
							if (!dictionary.TryGetValue(key, out combiner))
							{
								combiner = new TsMeshCombine.Combiner(material, gameObject.layer, base.transform, key.ToString());
								dictionary.Add(key, combiner);
							}
							combiner.Add(data);
						}
						UnityEngine.Object.DestroyImmediate(renderer);
						UnityEngine.Object.DestroyImmediate(meshFilter);
					}
				}
			}
		}
		foreach (TsMeshCombine.Combiner current in dictionary.Values)
		{
			current.Combine();
		}
		this.CleanUp(base.transform);
		UnityEngine.Object.Destroy(this);
	}

	private void CleanUp(Transform target)
	{
		int childCount = target.childCount;
		Transform[] array = new Transform[childCount];
		for (int i = 0; i < childCount; i++)
		{
			array[i] = target.GetChild(i);
		}
		Transform[] array2 = array;
		for (int j = 0; j < array2.Length; j++)
		{
			Transform transform = array2[j];
			GameObject gameObject = transform.gameObject;
			MeshFilter component = transform.GetComponent<MeshFilter>();
			Renderer component2 = transform.GetComponent<Renderer>();
			if (component2 != null && component == null)
			{
				UnityEngine.Object.DestroyImmediate(component2);
			}
			int num = transform.GetComponents<Component>().Length;
			bool flag = !gameObject.activeInHierarchy || num <= 1 || (num == 2 && gameObject.collider != null && !gameObject.collider.enabled);
			if (transform.childCount == 0)
			{
				if (flag)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
			}
			else
			{
				this.CleanUp(transform);
				if (flag && transform.childCount == 0)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
			}
		}
	}
}
