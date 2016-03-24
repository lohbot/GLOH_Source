using System;
using UnityEngine;

namespace WellFired.Shared
{
	public class GridRenderingCamera : MonoBehaviour
	{
		public float gridWidth = 1f;

		public float gridSpacing = 2f;

		public Vector2 origin = new Vector2(-10f, -10f);

		private Camera gridCamera;

		public bool[,] Grid
		{
			get;
			set;
		}

		private void Start()
		{
			this.gridCamera = base.GetComponent<Camera>();
		}

		private void OnPostRender()
		{
			if (this.Grid == null)
			{
				return;
			}
			GridRenderer.RenderGrid(this.Grid, Color.black, this.origin, this.gridSpacing, this.gridWidth, this.gridCamera);
		}
	}
}
