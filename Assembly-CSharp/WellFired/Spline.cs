using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WellFired
{
	[Serializable]
	public class Spline
	{
		[SerializeField]
		private Color splineColor = Color.white;

		[SerializeField]
		private float displayResolution = 20f;

		[SerializeField]
		private AbstractSplineSolver splineSolver;

		public int CurrentSegment
		{
			get;
			private set;
		}

		public bool IsClosed
		{
			get;
			private set;
		}

		private bool IsReversed
		{
			get;
			set;
		}

		public Color SplineColor
		{
			get
			{
				return this.splineColor;
			}
			set
			{
				this.splineColor = value;
			}
		}

		public float DisplayResolution
		{
			get
			{
				return this.displayResolution;
			}
			set
			{
				this.displayResolution = value;
			}
		}

		public List<SplineKeyframe> Nodes
		{
			get
			{
				return this.SplineSolver.Nodes;
			}
		}

		public AbstractSplineSolver SplineSolver
		{
			get
			{
				return this.splineSolver;
			}
			private set
			{
				this.splineSolver = value;
			}
		}

		public Vector3 LastNode
		{
			get
			{
				return this.SplineSolver.Nodes[this.SplineSolver.Nodes.Count].Position;
			}
		}

		public void BuildFromKeyframes(List<SplineKeyframe> keyframes)
		{
			bool flag = this.SplineSolver == null;
			if (this.SplineSolver != null)
			{
				flag = (keyframes.Count<SplineKeyframe>() != this.Nodes.Count<SplineKeyframe>());
				if (!(this.SplineSolver is LinearSplineSolver) && this.SplineSolver.Nodes.Count == 2)
				{
					flag = true;
				}
				else if (!(this.SplineSolver is QuadraticSplineSolver) && this.SplineSolver.Nodes.Count == 3)
				{
					flag = true;
				}
				else if (!(this.SplineSolver is QuadraticSplineSolver) && this.SplineSolver.Nodes.Count == 4)
				{
					flag = true;
				}
				else if (!(this.SplineSolver is CatmullRomSplineSolver))
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (this.SplineSolver != null)
				{
					UnityEngine.Object.DestroyImmediate(this.SplineSolver);
				}
				if (keyframes.Count == 2)
				{
					this.SplineSolver = ScriptableObject.CreateInstance<LinearSplineSolver>();
				}
				else if (keyframes.Count == 3)
				{
					this.SplineSolver = ScriptableObject.CreateInstance<QuadraticSplineSolver>();
				}
				else if (keyframes.Count == 4)
				{
					this.SplineSolver = ScriptableObject.CreateInstance<CubicBezierSplineSolver>();
				}
				else
				{
					this.SplineSolver = ScriptableObject.CreateInstance<CatmullRomSplineSolver>();
				}
			}
			this.SplineSolver.Nodes = keyframes;
			this.SplineSolver.Build();
		}

		private Vector3 GetPosition(float time)
		{
			return this.SplineSolver.GetPosition(time);
		}

		public Vector3 GetPositionOnPath(float time)
		{
			if (time < 0f || time > 1f)
			{
				if (this.IsClosed)
				{
					if (time < 0f)
					{
						time += 1f;
					}
					else
					{
						time -= 1f;
					}
				}
				else
				{
					time = Mathf.Clamp01(time);
				}
			}
			return this.SplineSolver.GetPositionOnPath(time);
		}

		public void Close()
		{
			if (this.IsClosed)
			{
				throw new Exception("Closing a Spline that is already closed");
			}
			this.IsClosed = true;
			this.SplineSolver.Close();
		}

		public void Reverse()
		{
			if (!this.IsReversed)
			{
				this.SplineSolver.Reverse();
				this.IsReversed = true;
			}
			else
			{
				this.SplineSolver.Reverse();
				this.IsReversed = false;
			}
		}

		public void OnDrawGizmos()
		{
			if (this.SplineSolver == null)
			{
				return;
			}
			this.SplineSolver.OnInternalDrawGizmos(this.SplineColor, this.DisplayResolution);
		}
	}
}
