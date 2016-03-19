using System;
using System.Collections.Generic;
using UnityEngine;

public static class TsMeshUtil
{
	public struct CombineData
	{
		public Mesh mesh;

		public int subMeshIndex;

		public Matrix4x4 transform;
	}

	private static int mMaxVertexCount = 65000;

	public static int MAX_VERTICES
	{
		get
		{
			return TsMeshUtil.mMaxVertexCount;
		}
		set
		{
			TsMeshUtil.mMaxVertexCount = Mathf.Min(value, 65000);
		}
	}

	public static Mesh Combine(ICollection<TsMeshUtil.CombineData> combines, string materialName)
	{
		int num = 0;
		int num2 = 0;
		foreach (TsMeshUtil.CombineData current in combines)
		{
			if (current.mesh != null)
			{
				num += current.mesh.vertexCount;
				num2 += current.mesh.GetTriangles(current.subMeshIndex).Length;
			}
		}
		if (num > TsMeshUtil.MAX_VERTICES)
		{
			return null;
		}
		Vector3[] array = null;
		Vector3[] array2 = null;
		Vector4[] array3 = null;
		Vector2[] array4 = null;
		Vector2[] array5 = null;
		Color[] array6 = null;
		int[] array7 = new int[num2];
		int num3 = 0;
		num3 = 0;
		foreach (TsMeshUtil.CombineData current2 in combines)
		{
			if (current2.mesh)
			{
				if (array == null && current2.mesh.vertices.Length > 0)
				{
					array = new Vector3[num];
				}
				TsMeshUtil.CopyVertex(current2.mesh.vertexCount, current2.mesh.vertices, array, ref num3, current2.transform);
			}
		}
		num3 = 0;
		foreach (TsMeshUtil.CombineData current3 in combines)
		{
			if (current3.mesh)
			{
				if (array2 == null && current3.mesh.normals.Length > 0)
				{
					array2 = new Vector3[num];
				}
				Matrix4x4 transform = current3.transform;
				transform = transform.inverse.transpose;
				TsMeshUtil.CopyNormal(current3.mesh.vertexCount, current3.mesh.normals, array2, ref num3, transform);
			}
		}
		num3 = 0;
		foreach (TsMeshUtil.CombineData current4 in combines)
		{
			if (current4.mesh)
			{
				if (array3 == null && current4.mesh.tangents.Length > 0)
				{
					array3 = new Vector4[num];
				}
				Matrix4x4 transform2 = current4.transform;
				transform2 = transform2.inverse.transpose;
				TsMeshUtil.CopyTangents(current4.mesh.vertexCount, current4.mesh.tangents, array3, ref num3, transform2);
			}
		}
		num3 = 0;
		foreach (TsMeshUtil.CombineData current5 in combines)
		{
			if (current5.mesh)
			{
				if (array4 == null && current5.mesh.uv.Length > 0)
				{
					array4 = new Vector2[num];
				}
				TsMeshUtil.CopyTexUV(current5.mesh.vertexCount, current5.mesh.uv, array4, ref num3);
			}
		}
		num3 = 0;
		foreach (TsMeshUtil.CombineData current6 in combines)
		{
			if (current6.mesh)
			{
				if (array5 == null && current6.mesh.uv1.Length > 0)
				{
					array5 = new Vector2[num];
				}
				TsMeshUtil.CopyTexUV(current6.mesh.vertexCount, current6.mesh.uv1, array5, ref num3);
			}
		}
		num3 = 0;
		foreach (TsMeshUtil.CombineData current7 in combines)
		{
			if (current7.mesh)
			{
				if (array6 == null && current7.mesh.colors.Length > 0)
				{
					array6 = new Color[num];
				}
				TsMeshUtil.CopyColors(current7.mesh.vertexCount, current7.mesh.colors, array6, ref num3);
			}
		}
		int num4 = 0;
		int num5 = 0;
		foreach (TsMeshUtil.CombineData current8 in combines)
		{
			if (current8.mesh)
			{
				int[] triangles = current8.mesh.GetTriangles(current8.subMeshIndex);
				if (array7 == null && triangles.Length > 0)
				{
					array7 = new int[num2];
				}
				for (int i = 0; i < triangles.Length; i++)
				{
					array7[i + num4] = triangles[i] + num5;
				}
				num4 += triangles.Length;
				num5 += current8.mesh.vertexCount;
			}
		}
		Mesh mesh = new Mesh();
		mesh.name = string.Format("Combined Mesh ({1} : {0})", combines.Count, materialName);
		mesh.vertices = array;
		mesh.normals = array2;
		mesh.colors = array6;
		mesh.uv = array4;
		mesh.uv1 = array5;
		mesh.tangents = array3;
		mesh.triangles = array7;
		mesh.Optimize();
		return mesh;
	}

	private static void CopyVertex(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = transform.MultiplyPoint(src[i]);
		}
		offset += vertexcount;
	}

	private static void CopyNormal(int vertexcount, Vector3[] src, Vector3[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = transform.MultiplyVector(src[i]).normalized;
		}
		offset += vertexcount;
	}

	private static void CopyTexUV(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset += vertexcount;
	}

	private static void CopyColors(int vertexcount, Color[] src, Color[] dst, ref int offset)
	{
		for (int i = 0; i < src.Length; i++)
		{
			dst[i + offset] = src[i];
		}
		offset += vertexcount;
	}

	private static void CopyTangents(int vertexcount, Vector4[] src, Vector4[] dst, ref int offset, Matrix4x4 transform)
	{
		for (int i = 0; i < src.Length; i++)
		{
			Vector4 vector = src[i];
			Vector3 normalized = new Vector3(vector.x, vector.y, vector.z);
			normalized = transform.MultiplyVector(normalized).normalized;
			dst[i + offset] = new Vector4(normalized.x, normalized.y, normalized.z, vector.w);
		}
		offset += vertexcount;
	}
}
