using StageHelper;
using System;
using TsBundle;
using UnityEngine;
using UnityForms;

public class SoldierBatch_Input : InputCommandLayer
{
	private TsWeakReference<SoldierBatch> m_SoldierBatch;

	private Vector3 m_veScrollStart = Vector3.zero;

	private TsLayerMask mc_kPlunderPickLayer = TsLayer.PC_DECORATION;

	public SoldierBatch_Input(SoldierBatch pkPlunderPrepare)
	{
		this.m_SoldierBatch = pkPlunderPrepare;
	}

	public override bool Update(INPUT_INFO curInput)
	{
		if (!CommonTasks.IsEndOfPrework)
		{
			return false;
		}
		this.InputKeyBoard();
		bool flag;
		if (TsPlatform.IsMobile && !TsPlatform.IsEditor)
		{
			flag = (curInput.evt == INPUT_INFO.INPUT_EVENT.DRAG && Input.touchCount == 1);
		}
		else
		{
			flag = (curInput.evt == INPUT_INFO.INPUT_EVENT.DRAG);
		}
		if (flag)
		{
			this.InputMouse();
		}
		else if (this.m_veScrollStart != Vector3.zero)
		{
			this.m_veScrollStart = Vector3.zero;
			this.m_SoldierBatch.CastedTarget.CAMERA.SetScrollView(false);
		}
		return false;
	}

	public void InputKeyBoard()
	{
	}

	private void InputMouse()
	{
		if (this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID != 0L)
		{
			return;
		}
		bool flag = false;
		if (NkInputManager.GetMouseButton(0) || NkInputManager.GetMouseButton(2))
		{
			flag = true;
		}
		if (NkInputManager.GetMouseButtonUp(0) || NkInputManager.GetMouseButtonUp(2))
		{
			flag = false;
		}
		if (flag)
		{
			if (this.m_veScrollStart == Vector3.zero)
			{
				this.m_veScrollStart = NkInputManager.mousePosition;
				this.m_SoldierBatch.CastedTarget.CAMERA.SetScrollView(true);
			}
			else
			{
				Vector3 vector = this.m_veScrollStart - NkInputManager.mousePosition;
				this.m_SoldierBatch.CastedTarget.CAMERA.CameraScrollMove(vector.x, vector.y);
				this.m_veScrollStart = NkInputManager.mousePosition;
			}
		}
		if (!flag)
		{
			this.m_veScrollStart = Vector3.zero;
			this.m_SoldierBatch.CastedTarget.CAMERA.SetScrollView(false);
		}
	}

