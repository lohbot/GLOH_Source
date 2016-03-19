using Ndoors.Framework.Stage;
using NPatch;
using System;
using UnityEngine;
using UnityForms;

public class CharMoveCommandLayer : InputCommandLayer
{
	public enum CharKeyMoveState
	{
		MOVE_FORWARD = 1,
		MOVE_BACK,
		MOVE_LEFTTURN = 4,
		MOVE_RIGHTTURN = 8
	}

	protected sbyte CurCharKeyMoveStatus;

	protected sbyte PrevCharKeyMoveStatus;

	private float fSafeClickTime;

	protected ushort m_ushDebugLevel;

	public static bool bDragMove;

	private NrCharUser pkDragMoveChar;

	private Vector3 vMoveDir = Vector3.zero;

	private bool bMouseCharMove;

	public bool preRunState = true;

	public CharMoveCommandLayer()
	{
		this.InitKeyboardMove();
		if (TsPlatform.IsWeb)
		{
			base.AddKeyInputDelegate(new KeyInputDelegate(this.MyCharMoveKeyboard));
			base.AddBothPressInputDelegate(new InputDelegate(this.BothCharMove));
			base.AddTapInputDelegate(new InputDelegate(this.TabCharMove));
			base.AddNoChangeInputDelegate(new InputDelegate(this.NoChangeMove));
		}
		else if (TsPlatform.IsMobile)
		{
			base.AddTapInputDelegate(new InputDelegate(this.TabCharMove));
		}
	}

	public override void InitCommandLayer()
	{
		this.InitKeyboardMove();
	}

	public void InitKeyboardMove()
	{
		this.CurCharKeyMoveStatus = 0;
		this.PrevCharKeyMoveStatus = 0;
		this.InitDragMove();
	}

	public void InitDragMove()
	{
		CharMoveCommandLayer.bDragMove = false;
		this.pkDragMoveChar = null;
	}

	public void StartDragMove()
	{
		CharMoveCommandLayer.bDragMove = true;
		this.pkDragMoveChar = (NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser);
	}

	public void SetMoveDir(ref Vector3 dir)
	{
		this.vMoveDir = dir;
	}

	public override bool Update(INPUT_INFO curInput)
	{
		if (!this.PreCheckUpdate())
		{
			return false;
		}
		base.ExcuteKeyInputDelegate();
		INPUT_INFO.INPUT_EVENT evt = curInput.evt;
		switch (evt)
		{
		case INPUT_INFO.INPUT_EVENT.NO_CHANGE:
			this.NoChangeMove();
			return false;
		case INPUT_INFO.INPUT_EVENT.PRESS:
			return false;
		case INPUT_INFO.INPUT_EVENT.DOUBLE_PRESS:
		case INPUT_INFO.INPUT_EVENT.RELEASE:
			return false;
		case INPUT_INFO.INPUT_EVENT.MIDDLE_PRESS:
		case INPUT_INFO.INPUT_EVENT.RIGHT_PRESS:
		case INPUT_INFO.INPUT_EVENT.HOLD_PRESS:
		case INPUT_INFO.INPUT_EVENT.RIGHT_RELEASE:
			IL_49:
			switch (evt)
			{
			}
			return false;
		case INPUT_INFO.INPUT_EVENT.BOTH_PRESS:
			if (!TsPlatform.IsMobile)
			{
				this.BothCharMove();
			}
			return false;
		case INPUT_INFO.INPUT_EVENT.TAP:
			this.TabCharMove();
			return false;
		}
		goto IL_49;
	}

