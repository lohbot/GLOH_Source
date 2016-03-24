using System;
using System.Collections.Generic;
using UnityEngine;

public class SplineTrailRenderer : MonoBehaviour
{
	public enum MeshDisposition
	{
		Continuous,
		Fragmented
	}

	public enum FadeType
	{
		None,
		MeshShrinking,
		Alpha,
		Both
	}

	public class AdvancedParameters
	{
		public int baseNbQuad = 1024;

		public int nbQuadIncrement = 256;

		public int nbSegmentToParametrize = 3;

		public float lengthToRedraw;

		public bool shiftMeshData = true;
	}

	private const int NbVertexPerQuad = 4;

	private const int NbTriIndexPerQuad = 6;

	public bool emit = true;

	public float emissionDistance = 1f;

	public float height = 1f;

	public float width = 0.2f;

	public Color vertexColor = Color.white;

	public Vector3 normal = new Vector3(0f, 0f, 1f);

	public SplineTrailRenderer.MeshDisposition meshDisposition;

	public SplineTrailRenderer.FadeType fadeType;

	public float fadeLengthBegin = 5f;

	public float fadeLengthEnd = 5f;

	public float maxLength = 50f;

	public bool debugDrawSpline;

	private SplineTrailRenderer.AdvancedParameters advancedParameters = new SplineTrailRenderer.AdvancedParameters();

	[HideInInspector]
	public CatmullRomSpline spline;

	private Vector3[] vertices;

	private int[] triangles;

	private Vector2[] uv;

	private Color[] colors;

	private Vector3[] normals;

	private Vector3 origin;

	private int maxInstanciedTriCount;

	private Mesh mesh;

	private int allocatedNbQuad;

	private int lastStartingQuad;

	private int quadOffset;

	public void Clear()
	{
		this.Init();
	}

	public void ImitateTrail(SplineTrailRenderer trail)
	{
		this.emit = trail.emit;
		this.emissionDistance = trail.emissionDistance;
		this.height = trail.height;
		this.width = trail.width;
		this.vertexColor = trail.vertexColor;
		this.normal = trail.normal;
		this.meshDisposition = trail.meshDisposition;
		this.fadeType = trail.fadeType;
		this.fadeLengthBegin = trail.fadeLengthBegin;
		this.fadeLengthEnd = trail.fadeLengthEnd;
		this.maxLength = trail.maxLength;
		this.debugDrawSpline = trail.debugDrawSpline;
		base.renderer.material = trail.renderer.material;
	}

	private void Awake()
	{
		this.spline = new CatmullRomSpline();
	}

	private void Start()
	{
		this.Init();
	}

	private void LateUpdate()
	{
		if (this.spline.knots.Count < 5)
		{
			Debug.LogError("Trailer Is Failed!!, Need to Reset!!");
			return;
		}
		if (this.emit)
		{
			List<Knot> knots = this.spline.knots;
			Vector3 position = base.transform.position;
			knots[knots.Count - 1].position = position;
			knots[knots.Count - 2].position = position;
			if (Vector3.Distance(knots[knots.Count - 3].position, position) > this.emissionDistance && Vector3.Distance(knots[knots.Count - 4].position, position) > this.emissionDistance)
			{
				knots.Add(new Knot(position));
			}
		}
		this.RenderMesh();
	}

