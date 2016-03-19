using StageHelper;
using System;
using UnityEngine;
using UnityForms;

public class Battle_Input : InputCommandLayer
{
	private TsWeakReference<Battle> m_Battle;

	private Vector3 m_veScrollStart = Vector3.zero;

	private TsLayerMask mc_kBattlePickLayer = TsLayer.PC_DECORATION;

	public Battle_Input(Battle pkBattle)
	{
		this.m_Battle = pkBattle;
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
		else
		{
			if (this.m_veScrollStart != Vector3.zero)
			{
				this.m_veScrollStart = Vector3.zero;
				this.m_Battle.CastedTarget.BattleCamera.SetScrollView(false);
			}
			if (NkInputManager.GetMouseButtonUp(0) || NkInputManager.GetMouseButtonUp(2))
			{
				this.m_Battle.CastedTarget.SetCameraMove(flag);
			}
		}
		this.GridInputMouse();
		return false;
	}

	public void InputKeyBoard()
	{
		if (this.m_Battle.CastedTarget == null)
		{
			return;
		}
		if (this.m_Battle.CastedTarget.InputControlTrigger)
		{
			return;
		}
		if (NrTSingleton<UIManager>.Instance.FocusObject != null)
		{
			return;
		}
		if (!TsPlatform.IsEditor)
		{
			return;
		}
		NkBattleChar currentSelectChar = this.m_Battle.CastedTarget.GetCurrentSelectChar();
		if (currentSelectChar != null && NkInputManager.GetKeyUp(KeyCode.Alpha1))
		{
			int battleSkillUnique = currentSelectChar.GetSoldierInfo().SelectBattleSkillByWeapon(1);
			if (this.m_Battle.CastedTarget.CanSelecActionBattleSkill(battleSkillUnique))
			{
				this.m_Battle.CastedTarget.m_iBattleSkillIndex = 1;
				this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_SKILL;
				this.m_Battle.CastedTarget.ShowBattleSkillRange(true, battleSkillUnique);
			}
		}
		if (NkInputManager.GetKeyUp(KeyCode.R) && this.m_Battle.CastedTarget.CurrentTurnAlly == this.m_Battle.CastedTarget.MyAlly)
		{
			this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS;
			Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("364"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
		}
		if (NkInputManager.GetKeyUp(KeyCode.Tab) && this.m_Battle.CastedTarget.CurrentTurnAlly == this.m_Battle.CastedTarget.MyAlly)
		{
			this.m_Battle.CastedTarget.SelectNextChar();
		}
		if (NkInputManager.GetKeyUp(KeyCode.Z))
		{
			this.m_Battle.CastedTarget.Send_GS_BF_HOPE_TO_ENDTURN_REQ();
		}
		if (NkInputManager.GetKeyUp(KeyCode.X))
		{
			Battle_Control_Dlg battle_Control_Dlg = NrTSingleton<FormsManager>.Instance.GetForm(G_ID.BATTLE_CONTROL_DLG) as Battle_Control_Dlg;
			if (battle_Control_Dlg == null)
			{
				return;
			}
			battle_Control_Dlg.RequestRetreat();
		}
		if (NkInputManager.GetKeyUp(KeyCode.V) && TsPlatform.IsEditor)
		{
			this.m_Battle.CastedTarget.Send_GS_BATTLE_AUTO_REQ();
		}
		if (NkInputManager.GetKeyUp(KeyCode.C))
		{
		}
		if (!NkInputManager.GetKey(KeyCode.LeftShift) || NkInputManager.GetKeyUp(KeyCode.M))
		{
		}
		if (!NkInputManager.GetKey(KeyCode.LeftShift) || NkInputManager.GetKeyUp(KeyCode.N))
		{
		}
	}

	private void InputMouse()
	{
		if (this.m_Battle.CastedTarget == null)
		{
			return;
		}
		if (this.m_Battle.CastedTarget.InputControlTrigger)
		{
			return;
		}
		if (this.m_Battle.CastedTarget.BattleCamera == null)
		{
			return;
		}
		if (this.m_veScrollStart == Vector3.zero)
		{
			this.m_veScrollStart = NkInputManager.mousePosition;
			this.m_Battle.CastedTarget.BattleCamera.SetScrollView(true);
		}
		else
		{
			Vector3 vector = this.m_veScrollStart - NkInputManager.mousePosition;
			this.m_Battle.CastedTarget.BattleCamera.CameraScrollMove(vector.x, vector.y);
			this.m_veScrollStart = NkInputManager.mousePosition;
			if (vector.x != 0f || vector.y != 0f)
			{
				this.m_Battle.CastedTarget.SetCameraMove(true);
			}
		}
	}

	private void GridInputMouse()
	{
		if (this.m_Battle.CastedTarget == null)
		{
			return;
		}
		if (this.m_Battle.CastedTarget.InputControlTrigger)
		{
			return;
		}
		if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
		{
			return;
		}
		if (!this.m_Battle.CastedTarget.IsEnableMouseInput())
		{
			return;
		}
		NmBattleGrid nmBattleGrid = null;
		bool mouseButtonUp = NkInputManager.GetMouseButtonUp(0);
		bool mouseButtonUp2 = NkInputManager.GetMouseButtonUp(1);
		if (NkRaycast.Raycast(this.mc_kBattlePickLayer))
		{
			GameObject gameObject = NkRaycast.HIT.transform.gameObject;
			if (null != gameObject)
			{
				nmBattleGrid = gameObject.GetComponent<NmBattleGrid>();
				if (null != nmBattleGrid)
				{
					eBATTLE_ALLY aLLY = nmBattleGrid.ALLY;
					short sTARTPOS_INDEX = nmBattleGrid.STARTPOS_INDEX;
					int iNDEX = nmBattleGrid.INDEX;
					short bUID = nmBattleGrid.BUID;
					NkBattleChar charByBUID = NrTSingleton<NkBattleCharManager>.Instance.GetCharByBUID(bUID);
					Vector3 pOINT = NkRaycast.POINT;
					BATTLESKILL_BASE bATTLESKILL_BASE = null;
					int num = 0;
					if (this.m_Battle.CastedTarget.m_iBattleSkillIndex >= 0 && this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
					{
						NkBattleChar nkBattleChar = this.m_Battle.CastedTarget.SelectBattleSkillChar();
						if (nkBattleChar == null)
						{
							return;
						}
						int skillUnique = nkBattleChar.GetSoldierInfo().SelectBattleSkillByWeapon(this.m_Battle.CastedTarget.m_iBattleSkillIndex);
						bATTLESKILL_BASE = NrTSingleton<BattleSkill_Manager>.Instance.GetBattleSkillBase(skillUnique);
						if (bATTLESKILL_BASE == null)
						{
							return;
						}
						this.m_Battle.CastedTarget.GRID_MANAGER.SetSelectBattleSkillGrid();
						if (bATTLESKILL_BASE.m_nSkillTargetType == 1 || bATTLESKILL_BASE.m_nSkillTargetType == 2)
						{
							num = 1;
						}
						else if (bATTLESKILL_BASE.m_nSkillTargetType == 3)
						{
							num = 2;
						}
						else if (bATTLESKILL_BASE.m_nSkillTargetType == 4)
						{
							num = 3;
						}
					}
					if (this.m_Battle.CastedTarget.MyAlly != aLLY)
					{
						if (bATTLESKILL_BASE != null)
						{
							if ((num == 2 || num == 3) && bUID >= 0)
							{
								this.m_Battle.CastedTarget.GRID_MANAGER.ActiveBattleSkillGrid(aLLY, sTARTPOS_INDEX, iNDEX, bATTLESKILL_BASE.m_nSkillUnique);
							}
						}
						if (mouseButtonUp)
						{
							if (this.m_Battle.CastedTarget.IsEmotionSet)
							{
								this.m_Battle.CastedTarget.Send_GS_BATTLE_EMOTICON_REQ(bUID);
							}
							else if (this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_ATTACK_LAND)
							{
								this.m_Battle.CastedTarget.Send_AttackLand_Order(pOINT);
								this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
							}
							else if (this.m_Battle.CastedTarget.m_iBattleSkillIndex >= 0 && this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
							{
								if (num == 2 || num == 3)
								{
									this.m_Battle.CastedTarget.GRID_MANAGER.ActiveBattleSkillGrid(aLLY, sTARTPOS_INDEX, iNDEX, bATTLESKILL_BASE.m_nSkillUnique);
									this.m_Battle.CastedTarget.Send_BattleSkill_Order(this.m_Battle.CastedTarget.m_iBattleSkillIndex, this.m_Battle.CastedTarget.SelectBattleSkillChar(), charByBUID, pOINT, (short)iNDEX);
									this.m_Battle.CastedTarget.Init_BattleSkill_Input(false);
									this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
								}
								else
								{
									Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("578"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
								}
							}
							else if (this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_SEARCH)
							{
								NkBattleChar currentSelectChar = this.m_Battle.CastedTarget.GetCurrentSelectChar();
								if (currentSelectChar != null)
								{
								}
								this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
							}
							else
							{
								if (Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_PLUNDER || Battle.BATTLE.BattleRoomtype == eBATTLE_ROOMTYPE.eBATTLE_ROOMTYPE_INFINITY)
								{
									if (Battle.BATTLE.GetCheckTargetBt() && charByBUID != null && charByBUID.GetSoldierInfo().GetHP() > 0 && charByBUID.GetCharKindInfo().GetCharKind() != 916)
									{
										Battle.BATTLE.Send_GS_BATTLE_PLUNDER_AGGROADD_REQ(charByBUID.GetBUID());
										Battle.BATTLE.ClickCheckTargetBt();
										Battle.BATTLE.SetTargetBtDisCount();
										NrTSingleton<NkEffectManager>.Instance.AddEffect("FX_ATTACK_TARGET", charByBUID);
									}
								}
								else
								{
									NkBattleChar currentSelectChar2 = this.m_Battle.CastedTarget.GetCurrentSelectChar();
									if (currentSelectChar2 != null && charByBUID != null)
									{
										this.m_Battle.CastedTarget.GRID_MANAGER.ActiveAttack(aLLY, sTARTPOS_INDEX, iNDEX, charByBUID);
										if (charByBUID.GetSoldierInfo().GetHP() > 0)
										{
											currentSelectChar2.OrderAttackReq(charByBUID, (short)iNDEX, nmBattleGrid.GetCenter());
										}
									}
								}
								this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
							}
						}
					}
					else
					{
						if (bATTLESKILL_BASE != null && (num == 1 || num == 3) && bUID >= 0)
						{
							this.m_Battle.CastedTarget.GRID_MANAGER.ActiveBattleSkillGrid(aLLY, sTARTPOS_INDEX, iNDEX, bATTLESKILL_BASE.m_nSkillUnique);
						}
						if (mouseButtonUp)
						{
							if (this.m_Battle.CastedTarget.IsEmotionSet)
							{
								this.m_Battle.CastedTarget.Send_GS_BATTLE_EMOTICON_REQ(bUID);
							}
							else if (this.m_Battle.CastedTarget.m_iBattleSkillIndex >= 0 && this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
							{
								if (num == 1 || num == 3)
								{
									this.m_Battle.CastedTarget.GRID_MANAGER.ActiveBattleSkillGrid(aLLY, sTARTPOS_INDEX, iNDEX, bATTLESKILL_BASE.m_nSkillUnique);
									this.m_Battle.CastedTarget.Send_BattleSkill_Order(this.m_Battle.CastedTarget.m_iBattleSkillIndex, this.m_Battle.CastedTarget.SelectBattleSkillChar(), charByBUID, pOINT, (short)iNDEX);
									this.m_Battle.CastedTarget.Init_BattleSkill_Input(false);
									this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
								}
							}
							else if (this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS)
							{
								NkBattleChar currentSelectChar3 = this.m_Battle.CastedTarget.GetCurrentSelectChar();
								if (currentSelectChar3 != null)
								{
									int num2 = currentSelectChar3.OrderChangePosReq(sTARTPOS_INDEX, (short)iNDEX);
									if (num2 < 0)
									{
										if (num2 < -1)
										{
											Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("176"), SYSTEM_MESSAGE_TYPE.NAGATIVE_MESSAGE);
										}
										else
										{
											Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("404"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
										}
									}
								}
								this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
							}
							else if (charByBUID != null)
							{
								if (charByBUID.GetTurnState() == eBATTLE_TURN_STATE.eBATTLE_TURN_STATE_ENABLE)
								{
									this.m_Battle.CastedTarget.SelectCharacter(bUID);
								}
								this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
							}
							else
							{
								this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
							}
						}
						else if (this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS)
						{
							NkBattleChar currentSelectChar4 = this.m_Battle.CastedTarget.GetCurrentSelectChar();
							if (currentSelectChar4 != null)
							{
								this.m_Battle.CastedTarget.GRID_MANAGER.ActiveChangePos(nmBattleGrid.ALLY, nmBattleGrid.STARTPOS_INDEX, currentSelectChar4.GetBUID(), (short)iNDEX);
							}
						}
						else
						{
							this.m_Battle.CastedTarget.GRID_MANAGER.SetOver(nmBattleGrid);
						}
					}
				}
			}
		}
		if (mouseButtonUp2)
		{
			if (this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_CHANGEPOS)
			{
				this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
				Main_UI_SystemMessage.ADDMessage(NrTSingleton<NrTextMgr>.Instance.GetTextFromNotify("402"), SYSTEM_MESSAGE_TYPE.IMPORTANT_MESSAGE);
			}
			else if (this.m_Battle.CastedTarget.REQUEST_ORDER == eBATTLE_ORDER.eBATTLE_ORDER_SKILL)
			{
				this.m_Battle.CastedTarget.REQUEST_ORDER = eBATTLE_ORDER.eBATTLE_ORDER_NONE;
				this.m_Battle.CastedTarget.Init_BattleSkill_Input(true);
			}
		}
		if (null == nmBattleGrid)
		{
			this.m_Battle.CastedTarget.GRID_MANAGER.SetOver(null);
			if (this.m_Battle.CastedTarget.m_iBattleSkillIndex < 0 && this.m_Battle.CastedTarget.REQUEST_ORDER != eBATTLE_ORDER.eBATTLE_ORDER_SKILL && this.m_Battle.CastedTarget.GetCurrentSelectChar() == null)
			{
				this.m_Battle.CastedTarget.GRID_MANAGER.InitAll();
			}
		}
	}
}
