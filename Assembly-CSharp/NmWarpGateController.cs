using PROTOCOL;
using PROTOCOL.GAME;
using PROTOCOL.GAME.ID;
using System;
using UnityEngine;
using UnityForms;

public class NmWarpGateController : MonoBehaviour
{
	public int nGateIndex;

	private void OnMouseExit()
	{
		NrTSingleton<FormsManager>.Instance.CloseForm(G_ID.TOOLTIP_DLG);
	}

	private void OnMouseEnter()
	{
		if (0 >= this.nGateIndex)
		{
			return;
		}
		string gateToolTip = NrTSingleton<MapManager>.Instance.GetGateToolTip(this.nGateIndex);
		if (string.IsNullOrEmpty(gateToolTip))
		{
			return;
		}
		Tooltip_Dlg tooltip_Dlg = NrTSingleton<FormsManager>.Instance.LoadForm(G_ID.TOOLTIP_DLG) as Tooltip_Dlg;
		if (tooltip_Dlg == null)
		{
			return;
		}
		tooltip_Dlg.Set_Tooltip(G_ID.TOOLTIP_DLG, gateToolTip);
	}

	private void OnMouseUpAsButton()
	{
		if (NkInputManager.GetMouseButtonUp(0))
		{
			NrCharBase @char = NrTSingleton<NkCharManager>.Instance.GetChar(1);
			if (@char != null)
			{
				@char.m_kCharMove.HideMoveMark = true;
				@char.PickMoveStart(base.gameObject.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
				NrTSingleton<NkClientLogic>.Instance.SetPickingEnable(false);
			}
		}
	}

	private void OnMouseOver()
	{
	}

	private void OnTriggerEnter(Collider kCollider)
	{
		if (0 >= this.nGateIndex)
		{
			return;
		}
		if (NrTSingleton<NkClientLogic>.Instance.GetWarpGateIndex() == this.nGateIndex)
		{
			return;
		}
		NrCharInfoAdaptor component = kCollider.gameObject.GetComponent<NrCharInfoAdaptor>();
		if (null == component)
		{
			return;
		}
		if (component.CharInfo.Get_Char_ID() != 1)
		{
			return;
		}
		GATE_INFO gateInfo = NrTSingleton<NrBaseTableManager>.Instance.GetGateInfo(this.nGateIndex.ToString());
		if (gateInfo == null)
		{
			return;
		}
		if (gateInfo.SRC_MAP_IDX != NrTSingleton<NkCharManager>.Instance.m_kMyCharInfo.m_kCharMapInfo.m_nMapIndex)
		{
			return;
		}
		if (!NrTSingleton<NkClientLogic>.Instance.ShowDownLoadUI(gateInfo.GATE_IDX, 0))
		{
			return;
		}
		GS_WARP_REQ gS_WARP_REQ = new GS_WARP_REQ();
		gS_WARP_REQ.nGateIndex = this.nGateIndex;
		SendPacket.GetInstance().SendObject(eGAME_PACKET_ID.GS_WARP_REQ, gS_WARP_REQ);
		NrTSingleton<NkClientLogic>.Instance.CharWarpRequest(this.nGateIndex);
	}
}
