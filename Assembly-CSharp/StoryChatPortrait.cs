using Ndoors.Memory;
using System;
using UnityEngine;
using UnityForms;

public class StoryChatPortrait
{
	public long m_i64PersonID;

	public Texture2D m_PortraitTexutre;

	public StoryChatPortrait()
	{
		this.m_i64PersonID = 0L;
		this.m_PortraitTexutre = null;
	}

	public void Set(long i64PersonID, bool bReFresh)
	{
		this.m_i64PersonID = i64PersonID;
		if (this.m_i64PersonID > 0L && this.m_i64PersonID > 11L)
		{
			string userPortraitURL = NrTSingleton<NkCharManager>.Instance.GetUserPortraitURL(i64PersonID);
			WebFileCache.RequestImageWebFile(userPortraitURL, new WebFileCache.ReqTextureCallback(this.ReqWebUserStoryChatImageCallback), this.m_i64PersonID);
		}
	}

	public void SetTexture(long i64PersonID, Texture2D _Texture)
	{
		this.m_i64PersonID = i64PersonID;
		if (this.m_i64PersonID > 0L && this.m_i64PersonID > 11L)
		{
			this.m_PortraitTexutre = _Texture;
		}
	}

	private void ReqWebUserStoryChatImageCallback(Texture2D txtr, object _param)
	{
		long i64PersonID = (long)_param;
		if (txtr != null)
		{
			this.m_PortraitTexutre = txtr;
			if (NrTSingleton<FormsManager>.Instance.IsForm(G_ID.STORYCHAT_DLG))
			{
				StoryChatDlg storyChatDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.STORYCHAT_DLG) as StoryChatDlg;
				storyChatDlg.UpdateUserPersonID(i64PersonID);
			}
		}
		else
		{
			this.m_PortraitTexutre = null;
		}
	}
}
