using System;
using System.Collections.Generic;
using UnityEngine;

public class TsShadowLodManager : MonoBehaviour
{
	private static TsShadowLodManager ms_Instance;

	public float m_Distance = 10f;

	private float m_SqrDist;

	private List<Light> m_Lights = new List<Light>();

	private bool m_bIsUpdateTime = true;

	private float m_Delay;

	[SerializeField]
	private Transform m_Target;

	public static TsShadowLodManager Instance
	{
		get
		{
			if (TsShadowLodManager.ms_Instance == null)
			{
				TsShadowLodManager.ms_Instance = (UnityEngine.Object.FindObjectOfType(typeof(TsShadowLodManager)) as TsShadowLodManager);
				if (TsShadowLodManager.ms_Instance == null)
				{
					Camera main = Camera.main;
					if (main != null)
					{
						TsShadowLodManager.ms_Instance = main.gameObject.AddComponent<TsShadowLodManager>();
					}
					else
					{
						GameObject gameObject = new GameObject("@TsShadowLodManager");
						TsShadowLodManager.ms_Instance = gameObject.AddComponent<TsShadowLodManager>();
					}
				}
			}
			return TsShadowLodManager.ms_Instance;
		}
	}

	public float sqrMagnitude
	{
		get
		{
			return this.m_SqrDist;
		}
	}

	public float magnitude
	{
		get
		{
			return this.m_Distance;
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			return this.m_Target.position;
		}
	}

	public bool IsUpdateTime
	{
		get
		{
			return this.m_bIsUpdateTime;
		}
	}

	private void Awake()
	{
		this.m_SqrDist = this.m_Distance * this.m_Distance;
	}

	private void Start()
	{
		if (TsShadowLodManager.ms_Instance != this)
		{
			TsShadowLodManager.ms_Instance = this;
		}
		if (this.m_Target == null)
		{
			this.m_Target = Camera.main.transform;
		}
		this.Refresh();
	}

	private void _CollectLights()
	{
		this.m_Lights.Clear();
		GameObject[] array = GameObject.FindGameObjectsWithTag(TsTag.LIGHT.ToString());
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			Light light = gameObject.light;
			if (light != null && light.type != LightType.Directional && light.shadows != LightShadows.None)
			{
				this.m_Lights.Add(light);
			}
		}
	}

	private void OnDisable()
	{
		this._CollectLights();
		foreach (Light current in this.m_Lights)
		{
			GameObject gameObject = current.gameObject;
			MonoBehaviour component = gameObject.GetComponent<TsShadowLOD>();
			if (component)
			{
				component.enabled = false;
				current.shadows = LightShadows.None;
			}
		}
		this.m_Lights.Clear();
	}

	private void OnEnable()
	{
		this._CollectLights();
		foreach (Light current in this.m_Lights)
		{
			GameObject gameObject = current.gameObject;
			MonoBehaviour component = gameObject.GetComponent<TsShadowLOD>();
			if (component)
			{
				component.enabled = true;
				current.shadows = LightShadows.None;
			}
		}
		this.m_Lights.Clear();
	}

	private void Refresh()
	{
		this._CollectLights();
		foreach (Light current in this.m_Lights)
		{
			GameObject gameObject = current.gameObject;
			if (gameObject.GetComponent<TsShadowLOD>() == null)
			{
				gameObject.AddComponent("TsShadowLOD");
				gameObject.light.shadows = LightShadows.None;
			}
		}
		this.m_Lights.Clear();
	}

	public LightShadows GetLightShadows()
	{
		return TsQualityManager.Instance.CurrQuality.ShadowType;
	}

	private void FixedUpdate()
	{
		this.m_Delay += Time.deltaTime;
		if (this.m_Delay > 2f)
		{
			this.m_Delay -= 2f;
			this.m_bIsUpdateTime = true;
		}
		else
		{
			this.m_bIsUpdateTime = false;
		}
	}
}
