using System;
using UnityEngine;

namespace WellFired.Shared
{
	public static class GridRenderer
	{
		private static string defaultShader = "Shader \"Lines/Colored Blended\" {SubShader { Pass { \tBlend SrcAlpha OneMinusSrcAlpha \tZWrite Off Cull Off Fog { Mode Off } \tBindChannels {\tBind \"vertex\", vertex Bind \"color\", color }} } }";

		private static Material cachedDefaultRenderMaterial;

		private static Material defaultRenderMaterial
		{
			get
			{
				if (!GridRenderer.cachedDefaultRenderMaterial)
				{
					GridRenderer.cachedDefaultRenderMaterial = new Material(GridRenderer.defaultShader);
				}
				return GridRenderer.cachedDefaultRenderMaterial;
			}
		}

		public static void RenderGrid(bool[,] grid, Color color, Vector2 origin, float gridSpacing, float lineWidth, Camera camera)
		{
			GridRenderer.RenderGridLines(grid, color, origin, gridSpacing, lineWidth, camera, GridRenderer.defaultRenderMaterial);
		}

		private static void RenderGridLines(bool[,] grid, Color color, Vector2 origin, float gridSpacing, float lineWidth, Camera camera, Material renderMaterial)
		{
			Transform transform = camera.transform;
			int num = grid.GetUpperBound(0) + 1;
			int num2 = grid.GetUpperBound(0) + 1;
			Vector3[,] array = new Vector3[num, num2];
			for (int i = 0; i < num; i++)
			{
				float num3 = origin.x + (float)i * gridSpacing;
				for (int j = 0; j < num2; j++)
				{
					float num4 = origin.y + (float)j * gridSpacing;
					float x = num3;
					float y = num4;
					array[i, j] = new Vector3(x, y);
				}
			}
			renderMaterial.SetPass(0);
			GL.Begin(7);
			GL.Color(color);
			float d = Mathf.Max(0f, 0.5f * lineWidth);
			for (int k = 0; k < 2; k++)
			{
				for (int l = 0; l < num - 1; l++)
				{
					for (int m = 0; m < num2 - 1; m++)
					{
						if (grid[l, m])
						{
							Vector3 a = array[l, m];
							Vector3 vector = (k != 0) ? array[l, m + 1] : array[l + 1, m];
							Vector3 a2 = (k != 0) ? array[l + 1, m] : array[l, m + 1];
							Vector3 a3 = array[l + 1, m + 1];
							Vector3 a4 = Vector3.Cross(a - vector, transform.forward).normalized;
							if (camera.orthographic)
							{
								a4 *= camera.orthographicSize * 2f / camera.pixelHeight;
							}
							else
							{
								a4 *= (camera.ScreenToWorldPoint(new Vector3(0f, 0f, 50f)) - camera.ScreenToWorldPoint(new Vector3(20f, 0f, 50f))).magnitude / 20f;
							}
							GL.Vertex(a - d * a4);
							GL.Vertex(a + d * a4);
							GL.Vertex(vector + d * a4);
							GL.Vertex(vector - d * a4);
							GL.Vertex(a2 - d * a4);
							GL.Vertex(a2 + d * a4);
							GL.Vertex(a3 + d * a4);
							GL.Vertex(a3 - d * a4);
						}
					}
				}
			}
			GL.End();
		}
	}
}
