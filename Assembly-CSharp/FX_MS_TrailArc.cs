using System;
using UnityEngine;

public class FX_MS_TrailArc : MonoBehaviour
{
	private class TrailPoint
	{
		public float timeCreated;

		public Vector3 position = Vector3.zero;

		public float timeAlive
		{
			get
			{
				return Time.time - this.timeCreated;
			}
		}

		public TrailPoint(Vector3 kPoint)
		{
			this.position = kPoint;
			this.timeCreated = Time.time;
		}

		public void update()
		{
			this.timeCreated = Time.time;
		}
	}

	private int savedIndex;

	private int pointIndex;

	public Material material;

	private bool Emit;

	public float lifetime = 1f;

	private float fadeOutRatio = 1f;

	private float lifeTimeRatio = 1f;

	[SerializeField]
	public float[] widths;

	public float pointDistance = 0.5f;

	private float pointSqrDistance;

	public int segmentsPerPoint = 10;

	private float tRatio = 0.2f;

	private GameObject TrailObject;

	private Mesh TrailMesh;

	private Material TrailMaterial;

	private Vector3[] saved;

	private Vector3[] savedUp;

	private int savedCnt;

	private FX_MS_TrailArc.TrailPoint[] points;

	private FX_MS_TrailArc.TrailPoint[] pointsUp;

	private int pointCnt;

	private int displayCnt;

	private float lastPointCreationTime;

	private float averageCreationTime;

	private float averageInsertionTime;

	private float elapsedInsertionTime;

	private bool initialized;

	public bool emit
	{
		get
		{
			return this.Emit;
		}
		set
		{
			this.Emit = value;
		}
	}

	private void Start()
	{
		this.saved = new Vector3[256];
		this.savedUp = new Vector3[this.saved.Length];
		this.points = new FX_MS_TrailArc.TrailPoint[this.saved.Length * this.segmentsPerPoint];
		this.pointsUp = new FX_MS_TrailArc.TrailPoint[this.points.Length];
		this.tRatio = 1f / (float)this.segmentsPerPoint;
		this.pointSqrDistance = this.pointDistance * this.pointDistance;
		this.displayCnt = 0;
		this.pointCnt = 0;
		this.savedCnt = 0;
		this.TrailObject = new GameObject("Trail");
		this.TrailObject.transform.position = Vector3.zero;
		this.TrailObject.transform.rotation = Quaternion.identity;
		this.TrailObject.transform.localScale = Vector3.one;
		MeshFilter meshFilter = (MeshFilter)this.TrailObject.AddComponent(typeof(MeshFilter));
		this.TrailMesh = meshFilter.mesh;
		this.TrailObject.AddComponent(typeof(MeshRenderer));
		this.TrailMaterial = new Material(this.material);
		this.TrailObject.renderer.material = this.TrailMaterial;
	}

	private void FixedUpdate()
	{
		if (this.Emit)
		{
			if (!this.initialized)
			{
				this._initPoints();
			}
			if ((this.saved[this.savedCnt - 1] - base.transform.position).sqrMagnitude > this.pointSqrDistance)
			{
				this.saved[this.savedCnt] = base.transform.position;
				this.savedUp[this.savedCnt] = base.transform.parent.forward;
				this.savedCnt++;
				if (this.averageCreationTime == 0f)
				{
					this.averageCreationTime = Time.time - this.lastPointCreationTime;
				}
				else
				{
					float num = Time.time - this.lastPointCreationTime;
					this.averageCreationTime = (this.averageCreationTime + num) * 0.5f;
				}
				this.averageInsertionTime = this.averageCreationTime * this.tRatio;
				this.lastPointCreationTime = Time.time;
				if (this.savedCnt > 3)
				{
					this._findCoordinates(this.savedCnt - 3);
				}
			}
			if (this.TrailMaterial.HasProperty("_TintColor"))
			{
				this._FadeOut_Alpha(this.TrailMaterial.GetColor("_TintColor"));
			}
			if (this.displayCnt < this.pointCnt)
			{
				this.elapsedInsertionTime += Time.deltaTime;
				while (this.elapsedInsertionTime > this.averageInsertionTime)
				{
					if (this.displayCnt < this.pointCnt)
					{
						this.displayCnt++;
					}
					this.elapsedInsertionTime -= this.averageInsertionTime;
				}
			}
			if (this.displayCnt < 2)
			{
				this.TrailObject.renderer.enabled = false;
				return;
			}
			this.TrailObject.renderer.enabled = true;
			this._RebuildTrailMesh();
		}
		else
		{
			this.displayCnt = 0;
			this.pointCnt = 0;
			this.savedCnt = 0;
		}
	}

