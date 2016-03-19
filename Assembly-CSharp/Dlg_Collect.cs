using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class Dlg_Collect : Form
{
	private enum eANISTATE
	{
		eANISTATE_STARTANI,
		eANISTATE_DECIDEANI
	}

	private Box m_bxCollectingTime;

	private int m_CharUnique;

	public UIBaseInfoLoader imgLoder;

	public bool m_bSendPacket;

	private float m_BarValue;

	private Dlg_Collect.eANISTATE m_eAnitype;

	private float m_BarAniStartTime;

	private float m_StartAniValue;

	private float m_MaxTime = 3f;

	private float m_StartTime;

	private float m_BarChangeTime;

	private float m_BarWaitingTime;

	private float m_MaxBarValue = 1f;

	private float m_BarTotalWidth;

	private bool m_bChangeState;

	private bool m_bBarWaiting;

	private float m_MovingBarValue;

	private bool m_bValue;

	private bool m_bSuccess;

	private float m_fDistanceFromUser;

	public int CharUnique
	{
		get
		{
			return this.m_CharUnique;
		}
		set
		{
			this.m_CharUnique = value;
		}
	}

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "Collect/DLG_Collect", G_ID.DLG_COLLECT, true);
	}

	public override void SetComponent()
	{
		this.m_bxCollectingTime = (base.GetControl("ProgressBar_collectbarYellow") as Box);
		this.m_BarTotalWidth = this.m_bxCollectingTime.width;
		this.m_bxCollectingTime.SetSize(1f, this.m_bxCollectingTime.height);
		this.m_bSendPacket = false;
		this.m_fDistanceFromUser = 0f;
		base.SetLocation((GUICamera.width - base.GetSizeX()) / 2f, GUICamera.height - 150f);
	}

	public void CollectStart(int charunique, int posx, int posy, bool success)
	{
		NrCharObject nrCharObject = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique((short)charunique) as NrCharObject;
		if (nrCharObject == null)
		{
			return;
		}
		if (nrCharObject.GetCharKindInfo() == null)
		{
			return;
		}
		this.m_CharUnique = charunique;
		this.m_StartTime = Time.realtimeSinceStartup;
		this.m_BarAniStartTime = Time.realtimeSinceStartup;
		this.m_bChangeState = true;
		this.m_bSuccess = success;
		this.m_fDistanceFromUser = NrTSingleton<NkCharManager>.Instance.GetDistanceCharPos(1, nrCharObject.GetID());
	}

	public void StartBarAni()
	{
		float num = Time.realtimeSinceStartup - this.m_StartTime;
		float num2 = Time.realtimeSinceStartup - this.m_BarAniStartTime;
		float startAniValue = num2 * (1f / this.m_MaxTime);
		this.m_StartAniValue = startAniValue;
		if (this.m_StartAniValue >= 1f)
		{
			this.m_StartAniValue = 1f;
		}
		if (this.m_StartAniValue >= 1f && num >= this.m_MaxTime)
		{
			this.m_eAnitype = Dlg_Collect.eANISTATE.eANISTATE_DECIDEANI;
		}
		this.m_bxCollectingTime.SetSize(this.m_BarTotalWidth * this.m_StartAniValue, this.m_bxCollectingTime.height);
	}

	public void MovingBarWidth()
	{
		float num = Time.realtimeSinceStartup - this.m_StartTime;
		float num2 = Time.realtimeSinceStartup - this.m_BarChangeTime;
		float num3 = num2 / 10f;
		if (this.m_bBarWaiting)
		{
			float num4 = Time.realtimeSinceStartup - this.m_BarWaitingTime;
			if (num4 >= 1f)
			{
				this.m_bBarWaiting = false;
				this.m_bChangeState = true;
			}
		}
		else if (this.m_bChangeState)
		{
			if (num <= this.m_MaxTime)
			{
				this.m_MovingBarValue += 0.1f;
				if (this.m_MovingBarValue >= 0.9f)
				{
					this.m_MovingBarValue = 0.9f;
				}
			}
			else if (this.m_bSuccess)
			{
				this.m_MovingBarValue = this.m_MaxBarValue;
			}
			else
			{
				this.m_MovingBarValue = this.m_MaxBarValue;
			}
			this.m_BarChangeTime = Time.realtimeSinceStartup;
			if (this.m_MovingBarValue >= this.m_BarValue)
			{
				this.m_bValue = true;
			}
			else
			{
				this.m_bValue = false;
			}
			this.m_bChangeState = false;
		}
		else if (this.m_bValue)
		{
			if (this.m_MovingBarValue >= this.m_BarValue)
			{
				this.m_BarValue += num3;
			}
			else
			{
				this.m_bBarWaiting = true;
				this.m_BarWaitingTime = Time.realtimeSinceStartup;
			}
		}
		else if (this.m_MovingBarValue <= this.m_BarValue)
		{
			this.m_BarValue -= num3;
		}
		else
		{
			this.m_bBarWaiting = true;
			this.m_BarWaitingTime = Time.realtimeSinceStartup;
		}
		this.m_bxCollectingTime.SetSize(this.m_BarTotalWidth * this.m_BarValue, this.m_bxCollectingTime.height);
	}

	public override void Update()
	{
		if (!this.m_bSendPacket)
		{
			if (this.m_eAnitype == Dlg_Collect.eANISTATE.eANISTATE_STARTANI)
			{
				this.StartBarAni();
			}
			else if (this.m_eAnitype == Dlg_Collect.eANISTATE.eANISTATE_DECIDEANI)
			{
				GS_COLLECT_FINISH_REQ gS_COLLECT_FINISH_REQ = new GS_COLLECT_FINISH_REQ();
				gS_COLLECT_FINISH_REQ.i32CharUnique = this.m_CharUnique;
				SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLLECT_FINISH_REQ, gS_COLLECT_FINISH_REQ);
				this.m_bSendPacket = true;
			}
			NrCharBase charByCharUnique = NrTSingleton<NkCharManager>.Instance.GetCharByCharUnique((short)this.m_CharUnique);
			float num;
			if (charByCharUnique == null)
			{
				num = 10f;
			}
			else
			{
				num = NrTSingleton<NkCharManager>.Instance.GetDistanceCharPos(1, charByCharUnique.GetID());
			}
			if (Mathf.Abs(this.m_fDistanceFromUser - num) > 1f)
			{
				this.OnClose();
			}
		}
	}

	public override void OnClose()
	{
		if (!this.m_bSendPacket && this.m_StartAniValue < 1f)
		{
			GS_COLLECT_CANCEL_REQ gS_COLLECT_CANCEL_REQ = new GS_COLLECT_CANCEL_REQ();
			gS_COLLECT_CANCEL_REQ.i32CharUnique = this.m_CharUnique;
			SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_COLLECT_CANCEL_REQ, gS_COLLECT_CANCEL_REQ);
			this.m_bSendPacket = true;
		}
	}
}
