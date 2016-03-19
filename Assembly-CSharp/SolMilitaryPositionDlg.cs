using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SolMilitaryPositionDlg : Form
{
	private Label BattlePosSolName;

	private DrawTexture BattlePosSolWeapon1;

	private DrawTexture BattlePosSolWeapon2;

	private DrawTexture[] BattlePosSolImage;

	private UIRadioBtn[] BattlePosSolSelect;

	private DrawTexture BattlePosSelectedPos;

	private Button BattlePosReturn;

	private Button BattlePosApply;

	private float fBattlePosSlotSize;

	private float fBattlePosSlotSpaceX;

	private float fBattlePosSlotSpaceY;

	private short nSelectedBattelPos = -1;

	private float fBattlePosStartX;

	private float fBattlePosStartY;

	private int nCurrentMilitaryUnique;

	private NkSoldierInfo[] m_kBattlePosSolList = new NkSoldierInfo[6];

	public override void InitializeComponent()
	{
		UIBaseFileManager instance = NrTSingleton<UIBaseFileManager>.Instance;
		Form form = this;
		form.Scale = true;
		instance.LoadFileAll(ref form, "Soldier/dlg_position", G_ID.SOLMILITARYPOSITION_DLG, true);
		if (TsPlatform.IsMobile)
		{
			base.ShowBlackBG(0.5f);
		}
	}

	public override void SetComponent()
	{
		this.BattlePosSolName = (base.GetControl("Label_position_chaname") as Label);
		this.BattlePosSolWeapon1 = (base.GetControl("DrawTexture_position_weapon01") as DrawTexture);
		this.BattlePosSolWeapon2 = (base.GetControl("DrawTexture_position_weapon02") as DrawTexture);
		this.BattlePosSolImage = new DrawTexture[9];
		this.BattlePosSolSelect = new UIRadioBtn[9];
		for (int i = 0; i < 9; i++)
		{
			string str = (i + 1).ToString();
			this.BattlePosSolImage[i] = (base.GetControl("DrawTexture_sol_0" + str) as DrawTexture);
			this.BattlePosSolImage[i].SetLocation(this.BattlePosSolImage[i].GetLocationX(), this.BattlePosSolImage[i].GetLocationY(), -0.11f);
			Vector2 size = this.BattlePosSolImage[i].GetSize();
			this.BattlePosSolSelect[i] = UICreateControl.RadioBtn(this, "BattlePosSolSelect0" + str, string.Empty, size.x, size.y);
			this.BattlePosSolSelect[i].Data = i;
			this.BattlePosSolSelect[i].Layer = 3;
			this.BattlePosSolSelect[i].SetLocation(this.BattlePosSolImage[i].GetLocationX(), this.BattlePosSolImage[i].GetLocationY(), -0.12f);
			((UIButton)this.BattlePosSolSelect[i].layers[0]).SetLocation(this.BattlePosSolImage[i].GetLocationX(), this.BattlePosSolImage[i].GetLocationY(), -0.13f);
			this.BattlePosSolSelect[i].SetValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBattlePosSolSelect));
		}
		this.BattlePosSelectedPos = (base.GetControl("DrawTexture_selectsol") as DrawTexture);
		this.BattlePosSelectedPos.Visible = false;
		this.BattlePosReturn = (base.GetControl("btn_return") as Button);
		this.BattlePosReturn.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBattlePosCancel));
		this.BattlePosApply = (base.GetControl("btn_Apply") as Button);
		this.BattlePosApply.AddValueChangedDelegate(new EZValueChangedDelegate(this.OnClickBattlePosChange));
		this.fBattlePosSlotSize = this.BattlePosSolImage[0].GetSize().x;
		this.fBattlePosSlotSpaceX = this.BattlePosSolImage[1].GetLocationX() - (this.BattlePosSolImage[0].GetLocationX() + this.fBattlePosSlotSize);
		this.fBattlePosSlotSpaceY = this.BattlePosSolImage[3].GetLocationY() - (this.BattlePosSolImage[0].GetLocationY() + this.BattlePosSolImage[0].GetSize().y);
		this.fBattlePosStartX = this.BattlePosSolImage[0].GetLocationX();
		this.fBattlePosStartY = this.BattlePosSolImage[0].GetLocationY();
		if (!this.MakeMilitary())
		{
			base.CloseNow();
		}
		base.SetScreenCenter();
		this.InitBattlePosInfo();
		this.InitData();
	}

	public override void InitData()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "FORMATION", "OPEN", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}

	public void SetClearData()
	{
		this.nSelectedBattelPos = -1;
		this.BattlePosSolName.Text = string.Empty;
		this.BattlePosSolWeapon1.Visible = false;
		this.BattlePosSolWeapon2.Visible = false;
		this.BattlePosSelectedPos.Visible = false;
		for (int i = 0; i < 6; i++)
		{
			this.m_kBattlePosSolList[i] = new NkSoldierInfo();
		}
		NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
		if (charPersonInfo == null)
		{
			return;
		}
		for (int j = 0; j < 6; j++)
		{
			NkSoldierInfo soldierInfo = charPersonInfo.GetSoldierInfo(j);
			if (soldierInfo != null && soldierInfo.IsValid())
			{
				this.m_kBattlePosSolList[(int)soldierInfo.GetSolPosIndex()].Set(soldierInfo);
			}
		}
	}

	private void InitSelectedSolinfo()
	{
		this.SetClearData();
	}

	private void InitBattlePosInfo()
	{
		this.SetClearData();
		this.ShowBattlePosInfo();
	}

	private void ShowBattlePosInfo()
	{
		for (int i = 0; i < 9; i++)
		{
			this.BattlePosSolSelect[i].Visible = true;
			this.BattlePosSolSelect[i].SetSize(this.fBattlePosSlotSize, this.fBattlePosSlotSize);
			this.BattlePosSolSelect[i].SetTexture("Com_I_Transparent");
			((UIButton)this.BattlePosSolSelect[i].layers[0]).Visible = true;
			((UIButton)this.BattlePosSolSelect[i].layers[0]).SetSize(this.fBattlePosSlotSize, this.fBattlePosSlotSize);
			this.BattlePosSolImage[i].Visible = false;
			float num = (float)(i % 3) * this.fBattlePosSlotSize + (float)(i % 3) * this.fBattlePosSlotSpaceX;
			float num2 = (float)(i / 3) * this.fBattlePosSlotSize + (float)(i / 3) * this.fBattlePosSlotSpaceY;
			this.BattlePosSolImage[i].SetLocation(this.fBattlePosStartX + num, this.fBattlePosStartY + num2);
		}
		for (int j = 0; j < 6; j++)
		{
			NkSoldierInfo nkSoldierInfo = this.m_kBattlePosSolList[j];
			if (nkSoldierInfo != null && nkSoldierInfo.IsValid())
			{
				if (nkSoldierInfo.GetBattlePos() >= 0)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
					if (charKindInfo != null)
					{
						float num3 = (float)charKindInfo.GetBattleSizeX() * this.fBattlePosSlotSize;
						float num4 = (float)charKindInfo.GetBattleSizeY() * this.fBattlePosSlotSize;
						if (num3 > this.fBattlePosSlotSize || num4 > this.fBattlePosSlotSize)
						{
							this.BattlePosSolSelect[(int)nkSoldierInfo.GetBattlePos()].SetSize(num3, num4);
							((UIButton)this.BattlePosSolSelect[(int)nkSoldierInfo.GetBattlePos()].layers[0]).SetSize(num3, num4);
							if (num3 > this.fBattlePosSlotSize)
							{
								for (int k = 1; k < (int)charKindInfo.GetBattleSizeX(); k++)
								{
									this.BattlePosSolSelect[(int)nkSoldierInfo.GetBattlePos() + k].Visible = false;
								}
								float x = this.fBattlePosSlotSize / 2f * (float)(charKindInfo.GetBattleSizeX() - 1);
								this.BattlePosSolImage[(int)nkSoldierInfo.GetBattlePos()].MoveLocation(x, 0f);
							}
							if (num4 > this.fBattlePosSlotSize)
							{
								for (int l = 1; l < (int)charKindInfo.GetBattleSizeY(); l++)
								{
									this.BattlePosSolSelect[(int)nkSoldierInfo.GetBattlePos() + l * 3].Visible = false;
								}
								float y = this.fBattlePosSlotSize / 2f * (float)(charKindInfo.GetBattleSizeY() - 1);
								this.BattlePosSolImage[(int)nkSoldierInfo.GetBattlePos()].MoveLocation(0f, y);
							}
						}
						this.BattlePosSolImage[(int)nkSoldierInfo.GetBattlePos()].Visible = true;
						this.BattlePosSolImage[(int)nkSoldierInfo.GetBattlePos()].SetTexture(eCharImageType.SMALL, nkSoldierInfo.GetCharKind(), (int)nkSoldierInfo.GetGrade());
					}
				}
			}
		}
	}

	private bool IsValidChangeBattlePos()
	{
		BATTLE_POS_GRID info = BASE_BATTLE_POS_Manager.GetInstance().GetInfo(0);
		if (info == null)
		{
			return false;
		}
		BATTLE_POS_GRID bATTLE_POS_GRID = new BATTLE_POS_GRID();
		bATTLE_POS_GRID.Set(info, 0);
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo nkSoldierInfo = this.m_kBattlePosSolList[i];
			if (nkSoldierInfo != null && nkSoldierInfo.IsValid())
			{
				if (nkSoldierInfo.GetBattlePos() >= 0)
				{
					NrCharKindInfo charKindInfo = NrTSingleton<NrCharKindInfoManager>.Instance.GetCharKindInfo(nkSoldierInfo.GetCharKind());
					if (charKindInfo != null)
					{
						if (!BASE_BATTLE_POS_Manager.GetInstance().IsEnablePos(charKindInfo, nkSoldierInfo.GetBattlePos(), bATTLE_POS_GRID))
						{
							return false;
						}
						bATTLE_POS_GRID.SetBUID((short)nkSoldierInfo.GetSolPosIndex(), (byte)nkSoldierInfo.GetBattlePos(), charKindInfo.GetBattleSizeX(), charKindInfo.GetBattleSizeY());
					}
				}
			}
		}
		return true;
	}

	private void OnClickBattlePosSolSelect(IUIObject obj)
	{
		UIRadioBtn uIRadioBtn = (UIRadioBtn)obj;
		if (uIRadioBtn == null || !uIRadioBtn.Value)
		{
			return;
		}
		int num = (int)uIRadioBtn.Data;
		short num2 = (short)num;
		if (num2 == this.nSelectedBattelPos)
		{
			return;
		}
		if (this.nSelectedBattelPos < 0)
		{
			this.nSelectedBattelPos = num2;
			bool flag = false;
			for (int i = 0; i < 6; i++)
			{
				NkSoldierInfo nkSoldierInfo = this.m_kBattlePosSolList[i];
				if (nkSoldierInfo != null && nkSoldierInfo.IsValid())
				{
					if (this.nSelectedBattelPos == nkSoldierInfo.GetBattlePos())
					{
						string empty = string.Empty;
						NrTSingleton<CTextParser>.Instance.ReplaceParam(ref empty, new object[]
						{
							NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("1469"),
							"count",
							nkSoldierInfo.GetLevel().ToString()
						});
						this.BattlePosSolName.Text = string.Format("{0}({1})", nkSoldierInfo.GetName(), empty);
						int num3 = nkSoldierInfo.GetEquipWeaponOrigin();
						this.BattlePosSolWeapon1.Visible = (num3 > 0);
						if (num3 > 0)
						{
							this.BattlePosSolWeapon1.SetTexture(string.Format("Win_I_Weapon{0}", num3.ToString()));
						}
						num3 = nkSoldierInfo.GetEquipWeaponExtention();
						this.BattlePosSolWeapon2.Visible = (num3 > 0);
						if (num3 > 0)
						{
							this.BattlePosSolWeapon2.SetTexture(string.Format("Win_I_Weapon{0}", num3.ToString()));
						}
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.BattlePosSolName.Text = string.Empty;
			}
			if (this.nSelectedBattelPos >= 0 && this.nSelectedBattelPos < 9)
			{
				this.BattlePosSelectedPos.SetLocation(this.BattlePosSolSelect[(int)this.nSelectedBattelPos].GetLocationX(), this.BattlePosSolSelect[(int)this.nSelectedBattelPos].GetLocationY(), -0.14f);
				this.BattlePosSelectedPos.SetSize(this.BattlePosSolSelect[(int)this.nSelectedBattelPos].GetSize().x, this.BattlePosSolSelect[(int)this.nSelectedBattelPos].GetSize().y);
				this.BattlePosSelectedPos.Visible = true;
			}
			return;
		}
		NkSoldierInfo nkSoldierInfo2 = null;
		NkSoldierInfo nkSoldierInfo3 = null;
		short battlePos = -1;
		short battlePos2 = -1;
		for (int j = 0; j < 6; j++)
		{
			NkSoldierInfo nkSoldierInfo = this.m_kBattlePosSolList[j];
			if (nkSoldierInfo != null && nkSoldierInfo.IsValid())
			{
				if (nkSoldierInfo.GetBattlePos() >= 0)
				{
					if (num2 == nkSoldierInfo.GetBattlePos())
					{
						nkSoldierInfo3 = nkSoldierInfo;
					}
					else if (this.nSelectedBattelPos == nkSoldierInfo.GetBattlePos())
					{
						nkSoldierInfo2 = nkSoldierInfo;
					}
				}
			}
		}
		if (nkSoldierInfo2 == null && nkSoldierInfo3 == null)
		{
			this.nSelectedBattelPos = -1;
			uIRadioBtn.Value = false;
			this.BattlePosSelectedPos.Visible = false;
			return;
		}
		if (nkSoldierInfo2 != null)
		{
			battlePos = nkSoldierInfo2.GetBattlePos();
			nkSoldierInfo2.SetBattlePos(num2);
		}
		if (nkSoldierInfo3 != null)
		{
			battlePos2 = nkSoldierInfo3.GetBattlePos();
			nkSoldierInfo3.SetBattlePos(this.nSelectedBattelPos);
		}
		if (!this.IsValidChangeBattlePos())
		{
			if (nkSoldierInfo2 != null)
			{
				nkSoldierInfo2.SetBattlePos(battlePos);
			}
			if (nkSoldierInfo3 != null)
			{
				nkSoldierInfo3.SetBattlePos(battlePos2);
			}
		}
		this.ShowBattlePosInfo();
		this.nSelectedBattelPos = -1;
		uIRadioBtn.Value = false;
		this.BattlePosSelectedPos.Visible = false;
	}

	private bool MakeMilitary()
	{
		if (NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.GetMilitaryList() == null)
		{
			return false;
		}
		this.nCurrentMilitaryUnique = 0;
		this.InitSelectedSolinfo();
		return true;
	}

	private void OnClickBattlePosCancel(IUIObject obj)
	{
		this.InitBattlePosInfo();
		this.ShowBattlePosInfo();
	}

	private void OnClickBattlePosChange(IUIObject obj)
	{
		GS_SOLDIER_CHANGE_BATTLEPOS_REQ gS_SOLDIER_CHANGE_BATTLEPOS_REQ = new GS_SOLDIER_CHANGE_BATTLEPOS_REQ();
		gS_SOLDIER_CHANGE_BATTLEPOS_REQ.MilitaryUnique = (byte)this.nCurrentMilitaryUnique;
		for (int i = 0; i < 6; i++)
		{
			NkSoldierInfo nkSoldierInfo = this.m_kBattlePosSolList[i];
			if (nkSoldierInfo == null || !nkSoldierInfo.IsValid())
			{
				gS_SOLDIER_CHANGE_BATTLEPOS_REQ.SolID[i] = 0L;
				gS_SOLDIER_CHANGE_BATTLEPOS_REQ.BattlePos[i] = -1;
			}
			else
			{
				gS_SOLDIER_CHANGE_BATTLEPOS_REQ.SolID[i] = nkSoldierInfo.GetSolID();
				gS_SOLDIER_CHANGE_BATTLEPOS_REQ.BattlePos[i] = (int)nkSoldierInfo.GetBattlePos();
			}
		}
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_SOLDIER_CHANGE_BATTLEPOS_REQ, gS_SOLDIER_CHANGE_BATTLEPOS_REQ);
		base.CloseNow();
	}

	public override void OnClose()
	{
		TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "FORMATION", "CLOSE", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
	}
}
