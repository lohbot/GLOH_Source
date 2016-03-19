using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class NkIndunManager : NrTSingleton<NkIndunManager>
{
	private Texture2D m_IndunUIBackTexture;

	private bool m_bLoadUIBackTexture;

	private TsWeakReference<INDUN_INFO> m_pkIndunInfo;

	private int m_nIndunUnique = -1;

	public Texture2D IndunUIBackTexture
	{
		get
		{
			return this.m_IndunUIBackTexture;
		}
	}

	public bool LoadUIBackTexture
	{
		get
		{
			return this.m_bLoadUIBackTexture;
		}
		set
		{
			this.m_bLoadUIBackTexture = value;
		}
	}

	public TsWeakReference<INDUN_INFO> IndunInfo
	{
		get
		{
			return this.m_pkIndunInfo;
		}
	}

	private NkIndunManager()
	{
		this.m_IndunUIBackTexture = null;
	}

	public void Clear()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.INDUNTIME_DLG);
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.INDUN_INFO_DLG);
		this.m_IndunUIBackTexture = null;
		this.m_nIndunUnique = -1;
		this.m_pkIndunInfo = null;
		NrTSingleton<NrNpcPosManager>.Instance.ClearIndunExceptMovePos();
	}

	public void SetIndunInfo(int nIndunIDX, int nIndunUnique)
	{
		if (this.m_nIndunUnique == nIndunUnique)
		{
			return;
		}
		this.m_nIndunUnique = nIndunUnique;
		this.m_pkIndunInfo = NrTSingleton<NrBaseTableManager>.Instance.GetIndunInfo(nIndunIDX.ToString());
		this.LoadCurrentIndunUIBackTexture();
	}

	public bool LoadCurrentIndunUIBackTexture()
	{
		if (this.m_pkIndunInfo == null)
		{
			Debug.Log("IndunInfo is NULL");
			return false;
		}
		this.m_bLoadUIBackTexture = false;
		this.m_IndunUIBackTexture = null;
		string str = "UI/Indun";
		string text = string.Empty;
		if (string.Empty != this.m_pkIndunInfo.CastedTarget.IndunImagePath && this.m_pkIndunInfo.CastedTarget.IndunImagePath.ToUpper() != "NULL")
		{
			text = this.m_pkIndunInfo.CastedTarget.IndunImagePath;
		}
		else
		{
			if (!TsPlatform.IsMobile)
			{
				text = "burnpan";
			}
			else
			{
				text = "burnpan_mobile";
			}
			Debug.LogError(string.Format("Fail Load {0}", this.m_pkIndunInfo.CastedTarget.IndunImagePath));
		}
		if (string.Empty != text && 2 < text.Length)
		{
			string path = str + "/" + text;
			if (!NrTSingleton<FormsManager>.Instance.RequestUIBundleDownLoad(path, new PostProcPerItem(this.LoadCompleteIndunUIBackTexture), text))
			{
				return false;
			}
		}
		return true;
	}

	private void LoadCompleteIndunUIBackTexture(WWWItem _item, object _param)
	{
		if (_item != null && _item.canAccessAssetBundle)
		{
			if (null == this.m_IndunUIBackTexture)
			{
				this.m_IndunUIBackTexture = (_item.mainAsset as Texture2D);
			}
			this.m_bLoadUIBackTexture = true;
		}
	}

	public void SetIndunTime(float fTime)
	{
		IndunTime_DLG indunTime_DLG = (IndunTime_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INDUNTIME_DLG);
		indunTime_DLG.SetStayTime(fTime);
	}

	public void SetIndunInfoDlg(float fTime, int nUserNum)
	{
		if (this.m_pkIndunInfo.CastedTarget.m_bShowUI)
		{
			IndunInfo_DLG indunInfo_DLG = (IndunInfo_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INDUN_INFO_DLG);
			if (indunInfo_DLG != null)
			{
				indunInfo_DLG.SetIndunInfo(fTime, nUserNum);
			}
		}
	}

	public void SetResult(int nIndunUnique, bool bWin, eINDUN_CLOSE_REASON eReason, long nRewardGold)
	{
		if (this.m_nIndunUnique != nIndunUnique)
		{
			return;
		}
		if (this.m_pkIndunInfo.CastedTarget.m_bShowUI)
		{
			IndunResult_DLG indunResult_DLG = (IndunResult_DLG)NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.INDUN_RESULT_DLG);
			if (indunResult_DLG != null)
			{
				indunResult_DLG.SetReult(bWin, eReason, nRewardGold);
			}
		}
		this.Clear();
	}

	public void Main_UI_Show()
	{
		IndunTime_DLG indunTime_DLG = (IndunTime_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INDUNTIME_DLG);
		if (indunTime_DLG != null)
		{
			indunTime_DLG.Show();
		}
		IndunInfo_DLG indunInfo_DLG = (IndunInfo_DLG)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.INDUN_INFO_DLG);
		if (indunInfo_DLG != null)
		{
			indunInfo_DLG.Show();
		}
	}
}
