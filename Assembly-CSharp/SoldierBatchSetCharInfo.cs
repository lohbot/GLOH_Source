using System;
using UnityEngine;
using UnityForms;

public class SoldierBatchSetCharInfo
{
	public GameObject m_goChar;

	private long m_SetSolID;

	public long m_FriendPersonID;

	public int m_FriendCharKind;

	public byte m_nObjectid;

	public long m_SolID
	{
		get
		{
			return this.m_SetSolID;
		}
		set
		{
			this.m_SetSolID = value;
			PlunderSolNumDlg plunderSolNumDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.PLUNDERSOLNUM_DLG) as PlunderSolNumDlg;
			if (plunderSolNumDlg != null)
			{
				plunderSolNumDlg.SetSeleteSol(this.m_SetSolID);
			}
		}
	}

	public void Init()
	{
		if (this.m_goChar != null)
		{
			UnityEngine.Object.Destroy(this.m_goChar);
			this.m_goChar = null;
		}
		this.m_SolID = 0L;
		this.m_FriendPersonID = 0L;
		this.m_FriendCharKind = 0;
		this.m_nObjectid = 0;
	}
}
