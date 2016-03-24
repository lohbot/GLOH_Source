using System;
using UnityEngine;
using UnityForms;

namespace CostumeRoomDlg.MY_CHAR_LIST
{
	public class CallbackProcessor
	{
		private CostumeRoom_Dlg _costumeRoomDlg;

		private MyCharListSetter _myCharListSetter;

		public CallbackProcessor(CostumeRoom_Dlg costumeRoomDlg, MyCharListSetter owner)
		{
			this._costumeRoomDlg = costumeRoomDlg;
			this._myCharListSetter = owner;
		}

		public void OnClickListUpBox(IUIObject obj)
		{
			if (obj == null || obj.Data == null)
			{
				return;
			}
			NkSoldierInfo nkSoldierInfo = (NkSoldierInfo)obj.Data;
			if (nkSoldierInfo == null)
			{
				Debug.LogError("ERROR, MyCharListSetter.cs, OnClickListUpBox(), soldierInfo is Null");
				return;
			}
			if (this.DifferentKindCharClicked(nkSoldierInfo))
			{
				this.InitCostumeRoom(nkSoldierInfo);
				return;
			}
			this._myCharListSetter._SelectedSolInfo = nkSoldierInfo;
			this._myCharListSetter.RefreshMyCharList(ref this._costumeRoomDlg._variables._mySolKindListBox);
			this.RefreshCostumeSaleListBox(nkSoldierInfo);
			this._costumeRoomDlg.InitCostumeView(NrTSingleton<NrCharCostumeTableManager>.Instance.GetSoldierCostumeData(nkSoldierInfo));
			this.PlayAttackAni();
		}

		public void OnClickSameHeroViewCheckBox(IUIObject obj)
		{
			if (obj == null)
			{
				return;
			}
			CheckBox checkBox = obj as CheckBox;
			if (checkBox.IsChecked())
			{
				this._myCharListSetter.InitMyCharList(ref this._costumeRoomDlg._variables._mySolKindListBox, this._costumeRoomDlg.GetSettedCostumeKindList(), null);
			}
			else
			{
				this._myCharListSetter.InitMyCharList(ref this._costumeRoomDlg._variables._mySolKindListBox, NrTSingleton<NrCharCostumeTableManager>.Instance.GetCostumeKindList(), null);
			}
		}

		private bool DifferentKindCharClicked(NkSoldierInfo solInfo)
		{
			return solInfo != null && solInfo.GetCharKind() != this._costumeRoomDlg._settingCostumeKind;
		}

		private void InitCostumeRoom(NkSoldierInfo soldierInfo)
		{
			if (soldierInfo == null)
			{
				return;
			}
			this._costumeRoomDlg._variables._sameKindSolCheckBox.SetCheckState(1);
			this._costumeRoomDlg.InitCostumeRoom(soldierInfo.GetCharKind(), null);
		}

		private void RefreshCostumeSaleListBox(NkSoldierInfo solInfo)
		{
			if (solInfo == null)
			{
				return;
			}
			CharCostumeInfo_Data soldierCostumeData = NrTSingleton<NrCharCostumeTableManager>.Instance.GetSoldierCostumeData(solInfo);
			this._costumeRoomDlg._costumeListSetter.SetSelectedCostume(soldierCostumeData);
			this._costumeRoomDlg._costumeListSetter.RefreshCostumeListBox(ref this._costumeRoomDlg._variables._costumeListBox);
		}

		private void PlayAttackAni()
		{
			this._costumeRoomDlg._costumeViewerSetter._costumeCharSetter._animationProcessor.PlayAttackAni(true);
		}
	}
}