	private void RenderMesh()
	{
		if (this.advancedParameters.nbSegmentToParametrize == 0)
		{
			this.spline.Parametrize();
		}
		else
		{
			this.spline.Parametrize(this.spline.NbSegments - this.advancedParameters.nbSegmentToParametrize, this.spline.NbSegments);
		}
		float num = Mathf.Max(this.spline.Length() - 0.1f, 0f);
		int num2 = (int)(1f / this.width * num) + 1 - this.quadOffset;
		if (this.allocatedNbQuad < num2)
		{
			this.Reallocate(num2);
			num = Mathf.Max(this.spline.Length() - 0.1f, 0f);
			num2 = (int)(1f / this.width * num) + 1 - this.quadOffset;
		}
		int num3 = this.lastStartingQuad;
		float num4 = (float)num3 * this.width + (float)this.quadOffset * this.width;
		this.maxInstanciedTriCount = Math.Max(this.maxInstanciedTriCount, (num2 - 1) * 6);
		CatmullRomSpline.Marker marker = new CatmullRomSpline.Marker();
		this.spline.PlaceMarker(marker, num4, null);
		Vector3 a = this.spline.GetPosition(marker);
		Vector3 vector = this.spline.GetTangent(marker);
		Vector3 a2 = CatmullRomSpline.ComputeBinormal(vector, this.normal);
		int num5 = (this.meshDisposition != SplineTrailRenderer.MeshDisposition.Fragmented) ? (num2 - 1) : (num2 - 1);
		int num6 = this.vertices.Length;
		int num7 = this.uv.Length;
		int num8 = this.triangles.Length;
		int num9 = this.colors.Length;
		for (int i = num3; i < num5; i++)
		{
			float num10 = num4 + this.width;
			int num11 = i * 4;
			int num12 = i * 6;
			this.spline.MoveMarker(marker, num10);
			Vector3 position = this.spline.GetPosition(marker);
			Vector3 tangent = this.spline.GetTangent(marker);
			Vector3 vector2 = CatmullRomSpline.ComputeBinormal(tangent, this.normal);
			float num13 = this.FadeMultiplier(num4, num);
			float num14 = this.FadeMultiplier(num10, num);
			float num15 = num13 * this.height;
			float num16 = num14 * this.height;
			if (this.fadeType == SplineTrailRenderer.FadeType.Alpha || this.fadeType == SplineTrailRenderer.FadeType.None)
			{
				num15 = ((num13 <= 0f) ? 0f : this.height);
				num16 = ((num14 <= 0f) ? 0f : this.height);
			}
			if (this.meshDisposition == SplineTrailRenderer.MeshDisposition.Continuous)
			{
				if (num6 > num11 + 4)
				{
					this.vertices[num11] = base.transform.InverseTransformPoint(a - this.origin + a2 * (num15 * 0.5f));
					this.vertices[num11 + 1] = base.transform.InverseTransformPoint(a - this.origin + -a2 * (num15 * 0.5f));
					this.vertices[num11 + 2] = base.transform.InverseTransformPoint(position - this.origin + vector2 * (num16 * 0.5f));
					this.vertices[num11 + 3] = base.transform.InverseTransformPoint(position - this.origin + -vector2 * (num16 * 0.5f));
				}
				if (num7 > num11 + 4)
				{
					this.uv[num11] = new Vector2(num4 / this.height, 1f);
					this.uv[num11 + 1] = new Vector2(num4 / this.height, 0f);
					this.uv[num11 + 2] = new Vector2(num10 / this.height, 1f);
					this.uv[num11 + 3] = new Vector2(num10 / this.height, 0f);
				}
			}
			else
			{
				Vector3 a3 = a + vector * this.width * -0.5f - this.origin;
				if (num6 > num11 + 4)
				{
					this.vertices[num11] = base.transform.InverseTransformPoint(a3 + a2 * (num15 * 0.5f));
					this.vertices[num11 + 1] = base.transform.InverseTransformPoint(a3 + -a2 * (num15 * 0.5f));
					this.vertices[num11 + 2] = base.transform.InverseTransformPoint(a3 + vector * this.width + a2 * (num15 * 0.5f));
					this.vertices[num11 + 3] = base.transform.InverseTransformPoint(a3 + vector * this.width + -a2 * (num15 * 0.5f));
				}
				if (num7 > num11 + 4)
				{
					this.uv[num11] = new Vector2(0f, 1f);
					this.uv[num11 + 1] = new Vector2(0f, 0f);
					this.uv[num11 + 2] = new Vector2(1f, 1f);
					this.uv[num11 + 3] = new Vector2(1f, 0f);
				}
			}
			if (num8 > num12 + 6)
			{
				this.triangles[num12] = num11;
				this.triangles[num12 + 1] = num11 + 1;
				this.triangles[num12 + 2] = num11 + 2;
				this.triangles[num12 + 3] = num11 + 2;
				this.triangles[num12 + 4] = num11 + 1;
				this.triangles[num12 + 5] = num11 + 3;
			}
			if (num9 > num11 + 4)
			{
				this.colors[num11] = this.vertexColor;
				this.colors[num11 + 1] = this.vertexColor;
				this.colors[num11 + 2] = this.vertexColor;
				this.colors[num11 + 3] = this.vertexColor;
			}
			if ((this.fadeType == SplineTrailRenderer.FadeType.Alpha || this.fadeType == SplineTrailRenderer.FadeType.Both) && num9 > num11 + 4)
			{
				Color[] expr_6F0_cp_0 = this.colors;
				int expr_6F0_cp_1 = num11;
				expr_6F0_cp_0[expr_6F0_cp_1].a = expr_6F0_cp_0[expr_6F0_cp_1].a * num13;
				Color[] expr_70D_cp_0 = this.colors;
				int expr_70D_cp_1 = num11 + 1;
				expr_70D_cp_0[expr_70D_cp_1].a = expr_70D_cp_0[expr_70D_cp_1].a * num13;
				Color[] expr_72A_cp_0 = this.colors;
				int expr_72A_cp_1 = num11 + 2;
				expr_72A_cp_0[expr_72A_cp_1].a = expr_72A_cp_0[expr_72A_cp_1].a * num14;
				Color[] expr_747_cp_0 = this.colors;
				int expr_747_cp_1 = num11 + 3;
				expr_747_cp_0[expr_747_cp_1].a = expr_747_cp_0[expr_747_cp_1].a * num14;
			}
			a = position;
			vector = tangent;
			a2 = vector2;
			num4 = num10;
		}
		for (int j = (num2 - 1) * 6; j < this.maxInstanciedTriCount; j++)
		{
			if (j < this.triangles.Length)
			{
				this.triangles[j] = 0;
			}
		}
		this.lastStartingQuad = ((this.advancedParameters.lengthToRedraw != 0f) ? Math.Max(0, num2 - ((int)(this.advancedParameters.lengthToRedraw / this.width) + 5)) : Math.Max(0, num2 - ((int)(this.maxLength / this.width) + 5)));
		this.mesh.Clear();
		this.mesh.vertices = this.vertices;
		this.mesh.uv = this.uv;
		this.mesh.triangles = this.triangles;
		this.mesh.colors = this.colors;
		this.mesh.normals = this.normals;
	}

