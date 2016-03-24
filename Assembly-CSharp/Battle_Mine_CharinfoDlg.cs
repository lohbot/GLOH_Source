using PROTOCOL.GAME;
using System;
using UnityEngine;
using UnityForms;

public class Battle_Mine_CharinfoDlg : Form
{
	private Label[][] m_lbCharName;

	private DrawTexture[][] m_dwBG;

	private Label[] m_lbRemainTurn;

	private eBATTLE_ALLY m_eMyAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;

	private short m_nMYStartPos = -1;

	private long[][] m_nPersonID;

	public int[] m_nEraseTurnCount;

	private GameObject[][] m_goChangeEffect;

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		this.TopMost = true;
		Form form = this;
		instance.LoadFileAll(ref form, "Mine/DLG_mine_charinfo", G_ID.BATTLE_MINE_CHARINFO_DLG, false);
	}

	public override void SetComponent()
	{
		this.m_lbCharName = new Label[2][];
		this.m_dwBG = new DrawTexture[2][];
		this.m_nPersonID = new long[2][];
		this.m_goChangeEffect = new GameObject[2][];
		this.m_lbCharName[0] = new Label[3];
		this.m_lbCharName[1] = new Label[3];
		this.m_dwBG[0] = new DrawTexture[3];
		this.m_dwBG[1] = new DrawTexture[3];
		this.m_nPersonID[0] = new long[3];
		this.m_nPersonID[1] = new long[3];
		this.m_goChangeEffect[0] = new GameObject[3];
		this.m_goChangeEffect[1] = new GameObject[3];
		this.m_lbRemainTurn = new Label[3];
		this.m_nEraseTurnCount = new int[3];
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				string name = string.Format("Label_Ally{0}CharName{1}", i.ToString(), (j + 1).ToString());
				this.m_lbCharName[i][j] = (base.GetControl(name) as Label);
				this.m_nEraseTurnCount[j] = 0;
				name = string.Format("DT_Ally{0}BG{1}", i.ToString(), (j + 1).ToString());
				this.m_dwBG[i][j] = (base.GetControl(name) as DrawTexture);
				NrTSingleton<FormsManager>.Instance.AttachEffectKey("FX_UI_PLAYERCHANGE", this.m_dwBG[i][j], this.m_dwBG[i][j].GetSize());
				this.m_dwBG[i][j].AddGameObjectDelegate(new EZGameObjectDelegate(this.EffectDownLoad));
			}
		}
		this.m_lbRemainTurn[0] = (base.GetControl("Label_Label13") as Label);
		this.m_lbRemainTurn[1] = (base.GetControl("Label_Label14") as Label);
		this.m_lbRemainTurn[2] = (base.GetControl("Label_Label15") as Label);
		this.Hide();
	}

	public override void InitData()
	{
		base.InitData();
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, 0f);
	}

	public override void ChangedResolution()
	{
		base.ChangedResolution();
		base.SetLocation(GUICamera.width / 2f - base.GetSizeX() / 2f, 0f);
	}

	public override void Update()
	{
		base.Update();
		this.ChangeMyCharHPColor();
	}

	public void UPdateTurnInfo()
	{
		for (int i = 0; i < 3; i++)
		{
			int num = 0;
			if (i == 0)
			{
				num = 1;
			}
			else if (i == 1)
			{
				num = 0;
			}
			else if (i == 2)
			{
				num = 2;
			}
			if (this.m_lbRemainTurn[num] != null)
			{
				int num2 = this.m_nEraseTurnCount[num] - Battle.BATTLE.m_nTurnCount;
				if (num2 < 0)
				{
					num2 = 0;
				}
				this.m_lbRemainTurn[num].SetText(num2.ToString());
			}
		}
	}

	public void Set(BATTLE_MINE_CHARINFO pkInfo)
	{
		if (pkInfo == null)
		{
			return;
		}
		if (Battle.BATTLE == null)
		{
			this.Close();
			return;
		}
		if (pkInfo.nAlly < 0 || pkInfo.nAlly >= 2)
		{
			return;
		}
		NrMyCharInfo kMyCharInfo = NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo;
		int nAlly = (int)pkInfo.nAlly;
		int num = 0;
		if (pkInfo.nStartPos == 0)
		{
			num = 1;
		}
		else if (pkInfo.nStartPos == 1)
		{
			num = 0;
		}
		else if (pkInfo.nStartPos == 2)
		{
			num = 2;
		}
		string text = string.Empty;
		if (this.m_nPersonID[nAlly][num] != pkInfo.nPersonID)
		{
			this.m_nEraseTurnCount[num] = pkInfo.nEndTurn;
		}
		if (pkInfo.nPersonID == kMyCharInfo.m_PersonID)
		{
			text = NrTSingleton<CTextParser>.Instance.GetTextColor("1107") + TKString.NEWString(pkInfo.szCharName);
			this.m_eMyAlly = (eBATTLE_ALLY)nAlly;
			this.m_nMYStartPos = (short)pkInfo.nStartPos;
		}
		else
		{
			text = TKString.NEWString(pkInfo.szCharName);
			if (nAlly == (int)this.m_eMyAlly && this.m_nMYStartPos == (short)pkInfo.nStartPos)
			{
				this.m_eMyAlly = eBATTLE_ALLY.eBATTLE_ALLY_INVALID;
				this.m_nMYStartPos = -1;
			}
		}
		this.m_lbCharName[nAlly][num].SetText(text);
		if (this.m_goChangeEffect[nAlly][num] != null)
		{
			if (this.m_goChangeEffect[nAlly][num].activeInHierarchy)
			{
				this.m_goChangeEffect[nAlly][num].SetActive(false);
			}
			this.m_goChangeEffect[nAlly][num].SetActive(true);
		}
		this.m_nPersonID[nAlly][num] = pkInfo.nPersonID;
	}

	public void HiddenName()
	{
		if (!NrTSingleton<NkBattleReplayManager>.Instance.m_bHiddenEnemyName)
		{
			return;
		}
		int num = (Battle.BATTLE.MyAlly != eBATTLE_ALLY.eBATTLE_ALLY_0) ? 0 : 1;
		for (int i = 0; i < this.m_lbCharName[num].Length; i++)
		{
			if (this.m_lbCharName[num][i].GetText() != string.Empty && this.m_lbCharName[num][i].GetText() != string.Empty)
			{
				this.m_lbCharName[num][i].SetText("????");
			}
		}
		if (Battle.BATTLE.MyAlly == eBATTLE_ALLY.eBATTLE_ALLY_INVALID)
		{
			int num2 = (num != 0) ? 0 : 1;
			for (int j = 0; j < this.m_lbCharName[num2].Length; j++)
			{
				if (this.m_lbCharName[num2][j].GetText() != string.Empty && this.m_lbCharName[num2][j].GetText() != string.Empty)
				{
					this.m_lbCharName[num2][j].SetText("????");
				}
			}
		}
	}

	private void ChangeMyCharHPColor()
	{
	}

	public void EffectDownLoad(IUIObject control, GameObject obj)
	{
		if (control == null || null == obj)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (this.m_dwBG[i][j] == control)
				{
					this.m_goChangeEffect[i][j] = obj;
					if (this.m_goChangeEffect[i][j] != null)
					{
						if (i == 0)
						{
							this.m_goChangeEffect[i][j].transform.localScale = new Vector3(-1f, 1f, 1f);
							this.m_goChangeEffect[i][j].transform.localPosition = new Vector3(this.m_dwBG[i][j].width, 0f, 0f);
						}
						else
						{
							this.m_goChangeEffect[i][j].transform.localPosition = new Vector3(this.m_dwBG[i][j].width, 0f, 0f);
						}
						this.m_goChangeEffect[i][j].SetActive(false);
					}
				}
			}
		}
	}
}