	public void BothCharMove()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return;
		}
		NrCharUser nrCharUser = (NrCharUser)@char;
		if (nrCharUser.GetFollowCharPersonID() > 0L)
		{
			if (!(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_STOPAUTOMOVE) is StopAutoMove))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
			}
			return;
		}
		if (!this.bMouseCharMove)
		{
			nrCharUser.m_kCharMove.MoveStop(true, false);
			nrCharUser.m_kCharMove.SetIncreaseMove();
		}
		nrCharUser.m_kCharMove.MouseMove();
		this.bMouseCharMove = true;
	}

	public void NoChangeMove()
	{
		if (this.bMouseCharMove)
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char == null)
			{
				return;
			}
			if (@char != null)
			{
				@char.m_kCharMove.MoveStop(true, true);
				@char.m_kCharMove.SendCharMovePacketForKeyBoardMove(true);
			}
			this.bMouseCharMove = false;
			this.SetSafeClickTime(-0.2f);
		}
	}

	public void TabCharMove()
	{
		if (!NrTSingleton<NkQuestManager>.Instance.IsCompletedFirstQuest())
		{
			return;
		}
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser != null)
		{
			if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
			{
				nrCharUser.m_kCharMove.MoveStop(true, true);
				return;
			}
			if (this.bMouseCharMove)
			{
				nrCharUser.m_kCharMove.MoveStop(true, true);
				nrCharUser.m_kCharMove.SendCharMovePacketForKeyBoardMove(true);
				this.bMouseCharMove = false;
				this.SetSafeClickTime(-0.2f);
				return;
			}
		}
		this.UpdateWalkEffect(false);
		if (!NrTSingleton<NkClientLogic>.Instance.IsPickingEnable())
		{
			NrTSingleton<NkClientLogic>.Instance.SetPickingEnable(true);
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
		{
			return;
		}
		if (this.fSafeClickTime > 0f && Time.time - this.fSafeClickTime < 0.5f)
		{
			return;
		}
		NrCharUser nrCharUser2 = nrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		if (nrCharUser2.GetFollowCharPersonID() > 0L)
		{
			NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
		}
		else
		{
			if (Launcher.Instance.LocalPatchLevel != Launcher.Instance.PatchLevelMax && NrTSingleton<NrAutoPath>.Instance.IsAutoMoving())
			{
				string textFromMessageBox = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("168");
				string textFromMessageBox2 = NrTSingleton<NrTextMgr>.Instance.GetTextFromMessageBox("240");
				MsgBoxUI msgBoxUI = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.MSGBOX_DLG) as MsgBoxUI;
				msgBoxUI.SetMsg(new YesDelegate(this.OnAutoMoveStop), null, textFromMessageBox, textFromMessageBox2, eMsgType.MB_OK_CANCEL);
				msgBoxUI.SetButtonOKText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("320"));
				msgBoxUI.SetButtonCancelText(NrTSingleton<NrTextMgr>.Instance.GetTextFromInterface("321"));
				return;
			}
			NrCharBase pickChar = NrTSingleton<NkClientLogic>.Instance.GetPickChar();
			if (pickChar != null)
			{
				if (NrTSingleton<NkClientLogic>.Instance.GetFocusChar() == null)
				{
					pickChar.CancelClickMe();
					nrCharUser2.m_kCharMove.SetTargetChar(null);
				}
				else
				{
					pickChar.SetClickMe();
					if (!pickChar.IsCharKindATB(32L))
					{
						nrCharUser2.m_kCharMove.SetTargetChar(pickChar);
					}
					else
					{
						nrCharUser2.m_kCharMove.SetTargetChar(null);
					}
				}
			}
			else
			{
				NrCharBase targetChar = nrCharUser2.m_kCharMove.GetTargetChar();
				if (targetChar != null)
				{
					targetChar.CancelClickMe();
				}
				nrCharUser2.m_kCharMove.SetTargetChar(null);
			}
			nrCharUser2.PickingMove();
		}
		Dlg_Collect dlg_Collect = (Dlg_Collect)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_COLLECT);
		if (dlg_Collect != null)
		{
			dlg_Collect.OnClose();
		}
		this.fSafeClickTime = 0f;
	}

	public void OnAutoMoveStop(object a_oObject)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null && @char.IsCharKindATB(1L))
		{
			NrCharUser nrCharUser = (NrCharUser)@char;
			nrCharUser.SetFollowCharPersonID(0L, string.Empty);
			nrCharUser.m_kCharMove.MoveStop(true, false);
		}
	}

	public void DragMoveUpdate(bool bInputChanged)
	{
		if (TsPlatform.IsMobile)
		{
			if (CharMoveCommandLayer.bDragMove)
			{
				this.DragCharMove();
			}
			else if (bInputChanged)
			{
				this.NoChangeMove();
			}
		}
	}

	public void DragCharMove()
	{
		if (!CharMoveCommandLayer.bDragMove || this.pkDragMoveChar == null)
		{
			return;
		}
		if (!this.bMouseCharMove)
		{
			this.pkDragMoveChar.m_kCharMove.MoveStop(true, false);
			this.pkDragMoveChar.m_kCharMove.SetIncreaseMove();
			this.bMouseCharMove = true;
		}
		this.pkDragMoveChar.m_kCharMove.MobileTabDragMoveToDirection(ref this.vMoveDir);
	}

	public override bool PreCheckUpdate()
	{
		return Scene.CurScene == Scene.Type.WORLD && !NrTSingleton<NrDebugConsole>.Instance.IsActive() && !NrTSingleton<NkClientLogic>.Instance.IsNPCTalkState() && StageSystem.IsStable;
	}

	private void MyCharMoveKeyboard()
	{
		if (NkInputManager.GetKeyDown(KeyCode.W) || NkInputManager.GetKeyDown(KeyCode.UpArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((uint)this.CurCharKeyMoveStatus | 1u);
		}
		else if (NkInputManager.GetKeyUp(KeyCode.W) || NkInputManager.GetKeyUp(KeyCode.UpArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((int)this.CurCharKeyMoveStatus & -2);
		}
		if (NkInputManager.GetKeyDown(KeyCode.S) || NkInputManager.GetKeyDown(KeyCode.DownArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((uint)this.CurCharKeyMoveStatus | 2u);
		}
		else if (NkInputManager.GetKeyUp(KeyCode.S) || NkInputManager.GetKeyUp(KeyCode.DownArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((int)this.CurCharKeyMoveStatus & -3);
		}
		if (NkInputManager.GetKeyDown(KeyCode.A) || NkInputManager.GetKeyDown(KeyCode.LeftArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((uint)this.CurCharKeyMoveStatus | 4u);
		}
		else if (NkInputManager.GetKeyUp(KeyCode.A) || NkInputManager.GetKeyUp(KeyCode.LeftArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((int)this.CurCharKeyMoveStatus & -5);
		}
		if (NkInputManager.GetKeyDown(KeyCode.D) || NkInputManager.GetKeyDown(KeyCode.RightArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((uint)this.CurCharKeyMoveStatus | 8u);
		}
		else if (NkInputManager.GetKeyUp(KeyCode.D) || NkInputManager.GetKeyUp(KeyCode.RightArrow))
		{
			this.CurCharKeyMoveStatus = (sbyte)((int)this.CurCharKeyMoveStatus & -9);
		}
		this.CharKeyBoardMove();
	}

	private void CharKeyBoardMove()
	{
		if ((int)this.CurCharKeyMoveStatus == 0 && (int)this.PrevCharKeyMoveStatus == 0)
		{
			return;
		}
		NrTSingleton<NkClientLogic>.Instance.InitPickChar();
		sbyte prevCharKeyMoveStatus = this.PrevCharKeyMoveStatus;
		this.PrevCharKeyMoveStatus = this.CurCharKeyMoveStatus;
		NrCharUser nrCharUser = NrTSingleton<NkCharManager>.Instance.GetChar(1) as NrCharUser;
		if (nrCharUser == null)
		{
			return;
		}
		if (!nrCharUser.IsReadyCharAction())
		{
			return;
		}
		if (nrCharUser.GetFollowCharPersonID() > 0L)
		{
			if (!(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_STOPAUTOMOVE) is StopAutoMove))
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
			}
			this.CurCharKeyMoveStatus = 0;
			this.PrevCharKeyMoveStatus = 0;
			return;
		}
		if ((int)this.CurCharKeyMoveStatus == 0)
		{
			if ((int)prevCharKeyMoveStatus > 0)
			{
				nrCharUser.m_kCharMove.MoveStop(true, true);
				nrCharUser.m_kCharMove.SendCharMovePacketForKeyBoardMove(true);
			}
			return;
		}
		if ((int)prevCharKeyMoveStatus == 0)
		{
			nrCharUser.m_kCharMove.MoveStop(false, false);
			nrCharUser.m_kCharMove.SetIncreaseMove();
		}
		nrCharUser.m_kCharMove.KeyboardMove();
		NrTSingleton<CRightClickMenu>.Instance.CloseUI(CRightClickMenu.CLOSEOPTION.CLICK);
	}

	public void MyCharMoveMouse()
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char == null)
		{
			return;
		}
		if (NkInputManager.GetMouseButton(0) && NkInputManager.GetMouseButton(1))
		{
			NrCharUser nrCharUser = (NrCharUser)@char;
			if (nrCharUser.GetFollowCharPersonID() > 0L)
			{
				if (!(NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_STOPAUTOMOVE) is StopAutoMove))
				{
					NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
				}
				return;
			}
			if (!this.bMouseCharMove)
			{
				nrCharUser.m_kCharMove.MoveStop(true, false);
				nrCharUser.m_kCharMove.SetIncreaseMove();
			}
			nrCharUser.m_kCharMove.MouseMove();
			this.bMouseCharMove = true;
		}
		else if (NkInputManager.GetMouseButtonUp(0))
		{
			if (this.bMouseCharMove)
			{
				if (@char != null)
				{
					@char.m_kCharMove.MoveStop(true, true);
					@char.m_kCharMove.SendCharMovePacketForKeyBoardMove(true);
				}
				this.bMouseCharMove = false;
				this.SetSafeClickTime(-0.2f);
				return;
			}
			this.UpdateWalkEffect(false);
			if (NrTSingleton<FormsManager>.Instance.IsMouseOverForm())
			{
				return;
			}
			if (!NrTSingleton<NkClientLogic>.Instance.IsPickingEnable())
			{
				NrTSingleton<NkClientLogic>.Instance.SetPickingEnable(true);
				return;
			}
			if (!NrTSingleton<NkClientLogic>.Instance.IsMovable())
			{
				return;
			}
			if (this.fSafeClickTime > 0f && Time.time - this.fSafeClickTime < 0.5f)
			{
				return;
			}
			NrCharUser nrCharUser2 = (NrCharUser)@char;
			if (nrCharUser2.GetFollowCharPersonID() > 0L)
			{
				NrTSingleton<FormsManager>.Instance.ShowForm(G_ID.DLG_STOPAUTOMOVE);
			}
			else
			{
				NrCharBase pickChar = NrTSingleton<NkClientLogic>.Instance.GetPickChar();
				if (pickChar != null)
				{
					if (NrTSingleton<NkClientLogic>.Instance.GetFocusChar() == null)
					{
						pickChar.CancelClickMe();
						nrCharUser2.m_kCharMove.SetTargetChar(null);
					}
					else
					{
						pickChar.SetClickMe();
						if (!pickChar.IsCharKindATB(32L))
						{
							nrCharUser2.m_kCharMove.SetTargetChar(pickChar);
						}
						else
						{
							nrCharUser2.m_kCharMove.SetTargetChar(null);
						}
					}
				}
				else
				{
					NrCharBase targetChar = nrCharUser2.m_kCharMove.GetTargetChar();
					if (targetChar != null)
					{
						targetChar.CancelClickMe();
					}
					nrCharUser2.m_kCharMove.SetTargetChar(null);
				}
				nrCharUser2.PickingMove();
			}
			Dlg_Collect dlg_Collect = (Dlg_Collect)NrTSingleton<FormsManager>.Instance.GetForm(G_ID.DLG_COLLECT);
			if (dlg_Collect != null)
			{
				dlg_Collect.OnClose();
			}
			this.fSafeClickTime = 0f;
		}
		else if (this.bMouseCharMove)
		{
			if (@char != null)
			{
				@char.m_kCharMove.MoveStop(true, true);
				@char.m_kCharMove.SendCharMovePacketForKeyBoardMove(true);
			}
			this.bMouseCharMove = false;
			this.SetSafeClickTime(-0.2f);
		}
	}

	private void UpdateWalkEffect(bool bDirectUpdate)
	{
		NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
		if (@char != null)
		{
			bool flag = @char.m_kCharMove.IsMoving();
			bool flag2 = this.preRunState != flag;
			if (!bDirectUpdate && !flag2)
			{
				return;
			}
			this.preRunState = flag;
			Vector3 centerPosition = @char.GetCenterPosition();
			Vector3 vPos = @char.m_kCharMove.GetCharPos();
			string effectKey = string.Empty;
			Ray ray = new Ray(centerPosition, Vector3.down);
			if (NkRaycast.Raycast(ray))
			{
				effectKey = this.GetEffectState(NkRaycast.HIT.transform, flag);
				vPos = NkRaycast.HIT.point;
			}
			@char.OnWalkEffect(effectKey, vPos);
		}
	}

	private string GetEffectState(Transform TFM, bool bRunState)
	{
		if (!(null != TFM.gameObject) || TFM.gameObject.layer != TsLayer.WATER)
		{
			return string.Empty;
		}
		if (bRunState)
		{
			return "FX_RUN_WATER";
		}
		return "FX_STAY_WATER";
	}

	public void SetSafeClickTime(float addtime)
	{
		this.fSafeClickTime = Time.time + addtime;
	}
}