	private void OnDrawGizmos()
	{
		if (this.advancedParameters != null && this.spline != null && this.debugDrawSpline)
		{
			this.spline.DebugDrawSpline();
		}
	}

	private void Init()
	{
		this.origin = Vector3.zero;
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		if (this.mesh == null)
		{
			Debug.LogError("Dosen't Exist MeshFilter!!");
			return;
		}
		this.mesh.MarkDynamic();
		this.allocatedNbQuad = this.advancedParameters.baseNbQuad;
		this.maxInstanciedTriCount = 0;
		this.lastStartingQuad = 0;
		this.quadOffset = 0;
		this.vertices = new Vector3[this.advancedParameters.baseNbQuad * 4];
		this.triangles = new int[this.advancedParameters.baseNbQuad * 6];
		this.uv = new Vector2[this.advancedParameters.baseNbQuad * 4];
		this.colors = new Color[this.advancedParameters.baseNbQuad * 4];
		this.normals = new Vector3[this.advancedParameters.baseNbQuad * 4];
		if (this.normal == Vector3.zero)
		{
			this.normal = (base.transform.position - Camera.main.transform.position).normalized;
		}
		for (int i = 0; i < this.normals.Length; i++)
		{
			this.normals[i] = this.normal;
		}
		this.spline.Clear();
		List<Knot> knots = this.spline.knots;
		Vector3 position = base.transform.position;
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
		knots.Add(new Knot(position));
	}

	private void Reallocate(int nbQuad)
	{
		if (this.advancedParameters.shiftMeshData && this.lastStartingQuad > 0)
		{
			int num = 0;
			for (int i = this.lastStartingQuad; i < nbQuad; i++)
			{
				if (i < this.allocatedNbQuad * 4)
				{
					this.vertices[num] = this.vertices[i];
					this.uv[num] = this.uv[i];
					this.colors[num] = this.colors[i];
					this.normals[num] = this.normals[i];
				}
				if (i < this.allocatedNbQuad * 6)
				{
					this.triangles[num] = this.triangles[i];
				}
				num++;
			}
			this.quadOffset += this.lastStartingQuad;
			this.lastStartingQuad = 0;
		}
		if (this.allocatedNbQuad < nbQuad - this.quadOffset)
		{
			this.Clear();
		}
	}

	private float FadeMultiplier(float distance, float length)
	{
		float a = Mathf.Clamp01((distance - Mathf.Max(length - this.maxLength, 0f)) / this.fadeLengthBegin);
		float b = Mathf.Clamp01((length - distance) / this.fadeLengthEnd);
		return Mathf.Min(a, b);
	}
}
