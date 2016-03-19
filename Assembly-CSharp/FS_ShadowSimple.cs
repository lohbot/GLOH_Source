using System;
using UnityEngine;

[AddComponentMenu("Fast Shadows/Simple Shadow")]
public class FS_ShadowSimple : MonoBehaviour
{
	[HideInInspector]
	public float maxProjectionDistance = 100f;

	[HideInInspector]
	public float girth = 1f;

	[HideInInspector]
	public float shadowHoverHeight = 0.2f;

	public LayerMask layerMask = -1;

	[HideInInspector]
	public Material shadowMaterial;

	[HideInInspector]
	public bool isStatic;

	[HideInInspector]
	public bool useLightSource;

	[HideInInspector]
	public GameObject lightSource;

	[HideInInspector]
	public Vector3 lightDirection = new Vector3(0f, -1f, 0f);

	[HideInInspector]
	public bool isPerspectiveProjection;

	[HideInInspector]
	public bool doVisibilityCulling;

	[HideInInspector]
	public Rect uvs = new Rect(0f, 0f, 1f, 1f);

	private float _girth;

	private bool _isStatic;

	private Vector3 _lightDirection = Vector3.zero;

	private bool isGoodPlaneIntersect;

	private Color gizmoColor = Color.white;

	private Vector3[] _corners = new Vector3[4];

	private Ray r = default(Ray);

	private RaycastHit rh = default(RaycastHit);

	private Bounds bounds = default(Bounds);

	private Color _color = new Color(1f, 1f, 1f, 0f);

	private Vector3 _normal;

	private GameObject[] cornerGOs = new GameObject[4];

	private GameObject shadowCaster;

	private Plane shadowPlane = default(Plane);

	private FS_MeshKey meshKey;

	public Vector3[] corners
	{
		get
		{
			return this._corners;
		}
	}

	public Color color
	{
		get
		{
			return this._color;
		}
	}

	public Vector3 normal
	{
		get
		{
			return this._normal;
		}
	}

	private void Awake()
	{
		this._isStatic = this.isStatic;
		if (this.shadowMaterial == null)
		{
			this.shadowMaterial = (Material)Resources.Load("FS_ShadowMaterial");
			if (this.shadowMaterial == null)
			{
				Debug.LogWarning("Shadow Material is not set for " + base.name);
			}
		}
		if (this.isStatic)
		{
			this.CalculateShadowGeometry();
		}
	}

