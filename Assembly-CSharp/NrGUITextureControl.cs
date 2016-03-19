using System;
using UnityEngine;

public class NrGUITextureControl : MonoBehaviour
{
	public GUITexture _Texture;

	private Vector2 m_Screen = Vector2.zero;

	private bool bSet;

	private void Start()
	{
		this._Texture = base.gameObject.GetComponent<GUITexture>();
		this.m_Screen.x = (float)Screen.width;
		this.m_Screen.y = (float)Screen.height;
		this._Texture.pixelInset = new Rect(-(this.m_Screen.x / 2f), -(this.m_Screen.y / 2f), this.m_Screen.x, this.m_Screen.y);
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void Update()
	{
		if (this.bSet)
		{
			return;
		}
		if (this.m_Screen.x != (float)Screen.width || this.m_Screen.y != (float)Screen.height)
		{
			this.m_Screen.x = GUICamera.width;
			this.m_Screen.y = GUICamera.height;
			this._Texture.pixelInset = new Rect(-(this.m_Screen.x / 2f), -(this.m_Screen.y / 2f), this.m_Screen.x, this.m_Screen.y);
			this.bSet = true;
		}
	}
}
