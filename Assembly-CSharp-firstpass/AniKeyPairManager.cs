using System;
using System.Collections.Generic;

public class AniKeyPairManager
{
	private Dictionary<E_CAMARA_STATE_ANI, AniKeyPair> m_Keys = new Dictionary<E_CAMARA_STATE_ANI, AniKeyPair>();

	public void AddData(E_CAMARA_STATE_ANI _camerastate, string CameraAni, string ActionAni)
	{
		if (!this.m_Keys.ContainsKey(_camerastate))
		{
			AniKeyPair value = new AniKeyPair(_camerastate, CameraAni, ActionAni);
			this.m_Keys.Add(_camerastate, value);
		}
	}

	public AniKeyPair GetData(E_CAMARA_STATE_ANI _camerastate)
	{
		if (this.m_Keys.ContainsKey(_camerastate))
		{
			return this.m_Keys[_camerastate];
		}
		return null;
	}

	public void Clear()
	{
		this.m_Keys.Clear();
	}
}