	public void CalculateShadowGeometry()
	{
		if (this.shadowMaterial == null)
		{
			return;
		}
		if (this.useLightSource && this.lightSource == null)
		{
			this.useLightSource = false;
			Debug.LogWarning("No light source object given using light direction vector.");
		}
		if (this.useLightSource)
		{
			Vector3 a = base.transform.position - this.lightSource.transform.position;
			float magnitude = a.magnitude;
			if (magnitude == 0f)
			{
				return;
			}
			this.lightDirection = a / magnitude;
		}
		else if (this.lightDirection != this._lightDirection || this.lightDirection == Vector3.zero)
		{
			if (this.lightDirection == Vector3.zero)
			{
				Debug.LogWarning("Light Direction vector cannot be zero. assuming -y.");
				this.lightDirection = -Vector3.up;
			}
			this.lightDirection.Normalize();
			this._lightDirection = this.lightDirection;
		}
		if (this.shadowCaster == null || this.girth != this._girth)
		{
			if (this.shadowCaster == null)
			{
				this.shadowCaster = new GameObject("shadowSimple");
				this.cornerGOs = new GameObject[4];
				for (int i = 0; i < 4; i++)
				{
					GameObject gameObject = this.cornerGOs[i] = new GameObject("c" + i);
					gameObject.transform.parent = this.shadowCaster.transform;
				}
				this.shadowCaster.transform.parent = base.transform;
				this.shadowCaster.transform.localPosition = Vector3.zero;
				this.shadowCaster.transform.localRotation = Quaternion.identity;
				this.shadowCaster.transform.localScale = Vector3.one;
			}
			Vector3 forward;
			if (Mathf.Abs(Vector3.Dot(base.transform.forward, this.lightDirection)) < 0.9f)
			{
				forward = base.transform.forward - Vector3.Dot(base.transform.forward, this.lightDirection) * this.lightDirection;
			}
			else
			{
				forward = base.transform.up - Vector3.Dot(base.transform.up, this.lightDirection) * this.lightDirection;
			}
			this.shadowCaster.transform.rotation = Quaternion.LookRotation(forward, -this.lightDirection);
			this.cornerGOs[0].transform.position = this.shadowCaster.transform.position + this.girth * (this.shadowCaster.transform.forward - this.shadowCaster.transform.right);
			this.cornerGOs[1].transform.position = this.shadowCaster.transform.position + this.girth * (this.shadowCaster.transform.forward + this.shadowCaster.transform.right);
			this.cornerGOs[2].transform.position = this.shadowCaster.transform.position + this.girth * (-this.shadowCaster.transform.forward + this.shadowCaster.transform.right);
			this.cornerGOs[3].transform.position = this.shadowCaster.transform.position + this.girth * (-this.shadowCaster.transform.forward - this.shadowCaster.transform.right);
			this._girth = this.girth;
		}
		Transform transform = this.shadowCaster.transform;
		this.r.origin = transform.position;
		this.r.direction = this.lightDirection;
		if (this.maxProjectionDistance > 0f && Physics.Raycast(this.r, out this.rh, this.maxProjectionDistance, this.layerMask))
		{
			if (this.doVisibilityCulling && !this.isPerspectiveProjection)
			{
				Plane[] cameraFustrumPlanes = FS_ShadowManager.Manager().getCameraFustrumPlanes();
				this.bounds.center = this.rh.point;
				this.bounds.size = new Vector3(2f * this.girth, 2f * this.girth, 2f * this.girth);
				if (!GeometryUtility.TestPlanesAABB(cameraFustrumPlanes, this.bounds))
				{
					return;
				}
			}
			Vector3 forward;
			if (Mathf.Abs(Vector3.Dot(base.transform.forward, this.lightDirection)) < 0.9f)
			{
				forward = base.transform.forward - Vector3.Dot(base.transform.forward, this.lightDirection) * this.lightDirection;
			}
			else
			{
				forward = base.transform.up - Vector3.Dot(base.transform.up, this.lightDirection) * this.lightDirection;
			}
			this.shadowCaster.transform.rotation = Quaternion.Lerp(this.shadowCaster.transform.rotation, Quaternion.LookRotation(forward, -this.lightDirection), 0.01f);
			float num = this.rh.distance - this.shadowHoverHeight;
			float num2 = 1f - num / this.maxProjectionDistance;
			if (num2 < 0f)
			{
				return;
			}
			num2 = Mathf.Clamp01(num2);
			this._color.a = num2;
			this._normal = this.rh.normal;
			Vector3 inPoint = this.rh.point - this.shadowHoverHeight * this.lightDirection;
			this.shadowPlane.SetNormalAndPosition(this._normal, inPoint);
			this.isGoodPlaneIntersect = true;
			if (this.useLightSource && this.isPerspectiveProjection)
			{
				this.r.origin = this.lightSource.transform.position;
				Vector3 a2 = this.cornerGOs[0].transform.position - this.lightSource.transform.position;
				float magnitude2 = a2.magnitude;
				this.r.direction = a2 / magnitude2;
				float num3;
				this.isGoodPlaneIntersect = (this.isGoodPlaneIntersect && this.shadowPlane.Raycast(this.r, out num3));
				this._corners[0] = this.r.origin + this.r.direction * num3;
				a2 = this.cornerGOs[1].transform.position - this.lightSource.transform.position;
				this.r.direction = a2 / magnitude2;
				this.isGoodPlaneIntersect = (this.isGoodPlaneIntersect && this.shadowPlane.Raycast(this.r, out num3));
				this._corners[1] = this.r.origin + this.r.direction * num3;
				a2 = this.cornerGOs[2].transform.position - this.lightSource.transform.position;
				this.r.direction = a2 / magnitude2;
				this.isGoodPlaneIntersect = (this.isGoodPlaneIntersect && this.shadowPlane.Raycast(this.r, out num3));
				this._corners[2] = this.r.origin + this.r.direction * num3;
				a2 = this.cornerGOs[3].transform.position - this.lightSource.transform.position;
				this.r.direction = a2 / magnitude2;
				this.isGoodPlaneIntersect = (this.isGoodPlaneIntersect && this.shadowPlane.Raycast(this.r, out num3));
				this._corners[3] = this.r.origin + this.r.direction * num3;
				if (this.doVisibilityCulling)
				{
					Plane[] cameraFustrumPlanes2 = FS_ShadowManager.Manager().getCameraFustrumPlanes();
					this.bounds.center = this.rh.point;
					this.bounds.size = Vector3.zero;
					this.bounds.Encapsulate(this._corners[0]);
					this.bounds.Encapsulate(this._corners[1]);
					this.bounds.Encapsulate(this._corners[2]);
					this.bounds.Encapsulate(this._corners[3]);
					if (!GeometryUtility.TestPlanesAABB(cameraFustrumPlanes2, this.bounds))
					{
						return;
					}
				}
			}
			else
			{
				this.r.origin = this.cornerGOs[0].transform.position;
				float num3;
				this.isGoodPlaneIntersect = this.shadowPlane.Raycast(this.r, out num3);
				if (!this.isGoodPlaneIntersect && num3 == 0f)
				{
					return;
				}
				this.isGoodPlaneIntersect = true;
				this._corners[0] = this.r.origin + this.r.direction * num3;
				this.r.origin = this.cornerGOs[1].transform.position;
				this.isGoodPlaneIntersect = this.shadowPlane.Raycast(this.r, out num3);
				if (!this.isGoodPlaneIntersect && num3 == 0f)
				{
					return;
				}
				this.isGoodPlaneIntersect = true;
				this._corners[1] = this.r.origin + this.r.direction * num3;
				this.r.origin = this.cornerGOs[2].transform.position;
				this.isGoodPlaneIntersect = this.shadowPlane.Raycast(this.r, out num3);
				if (!this.isGoodPlaneIntersect && num3 == 0f)
				{
					return;
				}
				this.isGoodPlaneIntersect = true;
				this._corners[2] = this.r.origin + this.r.direction * num3;
				this.r.origin = this.cornerGOs[3].transform.position;
				this.isGoodPlaneIntersect = this.shadowPlane.Raycast(this.r, out num3);
				if (!this.isGoodPlaneIntersect && num3 == 0f)
				{
					return;
				}
				this.isGoodPlaneIntersect = true;
				this._corners[3] = this.r.origin + this.r.direction * num3;
			}
			if (this.isGoodPlaneIntersect)
			{
				if (this.meshKey == null || this.meshKey.mat != this.shadowMaterial || this.meshKey.isStatic != this.isStatic)
				{
					this.meshKey = new FS_MeshKey(this.shadowMaterial, this.isStatic);
				}
				FS_ShadowManager.Manager().registerGeometry(this, this.meshKey);
				this.gizmoColor = Color.white;
			}
			else
			{
				this.gizmoColor = Color.magenta;
			}
		}
		else
		{
			this.isGoodPlaneIntersect = false;
			this.gizmoColor = Color.red;
		}
	}