	public void DestoryTrail()
	{
		this.TrailMesh.Clear();
		this.Emit = false;
		this.displayCnt = 0;
		this.pointCnt = 0;
		this.savedCnt = 0;
		this.initialized = false;
	}

	private void _findCoordinates(int index)
	{
		if (index == 0 || index >= this.savedCnt - 2)
		{
			return;
		}
		Vector3 b = this.saved[index - 1];
		Vector3 vector = this.saved[index];
		Vector3 a = this.saved[index + 1];
		Vector3 a2 = this.saved[index + 2];
		Vector3 a3 = 0.5f * (a - b);
		Vector3 a4 = 0.5f * (a2 - vector);
		int num = index * this.segmentsPerPoint;
		for (int i = num; i < num + this.segmentsPerPoint; i++)
		{
			float num2 = (float)(i - num) * this.tRatio;
			float num3 = num2 * num2;
			float num4 = num3 * num2;
			float d = 2f * num4 - 3f * num3 + 1f;
			float d2 = 3f * num3 - 2f * num4;
			float d3 = num4 - 2f * num3 + num2;
			float d4 = num4 - num3;
			int num5 = i - this.segmentsPerPoint;
			this.points[num5] = new FX_MS_TrailArc.TrailPoint(d * vector + d2 * a + d3 * a3 + d4 * a4);
			this.pointsUp[num5] = new FX_MS_TrailArc.TrailPoint(Vector3.Lerp(this.savedUp[index], this.savedUp[index + 1], num2));
		}
		this.pointCnt = num;
	}

	private void _initPoints()
	{
		this.saved[this.savedCnt] = base.transform.TransformPoint(0f, 0f, -this.pointDistance);
		this.savedUp[this.savedCnt] = base.transform.parent.forward;
		this.savedCnt++;
		this.saved[this.savedCnt] = base.transform.position;
		this.savedUp[this.savedCnt] = base.transform.parent.forward;
		this.savedCnt++;
		this.lastPointCreationTime = Time.time;
		this.initialized = true;
	}

	private void _FadeOut_Alpha(Color color)
	{
		color.a -= this.fadeOutRatio * this.lifeTimeRatio * Time.deltaTime;
		if (color.a > 0f)
		{
			this.TrailMaterial.SetColor("_TintColor", color);
		}
	}

	private float _ComputeTrailWidth(float ratio)
	{
		if (this.widths.Length == 0)
		{
			return 1f;
		}
		if (this.widths.Length == 1)
		{
			return this.widths[0];
		}
		if (this.widths.Length == 2)
		{
			return Mathf.Lerp(this.widths[1], this.widths[0], ratio);
		}
		float num = (float)(this.widths.Length - 1) - ratio * (float)(this.widths.Length - 1);
		if (num == (float)(this.widths.Length - 1))
		{
			return this.widths[this.widths.Length - 1];
		}
		int num2 = (int)Mathf.Floor(num);
		float t = num - (float)num2;
		return Mathf.Lerp(this.widths[num2], this.widths[num2 + 1], t);
	}

	private void _RebuildTrailMesh()
	{
		this.lifeTimeRatio = 1f / this.lifetime;
		Vector3[] array = new Vector3[this.displayCnt * 2];
		Vector2[] array2 = new Vector2[this.displayCnt * 2];
		int[] array3 = new int[(this.displayCnt - 1) * 6];
		float num = 1f / (float)(this.displayCnt - 1);
		for (int i = 0; i < this.displayCnt; i++)
		{
			float num2 = (float)i * num;
			FX_MS_TrailArc.TrailPoint trailPoint = this.points[i];
			float d = this._ComputeTrailWidth(num2);
			array[i * 2] = trailPoint.position - this.pointsUp[i].position * d * 0.5f;
			array[i * 2 + 1] = trailPoint.position + this.pointsUp[i].position * d * 0.5f;
			array2[i * 2] = new Vector2(num2 * 0.5f, 0f);
			array2[i * 2 + 1] = new Vector2(num2 * 0.5f, 1f);
			if (i > 0)
			{
				int num3 = (i - 1) * 6;
				int num4 = i * 2;
				array3[num3] = num4 - 2;
				array3[num3 + 1] = num4 - 1;
				array3[num3 + 2] = num4;
				array3[num3 + 3] = num4;
				array3[num3 + 4] = num4 - 1;
				array3[num3 + 5] = num4 + 1;
			}
		}
		this.TrailObject.transform.position = Vector3.zero;
		this.TrailObject.transform.rotation = Quaternion.identity;
		this.TrailMesh.Clear();
		this.TrailMesh.vertices = array;
		this.TrailMesh.uv = array2;
		this.TrailMesh.triangles = array3;
	}
}
