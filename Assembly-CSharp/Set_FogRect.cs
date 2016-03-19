using System;
using UnityEngine;

public class Set_FogRect : MonoBehaviour
{
	public Color[] m_Color;

	public Rect[] m_Rect;

	public float Speed;

	public GameObject m_pkUser;

	public int m_CurrentRect;

	public Color m_CurColor = Color.white;

	private Color m_DestColor = Color.white;

	private void Start()
	{
		RenderSettings.fog = true;
		this.m_Color = new Color[8];
		this.m_Rect = new Rect[8];
		this.m_Color[0] = NkUtil.GetColorA(167, 167, 151, 255);
		this.m_Rect[0] = new Rect(0f, 0f, 0f, 0f);
		this.m_Color[1] = NkUtil.GetColorA(130, 151, 167, 255);
		this.m_Rect[1] = new Rect(0f, 0f, 700f, 1600f);
		this.m_Color[2] = NkUtil.GetColorA(167, 130, 130, 255);
		this.m_Rect[2] = new Rect(1400f, 950f, 210f, 300f);
		this.m_Color[3] = NkUtil.GetColorA(167, 130, 130, 255);
		this.m_Rect[3] = new Rect(1270f, 1030f, 300f, 180f);
		this.m_Color[4] = NkUtil.GetColorA(167, 130, 130, 255);
		this.m_Rect[4] = new Rect(1450f, 1200f, 100f, 100f);
		this.m_Color[5] = NkUtil.GetColorA(167, 130, 130, 255);
		this.m_Rect[5] = new Rect(750f, 900f, 280f, 200f);
		this.m_Color[6] = NkUtil.GetColorA(136, 167, 130, 255);
		this.m_Rect[6] = new Rect(700f, 0f, 1350f, 900f);
		this.m_Color[7] = NkUtil.GetColorA(90, 70, 25, 255);
		this.m_Rect[7] = new Rect(800f, 1170f, 700f, 600f);
		this.m_CurColor = this.m_Color[0];
		this.m_DestColor = this.m_Color[0];
		this.m_pkUser = null;
	}

	private void Update()
	{
		if (this.m_pkUser == null)
		{
			this.m_pkUser = NrTSingleton<NkCharManager>.Instance.GetMyCharObject();
			if (this.m_pkUser == null)
			{
				return;
			}
		}
		this.m_CurrentRect = 0;
		int i;
		for (i = 1; i < this.m_Rect.Length; i++)
		{
			Vector2 point = new Vector2(this.m_pkUser.transform.position.x, this.m_pkUser.transform.position.z);
			if (this.m_Rect[i].Contains(point) && this.m_CurrentRect != i)
			{
				this.m_CurrentRect = i;
				this.m_DestColor = this.m_Color[i];
			}
		}
		if (i >= this.m_Rect.Length && this.m_CurrentRect == 0)
		{
			this.m_DestColor = this.m_Color[0];
			this.m_CurrentRect = 0;
		}
		this.m_CurColor.r = Mathf.Lerp(this.m_CurColor.r, this.m_DestColor.r, this.Speed / 100f);
		this.m_CurColor.g = Mathf.Lerp(this.m_CurColor.g, this.m_DestColor.g, this.Speed / 100f);
		this.m_CurColor.b = Mathf.Lerp(this.m_CurColor.b, this.m_DestColor.b, this.Speed / 100f);
		RenderSettings.fogColor = this.m_CurColor;
	}

	protected void OnDisable()
	{
		this.m_pkUser = null;
	}
}