	private void Update()
	{
		if (this._isStatic != this.isStatic)
		{
			if (this.isStatic)
			{
				this.meshKey = new FS_MeshKey(this.shadowMaterial, this.isStatic);
				this.CalculateShadowGeometry();
				FS_ShadowManager.Manager().RecalculateStaticGeometry(null, this.meshKey);
			}
			else
			{
				FS_MeshKey fS_MeshKey = this.meshKey;
				this.meshKey = new FS_MeshKey(this.shadowMaterial, this.isStatic);
				FS_ShadowManager.Manager().RecalculateStaticGeometry(this, fS_MeshKey);
			}
			this._isStatic = this.isStatic;
		}
		if (!this.isStatic)
		{
			this.CalculateShadowGeometry();
		}
	}

	private void OnDrawGizmos()
	{
		if (this.shadowCaster != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(this.shadowCaster.transform.position, this.shadowCaster.transform.up);
			Gizmos.DrawRay(this.shadowCaster.transform.position, this.shadowCaster.transform.forward);
			Gizmos.DrawRay(this.shadowCaster.transform.position, this.shadowCaster.transform.right);
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(this.shadowCaster.transform.position, base.transform.forward);
			Gizmos.color = this.gizmoColor;
			if (this.isGoodPlaneIntersect)
			{
				Gizmos.DrawLine(this.cornerGOs[0].transform.position, this.corners[0]);
				Gizmos.DrawLine(this.cornerGOs[1].transform.position, this.corners[1]);
				Gizmos.DrawLine(this.cornerGOs[2].transform.position, this.corners[2]);
				Gizmos.DrawLine(this.cornerGOs[3].transform.position, this.corners[3]);
				Gizmos.DrawLine(this.cornerGOs[0].transform.position, this.cornerGOs[1].transform.position);
				Gizmos.DrawLine(this.cornerGOs[1].transform.position, this.cornerGOs[2].transform.position);
				Gizmos.DrawLine(this.cornerGOs[2].transform.position, this.cornerGOs[3].transform.position);
				Gizmos.DrawLine(this.cornerGOs[3].transform.position, this.cornerGOs[0].transform.position);
				Gizmos.DrawLine(this.corners[0], this.corners[1]);
				Gizmos.DrawLine(this.corners[1], this.corners[2]);
				Gizmos.DrawLine(this.corners[2], this.corners[3]);
				Gizmos.DrawLine(this.corners[3], this.corners[0]);
			}
		}
	}
}
