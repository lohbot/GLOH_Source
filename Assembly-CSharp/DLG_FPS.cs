using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class DLG_FPS : Form
{
	private Label m_lbFPS;

	private Label m_lbSoldierPos;

	private Label m_lbDUN;

	private Label m_lbTotal;

	private Label m_lbScene;

	private Label m_lbMonster;

	private Label m_lbNPC;

	private Label m_lbUser;

	private Label m_lbMax;

	private Label m_lbCurrent;

	private Label m_lbTotal_Quest;

	private Label m_lbPing;

	private Label m_lbTotalTime;

	private Label m_lbRealTime;

	private Label m_lbUseNetTime;

	private Label m_lbUseNetSize;

	public float updateInterval = 0.5f;

	private double lastInterval;

	private float frames;

	private float fps;

	private GameObject m_UserObject;

	private float sendtime;

	public float m_fFpsSec;

	public int m_dwGSTick;

	public int m_dwWSTick;

	private int MaxUser;

	private int CurUser;

	private int Monster;

	private int NPC;

	private int User;

	private long TotalTime;

	private long RealTime;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		instance.LoadFileAll(ref form, "dlg_FPS", G_ID.DLG_FPS, true);
		this.sendtime = Time.time;
		this.sendtime += 1f;
	}

	public override void SetComponent()
	{
		this.m_lbFPS = (base.GetControl("Label_Label4") as Label);
		this.m_lbSoldierPos = (base.GetControl("Label_Label4_C") as Label);
		this.m_lbDUN = (base.GetControl("Label_Label2") as Label);
		this.m_UserObject = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMyCharObject();
		NrCharMapInfo kCharMapInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo;
		this.m_lbDUN.Text = string.Concat(new object[]
		{
			"MAP INDEX: ",
			kCharMapInfo.m_nMapIndex,
			" MAP UNIQUE:",
			kCharMapInfo.MapUnique,
			" INDUN UNIQUE:",
			NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_nIndunUnique,
			"\r\n",
			Screen.width.ToString(),
			" ",
			Screen.height.ToString()
		});
		this.m_lbTotal = (base.GetControl("Label_Total") as Label);
		this.m_lbScene = (base.GetControl("Label_Scene") as Label);
		this.m_lbPing = (base.GetControl("Label_ping") as Label);
		this.m_lbTotalTime = (base.GetControl("Label_TotalTime") as Label);
		this.m_lbRealTime = (base.GetControl("Label_RealTime") as Label);
		this.m_lbMonster = (base.GetControl("Label_Mob") as Label);
		this.m_lbNPC = (base.GetControl("Label_NPC") as Label);
		this.m_lbUser = (base.GetControl("Label_User") as Label);
		this.m_lbMax = (base.GetControl("Label_Max") as Label);
		this.m_lbCurrent = (base.GetControl("Label_Current") as Label);
		this.m_lbTotal_Quest = (base.GetControl("Label_Total_Quest") as Label);
		this.m_lbUseNetTime = (base.GetControl("Label_UseNetTime") as Label);
		this.m_lbUseNetSize = (base.GetControl("Label_UseNetSize") as Label);
	}

	public override void Update()
	{
		base.Update();
		this.frames += 1f;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if ((double)realtimeSinceStartup > this.lastInterval + (double)this.updateInterval)
		{
			this.fps = (float)((double)this.frames / ((double)realtimeSinceStartup - this.lastInterval));
			this.frames = 0f;
			this.lastInterval = (double)realtimeSinceStartup;
		}
		if (Battle.BATTLE == null)
		{
			string text = string.Format("PING : {0}ms GS : {1} WS : {2}", this.m_fFpsSec * 1000f, this.m_dwGSTick, this.m_dwWSTick);
			this.m_lbPing.Text = text;
		}
		else
		{
			string text2 = string.Format("PING : {0}ms GS : {1} WS : {2} Order : {3} CInfo : {4}", new object[]
			{
				this.m_fFpsSec * 1000f,
				this.m_dwGSTick,
				this.m_dwWSTick,
				Battle.BATTLE.m_fOrderPing,
				Battle.BATTLE.m_fInfoPing
			});
			this.m_lbPing.Text = text2;
		}
		this.m_lbFPS.Text = "FPS: " + this.fps.ToString("f2");
		if (this.m_UserObject != null)
		{
			this.m_lbSoldierPos.Text = this.m_UserObject.transform.position.ToString() + " _ANGLES:" + this.m_UserObject.transform.localEulerAngles.y.ToString();
		}
		else
		{
			this.m_lbSoldierPos.Text = string.Empty;
		}
		float num = (float)NrTSingleton<NrGlobalReference>.Instance.GetDownloadInfo().m_si32TotalSizeDownloaded;
		num /= 1048576f;
		float num2 = (float)NrTSingleton<NrGlobalReference>.Instance.GetDownloadInfo().m_si32SceneSizeDownloaded;
		num2 /= 1048576f;
		this.m_lbTotal.Text = "TotalDownloadSize : " + num.ToString("N2") + "MB";
		string text3 = string.Format("SceneDownloadSize : {0}MB", num2.ToString("N2"));
		text3 = string.Format("{0}\nAppMemory : {1}MB", text3, NrTSingleton<NrMainSystem>.Instance.CurAppMemory);
		this.m_lbScene.Text = text3;
		this.m_lbTotalTime.Text = "Total Play Time = " + PublicMethod.ConvertTime(this.TotalTime);
		this.m_lbRealTime.Text = "Real Play Time = " + PublicMethod.ConvertTime(this.RealTime);
		this.m_lbMonster.Text = "Monster : " + this.Monster.ToString();
		this.m_lbNPC.Text = "NPC: " + this.NPC.ToString();
		this.m_lbUser.Text = "User: " + this.User.ToString();
		this.m_lbMax.Text = "MaxUser : " + this.MaxUser.ToString();
		this.m_lbCurrent.Text = "CurUser: " + this.CurUser.ToString();
		long time = (long)(Time.realtimeSinceStartup - PublicMethod.CLIENTTIME);
		this.m_lbUseNetTime.Text = "Using Net Time = " + PublicMethod.ConvertTime(time) + " sec";
		long num3 = SendPacket.GetInstance().GetTotalSendPacketSize();
		num3 += BaseNet_Game.GetInstance().GetTotalReceivePacketSize();
		this.m_lbUseNetSize.Text = "Using Net Size = " + string.Format("{0:###,###,###,##0}", num3) + " bytes";
		if (Time.time >= this.sendtime)
		{
			this.sendtime += 1f;
			if (NrTSingleton<NkClientLogic>.Instance.IsWorldScene())
			{
				NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
				if (@char != null)
				{
					GS_SERVER_CHARINFO_REQ gS_SERVER_CHARINFO_REQ = new GS_SERVER_CHARINFO_REQ();
					gS_SERVER_CHARINFO_REQ.siCharUnique = @char.GetCharUnique();
					gS_SERVER_CHARINFO_REQ.bTimeOnly = false;
					SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SERVER_CHARINFO_REQ, gS_SERVER_CHARINFO_REQ);
				}
			}
		}
	}

	public void SetUserInfo(int curuser, int maxuser, long TTime, long RTime)
	{
		this.MaxUser = maxuser;
		this.CurUser = curuser;
		this.TotalTime = TTime;
		this.RealTime = RTime;
	}

	public void SetMapCharInfo(int monstercnt, int npccnt, int usercnt)
	{
		this.Monster = monstercnt;
		this.NPC = npccnt;
		this.User = usercnt;
	}

	public override void Show()
	{
		this.m_lbTotal_Quest.Text = "QuestTotalCount : " + NrTSingleton<NkQuestManager>.Instance.GetTotalQuestCount().ToString();
		NrTSingleton<NrMainSystem>.Instance.m_bPingTest = true;
		base.Show();
	}

	public override void OnClose()
	{
		NrTSingleton<NrMainSystem>.Instance.m_bPingTest = false;
	}
}
