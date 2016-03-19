using System;

public class AniKeyPair
{
	private E_CAMARA_STATE_ANI m_CameraStste;

	private string m_CameraAniKey = string.Empty;

	private string m_ActionAniKey = string.Empty;

	private float m_PlayTime;

	public E_CAMARA_STATE_ANI CameraStste
	{
		get
		{
			return this.m_CameraStste;
		}
	}

	public string CameraAniKey
	{
		get
		{
			return this.m_CameraAniKey;
		}
	}

	public string ActionAniKey
	{
		get
		{
			return this.m_ActionAniKey;
		}
	}

	public float PlayTime
	{
		get
		{
			return this.m_PlayTime;
		}
		set
		{
			this.m_PlayTime = value;
		}
	}

	public AniKeyPair(E_CAMARA_STATE_ANI _camerastate, string CameraAni, string ActionAni)
	{
		this.m_CameraStste = _camerastate;
		this.m_CameraAniKey = CameraAni;
		this.m_ActionAniKey = ActionAni;
	}
}
