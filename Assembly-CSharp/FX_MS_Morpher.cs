using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class FX_MS_Morpher : MonoBehaviour
{
	[SerializeField]
	public WrapMode _WrapMode = WrapMode.Loop;

	[HideInInspector]
	public List<Mesh> mlistMesh = new List<Mesh>();

	[HideInInspector]
	public List<float> mlistMorphTime = new List<float>();

	private bool _isActive = true;

	private Mesh _tranMesh;

	private int _currentMeshIndex;

	private int _nextMeshIndex = 1;

	private bool _isInit;

	private float _startTime;

	private float _endTime;

	public float CurrentMorphTime
	{
		get
		{
			return this.mlistMorphTime[this._currentMeshIndex];
		}
	}

	public int MeshInfoCount
	{
		get
		{
			return this.mlistMesh.Count;
		}
		set
		{
			if (value != this.mlistMesh.Count)
			{
				if (value < 2)
				{
					Debug.Log("The mesh morpher needs at least 2 source meshes. value is " + value);
					this._isActive = false;
					return;
				}
				List<Mesh> list = new List<Mesh>(this.mlistMesh);
				if (value < this.mlistMesh.Count)
				{
					this.mlistMesh.CopyTo(list.ToArray(), 0);
					list.RemoveRange(value, this.mlistMesh.Count - value);
				}
				else if (value > this.mlistMesh.Count)
				{
					for (int i = this.mlistMesh.Count; i < value; i++)
					{
						list.Add(new Mesh());
					}
				}
				this.mlistMesh = list;
				List<float> list2 = new List<float>(this.mlistMorphTime);
				int num = value - 1;
				if (num < this.mlistMorphTime.Count)
				{
					this.mlistMorphTime.CopyTo(list2.ToArray(), 0);
					list2.RemoveRange(num, this.mlistMorphTime.Count - num);
				}
				else if (num > this.mlistMorphTime.Count)
				{
					for (int j = this.mlistMorphTime.Count; j < num; j++)
					{
						list2.Add(0f);
					}
				}
				this.mlistMorphTime = list2;
			}
		}
	}

	private static void Morph(Mesh srcMesh, Mesh destMesh, float time, float maxTime, ref Mesh targetMesh)
	{
		if (srcMesh.vertexCount != destMesh.vertexCount || targetMesh.vertexCount != srcMesh.vertexCount)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Faild Morph() : srcMesh =",
				srcMesh.name,
				"   dstMesh =",
				destMesh.name,
				"   TargetMesh =",
				targetMesh.name
			}));
			return;
		}
		float num = time / maxTime;
		num = Mathf.Clamp(num, 0f, 1f);
		Vector3[] vertices = srcMesh.vertices;
		Vector3[] vertices2 = destMesh.vertices;
		Vector3[] vertices3 = targetMesh.vertices;
		for (int i = 0; i < vertices3.Length; i++)
		{
			vertices3[i] = Vector3.Lerp(vertices[i], vertices2[i], num);
		}
		targetMesh.vertices = vertices3;
		targetMesh.RecalculateBounds();
	}

	private void Awake()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		if (component == null)
		{
			Debug.Log("Not Found MeshFilter!!!!");
			Debug.Break();
			this._isActive = false;
			return;
		}
		if (this.mlistMesh.Count > 0)
		{
			component.mesh = this.mlistMesh[0];
			this._tranMesh = component.mesh;
			for (int i = 0; i < this.mlistMesh.Count; i++)
			{
				if (this.mlistMesh[i] == null)
				{
					Debug.Log("No Mesh setting yet. Can't Continue Mesh Morph");
					Debug.Break();
					this._isActive = false;
					return;
				}
			}
			for (int j = 0; j < this.mlistMesh.Count; j++)
			{
				if (this.mlistMesh[j].vertexCount != this._tranMesh.vertexCount)
				{
					Debug.Log("Doesn't have the same number of vertices as the first mesh");
					Debug.Break();
					this._isActive = false;
					return;
				}
			}
			return;
		}
		Debug.Log("No Mesh setting yet. Can't Continue Mesh Morph");
		Debug.Break();
		this._isActive = false;
	}

	private void Update()
	{
		if (!this._isActive)
		{
			return;
		}
		if (!this._isInit)
		{
			this._isInit = true;
			this._startTime = Time.time;
			this._endTime = Time.time + this.CurrentMorphTime;
			MeshFilter component = base.GetComponent<MeshFilter>();
			component.mesh = this.mlistMesh[this._currentMeshIndex];
			this._tranMesh = component.mesh;
		}
		FX_MS_Morpher.Morph(this.mlistMesh[this._currentMeshIndex], this.mlistMesh[this._nextMeshIndex], Time.time - this._startTime, this._endTime - this._startTime, ref this._tranMesh);
		if (this._endTime <= Time.time)
		{
			this._Next();
			this._isInit = false;
		}
	}

	private void _Next()
	{
		if (this._WrapMode == WrapMode.Once)
		{
			this._currentMeshIndex++;
			this._nextMeshIndex = this._currentMeshIndex + 1;
			if (this.mlistMesh.Count <= this._nextMeshIndex)
			{
				this._currentMeshIndex = 0;
				this._nextMeshIndex = 1;
				this._isActive = false;
			}
		}
		else if (this._WrapMode == WrapMode.Loop)
		{
			this._currentMeshIndex++;
			this._nextMeshIndex = this._currentMeshIndex + 1;
			if (this.mlistMesh.Count <= this._nextMeshIndex)
			{
				this._currentMeshIndex = 0;
				this._nextMeshIndex = 1;
			}
		}
	}
}