	public void GridInputMouse()
	{
		bool mouseButtonUp = NkInputManager.GetMouseButtonUp(0);
		bool mouseButtonDown = NkInputManager.GetMouseButtonDown(0);
		if (mouseButtonDown && NrTSingleton<UIManager>.Instance.DragUpUI)
		{
			return;
		}
		if (mouseButtonUp && NrTSingleton<UIManager>.Instance.DragUpUI)
		{
			this.m_SoldierBatch.CastedTarget.InitSelectMoveChar(this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind);
			this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.Init();
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_CHAT) != null && NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BABELTOWER_CHAT).Visible)
		{
			return;
		}
		if (this.m_SoldierBatch.CastedTarget.IsMessageBox)
		{
			return;
		}
		SoldierBatchGrid soldierBatchGrid = null;
		if (NkRaycast.Raycast(this.mc_kPlunderPickLayer))
		{
			GameObject gameObject = NkRaycast.HIT.transform.gameObject;
			if (null != gameObject)
			{
				soldierBatchGrid = gameObject.GetComponent<SoldierBatchGrid>();
				if (null != soldierBatchGrid)
				{
					eBATTLE_ALLY aLLY = soldierBatchGrid.ALLY;
					short sTARTPOS_INDEX = soldierBatchGrid.STARTPOS_INDEX;
					int iNDEX = soldierBatchGrid.INDEX;
					long solID = soldierBatchGrid.SolID;
					long personID = soldierBatchGrid.PersonID;
					int charKind = soldierBatchGrid.CharKind;
					byte objID = soldierBatchGrid.ObjID;
					if (aLLY == eBATTLE_ALLY.eBATTLE_ALLY_0)
					{
						soldierBatchGrid.SetMODE(E_RENDER_MODE.ACTIVE_SELECT);
					}
					if (mouseButtonDown)
					{
						if (aLLY != eBATTLE_ALLY.eBATTLE_ALLY_0)
						{
							return;
						}
						if (solID != 0L && this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID == 0L)
						{
							this.SetMakeUpChar(solID, personID, charKind, objID);
						}
					}
					else if (mouseButtonUp)
					{
						if (aLLY != eBATTLE_ALLY.eBATTLE_ALLY_0)
						{
							if (this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID != 0L)
							{
								this.m_SoldierBatch.CastedTarget.InitCharBattlePos(this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind);
							}
							this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.Init();
							return;
						}
						if (solID != 0L)
						{
							if (this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID != solID)
							{
								if (this.m_SoldierBatch.CastedTarget.EnableChangePos(this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, solID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind) && !this.m_SoldierBatch.CastedTarget.ChangePos(this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, solID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind))
								{
									this.m_SoldierBatch.CastedTarget.InitSelectMoveChar(this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind);
								}
							}
							else
							{
								this.m_SoldierBatch.CastedTarget.InitSelectMoveChar(solID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind);
							}
							this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.Init();
						}
						else
						{
							if (!this.m_SoldierBatch.CastedTarget.InsertEmptyGrid((byte)sTARTPOS_INDEX, (byte)iNDEX, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_nObjectid))
							{
								this.m_SoldierBatch.CastedTarget.InitSelectMoveChar(this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind);
							}
							this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.Init();
						}
					}
				}
			}
		}
		else if (mouseButtonUp)
		{
			if (this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID != 0L)
			{
				this.m_SoldierBatch.CastedTarget.InitCharBattlePos(this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID, this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind);
			}
			this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.Init();
		}
		this.m_SoldierBatch.CastedTarget.SelectGrid = soldierBatchGrid;
	}

	public void SetMakeUpChar(long SolID, long nFriendPersonID, int nFriendCharKind, byte nObjectID)
	{
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_BABEL_TOWER || SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_MYTHRAID)
		{
			NrPersonInfoUser charPersonInfo = NrTSingleton<NkCharManager>.Instance.GetCharPersonInfo(1);
			if (SoldierBatch.BABELTOWER_INFO.Count != 1 && charPersonInfo.GetSoldierInfoFromSolID(SolID) == null)
			{
				return;
			}
		}
		this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_SolID = SolID;
		this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendPersonID = nFriendPersonID;
		this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_FriendCharKind = nFriendCharKind;
		this.m_SoldierBatch.CastedTarget.MakeUpCharInfo.m_nObjectid = nObjectID;
		if (SoldierBatch.SOLDIER_BATCH_MODE == eSOLDIER_BATCH_MODE.MODE_PVP_MAKEUP2)
		{
			ColosseumCardSettingDlg colosseumCardSettingDlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.COLOSSEUM_SETTING_CARD_DLG) as ColosseumCardSettingDlg;
			if (colosseumCardSettingDlg == null)
			{
				colosseumCardSettingDlg = (NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.COLOSSEUM_SETTING_CARD_DLG) as ColosseumCardSettingDlg);
			}
			colosseumCardSettingDlg.SetSolInfo((int)SolID);
		}
		GameObject charFromSolID = this.m_SoldierBatch.CastedTarget.GetCharFromSolID(SolID);
		if (charFromSolID != null)
		{
			TsPositionFollowerTerrain component = charFromSolID.GetComponent<TsPositionFollowerTerrain>();
			if (component != null)
			{
				component.enabled = true;
				TsAudioManager.Instance.AudioContainer.RequestAudioClip("UI_SFX", "PLUNDER", "HERO-OUT", new PostProcPerItem(NrAudioClipDownloaded.OnEventAudioClipDownloadedImmedatePlay));
			}
		}
	}
}
